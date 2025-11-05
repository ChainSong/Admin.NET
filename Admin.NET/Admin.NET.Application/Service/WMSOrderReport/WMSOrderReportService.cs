using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Identity;
using Nest;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// WMSOrder服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSOrderReportService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSOrder> _rep;
    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;

    //private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    //private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    //private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSInventoryUsed> _repInventoryUsed;

    private readonly SqlSugarRepository<WMSInstruction> _repInstruction;
    private readonly SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation;

    private readonly SqlSugarRepository<WMSPickTask> _repPickTask;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;


    private readonly SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail;
    private readonly SqlSugarRepository<WMSPreOrder> _repPreOrder;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;
    //private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    private readonly SysWorkFlowService _repWorkFlowService;

    //private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;


    public WMSOrderReportService(SqlSugarRepository<WMSOrder> rep, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<WMSInventoryUsable> repInventoryUsable, ISqlSugarClient db, UserManager userManager, SqlSugarRepository<WMSInventoryUsed> repInventoryUsed, SqlSugarRepository<WMSInstruction> repInstruction, SqlSugarRepository<WMSOrderAllocation> repOrderAllocation, SqlSugarRepository<WMSPickTask> repPickTask, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WMSPreOrderDetail> repPreOrderDetail, SqlSugarRepository<WMSPreOrder> repPreOrder, SqlSugarRepository<WMSWarehouse> repWarehouse, SqlSugarRepository<WMSPackage> repPackage, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysWorkFlowService repWorkFlowService)
    {
        _rep = rep;
        _repOrderDetail = repOrderDetail;
        //_repReceipt = repReceipt;
        //_repReceiptDetail = repReceiptDetail;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repInventoryUsable = repInventoryUsable;
        //_db = db;
        _userManager = userManager;
        _repInventoryUsed = repInventoryUsed;
        _repInstruction = repInstruction;
        _repOrderAllocation = repOrderAllocation;
        _repPickTask = repPickTask;
        _repPickTaskDetail = repPickTaskDetail;
        _repPreOrderDetail = repPreOrderDetail;
        _repPreOrder = repPreOrder;
        _repWarehouse = repWarehouse;
        _repPackage = repPackage;
        _repPackageDetail = repPackageDetail;
        //_repWorkFlow = repWorkFlow;
        _repWorkFlowService = repWorkFlowService;

    }

    //根据传入的List<Id> id导出s，查询出对应的订单信息，并导出Excel
    [HttpPost]
    //[ServiceFilter(typeof(UnitOfWorkAttribute))]
    public ActionResult ExportWMSOrderByRFID(List<long> ids)
    {
        //    string strSql = @"select  distinct 
        //            CompleteTime '出库日期',
        //            WMS_OrderDetail.PoCode '合同号',
        //            WMS_OrderDetail.ExternOrderNumber  'JOB号', 
        //            WMS_OrderDetail.ExternOrderNumber  '出库单号', 
        //            RFIDInfo.PackageNumber '箱号',
        //            WMS_OrderDetail.SKU '货号',
        //            (case isnull(RFID,'') when ''  then WMS_OrderDetail.AllocatedQty  else 1 end) '出库数量',
        //            WMS_OrderDetail.Onwer '类型',
        //            OrderAddress.Name '收货人',
        //            OrderAddress.Address '地址',
        //            OrderAddress.Phone '电话',
        //'' 'JOB号总箱数',	
        //'' '承运人',
        //WMS_OrderDetail.GoodsName '品名',
        //'' '单号',	
        //RFIDInfo.RFID '防伪码',
        //OrderAddress.CompanyName '最终用户名称',
        //            (select COUNT(1) from WMS_Package where ExternOrderNumber=WMS_Order.ExternOrderNumber) '箱数量'
        //            from 
        //            WMS_Order left join WMS_OrderDetail
        //            on WMS_Order.Id=WMS_OrderDetail.OrderId
        //            outer apply (select top 1* from WMS_OrderAddress where ExternOrderNumber=WMS_Order.ExternOrderNumber) OrderAddress
        //            left join WMS_PickTaskDetail  on  WMS_OrderDetail.Id=WMS_PickTaskDetail.OrderDetailId
        //            outer apply (
        //            select distinct  rfid ,PackageNumber,PickTaskNumber from WMS_RFIDInfo where  WMS_PickTaskDetail.PickTaskNumber=WMS_RFIDInfo.PickTaskNumber
        //            and  WMS_PickTaskDetail.SKU=WMS_RFIDInfo.SKU and Status=99
        //            ) RFIDInfo
        //            where  WMS_Order.Id in (" + string.Join(",", ids) + ")";
        //    string strSql = @"  select distinct
        //            CompleteTime '出库日期',
        //            WMS_OrderDetail.PoCode '合同号',
        //            WMS_OrderDetail.ExternOrderNumber  'JOB号', 
        //            WMS_OrderDetail.ExternOrderNumber  '出库单号', 
        //            isnull(RFIDInfo.PackageNumber,Package.PackageNumber) '箱号', 
        //            WMS_OrderDetail.SKU '货号',
        //            (case isnull(RFID, '') when ''  then WMS_OrderDetail.AllocatedQty  else 1 end) '出库数量',
        //            (select COUNT(1) from WMS_Package where ExternOrderNumber = WMS_Order.ExternOrderNumber) '组合箱数',
        //WMS_OrderDetail.Onwer '类型',
        //            OrderAddress.Name '收货人',
        //            OrderAddress.Address '地址',
        //            OrderAddress.Phone '电话',
        //(select COUNT(1) from WMS_Package where ExternOrderNumber = WMS_Order.ExternOrderNumber)  'JOB号总箱数',	
        //Package.ExpressCompany '承运人',
        //WMS_OrderDetail.GoodsName '品名',
        //Package.ExpressNumber '顺丰单号',	
        //Package.ExpressCompany '顺丰单号',	
        //RFIDInfo.RFID '防伪码',
        //OrderAddress.CompanyName '最终用户名称'
        //from
        //            WMS_Order left join WMS_OrderDetail
        //            on WMS_Order.Id = WMS_OrderDetail.OrderId
        //            outer apply(select top 1 * from WMS_OrderAddress where ExternOrderNumber = WMS_Order.ExternOrderNumber) OrderAddress
        //            left join WMS_PickTaskDetail  on WMS_OrderDetail.Id = WMS_PickTaskDetail.OrderDetailId
        //              outer apply(
        //            select distinct  rfid , PackageNumber, PickTaskNumber from WMS_RFIDInfo where  WMS_PickTaskDetail.PickTaskNumber= WMS_RFIDInfo.PickTaskNumber
        //            and WMS_PickTaskDetail.SKU= WMS_RFIDInfo.SKU and Status = 99
        //            ) RFIDInfo
        //              outer apply(
        //            select distinct top 1 PackageNumber, ExpressCompany, ExpressNumber from WMS_Package where WMS_PickTaskDetail.PickTaskNumber = WMS_Package.PickTaskNumber
        //            and(isnull(RFIDInfo.PackageNumber, '') = '' or  WMS_Package.PackageNumber = RFIDInfo.PackageNumber)
        //            ) Package
        //            where  WMS_Order.Id in (" + string.Join(",", ids) + ")";


        string strSql = @"  select distinct
                CompleteTime '出库日期',
                WMS_OrderDetail.PoCode '合同号',
                WMS_OrderDetail.ExternOrderNumber  'JOB号', 
                WMS_OrderDetail.ExternOrderNumber  '出库单号', 
                isnull(RFIDInfo.PackageNumber,Package.PackageNumber) '箱号', 
                WMS_OrderDetail.SKU '货号',
                (case isnull(RFID, '') when ''  then Package.Qty  else 1 end) '出库数量',
                (select COUNT(1) from WMS_Package where ExternOrderNumber = WMS_Order.ExternOrderNumber) '组合箱数',
				WMS_OrderDetail.Onwer '类型',
                OrderAddress.Name '收货人',
                OrderAddress.Address '地址',
                OrderAddress.Phone '电话',
				(select COUNT(1) from WMS_Package where ExternOrderNumber = WMS_Order.ExternOrderNumber)  'JOB号总箱数',	
				Package.ExpressCompany '承运人',
				WMS_OrderDetail.GoodsName '品名',
				Package.ExpressNumber '顺丰单号',	
				Package.ExpressCompany '顺丰单号',	
				RFIDInfo.RFID '防伪码',
				OrderAddress.CompanyName '最终用户名称'
				from
                WMS_Order left join WMS_OrderDetail
                on WMS_Order.Id = WMS_OrderDetail.OrderId
                outer apply(select top 1 * from WMS_OrderAddress where ExternOrderNumber = WMS_Order.ExternOrderNumber) OrderAddress
                left join WMS_PickTaskDetail  on WMS_OrderDetail.Id = WMS_PickTaskDetail.OrderDetailId
                  outer apply(
                select distinct  rfid , PackageNumber, PickTaskNumber from WMS_RFIDInfo where  WMS_PickTaskDetail.PickTaskNumber= WMS_RFIDInfo.PickTaskNumber
                and WMS_PickTaskDetail.SKU= WMS_RFIDInfo.SKU and Status = 99
                ) RFIDInfo
                 outer apply(
                select distinct top 1 WMS_Package.PackageNumber, ExpressCompany, ExpressNumber,sum(Qty) Qty from WMS_Package
				left join WMS_PackageDetail  on WMS_Package.Id=WMS_PackageDetail.PackageId
				where WMS_PickTaskDetail.PickTaskNumber = WMS_Package.PickTaskNumber
                and(isnull(RFIDInfo.PackageNumber, '') = '' or  WMS_Package.PackageNumber = RFIDInfo.PackageNumber) 
				and  WMS_PackageDetail.SKU=WMS_OrderDetail.SKU
				group by WMS_Package.PackageNumber, ExpressCompany, ExpressNumber
                ) Package
                where  WMS_Order.Id in (" + string.Join(",", ids) + ")";

        //执行sql 语句
        var data = _rep.Context.Ado.GetDataTable(strSql.ToString());

        //类型转换
        //var wMSRFIDInfos = wMSRFIDs.Adapt<List<WMSRFIDImportExport>>();
        //var wMSRFIDInfos = wMSRFIDs.MapTo<List<WMSRFIDInfoOutput>>();

        //return query.ToListAsync();
        //return await query.ToListAsync();
        //IASNExcelInterface factoryExcel = ASNExcelFactory.ASNExcel();
        //factoryExcel._repTableColumns = _repTableColumns;
        //factoryExcel._userManager = _userManager;
        //factoryExcel._repASN = _rep;
        //var response = factoryExcel.Export(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(data);
        var fs = new MemoryStream(result.Result);
        ////return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "RFID_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }
}

