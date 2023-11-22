using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// 仓库用户关系服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WarehouseUserMappingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WarehouseUserMapping> _rep;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;
    private readonly UserManager _userManager;
    public WarehouseUserMappingService(SqlSugarRepository<WarehouseUserMapping> rep, SqlSugarRepository<WMSWarehouse> repWarehouse, UserManager userManager)
    {
        _rep = rep;
        _repWarehouse = repWarehouse;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询仓库用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WarehouseUserMappingOutput>> Page(WarehouseUserMappingInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.UserId > 0, u => u.UserId == input.UserId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.Status > 0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)

                    .Select<WarehouseUserMappingOutput>()
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
    /// 增加仓库用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(List<AddWarehouseUserMappingInput> input)
    {

        var entity = input.Adapt<List<WarehouseUserMapping>>();
        entity.ForEach(a =>
        {
            a.Creator = _userManager.Account;
            a.CreationTime = DateTime.Now;
        });
        await _rep.DeleteAsync(a => a.UserId == input.First().UserId);
        await _rep.InsertRangeAsync(entity.Where(a => a.Status == 1).ToList());
    }

    /// <summary>
    /// 删除仓库用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWarehouseUserMappingInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新仓库用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWarehouseUserMappingInput input)
    {
        var entity = input.Adapt<WarehouseUserMapping>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取仓库用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WarehouseUserMapping> Get([FromQuery] QueryByIdWarehouseUserMappingInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取仓库用户关系列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WarehouseUserMappingOutput>> List(WarehouseUserMapping input)
    {
        return await _rep.AsQueryable().Where(a => a.UserId == input.UserId).Select<WarehouseUserMappingOutput>().ToListAsync();
    }




}

