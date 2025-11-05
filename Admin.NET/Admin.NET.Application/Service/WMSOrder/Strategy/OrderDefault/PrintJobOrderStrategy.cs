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
    public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
    public PrintJobOrderStrategy()
    {
    }
    //处理打印发运单
    public async Task<Response<PrintBase<List<WMSOrderJobPrintDto>>>> PrintJobList(List<long> request)
    {
        Response<PrintBase<List<WMSOrderJobPrintDto>>> response = new Response<PrintBase<List<WMSOrderJobPrintDto>>>();
        //查出库信息
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
            List<WMSOrderJobPrintDto> orderPrintDtos = new List<WMSOrderJobPrintDto>();
            orderPrintDtos = orders.Adapt<List<WMSOrderJobPrintDto>>();
            foreach (var order in orderPrintDtos)
            {
                order.Customer = customer;
                order.Warehouse = warehouse;
                order.CustomerConfig = customer.CustomerConfig;
                order.CustomerDetail = customer.Details.FirstOrDefault();
                order.OrderAddress.Address = order.OrderAddress.Province + order.OrderAddress.City + order.OrderAddress.County + order.OrderAddress.Address;
                // 获取包装信息
                order.PackageInfo = await GetPackageSingleInfoAsync(order);
                // 获取HACH出库对接信息表
                order.outBound = await GetHachObInfo(order);
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
    private async Task<HachWmsOutBound> GetHachObInfo(WMSOrderJobPrintDto order)
    {
        HachWmsOutBound hachWmsOutBound = new HachWmsOutBound();
        if (order != null)
        {
            hachWmsOutBound = await _repOb.AsQueryable()
                              .Where(a => a.OrderNumber == order.ExternOrderNumber)
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
    private async Task<WMSOrderPrintJobPackageDto> GetPackageSingleInfoAsync(WMSOrderJobPrintDto order, string sort = "Desc")
    {
        // 参数验证
        if (order == null)
            throw new ArgumentNullException(nameof(order));

        // 获取包裹信息
        var package = await GetPackageByOrderAsync(order, sort);
        if (package == null)
            return new WMSOrderPrintJobPackageDto();

        // 构建返回结果
        return await BuildPackageDto(package, order);
    }

    private async Task<WMSPackage> GetPackageByOrderAsync(Admin.NET.Core.Entity.WMSOrder order, string sort)
    {
        try
        {
            // 构建查询条件
            var query = _repPackage.AsQueryable();

            if (order.Id > 0)
                query = query.Where(a => a.OrderId == order.Id);
            else if (!string.IsNullOrEmpty(order.ExternOrderNumber))
                query = query.Where(a => a.ExternOrderNumber == order.ExternOrderNumber);
            else
                return null; // 如果没有有效条件，直接返回null

            // 排序并获取第一条记录
            if (sort.Equals("Desc", StringComparison.OrdinalIgnoreCase))
                query = query.OrderByDescending(a => a.CreationTime);
            else
                query = query.OrderBy(a => a.CreationTime);

            return await query.Includes(a => a.Details).FirstAsync();
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task<WMSOrderPrintJobPackageDto> BuildPackageDto(WMSPackage package, Admin.NET.Core.Entity.WMSOrder order)
    {
        var packageDto = new WMSOrderPrintJobPackageDto
        {
            Id = package.Id,
            OId = package.OrderId,
            PackageNumber = package.PackageNumber,
            ExternOrderNumber = package.ExternOrderNumber,
            OrderNumber = package.OrderNumber,
            ExpressNumber = package.ExpressNumber,
            ExpressCompany = package.ExpressCompany,
            Details = await BuildPackageDetailsAsync(package, order) // 使用异步版本
        };

        return packageDto;
    }

    private async Task<List<WMSOrderPrintJobPackageDetailDto>> BuildPackageDetailsAsync(WMSPackage package, Admin.NET.Core.Entity.WMSOrder order)
    {
        if (package== null || !package.Details.Any())
            return new List<WMSOrderPrintJobPackageDetailDto>();

        var totalCount = package.Details.Count;
        var details = new List<WMSOrderPrintJobPackageDetailDto>();
        var sequenceIndex = 0;

        foreach (var item in package.Details)
        {
            var combinationQty = await CalculateCombinationQty(item);

            details.Add(new WMSOrderPrintJobPackageDetailDto
            {
                Sequence = sequenceIndex++,
                PackageId = item.PackageId,
                PackageNumber = item.PackageNumber,
                CompleteTime = order.CompleteTime,
                PoCode = item.PoCode,
                BoxCode = item.PackageNumber,
                SKU = item.SKU,
                AllocatedQty = item.Qty,
                CombinationQty = combinationQty,
                Onwer = item.Onwer,
                JobTotalQty = totalCount
            });
        }

        return details;
    }
    /// <summary>
    /// 计算组合数量
    /// </summary>
    /// <returns></returns>
    private async Task<double> CalculateCombinationQty(WMSPackageDetail detail)
    {
        if (string.IsNullOrEmpty(detail.SKU))
            return 0;

        try
        {
            // 并行查询父SKU和子SKU
            var parentTask = _repProductBom.AsQueryable()
                .Where(a => a.CustomerId == detail.CustomerId && a.SKU == detail.SKU)
                .Select(x => x.Qty)
                .ToListAsync();

            var childTask = _repProductBom.AsQueryable()
                .Where(a => a.CustomerId == detail.CustomerId && a.ChildSKU == detail.SKU)
                .Select(x => x.Qty)
                .ToListAsync();

            await Task.WhenAll(parentTask, childTask);

            var parentQtys = await parentTask;
            var childQtys = await childTask;

            // 计算组合数量
            if (parentQtys.Any())
            {
                var totalParentQty = parentQtys.Sum();
                return totalParentQty > 0 ? detail.Qty % totalParentQty : 0;
            }

            if (childQtys.Any())
            {
                var totalChildQty = childQtys.Sum();
                return totalChildQty > 0 ? detail.Qty % totalChildQty : 0;
            }

            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}
