
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Common.EnumCommon;
using Admin.NET.Application.Enumerate;
using AutoMapper;
using NewLife.Net;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Application.Service.Enumerate;

namespace Admin.NET.Application.Strategy
{
    public class AutomatedAllocationDefaultStrategy : IAutomatedAllocationInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }

        public AutomatedAllocationDefaultStrategy(
            )
        {

        }

        //处理分配业务
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var orderData = _repOrder.AsQueryable().Where(a => request.Contains(a.Id)).ToList();
            if (orderData != null && orderData.Where(a => a.OrderStatus >= (int)OrderStatusEnum.已分配).ToList().Count > 0)
            {
                orderData.ToList().ForEach(b =>
                {
                    if (b.OrderStatus >= (int)OrderStatusEnum.分配中)
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

            //           ,[TableName]
            //,[BusinessType]
            //,[OperationId]
            //,[Creator]
            //,[CreationTime]
            var InstructionTaskNo = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //将需要分配的订单发送到分配队列
            //获取需要分配的订单，采用自动分配
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSOrder, WMSInstruction>()
                     .ForMember(a => a.TableName, opt => opt.MapFrom(c => "WMS_Order"))

                     .ForMember(a => a.InstructionType, opt => opt.MapFrom(c => "分配"))

                     .ForMember(a => a.BusinessType, opt => opt.MapFrom(c => "自动分配"))

                     .ForMember(a => a.InstructionStatus, opt => opt.MapFrom(c => InstructionStatusEnum.新增))

                     .ForMember(a => a.OperationId, opt => opt.MapFrom(c => c.Id))
                     .ForMember(a => a.Id, opt => opt.Ignore())
                     .ForMember(a => a.InstructionTaskNo, opt => opt.MapFrom(c => InstructionTaskNo))
                     //添加创建人为当前用户
                     .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                     .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));




            });


            var mapper = new Mapper(config);

            var data = mapper.Map<List<WMSInstruction>>(orderData);

            await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.分配中 }, a => orderData.Select(c => c.Id).Contains(a.Id));
            await _repInstruction.InsertRangeAsync(data);
            var InstructionQueue = _repInstruction.AsQueryable().Where(a =>
          a.CustomerId == orderData.First().CustomerId &&
          a.WarehouseId == orderData.First().WarehouseId &&
          a.InstructionType == "分配" &&
          a.InstructionStatus== (int)InstructionStatusEnum.新增
          ).OrderBy(a => a.InstructionTaskNo).Select(a => a.InstructionTaskNo).Distinct();
            //分配队列按照客户ID+仓库ID创建

            //获取需要分配的订单，采用自动分配
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<WMSOrder, WMS_Order>()
            //         .ForMember(a => a.PreOrderId, opt => opt.MapFrom(c => c.Id))
            //         //添加创建人为当前用户
            //         .ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //         .ForMember(a => a.OrderDetails, opt => opt.MapFrom(c => c.PreOrderDetails))
            //         //添加库存状态为可用
            //         .ForMember(a => a.OrderStatus, opt => opt.MapFrom(c => OrderStatusEnum.待处理))
            //         .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //         //忽略修改时间
            //         .ForMember(a => a.UpdateTime, opt => opt.Ignore())
            //         .ForMember(a => a.CompleteTime, opt => opt.Ignore());


            //    cfg.CreateMap<WMS_PreOrderDetail, WMS_OrderDetail>()
            //   .ForMember(a => a.PreOrderDetailId, opt => opt.MapFrom(c => c.Id))
            //   //添加创建人为当前用户
            //   .ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //   //忽略修改时间
            //   .ForMember(a => a.UpdateTime, opt => opt.Ignore());

            //});

            //orderData.ToList().ForEach(a =>
            //{
            //    //声明分配后需要插入的库存
            //    List<WMSInventoryUsable> invs = new List<WMSInventoryUsable>();
            //    //声明分配后需要插入的出库明细WMS_OrderAllocation
            //    List<WMSOrderAllocation> ods = new List<WMSOrderAllocation>();
            //    if (a.OrderStatus == (int)PreOrderStatusEnum.新增)
            //    {
            //        //判断订单数量足不足够分配
            //        var iCheck =



            //        //获取明细
            //        a.Details.ForEach(pd =>
            //        {
            //            //获取可用库存
            //            var inventory = _wms_inventory_usableRepository.GetAll()
            //                //必须的条件
            //                .Where(i =>
            //                i.CustomerId == pd.CustomerId &&
            //                i.WarehouseId == pd.WarehouseId &&
            //                i.SKU == pd.SKU &&
            //                i.InventoryStatus == (int)InventoryStatusEnum.可用
            //                )
            //                //可选的条件 UPC
            //                .WhereIf(!pd.UPC.IsNullOrWhiteSpace(), a => a.UPC == pd.UPC)
            //                //可选的条件 UnitCode
            //                .WhereIf(!pd.UnitCode.IsNullOrWhiteSpace(), a => a.UnitCode == pd.UnitCode)
            //                //可选的条件 Onwer
            //                .WhereIf(!pd.Onwer.IsNullOrWhiteSpace(), a => a.Onwer == pd.Onwer)
            //                //可选的条件 BoxCode
            //                .WhereIf(!pd.BoxCode.IsNullOrWhiteSpace(), a => a.BoxCode == pd.BoxCode)
            //                //可选的条件 TrayCode
            //                .WhereIf(!pd.TrayCode.IsNullOrWhiteSpace(), a => a.TrayCode == pd.TrayCode)
            //                //可选的条件 BatchCode
            //                .WhereIf(!pd.BatchCode.IsNullOrWhiteSpace(), a => a.BatchCode == pd.BatchCode);
            //            if (inventory.Any() && inventory.Sum(i => i.Qty) > pd.AllocatedQty)
            //            {
            //                inventory.ToList().ForEach(iv =>
            //                {
            //                    //这利分为两部分，
            //                    //1部分是当前行库存满足分配要求，
            //                    //2部分是当前行库存不满足分配要求，
            //                    if (iv.Qty >= pd.AllocatedQty)
            //                    {

            //                    }

            //                });
            //            }
            //            else
            //            {

            //            }


            //        });

            //    }

            //});




            //var mapper = new Mapper(config);

            //var asnData = mapper.Map<List<WMS_Order>>(preOrderData);
            //int LineNumber = 1;
            //asnData.ForEach(item =>
            //{
            //    //var CustomerId = _customerusermappingManager.Query().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
            //    //var WarehouseId = _warehouseusermappingManager.Query().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
            //    var OrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //    item.OrderNumber = OrderNumber;
            //    //item.CustomerId = CustomerId;
            //    //item.WarehouseId = WarehouseId;
            //    //item.DetailCount = item.OrderDetails.Sum(pd => pd.OriginalQty);
            //    item.OrderDetails.ForEach(a =>
            //    {
            //        a.OrderNumber = OrderNumber;
            //        //a.CustomerId = CustomerId;
            //        //a.CustomerName = item.CustomerName;
            //        //a.WarehouseId = WarehouseId;
            //        //a.WarehouseName = item.WarehouseName;
            //        a.ExternOrderNumber = item.ExternOrderNumber;
            //        a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
            //    });
            //    LineNumber++;
            //});

            //开始校验数据
            //_wms_preorderRepository.GetDbContext().BulkInsert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            response.Msg = "已发送到分配队列：所处在队列第" + InstructionQueue.Count() + "批次";
            return response;

        }
    }
}
