using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.Collections.Generic;
using System.Data;
using System.IO;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// MMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class MMSReceiptService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<MMSReceipt> _rep;
    private readonly SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail;
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<MMSSupplier> _repSupplier;
    private readonly SqlSugarRepository<SupplierUserMapping> _repSupplierUser;


    private readonly SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail;
    private readonly SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable;

    public MMSReceiptService(SqlSugarRepository<MMSReceipt> rep, SqlSugarRepository<MMSReceiptDetail> repMReceiptDetail, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<MMSSupplier> repSupplier, SqlSugarRepository<SupplierUserMapping> repSupplierUser, SqlSugarRepository<MMSReceiptReceivingDetail> repMReceiptReceivingDetail, SqlSugarRepository<MMSReceiptReceiving> repMReceiptReceiving, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<MMSInventoryUsable> repInventoryUsable)
    {
        _rep = rep;
        _repMReceiptDetail = repMReceiptDetail;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repSupplier = repSupplier;
        _repSupplierUser = repSupplierUser;
        _repMReceiptReceivingDetail = repMReceiptReceivingDetail;
        _repMReceiptReceiving = repMReceiptReceiving;
        _repWarehouseUser = repWarehouseUser;
        _repInventoryUsable = repInventoryUsable;
    }

    /// <summary>
    /// 分页查询MMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<MMSReceiptOutput>> Page(MMSReceiptInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PurchaseOrderNumber), u => u.PurchaseOrderNumber.Contains(input.PurchaseOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ReceiptStatus > 0, u => u.ReceiptStatus == input.ReceiptStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptType), u => u.ReceiptType.Contains(input.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Contact), u => u.Contact.Contains(input.Contact.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ContactInfo), u => u.ContactInfo.Contains(input.ContactInfo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
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

                    .Select<MMSReceiptOutput>()
;
        if (input.ReceiptTimeRange != null && input.ReceiptTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime > start);
            if (input.ReceiptTimeRange.Count > 1 && input.ReceiptTimeRange[1].HasValue)
            {
                var end = input.ReceiptTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
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
    /// 增加MMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response<List<OrderStatusDto>>> Add(AddOrUpdateMMSReceiptInput input)
    {
        //Response<List<OrderStatusDto>> response= new Response<List<OrderStatusDto>>();
        var entity = input.Adapt<WMSASN>();
        List<AddOrUpdateMMSReceiptInput> entityListDtos = new List<AddOrUpdateMMSReceiptInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IMReceiptInterface factory = MReceiptFactory.AddOrUpdate(input.SupplierId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repMReceipt = _rep;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceiving = _repMReceiptReceiving;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repSupplier = _repSupplier;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repInventoryUsable = _repInventoryUsable;
        //factory._repASNDetail = _repASNDetail;
        //factory._repCustomerUser = _repCustomerUser;
        //factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.AddStrategy(entityListDtos);
        //var entity = input.Adapt<MMSMaterial>();
        //await _rep.InsertAsync(entity); 
    }

    /// <summary>
    /// 删除MMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteMMSReceiptInput input)
    {
        //var entity = input.Adapt<MMSReceipt>();
        //await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新MMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response<List<OrderStatusDto>>> Update(AddOrUpdateMMSReceiptInput input)
    {
        //Response<List<OrderStatusDto>> response= new Response<List<OrderStatusDto>>();
        //var entity = input.Adapt<WMSASN>();
        List<AddOrUpdateMMSReceiptInput> entityListDtos = new List<AddOrUpdateMMSReceiptInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IMReceiptInterface factory = MReceiptFactory.AddOrUpdate(input.SupplierId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repMReceipt = _rep;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceiving = _repMReceiptReceiving;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repSupplier = _repSupplier;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repInventoryUsable = _repInventoryUsable;

        //factory._repASNDetail = _repASNDetail;
        //factory._repCustomerUser = _repCustomerUser;
        //factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.AddStrategy(entityListDtos);
        //var entity = input.Adapt<MMSMaterial>();
        //await _rep.InsertAsync(entity); 
    }




    /// <summary>
    /// 获取MMSReceipt 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<MMSReceipt> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取MMSReceipt列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<MMSReceiptOutput>> List([FromQuery] MMSReceiptInput input)
    {
        return await _rep.AsQueryable().Select<MMSReceiptOutput>().ToListAsync();
    }

    [HttpPost]
    public ActionResult ExportReceipt(List<long> input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐
        IMReceiptExcelInterface factory = MReceiptExcelFactory.Export();

        factory._userManager = _userManager;
        factory._repMReceipt = _rep;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceiving = _repMReceiptReceiving;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repSupplier = _repSupplier;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        var response = factory.ExportReceipt(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "入库单.xlsx" // 配置文件下载显示名
        };
    }


    [HttpPost]
    public ActionResult ExportReceiptReceiving(List<long> input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐
        IMReceiptExcelInterface factory = MReceiptExcelFactory.Export();

        factory._userManager = _userManager;
        factory._repMReceipt = _rep;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceiving = _repMReceiptReceiving;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repSupplier = _repSupplier;
        factory._repTableColumns= _repTableColumns;
        factory._repTableColumnsDetail= _repTableColumnsDetail;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repInventoryUsable = _repInventoryUsable;
        var response = factory.ExportReceiptReceiving(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "上架单模板.xlsx" // 配置文件下载显示名
        };
    }


}

