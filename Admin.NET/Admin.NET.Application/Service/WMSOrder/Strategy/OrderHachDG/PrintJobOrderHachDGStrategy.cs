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

        WMSOrderPrintCustomerInfo PrintData = new WMSOrderPrintCustomerInfo();
        //查对接表数据
        var Sql = $@"SELECT distinct DeliveryNumber,ContactName as CustomerName,ContactName as CustomerCode,Address,Telephone as ContactPhone FROM hach_wms_outBound 
                            where DeliveryNumber IN (SELECT DeliveryNumber FROM hach_wms_outBound 
                            WHERE ordernumber in( SELECT ExternOrderNumber FROM WMS_Order 
                            WHERE ID={string.Join(",", request)}))";

        PrintData = await _repOb.Context.Ado.SqlQuerySingleAsync<WMSOrderPrintCustomerInfo>(Sql.ToString());

        //查出库信息
        List<Admin.NET.Core.Entity.WMSOrder> orders = await _repOrder.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.OrderAddress)
                 .Where(u => request.Contains(u.Id)).ToListAsync();
        
        if (orders.Select(a => a.CustomerId).Distinct().Count() > 1)
        {
            throw Oops.Oh("请选择同一个客户的订单进行打印！");
        }
        if (orders.Select(a => a.OrderStatus<80).Distinct().Count() > 1)
        {
            throw Oops.Oh("DN下存在未交接的SO,不允许打印!！");
        }

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(orders.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Print_Job_Order, orders.First().OrderType);
        //获取仓库信息
        WMSWarehouse warehouse = await _repWarehouse.AsQueryable().Where(o => o.Id == orders.First().WarehouseId).FirstAsync();
        //获取客户信息
        WMSCustomer customer = await _repCustomer.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.CustomerConfig)
                 .Where(o => o.Id == orders.First().CustomerId).FirstAsync();
        try
        {
            //string SqlDetail = $@"SELECT  distinct p.ExternOrderNumber, o.CompleteTime, od.PoCode, p.PackageNumber,
            //                      CASE  WHEN ISNULL(od.Str2, '') = '' THEN od.SKU ELSE od.Str2 END AS 'SKU',
            //                      -- 母件=1套，普通物料=该箱里该SKU的数量汇总
            //                      CASE WHEN ISNULL(od.Str2, '') = ''  THEN SUM(ISNULL(pd.Qty, 0))    -- 普通物料：按明细数量
            //                      -- 母件：每箱一套
            //                      ELSE 1  END AS Qty,1 AS CombinedBoxesNumber,
            //                      'DGGC' AS Type,
            //                      (SELECT COUNT(Id) FROM WMS_Package WHERE OrderId = o.Id) AS JOBTotalBox FROM WMS_Package p
            //                      LEFT JOIN WMS_PackageDetail pd ON p.Id = pd.PackageId LEFT JOIN WMS_Order o 
            //                      ON p.OrderId = o.Id LEFT JOIN WMS_OrderDetail od 
            //                      ON o.Id = od.OrderId AND od.SKU = pd.SKU
            //                      WHERE p.ExternOrderNumber in
            //                      (SELECT OrderNumber FROM hach_wms_outBound WHERE DeliveryNumber IN (
            //                      SELECT DeliveryNumber FROM hach_wms_outBound WHERE ordernumber in(
            //                      SELECT ExternOrderNumber FROM WMS_Order WHERE ID in ({string.Join(",", request)})))
            //                      )GROUP BY p.ExternOrderNumber,o.CompleteTime,od.PoCode,p.PackageNumber,od.Str2, 
            //                      CASE WHEN ISNULL(od.Str2, '') = '' THEN od.SKU
            //                      ELSE od.Str2 END,o.Id,
            //                      CASE  WHEN ISNULL(od.Str2, '') = '' THEN 0 ELSE 1 END;";

            string SqlDetail = $@"WITH OD AS (SELECT OrderId,SKU,MAX(PoCode) AS PoCode,MAX(Str2) AS Str2,Onwer FROM WMS_OrderDetail GROUP BY OrderId, SKU,Onwer),
                                  PD_SUM AS (SELECT PackageId,SKU,SUM(Qty) AS Qty FROM WMS_PackageDetail GROUP BY PackageId, SKU)
                                  SELECT p.ExternOrderNumber,o.CompleteTime,od.PoCode,p.PackageNumber,
                                  CASE WHEN ISNULL(od.Str2,'')='' THEN od.SKU ELSE od.Str2 END AS SKU,
                                  --CASE WHEN ISNULL(od.Str2,'')='' THEN ISNULL(ps.Qty,0) ELSE 1 END AS Qty,
                                  --CASE WHEN ISNULL(od.Str2,'')='' THEN ISNULL(ps.Qty,0) ELSE ps.qty END AS Qty,
                                  ps.qty  AS Qty,
                                  (CASE WHEN ISNULL(od.Str2,'')='' THEN 0 ELSE 1 END) AS CombinedBoxesNumber,
                                  od.Onwer AS Type,(SELECT COUNT(Id) FROM WMS_Package WHERE OrderId in(
                                  select Id from WMS_Order where dn in(select dn from wms_order where id in ({string.Join(",", request)})))) AS JOBTotalBox
                                  FROM WMS_Order o
                                  LEFT JOIN WMS_Package p ON o.Id = p.OrderId
                                  LEFT JOIN OD od ON o.Id = od.OrderId
                                  LEFT JOIN PD_SUM ps ON ps.PackageId = p.Id AND ps.SKU = od.SKU
                                  WHERE o.Dn in (select dn from wms_order where id in ({string.Join(",", request)}))";

            var details = await _repOb.Context.Ado.SqlQueryAsync<WMSOrderPrintDetail>(SqlDetail.ToString());

            List<WMSOrderJobPrintDto> orderPrintDtos = new List<WMSOrderJobPrintDto>();
            orderPrintDtos = orders.Adapt<List<WMSOrderJobPrintDto>>();
            foreach (var order in orderPrintDtos)
            {
                order.Customer = customer;
                order.Warehouse = warehouse;
                order.CustomerConfig = customer.CustomerConfig;
                order.CustomerDetail = customer.Details.FirstOrDefault();
                order.OrderAddress.Address = order.OrderAddress.Province + order.OrderAddress.City + order.OrderAddress.County + order.OrderAddress.Address;
                order.wMSOrderPrintCustomerInfo = PrintData;
                order.JobDetails = details;
            }

            response.Data = new PrintBase<List<WMSOrderJobPrintDto>>()
            {
                PrintTemplate = string.IsNullOrEmpty(workflow) ? OutboundWorkFlowConst.Workflow_Print_Order : workflow,
                Data = orderPrintDtos
            };
            response.Code = StatusCode.Success;
            response.Msg = "获取成功";
        }
        catch (Exception ex)
        {
            response.Code = StatusCode.Error;
            response.Msg = ex.Message;
        }
        return response;
    }
}

