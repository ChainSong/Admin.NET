// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using XAct;
using RulesEngine.Models;

namespace Admin.NET.Application.Service;
public class PrintJobOrderHachDGStrategy : IPrintJobOrderStrategy
{
    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }
    public SysWorkFlowService _repWorkFlowService { get; set; }
    public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public UserManager _userManager { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WMSCustomer> _repCustomer { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<HachWmsOutBound> _repOb { get; set; }
    public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
    public PrintJobOrderHachDGStrategy()
    {
    }
    //处理打印发运单
    public async Task<Response<PrintBase<List<WMSOrderJobPrintDto>>>> PrintJobList(List<long> request)
    {
        Response<PrintBase<List<WMSOrderJobPrintDto>>> response = new Response<PrintBase<List<WMSOrderJobPrintDto>>>();
        string workflow = string.Empty;
        WMSOrderPrintCustomerInfo PrintData = new WMSOrderPrintCustomerInfo();

        List<WMSOrderPrintCustomerInfo> PrintList = new List<WMSOrderPrintCustomerInfo>();

        List<WMSOrderJobPrintDto> resultList = new List<WMSOrderJobPrintDto>();
        //查对接表数据

        var Sql = $@"SELECT distinct dn as DeliveryNumber,od.Name as CustomerName,od.Name as CustomerCode,Address,od.Phone as ContactPhone FROM WMS_Order o
                      left join WMS_OrderAddress od on o.ExternOrderNumber = od.ExternOrderNumber
                      WHERE o.Dn in (select dn from wms_order where id in ({string.Join(",", request)}))";
        PrintList = await _repOb.Context.Ado.SqlQueryAsync<WMSOrderPrintCustomerInfo>(Sql.ToString());

        if (PrintList==null || PrintList.Count==0)
        {
            throw Oops.Oh("获取dn数据繁忙！");
        }

       
        foreach (var item in PrintList)
        {
            WMSOrderJobPrintDto result = new WMSOrderJobPrintDto();

            //查出库信息
            List<WMSOrder> orders = await _repOrder.AsQueryable()
                     .Includes(a => a.Details)
                     .Includes(a => a.OrderAddress)
                     .Where(a=>a.Dn == item.DeliveryNumber)
                     .ToListAsync();

            var firstOrder = orders.FirstOrDefault();
            if (firstOrder == null)
            {
                throw Oops.Oh($"DN:{item.DeliveryNumber} 未找到订单信息，无法打印");
            }

            var oa = firstOrder.OrderAddress;
            if (oa == null)
            {
                // 如果地址必须有，直接抛错
                throw Oops.Oh($"DN:{item.DeliveryNumber} 订单缺少地址信息，无法打印");

                // 地址可选，给空对象
                // oa = new WMSOrderAddress();
            }

            // 确保 result.OrderAddress 不为空
            result.OrderAddress ??= new WMSOrderAddress(); // 如果是 DTO 类型，请替换

            if (orders.Select(a => a.CustomerId).Distinct().Count() > 1)
            {
                throw Oops.Oh("请选择同一个客户的订单进行打印！");
            }
            if (orders.Select(a => a.OrderStatus < 80).Distinct().Count() > 1)
            {
                var so = orders.Where(a => a.OrderStatus < 80).Distinct().Select(a => a.ExternOrderNumber).FirstOrDefault();
                throw Oops.Oh($"DN:{item.DeliveryNumber} 下存在未交接的SO:{so},不允许打印!！");
            }

            //获取仓库信息
            WMSWarehouse warehouse = await _repWarehouse.AsQueryable().Where(o => o.Id == orders.First().WarehouseId).FirstAsync();

            //获取客户信息
            WMSCustomer customer = await _repCustomer.AsQueryable()
                     .Includes(a => a.Details)
                     .Includes(a => a.CustomerConfig)
                     .Where(o => o.Id == orders.First().CustomerId).FirstAsync();

             workflow = await _repWorkFlowService.GetSystemWorkFlow(orders.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Print_Job_Order, orders.First().OrderType);
            try
            {
                string SqlDetail = $@"WITH PD AS (
                               SELECT p.PackageNumber,MAX(p.PackageTime) AS CompleteTime,p.CustomerId, pd.SKU, SUM(pd.Qty) AS PackQty
                               FROM wms_package p
                               INNER JOIN wms_packagedetail pd ON p.Id = pd.PackageId INNER JOIN wms_order o ON p.OrderId = o.Id WHERE o.DN in ({item.DeliveryNumber})
                               GROUP BY p.PackageNumber, p.CustomerId, pd.SKU),
                               OD AS (SELECT o.Id AS OrderId,MAX(od.PoCode) AS PoCode,
                               MAX(od.Onwer) AS OrderType FROM wms_order o
                               LEFT JOIN wms_orderdetail od ON o.Id = od.OrderId WHERE o.DN in ({item.DeliveryNumber}) GROUP BY o.Id),
                               T AS (
                               SELECT pd.PackageNumber,pd.CompleteTime,od.PoCode,CASE WHEN ISNULL(od2.Str2,'')='' THEN pd.SKU ELSE od2.Str2 END AS SKU,
                               pd.PackQty AS OrderQty,ISNULL((SELECT SUM(qty) FROM wms_productBom b 
                               WHERE b.sku = pd.SKU AND b.CustomerId = pd.CustomerId ),1) AS SkuQty,
                               CASE WHEN ISNULL(od2.Str2,'')='' THEN 0 ELSE 1 END AS CombinedBoxesNumber,
                               od.OrderType
                               FROM PD pd
                               CROSS APPLY ( SELECT TOP 1 od.Str2 FROM wms_orderdetail od INNER JOIN wms_order o ON od.OrderId = o.Id WHERE o.DN in ({item.DeliveryNumber}) 
                               AND od.SKU = pd.SKU) od2
                               CROSS APPLY (
                               SELECT TOP 1 o.Id AS OrderId FROM wms_order o WHERE o.DN in ({item.DeliveryNumber}))   o1 LEFT JOIN OD od ON od.OrderId = o1.OrderId),
                               BOX AS (SELECT COUNT(DISTINCT PackageNumber) AS JOBTotalBox FROM T)
                               SELECT t.PackageNumber,t.CompleteTime,t.PoCode,
                               t.SKU,FLOOR(t.OrderQty / NULLIF(t.SkuQty,0)) AS qty,t.OrderType AS Type,t.CombinedBoxesNumber, b.JOBTotalBox FROM T t CROSS JOIN BOX b
                               ORDER BY t.PackageNumber, t.SKU; ";

                 var details = await _repOb.Context.Ado.SqlQueryAsync<WMSOrderPrintDetail>(SqlDetail.ToString());

                result= item.Adapt<WMSOrderJobPrintDto>();
                result.Customer = customer;
                result.Warehouse = warehouse;
                result.CustomerConfig=customer.CustomerConfig;
                result.CustomerDetail=customer.Details.FirstOrDefault();
                result.OrderAddress = oa;
                result.OrderAddress.Address =  $"{oa?.Province ?? ""}{oa?.City ?? ""}{oa?.County ?? ""}{oa?.Address ?? ""}";
                result.wMSOrderPrintCustomerInfo = item;
                result.JobDetails = details;
            }
            catch (Exception ex)
            {
                throw;
            }
            resultList.Add(result);
        }

        response.Data = new PrintBase<List<WMSOrderJobPrintDto>>()
        {
            PrintTemplate = string.IsNullOrEmpty(workflow) ? OutboundWorkFlowConst.Workflow_Print_Order : workflow,
            Data = resultList
        };
        response.Code = StatusCode.Success;
        response.Msg = "获取成功";
        return response;
    }
}

