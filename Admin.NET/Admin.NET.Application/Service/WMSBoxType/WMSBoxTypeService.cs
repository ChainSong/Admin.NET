using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSBoxType服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSBoxTypeService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSBoxType> _rep;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly UserManager _userManager;
    public WMSBoxTypeService(SqlSugarRepository<WMSBoxType> rep, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager)
    {
        _rep = rep;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询WMSBoxType
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSBoxTypeOutput>> Page(WMSBoxTypeInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxCode), u => u.BoxCode.Contains(input.BoxCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxType), u => u.BoxType.Contains(input.BoxType.Trim()))
                    .WhereIF(input.BoxStatus > 0, u => u.BoxStatus == input.BoxStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<WMSBoxTypeOutput>()
;
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSBoxType
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSBoxTypeInput input)
    {
        var entity = input.Adapt<WMSBoxType>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSBoxType
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSBoxTypeInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //var entity = input.Adapt<WMSBoxType>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSBoxType
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSBoxTypeInput input)
    {
        var entity = input.Adapt<WMSBoxType>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSBoxType 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSBoxType> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSBoxType列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSBoxTypeOutput>> List([FromQuery] WMSBoxTypeInput input)
    {
        return await _rep.AsQueryable().Select<WMSBoxTypeOutput>().ToListAsync();
    }



    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectBoxType")]
    public async Task<List<SelectListItem>> SelectBoxType(dynamic input)
    {

        try
        {
            string boxTypeInput = input.inputData;
            //获取可以操作的仓库的箱类型
            var getWarehouse = await _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).ToListAsync();
            // 获取可以使用的仓库权限
            if (!string.IsNullOrEmpty(boxTypeInput))
            {
                return await _rep.AsQueryable().Where(a => getWarehouse.Select(b => b.WarehouseId).Contains(a.WarehouseId)).Select(a => new SelectListItem { Text = a.BoxType, Value = a.BoxCode.ToString() }).ToListAsync();

                //return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerName) && a.CustomerName.Contains(customerInput)).Select(a => new SelectListItem { Text = a.CustomerName, Value = a.Id.ToString() }).ToListAsync();
                //return await _rep.AsQueryable().Where(a => warehouse.Contains(a.WarehouseName) && a.WarehouseName.Contains(warehouseinput)).Select(a => new SelectListItem { Text = a.WarehouseName, Value = a.Id.ToString() }).Distinct().ToListAsync();
                //return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerId) && a.CustomerId == customerId && a.SKU.Contains(sku)).Select(a => new SelectListItem { Text = a.SKU, Value = a.GoodsName.ToString() }).Distinct().Take(6).ToListAsync();
            }
            else
            {
                return await _rep.AsQueryable().Select(a => new SelectListItem { Text = a.BoxType, Value = a.BoxCode.ToString() }).ToListAsync();
                //return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerName)).Select(a => new SelectListItem { Text = a.CustomerName, Value = a.Id.ToString() }).ToListAsync();
                //return await _rep.AsQueryable().Where(a => warehouse.Contains(a.WarehouseName)).Select(a => new SelectListItem { Text = a.WarehouseName, Value = a.Id.ToString() }).Distinct().ToListAsync();
            }
        }
        catch (Exception)
        {
            throw Oops.Oh("请选择客户");
        }
        //获取可以使用的仓库权限
        //var customer = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.CustomerName).ToList();
        //return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerName)).Select(a => new SelectListItem { Text = a.CustomerName, Value = a.Id.ToString() }).ToListAsync();
    }




}

