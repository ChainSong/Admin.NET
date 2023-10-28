
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using MyProject.ReceiptReceivingCore;
//using AutoMapper;

//namespace MyProject.ReceiptCore.Strategy
//{

//    public class ReceiptDefaultStrategy : IReceiptInterface
//    {
//        /// <summary>
//        ///【WMS_Receipt】仓储层
//        /// </summary>
//        private readonly IRepository<WMS_Receipt, long> _wms_receiptRepository;

//        /// <summary>
//        ///【WMS_Receipt】领域服务
//        /// </summary>

//        private IWMS_ReceiptManager _wms_receiptManager;

//        /// <summary>
//        ///【WMS_ASN】仓储层
//        /// </summary>
//        private readonly IRepository<WMS_ASN, long> _wms_asnRepository;

//        /// <summary>
//        ///【WMS_ASN】领域服务
//        /// </summary>
//        private IWMS_ASNManager _wms_asnManager;

//        /// <summary>
//        ///【WMS_ReceiptReceiving】仓储层
//        /// </summary>
//        private readonly IRepository<WMS_ASNDetail, long> _wms_asndetailRepository;

//        public ReceiptDefaultStrategy(

//            IRepository<WMS_ASN, long> wms_asnRepository,
//            IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
//                        IRepository<WMS_Receipt, long> wms_receiptRepository
//        )
//        {
//            _wms_asnRepository = wms_asnRepository;
//            _wms_asndetailRepository = wms_asndetailRepository;
//            _wms_receiptRepository = wms_receiptRepository;
//        }

//        //默认方法不做任何处理
//        public Response<List<OrderStatusDto>> Strategy(List<WMS_ASN> request, IAbpSessionExtension AbpSession)
//        {


//            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
//            List<WMS_Receipt> receipts = new List<WMS_Receipt>();
//            //判断ASN  是否可以转入库单
//            //1 ，判断状态是否正确
//            if (request.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).Count() > 0)
//            {
//                request.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).ToList().ForEach(b =>
//                {
//                    response.Data.Add(new OrderStatusDto()
//                    {
//                        ExternOrder = b.ExternReceiptNumber,
//                        SystemOrder = b.ASNNumber,
//                        Type = b.ReceiptType,
//                        Msg = "状态异常"
//                    });
//                });


//                response.Code = StatusCode.error;
//                response.Msg = "状态异常";
//                return response;
//            }
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<WMS_ASN, WMS_Receipt>()
//                 //自定义投影，将ASN ID 投影到入库表中
//                 .ForMember(a => a.ASNId, opt => opt.MapFrom(c => c.Id))
//                 //添加创建人为当前用户
//                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => AbpSession.UserName))
//                 //添加入库单时间 默认入库单时间为当前时间
//                 .ForMember(a => a.ReceiptTime, opt => opt.MapFrom(c => DateTime.Now))

//                 //ASN 明细 转换成入库明细
//                 .ForMember(a => a.ReceiptDetails, opt => opt.MapFrom(c => c.ASNDetails))
//                 //添加入库单状态
//                 .ForMember(a => a.ReceiptStatus, opt => opt.MapFrom(c => ReceiptStatusEnum.新增))
//                 //忽略需要转换的字段
//                 .ForMember(a => a.Id, opt => opt.Ignore())
//                 .ForMember(a => a.Updator, opt => opt.Ignore())
//                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
//                cfg.CreateMap<WMS_ASNDetail, WMS_ReceiptDetail>()
//                 //自定义投影，将ASN明细 ID 投影到入库明细表中
//                 .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))
//                 //默认转入同等数量的入库单
//                 .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => c.ExpectedQty))
//                 //添加创建人为当前用户
//                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => AbpSession.UserName))
//                 //忽略需要转换的字段
//                 .ForMember(a => a.Id, opt => opt.Ignore())
//                 .ForMember(a => a.Updator, opt => opt.Ignore())
//                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
//            });
//            var mapper = new Mapper(config);
//            receipts = mapper.Map<List<WMS_Receipt>>(request);
//            receipts.ForEach(item =>
//            {
//                //通过雪花ID 生成入库单号
//                item.ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
//                item.ReceiptDetails.ForEach(a => a.ReceiptNumber = item.ReceiptNumber);
//            });
//            //request.ForEach(item =>
//            //{

