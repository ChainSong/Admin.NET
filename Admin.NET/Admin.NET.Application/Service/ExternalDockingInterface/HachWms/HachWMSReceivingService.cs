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
using Admin.NET.Application.Service.ExternalDocking_Interface.HachWms.Dto;
using Microsoft.AspNetCore.Authorization;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using XAct;
using Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
using Furion.FriendlyException;
using Admin.NET.Application.Service.ExternalDockingInterface.Helper;
using Admin.NET.Application.Service.ExternalDockingInterface.HachWms.Enumerate;
using Admin.NET.Application.Enumerate;

namespace Admin.NET.Application.Service.ExternalDocking_Interface.HachWms;

/// <summary>
/// EXTERNAL DOCKING INTERFACE : HACHWMS RECEIVING 
/// </summary>

[ApiDescriptionSettings("hachWMSReceiving", Order = 3, Groups = new[] { "HACHWMS INTERFACE" })]
public class HachWMSReceivingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<HachWmsReceiving> _hachWmsReceivingRep;
    private readonly SqlSugarRepository<WMSASN> _wMSASNRep;
    private readonly SqlSugarRepository<WMSProduct> _wMSProductRep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly LogHelper _logHelper;
    private readonly GetEnum _enumRep;

    public HachWMSReceivingService(
        SqlSugarRepository<HachWmsReceiving> hachWmsReceivingRep,
        SqlSugarRepository<WMSASN> wMSASNRep,
        SqlSugarRepository<WMSProduct> wMSProductRep,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep,
         LogHelper logHelper,
         GetEnum enumRep,
        UserManager userManager)
    {
        _hachWmsReceivingRep = hachWmsReceivingRep;
        _wMSASNRep = wMSASNRep;
        _wMSProductRep = wMSProductRep;
        _userManager = userManager;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _logHelper = logHelper;
        _enumRep = enumRep;
    }
    [HttpPost]
    [Authorize]
    [ApiDescriptionSettings(Name = "putASNData")]
    public async Task<HachWMSResponse> asyncSyncReceiving(List<HachWmsReceivingInput> input)
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
            true,jsonPayload);

        #region 基础验证
        if (_userManager.UserId == null)
        {
            await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH",
                LogHelper.LogLevel.Info, ErrorCode.Unauthorized.GetDescription(), true);
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
        {
            await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH",
             LogHelper.LogLevel.Info, ErrorCode.BadRequest.GetDescription(), true);

            return new HachWMSResponse { Success = false, Result = ErrorCode.BadRequest.GetDescription() };
        }

        //单次请求最多允许20条，当前21条
        if (input.Count > MaxBatch)
        {
            await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH",
            LogHelper.LogLevel.Info, $"单次最多允许 {MaxBatch}条请求，当前 {input.Count}条", true);
            return new HachWMSResponse { Success = false, Result = $"A maximum of  {MaxBatch} equests are allowed per request，currently {input.Count} items" };
        }

        //HachWmsReceiving receiving = new HachWmsReceiving();
        // 获取客户授权配置
        HachWmsAuthorizationConfig wmsAuthorizationConfig = new HachWmsAuthorizationConfig();
        #endregion

        foreach (var asn in input)
        {
            var orderResult = new HachWMSDetailResponse { Success = true };
            try
            {
                string syncOrderNo = asn.OrderNo == null ? asn.ShipmentNum + asn.ReceiptNum + asn.DocNumber : asn.OrderNo;

                #region 检验单号
                if (string.IsNullOrWhiteSpace(syncOrderNo))
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                        $"orderNo缺失（（orderNo/ShipmentNum ReceiptNum DocNumber）至少一组）", true);
                    orderResult.Success = false;
                    orderResult.Message = "orderNo is missing（(OrderNo/ShipmentNum-ReceiptNum-DocNumber) at least one set）";
                    orderResult.Remark = "";
                    response.Items.Add(orderResult);
                    continue;
                }

                // 安全截断（假设库里 ExternReceiptNumber nvarchar(100)）
                if (syncOrderNo.Length > 120) syncOrderNo = syncOrderNo[..120];
                orderResult.Remark = syncOrderNo;
                #endregion
                #region 订单校验
              
                // 2.2) 输入校验
                #region 输入校验
                if (asn.items == null || asn.items.Count == 0)
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                       $"订单:{syncOrderNo} 明细不能为空", true);
                    //订单明细不能为空
                    orderResult.Message = $"orderNo：{syncOrderNo} details are empty";
                    orderResult.Success = false;
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                //校验仓库LocationCode
                #region 校验仓库LocationCode
                //var warehouseCode = _enumRep.GetEnumDescriptionOrDefault<IbOrderTypeEnum>(asn.LocationCode, "");
                //if (string.IsNullOrEmpty(warehouseCode))
                //{
                //    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                //      $"订单:{syncOrderNo} 仓库LocationCode有误", true);
                //    orderResult.Message = $"orderNo：{syncOrderNo} Warehouse Location Code is incorrect";
                //    orderResult.Success = false;
                //    response.Items.Add(orderResult);
                //    continue; 
                //}
                #endregion
                wmsAuthorizationConfig = await GetCustomerInfo("putASNData", asn.LocationCode);
                if (wmsAuthorizationConfig==null)
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                    $"订单:{syncOrderNo} 获取仓库LocationCode信息失败", true);
                    orderResult.Message = $"orderNo：{syncOrderNo} Failed to obtain warehouse Location Code information";
                    orderResult.Success = false;
                    response.Items.Add(orderResult);
                    continue;
                }
                //物料编码空值校验
                #region 物料编码空值校验
                if (asn.items.Any(i => string.IsNullOrWhiteSpace(i.ItemNum)))
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                    $"订单:{syncOrderNo} 明细行物料编码不能为空", true);
                    //订单明细行物料编码不能为空
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} There is an empty <ItemNum>";
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                //明细数量校验
                #region 明细数量校验
                if (asn.items.Any(i => i.Quantity <= 0))
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                   $"订单:{syncOrderNo} 明细行数量必须大于0", true);
                    //订单明细行数量必须大于0
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} There is an illegal quantity（Quantity <= 0）";
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                // LineNum 去重检查
                #region LineNum 去重检查
                var dupLines = asn.items.GroupBy(i => i.LineNum).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                if (dupLines.Count > 0)
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                $"订单:{syncOrderNo} 明细行号重复", true);
                    //订单明细行号重复
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} duplicate detailed line numbers：{string.Join(",", dupLines)}";
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                // 2.3) 幂等：先查已存在（DB 有唯一约束时也要提前查）
                #region 幂等：先查已存在（DB 有唯一约束时也要提前查）
                var existing = await _wMSASNRep.AsQueryable()
                    .Where(x => x.ASNStatus != -1)
                    .Where(x => x.ExternReceiptNumber == syncOrderNo)
                    .Where(a=>a.WarehouseId==wmsAuthorizationConfig.WarehouseId)
                    .Where(a => a.CustomerId == wmsAuthorizationConfig.CustomerId)
                    .AnyAsync();
                if (existing)
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                $"订单:{syncOrderNo} 已存在", true);
                    // 视为幂等：直接返回成功或提示已存在
                    orderResult.Success = false;
                    orderResult.Message = $"orderNo：{syncOrderNo} already exists (idempotent)";
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                // 2.4) SKU 校验（统一大小写）
                #region SKU 校验（统一大小写）
                var skus = asn.items
                           .Select(x => x.ItemNum?.Trim())
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
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                     $"订单:{syncOrderNo} 缺失SKU:{string.Join(", ", missingSkus)}", true);
                    orderResult.Success = false;
                    orderResult.Message = $"OrderNo：{syncOrderNo} Missing SKU：{string.Join(", ", missingSkus)}";
                    response.Items.Add(orderResult);
                    continue;
                }
                #endregion
                #endregion
                // 2.5) 事务写入（对接表 + 业务表）
                var tranRes = await _wMSASNRep.Context.Ado.UseTranAsync(async () =>
                {
                    // 对接表
                    var receiving = MapReceiving(asn, syncOrderNo, _userManager?.UserId);
                    await _hachWmsReceivingRep.Context.InsertNav(receiving)
                        .Include(a => a.items)
                        .ExecuteReturnEntityAsync();

                    // 业务表（细化到产品映射）
                    var result = await SyncWmsAsnTransactional(receiving, wmsAuthorizationConfig, products);
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
                    {
                        await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                      $"订单:{syncOrderNo}已存在", true);
                        orderResult.Message = $"orderNo：{syncOrderNo} already exists (unique constraint)";//已存在（唯一约束）
                    }
                    else
                    {
                        await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                     $"订单:{syncOrderNo}处理失败", true);
                        orderResult.Message = $"orderNo：{syncOrderNo} processing failed：{msg}";//处理失败
                    }
                }
                else
                {
                    await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                    $"订单:{syncOrderNo}处理成功", true);
                    orderResult.Success = true;
                    orderResult.Message = $"orderNo：{syncOrderNo} processed successfully";//处理成功
                }
                
                response.Items.Add(orderResult);
            }
            catch (Exception ex)
            {
                await _logHelper.LogAsync(LogHelper.LogMainType.入库订单下发, batchId, "BATCH", LogHelper.LogLevel.Info,
                  $"订单:{orderResult.Remark}处理异常", true);
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

    private HachWmsReceiving MapReceiving(HachWmsReceivingInput asn, string orderNo, long? userId)
    {
        var receiving = asn.Adapt<HachWmsReceiving>(); // 确保 Mapster 配置包含 items
        receiving.OrderNo = orderNo;
        receiving.CreateUserId = userId ?? 0;
        return receiving;
    }

    /// <summary>
    /// 在事务中写业务表，外层已开启事务
    /// 传入已查好的产品列表避免重复查询
    /// </summary>
    private async Task<HachWMSResponse> SyncWmsAsnTransactional(
        HachWmsReceiving receiving,
        HachWmsAuthorizationConfig cfg,
        List<WMSProduct> productLight)
    {
        var res = new HachWMSResponse { Success = true, Result = "OK" };

        var now = DateTime.UtcNow;
        var asnNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();

        var wmsAsn = new WMSASN
        {
            ASNNumber = asnNumber,
            ExternReceiptNumber = receiving.OrderNo,
            CustomerId = cfg.CustomerId!.Value,
            CustomerName = cfg.CustomerName,
            WarehouseId = cfg.WarehouseId!.Value,
            WarehouseName = cfg.WarehouseName,
            ASNStatus = 1, // TODO: 常量化/枚举化
            ReceiptType = _enumRep.GetEnumDescriptionOrDefault<IbOrderStatusEnum>(receiving.DocType,"收货入库"),
            Creator = (_userManager?.UserId ?? 0).ToString(),
            CreationTime = now,
            TenantId = cfg.TenantId ?? 1300000000001, // 尽量从配置/上下文取
            ExpectDate=DateTime.Now
        };

        var prodMap = productLight.ToDictionary(p => ((string)p.SKU).ToUpper(), p => p);
        var details = new List<WMSASNDetail>(receiving.items.Count);
        foreach (var item in receiving.items)
        {
            var skuKey = item.ItemNum.Trim().ToUpper();
            if (!prodMap.TryGetValue(skuKey, out var prod))
            {
                return new HachWMSResponse { Success = false, Result = $"SKU 不存在：{item.ItemNum}" };
            }

            details.Add(new WMSASNDetail
            {
                ASNNumber = wmsAsn.ASNNumber,
                ExternReceiptNumber = wmsAsn.ExternReceiptNumber,
                CustomerId = wmsAsn.CustomerId,
                CustomerName = wmsAsn.CustomerName,
                WarehouseId = wmsAsn.WarehouseId,
                WarehouseName = wmsAsn.WarehouseName,
                LineNumber = item.LineNum,
                SKU = item.ItemNum,
                UPC = "",
                GoodsName = "", // TODO: 需要则从产品取
                GoodsType = prod.GoodsType,
                PoCode = "",
                Weight = 0,
                Volume = 0,
                ExpectedQty = item.Quantity,
                ReceivedQty = 0,
                ReceiptQty = 0,
                CreationTime = now,
                Creator = wmsAsn.Creator,
                TenantId = wmsAsn.TenantId
            });
        }

        wmsAsn.Details = details;

        // 一次 Nav 写入
        await _wMSASNRep.Context.InsertNav(wmsAsn)
            .Include(a => a.Details)
            .ExecuteReturnEntityAsync();

        return res;
    }

    //获取客户信息
    private async Task<HachWmsAuthorizationConfig> GetCustomerInfo(string Type,string? WarehouseCode=null)
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
            .WhereIF(!string.IsNullOrEmpty(WarehouseCode),a=>a.WarehouseCode==WarehouseCode)
            .OrderByDescending(a => a.Id)
            .FirstAsync();

        return hachCustomerMappings;
    }
}
