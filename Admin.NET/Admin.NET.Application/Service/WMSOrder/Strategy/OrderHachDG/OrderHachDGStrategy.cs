
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using AutoMapper;
using NewLife.Net;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Application.Service.Enumerate;
using Org.BouncyCastle.Asn1.Cmp;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;

namespace Admin.NET.Application.Strategy
{
    public class OrderHachDGStrategy : IOrderInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }
        public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSHandover> _repHandover { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }


        public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
        public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

        public OrderHachDGStrategy()
        {

        }

        //完成
        public async Task<Response<List<OrderStatusDto>>> CompleteOrder(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var orderData = await _repOrder.AsQueryable().Includes(a => a.Allocation).Includes(a => a.OrderAddress).Where(a => request.Contains(a.Id)).ToListAsync();
            if (orderData != null && orderData.Where(a => a.OrderStatus < (int)OrderStatusEnum.已分配 || a.OrderStatus == (int)OrderStatusEnum.完成).ToList().Count > 0)
            {
                orderData.ToList().ForEach(b =>
                {
                    if (b.OrderStatus < (int)OrderStatusEnum.已分配 || b.OrderStatus == (int)OrderStatusEnum.完成)
                        response.Data.Add(new OrderStatusDto()
                        {
                            Id = b.Id,
                            ExternOrder = b.ExternOrderNumber,
                            SystemOrder = b.PreOrderNumber,
                            Type = b.OrderType,
                            StatusCode = StatusCode.Warning,
                            //StatusMsg = (string)StatusCode.warning,
                            Msg = "订单状态异常"
                        });
                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }
            //判断有没有包装信息
            var package = await _repPickTaskDetail.AsQueryable().Where(a => request.Contains(a.OrderId)).ToListAsync();
            if (package.Count() == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "订单异常:没有拣货信息";
                return response;
            }
            foreach (var item in request)
            {
                if (package.Where(a => a.PickStatus != (int)PickTaskStatusEnum.包装完成).Count() > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常:没有完成包装信息";
                    return response;
                }
            }

            foreach (var item in orderData)
            {
                //判断是不是都已经交接
                var handover = await _repHandover.AsQueryable().Where(a => request.Contains(item.Id)).ToListAsync();
                if (handover == null || handover.Count == 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常:+" + item.ExternOrderNumber + "没有完成交接信息";
                    return response;
                }
            }


            await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.完成, CompleteTime = DateTime.Now }, a => orderData.Select(c => c.Id).Contains(a.Id));
            //await _repOrderDetail.UpdateAsync(a => new WMSOrderDetail { OrderStatus = (int)OrderStatusEnum.完成, CompleteTime=DateTime.Now }, a => orderData.Select(c => c.Id).Contains(a.Id));

            await _repPreOrder.UpdateAsync(a => new WMSPreOrder { PreOrderStatus = (int)PreOrderStatusEnum.完成, CompleteTime = DateTime.Now }, a => orderData.Select(c => c.PreOrderId).Contains(a.Id));
            //_repPickTask.AsQueryable().Includes(a => a.Detail).Where(a => request.Contains(a.Id)).ToList();
            //await _repOrder.Context.InsertNav(pickTasks).Include(a => a.Details).ExecuteCommandAsync();
            List<WMSInstruction> wMSInstructions = new List<WMSInstruction>();
            foreach (var item in orderData)
            {

                //if (item.CustomerName != "哈希")
                //{
                //插入反馈指令
                WMSInstruction wMSInstruction = new WMSInstruction();
                //wMSInstruction.OrderId = orderData[0].Id;
                wMSInstruction.InstructionStatus = (int)InstructionStatusEnum.新增;
                wMSInstruction.InstructionType = "出库单回传HachDG";
                wMSInstruction.BusinessType = "出库单回传HachDG";
                //wMSInstruction.InstructionTaskNo = DateTime.Now;
                wMSInstruction.CustomerId = item.CustomerId;
                wMSInstruction.CustomerName = item.CustomerName;
                wMSInstruction.WarehouseId = item.WarehouseId;
                wMSInstruction.WarehouseName = item.WarehouseName;
                wMSInstruction.OperationId = item.Id;
                wMSInstruction.OrderNumber = item.ExternOrderNumber;
                wMSInstruction.Creator = _userManager.Account;
                wMSInstruction.CreationTime = DateTime.Now;
                wMSInstruction.InstructionTaskNo = item.ExternOrderNumber;
                wMSInstruction.TableName = "WMS_Order";
                wMSInstruction.InstructionPriority = 0;
                wMSInstruction.Remark = "";
                wMSInstructions.Add(wMSInstruction);

                WMSInstruction wMSInstructionGRHach = new WMSInstruction();
                //wMSInstruction.OrderId = orderData[0].Id;
                wMSInstructionGRHach.InstructionStatus = (int)InstructionStatusEnum.新增;
                wMSInstructionGRHach.InstructionType = "出库单防伪码回传HachDG";
                wMSInstructionGRHach.BusinessType = "出库单防伪码回传HachDG";
                wMSInstructionGRHach.CustomerId = item.CustomerId;
                wMSInstructionGRHach.CustomerName = item.CustomerName;
                wMSInstructionGRHach.WarehouseId = item.WarehouseId;
                wMSInstructionGRHach.WarehouseName = item.WarehouseName;
                wMSInstructionGRHach.OperationId = item.Id;
                wMSInstructionGRHach.OrderNumber = item.ExternOrderNumber;
                wMSInstructionGRHach.Creator = _userManager.Account;
                wMSInstructionGRHach.CreationTime = DateTime.Now;
                wMSInstructionGRHach.InstructionTaskNo = item.ExternOrderNumber;
                wMSInstructionGRHach.TableName = "WMS_Order";
                wMSInstructionGRHach.InstructionPriority = 1;
                wMSInstructionGRHach.Remark = "";
                wMSInstructions.Add(wMSInstructionGRHach);



                WMSInstruction wMSInstructionAFCGRHach = new WMSInstruction();
                //wMSInstruction.OrderId = orderData[0].Id;
                wMSInstructionAFCGRHach.InstructionStatus = (int)InstructionStatusEnum.新增;
                wMSInstructionAFCGRHach.InstructionType = "出库单序列号回传HachDG";
                wMSInstructionAFCGRHach.BusinessType = "出库单序列号回传HachDG";
                wMSInstructionAFCGRHach.CustomerId = item.CustomerId;
                wMSInstructionAFCGRHach.CustomerName = item.CustomerName;
                wMSInstructionAFCGRHach.WarehouseId = item.WarehouseId;
                wMSInstructionAFCGRHach.WarehouseName = item.WarehouseName;
                wMSInstructionAFCGRHach.OperationId = item.Id;
                wMSInstructionAFCGRHach.OrderNumber = item.ExternOrderNumber;
                wMSInstructionAFCGRHach.Creator = _userManager.Account;
                wMSInstructionAFCGRHach.CreationTime = DateTime.Now;
                wMSInstructionAFCGRHach.InstructionTaskNo = item.ExternOrderNumber;
                wMSInstructionAFCGRHach.TableName = "WMS_Order";
                wMSInstructionAFCGRHach.InstructionPriority = 1;
                wMSInstructionAFCGRHach.Remark = "";
                wMSInstructions.Add(wMSInstructionAFCGRHach);
                WMSInstruction wMSInstructionSNGRHach = new WMSInstruction();
                //wMSInstruction.OrderId = orderData[0].Id;
                wMSInstructionSNGRHach.InstructionStatus = (int)InstructionStatusEnum.新增;
                wMSInstructionSNGRHach.InstructionType = "出库装箱回传HachDG";
                wMSInstructionSNGRHach.BusinessType = "出库装箱回传HachDG";
                //wMSInstruction.InstructionTaskNo = DateTime.Now;
                wMSInstructionSNGRHach.CustomerId = item.CustomerId;
                wMSInstructionSNGRHach.CustomerName = item.CustomerName;
                wMSInstructionSNGRHach.WarehouseId = item.WarehouseId;
                wMSInstructionSNGRHach.WarehouseName = item.WarehouseName;
                wMSInstructionSNGRHach.OperationId = item.Id;
                wMSInstructionSNGRHach.OrderNumber = item.ExternOrderNumber;
                wMSInstructionSNGRHach.Creator = _userManager.Account;
                wMSInstructionSNGRHach.CreationTime = DateTime.Now;
                wMSInstructionSNGRHach.InstructionTaskNo = item.ExternOrderNumber;
                wMSInstructionSNGRHach.TableName = "WMS_Order";
                wMSInstructionSNGRHach.InstructionPriority = 99;
                wMSInstructionSNGRHach.Remark = "";
                wMSInstructions.Add(wMSInstructionSNGRHach);

            }
            if (wMSInstructions.Count > 0)
            {
                await _repInstruction.InsertRangeAsync(wMSInstructions);
            }
            //获取分配的库存ID

            var InventoryIds = _repOrderAllocation.AsQueryable().Where(a => request.Contains(a.OrderId)).Select(a => a.InventoryId).ToList();

            await _repInventoryUsable.UpdateAsync(a => new WMSInventoryUsable { InventoryStatus = (int)InventoryStatusEnum.出库, UpdateTime = DateTime.Now }, a => InventoryIds.Contains(a.Id));

            var orderResule = _repOrder.AsQueryable().Where(a => request.Contains(a.Id)).ToList();

            orderResule.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternOrderNumber,
                    SystemOrder = b.OrderNumber,
                    Type = b.OrderType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.Success.ToString(),
                    Msg = "订单已完成"
                }); ;
            });

            //开始校验数据
            //_wms_preorderRepository.GetDbContext().BulkInsert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;

        }
    }
}
