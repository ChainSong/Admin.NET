
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.ReceiptCore.Interface;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;


namespace Admin.NET.Application.Strategy
{
    public class ReceiptInventoryDefaultStrategy : IReceiptInventoryInterface
    {

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
        public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }

        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

        public SqlSugarRepository<WMSASN> _repASN { get; set; }

        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        //public ISqlSugarClient _db { get; set; }


        public ReceiptInventoryDefaultStrategy(

        )
        {
        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<WMSReceiptReceiving, WMSInventoryUsable>()
                //自定义投影，将上架表ID 投影到库存表中
                .ForMember(a => a.ReceiptReceivingId, opt => opt.MapFrom(c => c.Id))
                //添加创建人为当前用户
                .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                //创建时间
                .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                //库龄
                .ForMember(a => a.InventoryTime, opt => opt.MapFrom(c => DateTime.Now))
                //数量
                .ForMember(a => a.Qty, opt => opt.MapFrom(c => c.ReceivedQty))
                //添加库存状态为可用
                .ForMember(a => a.InventoryStatus, opt => opt.MapFrom(c => (int)InventoryStatusEnum.可用))
                //.ForMember(a => a.InventoryStatus, opt => opt.MapFrom(c => 1))
                //将为Null的字段设置为"" () 
                .AddTransform<string>(a => a == null ? "" : a)
                //.AddTransform<DateTime?>(a => a is null ? null : )
                //忽略空值映射
                //.IgnoreNullValues(true)
                //忽略需要转换的字段 
                .ForMember(a => a.Id, opt => opt.Ignore())
                .ForMember(a => a.Updator, opt => opt.Ignore())
                .ForMember(a => a.UpdateTime, opt => opt.Ignore())
                );

            //config.

            var mapper = new Mapper(config);


            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            List<OrderStatusDto> orderStatuses = new List<OrderStatusDto>();
            //List<WMS_ReceiptDetail> receiptDetails = new List<WMS_ReceiptDetail>();

            //receipts.WMS_Receipts = new List<WMS_ReceiptEditDto>();
            //判断receipts  是否可以转入库单
            List<WMSInventoryUsable> inventory = new List<WMSInventoryUsable>();

