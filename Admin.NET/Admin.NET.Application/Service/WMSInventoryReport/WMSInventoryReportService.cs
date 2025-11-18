using Admin.NET.Application.Const;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Application.Service.WMSInventoryReport.Dto;
using Admin.NET.Application.Service.WMSInventoryReport.Factory;
using Admin.NET.Application.Service.WMSInventoryReport.Interface;
using Admin.NET.Common.枚举值帮助类;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Admin.NET.Application;
/// <summary>
/// WMSInstruction服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSInventoryReportService : IDynamicApiController, ITransient
{
    //private readonly SqlSugarRepository<WMSInstruction> _rep;

    private readonly UserManager _userManager;



    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;


    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    //private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    public WMSInventoryReportService(UserManager userManager, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WMSInventoryUsable> repInventoryUsable, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<TableColumns> repTableColumns)
    {

        _userManager = userManager;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
        _repInventoryUsable = repInventoryUsable;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repTableColumns = repTableColumns;
    }


    /// <summary>
    /// InvrntoryDataPage
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "InvrntoryDataPage")]
    public async Task<SqlSugarPagedList<WMSInventoryUsableReport>> InvrntoryDataPage(WMSInventoryUsableInput input)
    {
        if (_userManager == null)
        {
            input.CustomerId = -1;
        }
        //使用简单工厂定制化  / 
        IInvrntoryInterface factory = InvrntoryFactory.InvrntoryData(input.CustomerId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repWarehouseUser = _repWarehouseUser;

        var response = await factory.InvrntoryDataPage(input);
        return response;

        //return await _repInventoryUsable.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    }



    /// <summary>
    /// InvrntoryData
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "InvrntoryData")]
    public async Task<SqlSugarPagedList<WMSInventoryUsableOutput>> InvrntoryData(WMSInventoryUsableInput input)
    {

        //使用简单工厂定制化  / 
        IInvrntoryInterface factory = InvrntoryFactory.InvrntoryData(input.CustomerId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repWarehouseUser = _repWarehouseUser;
        var response = await factory.InvrntoryData(input);
        return response;

        //return await _repInventoryUsable.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    }



    /// <summary>
    /// InvrntoryData
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "InvrntoryDataExport")]
    public ActionResult InvrntoryDataExport(WMSInventoryUsableInput input)
    {

        //使用简单工厂定制化  / 
        IInvrntoryInterface factory = InvrntoryFactory.InvrntoryData(input.CustomerId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        var response = factory.InvrntoryDataExport(input);
        //var response = factory.Strategy(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        //return new FileStreamResult(fs, "application/octet-stream")
        //{
        //    FileDownloadName = "库存报表" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx" // 配置文件下载显示名
        //};
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "库存报表.xlsx" // 配置文件下载显示名
        };
        //return await _repInventoryUsable.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    }



    /// <summary>
    /// InvrntoryData
    /// </summary>
    ///// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "InvrntoryDataExport")]
    //public async Task<List<WMSInventoryUsable>> InvrntoryDataReport(WMSInventoryUsableInput input)
    //{

        //return null;
        //return Json(new { Code = "" });
        //使用简单工厂定制化  / 
        //IInvrntoryInterface factory = InvrntoryFactory.InvrntoryData(input.CustomerId);
        ////factory._db = _db;
        //factory._userManager = _userManager;
        //factory._repCustomerUser = _repCustomerUser;
        //factory._repInventoryUsable = _repInventoryUsable;
        //factory._repWarehouseUser = _repWarehouseUser;
        //factory._repTableColumns = _repTableColumns;
        //factory._repTableColumnsDetail = _repTableColumnsDetail;
        //var response = factory.InvrntoryDataExport(input);
        ////var response = factory.Strategy(input);
        //IExporter exporter = new ExcelExporter();
        //var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        //var fs = new MemoryStream(result.Result);
        ////return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        //return new FileStreamResult(fs, "application/octet-stream")
        //{
        //    FileDownloadName = "库存报表" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx" // 配置文件下载显示名
        //};

        //return await _repInventoryUsable.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    //}



    /// <summary>
    /// 可用库存汇总报表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "AvailableInventorySummaryReport")]
    public async Task<SqlSugarPagedList<WMSInventoryUsable>> AvailableInventorySummaryReport(WMSInventoryUsableInput input)
    {
        var query = _repInventoryUsable.AsQueryable()
                   .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                   .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                   .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                   .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                   .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                   .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                   .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                   .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                   .WhereIF(input.InventoryStatus != 0, u => u.InventoryStatus == input.InventoryStatus)
                   .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                   .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                   .GroupBy(a => new { a.CustomerId, a.CustomerName, a.WarehouseId, a.WarehouseName, a.SKU,a.GoodsName })
                   .Select(a => new WMSInventoryUsable
                   {
                       CustomerName = a.CustomerName,
                       WarehouseName = a.WarehouseName,
                       GoodsName = a.GoodsName,
                       SKU = a.SKU, 
                       Qty = SqlFunc.AggregateSum(a.Qty) 
                   });
        query = query.OrderBuilder(input,"", "SKU");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// AvailableInventorySummaryReportExport
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "AvailableInventorySummaryReportExport")]
    public async Task<ActionResult> AvailableInventorySummaryReportExport(WMSInventoryUsableInput input)
    {
        try
        {
            var query = await _repInventoryUsable.AsQueryable()
                     .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                     .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                     .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                     .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                     .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                     .WhereIF(input.InventoryStatus != 0, u => u.InventoryStatus == input.InventoryStatus)
                     .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                     .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                     .GroupBy(a => new
                     {
                         a.CustomerId,
                         a.CustomerName,
                         a.WarehouseId,
                         a.WarehouseName,
                         a.SKU,
                         a.GoodsName,
                         a.Area,
                         a.Location,
                         a.PoCode,
                         a.Onwer,
                         a.CreationTime,
                         a.BatchCode,
                         a.GoodsType,
                         a.InventoryStatus
                     })
                     .Select(a => new WMSInventoryUsableExport
                     {
                         CustomerName = a.CustomerName,
                         WarehouseName = a.WarehouseName,
                         AreaName = a.Area,
                         LocationName = a.Location,
                         PoCode = a.PoCode,
                         SKU = a.SKU,
                         Qty = SqlFunc.AggregateSum(a.Qty),
                         Onwer = a.Onwer,
                         CreateTime = a.CreationTime.ToString(),
                         BatchNumber = a.BatchCode,
                         GoodsType = a.GoodsType,
                         InventoryStatus = a.InventoryStatus.ToString(),
                     })
                     .OrderBuilder(input, "", "SKU")
                     .ToListAsync();

            if (query == null || query.Count == 0)
            {
                throw Oops.Oh("未查询到可用库存");
            }

            query.ForEach(x =>
            {
                if (int.TryParse(x.InventoryStatus, out var status))
                {
                    x.InventoryStatus = EnumHelper.GetEnumName<InventoryStatusEnum>(status);
                }
            });
            IExporter exporter = new ExcelExporter();
            var result = exporter.ExportAsByteArray<WMSInventoryUsableExport>(query);
            var fs = new MemoryStream(result.Result);
            return new FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = "库存报表.xlsx" // 配置文件下载显示名
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}

