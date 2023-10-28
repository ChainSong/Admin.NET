using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Admin.NET.Application;
/// <summary>
/// WMSOrder服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSOrder> _rep;
    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;

    private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable;
    private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
  
    private readonly SqlSugarRepository<WMSInstruction> _repInstruction;
    private readonly SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation;

    public WMSOrderService(SqlSugarRepository<WMSOrder> rep, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSReceipt> repReceipt, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<WMSInventoryUsable> repTableInventoryUsable, ISqlSugarClient db, UserManager userManager, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed, SqlSugarRepository<WMSInstruction> repInstruction, SqlSugarRepository<WMSOrderAllocation> repOrderAllocation)
    {
        _rep = rep;
        _repOrderDetail = repOrderDetail;
        _repReceipt = repReceipt;
        _repReceiptDetail = repReceiptDetail;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repTableInventoryUsable = repTableInventoryUsable;
        _db = db;
        _userManager = userManager;
        _repTableInventoryUsed = repTableInventoryUsed;
        _repInstruction = repInstruction;
        _repOrderAllocation = repOrderAllocation;
    }

    /// <summary>
    /// 分页查询WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSOrderOutput>> Page(WMSOrderInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.PreOrderId > 0, u => u.PreOrderId == input.PreOrderId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderType), u => u.OrderType.Contains(input.OrderType.Trim()))
                    .WhereIF(input.OrderStatus > 0, u => u.OrderStatus == input.OrderStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
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
                    .WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                    .WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)

                    .Select<WMSOrderOutput>()
;
        if (input.OrderTimeRange != null && input.OrderTimeRange.Count > 0)
        {
            DateTime? start = input.OrderTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.OrderTime > start);
            if (input.OrderTimeRange.Count > 1 && input.OrderTimeRange[1].HasValue)
            {
                var end = input.OrderTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OrderTime < end);
            }
        }
        if (input.CompleteTimeRange != null && input.CompleteTimeRange.Count > 0)
        {
            DateTime? start = input.CompleteTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CompleteTime > start);
            if (input.CompleteTimeRange.Count > 1 && input.CompleteTimeRange[1].HasValue)
            {
                var end = input.CompleteTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CompleteTime < end);
            }
        }
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
        if (input.DateTime1Range != null && input.DateTime1Range.Count > 0)
        {
            DateTime? start = input.DateTime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1Range.Count > 1 && input.DateTime1Range[1].HasValue)
            {
                var end = input.DateTime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2Range != null && input.DateTime2Range.Count > 0)
        {
            DateTime? start = input.DateTime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2Range.Count > 1 && input.DateTime2Range[1].HasValue)
            {
                var end = input.DateTime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3Range != null && input.DateTime3Range.Count > 0)
        {
            DateTime? start = input.DateTime3Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
            if (input.DateTime3Range.Count > 1 && input.DateTime3Range[1].HasValue)
            {
                var end = input.DateTime3Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4Range != null && input.DateTime4Range.Count > 0)
        {
            DateTime? start = input.DateTime4Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
            if (input.DateTime4Range.Count > 1 && input.DateTime4Range[1].HasValue)
            {
                var end = input.DateTime4Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5Range != null && input.DateTime5Range.Count > 0)
        {
            DateTime? start = input.DateTime5Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
            if (input.DateTime5Range.Count > 1 && input.DateTime5Range[1].HasValue)
            {
                var end = input.DateTime5Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSOrderInput input)
    {
        var entity = input.Adapt<WMSOrder>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSOrderInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSOrderInput input)
    {
        var entity = input.Adapt<WMSOrder>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSOrder 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSOrder> Get(long id)
    {
        var entity = await _rep.AsQueryable()
           .Includes(a => a.Details)
           .Includes(a => a.OrderAddress)
           .Includes(a => a.Allocation)
           .Where(u => u.Id == id).FirstAsync();
        return entity;
        //var entity = await _rep.AsQueryable()
        //    .Includes(a => a.Details)
        //    .Includes(a => a.OrderAddress)
        //    .Includes(a => a.Allocation)
        //    .Where(u => u.Id == id).FirstAsync();
        //return entity;
    }

    /// <summary>
    /// 获取WMSOrder列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSOrderOutput>> List([FromQuery] WMSOrderInput input)
    {
        return await _rep.AsQueryable().Select<WMSOrderOutput>().ToListAsync();
    }



    [HttpPost]
    public async Task<Response<List<OrderStatusDto>>> AutomatedAllocation(List<long> input)
    {
        //使用简单工厂定制化  / 

        IAutomatedAllocationInterface factory = AutomatedAllocationFactory.AutomatedAllocation();


        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        var response = await factory.Strategy(input);

        return response;

    }
     
}

