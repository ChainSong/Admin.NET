
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
//using MyProject.ReceiptReceivingCore.Dto;
using Admin.NET.Application.Dtos.Enum;

using SkiaSharp;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Common.SnowflakeCommon;
using FluentEmail.Core;

namespace Admin.NET.Application.Strategy
{
    public class MReceiptReceivingDefaultStrategy : IMReceiptReceivingInterface
    {

        //public ISqlSugarClient _db { get; set; }

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
        //public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

        public MReceiptReceivingDefaultStrategy(

        )
        {

        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<MMSReceiptReceivingDetail> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //解析DataTable
            //response.Data = receipts;
            //上架方法是将 入库单的数据，按照每一行拆开添加 库区库位之后 整合数据插入到上架表
            //按照订单号，SKU,批次号，行号的方式整合数据 ：系统约定入库单每一单的最小颗粒度为 SKU+LineNumber
            List<MMSReceiptReceivingDetail> receiptReceivingDetails = new List<MMSReceiptReceivingDetail>();
            List<MMSReceiptReceiving> receiptReceivings = new List<MMSReceiptReceiving>();

            //使用 一个临时变量作为缓存使用，减少数据库访问次数，且减少内存消耗
            //WMSReceipt receiptOrderTemp = new WMSReceipt();

            var supplierCheck = _repSupplierUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.SupplierName).ToList().Contains(a.SupplierName)).ToList();
            if (supplierCheck.GroupBy(a => a.SupplierName).Count() != request.GroupBy(a => a.SupplierName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少供应商操作权限";
                return response;
            }

            //先判断是否能操作仓库
            var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
            if (warehouseCheck.GroupBy(a => a.WarehouseName).Count() != request.GroupBy(a => a.WarehouseName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少仓库操作权限";
                return response;
            }

            //获取同订单下的数据作为校验
            var checkData = await _repMReceipt.AsQueryable().Includes(a => a.Details).Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToListAsync();


            //var config = new MapperConfiguration(cfg => cfg.CreateMap<WMSReceiptDetail, WMSReceiptReceiving>());
            //var mapper = new Mapper(config);

            var config = new MapperConfiguration(cfg =>
            {
                //cfg => cfg.CreateMap<MMSReceiptDetail, MMSReceiptReceiving>(),
                cfg.CreateMap<MMSReceipt, MMSReceiptReceiving>()
            //自定义投影，将上架表ID 投影到库存表中
            .ForMember(a => a.ReceiptId, opt => opt.MapFrom(c => c.Id))
            //添加创建人为当前用户
            .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
            //创建时间
            .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //.ForMember(a => a.ReceiptReceivingTime, opt => opt.MapFrom(c => DateTime.Now))
            .ForMember(a => a.ReceiptReceivingStartTime, opt => opt.MapFrom(c => DateTime.Now))
            .ForMember(a => a.ReceiptReceivingEndTime, opt => opt.MapFrom(c => DateTime.Now))
            //数量
            //.ForMember(a => a.Qty, opt => opt.MapFrom(c => c.qty))
            //添加库存状态为可用
            .ForMember(a => a.ReceiptReceivingStatus, opt => opt.MapFrom(c => (int)MReceiptReceivingStatusEnum.已上架))
            .ForMember(a => a.ReceiptReceivingType, opt => opt.MapFrom(c => (int)MReceiptReceivingTypeEnum.正常上架))
            //.ForMember(a => a.InventoryStatus, opt => opt.MapFrom(c => 1))
            //将为Null的字段设置为"" () 
            .AddTransform<string>(a => a == null ? "" : a)
            .AddTransform<int?>(a => a == null ? 0 : a)
            .AddTransform<double?>(a => a == null ? 0 : a)
            //.AddTransform<DateTime?>(a => a is null ? null : )
            //忽略空值映射
            //.IgnoreNullValues(true)
            //忽略需要转换的字段 
            //.ForMember(a => a.Id, opt => opt.Ignore())
            .ForMember(a => a.Id, opt => opt.MapFrom(c => 0))
            .ForMember(a => a.Updator, opt => opt.Ignore())
            .ForMember(a => a.UpdateTime, opt => opt.Ignore());
                cfg.CreateMap<MMSReceiptDetail, MMSReceiptReceivingDetail>()
                 .AddTransform<string>(a => a == null ? "" : a)
                 .AddTransform<int?>(a => a == null ? 0 : a)
                 .AddTransform<double?>(a => a == null ? 0 : a)
                 .ForMember(a => a.GoodsStatus, opt => opt.MapFrom(c => (int)MGoodsStatusEnum.正常))
                 .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                 .ForMember(a => a.ReceiptId, opt => opt.MapFrom(c => c.Id))
                 .ForMember(a => a.Id, opt => opt.MapFrom(c => 0))
                 //忽略需要转换的字段 
                 .ForMember(a => a.Updator, opt => opt.Ignore())
                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
                //.ForMember(a => a.ReceiptDetailId, opt => opt.MapFrom(c => c.Id));
            });

            //config.

            var mapper = new Mapper(config);
            //receiptReceivings = checkData.Adapt<List<MMSReceiptReceiving>>();
            receiptReceivings = mapper.Map<List<MMSReceiptReceiving>>(checkData);

            foreach (var item in request)
            {
                var receiptOrderTemp = checkData.Where(a => a.ReceiptNumber == item.ReceiptNumber).First();
                if (receiptOrderTemp == null || receiptOrderTemp.ReceiptNumber != item.ReceiptNumber)
                {

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

                var receiptOrderLineData = receiptOrderTemp.Details.Where(a => a.SKU == item.SKU).FirstOrDefault();

                if (receiptOrderLineData != null)
                {

                    var dto = mapper.Map<MMSReceiptReceivingDetail>(receiptOrderLineData);

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
                    dto.GoodsStatus = (int)GoodsStatusEnum.正常;
                    dto.ReceivedQty = item.ReceivedQty;
                    dto.ReceiptDetailId = receiptOrderLineData.Id;
                    //dto.ReceiptReceivingStatus = (int)ReceiptReceivingStatusEnum.上架;
                    receiptReceivingDetails.Add(dto);
                }
                else
                {
                    continue;
                }
            }


            foreach (var item in receiptReceivings)
            {
                var receiptReceivingNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                item.ReceiptReceivingNumber = receiptReceivingNumber;
                var receiptReceivingDetailData = receiptReceivingDetails.Where(a => a.ReceiptNumber == item.ReceiptNumber);
                receiptReceivingDetailData.ForEach(a =>
                {
                    a.ReceiptReceivingNumber = receiptReceivingNumber;
                });
                item.Details = receiptReceivingDetailData.ToList();
            }
            //先删除原来的单子

            await _repMReceiptReceivingDetail.DeleteAsync(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            await _repMReceiptReceiving.DeleteAsync(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            await _repMReceiptReceiving.Context.InsertNav(receiptReceivings).Include(a => a.Details).ExecuteCommandAsync();
            var receiptData = _repMReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToList();
            receiptData.ForEach(c =>
            {
                c.ReceiptStatus = (int)MReceiptReceivingStatusEnum.已上架;
            });
            await _repMReceipt.UpdateRangeAsync(receiptData);
            //await _repReceipt.AsUpdateable(receiptData).ExecuteCommandAsync();

            //.BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //修改入库单的上架数量
            //_wms_receiptdetailRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(a => new WMS_ReceiptDetail { ReceivedQty = _wms_receiptreceivingRepository.GetAll().Where(re => re.ReceiptDetailId == a.Id).Sum(c => c.ReceivedQty) });
            var checkDataTemp = _repMReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));

            checkDataTemp.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ReceiptNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单上架成功"
                });

            });

            //if (response.Data.Count > 0)
            //{
            //    response.Code = StatusCode.Error;
            //    response.Msg = "失败";
            //    //throw new NotImplementedException();
            //    return response;
            //}
            //删除已经上架的明细


            //_repMReceiptReceiving.Delete(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            //_repMReceiptReceiving.Delete(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));
            //上架明细添加到上架表
            //await _repReceiptReceiving.InsertRangeAsync(receiptReceivings.ToList());

            //await _repReceiptReceiving.AsInsertable(receiptReceivings).ExecuteCommandAsync();
            //修改入库单的状态
            //_wms_receiptManager.Query().Where(a => (request as List<WMS_ReceiptReceivingEditDto>).Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //var receiptData = _repReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber)).ToList();

            //receiptData.ForEach(c =>
            //{
            //    c.ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架;
            //});
            //await _repReceipt.UpdateRangeAsync(receiptData);
            //await _repReceipt.AsUpdateable(receiptData).ExecuteCommandAsync();

            //.BatchUpdate(new WMS_Receipt { ReceiptStatus = (int)ReceiptReceivingStatusEnum.上架 });
            //修改入库单的上架数量
            //_wms_receiptdetailRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(a => new WMS_ReceiptDetail { ReceivedQty = _wms_receiptreceivingRepository.GetAll().Where(re => re.ReceiptDetailId == a.Id).Sum(c => c.ReceivedQty) });
            //var checkDataTemp = _repReceipt.AsQueryable().Where(a => request.Select(b => b.ReceiptNumber).Contains(a.ReceiptNumber));

            //checkDataTemp.ToList().ForEach(b =>
            //{
            //    response.Data.Add(new OrderStatusDto()
            //    {
            //        ExternOrder = b.ExternReceiptNumber,
            //        SystemOrder = b.ReceiptNumber,
            //        Type = b.ReceiptType,
            //        StatusCode = StatusCode.Success,
            //        //StatusMsg = StatusCode.warning.ToString(),
            //        Msg = "订单上架成功"
            //    });

            //});
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