// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
using Admin.NET.Application.Service.ExternalDocking_Interface.HachWms.Dto;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.ExternalDocking_Interface.HachWms;
/// <summary>
/// EXTERNAL DOCKING INTERFACE : HACHWMS PRODUCT 
/// </summary>
[ApiDescriptionSettings("HachWmsProduct", Order = 2, Groups = new[] { "HACHWMS INTERFACE" })]
public class HachWmsProductService : IDynamicApiController, ITransient
{
    #region 依赖注入
    private readonly SqlSugarRepository<HachWmsProduct> _hachWmsProductRep;
    private readonly SqlSugarRepository<WMSProduct> _wMSProductRep;
    private readonly SqlSugarRepository<WMSHachCustomerMapping> _wMSHachCustomerMappingRep;
    public HachWmsProductService(SqlSugarRepository<HachWmsProduct> hachWmsProductRep
        , SqlSugarRepository<WMSHachCustomerMapping> wMSHachCustomerMappingRep,
        SqlSugarRepository<WMSProduct> wMSProductRep)
    {
        _hachWmsProductRep = hachWmsProductRep;
        _wMSHachCustomerMappingRep = wMSHachCustomerMappingRep;
        _wMSProductRep = wMSProductRep;
    }
    #endregion

    /// <summary>
    /// 异步 同步产品数据
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    //[Authorize]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "putSKUData")]
    public async Task<HachWMSResponse> asyncSyncProductData(HachWmsProductInput input)
    {
        HachWMSResponse response = new HachWMSResponse()
        {
            Success = true,
            Result = "成功"
        };

        List<WMSHachCustomerMapping> hachCustomerList = new List<WMSHachCustomerMapping>();
        HachWmsProduct hachWmsProductInput = new HachWmsProduct();
        WMSProduct wMSProduct = new WMSProduct();
        HachWmsProduct hachWmsProduct = new HachWmsProduct();

        hachWmsProductInput = input.Adapt<HachWmsProduct>();
        hachWmsProduct = await asyncSyncHachWmsProduct(hachWmsProductInput);
        hachCustomerList = await GetCustomerInfo("CustomerType");

        foreach (var customer in hachCustomerList)
        {
            var wMSProductInfo = await asyncSyncWmsProduct(hachWmsProduct, customer);
            if (wMSProductInfo == null)
            {
                response = new HachWMSResponse
                {
                    Success = false,
                    Result = "" + hachWmsProduct.ItemNumber + "对接失败"
                };
            }
        }
        return response;
    }
    /// <summary>
    /// 处理产品的对接信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<HachWmsProduct> asyncSyncHachWmsProduct(HachWmsProduct input)
    {

        //先查询对接表的产品信息
        var hachProductList = await _hachWmsProductRep.AsQueryable()
            .Where(p => p.ItemNumber == input.ItemNumber)
            .Where(p => p.Status == true)
            .OrderByDescending(p => p.Id)
            .ToListAsync();

        if (hachProductList != null && hachProductList.Count > 0)
        {
            // 更新旧的产品状态为已过时或停用
            foreach (var product in hachProductList)
            {
                product.Status = false;  // 例如将状态改为 false 表示过时
            }
            // 批量更新旧的对接记录
            await _hachWmsProductRep.UpdateRangeAsync(hachProductList);
        }

        input.Status = true; // 设置新记录的状态为有效
        input.ReceivingTime = DateTime.Now;
        return await _hachWmsProductRep.InsertReturnEntityAsync(input);
    }
    /// <summary>
    /// 处理产品的业务信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<WMSProduct> asyncSyncWmsProduct(HachWmsProduct input, WMSHachCustomerMapping customer)
    {
        WMSProduct wMSProduct = new WMSProduct();
        WMSProduct wMSAddProduct = new WMSProduct();
        WMSProduct wMSProductInfo = new WMSProduct();

        //先查询wms的产品信息
        wMSProduct = await _wMSProductRep.AsQueryable()
                    .Where(p => p.CustomerId == customer.CustomerId && p.SKU == input.ItemNumber)
                    .Where(p => p.ProductStatus == 1)
                    .OrderByDescending(p => p.Id)
                    .FirstAsync();

        if (wMSProduct == null)
        {
            wMSAddProduct = new WMSProduct
            {
                CustomerId = customer.Id,
                CustomerName = customer.CustomerName,
                SKU = input.ItemNumber,
                ProductStatus = 1,
                GoodsName = input.DescriptionZhs,// 中文描述
                GoodsType = input.ItemType,//产品类别
                SKUClassification = input.MakeOrBuy,
                SKULevel = "",
                SuperId = 0,
                SKUGroup = "",
                ManufacturerSKU = "",
                ReplaceSKU = "",
                RetailSKU = "",
                BoxGroup = "",
                Country = "",
                Manufacturer = "",
                DangerCode = "",
                Volume = 0,
                StandardVolume = 0,
                Weight = 0,
                StandardNetWeight = 0,
                StandardWeight = 0,
                Price = 0,
                ActualPrice = 0,
                Cost = "",
                Color = "",
                Length = 0,
                Wide = 0,
                High = 0,
                ExpirationDate = 0,
                IsNFC = 0,
                IsRFID = 0,
                IsQC = 0,
                IsSN = 0,
                IsAssembly = 0,
                Remark = "",
                Creator = "HachWmsApi",
                CreationTime = DateTime.Now,
                Str1 = "",
                Str2 = "",
                Str3 = "",
                Str4 = "",
                Str5 = "",
                Str6 = "",
                Str7 = "",
                Str8 = "",
                Str9 = "",
                Str10 = "",
                Str11 = "",
                Str12 = "",
                Str13 = "",
                Str14 = "",
                Str15 = "",
                Str16 = "",
                Str17 = "",
                Str18 = "",
                Str19 = "",
                Str20 = "",
                DateTime1 = null,
                DateTime2 = null,
                DateTime3 = null,
                Int1 = 0,
                Int2 = 0,
                Int3 = 0,
                TenantId = 1300000000001
            };

            wMSProductInfo = await _wMSProductRep.InsertReturnEntityAsync(wMSAddProduct);
        }
        else
        {
            wMSProduct.GoodsName = input.DescriptionZhs;
            wMSProduct.GoodsType = input.ItemType;
            wMSProduct.SKUClassification = input.MakeOrBuy;
            wMSProduct.CreationTime = DateTime.Now;
            await _wMSProductRep.UpdateAsync(wMSProduct);
            wMSProductInfo = wMSProduct;
        }
        return wMSProductInfo;
    }
    /// <summary>
    /// 获取客户的信息
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    private async Task<List<WMSHachCustomerMapping>> GetCustomerInfo(string Type)
    {
        List<WMSHachCustomerMapping> hachCustomerMappings = new List<WMSHachCustomerMapping>();
        if (string.IsNullOrEmpty(Type))
        {
            return hachCustomerMappings;
        }
        hachCustomerMappings = await _wMSHachCustomerMappingRep.AsQueryable()
            .Where(a => a.Type == Type)
            .ToListAsync();
        return hachCustomerMappings;
    }
}
