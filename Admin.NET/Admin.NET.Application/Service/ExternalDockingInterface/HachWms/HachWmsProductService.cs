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
using Furion.FriendlyException;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBBatchCreateContainerServiceVersionResponse.Types;

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
    private readonly SqlSugarRepository<HachWmsAuthorizationConfig> _hachWmsAuthorizationConfigRep;
    private readonly UserManager _userManager;

    public HachWmsProductService(SqlSugarRepository<HachWmsProduct> hachWmsProductRep
        , SqlSugarRepository<WMSHachCustomerMapping> wMSHachCustomerMappingRep,
        SqlSugarRepository<WMSProduct> wMSProductRep,
        UserManager userManager,
        SqlSugarRepository<HachWmsAuthorizationConfig> hachWmsAuthorizationConfigRep)
    {
        _hachWmsProductRep = hachWmsProductRep;
        _wMSHachCustomerMappingRep = wMSHachCustomerMappingRep;
        _wMSProductRep = wMSProductRep;
        _hachWmsAuthorizationConfigRep = hachWmsAuthorizationConfigRep;
        _userManager = userManager;
    }
    #endregion

    /// <summary>
    /// 异步 同步产品数据
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ApiDescriptionSettings(Name = "putSKUData")]
    public async Task<HachWMSResponse> asyncSyncProductData(List<HachWmsProductInput> input)
    {
        if (_userManager.UserId==null)
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
     
        List<HachWmsAuthorizationConfig> hachCustomerList = new List<HachWmsAuthorizationConfig>();
        hachCustomerList = await GetCustomerInfo("putSKUData");

        if (hachCustomerList == null || hachCustomerList.Count == 0)
            return new HachWMSResponse { Success = false, Result = "no available customer configuration (AppId/interface permissions) matched" };

        try
        {
            foreach (var item in input)
            {

                if (input == null || string.IsNullOrWhiteSpace(item.ItemNumber))
                    return new HachWMSResponse { Success = false, Result = "<ItemNumber> cannot be empty" };

                // 5. 开启事务（跨两个表的一致性）
                var db = _hachWmsProductRep.Context;
                try
                {
                    await db.Ado.BeginTranAsync(); // 开始事务

                    // 1. 失效旧记录 + 插入新的对接记录
                    await db.Updateable<HachWmsProduct>()
                        .SetColumns(p => new HachWmsProduct { Status = false })
                        .Where(p => p.ItemNumber == item.ItemNumber && p.Status == true)
                        .ExecuteCommandAsync();

                    var hachEntity = item.Adapt<HachWmsProduct>();
                    hachEntity.Status = true;
                    hachEntity.TenantId = 1300000000001;
                    hachEntity.ReceivingTime = DateTime.Now;

                    // 插入对接表数据
                    var newHach = await _hachWmsProductRep.InsertReturnEntityAsync(hachEntity);

                    // 2. 遍历客户，更新业务表（WMSProduct）
                    foreach (var customer in hachCustomerList)
                    {
                        var detail = new HachWMSDetailResponse();

                        try
                        {
                            // 先查找是否已经存在
                            var exists = await _wMSProductRep.AsQueryable()
                                .Where(p => p.CustomerId == customer.CustomerId && p.SKU == item.ItemNumber
                                && p.ProductStatus == 1)
                                .OrderByDescending(p => p.Id)
                                .FirstAsync();

                            if (exists == null)
                            {
                                var insert = new WMSProduct
                                {
                                    CustomerId = customer.CustomerId!.Value,
                                    CustomerName = customer.CustomerName,
                                    SKU = item.ItemNumber,
                                    ProductStatus = 1,
                                    GoodsName = string.IsNullOrWhiteSpace(item.DescriptionZhs) ? item.DescriptionEn : item.DescriptionZhs,
                                    GoodsType = item.ItemType,
                                    SKUClassification = item.MakeOrBuy,
                                    Creator = _userManager.UserId.ToString(),
                                    CreationTime = DateTime.Now,
                                    TenantId = customer.TenantId ?? 1300000000001 // 默认 TenantId
                                };

                                await _wMSProductRep.InsertAsync(insert);
                                detail = new HachWMSDetailResponse()
                                {
                                    Remark = "CustomerName:" + customer.CustomerName,
                                    Success = true,
                                    Message = $"SKU:{item.ItemNumber}  Added successfully"
                                };
                            }
                            else
                            {
                                exists.GoodsName = string.IsNullOrWhiteSpace(item.DescriptionZhs) ? item.DescriptionEn : item.DescriptionZhs;
                                exists.GoodsType = item.ItemType;
                                exists.SKUClassification = item.MakeOrBuy;
                                exists.DateTime1 = DateTime.Now;  // 更新记录的时间

                                await _wMSProductRep.UpdateAsync(exists);
                                detail = new HachWMSDetailResponse()
                                {
                                    Remark = "CustomerName:" + customer.CustomerName,
                                    Success = true,
                                    Message = $"SKU:{item.ItemNumber}  Updated successful"
                                };
                            }
                        }
                        catch (Exception exPerCustomer)
                        {
                            detail = new HachWMSDetailResponse()
                            {
                                Remark = "CustomerName:" + customer.CustomerName,
                                Success = false,
                                Message = $"SKU:{item.ItemNumber}  Processing failed: " + exPerCustomer.Message
                            };
                            response.Success = false;
                            response.Result = $"SKU:{item.ItemNumber}  Processing failed: " + exPerCustomer.Message;
                            throw; // 抛出异常以触发事务回滚
                        }
                        finally
                        {
                            response.Items.Add(detail);
                        }
                    }
                    // 提交事务
                    await db.Ado.CommitTranAsync();

                }
                catch (Exception ex)
                {
                    // 如果有任何异常，回滚事务
                    await db.Ado.RollbackTranAsync();
                    return new HachWMSResponse
                    {
                        Success = false,
                        Result = "INTERNAL_ERROR: " + ex.Message
                    };
                }

                // 汇总消息
                if (response.Items.Any(d => !d.Success))
                {
                    response.Success = false;
                    response.Result = "partial processing failed, please check the details";
                }
                else
                {
                    response.Success = true;
                    response.Result = "success";
                }
            }
        }
        catch (Exception ex)
        {
            return new HachWMSResponse { Success = false, Result = "processing failed：" + ex.Message+ "" };
        }

        return response;
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
