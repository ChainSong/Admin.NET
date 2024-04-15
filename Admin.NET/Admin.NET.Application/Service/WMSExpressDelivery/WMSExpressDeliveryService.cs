using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSExpressDelivery服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSExpressDeliveryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSExpressDelivery> _rep;
    public WMSExpressDeliveryService(SqlSugarRepository<WMSExpressDelivery> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询WMSExpressDelivery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSExpressDeliveryOutput>> Page(WMSExpressDeliveryInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrEmpty(input.PackageNumber), u => u.PackageNumber == input.PackageNumber)
                    .WhereIF(!string.IsNullOrEmpty(input.ExpressNumber), u => u.ExpressNumber == input.ExternOrderNumber)
                    .Select<WMSExpressDeliveryOutput>();
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSExpressDelivery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSExpressDeliveryInput input)
    {
        var entity = input.Adapt<WMSExpressDelivery>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSExpressDelivery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSExpressDeliveryInput input)
    {
        var entity = input.Adapt<WMSExpressDelivery>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSExpressDelivery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSExpressDeliveryInput input)
    {
        var entity = input.Adapt<WMSExpressDelivery>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSExpressDelivery 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSExpressDelivery> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSExpressDelivery列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSExpressDeliveryOutput>> List([FromQuery] WMSExpressDeliveryInput input)
    {
        return await _rep.AsQueryable().Select<WMSExpressDeliveryOutput>().ToListAsync();
    }





}

