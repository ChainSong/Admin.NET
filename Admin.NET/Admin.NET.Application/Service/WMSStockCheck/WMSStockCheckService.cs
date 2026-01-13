using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSStockCheck服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSStockCheckService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSStockCheck> _rep;
    private readonly SqlSugarRepository<WMSStockCheckDetail> _repStockCheckDetail;
    private readonly SqlSugarRepository<WMSStockCheckDetailScan> _repStockCheckDetailScan;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    private readonly SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly UserManager _userManager;
    public WMSStockCheckService(SqlSugarRepository<WMSStockCheck> rep, SqlSugarRepository<WMSInventoryUsable> repInventoryUsable, SqlSugarRepository<WMSReceiptReceiving> repReceiptReceiving, SqlSugarRepository<WMSStockCheckDetail> repStockCheckDetail, SqlSugarRepository<WMSStockCheckDetailScan> repStockCheckDetailScan, UserManager userManager)
    {
        _rep = rep;
        _repInventoryUsable = repInventoryUsable;
        _repReceiptReceiving = repReceiptReceiving;
        _repStockCheckDetail = repStockCheckDetail;
        _repStockCheckDetailScan = repStockCheckDetailScan;
        _userManager = userManager;

    }

    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSStockCheckOutput>> Page(WMSStockCheckInput input)
    {
        var query = _rep.AsQueryable()
                    //.WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.StockCheckNumber), u => u.StockCheckNumber.Contains(input.StockCheckNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternNumber), u => u.ExternNumber.Contains(input.ExternNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseId), u => u.WarehouseId.Contains(input.WarehouseId.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.StockCheckType), u => u.StockCheckType.Contains(input.StockCheckType.Trim()))
                    //.WhereIF((input.StockCheckStatus != 0 || input.StockCheckStatus !=null), u => u.StockCheckStatus == input.StockCheckStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ToCheckUser), u => u.ToCheckUser.Contains(input.ToCheckUser.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ToCheckAccount), u => u.ToCheckAccount.Contains(input.ToCheckAccount.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Is_Difference), u => u.Is_Difference.Contains(input.Is_Difference.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Is_Deal), u => u.Is_Deal.Contains(input.Is_Deal.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str6), u => u.Str6.Contains(input.Str6.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str7), u => u.Str7.Contains(input.Str7.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str8), u => u.Str8.Contains(input.Str8.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str9), u => u.Str9.Contains(input.Str9.Trim()))
                     //.WhereIF(!string.IsNullOrWhiteSpace(input.Str10), u => u.Str10.Contains(input.Str10.Trim()))
                     //.WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                     //.WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                     //.WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)
                     //.WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                     //.WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)
                     .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSStockCheckOutput>();
;
        if (input.StockCheckDateRange != null && input.StockCheckDateRange.Count > 0)
        {
            DateTime? start = input.StockCheckDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.StockCheckDate > start);
            if (input.StockCheckDateRange.Count > 1 && input.StockCheckDateRange[1].HasValue)
            {
                var end = input.StockCheckDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.StockCheckDate < end);
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

    ///// <summary>
    ///// 增加WMSStockCheck
    ///// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Add")]
    //public async Task Add(AddWMSStockCheckInput input)
    //{
    //    var entity = input.Adapt<WMSStockCheck>();
    //    await _rep.InsertAsync(entity);
    //}

    /// <summary>
    /// 删除WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSStockCheckInput input)
    {
        var entity = input.Adapt<WMSStockCheck>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSStockCheckInput input)
    {
        var entity = input.Adapt<WMSStockCheck>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSStockCheck 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSStockCheck> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSStockCheckOutput>> List([FromQuery] WMSStockCheckInput input)
    {
        return await _rep.AsQueryable().Select<WMSStockCheckOutput>().ToListAsync();
    }
    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "getStockCheckInventory")]
    public async Task<Response<List<WMSInventoryUsable>>> GetStockCheckInventory(WMSStockCheckInput input)
    {
        //使用简单工厂定制化修改和新增的方法
        IStockCheckInterface factory = StockCheckFactory.GeInventoty(input.StockCheckType);
        factory._repInventoryUsable = _repInventoryUsable;
        var response = await factory.GetStockInfo(input);
        return response;
    }

    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response<List<WMSInventoryUsable>>> Add(WMSStockCheckInput input)
    {

        IStockCheckInterface factory = StockCheckFactory.GeInventoty(input.StockCheckType);
        factory._repInventoryUsable = _repInventoryUsable;
        var response = await factory.GetStockInfo(input);
        input.Details = response.Data.Adapt<List<WMSStockCheckDetailDto>>();
        //使用简单工厂定制化修改和新增的方法
        IStockCheckAddOrUpdateInterface factoryAddOrUpdate = StockCheckAddOrUpdateFactory.AddOrUpdate(input.StockCheckType);
        factoryAddOrUpdate._repInventoryUsable = _repInventoryUsable;
        factoryAddOrUpdate._rep = _rep;
        factoryAddOrUpdate._repStockCheckDetail = _repStockCheckDetail;
        factoryAddOrUpdate._userManager = _userManager;
        factoryAddOrUpdate._repStockCheckDetailScan = _repStockCheckDetailScan;
        var responseAddOrUpdate = await factoryAddOrUpdate.AddOrUpdate(input);
        return responseAddOrUpdate;
    }




}