//            //    var Receipt = new WMS_Receipt()
//            //    {
//            //        //Id = item.Id,
//            //        ASNId = item.Id,
//            //        ASNNumber = item.ASNNumber,
//            //        ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
//            //        ExternReceiptNumber = item.ExternReceiptNumber,
//            //        CustomerId = item.CustomerId,
//            //        CustomerName = item.CustomerName,
//            //        WarehouseId = item.WarehouseId,
//            //        WarehouseName = item.WarehouseName,
//            //        ReceiptTime = DateTime.Now,//默认当前时间
//            //        ReceiptStatus = (int)ReceiptStatusEnum.新增,//默认新增
//            //        ReceiptType = item.ReceiptType,
//            //        Contact = item.Contact,
//            //        ContactInfo = item.ContactInfo,
//            //        CompleteTime = item.CompleteTime,
//            //        Remark = item.Remark,
//            //        Creator = AbpSession.UserName,
//            //        CreationTime = DateTime.Now,
//            //        //Updator = item.Updator,
//            //        //UpdateTime = item.UpdateTime,
//            //        Str1 = item.Str1,
//            //        Str2 = item.Str2,
//            //        Str3 = item.Str3,
//            //        Str4 = item.Str4,
//            //        Str5 = item.Str5,
//            //        Str6 = item.Str6,
//            //        Str7 = item.Str7,
//            //        Str8 = item.Str8,
//            //        Str9 = item.Str9,
//            //        Str10 = item.Str10,
//            //        Str11 = item.Str11,
//            //        Str12 = item.Str12,
//            //        Str13 = item.Str13,
//            //        Str14 = item.Str14,
//            //        Str15 = item.Str15,
//            //        Str16 = item.Str16,
//            //        Str17 = item.Str17,
//            //        Str18 = item.Str18,
//            //        Str19 = item.Str19,
//            //        Str20 = item.Str20,
//            //        DateTime1 = item.DateTime1,
//            //        DateTime2 = item.DateTime2,
//            //        DateTime3 = item.DateTime3,
//            //        DateTime4 = item.DateTime4,
//            //        DateTime5 = item.DateTime5,
//            //        Int1 = item.Int1,
//            //        Int2 = item.Int2,
//            //        Int3 = item.Int3,
//            //        Int4 = item.Int4,
//            //        Int5 = item.Int5
//            //    };
//            //    Receipt.ReceiptDetails = new List<WMS_ReceiptDetail>();
//            //    item.ASNDetails.ForEach(detail =>
//            //    {
//            //        Receipt.ReceiptDetails.Add(new WMS_ReceiptDetail()
//            //        {
//            //            //Id = detail.Id,
//            //            //ReceiptId = detail.ReceiptId,
//            //            ASNId = detail.ASNId,
//            //            ASNDetailId = detail.Id,
//            //            ReceiptNumber = Receipt.ReceiptNumber,
//            //            ExternReceiptNumber = detail.ExternReceiptNumber,
//            //            ASNNumber = detail.ASNNumber,
//            //            CustomerId = detail.CustomerId,
//            //            CustomerName = detail.CustomerName,
//            //            WarehouseId = detail.WarehouseId,
//            //            WarehouseName = detail.WarehouseName,
//            //            LineNumber = detail.LineNumber,
//            //            SKU = detail.SKU,
//            //            UPC = detail.UPC,
//            //            GoodsType = detail.GoodsType,
//            //            GoodsName = detail.GoodsName,
//            //            BoxCode = detail.BoxCode,
//            //            TrayCode = detail.TrayCode,
//            //            BatchCode = detail.BatchCode,
//            //            ExpectedQty = detail.ExpectedQty,
//            //            ReceivedQty = detail.ReceivedQty,
//            //            //ReceiptQty = detail.ReceiptQty,
//            //            UnitCode = detail.UnitCode,
//            //            Onwer = detail.Onwer,
//            //            ProductionDate = detail.ProductionDate,
//            //            ExpirationDate = detail.ExpirationDate,
//            //            Remark = detail.Remark,
//            //            Creator = AbpSession.UserName,
//            //            CreationTime = DateTime.Now,
//            //            //Updator = detail.Updator,
//            //            //UpdateTime = detail.UpdateTime,
//            //            Str1 = detail.Str1,
//            //            Str2 = detail.Str2,
//            //            Str3 = detail.Str3,
//            //            Str4 = detail.Str4,
//            //            Str5 = detail.Str5,
//            //            Str6 = detail.Str6,
//            //            Str7 = detail.Str7,
//            //            Str8 = detail.Str8,
//            //            Str9 = detail.Str9,
//            //            Str10 = detail.Str10,
//            //            Str11 = detail.Str11,
//            //            Str12 = detail.Str12,
//            //            Str13 = detail.Str13,
//            //            Str14 = detail.Str14,
//            //            Str15 = detail.Str15,
//            //            Str16 = detail.Str16,
//            //            Str17 = detail.Str17,
//            //            Str18 = detail.Str18,
//            //            Str19 = detail.Str19,
//            //            Str20 = detail.Str20,
//            //            DateTime1 = detail.DateTime1,
//            //            DateTime2 = detail.DateTime2,
//            //            DateTime3 = detail.DateTime3,
//            //            DateTime4 = detail.DateTime4,
//            //            DateTime5 = detail.DateTime5,
//            //            Int1 = detail.Int1,
//            //            Int2 = detail.Int2,
//            //            Int3 = detail.Int3,
//            //            Int4 = detail.Int4,
//            //            Int5 = detail.Int5,
//            //        });
//            //    });
//            //    receiptDetails.AddRange(Receipt.ReceiptDetails);
//            //    receipts.Add(Receipt);
//            //});
//            if (receipts.Count > 0)
//            {
//                //插入入库单
//                _wms_receiptRepository.GetDbContext().BulkInsert(receipts, options => options.IncludeGraph = true);
//                //修改预入库单状态
//                _wms_asnRepository.GetAll().Where(a => request.Select(b => b.Id).Contains(a.Id)).BatchUpdate(new WMS_ASN() { ASNStatus = (int)ASNStatusEnum.转入库单, Updator = AbpSession.UserName, UpdateTime = DateTime.Now });
//                //修改预入库单明细收获信息
//                _wms_asndetailRepository.GetAll().Where(a => receipts.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(e =>
//                {
//                    e.ReceivedQty = receipts.Where(v => v.ASNId == e.ASNId).First().ReceiptDetails.Where(c => c.ASNDetailId == e.Id).Sum(g => g.ReceivedQty);
//                    e.Updator = AbpSession.UserName;
//                    e.UpdateTime = DateTime.Now;
//                    _wms_asndetailRepository.Update(e);
//                });
//            }
//            else
//            {
//                response.Code = StatusCode.error;
//                response.Msg = "系统异常";
//                return response;
//            }
//            request.ToList().ForEach(b =>
//            {
//                response.Data.Add(new OrderStatusDto()
//                {
//                    ExternOrder = b.ExternReceiptNumber,
//                    SystemOrder = b.ASNNumber,
//                    Type = b.ReceiptType,
//                    StatusCode = StatusCode.success,
//                    //StatusMsg = StatusCode.success.ToString(),
//                    Msg = "成功"
//                }); ;
//            });

