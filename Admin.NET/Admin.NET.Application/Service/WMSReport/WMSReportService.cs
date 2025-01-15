using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.ReceiptCore.Factory;
using Admin.NET.Application.ReceiptCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Factory;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
//using MyProject.ReceiptReceivingCore.Dto;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Admin.NET.Core.Do;
using Admin.NET.Application.Service.WMSReport.Dto;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSReportService : IDynamicApiController, ITransient
{

    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
    public WMSReportService(SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed)
    {

        //_db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableInventoryUsed = repTableInventoryUsed;
    }
    /// <summary>
    /// 增加WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ReportScreen")]
    public async Task<ReturnResult> UpdateFirstReviewPageOrdersListDetailByLineNo()
    {
        ReturnResult result = new ReturnResult();
        try
        {
            //1.客户：出入库，出入库金额数据
            result.CustOrderReceiptAmount = GetCustOrderReceiptAmountDto();
            result.TopFiveOrderCountAmount = GetTopFiveOrderCountAmountDto();
            result.TopThreeOrderCountAmount = GetTopThreeOrderCountAmountDto();
            result.TopFiveCustOrderCountGdp = GetTopFiveCustOrderCountGdpDto();
            result.AllMoneyQtyOut = GetAllMoneyQtyOutDto();
            result.AllMoneyQtyIn = GetAllMoneyQtyInDto();



            result.success = true;
            return result;
        }
        catch (Exception e)
        {
            result.msg = e.Message;
            result.success = false;
            return result;
            throw;
        }
    }

    //1.客户：出入库，出入库金额数据
    public List<CustOrderReceiptAmountDto> GetCustOrderReceiptAmountDto()
    {
        string sql = "SELECT CASE WHEN ina.CustomerName IS NULL THEN inb.CustomerName ELSE ina.CustomerName END AS CustomerName, CASE WHEN ina.WoSumQty IS NULL THEN 0 ELSE ina.WoSumQty END AS WoSumQty, CASE WHEN ina.WoSumPrice IS NULL THEN 0 ELSE ina.WoSumPrice END AS WoSumPrice, CASE WHEN inb.ReSumQty IS NULL THEN 0 ELSE inb.ReSumQty END AS ReSumQty, CASE WHEN inb.ReSumPrice IS NULL THEN 0 ELSE inb.ReSumPrice END AS ReSumPrice FROM( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS WoSumPrice,SUM(a.orderQty) AS WoSumQty FROM( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.OrderQty) orderQty FROM WMS_Order wo LEFT JOIN dbo.WMS_OrderDetail wod ON wo.Id=wod.OrderId WHERE YEAR(wo.OrderTime) = YEAR(GETDATE()) AND MONTH(wo.OrderTime) = MONTH(GETDATE())  GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName) ina FULL JOIN ( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END )) AS ReSumPrice,SUM(a.orderQty) AS ReSumQty FROM ( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.ReceivedQty) orderQty FROM WMS_Receipt wo LEFT JOIN dbo.WMS_ReceiptDetail wod ON wo.Id=wod.ReceiptId WHERE YEAR(wo.ReceiptTime) = YEAR(GETDATE()) AND MONTH(wo.ReceiptTime) = MONTH(GETDATE())  GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName ) inb ON ina.CustomerId=inb.CustomerId";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<CustOrderReceiptAmountDto>();
    }

    //2.Top前五货号，出库数量，金额
    public List<TopFiveOrderCountAmountDto> GetTopFiveOrderCountAmountDto()
    {
        string sql = "SELECT TOP 5 a.SKU,SUM(OrderQty) AS OrderQty,SUM(OrderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS TotalMoney FROM dbo.WMS_OrderDetail a LEFT JOIN dbo.WMS_Order wo ON wo.Id=a.OrderId LEFT JOIN WMS_Product b ON b.CustomerId = a.CustomerId AND b.SKU = a.SKU WHERE YEAR(wo.OrderTime) = YEAR(GETDATE()) AND MONTH(wo.OrderTime) = MONTH(GETDATE()) GROUP BY a.SKU ORDER BY SUM(OrderQty) DESC";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopFiveOrderCountAmountDto>();
    }

    //3.top前三客户 , 出库数量，金额
    public List<TopThreeOrderCountAmountDto> GetTopThreeOrderCountAmountDto()
    {
        string sql = "SELECT TOP 3 a.CustomerName,SUM(OrderQty) AS OrderQty,SUM(OrderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS TotalMoney FROM dbo.WMS_OrderDetail a LEFT JOIN dbo.WMS_Order wo ON wo.Id=a.OrderId LEFT JOIN WMS_Product b ON b.CustomerId = a.CustomerId AND b.SKU = a.SKU WHERE YEAR(wo.OrderTime) = YEAR(GETDATE()) AND MONTH(wo.OrderTime) = MONTH(GETDATE())  GROUP BY a.CustomerId,a.CustomerName ORDER BY SUM(OrderQty) DESC";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopThreeOrderCountAmountDto>();
    }

    //4.Top前五客户，订单数，占比
    public List<TopFiveCustOrderCountGdpDto> GetTopFiveCustOrderCountGdpDto()
    {
        string sql = "SELECT TOP 5 ina.CustomerName,ina.OrderCount,FORMAT((CAST(ina.OrderCount AS DECIMAL) / CAST(inb.sumcount AS DECIMAL)) * 100.0, 'N2') + '%' AS percentage FROM(SELECT *,1 AS leftjoins FROM(SELECT CustomerName,COUNT(0) AS OrderCount FROM dbo.WMS_Order WHERE YEAR(WMS_Order.OrderTime) = YEAR(GETDATE()) AND MONTH(WMS_Order.OrderTime) = MONTH(GETDATE())  GROUP BY CustomerId,CustomerName) a) ina LEFT JOIN (SELECT SUM(a.OrderCount) AS sumcount,1 AS leftjoins FROM (SELECT CustomerName,COUNT(0) AS OrderCount FROM dbo.WMS_Order WHERE YEAR(WMS_Order.OrderTime) = YEAR(GETDATE()) AND MONTH(WMS_Order.OrderTime) = MONTH(GETDATE())  GROUP BY CustomerId,CustomerName) a) inb ON inb.leftjoins = ina.leftjoins ORDER BY ina.OrderCount DESC";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopFiveCustOrderCountGdpDto>();
    }

    public List<AllMoneyQty> GetAllMoneyQtyInDto()
    {
        string sql = "SELECT SUM(iis.OutTotalMoney) OutTotalMoney,SUM(iis.OutOrderQty) OutOrderQty FROM( SELECT a.CustomerName,SUM(OrderQty) AS OutOrderQty,SUM(OrderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS OutTotalMoney FROM dbo.WMS_OrderDetail a LEFT JOIN dbo.WMS_Order wo ON wo.Id=a.OrderId LEFT JOIN WMS_Product b ON b.CustomerId = a.CustomerId AND b.SKU = a.SKU WHERE YEAR(wo.OrderTime) = YEAR(GETDATE()) AND MONTH(wo.OrderTime) = MONTH(GETDATE())   GROUP BY a.CustomerId,a.CustomerName) iis";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<AllMoneyQty>();
    }

    public List<AllMoneyQty> GetAllMoneyQtyOutDto()
    {
        string sql = "SELECT SUM(iis.InTotalMoney) InTotalMoney,SUM(iis.InReceiptqty) InReceiptqty FROM( SELECT a.CustomerName,SUM(Receiptqty) AS InReceiptqty,SUM(Receiptqty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS InTotalMoney FROM dbo.WMS_ReceiptDetail a LEFT JOIN dbo.WMS_Receipt wo ON wo.Id=a.ReceiptId LEFT JOIN WMS_Product b ON b.CustomerId = a.CustomerId AND b.SKU = a.SKU WHERE YEAR(wo.ReceiptTime) = YEAR(GETDATE()) AND MONTH(wo.ReceiptTime) = MONTH(GETDATE())  GROUP BY a.CustomerId,a.CustomerName) iis ";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<AllMoneyQty>();
    }

}

