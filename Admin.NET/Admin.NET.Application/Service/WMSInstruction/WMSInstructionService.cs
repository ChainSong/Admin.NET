using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSInstruction服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSInstructionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSInstruction> _rep;
    private readonly UserManager _userManager;
    public WMSInstructionService(SqlSugarRepository<WMSInstruction> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询WMSInstruction
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSInstructionOutput>> Page(WMSInstructionInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.CustomerId>0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId>0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Order), u => u.OrderNumber.Contains(input.Order.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TableName), u => u.TableName.Contains(input.TableName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.InstructionType), u => u.InstructionType.Contains(input.InstructionType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BusinessType), u => u.BusinessType.Contains(input.BusinessType.Trim()))
                    .WhereIF(input.OperationId>0, u => u.OperationId == input.OperationId)
                    .WhereIF(input.InstructionStatus > 0, u => u.InstructionStatus == input.InstructionStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.InstructionTaskNo), u => u.InstructionTaskNo.Contains(input.InstructionTaskNo.Trim()))
                    .WhereIF(input.InstructionPriority>0, u => u.InstructionPriority == input.InstructionPriority)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Message), u => u.Message.Contains(input.Message.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                     //.Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSInstructionOutput>()
;
        if(input.CreationTime != null && input.CreationTime.Count >0)
        {
                DateTime? start= input.CreationTime[0]; 
                query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
                if (input.CreationTime.Count >1 && input.CreationTime[1].HasValue)
                {
                    var end = input.CreationTime[1].Value.AddDays(1);
                    query = query.Where(u => u.CreationTime < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSInstruction
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSInstructionInput input)
    {
        var entity = input.Adapt<WMSInstruction>();
        await _rep.InsertAsync(entity);
    }




    /// <summary>
    /// 删除WMSInstruction
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSInstructionInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSInstruction
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSInstructionInput input)
    {
        var entity = input.Adapt<WMSInstruction>();
        entity.InstructionStatus = 1;
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取WMSInstruction 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSInstruction> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSInstruction列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSInstructionOutput>> List([FromQuery] WMSInstructionInput input)
    {
        return await _rep.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    }





}

