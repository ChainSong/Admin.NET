using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.ReceiptCore.Factory;
using Admin.NET.Application.ReceiptCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Factory;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.WMSReport.Dto;
using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Do;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using MathNet.Numerics.OdeSolvers;
using Microsoft.AspNetCore.Http;
using Nest;
//using MyProject.ReceiptReceivingCore.Dto;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinAccountGetAccountBasicInfoResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSReceiptReportService : IDynamicApiController, ITransient
{

    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
    public WMSReceiptReportService(SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed)
    {

        //_db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableInventoryUsed = repTableInventoryUsed;
    }

    [HttpPost]
    [UnitOfWork]
    [DisplayName("增加GetReceiptReport")]
    [ApiDescriptionSettings(Name = "GetReceiptReport")]
    public ActionResult GetReceiptReport([FromBody] List<long> ids)
    {
        string strSql = @"select WMS_Receipt.CompleteTime '入库完成时间',
        isnull((select top 1 ShipmentNum from hach_wms_receiving  where OrderNo=WMS_Receipt.ExternReceiptNumber),WMS_Receipt.ExternReceiptNumber) '运单号',  
        WMS_ReceiptDetail.PoCode '合同号',
        WMS_ReceiptDetail.SKU '货号',
        WMS_ReceiptDetail.ReceivedQty '入库数量',
        WMS_ReceiptDetail.Onwer 'ASN类型',
        (case when Product.IsRFID = 1 then 'Y' else 'N' end)  '是否序列号采集',
        '手工入账' '备注'
        from WMS_Receipt left join WMS_ReceiptDetail
        on WMS_Receipt.Id = WMS_ReceiptDetail.ReceiptId
        outer apply(select* from WMS_Product where WMS_ReceiptDetail.CustomerId= WMS_Product.CustomerId and WMS_ReceiptDetail.SKU= WMS_Product.SKU) Product" +
        @" where WMS_Receipt.id in (" + string.Join(',', ids.Select(a => a)) + ")";

        //执行sql 语句
        var data = _repTableInventoryUsed.Context.Ado.GetDataTable(strSql.ToString());
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(data);
        var fs = new MemoryStream(result.Result);
        ////return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "入库反馈" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }
}