//            //response.Data = receipts;
//            response.Code = StatusCode.success;
//            //throw new NotImplementedException();
//            return response;
//        }
//    }
//}


////using Abp.Application.Services;
////using Abp.Domain.Repositories;
////using Abp.Runtime.Session;
////using MyProject.ASNCore;
////using MyProject.ASNCore.DomainService;
////using MyProject.ASNCore.Dtos;
////using MyProject.Common.SnowflakeCommon;
////using MyProject.CommonCore;
////using MyProject.Dtos;
////using MyProject.ReceiptCore.DomainService;
////using MyProject.ReceiptCore.Dtos;
////using MyProject.ReceiptCore.Interface;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using Abp.EntityFrameworkCore.Repositories;
////using MyProject.Sessions.AbpSessionExtension;
////using EFCore.BulkExtensions;
////using MyProject.ReceiptCore.Enumerate;
////using MyProject.ASNCore.Enumerate;
////using MyProject.ReceiptReceivingCore;
////using AutoMapper;

////namespace MyProject.ReceiptCore.Strategy
////{

////    public class ReceiptDefaultStrategy : IReceiptInterface
////    {
////        /// <summary>
////        ///【WMS_Receipt】仓储层
////        /// </summary>
////        private readonly IRepository<WMS_Receipt, long> _wms_receiptRepository;

