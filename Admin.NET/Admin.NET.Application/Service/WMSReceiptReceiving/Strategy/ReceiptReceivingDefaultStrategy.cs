﻿
using AutoMapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using MyProject.ReceiptReceivingCore.Dto;
using Admin.NET.Application.CommonCore.EnumCommon;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;

namespace Admin.NET.Application.ReceiptReceivingCore.Strategy
{
    public class ReceiptReceivingDefaultStrategy : IReceiptReceivingInterface
    {
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
        public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }

        public ISqlSugarClient _db { get; set; }

        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

        public ReceiptReceivingDefaultStrategy(

        )
        {

        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<WMSReceiptReceivingEditDto> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            var config = new MapperConfiguration(cfg => cfg.CreateMap<WMSReceiptDetail, WMSReceiptReceiving>());
            var mapper = new Mapper(config);
            //解析DataTable
            //response.Data = receipts;
            //上架方法是将 入库单的数据，按照每一行拆开添加 库区库位之后 整合数据插入到上架表
            //按照订单号，SKU,批次号，行号的方式整合数据 ：系统约定入库单每一单的最小颗粒度为 SKU+LineNumber
            List<WMSReceiptReceiving> receiptReceivings = new List<WMSReceiptReceiving>();

            //使用 一个临时变量作为缓存使用，减少数据库访问次数，且减少内存消耗
            WMSReceipt receiptOrderTemp = new WMSReceipt();


            foreach (var item in request)
            {
                if (receiptOrderTemp == null || receiptOrderTemp.ReceiptNumber != item.ReceiptNumber)
                {
                    //获取同订单下的数据作为校验
                    receiptOrderTemp = _repReceipt.AsQueryable().Includes(a => a.Details).Where(a =>
                    a.ReceiptNumber == item.ReceiptNumber).FirstAsync().Result;
                }
                if (receiptOrderTemp == null || string.IsNullOrEmpty(receiptOrderTemp.ReceiptNumber))
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        Id = item.Id,
                        ExternOrder = item.ExternReceiptNumber,
                        SystemOrder = item.ReceiptNumber,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.success.ToString(),
                        Msg = "订单:" + item.ReceiptNumber + "不存在"
                    });
                    continue;
                    //response.Code = StatusCode.Error;
                    //response.Msg = "订单:" + item.ReceiptNumber + "不存在";
                    //return response;
                }
                if (receiptOrderTemp.ReceiptStatus == (int)ReceiptStatusEnum.完成)
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        Id = item.Id,
                        ExternOrder = item.ExternReceiptNumber,
                        SystemOrder = item.ReceiptNumber,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.success.ToString(),
                        Msg = "订单:" + item.ReceiptNumber + "已完成"
                    });
                    continue;
                    //response.Code = StatusCode.Error;
                    //response.Msg = "订单:" + item.ReceiptNumber + "已完成";
                    //return response;
                }

                var receiptOrderLineData = receiptOrderTemp.Details.Where(a => a.SKU == item.SKU && a.LineNumber == item.LineNumber).FirstOrDefault();

                if (receiptOrderLineData != null)
                {

                    var dto = mapper.Map<WMSReceiptReceiving>(receiptOrderLineData);

                    var area = _repLocation.AsQueryable().Where(a => a.WarehouseId == receiptOrderLineData.WarehouseId && a.Location == item.Location).First();

                    if (area == null)
                    {
                        response.Data.Add(new OrderStatusDto()
                        {
                            Id = item.Id,
                            ExternOrder = item.ExternReceiptNumber,
                            SystemOrder = item.ReceiptNumber,
                            StatusCode = StatusCode.Error,
                            //StatusMsg = StatusCode.success.ToString(),
                            Msg = "库位:" + item.Location + "在仓库：" + item.WarehouseName + "中不存在"
                        });
                        //response.Code = StatusCode.Error;
                        //response.Msg = "库位:" + item.Location + "在仓库：" + item.WarehouseName + "中不存在";
                        //return response;
                        continue;
                    }

                    dto.Area = area.AreaName;
                    dto.Location = item.Location;
                    dto.ReceivedQty = item.ReceivedQty;
                    dto.ReceiptDetailId = receiptOrderLineData.Id;
                    dto.ReceiptReceivingStatus = (int)ReceiptReceivingStatusEnum.上架;
                    receiptReceivings.Add(dto);
                }
                else
                {
                    continue;
                }
            }
            if (response.Data.Count > 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "失败";
                //throw new NotImplementedException();
                return response;
            }
            //删除已经上架的明细
            _repReceiptReceiving.Delete(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            //上架明细添加到上架表
            await _repReceiptReceiving.InsertRangeAsync(receiptReceivings.ToList());

            //await _repReceiptReceiving.AsInsertable(receiptReceivings).ExecuteCommandAsync();
            //修改入库单的状态
            //_wms_receiptManager.Query().Where(a => (request as List<WMS_ReceiptReceivingEditDto>).Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            var receiptData = _repReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToList();

            receiptData.ForEach(c =>
            {
                c.ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架;
            });
            await _repReceipt.UpdateRangeAsync(receiptData);
            //await _repReceipt.AsUpdateable(receiptData).ExecuteCommandAsync();

            //.BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //修改入库单的上架数量
            //_wms_receiptdetailRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(a => new WMS_ReceiptDetail { ReceivedQty = _wms_receiptreceivingRepository.GetAll().Where(re => re.ReceiptDetailId == a.Id).Sum(c => c.ReceivedQty) });

            response.Code = StatusCode.Success;
            response.Msg = "成功";
            //throw new NotImplementedException();
            return response;
        }


        //private List<Table_Columns> GetColumns(string TableName, IAbpSessionExtension abpSession)
        //{
        //    return _table_ColumnsManager.Query()
        //       .Where(a => a.TableName == TableName &&
        //         a.RoleName == abpSession.RoleName &&
        //         a.IsImportColumn == 1
        //       )
        //      .Select(a => new Table_Columns
        //      {
        //          DisplayName = a.DisplayName,
        //          DbColumnName = a.DbColumnName,
        //          IsImportColumn = a.IsImportColumn
        //      }).ToList();
        //}
    }
}