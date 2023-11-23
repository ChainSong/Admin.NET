using Admin.NET.Common.EnumCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSLocation服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSLocationService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSLocation> _rep;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;

    public WMSLocationService(SqlSugarRepository<WMSLocation> rep, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser)
    {
        _rep = rep;
        _repWarehouseUser = repWarehouseUser;
    }

    /// <summary>
    /// 分页查询WMSLocation
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSLocationOutput>> Page(WMSLocationInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.AreaId > 0, u => u.AreaId == input.AreaId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AreaName), u => u.AreaName.Contains(input.AreaName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                    .WhereIF(input.LocationStatus != 0, u => u.LocationStatus == input.LocationStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LocationType), u => u.LocationType.Contains(input.LocationType.Trim()))
                    .WhereIF(input.Classification > 0, u => u.Classification == input.Classification)
                    .WhereIF(input.Category > 0, u => u.Category == input.Category)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ABCClassification), u => u.ABCClassification.Contains(input.ABCClassification.Trim()))
                    .WhereIF(input.IsMultiLot > 0, u => u.IsMultiLot == input.IsMultiLot)
                    .WhereIF(input.IsMultiSKU > 0, u => u.IsMultiSKU == input.IsMultiSKU)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LocationLevel), u => u.LocationLevel.Contains(input.LocationLevel.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsPutOrder), u => u.GoodsPutOrder.Contains(input.GoodsPutOrder.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsPickOrder), u => u.GoodsPickOrder.Contains(input.GoodsPickOrder.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)

                    .Select<WMSLocationOutput>()
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
    /// 增加WMSLocation
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(AddWMSLocationInput input)
    {
        var entity = input.Adapt<WMSLocation>();
        //添加之前先校验有没有
        var data = _rep.AsQueryable().Where(a => a.Location == entity.Location && a.WarehouseId == entity.WarehouseId);
        if (data.Count() > 0)
        {
            return new Response() { Code = StatusCode.Error, Msg = "库位已经存在" };
        }
        else
        {
            await _rep.InsertAsync(entity);
        }
        return new Response() { Code = StatusCode.Success, Msg = "添加成功" };
    }

    /// <summary>
    /// 删除WMSLocation
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSLocationInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //删除
    }

    /// <summary>
    /// 更新WMSLocation
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response> Update(UpdateWMSLocationInput input)
    {
        var entity = input.Adapt<WMSLocation>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }

    ///// <summary>
    ///// 获取WMSLocation
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[HttpGet]
    //[ApiDescriptionSettings(Name = "Query")]
    //public async Task<WMSLocation> Get(long Id)
    //{
    //    return await _rep.GetFirstAsync(u => u.Id == Id);
    //}


    /// <summary>
    /// 获取WMSLocation 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSLocation> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSLocation列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSLocationOutput>> List([FromQuery] WMSLocationInput input)
    {
        return await _rep.AsQueryable().Select<WMSLocationOutput>().ToListAsync();
    }





}

