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
    public HachWmsOutBoundService(
        SqlSugarRepository<HachWmsOutBound> hachWmsOutBoundRep,
        SqlSugarRepository<WMSPreOrder> wMSPreorderRep,
        SqlSugarRepository<WMSProduct> wMSProductRep,
        SqlSugarRepository<WMSOrderAddress> wMSOrderAddressRep,
         UserManager userManager,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep)
    {
        _hachWmsOutBoundRep = hachWmsOutBoundRep;
        _wMSPreorderRep = wMSPreorderRep;
        _wMSProductRep = wMSProductRep;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize]
    [ApiDescriptionSettings(Name = "putSOData")]
    public async Task<HachWMSResponse> asyncSyncOutBound(List<HachWmsOutBoundInput> input)
    {
        if (_userManager.UserId == null)
        {
            throw Oops.Oh(ErrorCode.Unauthorized);
        }
        HachWMSResponse response = new HachWMSResponse()
        {
            Success = true,
            Result = "success",
            Items = new List<HachWMSDetailResponse>()
        };

        // 0) 基础校验
        const int MaxBatch = 20; 

        //入参不能为空
        if (input == null || input.Count == 0)
            return new HachWMSResponse { Success = false, Result = ErrorCode.BadRequest.GetDescription() };

        //单次请求最多允许20条，当前21条
        if (input.Count > MaxBatch)
            return new HachWMSResponse { Success = false, Result = $"a maximum of  {MaxBatch} equests are allowed per request，currently {input.Count} items" };

        HachWmsOutBound outBound = new HachWmsOutBound();
        HachWmsAuthorizationConfig wmsAuthorizationConfig = new HachWmsAuthorizationConfig();

        wmsAuthorizationConfig = await GetCustomerInfo("putSOData");
        if (wmsAuthorizationConfig == null )
            return new HachWMSResponse { Success = false, Result = "no available customer configuration (AppId/interface permissions) matched" };

        foreach (var order in input)
        {
            var orderResult = new HachWMSDetailResponse { Success = true };
            try
            {

                string syncOrderNo = order.OrderNumber == null ? order.SoNumber + order.DeliveryNumber : order.OrderNumber;

                if (string.IsNullOrWhiteSpace(syncOrderNo))
                {
                    orderResult.Success = false;
                    orderResult.Message = "orderNo is missing（(OrderNo/ShipmentNum-ReceiptNum-DocNumber) at least one set）";
                    orderResult.Remark = "";
                    response.Items.Add(orderResult);
                    continue;
                }
                // 安全截断（假设库里 ExternReceiptNumber nvarchar(100)）
                if (syncOrderNo.Length > 120) syncOrderNo = syncOrderNo[..120];
                orderResult.Remark = syncOrderNo;

                // 2.2) 输入校验
                if (order.items == null || order.items.Count == 0)
                {
                    //订单明细不能为空
                    orderResult.Message = $"orderNo：{syncOrderNo} details are empty";
                    orderResult.Success = false;
                    response.Items.Add(orderResult);
                    continue;
                }
                if (order.items.Any(i => string.IsNullOrWhiteSpace(i.ItemNumber)))
                {
                    //订单明细行物料编码不能为空
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} There is an empty <ItemNum>";
                    response.Items.Add(orderResult);
                    continue;
                }
                if (order.items.Any(i => Convert.ToDouble(i.Quantity) <= 0))
                {
                    //订单明细行数量必须大于0
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} There is an illegal quantity（Quantity <= 0）";
                    response.Items.Add(orderResult);
                    continue;
                }
                // LineNum 去重检查
                var dupLines = order.items.GroupBy(i => i.LineNumber).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                if (dupLines.Count > 0)
                {
                    //订单明细行号重复
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} duplicate detailed line numbers：{string.Join(",", dupLines)}";
                    response.Items.Add(orderResult);
                    continue;
                }

                // 2.3) 幂等：先查已存在（DB 有唯一约束时也要提前查）
                var existing = await _wMSPreorderRep.AsQueryable()
                        .Where(x => x.PreOrderStatus != -1 && x.ExternOrderNumber == syncOrderNo)
                        .AnyAsync();
                //var existing = await _wMSPreorderRep.GetFirstAsync(x => x.PreOrderStatus != -1 && x.ExternOrderNumber == syncOrderNo);
                if (existing)
                {
                    // 视为幂等：直接返回成功或提示已存在
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} already exists (idempotent)";
                    response.Items.Add(orderResult);
                    continue;
                }

                // 2.4) SKU 校验（统一大小写）
                var skus = order.items
                           .Select(x => x.ItemNumber?.Trim())
                           .Where(x => !string.IsNullOrWhiteSpace(x))
                           .Select(x => x.ToUpper()).Distinct().ToList();

                // 使用 HashSet 优化 Contains 操作
                var skuSet = new HashSet<string>(skus);
                var products = await _wMSProductRep.AsQueryable()
                              .Where(p => p.CustomerId == wmsAuthorizationConfig.CustomerId
                              && skus.Contains(SqlFunc.ToUpper(p.SKU)))
                              .Where(p => p.ProductStatus == 1)
                              .ToListAsync();

                var foundSkuSet = new HashSet<string>(products.Select(p => p.SKU.ToUpper()));

                var missingSkus = skus.Where(s => !foundSkuSet.Contains(s)).ToList();

                if (missingSkus.Count > 0)
                {
                    orderResult.Success = false;
                    orderResult.Message = $"OrderNo：{syncOrderNo} Missing SKU：{string.Join(", ", missingSkus)}";
                    response.Items.Add(orderResult);
                    continue;
                }

                // 2.5) 事务写入（对接表 + 业务表）
                var tranRes = await _wMSPreorderRep.Context.Ado.UseTranAsync(async () =>
                {
                    // 对接表
                    var hachWmsOutBound = MapOutBound(order, syncOrderNo, _userManager?.UserId);
                    await _hachWmsOutBoundRep.Context.InsertNav(hachWmsOutBound)
                        .Include(a => a.items)
                        .ExecuteReturnEntityAsync();
                    // 业务表（细化到产品映射）
                    var result = await SyncWmsPreOrderTransactional(order, wmsAuthorizationConfig, products);
                    if (!result.Success)
                    {
                        // 业务表失败时抛异常以回滚事务
                        throw new Exception(result.Result ?? "business table processing failed");
                    }
                });
                if (!tranRes.IsSuccess)
                {
                    orderResult.Success = false;
                    var msg = tranRes.ErrorMessage;
                    if (msg != null && (msg.Contains("UNIQUE") || msg.Contains("唯一") || msg.Contains("duplicate")))
                        orderResult.Message = $"orderNo：{syncOrderNo} already exists (unique constraint)";//已存在（唯一约束）
                    else
                        orderResult.Message = $"orderNo：{syncOrderNo} processing failed：{msg}";//处理失败
                }
                else
                {
                    orderResult.Success = true;
                    orderResult.Message = $"orderNo：{syncOrderNo} processed successfully";//处理成功
                }
                response.Items.Add(orderResult);
            }
            catch (Exception ex)
            {
                // 兜底异常
                orderResult.Message = $"orderNo：{orderResult.Remark} handle exceptions：{ex.Message}";
                orderResult.Success = false;
                response.Items.Add(orderResult);
            }
        }
        // 汇总成功与否
        response.Success = response.Items.All(i => i.Success);
        response.Result = response.Success ? "success" : "partial/complete failure";
        return response;
    }

    /// <summary>
    /// 映射对接表
    /// </summary>
    /// <param name="asn"></param>
    /// <param name="orderNo"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private HachWmsOutBound MapOutBound(HachWmsOutBoundInput asn, string orderNo, long? userId)
    {
        var outBound = asn.Adapt<HachWmsOutBound>(); // 确保 Mapster 配置包含 items
        outBound.OrderNumber = orderNo;
        outBound.CreateUserId = userId ?? 0;
        return outBound;
    }

    /// <summary>
    /// 在事务中写业务表，外层已开启事务
    /// 传入已查好的产品列表避免重复查询
    /// </summary>
    private async Task<HachWMSResponse> SyncWmsPreOrderTransactional(
        HachWmsOutBound outBound,
        HachWmsAuthorizationConfig wmsAuthorizationConfig,
        List<WMSProduct> productLight)
    {
        var res = new HachWMSResponse { Success = true, Result = "OK" };
        var now = DateTime.UtcNow;
        var preorderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();

        //主数据
        var wMSPreOrder = new WMSPreOrder
        {
            PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
            ExternOrderNumber = outBound.OrderNumber,
            CustomerId = wmsAuthorizationConfig.CustomerId.Value,
            CustomerName = wmsAuthorizationConfig.CustomerName,
            WarehouseId = wmsAuthorizationConfig.WarehouseId.Value,
            WarehouseName = wmsAuthorizationConfig.WarehouseName,
            OrderType = outBound.DocType,
            PreOrderStatus = 1,
            OrderTime = Convert.ToDateTime(outBound.ScheduleShippingDate),
            Po = "",
            So = outBound.SoNumber,//销售单号
            DetailCount = outBound.items.Count,
            Creator = (_userManager?.UserId ?? 0).ToString(),
            CreationTime = DateTime.Now,
            TenantId = wmsAuthorizationConfig.TenantId ?? 1300000000001
        };
        //地址信息
        var wMSOrderAddress = new WMSOrderAddress
        {
            PreOrderNumber = wMSPreOrder.PreOrderNumber,
            ExternOrderNumber = wMSPreOrder.ExternOrderNumber,
            Name = outBound.ContactName,
            CompanyName = outBound.CustomerName,
            CompanyType = outBound.EndUserName,
            //如果是最终用户的话 ，shiptype就是是
            ShipType = outBound.EndUserName == "最终用户" ? "是" : "否",
            Phone = outBound.Telephone,
            Address = outBound.Address,
            Creator = (_userManager?.UserId ?? 0).ToString(),
            CreationTime = DateTime.Now,
            TenantId = wmsAuthorizationConfig.TenantId ?? 1300000000001
        };

        var prodMap = productLight.ToDictionary(p => ((string)p.SKU).ToUpper(), p => p);
        var details = new List<WMSPreOrderDetail>(outBound.items.Count);

        foreach (var item in outBound.items)
        {
            var skuKey = item.ItemNumber.Trim().ToUpper();
            if (!prodMap.TryGetValue(skuKey, out var prod))
            {
                return new HachWMSResponse { Success = false, Result = $"SKU 不存在：{item.ItemNumber}" };
            }

            details.Add(new WMSPreOrderDetail
            {
                PreOrderNumber = wMSPreOrder.PreOrderNumber,
                ExternOrderNumber = wMSPreOrder.ExternOrderNumber,
                CustomerId = wMSPreOrder.CustomerId,
                CustomerName = wMSPreOrder.CustomerName,
                WarehouseId = wMSPreOrder.WarehouseId,
                WarehouseName = wmsAuthorizationConfig.WarehouseName,
                LineNumber = item.LineNumber,
                SKU = item.ItemNumber,
                GoodsName = item.ItemDescription,
                GoodsType = prod.GoodsType,
                OrderQty = Convert.ToDouble(item.Quantity),
                ActualQty = 0,
                PoCode = outBound.ContractNo,
                SoCode = "",
                Weight = 0,
                Volume = 0,
                UnitCode = item.Uom,
                CreationTime = DateTime.Now,
                Creator = _userManager.UserId.ToString(),
                Str2 = !string.IsNullOrEmpty(item.ParentItemNumber) ? item.ParentItemNumber : "",
                Int2 = item.ParentItemId.HasValue ? item.ParentItemId : 0,
            });
        }
        wMSPreOrder.Details = details;

        // 一次 Nav 写入
        await _wMSPreorderRep.Context.InsertNav(wMSPreOrder)
            .Include(a => a.Details)
            .ExecuteReturnEntityAsync();
        return res;
    }
    /// <summary>
    /// 获取客户信息
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    private async Task<HachWmsAuthorizationConfig> GetCustomerInfo(string Type)
    {
        HachWmsAuthorizationConfig hachCustomerMappings = new HachWmsAuthorizationConfig();

        if (string.IsNullOrEmpty(Type))
        {
            return hachCustomerMappings;
        }

        hachCustomerMappings = await _hachWmsAuthorizationConfigRep.AsQueryable()
            .Where(a => a.AppId == _userManager.UserId)
            .Where(a => a.Status == true)
            .Where(a => a.IsDelete == false)
            .Where(a => a.Type == "HachWMSApi")
            .Where(a => a.InterFaceName == Type)
            .OrderByDescending(a => a.Id)
            .FirstAsync();

        return hachCustomerMappings;
    }
}
