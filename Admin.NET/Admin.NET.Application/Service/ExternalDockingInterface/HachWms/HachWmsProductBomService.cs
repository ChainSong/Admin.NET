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
using Furion.FriendlyException;
using Admin.NET.Application.Service.ExternalDocking_Interface.Dto;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;

namespace Admin.NET.Application.Service.ExternalDocking_Interface.HachWms;
/// <summary>
/// EXTERNAL DOCKING INTERFACE : HACHWMS PRODUCT BOM 
/// </summary>

[ApiDescriptionSettings("HachWmsProductBom", Order = 5, Groups = new[] { "HACHWMS INTERFACE" })]
public class HachWmsProductBomService : IDynamicApiController, ITransient
{
    #region 依赖注入
    private readonly SqlSugarRepository<HachWmsProductBom> _hachWmsProductBomRep;
    private readonly SqlSugarRepository<HachWmsProductBomDetail> _wMSProductBomDetailRep;

    private readonly SqlSugarRepository<WMSProduct> _wMSProductRep;
    private readonly SqlSugarRepository<WMSProductBom> _wMSProductBomRep;

    private readonly SqlSugarRepository<WMSHachCustomerMapping> _wMSHachCustomerMappingRep;
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly UserManager _userManager;

    public HachWmsProductBomService(SqlSugarRepository<HachWmsProductBom> hachWmsProductBomRep
        , SqlSugarRepository<HachWmsProductBomDetail> wMSProductBomDetailRep,
        SqlSugarRepository<WMSProduct> wMSProductRep, SqlSugarRepository<WMSProductBom> wMSProductBomRep,
        UserManager userManager, SqlSugarRepository<WMSHachCustomerMapping> wMSHachCustomerMappingRep,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep)
    {
        _userManager = userManager;
        _hachWmsProductBomRep = hachWmsProductBomRep;
        _wMSProductBomDetailRep = wMSProductBomDetailRep;
        _wMSProductRep = wMSProductRep;
        _wMSProductBomRep = wMSProductBomRep;
        _wMSHachCustomerMappingRep = wMSHachCustomerMappingRep;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
    }
    #endregion

