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
    //[Authorize]
    [ApiDescriptionSettings(Name = "putSOData")]
    public async Task<HachWMSResponse> asyncSyncOutBound(List<HachWmsOutBoundInput> input)
    {
        HachWMSResponse response = new HachWMSResponse()
        {
            Success = true,
            Result = "成功"
        };
        HachWmsOutBound outBound = new HachWmsOutBound();

        HachWmsAuthorizationConfig wmsAuthorizationConfig = new HachWmsAuthorizationConfig();

        wmsAuthorizationConfig = await GetCustomerInfo("putASNData");

        foreach (var order in input)
        {
            string syncOrderNo = order.OrderNumber == null ? order.SoNumber + order.DeliveryNumber : order.OrderNumber;
            var existing = await _wMSPreorderRep.GetFirstAsync(x => x.PreOrderStatus != -1 && x.ExternOrderNumber == syncOrderNo);
            if (existing != null)
            {
                response.Result += $"订单号:{syncOrderNo} 已存在;";
            }
            else
            {
                var Skus = order.items.Select(x => x.ItemNumber).Distinct().ToList(); // Ensure Skus is distinct

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

                    continue; // 跳过当前循环，处理下一个 Order
                }
                else
                {
                    outBound = order.Adapt<HachWmsOutBound>();
                    outBound.OrderNumber = syncOrderNo;
                    outBound.CreateUserId = _userManager.UserId;
                    //写入对接表
                    var syncDockData = await SyncHachWmsOutBound(outBound, wmsAuthorizationConfig);
                    //写入业务表
                    var syncBusinessData = await SyncWmsPreOrder(outBound, wmsAuthorizationConfig);
                }
            }
        }

        return response;
    }

    /// <summary>
    /// 写入对接表
    /// </summary>
    /// <param name="receiving"></param>
    /// <returns></returns>
    private async Task<HachWmsOutBound> SyncHachWmsOutBound(HachWmsOutBound outBound, HachWmsAuthorizationConfig wmsAuthorizationConfig)
    {
        return await _hachWmsOutBoundRep.Context.InsertNav(outBound).Include(a => a.items).ExecuteReturnEntityAsync();
    }
    /// <summary>
    /// 写入业务表
    /// </summary>
    /// <param name="receiving"></param>
    /// <returns></returns>
    private async Task<HachWMSResponse> SyncWmsPreOrder(HachWmsOutBound outBound, HachWmsAuthorizationConfig wmsAuthorizationConfig)
    {
        HachWMSResponse hachWMSResponse = new HachWMSResponse()
        {
            Success = true,
            Result = "成功"
        };

        WMSPreOrder wMSPreOrder = new WMSPreOrder();
        WMSOrderAddress wMSOrderAddress = new WMSOrderAddress();
        List<WMSPreOrderDetail> wMSPreOrderDetail = new List<WMSPreOrderDetail>();
        WMSProduct wMSProduct = new WMSProduct();

        //主数据
        wMSPreOrder = new WMSPreOrder
        {
            PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
            ExternOrderNumber = outBound.OrderNumber,
            CustomerId = wmsAuthorizationConfig.CustomerId.Value,
            CustomerName = wmsAuthorizationConfig.CustomerName,
            WarehouseId = wmsAuthorizationConfig.WarehouseId.Value,
            WarehouseName = wmsAuthorizationConfig.WarehouseName,
            OrderType = "",
            PreOrderStatus = 1,
            OrderTime = Convert.ToDateTime(outBound.ScheduleShippingDate),
            Po = "",
            So = outBound.SoNumber,//销售单号
            DetailCount = outBound.items.Count,
            Creator = _userManager.UserId.ToString(),
            CreationTime = DateTime.Now,
            TenantId = 1300000000001
        };
        //地址信息
        wMSOrderAddress = new WMSOrderAddress
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
            CreationTime = DateTime.Now,
            Creator = _userManager.UserId.ToString(),
            TenantId = 1300000000001
        };
        //明细信息
        foreach (var item in outBound.items)
        {
            wMSProduct = await _wMSProductRep.AsQueryable()
            .Where(p => p.CustomerId == wmsAuthorizationConfig.CustomerId && p.SKU == item.ItemNumber)
            .Where(p => p.ProductStatus == 1)
            .OrderByDescending(p => p.Id)
            .FirstAsync();
            wMSPreOrderDetail.Add(new WMSPreOrderDetail
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
                GoodsType = wMSProduct.GoodsType,
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

        wMSPreOrder.OrderAddress = wMSOrderAddress;
        wMSPreOrder.Details = wMSPreOrderDetail;

        var orderResult = await _wMSPreorderRep.Context.InsertNav(wMSPreOrder)
            .Include(a => a.Details)
            .Include(a => a.OrderAddress)
            .ExecuteReturnEntityAsync();

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
