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
        
        var Sql = $@"SELECT distinct dn as DeliveryNumber,od.Name as CustomerName,od.Name as CustomerCode,Address,od.Phone as ContactPhone FROM WMS_Order o
                      left join WMS_OrderAddress od on o.ExternOrderNumber = od.ExternOrderNumber
                      WHERE o.Dn in (select dn from wms_order where id in ({string.Join(",", request)}))";
                      
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
            string SqlDetail = $@";WITH T AS (Select a.PackageNumber, Max(a.packageTime) as CompleteTime,Max(a.pocode) as PoCode,sku,sum(qty) as OrderQty,
                                   isnull((select sum(qty) from wms_productBom b where sku=a.sku and b.CustomerId = a.CustomerId),1) as SkuQty,CombinedBoxesNumber,OrderType
                                   from (select p.PackageNumber, p.packageTime, od.pocode,
                                   CASE WHEN ISNULL(od.Str2,'')='' THEN od.SKU ELSE od.Str2 END AS SKU,
                                   CASE WHEN ISNULL(od.Str2,'')='' THEN ISNULL(od.AllocatedQty,0) ELSE od.AllocatedQty END AS Qty,
                                   CASE WHEN ISNULL(od.Str2,'')='' THEN 0 ELSE 1 END AS CombinedBoxesNumber,
                                   p.customerId,od.Onwer AS OrderType from wms_package p 
                                   left join wms_order o on p.orderid=o.id
                                   left join wms_packagedetail pd on p.id = pd.packageid
                                   left join wms_orderdetail od  on p.orderid=od.orderid and od.orderid = pd.orderid   and pd.sku = od.sku
                                   where o.dn in (select dn from wms_order where id in ({string.Join(",", request)}))) a
                                   group by PackageNumber, a.pocode,sku, CombinedBoxesNumber,OrderType,CustomerId),
                                   BOX AS (SELECT COUNT(DISTINCT PackageNumber) AS JOBTotalBox FROM T )
                                   SELECT t.PackageNumber,t.CompleteTime,t.PoCode,t.SKU,FLOOR(t.OrderQty / NULLIF(t.SkuQty,0)) AS qty,
                                   t.OrderType AS Type,t.CombinedBoxesNumber,b.JOBTotalBox FROM T t CROSS JOIN BOX b;
                                   ";

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

