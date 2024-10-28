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
public class WMSChartService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASN> _repASN;
    private readonly SqlSugarRepository<WMSPreOrder> _repPreOrder;
    //private readonly SqlSugarRepository<WMSStockCheck> _rep;
    //private readonly SqlSugarRepository<WMSStockCheckDetail> _repStockCheckDetail;
    //private readonly SqlSugarRepository<WMSStockCheckDetailScan> _repStockCheckDetailScan;
    //private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    //private readonly SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving;
    //private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly UserManager _userManager;
    public WMSChartService(SqlSugarRepository<WMSASN> repASN, SqlSugarRepository<WMSPreOrder> repPreOrder, UserManager userManager)
    {
        _repASN = repASN;
        _repPreOrder = repPreOrder;
        //_repInventoryUsable = repInventoryUsable;
        //_repReceiptReceiving = repReceiptReceiving;
        //_repStockCheckDetail = repStockCheckDetail;
        //_repStockCheckDetailScan = repStockCheckDetailScan;
        _userManager = userManager;

    }

    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "ASNStatusChart")]
    public async Task<dynamic> ASNStatusChart()
    {
        //_rep.Context.Ado.SqlQuery<WMSInstruction>(" ");
        var result = _repASN.AsQueryable().Where(a => a.ASNStatus != 99 && a.ASNStatus != -1)
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
             .GroupBy(r => r.ASNStatus)
             .Select(a => new { Status = a.ASNStatus, Count = SqlFunc.AggregateCount(a) }).ToList();
        return result;
    }


    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "ASNNumChart")]
    public async Task<dynamic> ASNNumChart()
    {
        //_rep.Context.Ado.SqlQuery<WMSInstruction>(" ");
        var result = _repASN.AsQueryable().Where(a => a.ASNStatus != -1 && a.CreationTime >= DateTime.Now.AddDays(-5))
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
               .GroupBy(r => new { r.ASNStatus, CreationTime = r.CreationTime.Value.ToString("yyyy-MM-dd") })
             .Select(a => new { Status = a.ASNStatus, CreationTime = a.CreationTime.Value.ToString("yyyy-MM-dd"), Count = SqlFunc.AggregateCount(a) }).ToList();
        return result;
    }

    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "PreOrderStatusChart")]
    public async Task<dynamic> PreOrderStatusChart()
    {
        //_rep.Context.Ado.SqlQuery<WMSInstruction>(" ");
        var result = _repPreOrder.AsQueryable().Where(a => a.PreOrderStatus != 99 && a.PreOrderStatus != -1)
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .GroupBy(r => r.PreOrderStatus)
            .Select(a => new { Status = a.PreOrderStatus, Count = SqlFunc.AggregateCount(a) }).ToList();
        return result;
    }

    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "PreOrderNumChart")]
    public async Task<dynamic> PreOrderNumChart()
    {
        //_rep.Context.Ado.SqlQuery<WMSInstruction>(" ");
        var result = _repPreOrder.AsQueryable().Where(a => a.PreOrderStatus != -1 && a.CreationTime >= DateTime.Now.AddDays(-5))
                .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .GroupBy(r => new { r.PreOrderStatus, CreationTime = r.CreationTime.Value.ToString("yyyy-MM-dd") })
            .Select(a => new { Status = a.PreOrderStatus, CreationTime = a.CreationTime.Value.ToString("yyyy-MM-dd"), Count = SqlFunc.AggregateCount(a) }).ToList();
        return result;
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
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteWMSStockCheckInput input)
    //{
    //    var entity = input.Adapt<WMSStockCheck>();
    //    await _rep.DeleteAsync(entity);   //假删除
    //}

    /// <summary>
    /// 更新WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Update")]
    //public async Task Update(UpdateWMSStockCheckInput input)
    //{
    //    var entity = input.Adapt<WMSStockCheck>();
    //    await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    //}




    /// <summary>
    /// 获取WMSStockCheck 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpGet]
    //[ApiDescriptionSettings(Name = "Query")]
    //public async Task<WMSStockCheck> Get(long id)
    //{
    //    var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
    //    return entity;
    //}

    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    ///// <returns></returns>
    //[HttpGet]
    //[ApiDescriptionSettings(Name = "List")]
    //public async Task<List<WMSStockCheckOutput>> List([FromQuery] WMSStockCheckInput input)
    //{
    //    return await _rep.AsQueryable().Select<WMSStockCheckOutput>().ToListAsync();
    //}
    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "getStockCheckInventory")]
    //public async Task<Response<List<WMSInventoryUsable>>> GetStockCheckInventory(WMSStockCheckInput input)
    //{
    //    //使用简单工厂定制化修改和新增的方法
    //    IStockCheckInterface factory = StockCheckFactory.GeInventoty(input.StockCheckType);
    //    factory._repInventoryUsable = _repInventoryUsable;
    //    var response = await factory.GetStockInfo(input);
    //    return response;
    //}

    /// <summary>
    /// 获取WMSStockCheck列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Add")]
    //public async Task<Response<List<WMSInventoryUsable>>> Add(WMSStockCheckInput input)
    //{

    //    IStockCheckInterface factory = StockCheckFactory.GeInventoty(input.StockCheckType);
    //    factory._repInventoryUsable = _repInventoryUsable;
    //    var response = await factory.GetStockInfo(input);
    //    input.Details = response.Data.Adapt<List<WMSStockCheckDetailDto>>();
    //    //使用简单工厂定制化修改和新增的方法
    //    IStockCheckAddOrUpdateInterface factoryAddOrUpdate = StockCheckAddOrUpdateFactory.AddOrUpdate(input.StockCheckType);
    //    factoryAddOrUpdate._repInventoryUsable = _repInventoryUsable;
    //    factoryAddOrUpdate._rep = _rep;
    //    factoryAddOrUpdate._repStockCheckDetail = _repStockCheckDetail;
    //    factoryAddOrUpdate._userManager = _userManager;
    //    factoryAddOrUpdate._repStockCheckDetailScan = _repStockCheckDetailScan;
    //    var responseAddOrUpdate = await factoryAddOrUpdate.AddOrUpdate(input);
    //    return responseAddOrUpdate;
    //}




}