////        /// <summary>
////        ///【WMS_Receipt】领域服务
////        /// </summary>

////        private IWMS_ReceiptManager _wms_receiptManager;

////        /// <summary>
////        ///【WMS_ASN】仓储层
////        /// </summary>
////        private readonly IRepository<WMS_ASN, long> _wms_asnRepository;

////        /// <summary>
////        ///【WMS_ASN】领域服务
////        /// </summary>
////        private IWMS_ASNManager _wms_asnManager;

////        /// <summary>
////        ///【WMS_ReceiptReceiving】仓储层
////        /// </summary>
////        private readonly IRepository<WMS_ASNDetail, long> _wms_asndetailRepository;

////        public ReceiptDefaultStrategy(

////            IRepository<WMS_ASN, long> wms_asnRepository,
////            IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
////                        IRepository<WMS_Receipt, long> wms_receiptRepository
////        )
////        {
////            _wms_asnRepository = wms_asnRepository;
////            _wms_asndetailRepository = wms_asndetailRepository;
////            _wms_receiptRepository = wms_receiptRepository;
////        }

////        //默认方法不做任何处理
////        public Response<List<OrderStatusDto>> Strategy(List<WMS_ASN> request, IAbpSessionExtension AbpSession)
////        {


////            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
////            List<WMS_Receipt> receipts = new List<WMS_Receipt>();
////            //判断ASN  是否可以转入库单
////            //1 ，判断状态是否正确
////            if (request.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).Count() > 0)
////            {
////                request.Where(a => a.ASNStatus != 1 && a.ASNStatus != 5).ToList().ForEach(b =>
////                {
////                    response.Data.Add(new OrderStatusDto()
////                    {
////                        ExternOrder = b.ExternReceiptNumber,
////                        SystemOrder = b.ASNNumber,
////                        Type = b.ReceiptType,
////                        Msg = "状态异常"
////                    });
////                });


////                response.Code = StatusCode.error;
////                response.Msg = "状态异常";
////                return response;
////            }
////            var config = new MapperConfiguration(cfg =>
////            {
////                cfg.CreateMap<WMS_ASN, WMS_Receipt>()
////                 //自定义投影，将ASN ID 投影到入库表中
////                 .ForMember(a => a.ASNId, opt => opt.MapFrom(c => c.Id))
////                 //添加创建人为当前用户
////                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => AbpSession.UserName))
////                 //添加入库单时间 默认入库单时间为当前时间
////                 .ForMember(a => a.ReceiptTime, opt => opt.MapFrom(c => DateTime.Now))

////                 //ASN 明细 转换成入库明细
////                 .ForMember(a => a.ReceiptDetails, opt => opt.MapFrom(c => c.ASNDetails))
////                 //添加入库单状态
////                 .ForMember(a => a.ReceiptStatus, opt => opt.MapFrom(c => ReceiptStatusEnum.新增))
////                 //忽略需要转换的字段
////                 .ForMember(a => a.Id, opt => opt.Ignore())
////                 .ForMember(a => a.Updator, opt => opt.Ignore())
////                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
////                cfg.CreateMap<WMS_ASNDetail, WMS_ReceiptDetail>()
////                 //自定义投影，将ASN明细 ID 投影到入库明细表中
////                 .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))
////                 //默认转入同等数量的入库单
////                 .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => c.ExpectedQty))
////                 //添加创建人为当前用户
////                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => AbpSession.UserName))
////                 //忽略需要转换的字段
////                 .ForMember(a => a.Id, opt => opt.Ignore())
////                 .ForMember(a => a.Updator, opt => opt.Ignore())
////                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
////            });
////            var mapper = new Mapper(config);
////            receipts = mapper.Map<List<WMS_Receipt>>(request);
////            receipts.ForEach(item =>
////            {
////                //通过雪花ID 生成入库单号
////                item.ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
////                item.ReceiptDetails.ForEach(a => a.ReceiptNumber = item.ReceiptNumber);
////            });
////            //request.ForEach(item =>
////            //{

