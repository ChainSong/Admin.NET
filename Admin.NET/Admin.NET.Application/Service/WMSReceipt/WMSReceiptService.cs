﻿using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using NewLife.Net;
using NewLife.Security;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]

public class WMSReceiptService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSReceipt> _rep;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;


    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    //private readonly SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable;
    //private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
    private readonly SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo;
    private readonly SqlSugarRepository<WMSLocation> _repLocation;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    private readonly WMSReceiptReceivingService _repReceiptReceivingService;

    private readonly SysWorkFlowService _repWorkFlowService;

    private readonly SqlSugarRepository<WMSASN> _repASN;
    //注入ASNDetail仓储
    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;
    public WMSReceiptService(SqlSugarRepository<WMSReceipt> rep, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, ISqlSugarClient db, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<WMSInventoryUsable> repTableInventoryUsable, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed, SqlSugarRepository<WMSASN> repASN, SqlSugarRepository<WMSASNDetail> repASNDetail, SqlSugarRepository<WMSRFIDInfo> repRFIDInfo, SqlSugarRepository<SysWorkFlow> repWorkFlow, SqlSugarRepository<WMSProduct> repProduct, WMSReceiptReceivingService repReceiptReceivingService, SqlSugarRepository<WMSLocation> repLocation, SysWorkFlowService repWorkFlowService)
    {
        _rep = rep;
        _repReceiptDetail = repReceiptDetail;
        //_db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repTableInventoryUsable = repTableInventoryUsable;
        _repTableInventoryUsed = repTableInventoryUsed;
        _repASN = repASN;
        _repASNDetail = repASNDetail;
        _repRFIDInfo = repRFIDInfo;
        _repWorkFlow = repWorkFlow;
        _repProduct = repProduct;
        _repReceiptReceivingService = repReceiptReceivingService;
        _repLocation = repLocation;
        _repWorkFlowService = repWorkFlowService;
        //this._repTableInventoryUsable = repTableInventoryUsable;

    }

    /// <summary>
    /// 分页查询WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSReceiptOutput>> Page(WMSReceiptInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ASNId > 0, u => u.ASNId == input.ASNId)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId.HasValue && input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ReceiptStatus.HasValue && input.ReceiptStatus != 0, u => u.ReceiptStatus == input.ReceiptStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Po), u => u.Po.Contains(input.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.So), u => u.So.Contains(input.So.Trim()))
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
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSReceiptOutput>();


        if (input.ASNNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ASNNumber.IndexOf("\n") > 0)
            {
                numbers = input.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ASNNumber.IndexOf(',') > 0)
            {
                numbers = input.ASNNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (input.ASNNumber.IndexOf(' ') > 0)
            {
                numbers = input.ASNNumber.Split(' ').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => numbers.Contains(u.ASNNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()));
            }
        }

        if (input.ExternReceiptNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ExternReceiptNumber.IndexOf("\n") > 0)
            {
                numbers = input.ExternReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ExternReceiptNumber.IndexOf(',') > 0)
            {
                numbers = input.ExternReceiptNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (input.ExternReceiptNumber.IndexOf(' ') > 0)
            {
                numbers = input.ExternReceiptNumber.Split(' ').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => numbers.Contains(u.ExternReceiptNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()));
            }
        }

        if (input.ReceiptNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ReceiptNumber.IndexOf("\n") > 0)
            {
                numbers = input.ReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ReceiptNumber.IndexOf(',') > 0)
            {
                numbers = input.ReceiptNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (input.ReceiptNumber.IndexOf(' ') > 0)
            {
                numbers = input.ReceiptNumber.Split(' ').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => numbers.Contains(u.ReceiptNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()));
            }
        }

        if (input.ReceiptTime != null && input.ReceiptTime.Count > 0)
        {
            DateTime? start = input.ReceiptTime[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime >= start);
            if (input.ReceiptTime.Count > 1 && input.ReceiptTime[1].HasValue)
            {
                var end = input.ReceiptTime[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
            }
        }
        if (input.CompleteTime != null && input.CompleteTime.Count > 0)
        {
            DateTime? start = input.CompleteTime[0];
            query = query.WhereIF(start.HasValue, u => u.CompleteTime >= start);
            if (input.CompleteTime.Count > 1 && input.CompleteTime[1].HasValue)
            {
                var end = input.CompleteTime[1].Value.AddDays(1);
                query = query.Where(u => u.CompleteTime < end);
            }
        }
        if (input.CreationTime != null && input.CreationTime.Count > 0)
        {
            DateTime? start = input.CreationTime[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime >= start);
            if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
            {
                var end = input.CreationTime[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1 != null && input.DateTime1.Count > 0)
        {
            DateTime? start = input.DateTime1[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 >= start);
            if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
            {
                var end = input.DateTime1[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2 != null && input.DateTime2.Count > 0)
        {
            DateTime? start = input.DateTime2[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 >= start);
            if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
            {
                var end = input.DateTime2[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3 != null && input.DateTime3.Count > 0)
        {
            DateTime? start = input.DateTime3[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 >= start);
            if (input.DateTime3.Count > 1 && input.DateTime3[1].HasValue)
            {
                var end = input.DateTime3[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4 != null && input.DateTime4.Count > 0)
        {
            DateTime? start = input.DateTime4[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 >= start);
            if (input.DateTime4.Count > 1 && input.DateTime4[1].HasValue)
            {
                var end = input.DateTime4[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5 != null && input.DateTime5.Count > 0)
        {
            DateTime? start = input.DateTime5[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 >= start);
            if (input.DateTime5.Count > 1 && input.DateTime5[1].HasValue)
            {
                var end = input.DateTime5[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSReceiptInput input)
    {
        var entity = input.Adapt<WMSReceipt>();
        await _rep.InsertAsync(entity);
    }





    /// <summary>
    /// 删除WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> Delete(DeleteWMSReceiptInput input)
    {
        var entity = await _rep.GetByIdAsync(input.Id);
        //根据订单类型判断是否存在该流程
        //var workflow = await _repWorkFlow.AsQueryable()
        //   .Includes(a => a.SysWorkFlowSteps)
        //   .Where(a => a.WorkName == entity.CustomerName + InboundWorkFlowConst.Workflow_ReceiptReturn).FirstAsync();

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(entity.CustomerName, InboundWorkFlowConst.Workflow_Inbound, InboundWorkFlowConst.Workflow_ReceiptReturn, entity.ReceiptType);


        //使用简单工厂定制化  /
        List<DeleteWMSReceiptInput> request = new List<DeleteWMSReceiptInput>();
        request.Add(input);
        IReceiptReturnInterface factory = ReceiptReturnFactory.ReturnReceipt(workflow);
        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repASN = _repASN;
        factory._repASNDetail = _repASNDetail;
        factory._repRFIDInfo = _repRFIDInfo;
        //factory._repTableColumns = _repTableInventoryUsed;
        return await factory.Strategy(request);

    }

    /// <summary>
    /// 更新WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSReceiptInput input)
    {
        var entity = input.Adapt<WMSReceipt>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "SaveRFID")]
    public async Task<Response<List<OrderStatusDto>>> SaveRFID(UpdateWMSReceiptInput input)
    {
        var entity = await _rep.GetByIdAsync(input.Id);
        //根据订单类型判断是否存在该流程
        //var workflow = await _repWorkFlow.AsQueryable()
        //   .Includes(a => a.SysWorkFlowSteps)
        //   .Where(a => a.WorkName == entity.CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(entity.CustomerName, InboundWorkFlowConst.Workflow_Inbound, InboundWorkFlowConst.Workflow_CreateRFID, entity.ReceiptType);


        //使用简单工厂定制化  /
        //List<DeleteWMSReceiptInput> request = new List<DeleteWMSReceiptInput>();
        //request.Add(input);
        IReceiptRFIDInterface factory = ReceiptRFIDFactory.RFIDReceipt(workflow);
        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._userManager = _userManager;
        factory._repProduct = _repProduct;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repASN = _repASN;
        factory._repRFIDInfo = _repRFIDInfo;
        factory._repASNDetail = _repASNDetail;
        //factory._repTableColumns = _repTableInventoryUsed;
        return await factory.SaveRFID(input);
    }



    /// <summary>
    /// 获取WMSReceipt 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSReceipt> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSReceipt 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetReceipts")]
    public async Task<List<WMSReceipt>> GetReceipts(List<long> ids)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => ids.Contains(u.Id)).ToListAsync();
        return entity;
    }



    /// <summary>
    /// 获取WMSReceipt列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSReceiptOutput>> List([FromQuery] WMSReceiptInput input)
    {
        return await _rep.AsQueryable().Select<WMSReceiptOutput>().ToListAsync();
    }



    /// <summary>
    /// 导出上架单模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    public ActionResult ExportReceipt(WMSReceiptInput input)
    {
        try
        {
            //使用简单工厂定制化
            //使用简单工厂定制化
            IReceiptExcelInterface factory = ReceiptExportFactory.ExportReceipt();
            factory._repReceipt = _rep;
            factory._repReceiptDetail = _repReceiptDetail;
            factory._userManager = _userManager;
            factory._repTableColumns = _repTableColumns;
            factory._repTableColumnsDetail = _repTableColumnsDetail;
            //var receiptData = _rep.AsQueryable().Includes(a => a.Details).Where(a => input.Contains(a.Id));
            var response = factory.Export(input);
            IExporter exporter = new ExcelExporter();
            var result = exporter.ExportAsByteArray<DataTable>(response.Data);
            var fs = new MemoryStream(result.Result);
            //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
            return new FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = "入库单.xlsx" // 配置文件下载显示名
            };
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }


    [HttpPost]
    [UnitOfWork]
    public ActionResult ExportReceiptReceiving(List<long> input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐
        IReceiptReceivingExportInterface factory = ReceiptReceivingExportFactory.GetReceiptReceiving();

        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repTableInventoryUsable = _repTableInventoryUsable;
        factory._repTableInventoryUsed = _repTableInventoryUsed;
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



    [HttpPost]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> QuickInventory(List<long> input)
    {
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();
        //根据id 获取入库单数据，
        //通过入库单数据构建上架数据，
        var receiptData = await _rep.AsQueryable().Includes(a => a.Details).Where(a => input.Contains(a.Id)).ToListAsync();
        List<WMSReceiptReceiving> receiptReceivingList = new List<WMSReceiptReceiving>();

        //快捷入库，获取第一个库位信息

        var location = await _repLocation.AsQueryable().Where(a => a.LocationStatus == (int)LocationStatusEnum.可用 && a.WarehouseId == receiptData.First().WarehouseId).OrderBy(a => a.Id).ToListAsync();
        if (location.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "没有可用库位";
            response.Data = new List<OrderStatusDto>();
            return response;
        }


        var config = new MapperConfiguration(cfg => cfg.CreateMap<WMSReceiptDetail, WMSReceiptReceiving>()
         .ForMember(a => a.ReceiptDetailId, opt => opt.MapFrom(c => c.Id))
         .ForMember(a => a.Area, opt => opt.MapFrom(c => location.First().AreaName))
         .ForMember(a => a.Location, opt => opt.MapFrom(c => location.First().Location))
         .ForMember(a => a.Updator, opt => opt.Ignore())
         .ForMember(a => a.UpdateTime, opt => opt.Ignore())
         .ForMember(a => a.CreationTime, opt => opt.Ignore())
         .ForMember(a => a.Creator, opt => opt.Ignore())
         .ForMember(a => a.Id, opt => opt.Ignore())
        //.ForMember(a => a.ReceiptDetailId, opt => opt.MapFrom(c => c.Id))
        );
       
       

        var mapper = new Mapper(config);
        foreach (var item in receiptData)
        {
            var receiptReceiving = mapper.Map<List<WMSReceiptReceiving>>(item.Details);
            receiptReceivingList.AddRange(receiptReceiving);

        }
        var addReceiptReceiving = await _repReceiptReceivingService.Add(receiptReceivingList);
        if (addReceiptReceiving.Code == StatusCode.Success)
        {
            return await _repReceiptReceivingService.AddInventory(input);
        }

        response.Code = StatusCode.Error;
        response.Msg = "上架单生成失败";
        response.Data = new List<OrderStatusDto>();
        return response;
    }

}

