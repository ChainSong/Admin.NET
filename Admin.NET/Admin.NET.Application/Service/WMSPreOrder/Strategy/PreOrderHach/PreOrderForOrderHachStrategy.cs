
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
using Aliyun.OSS;
using Nest;
using SharpCompress.Common;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateContainerServiceRequest.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaSecOrderUploadCombinedShippingInfoRequest.Types.SubOrder.Types;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Reflection.Emit;

namespace Admin.NET.ApplicationCore.Strategy
{
    public class PreOrderForOrderHachStrategy : IPreOrderForOrderInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
        public SqlSugarRepository<WMSOrderDetailBom> _repOrderDetailBom { get; set; }

        public PreOrderForOrderHachStrategy(
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

            //获取Bom信息
            //var bomData = _repProductBom.AsQueryable().Where(a => preOrderCheck.De.Contains(a.PreOrderId)).ToList();

            //将需要分配的订单发送到分配队列
            //分配队列按照客户ID+仓库ID创建

            //获取需要分配的订单，采用自动分配
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSPreOrder, WMSOrder>()
                     .AddTransform<string>(a => a == null ? "" : a)
                     .ForMember(a => a.PreOrderId, opt => opt.MapFrom(c => c.Id))
                     .ForMember(a => a.Id, opt => opt.MapFrom(c => 0))
                     .ForMember(a => a.OrderTime, opt => opt.MapFrom(c => c.OrderTime))
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
               .ForMember(a => a.Id, opt => opt.MapFrom(c => 0))
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               //忽略修改时间
               .ForMember(a => a.UpdateTime, opt => opt.Ignore())
               //忽略修改人
               .ForMember(a => a.Updator, opt => opt.Ignore());
            });


            var mapper = new Mapper(config);

            var orderData = mapper.Map<List<WMSOrder>>(preOrderCheck);
            orderData.ForEach(item =>
            {
                var OrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                item.OrderNumber = OrderNumber;
                item.Details.ForEach(a =>
                {
                    a.OrderNumber = OrderNumber;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                });

                //补充DetailBom的信息
                var detailBomData = item.Details.Adapt<List<WMSOrderDetailBom>>();
                //获取Bom信息
                var bomData = _repProductBom.AsQueryable().Where(a => a.CustomerId == item.CustomerId && item.Details.Select(c => c.SKU).Contains(a.SKU)).ToList();

                item.DetailBoms = (from a in detailBomData
                                  join b in bomData on a.SKU equals b.SKU
                                  select new WMSOrderDetailBom
                                  {
                                      PreOrderId = a.PreOrderId,
                                      PreOrderDetailId = a.PreOrderDetailId,
                                      //OrderId = a.OrderId,
                                      PreOrderNumber = a.PreOrderNumber,
                                      OrderNumber = a.OrderNumber,
                                      ExternOrderNumber = a.ExternOrderNumber,
                                      CustomerId = a.CustomerId,
                                      CustomerName = a.CustomerName,
                                      WarehouseId = a.WarehouseId,
                                      WarehouseName = a.WarehouseName,
                                      LineNumber = a.LineNumber,
                                      SKU = a.SKU,
                                      ChildSKU = b.ChildSKU,
                                      UPC = a.UPC,
                                      GoodsName = a.GoodsName,
                                      ChildGoodsName = b.ChildGoodsName,
                                      GoodsType = a.GoodsType,
                                      OrderQty = a.OrderQty,
                                      ChildQty = (a.OrderQty*b.Qty),
                                      AllocatedQty = a.AllocatedQty,
                                      BoxCode = a.BoxCode,
                                      TrayCode = a.TrayCode,
                                      BatchCode = a.BatchCode,
                                      LotCode = a.LotCode,
                                      PoCode = a.PoCode,
                                      Weight = a.Weight,
                                      Volume = a.Volume,
                                      UnitCode = a.UnitCode,
                                      Onwer = a.Onwer,
                                      ProductionDate = a.ProductionDate,
                                      ExpirationDate = a.ExpirationDate,
                                      Remark = a.Remark,
                                      Str1 = a.Str1,
                                      Str2 = a.Str2,
                                      Str3 = a.Str3,
                                      Str4 = a.Str4,
                                      Str5 = a.Str5,
                                      Str6 = a.Str6,
                                      Str7 = a.Str7,
                                      Str8 = a.Str8,
                                      Str9 = a.Str9,
                                      Str10 = a.Str10,
                                      Str11 = a.Str11,
                                      Str12 = a.Str12,
                                      Str13 = a.Str13,
                                      Str14 = a.Str14,
                                      Str15 = a.Str15,
                                      Str16 = a.Str16,
                                      Str17 = a.Str17,
                                      Str18 = a.Str18,
                                      Str19 = a.Str19,
                                      Str20 = a.Str20,
                                      DateTime1 = a.DateTime1,
                                      DateTime2 = a.DateTime2,
                                      DateTime3 = a.DateTime3,
                                      DateTime4 = a.DateTime4,
                                      DateTime5 = a.DateTime5,
                                      Int1 = a.Int1,
                                      Int2 = a.Int2,
                                      Int3 = a.Int3,
                                      Int4 = a.Int4,
                                      Int5 = a.Int5,
                                      TenantId = a.TenantId,
                                  }).ToList();

            });

            //开始提交出库单
            //_repOrder.GetDbContext().BulkInsert(orderData, options => options.IncludeGraph = true);
            await _repOrder.Context.InsertNav(orderData).Include(a => a.Details).Include(a => a.DetailBoms).ExecuteCommandAsync();
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
                e.ActualQty = _repOrderDetail.AsQueryable().Where(b => b.PreOrderDetailId == e.Id).Sum(d => d.OrderQty);
                e.Updator = _userManager.Account;
                e.UpdateTime = DateTime.Now;
                _reppreOrderDetail.Update(e);
            });
            var preOrderData = _repPreOrder.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();
            //比较出库单数量和预出库单数量
            //preOrderData.ToList().ForEach(b =>
            //{
            //    b.


            //});

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
