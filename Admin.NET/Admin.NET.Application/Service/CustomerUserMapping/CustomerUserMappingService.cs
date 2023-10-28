using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// 客户用户关系服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class CustomerUserMappingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<CustomerUserMapping> _rep;
    private readonly UserManager _userManager;
    public CustomerUserMappingService(SqlSugarRepository<CustomerUserMapping> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询客户用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<CustomerUserMappingOutput>> Page(CustomerUserMappingInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.UserId>0, u => u.UserId == input.UserId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UserName), u => u.UserName.Contains(input.UserName.Trim()))
                    .WhereIF(input.CustomerId>0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.Status>0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<CustomerUserMappingOutput>()
;
        if(input.CreationTimeRange != null && input.CreationTimeRange.Count >0)
        {
                DateTime? start= input.CreationTimeRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
                if (input.CreationTimeRange.Count >1 && input.CreationTimeRange[1].HasValue)
                {
                    var end = input.CreationTimeRange[1].Value.AddDays(1);
                    query = query.Where(u => u.CreationTime < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加客户用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(List<AddCustomerUserMappingInput> input)
    {
        //var entity = input.Adapt<CustomerUserMapping>();
        //await _rep.InsertAsync(entity);

        var entity = input.Adapt<List<CustomerUserMapping>>();
        entity.ForEach(a =>
        {
            a.Creator = _userManager.Account;
            a.CreationTime = DateTime.Now;
        });
        await _rep.DeleteAsync(a => a.UserId == input.First().UserId);
        await _rep.InsertRangeAsync(entity.Where(a => a.Status == 1).ToList());
    }

    /// <summary>
    /// 删除客户用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteCustomerUserMappingInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新客户用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateCustomerUserMappingInput input)
    {
        var entity = input.Adapt<CustomerUserMapping>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取客户用户关系
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<CustomerUserMapping> Get([FromQuery] QueryByIdCustomerUserMappingInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取客户用户关系列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<CustomerUserMappingOutput>> List([FromQuery] CustomerUserMappingInput input)
    {
        return await _rep.AsQueryable().Select<CustomerUserMappingOutput>().ToListAsync();
    }





}

