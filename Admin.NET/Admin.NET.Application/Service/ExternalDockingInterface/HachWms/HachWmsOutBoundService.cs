// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Common.SnowflakeCommon;
using Microsoft.AspNetCore.Identity;
using StackExchange.Profiling.Internal;
using Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
using XAct;
using Microsoft.AspNetCore.Authorization;
using Furion.FriendlyException;
using Admin.NET.Application.Service.ExternalDockingInterface.Helper;
using Admin.NET.Application.Service.ExternalDockingInterface.HachWms.Enumerate;

namespace Admin.NET.Application.Service.ExternalDocking_Interface.HachWms.Dto;

/// <summary>
/// EXTERNAL DOCKING INTERFACE : HACHWMS OUTBOUND 
/// </summary>
[ApiDescriptionSettings("hachWMSOutBound", Order = 4, Groups = new[] { "HACHWMS INTERFACE" })]
public class HachWmsOutBoundService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<HachWmsOutBound> _hachWmsOutBoundRep;
    private readonly SqlSugarRepository<WMSPreOrder> _wMSPreorderRep;
    private readonly SqlSugarRepository<WMSProduct> _wMSProductRep;
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly UserManager _userManager;
    private readonly LogHelper _logHelper;
    private readonly GetEnum _enumRep;
    private readonly GetConfig _getConfigRep;

    public HachWmsOutBoundService(
        SqlSugarRepository<HachWmsOutBound> hachWmsOutBoundRep,
        SqlSugarRepository<WMSPreOrder> wMSPreorderRep,
        SqlSugarRepository<WMSProduct> wMSProductRep,
        SqlSugarRepository<WMSOrderAddress> wMSOrderAddressRep,
        UserManager userManager,
        GetEnum enumRep,
        LogHelper logHelper,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep,
        GetConfig getConfig)
    {
        _hachWmsOutBoundRep = hachWmsOutBoundRep;
        _wMSPreorderRep = wMSPreorderRep;
        _wMSProductRep = wMSProductRep;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _userManager = userManager;
        _logHelper = logHelper;
        _enumRep = enumRep;
        _getConfigRep = getConfig;
    }

    [HttpPost]
    [Authorize]
    [ApiDescriptionSettings(Name = "putSOData")]
    public async Task<HachWMSResponse> asyncSyncOutBound(List<HachWmsOutBoundInput> input)
    {
        // 生成批次号（日志追踪唯一标识）
        string batchId = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        // 记录上游请求原始报文
        string jsonPayload = System.Text.Json.JsonSerializer.Serialize(input, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        await _logHelper.LogAsync(
            LogHelper.LogMainType.出库订单下发,
            batchId,
            "BATCH",
            LogHelper.LogLevel.Info,
            "收到上游请求报文",
            true,
            jsonPayload
        );

        #region 基础验证
        if (_userManager.UserId == null)
        {
            await _logHelper.LogAsync(
            LogHelper.LogMainType.出库订单下发,
            batchId,
            "BATCH",
            LogHelper.LogLevel.Info,
           ErrorCode.Unauthorized.GetDescription(),
            true,
            "");
            throw Oops.Oh(ErrorCode.Unauthorized);
        }

        //const int MaxBatch = 20;
        var response = new HachWMSResponse { Success = true, Result = "success", Items = new List<HachWMSDetailResponse>() };

        if (input == null || input.Count == 0)
        {
            return new HachWMSResponse { Success = false, Result = OrderRespStatusEnum.RequestDataEmpty.GetDescription() };
        }
        //if (input.Count > MaxBatch)
        //{
        //    //单次最多允许 { MaxBatch}条请求，当前 { input.Count}条
        //    return new HachWMSResponse { Success = false, Result = $"A maximum of  {MaxBatch}requests are allowed at a time, currently there are{input.Count}requests" };
        //}
        // 获取客户授权配置
        HachWmsAuthorizationConfig wmsAuthorizationConfig = new HachWmsAuthorizationConfig();

        #endregion

        #region 外层事务控制：整体批量回滚
        // 用于记录出错的订单（仅错误）
        var errorOrders = new List<HachWMSDetailResponse>();

        // ----------------  ----------------
        var tranRes = await _wMSPreorderRep.Context.Ado.UseTranAsync(async () =>
        {
            // 遍历处理每个订单
            foreach (var order in input)
            {
                string syncOrderNo = order.OrderNumber ?? (order.SoNumber + order.DeliveryNumber);
                try
                {
                    if (string.IsNullOrEmpty(order.LocationCode))
                    {
                        throw new Exception($"orderNo：{syncOrderNo} “LocationCode” cannot be empty");
                    }
                    //根据仓库获取客户授权配置
                    wmsAuthorizationConfig = await _getConfigRep.GetCustomerInfo("putSOData", order.LocationCode);
                    if (wmsAuthorizationConfig == null)
                    {
                        throw new Exception($"orderNo：{syncOrderNo} Failed to obtain warehouse Location Code information");
                    }
                    #region Step 1：参数与数据校验 
                    if (string.IsNullOrWhiteSpace(syncOrderNo))
                        throw new Exception(OrderRespStatusEnum.OBMissingOrder.GetDescription());
                    //订单：｛syncOrderNo｝详细信息为空
                    if (order.items == null || order.items.Count == 0)
                        throw new Exception($"Order:  {syncOrderNo} details are empty");
                    //订单：｛syncOrderNo｝的商品编号为空
                    if (order.items.Any(i => string.IsNullOrWhiteSpace(i.ItemNumber)))
                        throw new Exception($"Order: {syncOrderNo} has an empty ItemNumber");
                    //订单：{ syncOrderNo}有数量 <= 0的详细行
                    if (order.items.Any(i => Convert.ToDouble(i.Quantity) <= 0))
                        throw new Exception($"Order:  {syncOrderNo} has detailed lines with quantity<=0");

                    //var dupLines = order.items.GroupBy(i => i.LineNumber).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    ////订单：｛syncOrderNo｝有重复的行号
                    //if (dupLines.Count > 0)
                    //    throw new Exception($"Order: {syncOrderNo} has duplicate line numbers: {string.Join(",", dupLines)}");


                    var DeliveryDetailIds = order.items.GroupBy(i => i.DeliveryDetailId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    //订单：｛syncOrderNo｝有重复的deliveryDetailId
                    if (DeliveryDetailIds.Count > 0)
                        throw new Exception($"Order: {syncOrderNo} has duplicate DeliveryDetailId: {string.Join(",", DeliveryDetailIds)}");
                    #endregion

                    #region 幂等检查：是否已存在
                    bool exists = await _wMSPreorderRep.AsQueryable()
                        .Where(x => x.PreOrderStatus != -1 && x.ExternOrderNumber == syncOrderNo)
                        .AnyAsync();
                    //订单  {syncOrderNo}已存在(幂等校验)
                    if (exists)
                        throw new Exception($"Order:  {syncOrderNo} already exists (idempotent parity check)");
                    #endregion

                    #region Step 2：SKU合法性校验
                    var skus = order.items.Select(x => x.ItemNumber?.Trim().ToUpper()).Distinct().ToList();
                    var products = await _wMSProductRep.AsQueryable()
                        .Where(p => p.CustomerId == wmsAuthorizationConfig.CustomerId && skus.Contains(SqlFunc.ToUpper(p.SKU)))
                        .Where(p => p.ProductStatus == 1)
                        .ToListAsync();

                    var found = new HashSet<string>(products.Select(p => p.SKU.ToUpper()));
                    var missing = skus.Where(s => !found.Contains(s)).ToList();
                    //订单 { syncOrderNo}缺少 SKU
                    if (missing.Count > 0)
                        throw new Exception($"Order: {syncOrderNo} Missing SKU：{string.Join(",", missing)}");
                    #endregion

                    #region Step 3：写入数据库（出库表 + 业务表）
                    var hachWmsOutBound = MapOutBound(order, syncOrderNo, _userManager?.UserId);
                    await _hachWmsOutBoundRep.Context.InsertNav(hachWmsOutBound)
                        .Include(a => a.items)
                        .ExecuteReturnEntityAsync();

                    var bizRes = await SyncWmsPreOrderTransactional(order, wmsAuthorizationConfig, products);

                    //Order: {syncOrderNo} 写入业务表失败
                    if (!bizRes.Success)
                        throw new Exception(bizRes.Result ?? $"Order: {syncOrderNo} Processing failed");

                    #endregion

                    #region Step 4：成功日志记录
                    await _logHelper.LogAsync(
                        LogHelper.LogMainType.出库订单下发,
                        batchId,
                        syncOrderNo,
                        LogHelper.LogLevel.Success,
                        $"订单 {syncOrderNo} 处理成功",
                        true
                    );
                    #endregion
                }
                catch (Exception ex)
                {
                    // === 出错订单记录 ===
                    var err = new HachWMSDetailResponse
                    {
                        Success = false,
                        Remark = syncOrderNo,
                        Message = ex.Message
                    };
                    errorOrders.Add(err);
                    // 写错误日志
                    await _logHelper.LogAsync(
                        LogHelper.LogMainType.出库订单下发,
                        batchId,
                        syncOrderNo,
                        LogHelper.LogLevel.Error,
                        ex.Message,
                        false
                    );

                    throw; // 抛出触发事务回滚
                }
            }
        });
        #endregion

        #region 事务结果处理
        // ---------------- 事务结果处理 ----------------
        if (!tranRes.IsSuccess)
        {
            response.Success = false;
            response.Result = $"All order processing failed:{tranRes.ErrorMessage}";
            response.Items = errorOrders;
            await _logHelper.LogAsync(
                LogHelper.LogMainType.出库订单下发,
                batchId,
                "BATCH",
                LogHelper.LogLevel.Error,
                response.Result,
                false
            );
        }
        else
        {
            response.Success = true;
            response.Result = "All orders processed successfully";
            response.Items = new List<HachWMSDetailResponse>(); // 不返回成功订单
            await _logHelper.LogAsync(
                LogHelper.LogMainType.出库订单下发,
                batchId,
                "BATCH",
                LogHelper.LogLevel.Success,
                response.Result,
                true
            );
        }
        #endregion

        return response;
    }

    /// <summary>
    /// 映射对接表 上游入参 → HachWmsOutBound）
    /// </summary>
    /// <param name="asn"></param>
    /// <param name="orderNo"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private HachWmsOutBound MapOutBound(HachWmsOutBoundInput ob, string orderNo, long? userId)
    {
        var outBound = ob.Adapt<HachWmsOutBound>();
        outBound.OrderNumber = orderNo;
        outBound.CreateUserId = userId ?? 0;
        return outBound;
    }


    /// <summary>
    /// 写入业务表（在事务内部执行）
    /// </summary>
    private async Task<HachWMSResponse> SyncWmsPreOrderTransactional(
        HachWmsOutBound outBound,
        HachWmsAuthorizationConfig wmsAuthorizationConfig,
        List<WMSProduct> productLight)
    {
        var res = new HachWMSResponse { Success = true, Result = "OK" };
        // 预处理主表
        var wMSPreOrder = new WMSPreOrder
        {
            PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
            ExternOrderNumber = outBound.OrderNumber,
            CustomerId = wmsAuthorizationConfig.CustomerId.Value,
            CustomerName = wmsAuthorizationConfig.CustomerName,
            WarehouseId = wmsAuthorizationConfig.WarehouseId.Value,
            WarehouseName = wmsAuthorizationConfig.WarehouseName,
            OrderType = _enumRep.GetEnumDescriptionOrDefault<ObOrderStatusEnum>(outBound.DocType, "大仓出库"),
            PreOrderStatus = 1,
            OrderTime = Convert.ToDateTime(outBound.ScheduleShippingDate),
            DetailCount = outBound.items.Count,
            Creator = (_userManager?.UserId ?? 0).ToString(),
            CreationTime = DateTime.Now,
            TenantId = wmsAuthorizationConfig.TenantId ?? 1300000000001
        };
        // 构造明细表
        var prodMap = productLight.ToDictionary(p => p.SKU.ToUpper(), p => p);
        var details = new List<WMSPreOrderDetail>();

        //构造地址
        var address = new WMSOrderAddress()
        {
            PreOrderNumber = wMSPreOrder.PreOrderNumber,
            ExternOrderNumber = wMSPreOrder.ExternOrderNumber,
            Name = outBound.ContactName,
            CompanyName = outBound.CustomerName,
            CompanyType = "终端用户",
            ShipType = "是",
            Phone = outBound.Telephone,
            Address = outBound.Address,
            CreationTime = DateTime.Now,
            Creator = _userManager.UserId.ToString()
            ,
            TenantId = wmsAuthorizationConfig.TenantId ?? 1300000000001
        };

        foreach (var item in outBound.items)
        {
            var skuKey = item.ItemNumber.Trim().ToUpper();
            if (!prodMap.TryGetValue(skuKey, out var prod))
                return new HachWMSResponse { Success = false, Result = $"SKU 不存在：{item.ItemNumber}" };

            details.Add(new WMSPreOrderDetail
            {
                PreOrderNumber = wMSPreOrder.PreOrderNumber,
                ExternOrderNumber = wMSPreOrder.ExternOrderNumber,
                CustomerId = wMSPreOrder.CustomerId,
                CustomerName = wMSPreOrder.CustomerName,
                WarehouseId = wMSPreOrder.WarehouseId,
                WarehouseName = wMSPreOrder.WarehouseName,
                LineNumber = item.LineNumber,
                SKU = item.ItemNumber,
                GoodsName = item.ItemDescription,
                GoodsType = "A品",
                OrderQty = Convert.ToDouble(item.Quantity),
                ActualQty = 0,
                PoCode = outBound.ContractNo,
                SoCode = "",
                UnitCode = item.Uom,
                CreationTime = DateTime.Now,
                Creator = _userManager.UserId.ToString(),
                Str2 = item.ParentItemNumber ?? "",
                Int2 = item.ParentItemId ?? 0,
                Onwer = outBound.Subinventory ?? "",
            });
        }
        // 导航写入主从表
        wMSPreOrder.Details = details;
        wMSPreOrder.OrderAddress = address;
        await _wMSPreorderRep.Context.InsertNav(wMSPreOrder)
            .Include(a => a.Details)
            .Include(a => a.OrderAddress)
            .ExecuteReturnEntityAsync();
        return res;
    }

    //// <summary>
    ///// 获取授权客户信息
    ///// </summary>
    //private async Task<HachWmsAuthorizationConfig> GetCustomerInfo(string Type)
    //{
    //    if (string.IsNullOrEmpty(Type))
    //        return null;

    //    return await _hachWmsAuthorizationConfigRep.AsQueryable()
    //        .Where(a => a.AppId == _userManager.UserId)
    //        .Where(a => a.Status && !a.IsDelete)
    //        .Where(a => a.Type == "HachWMSApi" && a.InterFaceName == Type)
    //        .OrderByDescending(a => a.Id)
    //        .FirstAsync();
    //}
}
