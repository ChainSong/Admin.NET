using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Magicodes.ExporterAndImporter.Excel;
using System.Collections.Generic;
using System.IO;

namespace Admin.NET.Application;
/// <summary>
/// WMS_RFPackageAcquisition服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFPackageAcquisitionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSRFPackageAcquisition> _rep;
    public WMSRFPackageAcquisitionService(SqlSugarRepository<WMSRFPackageAcquisition> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询WMS_RFPackageAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFPackageAcquisitionOutput>> Page(WMSRFPackageAcquisitionInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.Id>0, u => u.Id == input.Id)
                    .WhereIF(input.OrderId>0, u => u.OrderId == input.OrderId)
                    .WhereIF(input.PickTaskId>0, u => u.PickTaskId == input.PickTaskId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PackageNumber), u => u.PackageNumber.Contains(input.PackageNumber.Trim()))
                    .WhereIF(input.CustomerId>0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId>0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Lot), u => u.Lot.Contains(input.Lot.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SN), u => u.SN.Contains(input.SN.Trim()))
                    .WhereIF(input.ReceiptAcquisitionStatus>0, u => u.ReceiptAcquisitionStatus == input.ReceiptAcquisitionStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Type), u => u.Type.Contains(input.Type.Trim()))

                    .Select<WMSRFPackageAcquisitionOutput>()
;
        if(input.ProductionDateRange != null && input.ProductionDateRange.Count >0)
        {
                DateTime? start= input.ProductionDateRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
                if (input.ProductionDateRange.Count >1 && input.ProductionDateRange[1].HasValue)
                {
                    var end = input.ProductionDateRange[1].Value.AddDays(1);
                    query = query.Where(u => u.ProductionDate < end);
                }
        } 
        if(input.ExpirationDateRange != null && input.ExpirationDateRange.Count >0)
        {
                DateTime? start= input.ExpirationDateRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
                if (input.ExpirationDateRange.Count >1 && input.ExpirationDateRange[1].HasValue)
                {
                    var end = input.ExpirationDateRange[1].Value.AddDays(1);
                    query = query.Where(u => u.ExpirationDate < end);
                }
        } 
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
    /// 增加WMS_RFPackageAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSRFPackageAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFPackageAcquisition>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMS_RFPackageAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSRFPackageAcquisitionInput input)
    {
      var entity = input.Adapt<WMSRFPackageAcquisition>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMS_RFPackageAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSRFPackageAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFPackageAcquisition>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取WMS_RFPackageAcquisition 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSRFPackageAcquisition> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMS_RFPackageAcquisition列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSRFPackageAcquisitionOutput>> List([FromQuery] WMSRFPackageAcquisitionInput input)
    {
        return await _rep.AsQueryable().Select<WMSRFPackageAcquisitionOutput>().ToListAsync();
    }

    /// <summary>
    /// 导出WMS_RFPackageAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Export")]
    public async Task<FileStreamResult> Export(WMSRFPackageAcquisitionInput input)
    {
        var data = _rep.AsQueryable()
            .WhereIF(input.Id > 0, u => u.Id == input.Id)
            .WhereIF(input.OrderId > 0, u => u.OrderId == input.OrderId)
            .WhereIF(input.PickTaskId > 0, u => u.PickTaskId == input.PickTaskId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.PackageNumber), u => u.PackageNumber.Contains(input.PackageNumber.Trim()))
            .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
            .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Lot), u => u.Lot.Contains(input.Lot.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.SN), u => u.SN.Contains(input.SN.Trim()))
            .WhereIF(input.ReceiptAcquisitionStatus > 0, u => u.ReceiptAcquisitionStatus == input.ReceiptAcquisitionStatus)
            .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.Type), u => u.Type.Contains(input.Type.Trim()))
            .Select<WMSRFPackageAcquisitionOutput>();

        if (input.ProductionDateRange != null && input.ProductionDateRange.Count > 0)
        {
            DateTime? start = input.ProductionDateRange[0];
            data = data.WhereIF(start.HasValue, u => u.ProductionDate > start);
            if (input.ProductionDateRange.Count > 1 && input.ProductionDateRange[1].HasValue)
            {
                var end = input.ProductionDateRange[1].Value.AddDays(1);
                data = data.Where(u => u.ProductionDate < end);
            }
        }
        if (input.ExpirationDateRange != null && input.ExpirationDateRange.Count > 0)
        {
            DateTime? start = input.ExpirationDateRange[0];
            data = data.WhereIF(start.HasValue, u => u.ExpirationDate > start);
            if (input.ExpirationDateRange.Count > 1 && input.ExpirationDateRange[1].HasValue)
            {
                var end = input.ExpirationDateRange[1].Value.AddDays(1);
                data = data.Where(u => u.ExpirationDate < end);
            }
        }
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            data = data.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                data = data.Where(u => u.CreationTime < end);
            }
        }

        var dataList = await data.OrderByDescending(u => u.Id).ToListAsync();
        var exporter = new ExcelExporter();
        var result = await exporter.ExportAsByteArray(dataList);
        var fs = new MemoryStream(result);
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "RF包装采集_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"
        };
    }





}

