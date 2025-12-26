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
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinAccountGetAccountBasicInfoResponse.Types;
using MathNet.Numerics.OdeSolvers;
using Admin.NET.Application.Service;
using System.IO;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Nest;

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
    private readonly SqlSugarRepository<WMSRFReceiptAcquisition> _repRFReceiptAcquisition;
    private readonly SqlSugarRepository<WMSRFPackageAcquisition> _repRFPackageAcquisition;
    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
    public WMSReportService(SqlSugarRepository<WMSCustomer> repCustomer,
        SqlSugarRepository<CustomerUserMapping> repCustomerUser,
        SqlSugarRepository<WarehouseUserMapping> repWarehouseUser,
        UserManager userManager,
        SqlSugarRepository<TableColumns> repTableColumns,
        SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed,
        SqlSugarRepository<WMSRFPackageAcquisition> repRFPackageAcquisition,
        SqlSugarRepository<WMSRFReceiptAcquisition> repRFReceiptAcquisition)
    {

        //_db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableInventoryUsed = repTableInventoryUsed;
        _repRFPackageAcquisition = repRFPackageAcquisition;
        _repRFReceiptAcquisition = repRFReceiptAcquisition;
    }

    #region 第一版大屏数据展示
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


    #endregion

    #region 第二版大屏数据展示
    /// <summary>
    /// 增加WMSProduct
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ReportScreenSecond")]
    public async Task<ReturnResult> UpdateSecondReviewPageOrdersListDetailByLineNo()
    {
        ReturnResult result = new ReturnResult();
        try
        {
            //1.客户：出入库，出入库金额数据
            var list1 = LeftTable1();
            var list2 = LeftTable2();
            var list3 = LeftTable3();
            var list4 = GetTopPercentage();
            result.LeftTable1 = list1;
            result.LeftTable2 = list2;
            result.LeftTable3 = list3;



            string?[] list1Qty1 = new string?[list1.Count()];
            int?[] list1Qty2 = new int?[list1.Count()];
            int?[] list1Qty3 = new int?[list1.Count()];
            int?[] list1Qty4 = new int?[list1.Count()];

            string?[] list1Price1 = new string?[list1.Count()];
            int?[] list1Price2 = new int?[list1.Count()];
            int?[] list1Price3 = new int?[list1.Count()];
            int?[] list1Price4 = new int?[list1.Count()];
            int ink1 = 0;
            //入库数量&出库数量&库存数量
            foreach (var item in list1)
            {
                list1Qty1[ink1] = item.CustomerName;
                list1Qty2[ink1] = item.ReSumQty;//出库数量
                list1Qty3[ink1] = item.WoSumQty;//入库数量
                list1Qty4[ink1] = item.AllSumQty;//库存数量

                //入库金额&出库金额&库存金额
                list1Price1[ink1] = item.CustomerName;
                list1Price2[ink1] = item.ReSumPrice;//出库数量
                list1Price3[ink1] = item.WoSumPrice;//入库数量
                list1Price4[ink1] = item.AllSumPrice;//库存数量
                ink1++;
            }



            string?[] pic3Name = new string?[list2.Count()];
            int?[] pic3Last = new int?[list2.Count()];
            int?[] pic3Now = new int?[list2.Count()];
            int ins = 0;
            //当月订单数量
            foreach (var item in list2)
            {
                pic3Name[ins] = item.CustomerName;
                pic3Last[ins] = item.countslast;
                pic3Now[ins] = item.countsnow;
                ins++;
            }



            string?[] pic4Name = new string?[list3.Count()];
            int?[] pic4percentage = new int?[list3.Count()];
            int ing = 0;
            //当月客户区域占比图
            foreach (var item in list3)
            {
                pic4Name[ing] = item.BigName;
                pic4percentage[ing] = item.counts;
                ing++;
            }


            List<Nameval> val = new List<Nameval>();
            foreach (var item in list3)
            {
                Nameval nameval = new Nameval();
                nameval.name = item.BigName;
                nameval.value = item.counts;
                val.Add(nameval);
            }

            result.pic1Name = list1Qty1;
            result.pic1Out = list1Qty2;
            result.pic1In = list1Qty3;
            result.pic1All = list1Qty4;

            result.pic2Name = list1Price1;
            result.pic2Out = list1Price2;
            result.pic2In = list1Price3;
            result.pic2All = list1Price4;

            result.pic3Name = pic3Name;
            result.pic3Now = pic3Now;
            result.pic3Last = pic3Last;

            result.pic4Name = pic4Name;
            result.pic4percentage = pic4percentage;

            result.pic4NameVal = val;



            TopNumbers top = new TopNumbers();
            foreach (var item in list1)
            {
                top.num1 += item.WoSumQty;//入库数量
                top.num2 += item.WoSumPrice;//出库金额
                top.num3 += item.AllSumQty;//库存数量
                top.num5 += item.ReSumQty;//出库数量
                top.num6 += item.ReSumPrice;//出库金额
                top.num7 += item.AllSumPrice;//库存金额
            }



            //上月与当月库存量对比
            top.num4 = list4.FirstOrDefault().QtyPercentage;
            //上月与当月库存金额对比
            top.num8 = list4.FirstOrDefault().PricePercentage;

            result.EightNum = top;
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

    public class Nameval
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public class TopNumbers
    {
        public int num1 { get; set; }
        public int num2 { get; set; }
        public int num3 { get; set; }
        public string? num4 { get; set; }
        public int num5 { get; set; }
        public int num6 { get; set; }
        public int num7 { get; set; }
        public string? num8 { get; set; }
    }


    //左侧1 table
    public List<CustOrderReceiptAmountDtoTwo> LeftTable1()
    {
        string sql = "SELECT CASE WHEN ina.CustomerName IS NULL THEN inb.CustomerName ELSE ina.CustomerName END AS CustomerName, CASE WHEN ina.WoSumQty IS NULL THEN 0 ELSE ina.WoSumQty END AS WoSumQty, CASE WHEN ina.WoSumPrice IS NULL THEN 0 ELSE ina.WoSumPrice END AS WoSumPrice, CASE WHEN inb.ReSumQty IS NULL THEN 0 ELSE inb.ReSumQty END AS ReSumQty, CASE WHEN inb.ReSumPrice IS NULL THEN 0 ELSE inb.ReSumPrice END AS ReSumPrice, CASE WHEN inc.qtys IS NULL THEN 0 ELSE inc.qtys END AS AllSumQty, CASE WHEN inc.price IS NULL THEN 0 ELSE inc.price END AS AllSumPrice FROM( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS WoSumPrice,SUM(a.orderQty) AS WoSumQty FROM( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.OrderQty) orderQty FROM WMS_Order wo LEFT JOIN dbo.WMS_OrderDetail wod ON wo.Id=wod.OrderId WHERE YEAR(wo.OrderTime) = YEAR(GETDATE()) AND MONTH(wo.OrderTime) = MONTH(GETDATE()) AND DAY(wo.OrderTime)< DAY(GETDATE()) GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName) ina FULL JOIN ( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END )) AS ReSumPrice,SUM(a.orderQty) AS ReSumQty FROM ( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.ReceivedQty) orderQty FROM WMS_Receipt wo LEFT JOIN dbo.WMS_ReceiptDetail wod ON wo.Id=wod.ReceiptId WHERE YEAR(wo.ReceiptTime) = YEAR(GETDATE()) AND MONTH(wo.ReceiptTime) = MONTH(GETDATE()) AND DAY(wo.ReceiptTime)< DAY(GETDATE()) GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName ) inb ON ina.CustomerId=inb.CustomerId LEFT JOIN ( select SUM(Qty) AS qtys,SUM(Qty * b.Price) AS price,a.CustomerName,a.CustomerId from WMS_Inventory_Usable a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU WHERE InventoryStatus IN(1,10,20) AND YEAR(a.InventoryTime) = YEAR(GETDATE()) AND MONTH(a.InventoryTime) = MONTH(GETDATE()) AND DAY(a.InventoryTime)< DAY(GETDATE()) GROUP BY a.CustomerName,a.CustomerId ) inc ON ina.CustomerId=inc.CustomerId";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<CustOrderReceiptAmountDtoTwo>();
    }


    //左侧2 table
    public List<TopFiveOrderCountAmountDtoTwo> LeftTable2()
    {
        string sql = "SELECT a.CustomerName, CASE WHEN a.counts IS NULL THEN 0 ELSE a.counts END AS countslast, CASE WHEN b.counts IS NULL THEN 0 ELSE b.counts END AS countsnow FROM(SELECT COUNT(*) AS counts,wo.CustomerName FROM dbo.WMS_Order wo WHERE wo.OrderTime>EOMONTH(GETDATE(), -2) AND wo.OrderTime<=EOMONTH(GETDATE(), -1) GROUP BY wo.CustomerName) a LEFT JOIN( SELECT COUNT(*) AS counts,wo.CustomerName FROM dbo.WMS_Order wo WHERE wo.OrderTime>EOMONTH(GETDATE(), -1) AND wo.OrderTime<=EOMONTH(GETDATE(),0) GROUP BY wo.CustomerName) b ON a.CustomerName=b.CustomerName ORDER BY a.counts DESC";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopFiveOrderCountAmountDtoTwo>();
    }

    //左侧3 table
    public List<TopThreeOrderCountAmountDtoTwo> LeftTable3()
    {
        string sql = "SELECT aa.BigName,aa.counts,CAST(CAST((aa.counts * 100.0) /ab.sumcount AS DECIMAL(10, 2)) AS VARCHAR(50))+'%' AS Percentage FROM( SELECT 1 AS leftjoin,a.BigName, COUNT(*) AS counts FROM( SELECT big.BigName,wo.Id FROM dbo.WMS_Order wo LEFT JOIN WMS_OrderAddress wod ON wo.PreOrderId=wod.PreOrderId LEFT JOIN ( SELECT a.Name,b.Name AS BigName FROM Sys_Regions a LEFT JOIN Sys_Regions b ON a.SupperID =b.ID WHERE a.Grade=2 AND a.Name !='-') big ON wod.Province =big.Name WHERE wo.OrderTime>EOMONTH(GETDATE(), -1) AND wo.OrderTime<=EOMONTH(GETDATE(), 0) AND big.BigName IS NOT NULL) a GROUP BY a.BigName ) aa LEFT JOIN ( SELECT 1 AS leftjoin,COUNT(0) AS sumcount FROM dbo.WMS_Order wo LEFT JOIN WMS_OrderAddress wod ON wo.PreOrderId=wod.PreOrderId LEFT JOIN ( SELECT a.Name,b.Name AS BigName FROM Sys_Regions a LEFT JOIN Sys_Regions b ON a.SupperID =b.ID WHERE a.Grade=2 AND a.Name !='-' ) big ON wod.Province =big.Name WHERE wo.OrderTime>EOMONTH(GETDATE(), -1) AND wo.OrderTime<=EOMONTH(GETDATE(), 0) AND big.BigName IS NOT NULL ) ab ON aa.leftjoin=ab.leftjoin ORDER BY aa.counts DESC ";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopThreeOrderCountAmountDtoTwo>();
    }


    //顶部两个百分比
    public List<TopNumTwo> GetTopPercentage()
    {
        string sql = "SELECT CAST(CASE WHEN aaa.AllSumQty> bbb.AllSumQty THEN CAST(0-(aaa.AllSumQty *100.0) / bbb.AllSumQty AS DECIMAL(10, 2)) WHEN aaa.AllSumQty< bbb.AllSumQty THEN CAST((aaa.AllSumQty *100.0) / bbb.AllSumQty AS DECIMAL(10, 2)) END AS VARCHAR(50)) +'%' AS QtyPercentage, CAST(CASE WHEN aaa.AllSumPrice> bbb.AllSumPrice THEN CAST(0-(aaa.AllSumPrice *100.0) / bbb.AllSumPrice AS DECIMAL(10, 2)) WHEN aaa.AllSumPrice< bbb.AllSumPrice THEN CAST((aaa.AllSumPrice *100.0) / bbb.AllSumPrice AS DECIMAL(10, 2)) END AS VARCHAR(50)) +'%' AS PricePercentage FROM( SELECT '1' AS leftjoin, SUM(inc.qtys) AllSumQty, SUM(inc.price) AllSumPrice FROM( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END)) AS WoSumPrice,SUM(a.orderQty) AS WoSumQty FROM ( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.OrderQty) orderQty FROM WMS_Order wo LEFT JOIN dbo.WMS_OrderDetail wod ON wo.Id=wod.OrderId WHERE wo.OrderTime>EOMONTH(GETDATE(), -1) AND wo.OrderTime<=EOMONTH(GETDATE(),0) GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName) ina LEFT JOIN ( SELECT SUM(Qty) AS qtys,SUM(Qty * b.Price) AS price,a.CustomerName,a.CustomerId from WMS_Inventory_Usable a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU WHERE InventoryStatus IN(1,10,20) AND a.InventoryTime>EOMONTH(GETDATE(), -1) AND a.InventoryTime<=EOMONTH(GETDATE(), 0) GROUP BY a.CustomerName,a.CustomerId ) inc ON ina.CustomerId=inc.CustomerId ) aaa LEFT JOIN ( SELECT '1' AS leftjoin, SUM(inc.qtys) AllSumQty, SUM(inc.price) AllSumPrice FROM ( SELECT a.CustomerId,a.CustomerName,SUM(a.orderQty*(CASE WHEN b.price IS NULL THEN 0 ELSE b.price END )) AS WoSumPrice,SUM(a.orderQty) AS WoSumQty FROM ( SELECT wo.CustomerId,wo.CustomerName,wod.SKU,SUM(wod.OrderQty) orderQty FROM WMS_Order wo LEFT JOIN dbo.WMS_OrderDetail wod ON wo.Id=wod.OrderId WHERE wo.OrderTime>EOMONTH(GETDATE(), -1) AND wo.OrderTime<=EOMONTH(GETDATE(),0) GROUP BY wo.CustomerId,wo.CustomerName,wod.SKU) a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU GROUP BY a.CustomerId,a.CustomerName ) ina LEFT JOIN ( SELECT SUM(Qty) AS qtys,SUM(Qty * b.Price) AS price,a.CustomerName,a.CustomerId from WMS_Inventory_Usable a LEFT JOIN (SELECT price,CustomerId,SKU FROM WMS_Product) b ON b.CustomerId=a.CustomerId AND b.SKU=a.SKU WHERE InventoryStatus IN(1,10,20) AND a.InventoryTime>EOMONTH(GETDATE(), -2) AND a.InventoryTime<=EOMONTH(GETDATE(), -1) GROUP BY a.CustomerName,a.CustomerId ) inc ON ina.CustomerId=inc.CustomerId ) bbb ON aaa.leftjoin =bbb.leftjoin ";
        return _repCustomer.Context.Ado.GetDataTable(sql).TableToList<TopNumTwo>();
    }

    #endregion


    #region 出入库序列号查询

    /// <summary>
    /// 入库序列号查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetWMSRFReceiptAcquisitionPageList")]
    public async Task<SqlSugarPagedList<WMSRFReceiptAcquisition>> GetWMSRFReceiptAcquisition(WMSRFReceiptAcquisitionInput input)
    {
        SqlSugarPagedList<WMSRFReceiptAcquisition> wMSRFReceiptAcquisitions = new SqlSugarPagedList<WMSRFReceiptAcquisition>();
        try
        {
            wMSRFReceiptAcquisitions = await _repRFReceiptAcquisition.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.ExternReceiptNumber), a => a.ExternReceiptNumber.Contains(input.ExternReceiptNumber))
                .WhereIF(!string.IsNullOrEmpty(input.ASNNumber), a => a.ASNNumber.Contains(input.ASNNumber))
                .WhereIF(!string.IsNullOrEmpty(input.ReceiptNumber), a => a.ReceiptNumber.Contains(input.ReceiptNumber))
                .WhereIF(input.CustomerId != null && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .WhereIF(input.WarehouseId != null && input.WarehouseId > 0, a => a.WarehouseId == input.WarehouseId)
                .WhereIF(!string.IsNullOrEmpty(input.Type), a => a.Type.Contains(input.Type))
                .WhereIF(!string.IsNullOrEmpty(input.SKU), a => a.SKU.Contains(input.SKU))
                .WhereIF(!string.IsNullOrEmpty(input.Lot), a => a.Lot.Contains(input.Lot))
                .WhereIF(!string.IsNullOrEmpty(input.SN), a => a.SN.Contains(input.SN))
                .OrderByDescending(a => a.CreationTime)
                .ToPagedListAsync(input.Page, input.PageSize);
        }
        catch (Exception ex)
        {
            throw;
        }
        return wMSRFReceiptAcquisitions;
    }

    /// <summary>
    /// 入库序列号导出
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ExportWMSRFReceiptAcquisition")]
    public async Task<ActionResult> ExportWMSRFReceiptAcquisition(WMSRFReceiptAcquisitionInput input)
    {
        List<WMSRFReceiptAcquisitionExport> wMSRFReceiptAcquisitions = new List<WMSRFReceiptAcquisitionExport>();
        IExcelExporter excelExporter = new ExcelExporter();
        try
        {
            wMSRFReceiptAcquisitions = await _repRFReceiptAcquisition.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.ExternReceiptNumber), a => a.ExternReceiptNumber.Contains(input.ExternReceiptNumber))
                .WhereIF(!string.IsNullOrEmpty(input.ASNNumber), a => a.ASNNumber.Contains(input.ASNNumber))
                .WhereIF(!string.IsNullOrEmpty(input.ReceiptNumber), a => a.ReceiptNumber.Contains(input.ReceiptNumber))
                .WhereIF(input.CustomerId != null && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .WhereIF(input.WarehouseId != null && input.WarehouseId > 0, a => a.WarehouseId == input.WarehouseId)
                .WhereIF(!string.IsNullOrEmpty(input.Type), a => a.Type.Contains(input.Type))
                .WhereIF(!string.IsNullOrEmpty(input.SKU), a => a.SKU.Contains(input.SKU))
                .WhereIF(!string.IsNullOrEmpty(input.Lot), a => a.Lot.Contains(input.Lot))
                .WhereIF(!string.IsNullOrEmpty(input.SN), a => a.SN.Contains(input.SN))
                .Select(a => new WMSRFReceiptAcquisitionExport
                {
                    ExternReceiptNumber = a.ExternReceiptNumber,
                    ASNNumber = a.ASNNumber,
                    ReceiptNumber = a.ReceiptNumber,
                    CustomerName = a.CustomerName,
                    WarehouseName = a.WarehouseName,
                    Type = a.Type,
                    SKU = a.SKU,
                    Lot = a.Lot,
                    SN = a.SN,
                    ProductionDate = a.ProductionDate,
                    ExpirationDate = a.ExpirationDate,
                    CreationTime = a.CreationTime,
                    Creator = a.Creator,
                    UpdateTime = a.UpdateTime,
                    Updator = a.Updator,
                })
                .OrderByDescending(a => a.CreationTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
        // 使用 IExcelExporter 导出 DataTable 数据
        var res = await excelExporter.ExportAsByteArray(wMSRFReceiptAcquisitions);

        return new FileStreamResult(new MemoryStream(res), "application/octet-stream")
        {
            FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmm") + "入库SN报表.xlsx"
        };
    }

    /// <summary>
    /// 出库序列号查询
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetWMSRFPackageAcquisitionPageList")]
    public async Task<SqlSugarPagedList<WMSRFPackageAcquisition>> GetWMSRFPackageAcquisition(WMSRFPackageAcquisitionInput input)
    {
        SqlSugarPagedList<WMSRFPackageAcquisition> wMSRFPackageAcquisitions = new SqlSugarPagedList<WMSRFPackageAcquisition>();
        try
        {
            wMSRFPackageAcquisitions = await _repRFPackageAcquisition.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.ExternOrderNumber), a => a.ExternOrderNumber.Contains(input.ExternOrderNumber))
                .WhereIF(!string.IsNullOrEmpty(input.PreOrderNumber), a => a.PreOrderNumber.Contains(input.PreOrderNumber))
                .WhereIF(!string.IsNullOrEmpty(input.OrderNumber), a => a.OrderNumber.Contains(input.OrderNumber))
                .WhereIF(input.CustomerId != null && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .WhereIF(input.WarehouseId != null && input.WarehouseId > 0, a => a.WarehouseId == input.WarehouseId)
                .WhereIF(!string.IsNullOrEmpty(input.Type), a => a.Type.Contains(input.Type))
                .WhereIF(!string.IsNullOrEmpty(input.SKU), a => a.SKU.Contains(input.SKU))
                .WhereIF(!string.IsNullOrEmpty(input.Lot), a => a.Lot.Contains(input.Lot))
                .WhereIF(!string.IsNullOrEmpty(input.SN), a => a.SN.Contains(input.SN))
                .OrderByDescending(a => a.CreationTime)
                .ToPagedListAsync(input.Page, input.PageSize);
        }
        catch (Exception ex)
        {
            throw;
        }
        return wMSRFPackageAcquisitions;
    }
    /// <summary>
    /// 出库序列号导出
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ExportWMSRFPackageAcquisition")]
    public async Task<ActionResult> ExportWMSRFPackageAcquisition(WMSRFPackageAcquisitionInput input)
    {
        List<WMSRFPackageAcquisitionExport> wMSRFPackageAcquisitions = new List<WMSRFPackageAcquisitionExport>();
        IExcelExporter excelExporter = new ExcelExporter();

        try
        {
            wMSRFPackageAcquisitions = await _repRFPackageAcquisition.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(input.ExternOrderNumber), a => a.ExternOrderNumber.Contains(input.ExternOrderNumber))
                .WhereIF(!string.IsNullOrEmpty(input.PreOrderNumber), a => a.PreOrderNumber.Contains(input.PreOrderNumber))
                .WhereIF(!string.IsNullOrEmpty(input.OrderNumber), a => a.OrderNumber.Contains(input.OrderNumber))
                .WhereIF(input.CustomerId != null && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .WhereIF(input.WarehouseId != null && input.WarehouseId > 0, a => a.WarehouseId == input.WarehouseId)
                .WhereIF(!string.IsNullOrEmpty(input.Type), a => a.Type.Contains(input.Type))
                .WhereIF(!string.IsNullOrEmpty(input.SKU), a => a.SKU.Contains(input.SKU))
                .WhereIF(!string.IsNullOrEmpty(input.Lot), a => a.Lot.Contains(input.Lot))
                .WhereIF(!string.IsNullOrEmpty(input.SN), a => a.SN.Contains(input.SN))
                .Select(a => new  WMSRFPackageAcquisitionExport
                {
                    ExternOrderNumber = a.ExternOrderNumber,
                    PreOrderNumber = a.PreOrderNumber,
                    OrderNumber = a.OrderNumber,
                    CustomerName = a.CustomerName,
                    WarehouseName = a.WarehouseName,
                    Type = a.Type,
                    SKU = a.SKU,
                    Lot = a.Lot,
                    SN = a.SN,
                    ProductionDate = a.ProductionDate,
                    ExpirationDate = a.ExpirationDate,
                    CreationTime = a.CreationTime,
                    Creator = a.Creator,
                    Updator = a.Updator,
                    UpdateTime=a.UpdateTime
                })
                .OrderByDescending(a => a.CreationTime)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
        // 使用 IExcelExporter 导出 DataTable 数据
        var res = await excelExporter.ExportAsByteArray(wMSRFPackageAcquisitions);

        return new FileStreamResult(new MemoryStream(res), "application/octet-stream")
        {
            FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmm") + "出库(包装)SN报表.xlsx"
        };
    }
    #endregion

}