            foreach (var id in request)
            {
                //判断上架单和入库单数量是否一致，一致就加入库存
                WMSInventoryUsable aa = new WMSInventoryUsable();

                var receipt = _repReceipt.AsQueryable().Includes(b => b.Details).Includes(c => c.ReceiptReceivings).Where(b => b.Id == id).First();
                if (receipt != null && receipt.ReceiptStatus == (int)ReceiptStatusEnum.上架)
                {
                    //var receiptreceiving = _wms_receiptreceivingRepository.GetAll().Where(b => b.ReceiptId == a).ToList();
                    if (receipt.Details.Sum(r => r.ReceivedQty) == receipt.ReceiptReceivings.Sum(r => r.ReceivedQty))
                    {
                        //对比每一条数据，如果入库数量和上架数量一致，就加入库存
                        foreach (var detail in receipt.Details)
                        {
                            if (detail.ReceivedQty < receipt.ReceiptReceivings.Where(a => a.ReceiptDetailId == detail.Id).Sum(r => r.ReceivedQty))
                            {
                                orderStatuses.Add(new OrderStatusDto()
                                {
                                    Id = id,
                                    ExternOrder = receipt.ExternReceiptNumber,
                                    SystemOrder = receipt.ReceiptNumber,
                                    StatusCode = StatusCode.Error,
                                    //StatusMsg = StatusCode.error.ToString(), 
                                    Msg = "SKU：" + detail.SKU + "行号：" + detail.LineNumber + "，上架数量和订单不符"
                                });


                            }
                        }
                        if (orderStatuses.Count > 0)
                        {
                            response.Data = orderStatuses;
                            response.Code = StatusCode.Error;
                            response.Msg = "加入库存失败，请检查订单数量";
                            //throw new NotImplementedException();
                            return response;
                        }

                        var inventoryData = mapper.Map<List<WMSInventoryUsable>>(receipt.ReceiptReceivings);
                        //inventory.AddRange(inventoryData);
                        //添加到库存表
                        //await _repTableInventoryUsable.AsInsertable(inventoryData).ExecuteCommandAsync();
                        await _repTableInventoryUsable.InsertRangeAsync(inventoryData);

                        //修改入库单状态
                        await _repReceipt.AsUpdateable()
                          .SetColumns(p => p.ReceiptStatus == (int)ReceiptStatusEnum.完成)
                          .SetColumns(p => p.CompleteTime == DateTime.Now)
                          .Where(p => receipt.Id == p.Id)
                          .ExecuteCommandAsync();
                        //await _repReceipt.UpdateAsync(a => new WMSReceipt { ReceiptStatus = (int)ReceiptStatusEnum.完成, CompleteTime = DateTime.Now }, (a => a.Id == receipt.Id));
                        //修改入库单明细中的入库数量
                        //_wms_receiptdetailRepository.GetDbContext().BulkUpdate();
                        var receiptDetailData = _repReceiptDetail.AsQueryable().Where(a => a.ReceiptId == receipt.Id).ToList();
                        foreach (var item in receiptDetailData)
                        {
                            item.ReceiptQty = item.ReceiptQty + _repReceiptReceiving.AsQueryable().Where(re => re.ReceiptDetailId == item.Id).Sum(c => c.ReceivedQty);
                            item.Updator = _userManager.Account;
                            item.UpdateTime = DateTime.Now;
                        }
                        //receiptDetailData.ForEach(e =>
                        //{
                        //    e.ReceiptQty = e.ReceiptQty + _repReceiptReceiving.AsQueryable().Where(re => re.ReceiptDetailId == e.Id).Sum(c => c.ReceivedQty);
                        //    e.Updator = _userManager.Account;
                        //    e.UpdateTime = DateTime.Now;

                        //});
                        await _repReceiptDetail.UpdateRangeAsync(receiptDetailData);

                        //_repReceiptDetail.AsUpdateable(receiptDetailData).ExecuteCommandAsync();

                        //修改上架表中的状态
                        await _repReceiptReceiving.UpdateAsync(a => new WMSReceiptReceiving { ReceiptReceivingStatus = (int)ReceiptStatusEnum.完成, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => request.Contains(a.ReceiptId)));

                        //修改ASN 明细中的实际入库数量
                        //_wms_asndetailRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(v =>new WMS_ASNDetail{ ReceivedQty = _wms_receiptreceivingRepository.GetAll().Where(re => re.ASNDetailId == v.Id).Sum(c => c.ReceivedQty)});
                        var asnDetailData = _repASNDetail.AsQueryable().Where(a => a.ASNId == receipt.ASNId).ToList();
                        foreach (var item in asnDetailData)
                        {
                            item.ReceiptQty = item.ReceiptQty + _repReceiptReceiving.AsQueryable().Where(re => re.ASNDetailId == item.Id && re.ReceiptId == receipt.Id).Sum(c => c.ReceivedQty);
                            item.Updator = _userManager.Account;
                            item.UpdateTime = DateTime.Now;
                        }
                        //asnDetailData.ForEach(e =>
                        //{
                        //    e.ReceiptQty = e.ReceiptQty + _repReceiptReceiving.AsQueryable().Where(re => re.ASNDetailId == e.Id && re.ReceiptId == receipt.Id).Sum(c => c.ReceivedQty);
                        //    e.Updator = _userManager.Account;
                        //    e.UpdateTime = DateTime.Now;

                        //});
                        await _repASNDetail.UpdateRangeAsync(asnDetailData);

                        //修改ASN 状态
                        //需要判断是否全部入库完成，如果完成，则修改ASN状态为完成
                        var asn = await _repASNDetail.AsQueryable().Where(a => a.ASNId == receipt.ASNId && a.ExpectedQty > a.ReceiptQty).ToListAsync();
                        if (asn != null && asn.Count == 0)
                        {
                            await _repASN.AsUpdateable()
                            .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.完成)
                            .SetColumns(p => p.CompleteTime == DateTime.Now)
                            .Where(p => receipt.ASNId == p.Id)
                            .ExecuteCommandAsync();
                        }
                        else
                        {

                        }

                        //await _repASN.UpdateAsync(a => new WMSASN { ASNStatus = (int)ASNStatusEnum.完成, CompleteTime = DateTime.Now }, (a => a.Id == receipt.ASNId));


                        orderStatuses.Add(new OrderStatusDto()
                        {
                            Id = id,
                            ExternOrder = receipt.ExternReceiptNumber,
                            SystemOrder = receipt.ReceiptNumber,
                            StatusCode = StatusCode.Success,
                            //StatusMsg = StatusCode.success.ToString(),
                            Msg = "加入库存成功"
                        });
                    }
                    else
                    {
                        orderStatuses.Add(new OrderStatusDto()
                        {
                            Id = id,
                            ExternOrder = receipt.ExternReceiptNumber,
                            SystemOrder = receipt.ReceiptNumber,
                            StatusCode = StatusCode.Error,
                            //StatusMsg = StatusCode.error.ToString(), 
                            Msg = "请检查订单数量"
                        });
                        response.Data = orderStatuses;
                        response.Code = StatusCode.Error;
                        response.Msg = "加入库存失败，请检查订单数量";
                        //throw new NotImplementedException();
                        return response;
                    }
                }
                else if (receipt.ReceiptStatus != (int)ReceiptStatusEnum.上架)
                {
                    orderStatuses.Add(new OrderStatusDto()
                    {
                        Id = id,
                        ExternOrder = receipt.ExternReceiptNumber,
                        SystemOrder = receipt.ReceiptNumber,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.error.ToString(),
                        Msg = "请检查订单状态"
                    });
                    response.Data = orderStatuses;
                    response.Code = StatusCode.Error;
                    response.Msg = "加入库存失败，请检查订单状态";
                    //throw new NotImplementedException();
                    return response;
                }
            };

            response.Data = orderStatuses;
            response.Code = StatusCode.Success;
            response.Msg = "加入库存成功";
            //throw new NotImplementedException();
            return response;

        }
    }
}