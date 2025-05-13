
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos.Enum;
using XAct;
using Admin.NET.Application.Enumerate;
using Nest;
using SpliteToBox.Common;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;
using AutoMapper;

namespace Admin.NET.Application.Strategy
{
    public class ReceiptInvenrotyHachStrategy : IReceiptInventoryInterface
    {
        public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
        public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public ReceiptInvenrotyHachStrategy()
        {

        }
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {
            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<WMSReceiptDetail, WMSReceiptReceiving>());
            var mapper = new Mapper(config);
            //解析DataTable
            //response.Data = receipts;
            //上架方法是将 入库单的数据，按照每一行拆开添加 库区库位之后 整合数据插入到上架表
            //按照订单号，SKU,批次号，行号的方式整合数据 ：系统约定入库单每一单的最小颗粒度为 SKU+LineNumber
            List<WMSReceiptReceiving> receiptReceivings = new List<WMSReceiptReceiving>();


            //获取同订单下的数据作为校验
            var checkData = await _repReceipt.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id) && a.ReceiptStatus == (int)ReceiptStatusEnum.新增).ToListAsync();
            var location = await _repLocation.AsQueryable().Where(a=>a.LocationStatus==(int)LocationStatusEnum.可用).FirstAsync();
            //try
            //{
            //    foreach (var item in checkData)
            //    {
            //        //var receiptOrderLineData = receiptOrderTemp.Details.Where(a => a.SKU == item.SKU && a.LineNumber == item.LineNumber).FirstOrDefault();

                    

            //            var dto = mapper.Map<List<WMSReceiptReceiving>>(item.Details);

            //        //var area = _repLocation.AsQueryable().Where(a => a.WarehouseId == receiptOrderLineData.WarehouseId && a.Location == item.Location).First();

            //        dto.ForEach(c =>
            //        {
            //            c.Location = location.Location;
            //            //c.ExpirationDate = item.ExpirationDate;
            //            //c.ProductionDate = item.ProductionDate;
            //            //c.BatchCode = item.BatchCode;
            //            c.GoodsStatus = (int)GoodsStatusEnum.正常;
            //            //c.ReceivedQty = item.ReceivedQty;
            //            //c.ReceiptDetailId = receiptOrderLineData.Id;
            //            c.ReceiptReceivingStatus = (int)ReceiptReceivingStatusEnum.上架;
                      

            //        });
            //        receiptReceivings.AddRange(dto);
               
                      

            //    }
            //    if (response.Data.Count > 0)
            //    {
            //        response.Code = StatusCode.Error;
            //        response.Msg = "失败";
            //        //throw new NotImplementedException();
            //        return response;
            //    }
            //    //删除已经上架的明细
            //    _repReceiptReceiving.Delete(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            //    //上架明细添加到上架表
            //    await _repReceiptReceiving.InsertRangeAsync(receiptReceivings.ToList());

            //    //await _repReceiptReceiving.AsInsertable(receiptReceivings).ExecuteCommandAsync();
            //    //修改入库单的状态
            //    //_wms_receiptManager.Query().Where(a => (request as List<WMS_ReceiptReceivingEditDto>).Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //    var receiptData = _repReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToList();
            //    foreach (var item in receiptData)
            //    {
            //        item.ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架;
            //    }
            //    //receiptData.ForEach(c =>
            //    //{
            //    //    c.ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架;
            //    //});
            //    await _repReceipt.UpdateRangeAsync(receiptData);
            //    //await _repReceipt.AsUpdateable(receiptData).ExecuteCommandAsync();

            //    //.BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //    //修改入库单的上架数量
            //    //_wms_receiptdetailRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(a => new WMS_ReceiptDetail { ReceivedQty = _wms_receiptreceivingRepository.GetAll().Where(re => re.ReceiptDetailId == a.Id).Sum(c => c.ReceivedQty) });
            //    var checkDataTemp = await _repReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToListAsync();

            //    checkDataTemp.ToList().ForEach(b =>
            //    {
            //        response.Data.Add(new OrderStatusDto()
            //        {
            //            ExternOrder = b.ExternReceiptNumber,
            //            SystemOrder = b.ReceiptNumber,
            //            Type = b.ReceiptType,
            //            StatusCode = StatusCode.Success,
            //            //StatusMsg = StatusCode.warning.ToString(),
            //            Msg = "订单入库成功"
            //        });

            //    });
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            //throw new NotImplementedException();
            return response;
        }

        public Task<Response<List<OrderStatusDto>>> Strategy(List<WMSReceiptInput> request)
        {
            throw new NotImplementedException();
        }
    }

}