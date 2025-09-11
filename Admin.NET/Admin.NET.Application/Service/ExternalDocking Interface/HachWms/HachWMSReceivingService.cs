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

    public HachWMSReceivingService(
        SqlSugarRepository<HachWmsReceiving> hachWmsReceivingRep,
        SqlSugarRepository<WMSASN> wMSASNRep,
        SqlSugarRepository<WMSProduct> wMSProductRep,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep,
        UserManager userManager)
    {
        _hachWmsReceivingRep = hachWmsReceivingRep;
        _wMSASNRep = wMSASNRep;
        _wMSProductRep = wMSProductRep;
        _userManager = userManager;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
    }
    [HttpPost]
    //[Authorize]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "putASNData")]
    public async Task<HachWMSResponse> asyncSyncReceiving(List<HachWmsReceivingInput> input)
    {
        HachWMSResponse response = new HachWMSResponse()
        {
            Success = true,
        };
        HachWmsReceiving receiving = new HachWmsReceiving();
        HachWmsAuthorizationConfig wmsAuthorizationConfig = new HachWmsAuthorizationConfig();

        wmsAuthorizationConfig = await GetCustomerInfo("putASNData");

        foreach (var asn in input)
        {
            string syncOrderNo = asn.OrderNo == null ? asn.ShipmentNum + asn.ReceiptNum + asn.DocNumber : asn.OrderNo;
            var existing = await _wMSASNRep
                .GetFirstAsync(x => x.ASNStatus != -1 && x.ExternReceiptNumber == syncOrderNo);
            if (existing != null)
            {
                response.Result += $"订单号：{syncOrderNo}对接失败：订单号:{syncOrderNo} 已存在;";
                response.Success = false;
                continue;
            }
            else
            {
                var Skus = asn.items.Select(x => x.ItemNum).Distinct().ToList(); // Ensure Skus is distinct

                // 使用 HashSet 优化 Contains 操作
                var skuSet = new HashSet<string>(Skus);

                var products = await _wMSProductRep.AsQueryable()
                    .Where(p => p.CustomerId == wmsAuthorizationConfig.CustomerId && skuSet.Contains(p.SKU))
                    .Where(p => p.ProductStatus == 1)
                    .ToListAsync();

                // 如果产品数量少于 SKU 数量，查找缺少的 SKU
                if (products.Count < Skus.Count)
                {
                    var missingSkus = Skus.Except(products.Select(p => p.SKU)).ToList();  // 查找缺失的 SKU
                    response.Result += $"订单号：{syncOrderNo} 对接失败：系统缺失SKU: {string.Join(", ", missingSkus)};";
                    response.Success = false;
                    continue; // 跳过当前循环，处理下一个 ASN
                }
                receiving = asn.Adapt<HachWmsReceiving>();
                receiving.OrderNo = syncOrderNo;
                receiving.CreateUserId = _userManager.UserId;
                //写入对接表
                var syncDockData = await SyncHachWmsReceiving(receiving, wmsAuthorizationConfig);
                //写入业务表
                var syncBusinessData = await SyncWmsAsn(receiving, wmsAuthorizationConfig);
            }
        }
        response.Result = "成功";
        return response;
    }
    /// <summary>
    /// 写入对接表
    /// </summary>
    /// <param name="receiving"></param>
    /// <returns></returns>
    private async Task<HachWmsReceiving> SyncHachWmsReceiving(HachWmsReceiving receiving, HachWmsAuthorizationConfig wmsAuthorizationConfig)
    {
        var sql= _hachWmsReceivingRep.Context.InsertNav(receiving).Include(a => a.items).ToSqlValue();
        return await _hachWmsReceivingRep.Context.InsertNav(receiving).Include(a => a.items).ExecuteReturnEntityAsync();
    }
    /// <summary>
    /// 写入业务表
    /// </summary>
    /// <param name="receiving"></param>
    /// <returns></returns>
    private async Task<HachWMSResponse> SyncWmsAsn(HachWmsReceiving receiving, HachWmsAuthorizationConfig wmsAuthorizationConfig)
    {
        HachWMSResponse hachWMSResponse = new HachWMSResponse()
        {
            Success = true,
            Result = "成功"
        };
        WMSASN wMSASN = new WMSASN();
        List<WMSASNDetail> wMSASNDetail = new List<WMSASNDetail>();
        WMSProduct wMSProduct = new WMSProduct();
        wMSASN = new WMSASN
        {
            ASNNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
            ExternReceiptNumber = receiving.OrderNo,
            CustomerId = wmsAuthorizationConfig.CustomerId.Value,
            CustomerName = wmsAuthorizationConfig.CustomerName,
            WarehouseId = wmsAuthorizationConfig.WarehouseId.Value,
            WarehouseName = wmsAuthorizationConfig.WarehouseName,
            ASNStatus = 1,
            ReceiptType = receiving.DocType,
            Creator = _userManager.UserId.ToString(),
            CreationTime = DateTime.Now,
            TenantId = 1300000000001
        };
        foreach (var item in receiving.items)
        {
            wMSProduct = await _wMSProductRep.AsQueryable()
            .Where(p => p.CustomerId == wmsAuthorizationConfig.CustomerId.Value && p.SKU == item.ItemNum)
            .Where(p => p.ProductStatus == 1)
            .OrderByDescending(p => p.Id)
            .FirstAsync();
            wMSASNDetail.Add(new WMSASNDetail
            {
                ASNNumber = wMSASN.ASNNumber,
                ExternReceiptNumber = receiving.OrderNo,
                CustomerId = wmsAuthorizationConfig.CustomerId.Value,
                CustomerName = wmsAuthorizationConfig.CustomerName,
                WarehouseId = wmsAuthorizationConfig.WarehouseId.Value,
                WarehouseName = wmsAuthorizationConfig.WarehouseName,
                LineNumber = item.LineNum,
                SKU = item.ItemNum,
                UPC = "",
                GoodsName = "",
                GoodsType = wMSProduct.GoodsType,
                PoCode = "",
                Weight = 0,
                Volume = 0,
                ExpectedQty = item.Quantity,
                ReceivedQty = 0,
                ReceiptQty = 0,
                CreationTime = DateTime.Now,
                Creator = _userManager.UserId.ToString(),
                TenantId = 1300000000001
            });
        }
        wMSASN.Details = wMSASNDetail;
        var asnResult = await _wMSASNRep.Context.InsertNav(wMSASN).Include(a => a.Details).ExecuteReturnEntityAsync();
        return hachWMSResponse;
    }

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
