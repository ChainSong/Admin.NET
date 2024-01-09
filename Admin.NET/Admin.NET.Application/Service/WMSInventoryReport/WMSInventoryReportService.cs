using Admin.NET.Application.Const;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service.WMSInventoryReport.Dto;
using Admin.NET.Application.Service.WMSInventoryReport.Factory;
using Admin.NET.Application.Service.WMSInventoryReport.Interface;
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
    [ApiDescriptionSettings(Name = "InvrntoryDataPage")]
    public async Task<SqlSugarPagedList<WMSInventoryUsableReport>> InvrntoryDataPage(WMSInventoryUsableInput input)
    {

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
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "库存报表" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx" // 配置文件下载显示名
        };

        //return await _repInventoryUsable.AsQueryable().Select<WMSInstructionOutput>().ToListAsync();
    }



    /// <summary>
    /// InvrntoryData
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "InvrntoryDataExport")]
    public async Task<List<WMSInventoryUsable>>   InvrntoryDataReport(WMSInventoryUsableInput input)
    {

        return null;
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
    }

}

