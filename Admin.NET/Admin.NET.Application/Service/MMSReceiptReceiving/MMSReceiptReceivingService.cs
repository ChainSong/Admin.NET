using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using Admin.NET.Application.Service.Factory;
using System.Linq;
using Admin.NET.Applicationt.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.ReceiptCore.Factory;
using Admin.NET.Application.ReceiptCore.Interface;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.IO;
using Admin.NET.Application.ReceiptReceivingCore.Factory;

namespace Admin.NET.Application;
/// <summary>
/// MMSReceiptReceiving服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class MMSReceiptReceivingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<MMSReceiptReceiving> _rep;
    private readonly SqlSugarRepository<MMSReceipt> _repMReceipt;
    private readonly SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<MMSSupplier> _repSupplier;
    private readonly SqlSugarRepository<SupplierUserMapping> _repSupplierUser;
    private readonly SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving;
    private readonly SqlSugarRepository<WMSLocation> _repLocation;
    private readonly SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable;
    public MMSReceiptReceivingService(SqlSugarRepository<MMSReceiptReceiving> rep, SqlSugarRepository<MMSReceipt> repMReceipt, SqlSugarRepository<MMSReceiptDetail> repMReceiptDetail
, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<MMSSupplier> repSupplier
, SqlSugarRepository<SupplierUserMapping> repSupplierUser
, SqlSugarRepository<MMSReceiptReceivingDetail> repMReceiptReceivingDetail, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<MMSReceiptReceiving> repMReceiptReceiving
         , SqlSugarRepository<WMSLocation> repLocation  , SqlSugarRepository<MMSInventoryUsable> repInventoryUsable
        )
    {
        _rep = rep;
        _repMReceipt = repMReceipt;
        _repMReceiptDetail = repMReceiptDetail;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repSupplier = repSupplier;
        _repSupplierUser = repSupplierUser;
        _repMReceiptReceivingDetail = repMReceiptReceivingDetail;
        _repWarehouseUser = repWarehouseUser;
        _repMReceiptReceiving = repMReceiptReceiving;
        _repLocation = repLocation;
        _repInventoryUsable= repInventoryUsable;
    }

    /// <summary>
    /// 分页查询MMSReceiptReceiving
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<MMSReceiptReceivingOutput>> Page(MMSReceiptReceivingInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.ReceiptId > 0, u => u.ReceiptId == input.ReceiptId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptReceivingNumber), u => u.ReceiptReceivingNumber.Contains(input.ReceiptReceivingNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PurchaseOrderNumber), u => u.PurchaseOrderNumber.Contains(input.PurchaseOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ReceiptReceivingStatus > 0, u => u.ReceiptReceivingStatus == input.ReceiptReceivingStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptReceivingType), u => u.ReceiptReceivingType.Contains(input.ReceiptReceivingType.Trim()))
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

                    .Select<MMSReceiptReceivingOutput>()
