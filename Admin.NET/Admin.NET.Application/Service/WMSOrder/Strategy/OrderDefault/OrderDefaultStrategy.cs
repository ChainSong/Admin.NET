
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
    public class OrderDefaultStrategy : IOrderInterface
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

        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }


        public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
        public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

        public OrderDefaultStrategy()
        {

        }

        //处理分配业务
        public async Task<Response<List<OrderStatusDto>>> CompleteOrder(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var orderData = _repOrder.AsQueryable().Includes(a => a.Allocation).Includes(a=>a.OrderAddress).Where(a => request.Contains(a.Id)).ToList();
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
                    wMSInstruction.InstructionType = "HACH出库反馈";
                    wMSInstruction.BusinessType = "HACH出库反馈";
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
                //}
                if (item.OrderAddress.CompanyType == "分销商" && item.CustomerName=="哈希")
                {
                    //插入反馈指令
                    WMSInstruction wMSInstructionIssue = new WMSInstruction();
                    //wMSInstruction.OrderId = orderData[0].Id;
                    wMSInstructionIssue.InstructionStatus = (int)InstructionStatusEnum.新增;
                    wMSInstructionIssue.InstructionType = "HACH出库同步下发";
                    wMSInstructionIssue.BusinessType = "HACH出库同步下发"; 
                    wMSInstructionIssue.OrderNumber =item.ExternOrderNumber; 
                    //wMSInstruction.InstructionTaskNo = DateTime.Now;
                    wMSInstructionIssue.CustomerId = item.CustomerId;
                    wMSInstructionIssue.CustomerName = item.CustomerName;
                    wMSInstructionIssue.WarehouseId = item.WarehouseId;
                    wMSInstructionIssue.WarehouseName = item.WarehouseName;
                    wMSInstructionIssue.OperationId = item.Id;
                    wMSInstructionIssue.InstructionTaskNo = item.ExternOrderNumber;
                    wMSInstructionIssue.Creator = _userManager.Account;
                    wMSInstructionIssue.CreationTime = DateTime.Now;
                    wMSInstructionIssue.InstructionTaskNo = item.ExternOrderNumber;
                    wMSInstructionIssue.TableName = "WMS_Order";
                    wMSInstructionIssue.InstructionPriority = 0;
                    wMSInstructionIssue.Remark = "";
                    wMSInstructions.Add(wMSInstructionIssue);
                }
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
