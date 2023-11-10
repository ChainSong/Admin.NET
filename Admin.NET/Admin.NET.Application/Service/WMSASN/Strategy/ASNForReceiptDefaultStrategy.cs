
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.CommonCore.EnumCommon;
using Furion.DistributedIDGenerator;
using SkiaSharp;
using Admin.NET.Application.Enumerate;
using NewLife.Net;
using AutoMapper;
using Admin.NET.Common.SnowflakeCommon;

namespace Admin.NET.Application.Strategy
{
    public class ASNForReceiptDefaultStrategy : IASNForReceiptInterface
    {

        //注入数据库实例
        public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        //注入ASN仓储

        public SqlSugarRepository<WMSASN> _repASN { get; set; }

        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

        //注入仓库关系仓储

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public ASNForReceiptDefaultStrategy(

        )
        {

        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {


            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            List<WMSReceipt> receipts = new List<WMSReceipt>();
            var entityASN = _repASN.AsQueryable().Includes(a => a.Details).Where(b => request.Contains(b.Id)).ToList();

            //判断ASN  是否可以转入库单
            //1 ，判断状态是否正确
            if (entityASN.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).Count() > 0)
            {
                entityASN.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternReceiptNumber,
                        SystemOrder = b.ASNNumber,
                        Type = b.ReceiptType,
                        Msg = "状态异常"
                    });
                });


                response.Code = StatusCode.Error;
                response.Msg = "状态异常";
                return response;
            }
            //try
            //{
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSASN, WMSReceipt>()
                 //自定义投影，将ASN ID 投影到入库表中
                 .ForMember(a => a.ASNId, opt => opt.MapFrom(c => c.Id))
                 //添加创建人为当前用户
                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                 //添加入库单时间 默认入库单时间为当前时间
                 .ForMember(a => a.ReceiptTime, opt => opt.MapFrom(c => DateTime.Now))
                 //ASN 明细 转换成入库明细
                 .ForMember(a => a.Details, opt => opt.MapFrom(c => c.Details))
                 //添加入库单状态
                 .ForMember(a => a.ReceiptStatus, opt => opt.MapFrom(c => (int)ReceiptStatusEnum.新增))
                 //忽略需要转换的字段
                 .ForMember(a => a.Id, opt => opt.Ignore())
                 .ForMember(a => a.Updator, opt => opt.Ignore())
                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());

                cfg.CreateMap<WMSASNDetail, WMSReceiptDetail>()
                 //自定义投影，将ASN明细 ID 投影到入库明细表中
                 .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))
                 //默认转入同等数量的入库单
                 .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => c.ExpectedQty))
                 //添加创建人为当前用户
                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                 //忽略需要转换的字段
                 .ForMember(a => a.Id, opt => opt.Ignore())
                 .ForMember(a => a.Updator, opt => opt.Ignore())
                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
            });
            var mapper = new Mapper(config);
            receipts = mapper.Map<List<WMSReceipt>>(entityASN);
            receipts.ForEach(item =>
            {
                var ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //通过雪花ID 生成入库单号
                item.ReceiptNumber = ReceiptNumber;
                item.Details.ForEach(a => a.ReceiptNumber = item.ReceiptNumber);
            });
            //request.ForEach(item =>
            //{

            //    var Receipt = new WMS_Receipt()
            //    {
            //        //Id = item.Id,
            //        ASNId = item.Id,
            //        ASNNumber = item.ASNNumber,
            //        ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
            //        ExternReceiptNumber = item.ExternReceiptNumber,
            //        CustomerId = item.CustomerId,
            //        CustomerName = item.CustomerName,
            //        WarehouseId = item.WarehouseId,
            //        WarehouseName = item.WarehouseName,
            //        ReceiptTime = DateTime.Now,//默认当前时间
            //        ReceiptStatus = (int)ReceiptStatusEnum.新增,//默认新增
            //        ReceiptType = item.ReceiptType,
            //        Contact = item.Contact,
            //        ContactInfo = item.ContactInfo,
            //        CompleteTime = item.CompleteTime,
            //        Remark = item.Remark,
            //        Creator = AbpSession.UserName,
            //        CreationTime = DateTime.Now,
            //        //Updator = item.Updator,
            //        //UpdateTime = item.UpdateTime,
            //        Str1 = item.Str1,
            //        Str2 = item.Str2,
            //        Str3 = item.Str3,
            //        Str4 = item.Str4,
            //        Str5 = item.Str5,
            //        Str6 = item.Str6,
            //        Str7 = item.Str7,
            //        Str8 = item.Str8,
            //        Str9 = item.Str9,
            //        Str10 = item.Str10,
            //        Str11 = item.Str11,
            //        Str12 = item.Str12,
            //        Str13 = item.Str13,
            //        Str14 = item.Str14,
            //        Str15 = item.Str15,
            //        Str16 = item.Str16,
            //        Str17 = item.Str17,
            //        Str18 = item.Str18,
            //        Str19 = item.Str19,
            //        Str20 = item.Str20,
            //        DateTime1 = item.DateTime1,
            //        DateTime2 = item.DateTime2,
            //        DateTime3 = item.DateTime3,
            //        DateTime4 = item.DateTime4,
            //        DateTime5 = item.DateTime5,
            //        Int1 = item.Int1,
            //        Int2 = item.Int2,
            //        Int3 = item.Int3,
            //        Int4 = item.Int4,
            //        Int5 = item.Int5
            //    };
            //    Receipt.ReceiptDetails = new List<WMS_ReceiptDetail>();
            //    item.ASNDetails.ForEach(detail =>
            //    {
            //        Receipt.ReceiptDetails.Add(new WMS_ReceiptDetail()
            //        {
            //            //Id = detail.Id,
            //            //ReceiptId = detail.ReceiptId,
            //            ASNId = detail.ASNId,
            //            ASNDetailId = detail.Id,
            //            ReceiptNumber = Receipt.ReceiptNumber,
            //            ExternReceiptNumber = detail.ExternReceiptNumber,
            //            ASNNumber = detail.ASNNumber,
            //            CustomerId = detail.CustomerId,
            //            CustomerName = detail.CustomerName,
            //            WarehouseId = detail.WarehouseId,
            //            WarehouseName = detail.WarehouseName,
            //            LineNumber = detail.LineNumber,
            //            SKU = detail.SKU,
            //            UPC = detail.UPC,
            //            GoodsType = detail.GoodsType,
            //            GoodsName = detail.GoodsName,
            //            BoxCode = detail.BoxCode,
            //            TrayCode = detail.TrayCode,
            //            BatchCode = detail.BatchCode,
            //            ExpectedQty = detail.ExpectedQty,
            //            ReceivedQty = detail.ReceivedQty,
            //            //ReceiptQty = detail.ReceiptQty,
            //            UnitCode = detail.UnitCode,
            //            Onwer = detail.Onwer,
            //            ProductionDate = detail.ProductionDate,
            //            ExpirationDate = detail.ExpirationDate,
            //            Remark = detail.Remark,
            //            Creator = AbpSession.UserName,
            //            CreationTime = DateTime.Now,
            //            //Updator = detail.Updator,
            //            //UpdateTime = detail.UpdateTime,
            //            Str1 = detail.Str1,
            //            Str2 = detail.Str2,
            //            Str3 = detail.Str3,
            //            Str4 = detail.Str4,
            //            Str5 = detail.Str5,
            //            Str6 = detail.Str6,
            //            Str7 = detail.Str7,
            //            Str8 = detail.Str8,
            //            Str9 = detail.Str9,
            //            Str10 = detail.Str10,
            //            Str11 = detail.Str11,
            //            Str12 = detail.Str12,
            //            Str13 = detail.Str13,
            //            Str14 = detail.Str14,
            //            Str15 = detail.Str15,
            //            Str16 = detail.Str16,
            //            Str17 = detail.Str17,
            //            Str18 = detail.Str18,
            //            Str19 = detail.Str19,
            //            Str20 = detail.Str20,
            //            DateTime1 = detail.DateTime1,
            //            DateTime2 = detail.DateTime2,
            //            DateTime3 = detail.DateTime3,
            //            DateTime4 = detail.DateTime4,
            //            DateTime5 = detail.DateTime5,
            //            Int1 = detail.Int1,
            //            Int2 = detail.Int2,
            //            Int3 = detail.Int3,
            //            Int4 = detail.Int4,
            //            Int5 = detail.Int5,
            //        });
            //    });
            //    receiptDetails.AddRange(Receipt.ReceiptDetails);
            //    receipts.Add(Receipt);
            //});
            if (receipts.Count > 0)
            {
                //插入入库单
                ////开始插入订单
                await _db.InsertNav(receipts).Include(it => it.Details).ExecuteCommandAsync();

                await _repASN.UpdateAsync(it => new WMSASN() { ASNStatus = (int)ASNStatusEnum.转入库单, Updator = _userManager.Account, UpdateTime = DateTime.Now }, it => entityASN.Select(b => b.Id).Contains(it.Id));


                //await _repASNDetail.UpdateRangeAsync(_repASNDetail.AsQueryable().Where(a => receipts.Select(b => b.ASNId).Contains(a.ASNId)).Select(e => new WMSASNDetail()
                //{
                //    Id = e.Id,
                //    ReceiptQty = receipts.First(v => v.ASNId == e.ASNId).Details.Where(c => c.ASNDetailId == e.Id).Sum(g => g.ReceivedQty),
                //    Updator = _userManager.Account,
                //    UpdateTime = DateTime.Now

                //}).ToList());

                _repASNDetail.AsQueryable().Where(a => receipts.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(e =>
                {
                    e.ReceivedQty = receipts.Where(v => v.ASNId == e.ASNId).First().Details.Where(c => c.ASNDetailId == e.Id).Sum(g => g.ReceivedQty);
                    e.Updator = _userManager.Account;
                    e.UpdateTime = DateTime.Now;
                    _repASNDetail.Update(e);
                });
            }
            else
            {
                response.Code = StatusCode.Error;
                response.Msg = "系统异常";
                return response;
            }
            entityASN.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ASNNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.Success.ToString(),
                    Msg = "成功"
                }); ;
            });
            //}
            //catch (Exception er)
            //{

            //    throw;
            //}
            //response.Data = receipts;
            response.Code = StatusCode.Success;
            //throw new NotImplementedException();
            return response;
        }
    }
}


