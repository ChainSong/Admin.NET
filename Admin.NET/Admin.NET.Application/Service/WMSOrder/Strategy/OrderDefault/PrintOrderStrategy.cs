// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Dtos.Enum;
using Furion.FriendlyException;

namespace Admin.NET.Application.Service;
public class PrintOrderStrategy : IPrintOrderInterface
{

    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

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

    public PrintOrderStrategy()
    {

    }
    //处理打印发运单
    public async Task<Response<List<WMSOrderPrintDto>>> PrintShippingList(List<long> request)
    {
        Response<List<WMSOrderPrintDto>> response = new Response<List<WMSOrderPrintDto>>();
        //List<WMSOrderPrintDto> result = new List<WMSOrderPrintDto>();
        //获取订单信息
        //List<WMSOrder> orders = await _repOrder.GetListAsync(o => request.Contains(o.Id));
        List<WMSOrder> orders = await _repOrder.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.OrderAddress)
                 .Where(u => request.Contains(u.Id)).ToListAsync();
        if (orders.Select(a => a.CustomerId).Distinct().Count() > 1)
        {
            throw Oops.Oh("请选择同一个客户的订单进行打印！");
        }

        //获取仓库信息
        WMSWarehouse warehouse = await _repWarehouse.AsQueryable().Where(o => o.Id == orders.First().WarehouseId).FirstAsync();
        //获取客户信息
        WMSCustomer customer = await _repCustomer.AsQueryable()
                 .Includes(a => a.Details)
                 .Includes(a => a.CustomerConfig)
                 .Where(o => o.Id == orders.First().CustomerId).FirstAsync();
        //获取客户配置信息
        //WMSCustomerConfig customerConfig = await _re.AsQueryable().Where(o => o.Id == orders.First().CustomerId).FirstAsync();
        try
        {
            List<WMSOrderPrintDto> orderPrintDtos = new List<WMSOrderPrintDto>();
            orderPrintDtos = orders.Adapt<List<WMSOrderPrintDto>>();
            orderPrintDtos.ForEach(order =>
            {
                order.Customer = customer;
                order.Warehouse = warehouse;
                order.CustomerConfig = customer.CustomerConfig;
                order.CustomerDetail = customer.Details.FirstOrDefault();
                order.OrderAddress.Address=order.OrderAddress.Province+order.OrderAddress.City+order.OrderAddress.County+order.OrderAddress.Address;
                int sequence = 0;
                order.Details.ForEach(detail =>
                {
                    sequence++;
                    detail.Sequence = sequence;
                });
            });

            response.Data = orderPrintDtos;
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

}