;
        if (input.ReceiptReceivingStartTimeRange != null && input.ReceiptReceivingStartTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptReceivingStartTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptReceivingStartTime > start);
            if (input.ReceiptReceivingStartTimeRange.Count > 1 && input.ReceiptReceivingStartTimeRange[1].HasValue)
            {
                var end = input.ReceiptReceivingStartTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptReceivingStartTime < end);
            }
        }
        if (input.ReceiptReceivingEndTimeRange != null && input.ReceiptReceivingEndTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptReceivingEndTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptReceivingEndTime > start);
            if (input.ReceiptReceivingEndTimeRange.Count > 1 && input.ReceiptReceivingEndTimeRange[1].HasValue)
            {
                var end = input.ReceiptReceivingEndTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptReceivingEndTime < end);
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
    /// 增加MMSReceiptReceiving
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddMMSReceiptReceivingInput input)
    {
        var entity = input.Adapt<MMSReceiptReceiving>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除MMSReceiptReceiving
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteMMSReceiptReceivingInput input)
    {
        var entity = input.Adapt<MMSReceiptReceiving>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新MMSReceiptReceiving
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateMMSReceiptReceivingInput input)
    {
        var entity = input.Adapt<MMSReceiptReceiving>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取MMSReceiptReceiving 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<MMSReceiptReceiving> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a=>a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取MMSReceiptReceiving列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<MMSReceiptReceivingOutput>> List([FromQuery] MMSReceiptReceivingInput input)
    {
        return await _rep.AsQueryable().Select<MMSReceiptReceivingOutput>().ToListAsync();
    }


    /*请在此扩展应用服务实现*/
    /// <summary>
    /// 接收上传文件方法
    /// </summary>
    /// <param name="file">文件内容</param>
    /// <returns>文件名称</returns>
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> UploadExcelFile(IFormFile file)
    {


        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        string url = await ImprotExcel.WriteFile(file);
        var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
        //1根据用户的角色 解析出Excel
        IMReceiptReceivingExcelInterface factoryExcel = MReceiptReceivingExcelFactory.ExportReceipt();
        //factoryExcel._db = _db;
        factoryExcel._repMReceipt = _repMReceipt;
        factoryExcel._repMReceiptDetail = _repMReceiptDetail;
        factoryExcel._repMReceiptReceiving = _repMReceiptReceiving;
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._userManager = _userManager;
        //factoryExcel._repTableColumnsDetail = _repTableColumnsDetail;
        var data = factoryExcel.Strategy(dataExcel);


        var entityListDtos = data.Data.TableToList<MMSReceiptReceivingDetail>();
        //var entityListDtos = ObjectMapper.Map<List<WMS_ReceiptReceivingListDto>>(data.Data);

        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        var supplier = _repMReceipt.AsQueryable().Where(a => a.ReceiptNumber == entityListDtos.First().ReceiptNumber).First();
        long supplierId = 0;
        if (supplier != null)
        {
            supplierId = supplier.SupplierId;
        }
        else
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "数据错误" };
        }
        //使用简单工厂定制化修改和新增的方法
        IMReceiptReceivingInterface factory = MReceiptReceivingFactory.GetReceiptReceiving(supplierId);
        //factory._db = _db;
        factory._repMReceiptReceiving = _rep;
        factory._repMReceipt = _repMReceipt;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repTableColumns = _repTableColumns;
        factory._repLocation = _repLocation;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._userManager = _userManager;
        factory._repSupplier = _repSupplier;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repLocation = _repLocation;
        var response = await factory.Strategy(entityListDtos);
        return response;

    }




    /// <summary>
    /// 状态回退
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> ReturnReceiptReceiving(List<long> input)
    {
        //Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();
        //获取客户的id
        var customerData = _rep.AsQueryable().Where(a => a.Id == input[0]).First();
        long customerId = 0;
        //if (customerData != null)
        //{
        //    customerId = customerData.CustomerId;
        //}
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，

        IReceiptReceivingReturnInterface factory = ReceiptReceivingReturnFactory.ReturnReceiptReceiving(customerId);
        //factory._db = _db;
        //factory._repReceipt = _rep;
        //factory._repReceiptDetail = _repReceiptDetail;
        //factory._repReceiptReceiving = _repReceiptReceiving;
        //factory._repTableColumns = _repTableColumns;
        //factory._userManager = _userManager;
        //factory._repLocation = _repLocation;
        return await factory.Strategy(input);

        //return response;
    }


    /// <summary>
    ///【WMS_Receipt】通过id 加入库存
    /// </summary>
    //[AbpAuthorize(WMS_ReceiptPermissions.Node)]
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "AddInventory")]
    public async Task<Response<List<OrderStatusDto>>> AddInventory(List<long> input)
    {

        //var receipt = _rep.AsQueryable().Where(a => a.Id == input).FirstAsync();
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();


        //获取客户的id
        var supplier = _rep.AsQueryable().Where(a => a.Id == input.First()).First();
        long supplierId = 0;
        if (supplier != null)
        {
            supplierId = supplier.SupplierId;
        }
        //使用简单工厂定制化修改和新增的方法
        IMReceiptInventoryInterface factory = MReceiptInventoryFactory.AddInventory(supplier.SupplierId);
        //factory._db = _db;

        factory._repMReceiptReceiving = _rep;
        factory._repMReceipt = _repMReceipt;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repTableColumns = _repTableColumns;
        factory._repLocation = _repLocation;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._userManager = _userManager;
        factory._repSupplier = _repSupplier;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repLocation = _repLocation;
        factory._repInventoryUsable = _repInventoryUsable;

        //List<long> ids = new List<long>();
        //ids.Add(input);
        response = await factory.Strategy(input);

        //var entity = await _wms_receiptRepository.GetAllIncluding(a => a.ReceiptReceivings).Where(b => b.Id == input.Id).FirstAsync();
        //var dto = ObjectMapper.Map<WMS_ReceiptListDto>(entity);
        return response;
    }



    [HttpPost]
    public ActionResult ExportReceiptReceiving(List<long> input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐
        IMReceiptReceivingExcelInterface factory = MReceiptReceivingExcelFactory.ExportReceipt();

        factory._repMReceiptReceiving = _rep;
        factory._repMReceipt = _repMReceipt;
        factory._repMReceiptDetail = _repMReceiptDetail;
        factory._repTableColumns = _repTableColumns;
        //factory._repLocation = _repLocation;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._userManager = _userManager;
        factory._repSupplier = _repSupplier;
        factory._repSupplierUser = _repSupplierUser;
        factory._repMReceiptReceivingDetail = _repMReceiptReceivingDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._repLocation = _repLocation;
        //factory._repTableColumns = _repTableInventoryUsed;
        var response = factory.Strategy(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "上架单.xlsx" // 配置文件下载显示名
        };
    }

}

