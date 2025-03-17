using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// SysPresetQuery服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class SysPresetQueryService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysPresetQuery> _rep;
    private readonly UserManager _userManager;
    public SysPresetQueryService(SqlSugarRepository<SysPresetQuery> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询SysPresetQuery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<SysPresetQueryOutput>> Page(SysPresetQueryInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.QueryName), u => u.QueryName.Contains(input.QueryName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessName), u => u.BusinessName.Contains(input.BusinessName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.QueryForm), u => u.QueryForm.Contains(input.QueryForm.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))

                    .Select<SysPresetQueryOutput>()
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
    /// 增加SysPresetQuery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddSysPresetQueryInput input)
    {
        var entity = input.Adapt<SysPresetQuery>();
        entity.Creator = _userManager.Account;
        entity.CreationTime = DateTime.Now;
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除SysPresetQuery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteSysPresetQueryInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //var entity = input.Adapt<SysPresetQuery>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新SysPresetQuery
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateSysPresetQueryInput input)
    {
        var entity = input.Adapt<SysPresetQuery>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取SysPresetQuery 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<SysPresetQuery> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取SysPresetQuery列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<SysPresetQueryOutput>> List([FromQuery] SysPresetQueryInput input)
    {
        return await _rep.AsQueryable().Select<SysPresetQueryOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取SysPresetQuery列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryByUser")]
    public async Task<List<SysPresetQueryOutput>> QueryByUser([FromQuery] SysPresetQueryInput input)
    {
        return await _rep.AsQueryable().Select<SysPresetQueryOutput>()
            .Where(u => u.Creator == _userManager.Account)
            .Where(u => u.BusinessName == input.BusinessName)
            .ToListAsync();
    }



}

