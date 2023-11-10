using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSProduct服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSProductService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSProduct> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    public WMSProductService(SqlSugarRepository<WMSProduct> rep, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager)
    {
        _rep = rep;
        _repCustomerUser = repCustomerUser;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSProductOutput>> Page(WMSProductInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(input.ProductStatus > 0, u => u.ProductStatus == input.ProductStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKUClassification), u => u.SKUClassification.Contains(input.SKUClassification.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKULevel), u => u.SKULevel.Contains(input.SKULevel.Trim()))
                    .WhereIF(input.SuperId > 0, u => u.SuperId == input.SuperId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKUGroup), u => u.SKUGroup.Contains(input.SKUGroup.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ManufacturerSKU), u => u.ManufacturerSKU.Contains(input.ManufacturerSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RetailSKU), u => u.RetailSKU.Contains(input.RetailSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceSKU), u => u.ReplaceSKU.Contains(input.ReplaceSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxGroup), u => u.BoxGroup.Contains(input.BoxGroup.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Country), u => u.Country.Contains(input.Country.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Manufacturer), u => u.Manufacturer.Contains(input.Manufacturer.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DangerCode), u => u.DangerCode.Contains(input.DangerCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Cost), u => u.Cost.Contains(input.Cost.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Color), u => u.Color.Contains(input.Color.Trim()))
                    .WhereIF(input.ExpirationDate > 0, u => u.ExpirationDate == input.ExpirationDate)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str6), u => u.Str6.Contains(input.Str6.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str7), u => u.Str7.Contains(input.Str7.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str8), u => u.Str8.Contains(input.Str8.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str9), u => u.Str9.Contains(input.Str9.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str10), u => u.Str10.Contains(input.Str10.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str11), u => u.Str11.Contains(input.Str11.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str12), u => u.Str12.Contains(input.Str12.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str13), u => u.Str13.Contains(input.Str13.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str14), u => u.Str14.Contains(input.Str14.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str15), u => u.Str15.Contains(input.Str15.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str16), u => u.Str16.Contains(input.Str16.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str17), u => u.Str17.Contains(input.Str17.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str18), u => u.Str18.Contains(input.Str18.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str19), u => u.Str19.Contains(input.Str19.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str20), u => u.Str20.Contains(input.Str20.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)

                    .Select<WMSProductOutput>()
;
        if (input.CreationTime != null && input.CreationTime.Count > 0)
        {
            DateTime? start = input.CreationTime[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
            {
                var end = input.CreationTime[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSProductInput input)
    {
        var entity = input.Adapt<WMSProduct>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSProductInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSProductInput input)
    {
        var entity = input.Adapt<WMSProduct>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }



    /// <summary>
    /// 获取WMSProduct 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSProduct> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSProduct列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSProductOutput>> List([FromQuery] WMSProductInput input)
    {
        return await _rep.AsQueryable().Select<WMSProductOutput>().ToListAsync();
    }



    /// <summary>
    /// 获取SKU列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectSKU")]
    public async Task<List<SelectListItem>> SelectSKU(dynamic objData )
    {
        string sku = objData.inputData;
     
        if (!string.IsNullOrEmpty(sku) &&  sku.Length > 3)
        {
            long customerId = objData.objData.CustomerId;
            //获取可以使用的仓库权限
            var customer = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.CustomerId).ToList();
            //string sku = objData.inputData;
            //long customerId = objData.objData.CustomerId;
            return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerId) && a.CustomerId == customerId && a.SKU.Contains(sku)).Select(a => new SelectListItem { Text = a.SKU, Value = a.GoodsName.ToString() }).Distinct().ToListAsync();
        }
        else
        {
            return new List<SelectListItem>();
        }
    }


}