    /// <summary>
    /// 异步 同步产品数据
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ApiDescriptionSettings(Name = "putBOMData")]
    public async Task<HachWMSResponse> asyncSyncProductBomData(List<HachWmsProductBomInput> input)
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
        //基础校验
        const int MaxBatch = 20;
        //入参不能为空
        if (input == null || input.Count == 0)
            return new HachWMSResponse { Success = false, Result = ErrorCode.BadRequest.GetDescription() };
        //单次请求最多允许20条，当前21条
        if (input.Count > MaxBatch)
            return new HachWMSResponse { Success = false, Result = $"a maximum of  {MaxBatch} equests are allowed per request，currently {input.Count} items" };

        HachWmsProductBom hachWmsProductBom = new HachWmsProductBom();
        List<HachWmsAuthorizationConfig> wmsAuthorizationConfigList = new List<HachWmsAuthorizationConfig>();
        wmsAuthorizationConfigList = await GetCustomerInfo("putBOMData");

        if (wmsAuthorizationConfigList == null || wmsAuthorizationConfigList.Count == 0)
            return new HachWMSResponse { Success = false, Result = "no available customer configuration (AppId/interface permissions) matched" };

        foreach (var bom in input)
        {
            var orderResult = new HachWMSDetailResponse { Success = true };

            foreach (var customer in wmsAuthorizationConfigList)
            {
                try
                {
                    // 每个客户的独立处理
                    var customerResult = await ProcessCustomerBom(bom, customer);
                    if (!customerResult.Success)
                    {
                        response.Items.Add(customerResult);
                    }
                }
                catch (Exception ex)
                {
                    // 捕获异常并记录每个客户的失败情况
                    orderResult.Remark = $"CustomerName:{customer.CustomerName}";
                    orderResult.Message = $"ItemNumber：{bom.ItemNumber} 处理异常：{ex.Message}";
                    orderResult.Success = false;
                    response.Items.Add(orderResult);
                }
            }
        }
        // 汇总成功与否
        response.Success = response.Items.All(i => i.Success);
        response.Result = response.Success ? "success" : "partial/complete failure";
        return response;
    }

    // 处理单个客户的 BOM 逻辑
    private async Task<HachWMSDetailResponse> ProcessCustomerBom(HachWmsProductBomInput bom, HachWmsAuthorizationConfig customer)
    {
        var orderResult = new HachWMSDetailResponse { Success = true };

        // 输入校验等逻辑
        if (bom.items == null || bom.items.Count == 0)
        {
            orderResult.Remark = $"CustomerName:{customer.CustomerName}";
            orderResult.Message = $"ItemNumber：{bom.ItemNumber} details are empty";
            orderResult.Success = false;
            return orderResult;
        }

        var products = await _wMSProductRep.AsQueryable()
                      .Where(p => p.CustomerId == customer.CustomerId)
                      .Where(p => SqlFunc.ToUpper(p.SKU) == SqlFunc.ToUpper(bom.ItemNumber))
                      .Where(p => p.ProductStatus == 1)
                      .FirstAsync();

        if (products == null)
        {
            orderResult.Remark = $"CustomerName:{customer.CustomerName}";
            orderResult.Message = $"Missing ItemNumber：{bom.ItemNumber} ";
            orderResult.Success = false;
            return orderResult;
        }
        // **检查对接表是否已经插入过数据**
        var existingBom = await _hachWmsProductBomRep.AsQueryable()
                              .Where(p => p.ItemNumber == bom.ItemNumber)
                              .Where(p => p.Status)
                              .FirstAsync();
        if (existingBom == null)
        {
            // 对接表数据尚未插入，执行插入
            var hachWMSProductBom = MapProductBom(bom, _userManager?.UserId);
            await _hachWmsProductBomRep.Context.InsertNav(hachWMSProductBom)
                .Include(a => a.items)
                .ExecuteReturnEntityAsync();
        }
        // 事务处理（每个客户独立事务）
        var tranRes = await _wMSProductBomRep.Context.Ado.UseTranAsync(async () =>
        {
            try
            {
                var result = await SyncWmsProductBomTransactional(bom, customer, products);
                if (!result.Success)
                {
                    throw new Exception(result.Result ?? "Business table processing failed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Processing failed: {ex.Message}");
            }
        });

        if (!tranRes.IsSuccess)
        {
            orderResult.Remark = $"CustomerName:{customer.CustomerName}";
            orderResult.Message = tranRes.ErrorMessage;
            orderResult.Success = false;
        }

        return orderResult;
    }

    /// <summary>
    /// 映射产品BOM
    /// </summary>
    /// <param name="bom"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    private HachWmsProductBom MapProductBom(HachWmsProductBomInput bom, long? userId)
    {
        var productBom = bom.Adapt<HachWmsProductBom>(); // 确保 Mapster 配置包含 items
        productBom.CreateUserId = userId ?? 0;
        productBom.TenantId = 1300000000001;
        productBom.ReceivingTime = DateTime.Now;
        return productBom;
    }
    /// <summary>
    /// 在事务中写业务表，外层已开启事务
    /// 传入已查好的产品列表避免重复查询
    /// </summary>
    private async Task<HachWMSResponse> SyncWmsProductBomTransactional(
        HachWmsProductBom productBom,
        HachWmsAuthorizationConfig cfg,
        WMSProduct product)
    {
        var res = new HachWMSResponse { Success = true, Result = "OK" };

        List<WMSProductBom> bomDetailList = new List<WMSProductBom>();

        try
        {
            var detail = new HachWMSDetailResponse();

            // 先查找是否已经存在
            var exists = await _wMSProductBomRep.AsQueryable()
                .Where(p => p.CustomerId == cfg.CustomerId)
                .Where(p => p.SKU == product.SKU)
                .Where(p => p.ProductId == product.Id)
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            //这个sku  从来没有 下发过Bom信息
            if (exists == null || exists.Count == 0)
            {
                productBom.items.ForEach(item =>
                {
                    bomDetailList.Add(new WMSProductBom
                    {
                        CustomerId = cfg.CustomerId.Value,
                        CustomerName = cfg.CustomerName,
                        ProductId = product.Id,
                        SKU = product.SKU,
                        ChildSKUId = 0,
                        ChildSKU = item.ComponentItem,
                        ChildGoodsName = item.ComponentDesc,
                        Qty = item.ComponentQuantity,
                        UnitCode = item.ComponentUom,
                        Creator = _userManager?.UserId.ToString() ?? "0",
                        TenantId = product.TenantId,
                        Str1 = item.ComponentSeq,
                        DateTime1 = item.DateFrom,
                    });
                });

                await _wMSProductBomRep.InsertRangeAsync(bomDetailList);
                detail = new HachWMSDetailResponse()
                {
                    Remark = $"ItemNumber:{productBom.ItemNumber},ComponentItem counts{productBom.items.Count}Added successfully ",
                    Success = true,
                    Message = "Added successfully"
                };
            }
            else
            {
                foreach (var item in productBom.items)
                {
                    WMSProductBom wMSProductBom = new WMSProductBom();
                    wMSProductBom = exists.Where(a => a.ChildSKU == item.ComponentItem).FirstOrDefault();
                    //说明这个子SKU  已经下发了  现在做更新操作
                    if (wMSProductBom != null)
                    {
                        wMSProductBom.ChildGoodsName = item.ComponentDesc;
                        wMSProductBom.Qty = item.ComponentQuantity;
                        wMSProductBom.CreationTime = DateTime.Now;
                        wMSProductBom.DateTime1 = item.DateFrom;
                        await _wMSProductBomRep.UpdateAsync(wMSProductBom);
                        detail = new HachWMSDetailResponse()
                        {
                            Remark = $"ItemNumber:{productBom.ItemNumber};ComponentItem:{item.ComponentItem}",
                            Success = true,
                            Message = "Updated successfully"
                        };
                    }
                    else
                    {
                        wMSProductBom = new WMSProductBom
                        {
                            CustomerId = cfg.CustomerId.Value,
                            CustomerName = cfg.CustomerName,
                            ProductId = product.Id,
                            SKU = product.SKU,
                            ChildSKUId = 0,
                            ChildSKU = item.ComponentItem,
                            ChildGoodsName = item.ComponentDesc,
                            Qty = item.ComponentQuantity,
                            UnitCode = item.ComponentUom,
                            Creator = _userManager?.UserId.ToString() ?? "0",
                            TenantId = product.TenantId,
                            Str1 = item.ComponentSeq,
                            DateTime1 = item.DateFrom,
                        };
                        await _wMSProductBomRep.InsertAsync(wMSProductBom);
                        detail = new HachWMSDetailResponse()
                        {
                            Remark = $"ItemNumber:{productBom.ItemNumber};ComponentItem:{item.ComponentItem}",
                            Success = true,
                            Message = "Added successfully"
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            res = new HachWMSResponse { Success = false, Result = $"processing failed：{ex.Message}" };
        }

        return res;
    }

    /// <summary>
    /// 获取客户的信息
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    private async Task<List<HachWmsAuthorizationConfig>> GetCustomerInfo(string Type)
    {
        List<HachWmsAuthorizationConfig> hachCustomerMappings = new List<HachWmsAuthorizationConfig>();

        if (string.IsNullOrEmpty(Type))
        {
            return hachCustomerMappings;
        }

        hachCustomerMappings = await _hachWmsAuthorizationConfigRep.AsQueryable()
            .Where(a => a.AppId == _userManager.UserId)
            .Where(a => a.Type == "HachWMSApi")
            .Where(a => a.InterFaceName == Type)
            .ToListAsync();

        return hachCustomerMappings;
    }
}
