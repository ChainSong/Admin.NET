// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Service.Enumerate;

namespace Admin.NET.Application.Service;
public class OrderReturnHachStrategy : IOrderReturnInterface
{

    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

    public SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public UserManager _userManager { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

    public SqlSugarRepository<Admin.NET.Core.Entity.WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }

    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }

    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }

    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }

    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public async Task<Response<List<OrderStatusDto>>> OrderReturn(List<DeleteWMSOrderInput> request)
    {


        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
        //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
        //先判断状态是否正常 是否允许回退
        var ids = request.Select(b => b.Id);
        var order = _repOrder.AsQueryable().Where(a => ids.Contains(a.Id));
        await order.ForEachAsync(a =>
        {
            if (a.OrderStatus > (int)OrderStatusEnum.拣货中)
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = a.ExternOrderNumber,
                    SystemOrder = a.OrderNumber,
                    Type = a.OrderType,
                    StatusCode = StatusCode.Warning,
                    Msg = "状态异常"
                });
            }

        });
        if (response.Data != null && response.Data.Count > 0)
        {
            return response;
        }
        //得到分配信息
        var OrderAllocation = await _repOrderAllocation.AsQueryable().Where(b => ids.Contains(b.OrderId)).ToListAsync();

        //先回退分配信息
        var InventoryIds = OrderAllocation.Select(b => b.InventoryId).ToList();

        await _repInventoryUsable.Context.Updateable<WMSInventoryUsable>()
        .SetColumns(p => p.InventoryStatus == (int)InventoryStatusEnum.可用)
        .Where(p => InventoryIds.Contains(p.Id))
        .ExecuteCommandAsync();
        //await _repInventoryUsable.UpdateAsync(a => new WMSInventoryUsable { InventoryStatus = (int)InventoryStatusEnum.可用 }, a => InventoryIds.Contains(a.Id));
        //删除分配信息
        await _repOrderAllocation.DeleteAsync(OrderAllocation);

        //回退分配数量
        //await _repOrderDetail.UpdateAsync(a => new WMSOrderDetail { AllocatedQty = 0 }, a => ids.Contains(a.OrderId));
        await _repOrderDetail.Context.Updateable<WMSOrderDetail>()
        .SetColumns(p => p.AllocatedQty == 0)
        .Where(p => ids.Contains(p.OrderId))
        .ExecuteCommandAsync();

        var PreOrderIds = await order.Select(b => b.PreOrderId).ToListAsync();
        //先更新主表，在更新明细表
        //await _repPreOrder.UpdateAsync(a => new WMSPreOrder { PreOrderStatus = (int)PreOrderStatusEnum.新增 }, (a => PreOrderIds.Contains(a.Id)));

        await _repPreOrder.Context.Updateable<WMSPreOrder>()
      .SetColumns(p => p.PreOrderStatus == (int)PreOrderStatusEnum.新增)
      .Where(p => PreOrderIds.Contains(p.Id))
      .ExecuteCommandAsync();

        await _repPreOrderDetail.AsUpdateable()
       .SetColumns(p => p.ActualQty == 0)
       .SetColumns(p => p.Updator == _userManager.Account)
       .SetColumns(p => p.UpdateTime == DateTime.Now)
       .Where(p => PreOrderIds.Contains(p.PreOrderId))
       .ExecuteCommandAsync();


        //await _repPreOrderDetail.UpdateAsync(a => new WMSPreOrderDetail
        //{
        //    ActualQty = 0,
        //    Updator = _userManager.Account,
        //    UpdateTime = DateTime.Now
        //}, a => PreOrderIds.Contains(a.PreOrderId));

        //先删除明细表，再删除主表
        await _repOrderDetail.DeleteAsync(a => ids.Contains(a.OrderId));
        await _repOrder.DeleteAsync(a => ids.Contains(a.Id));

        //_wms_asndetailRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(c =>
        //{
        //    c.ReceiptQty = 0;
        //    _wms_asndetailRepository.Update(c);
        //}); 
        //先处理上架=>入库单

        await order.ForEachAsync(a =>
        {
            response.Data.Add(new OrderStatusDto()
            {
                ExternOrder = a.ExternOrderNumber,
                SystemOrder = a.OrderNumber,
                Type = a.OrderType,
                StatusCode = StatusCode.Success,
                Msg = "操作成功"
            });
        });
        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        //throw new NotImplementedException();
        return response;
    }
}
