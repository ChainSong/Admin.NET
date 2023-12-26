
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


namespace Admin.NET.Application.Strategy
{
    public class MReceiptInventoryDefaultStrategy : IMReceiptInventoryInterface
    {

        public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }
        public SqlSugarRepository<MMSReceipt> _repMReceipt { get; set; }
        public SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public SqlSugarRepository<MMSSupplier> _repSupplier { get; set; }
        public SqlSugarRepository<SupplierUserMapping> _repSupplierUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable { get; set; }

        //public SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable { get; set; }

        //public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }


        public MReceiptInventoryDefaultStrategy(

        )
        {
        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MMSReceiptReceivingDetail, MMSInventoryUsable>()
                //自定义投影，将上架表ID 投影到库存表中
                .ForMember(a => a.ReceiptReceivingId, opt => opt.MapFrom(c => c.ReceiptReceivingId))
                .ForMember(a => a.ReceiptReceivingDetailId, opt => opt.MapFrom(c => c.Id))
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
            List<MMSInventoryUsable> inventory = new List<MMSInventoryUsable>();

            foreach (var a in request)
            {
                //判断上架单和入库单数量是否一致，一致就加入库存
                WMSInventoryUsable aa = new WMSInventoryUsable();

                var receipt = _repMReceiptReceiving.AsQueryable().Includes(b => b.Details).Where(b => b.Id == a).First();
                if (receipt != null && receipt.ReceiptReceivingStatus == (int)MReceiptReceivingStatusEnum.已上架)
                {
                    //var receiptreceiving = _wms_receiptreceivingRepository.GetAll().Where(b => b.ReceiptId == a).ToList();
                    if (receipt.Details.Sum(r => r.ReceivedQty) == receipt.Details.Sum(r => r.ReceivedQty))
                    {
                        var inventoryData = mapper.Map<List<MMSInventoryUsable>>(receipt.Details);
                        //inventory.AddRange(inventoryData);
                        //添加到库存表
                        //await _repTableInventoryUsable.AsInsertable(inventoryData).ExecuteCommandAsync();
                        await _repInventoryUsable.InsertRangeAsync(inventoryData);
                        //修改入库单状态
                        await _repMReceipt.UpdateAsync(a => new MMSReceipt { ReceiptStatus = (int)MReceiptStatusEnum.完成, CompleteTime = DateTime.Now }, (a => a.Id == receipt.Id));
                        //修改入库单明细中的入库数量
                        //_wms_receiptdetailRepository.GetDbContext().BulkUpdate();
                        var receiptDetailData = _repMReceiptDetail.AsQueryable().Where(a => a.ReceiptId == receipt.Id).ToList();
                        receiptDetailData.ForEach(e =>
                        {
                            e.ReceivedQty = _repMReceiptReceivingDetail.AsQueryable().Where(re => re.ReceiptDetailId == e.Id).Sum(c => c.ReceivedQty);
                            e.Updator = _userManager.Account;
                            e.UpdateTime = DateTime.Now;

                        });
                        await _repMReceiptDetail.UpdateRangeAsync(receiptDetailData);
                        //_repReceiptDetail.AsUpdateable(receiptDetailData).ExecuteCommandAsync();
                        //修改上架表中的状态
                        await _repMReceiptReceiving.UpdateAsync(a => new MMSReceiptReceiving { ReceiptReceivingStatus = (int)MReceiptStatusEnum.完成,CompleteTime=DateTime.Now, Updator = _userManager.Account, UpdateTime = DateTime.Now }, (a => request.Contains(a.Id)));



                        orderStatuses.Add(new OrderStatusDto()
                        {
                            Id = a,
                            ExternOrder = receipt.ExternReceiptNumber,
                            SystemOrder = receipt.ReceiptNumber,
                            StatusCode = StatusCode.Success,
                            //StatusMsg = StatusCode.success.ToString(),
                            Msg = "订单添加成功"
                        });
                    }
                    else
                    {
                        orderStatuses.Add(new OrderStatusDto()
                        {
                            Id = a,
                            ExternOrder = receipt.ExternReceiptNumber,
                            SystemOrder = receipt.ReceiptNumber,
                            StatusCode = StatusCode.Error,
                            //StatusMsg = StatusCode.error.ToString(), 
                            Msg = "请检查订单数量"
                        });
                    }
                }
                else if (receipt.ReceiptReceivingStatus != (int)MReceiptReceivingStatusEnum.上架中)
                {
                    orderStatuses.Add(new OrderStatusDto()
                    {
                        Id = a,
                        ExternOrder = receipt.ExternReceiptNumber,
                        SystemOrder = receipt.ReceiptNumber,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.error.ToString(),
                        Msg = "请检查订单状态"
                    });
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