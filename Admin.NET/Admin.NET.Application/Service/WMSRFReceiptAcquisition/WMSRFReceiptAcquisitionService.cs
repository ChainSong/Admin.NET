using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Magicodes.ExporterAndImporter.Excel;
using System.Collections.Generic;
using System.IO;

namespace Admin.NET.Application;
/// <summary>
/// WMS_RFReceiptAcquisition服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFReceiptAcquisitionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSRFReceiptAcquisition> _rep;
    public WMSRFReceiptAcquisitionService(SqlSugarRepository<WMSRFReceiptAcquisition> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询WMS_RFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFReceiptAcquisitionOutput>> Page(WMSRFReceiptAcquisitionInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.ASNId>0, u => u.ASNId == input.ASNId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(input.ReceiptDetailId>0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
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

                    .Select<WMSRFReceiptAcquisitionOutput>()
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
    /// 增加WMS_RFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSRFReceiptAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFReceiptAcquisition>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMS_RFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSRFReceiptAcquisitionInput input)
    {
        //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
      var entity = input.Adapt<WMSRFReceiptAcquisition>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMS_RFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSRFReceiptAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFReceiptAcquisition>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取WMS_RFReceiptAcquisition 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSRFReceiptAcquisition> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMS_RFReceiptAcquisition列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSRFReceiptAcquisitionOutput>> List([FromQuery] WMSRFReceiptAcquisitionInput input)
    {
        return await _rep.AsQueryable().Select<WMSRFReceiptAcquisitionOutput>().ToListAsync();
    }

    /// <summary>
    /// 导出WMS_RFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Export")]
    public async Task<FileStreamResult> Export(WMSRFReceiptAcquisitionInput input)
    {
        var data = _rep.AsQueryable()
            .WhereIF(input.ASNId > 0, u => u.ASNId == input.ASNId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
            .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
            .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
            .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
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
            .Select<WMSRFReceiptAcquisitionOutput>();

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
            FileDownloadName = "RF入库采集_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx"
        };
    }





}

