// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service.WMSOrder.Interface;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSOrder.Strategy.OrderDefault;
public class PrintJobOrderStrategy : IPrintJobOrderStrategy
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
    public SqlSugarRepository<Admin.NET.Core.Entity.WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<HachWmsOutBound> _repOb { get; set; }

    public PrintJobOrderStrategy()
    {
    }
    //处理打印发运单
    public async Task<Response<PrintBase<List<WMSOrderPrintDto>>>> PrintJobList(List<long> request)
    {
        Response<PrintBase<List<WMSOrderPrintDto>>> response = new Response<PrintBase<List<WMSOrderPrintDto>>>();
        List<Admin.NET.Core.Entity.WMSOrder> orders = await _repOrder.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.OrderAddress)
                 .Where(u => request.Contains(u.Id)).ToListAsync();
        if (orders.Select(a => a.CustomerId).Distinct().Count() > 1)
        {
            throw Oops.Oh("请选择同一个客户的订单进行打印！");
        }
        var workflow = await _repWorkFlowService.GetSystemWorkFlow(orders.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Print_Order, orders.First().OrderType);
        //获取仓库信息
        WMSWarehouse warehouse = await _repWarehouse.AsQueryable().Where(o => o.Id == orders.First().WarehouseId).FirstAsync();
        //获取客户信息
        WMSCustomer customer = await _repCustomer.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.CustomerConfig)
                 .Where(o => o.Id == orders.First().CustomerId).FirstAsync();
        try
        {
            List<WMSOrderPrintDto> orderPrintDtos = new List<WMSOrderPrintDto>();
            orderPrintDtos = orders.Adapt<List<WMSOrderPrintDto>>();
            orderPrintDtos.ForEach(async order =>
            {
                order.Customer = customer;
                order.Warehouse = warehouse;
                order.CustomerConfig = customer.CustomerConfig;
                order.CustomerDetail = customer.Details.FirstOrDefault();
                order.OrderAddress.Address = order.OrderAddress.Province + order.OrderAddress.City + order.OrderAddress.County + order.OrderAddress.Address;
                //包装信息
                order.package = await GetPackageSingleInfo(order.Id, order.ExternOrderNumber, "Desc");
                //获取HACH出库对接信息表
                order.outBound = await GetHachObInfo(order.Id, order.ExternOrderNumber);
                int sequence = 0;
                order.Details.ForEach(detail =>
                {
                    sequence++;
                    detail.Sequence = sequence;
                    detail.CompleteTime = order.CompleteTime;//完成时间
                    detail.JobTotalQty=order.package.TotalCount;//JOB总箱数

                });
            });

            response.Data = new PrintBase<List<WMSOrderPrintDto>>()
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
            //response.Exception = ex.Message;
        }
        return response;
    }

    /// <summary>
    /// 获取HACH出库对接信息表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="OrderNumber"></param>
    /// <returns></returns>
    private async Task<HachWmsOutBound> GetHachObInfo(long? id = 0, string? OrderNumber = null)
    {
        HachWmsOutBound hachWmsOutBound = new HachWmsOutBound();
        if (id > 0 || !string.IsNullOrEmpty(OrderNumber))
        {
            hachWmsOutBound = await _repOb.AsQueryable()
         .WhereIF(id > 0, a => a.Id == id)
         .WhereIF(!string.IsNullOrEmpty(OrderNumber), a => a.OrderNumber == OrderNumber)
         .Includes(a => a.items)
         .FirstAsync();
        }
        return hachWmsOutBound;
    }
    /// <summary>
    /// 获取包装信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="OrderNumber"></param>
    /// <param name="Sort"></param>
    /// <returns></returns>
    private async Task<WMSOrderPackageDto> GetPackageSingleInfo(long? id = 0, string? OrderNumber = null, string Sort = "Desc")
    {
        WMSOrderPackageDto wMSPackage = new WMSOrderPackageDto();

        if (id > 0 || !string.IsNullOrEmpty(OrderNumber))
        {
            wMSPackage.package = await _repPackage.AsQueryable()
                                 .WhereIF(id > 0, a => a.Id == id)
                                 .WhereIF(!string.IsNullOrEmpty(OrderNumber), a => a.ExternOrderNumber == OrderNumber)
                                 .OrderByIF(Sort.Equals("Desc"), a => a.CreationTime, OrderByType.Desc)
                                 .ToListAsync();
        }

        wMSPackage.TotalCount = wMSPackage.package.Count;

        return wMSPackage;
    }
}
