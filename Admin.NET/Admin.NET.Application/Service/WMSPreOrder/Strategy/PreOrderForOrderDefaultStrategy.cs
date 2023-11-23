
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common.SnowflakeCommon;

namespace Admin.NET.ApplicationCore.Strategy
{
    public class PreOrderForOrderDefaultStrategy : IPreOrderForOrderInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }

        public PreOrderForOrderDefaultStrategy(
            )
        {

        }

        //处理预出库单业务
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var preOrderCheck = _repPreOrder.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();
            if (preOrderCheck != null && preOrderCheck.ToList().Count > 0)
            {
                preOrderCheck.ToList().ForEach(b =>
                {
                    if (b.PreOrderStatus >= (int)PreOrderStatusEnum.转出库)
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



            //将需要分配的订单发送到分配队列
            //分配队列按照客户ID+仓库ID创建

            //获取需要分配的订单，采用自动分配
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSPreOrder, WMSOrder>()
                     .AddTransform<string>(a => a == null ? "" : a)
                     .ForMember(a => a.PreOrderId, opt => opt.MapFrom(c => c.Id))
                     //添加创建人为当前用户
                     .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                     .ForMember(a => a.Details, opt => opt.MapFrom(c => c.Details))
                     //添加库存状态为可用
                     .ForMember(a => a.OrderStatus, opt => opt.MapFrom(c => OrderStatusEnum.新增))
                     .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                     .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                     //忽略修改时间
                     .ForMember(a => a.UpdateTime, opt => opt.Ignore())
                     //忽略修改人
                     .ForMember(a => a.Updator, opt => opt.Ignore())
                     .ForMember(a => a.CompleteTime, opt => opt.Ignore());


                cfg.CreateMap<WMSPreOrderDetail, WMSOrderDetail>()
               .AddTransform<string>(a => a == null ? "" : a)
               .ForMember(a => a.PreOrderDetailId, opt => opt.MapFrom(c => c.Id))
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               //忽略修改时间
               .ForMember(a => a.UpdateTime, opt => opt.Ignore())
               //忽略修改人
               .ForMember(a => a.Updator, opt => opt.Ignore())
               ;


            });


            var mapper = new Mapper(config);

            var orderData = mapper.Map<List<WMSOrder>>(preOrderCheck);
            //int LineNumber = 1;
            orderData.ForEach(item =>
            {
                //var CustomerId = _customerusermappingManager.Query().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                //var WarehouseId = _warehouseusermappingManager.Query().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                var OrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                item.OrderNumber = OrderNumber;
                //item.CustomerId = CustomerId;
                //item.WarehouseId = WarehouseId;
                //item.DetailCount = item.OrderDetails.Sum(pd => pd.OriginalQty);
                item.Details.ForEach(a =>
                {
                    a.OrderNumber = OrderNumber;
                    //a.CustomerId = CustomerId;
                    //a.CustomerName = item.CustomerName;
                    //a.WarehouseId = WarehouseId;
                    //a.WarehouseName = item.WarehouseName;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                    //a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                });
                //LineNumber++;
            });

            //开始提交出库单
            //_repOrder.GetDbContext().BulkInsert(orderData, options => options.IncludeGraph = true);
            await _db.InsertNav(orderData).Include(a => a.Details).ExecuteCommandAsync();
            //更新预出库单状态
            _repPreOrder.AsQueryable().Where(a => request.Contains(a.Id)).ToList().ForEach(e =>
            {
                e.PreOrderStatus = (int)PreOrderStatusEnum.转出库;
                e.Updator = _userManager.Account;
                e.UpdateTime = DateTime.Now;
                _repPreOrder.Update(e);
            });
            //更新转出库单数量
            _reppreOrderDetail.AsQueryable().Where(a => request.Contains(a.PreOrderId)).ToList().ForEach(e =>
            {
                e.OrderQty = _repOrderDetail.AsQueryable().Where(b => b.PreOrderDetailId == e.Id).Sum(d => d.OrderQty);
                e.Updator = _userManager.Account;
                e.UpdateTime = DateTime.Now;
                _reppreOrderDetail.Update(e);
            });
            var preOrderData = _repPreOrder.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();

            if (preOrderData != null && preOrderData.ToList().Count > 0)
            {
                preOrderData.ToList().ForEach(b =>
                {
                    if (b.PreOrderStatus >= (int)PreOrderStatusEnum.部分转出库)
                        response.Data.Add(new OrderStatusDto()
                        {
                            Id = b.Id,
                            ExternOrder = b.ExternOrderNumber,
                            SystemOrder = b.PreOrderNumber,
                            Type = b.OrderType,
                            StatusCode = StatusCode.Success,
                            //StatusMsg = (string)StatusCode.warning,
                            Msg = "转出库单成功"
                        });

                });
            }
            response.Code = StatusCode.Success;
            return response;

        }
    }
}
