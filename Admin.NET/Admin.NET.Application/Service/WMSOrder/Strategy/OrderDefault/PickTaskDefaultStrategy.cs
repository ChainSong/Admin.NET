
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
    public class PickTaskDefaultStrategy : IPickTaskInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }


        public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
        public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
        //public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

        public PickTaskDefaultStrategy(
            )
        {

        }

        //处理分配业务
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            var orderData = _repOrder.AsQueryable().Includes(a => a.Allocation).Includes(a => a.OrderAddress).Where(a => request.Contains(a.Id)).ToList();
            if (orderData != null && orderData.Where(a => a.OrderStatus != (int)OrderStatusEnum.已分配).ToList().Count > 0)
            {
                orderData.ToList().ForEach(b =>
                {
                    if (b.OrderStatus != (int)OrderStatusEnum.已分配)
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


            await _repOrder.UpdateAsync(a => new WMSOrder { OrderStatus = (int)OrderStatusEnum.拣货中 }, a => orderData.Select(c => c.Id).Contains(a.Id));
            List<WMSPickTask> pickTasks = new List<WMSPickTask>();
            //新需求，按照同客户地址拣货任务合并，
            orderData.ForEach(data =>
            {
                if (pickTasks == null || pickTasks.Where(a => a.Str1 == (data.OrderAddress.Phone + data.OrderAddress.City + data.OrderAddress.Address) + "".Trim()).ToList().Count == 0)
                {
                    var pickTaskNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                    //将需要分配的订单发送到分配队列
                    //获取需要分配的订单，采用自动分配
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<WMSOrderAllocation, WMSPickTaskDetail>()
                             .ForMember(a => a.PickStatus, opt => opt.MapFrom(c => (int)PickTaskStatusEnum.新增))
                             .ForMember(a => a.ExternOrderNumber, opt => opt.MapFrom(c => data.ExternOrderNumber))
                             .ForMember(a => a.OrderNumber, opt => opt.MapFrom(c => data.OrderNumber))
                             .ForMember(a => a.PreOrderNumber, opt => opt.MapFrom(c => data.PreOrderNumber))
                             .ForMember(a => a.PickTaskNumber, opt => opt.MapFrom(c => pickTaskNumber))
                             .ForMember(a => a.Id, opt => opt.Ignore())
                             //添加创建人为当前用户
                             .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                             .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
                    });

                    var mapper = new Mapper(config);
                    var detaildata = mapper.Map<List<WMSPickTaskDetail>>(data.Allocation);

                    pickTasks.Add(new WMSPickTask
                    {
                        CustomerId = data.CustomerId,
                        CustomerName = data.CustomerName,
                        WarehouseId = data.WarehouseId,
                        WarehouseName = data.WarehouseName,
                        PickTaskNumber = pickTaskNumber,
                        ExternOrderNumber = data.ExternOrderNumber,
                        OrderNumber = data.OrderNumber,
                        PickStatus = (int)OrderStatusEnum.新增,
                        PickType = "普通拣货",
                        //StartTime
                        //EndTime
                        PrintNum = 0,
                        Str1 = (data.OrderAddress.Phone + data.OrderAddress.City + data.OrderAddress.Address) + "".Trim(),
                        //PrintTime
                        //PrintPersonnel
                        //PickPlanPersonnel
                        //DetailQty = detaildata.Sum(a => a.Qty),
                        //DetailKindsQty = detaildata.GroupBy(a => a.SKU).Count(),
                        //PickContainer
                        Creator = _userManager.Account,
                        CreationTime = DateTime.Now,
                        Details = detaildata,

                    });
                }
                else
                {
                    var pickTasksdata = pickTasks.Where(a => a.Str1 == (data.OrderAddress.Phone + data.OrderAddress.City + data.OrderAddress.Address) + "".Trim()).First();
                    //将需要分配的订单发送到分配队列
                    //获取需要分配的订单，采用自动分配
                    var config = new MapperConfiguration(cfg =>
                      {
                          cfg.CreateMap<WMSOrderAllocation, WMSPickTaskDetail>()
                               .ForMember(a => a.PickStatus, opt => opt.MapFrom(c => (int)PickTaskStatusEnum.新增))
                               .ForMember(a => a.ExternOrderNumber, opt => opt.MapFrom(c => data.ExternOrderNumber))
                               .ForMember(a => a.OrderNumber, opt => opt.MapFrom(c => data.OrderNumber))
                               .ForMember(a => a.PreOrderNumber, opt => opt.MapFrom(c => data.PreOrderNumber))
                               .ForMember(a => a.PickTaskNumber, opt => opt.MapFrom(c => pickTasksdata.PickTaskNumber))
                               .ForMember(a => a.Id, opt => opt.Ignore())
                               //添加创建人为当前用户
                               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
                      });

                    var mapper = new Mapper(config);
                    var detaildata = mapper.Map<List<WMSPickTaskDetail>>(data.Allocation);

                    pickTasks.Where(a => a.Str1 == (data.OrderAddress.Phone + data.OrderAddress.City + data.OrderAddress.Address) + "".Trim()).First().Details.AddRange(detaildata);

                }

            });
            pickTasks.ToList().ForEach(a =>
            {
                a.DetailQty = a.Details.Sum(b => b.Qty);
                a.DetailKindsQty = a.Details.GroupBy(b => b.SKU).Count();
            });
            //_repPickTask.AsQueryable().Includes(a => a.Detail).Where(a => request.Contains(a.Id)).ToList();
            await _repOrder.Context.InsertNav(pickTasks).Include(a => a.Details).ExecuteCommandAsync();


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
                    Msg = "生成拣货任务成功"
                });
            });

            //开始校验数据
            //_wms_preorderRepository.GetDbContext().BulkInsert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;

        }
    }
}
