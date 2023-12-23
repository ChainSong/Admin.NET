using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// SupplierUserMapping服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class SupplierUserMappingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SupplierUserMapping> _rep;
    private readonly UserManager _userManager;
    public SupplierUserMappingService(SqlSugarRepository<SupplierUserMapping> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;

    }

    /// <summary>
    /// 分页查询SupplierUserMapping
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<SupplierUserMappingOutput>> Page(SupplierUserMappingInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.UserId > 0, u => u.UserId == input.UserId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName.Trim()))
                    .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
                    .WhereIF(input.Status > 0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<SupplierUserMappingOutput>()
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
    /// 增加SupplierUserMapping
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    //public async Task Add(AddSupplierUserMappingInput input)
    public async Task Add(List<AddSupplierUserMappingInput> input)
    {

        var entity = input.Adapt<List<SupplierUserMapping>>();
        entity.ForEach(a =>
        {
            a.Creator = _userManager.Account;
            a.CreationTime = DateTime.Now;
        });
        await _rep.DeleteAsync(a => a.UserId == input.First().UserId);
        await _rep.InsertRangeAsync(entity.Where(a => a.Status == 1).ToList());
        //var entity = input.Adapt<SupplierUserMapping>();
        //await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除SupplierUserMapping
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteSupplierUserMappingInput input)
    {
        var entity = input.Adapt<SupplierUserMapping>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新SupplierUserMapping
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateSupplierUserMappingInput input)
    {
        var entity = input.Adapt<SupplierUserMapping>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }





    /// <summary>
    /// 获取SupplierUserMapping 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<SupplierUserMapping> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取SupplierUserMapping列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<SupplierUserMappingOutput>> List( SupplierUserMapping input)
    {
        return await _rep.AsQueryable().Where(a => a.UserId == input.UserId).Select<SupplierUserMappingOutput>().ToListAsync();

        //return await _rep.AsQueryable().Select<SupplierUserMappingOutput>().ToListAsync();
    }
 






}

