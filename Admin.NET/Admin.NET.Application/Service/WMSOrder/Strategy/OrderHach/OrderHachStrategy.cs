
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
    public class OrderHachStrategy : IOrderInterface
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
        public SqlSugarRepository<WMSHandover> _repHandover { get; set; }
        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }


        public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
        public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
        public SqlSugarRepository<WMSPackage> _repPackage { get; set; }

        public OrderHachStrategy()
        {

        }

        //处理分配业务
        public async Task<Response<List<OrderStatusDto>>> CompleteOrder(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var orderData = _repOrder.AsQueryable().Includes(a => a.Allocation).Includes(a => a.OrderAddress).Where(a => request.Contains(a.Id)).ToList();
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
            var package = await _repPackage.AsQueryable().Where(a => request.Contains(a.OrderId)).ToListAsync();
            //foreach (var item in request)
            //{
            //    if (package.Where(a => a.OrderId == item).Count() == 0)
            //    {
            //        response.Code = StatusCode.Error;
            //        response.Msg = "订单异常:没有完成包装信息";
            //        return response;
            //    }
            //}

            await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.完成, CompleteTime = DateTime.Now }, a => orderData.Select(c => c.Id).Contains(a.Id));
            //await _repOrderDetail.UpdateAsync(a => new WMSOrderDetail { OrderStatus = (int)OrderStatusEnum.完成, CompleteTime=DateTime.Now }, a => orderData.Select(c => c.Id).Contains(a.Id));

            await _repPreOrder.UpdateAsync(a => new WMSPreOrder { PreOrderStatus = (int)PreOrderStatusEnum.完成, CompleteTime = DateTime.Now }, a => orderData.Select(c => c.PreOrderId).Contains(a.Id));
            //_repPickTask.AsQueryable().Includes(a => a.Detail).Where(a => request.Contains(a.Id)).ToList();
            //await _repOrder.Context.InsertNav(pickTasks).Include(a => a.Details).ExecuteCommandAsync();
            List<WMSInstruction> wMSInstructions = new List<WMSInstruction>();
            foreach (var item in orderData)
            {

                //if (item.CustomerName != "哈希" )
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
                wMSInstruction.Creator = _userManager.Account;
                wMSInstruction.CreationTime = DateTime.Now;
                wMSInstruction.InstructionTaskNo = item.ExternOrderNumber;
                wMSInstruction.TableName = "WMS_Order";
                wMSInstruction.InstructionPriority = 0;
                wMSInstruction.Remark = "";
                wMSInstructions.Add(wMSInstruction);
                //}item.OrderAddress.CompanyType == "分销商" &&
                if (item.CustomerName == "哈希")
                {
                    //插入反馈指令
                    WMSInstruction wMSInstructionIssue = new WMSInstruction();
                    //wMSInstruction.OrderId = orderData[0].Id;
                    wMSInstructionIssue.InstructionStatus = (int)InstructionStatusEnum.新增;
                    wMSInstructionIssue.InstructionType = "HACH出库同步下发";
                    wMSInstructionIssue.BusinessType = "HACH出库同步下发";
                    //wMSInstruction.InstructionTaskNo = DateTime.Now;
                    wMSInstructionIssue.OrderNumber = item.ExternOrderNumber;

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


                    //if (item.CustomerName != "哈希")
                    //{
                    //插入反馈指令
                    //WMSInstruction wMSInstructionGI = new WMSInstruction();
                    ////wMSInstruction.OrderId = orderData[0].Id;
                    //wMSInstructionGI.InstructionStatus = (int)InstructionStatusEnum.新增;
                    //wMSInstructionGI.InstructionType = "出库单回传HachDG";
                    //wMSInstructionGI.BusinessType = "出库单回传HachDG";
                    ////wMSInstruction.InstructionTaskNo = DateTime.Now;
                    //wMSInstructionGI.CustomerId = item.CustomerId;
                    //wMSInstructionGI.CustomerName = item.CustomerName;
                    //wMSInstructionGI.WarehouseId = item.WarehouseId;
                    //wMSInstructionGI.WarehouseName = item.WarehouseName;
                    //wMSInstructionGI.OperationId = item.Id;
                    //wMSInstructionGI.OrderNumber = item.ExternOrderNumber;
                    //wMSInstructionGI.Creator = _userManager.Account;
                    //wMSInstructionGI.CreationTime = DateTime.Now;
                    //wMSInstructionGI.InstructionTaskNo = item.ExternOrderNumber;
                    //wMSInstructionGI.TableName = "WMS_Order";
                    //wMSInstructionGI.InstructionPriority = 63;
                    //wMSInstructionGI.Remark = "";
                    //wMSInstructions.Add(wMSInstructionGI);



                    WMSInstruction wMSInstructionGI99 = new WMSInstruction();
                    //wMSInstruction.OrderId = orderData[0].Id;
                    wMSInstructionGI99.InstructionStatus = (int)InstructionStatusEnum.新增;
                    wMSInstructionGI99.InstructionType = "出库单回传HachDG";
                    wMSInstructionGI99.BusinessType = "出库单回传HachDG";
                    //wMSInstruction.InstructionTaskNo = DateTime.Now;
                    wMSInstructionGI99.CustomerId = item.CustomerId;
                    wMSInstructionGI99.CustomerName = item.CustomerName;
                    wMSInstructionGI99.WarehouseId = item.WarehouseId;
                    wMSInstructionGI99.WarehouseName = item.WarehouseName;
                    wMSInstructionGI99.OperationId = item.Id;
                    wMSInstructionGI99.OrderNumber = item.ExternOrderNumber;
                    wMSInstructionGI99.Creator = _userManager.Account;
                    wMSInstructionGI99.CreationTime = DateTime.Now;
                    wMSInstructionGI99.InstructionTaskNo = item.ExternOrderNumber;
                    wMSInstructionGI99.TableName = "WMS_Order";
                    wMSInstructionGI99.InstructionPriority = 99;
                    wMSInstructionGI99.Remark = "";
                    wMSInstructions.Add(wMSInstructionGI99);

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


                    //出库装箱回传判断DN 是不是都完成了。ND下的所有的so 都完成才可以插入出库装箱回传 (客户系统需要对接WMS)
                    //让安琪将DN 字段对接到业务表中 STR1 可以通过dn 字段来判断是不是所有的dn 都已经完成，那么可以插入装箱信息
                    var checkOrderDN = await _repOrder.AsQueryable().Where(a => a.Dn == item.Dn && a.OrderStatus != (int)OrderStatusEnum.完成).ToListAsync();
                    if (checkOrderDN != null || checkOrderDN.Count > 0)
                    {
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
                        wMSInstructionSNGRHach.OrderNumber = item.Dn;
                        wMSInstructionSNGRHach.Creator = _userManager.Account;
                        wMSInstructionSNGRHach.CreationTime = DateTime.Now;
                        wMSInstructionSNGRHach.InstructionTaskNo = item.Dn;
                        wMSInstructionSNGRHach.TableName = "WMS_Order";
                        wMSInstructionSNGRHach.InstructionPriority = 1;
                        wMSInstructionSNGRHach.Remark = "";
                        wMSInstructions.Add(wMSInstructionSNGRHach);
                    }
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