////            //    var Receipt = new WMS_Receipt()
////            //    {
////            //        //Id = item.Id,
////            //        ASNId = item.Id,
////            //        ASNNumber = item.ASNNumber,
////            //        ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString(),
////            //        ExternReceiptNumber = item.ExternReceiptNumber,
////            //        CustomerId = item.CustomerId,
////            //        CustomerName = item.CustomerName,
////            //        WarehouseId = item.WarehouseId,
////            //        WarehouseName = item.WarehouseName,
////            //        ReceiptTime = DateTime.Now,//默认当前时间
////            //        ReceiptStatus = (int)ReceiptStatusEnum.新增,//默认新增
////            //        ReceiptType = item.ReceiptType,
////            //        Contact = item.Contact,
////            //        ContactInfo = item.ContactInfo,
////            //        CompleteTime = item.CompleteTime,
////            //        Remark = item.Remark,
////            //        Creator = AbpSession.UserName,
////            //        CreationTime = DateTime.Now,
////            //        //Updator = item.Updator,
////            //        //UpdateTime = item.UpdateTime,
////            //        Str1 = item.Str1,
////            //        Str2 = item.Str2,
////            //        Str3 = item.Str3,
////            //        Str4 = item.Str4,
////            //        Str5 = item.Str5,
////            //        Str6 = item.Str6,
////            //        Str7 = item.Str7,
////            //        Str8 = item.Str8,
////            //        Str9 = item.Str9,
////            //        Str10 = item.Str10,
////            //        Str11 = item.Str11,
////            //        Str12 = item.Str12,
////            //        Str13 = item.Str13,
////            //        Str14 = item.Str14,
////            //        Str15 = item.Str15,
////            //        Str16 = item.Str16,
////            //        Str17 = item.Str17,
////            //        Str18 = item.Str18,
////            //        Str19 = item.Str19,
////            //        Str20 = item.Str20,
////            //        DateTime1 = item.DateTime1,
////            //        DateTime2 = item.DateTime2,
////            //        DateTime3 = item.DateTime3,
////            //        DateTime4 = item.DateTime4,
////            //        DateTime5 = item.DateTime5,
////            //        Int1 = item.Int1,
////            //        Int2 = item.Int2,
////            //        Int3 = item.Int3,
////            //        Int4 = item.Int4,
////            //        Int5 = item.Int5
////            //    };
////            //    Receipt.ReceiptDetails = new List<WMS_ReceiptDetail>();
////            //    item.ASNDetails.ForEach(detail =>
////            //    {
////            //        Receipt.ReceiptDetails.Add(new WMS_ReceiptDetail()
////            //        {
////            //            //Id = detail.Id,
////            //            //ReceiptId = detail.ReceiptId,
////            //            ASNId = detail.ASNId,
////            //            ASNDetailId = detail.Id,
////            //            ReceiptNumber = Receipt.ReceiptNumber,
////            //            ExternReceiptNumber = detail.ExternReceiptNumber,
////            //            ASNNumber = detail.ASNNumber,
////            //            CustomerId = detail.CustomerId,
////            //            CustomerName = detail.CustomerName,
////            //            WarehouseId = detail.WarehouseId,
////            //            WarehouseName = detail.WarehouseName,
////            //            LineNumber = detail.LineNumber,
////            //            SKU = detail.SKU,
////            //            UPC = detail.UPC,
////            //            GoodsType = detail.GoodsType,
////            //            GoodsName = detail.GoodsName,
////            //            BoxCode = detail.BoxCode,
////            //            TrayCode = detail.TrayCode,
////            //            BatchCode = detail.BatchCode,
////            //            ExpectedQty = detail.ExpectedQty,
////            //            ReceivedQty = detail.ReceivedQty,
////            //            //ReceiptQty = detail.ReceiptQty,
////            //            UnitCode = detail.UnitCode,
////            //            Onwer = detail.Onwer,
////            //            ProductionDate = detail.ProductionDate,
////            //            ExpirationDate = detail.ExpirationDate,
////            //            Remark = detail.Remark,
////            //            Creator = AbpSession.UserName,
////            //            CreationTime = DateTime.Now,
////            //            //Updator = detail.Updator,
////            //            //UpdateTime = detail.UpdateTime,
////            //            Str1 = detail.Str1,
////            //            Str2 = detail.Str2,
////            //            Str3 = detail.Str3,
////            //            Str4 = detail.Str4,
////            //            Str5 = detail.Str5,
////            //            Str6 = detail.Str6,
////            //            Str7 = detail.Str7,
////            //            Str8 = detail.Str8,
////            //            Str9 = detail.Str9,
////            //            Str10 = detail.Str10,
////            //            Str11 = detail.Str11,
////            //            Str12 = detail.Str12,
////            //            Str13 = detail.Str13,
////            //            Str14 = detail.Str14,
////            //            Str15 = detail.Str15,
////            //            Str16 = detail.Str16,
////            //            Str17 = detail.Str17,
////            //            Str18 = detail.Str18,
////            //            Str19 = detail.Str19,
////            //            Str20 = detail.Str20,
////            //            DateTime1 = detail.DateTime1,
////            //            DateTime2 = detail.DateTime2,
////            //            DateTime3 = detail.DateTime3,
////            //            DateTime4 = detail.DateTime4,
////            //            DateTime5 = detail.DateTime5,
////            //            Int1 = detail.Int1,
////            //            Int2 = detail.Int2,
////            //            Int3 = detail.Int3,
////            //            Int4 = detail.Int4,
////            //            Int5 = detail.Int5,
////            //        });
////            //    });
////            //    receiptDetails.AddRange(Receipt.ReceiptDetails);
////            //    receipts.Add(Receipt);
////            //});
////            if (receipts.Count > 0)
////            {
////                //插入入库单
////                _wms_receiptRepository.GetDbContext().BulkInsert(receipts, options => options.IncludeGraph = true);
////                //修改预入库单状态
////                _wms_asnRepository.GetAll().Where(a => request.Select(b => b.Id).Contains(a.Id)).BatchUpdate(new WMS_ASN() { ASNStatus = (int)ASNStatusEnum.转入库单, Updator = AbpSession.UserName, UpdateTime = DateTime.Now });
////                //修改预入库单明细收获信息
////                _wms_asndetailRepository.GetAll().Where(a => receipts.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(e =>
////                {
////                    e.ReceivedQty = receipts.Where(v => v.ASNId == e.ASNId).First().ReceiptDetails.Where(c => c.ASNDetailId == e.Id).Sum(g => g.ReceivedQty);
////                    e.Updator = AbpSession.UserName;
////                    e.UpdateTime = DateTime.Now;
////                    _wms_asndetailRepository.Update(e);
////                });
////            }
////            else
////            {
////                response.Code = StatusCode.error;
////                response.Msg = "系统异常";
////                return response;
////            }
////            request.ToList().ForEach(b =>
////            {
////                response.Data.Add(new OrderStatusDto()
////                {
////                    ExternOrder = b.ExternReceiptNumber,
////                    SystemOrder = b.ASNNumber,
////                    Type = b.ReceiptType,
////                    StatusCode = StatusCode.success,
////                    //StatusMsg = StatusCode.success.ToString(),
////                    Msg = "成功"
////                }); ;
////            });

////            //response.Data = receipts;
////            response.Code = StatusCode.success;
////            //throw new NotImplementedException();
////            return response;
////        }
////    }
////}


