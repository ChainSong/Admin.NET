
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
using Furion.DistributedIDGenerator;
using SkiaSharp;
using Admin.NET.Application.Enumerate;
using NewLife.Net;
using AutoMapper;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Application.Service;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;
using Admin.NET.Core.Service;
using SpliteToBox.Common;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;

namespace Admin.NET.Application.Strategy
{
    public class ASNForReceiptHachStrategy : IASNForReceiptInterface
    {

        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        //注入ASN仓储
        public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }


        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        //注入仓库关系仓储

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public SysCacheService _sysCacheService { get; set; }

        public ASNForReceiptHachStrategy()
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
                 .AddTransform<string>(a => a == null ? "" : a)
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
                 .AddTransform<string>(a => a == null ? "" : a)
                 //自定义投影，将ASN明细 ID 投影到入库明细表中
                 .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))
                 //默认转入同等数量的入库单
                 .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => c.ExpectedQty - c.ReceivedQty))
                 .ForMember(a => a.ReceiptQty, opt => opt.MapFrom(c => 0))
                 //添加创建人为当前用户
                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                 //忽略需要转换的字段
                 .ForMember(a => a.Id, opt => opt.Ignore())
                 .ForMember(a => a.Updator, opt => opt.Ignore())
                 .ForMember(a => a.UpdateTime, opt => opt.Ignore());
            });
            var mapper = new Mapper(config);
            receipts = mapper.Map<List<WMSReceipt>>(entityASN);


            return await Save(receipts, entityASN);
        }





        public async Task<Response<List<OrderStatusDto>>> StrategyPart(List<WMSASNForReceiptDetailDto> request)
        {


            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            List<WMSReceipt> receipts = new List<WMSReceipt>();
            var entityASN = _repASN.AsQueryable().Includes(a => a.Details).Where(b => request.Select(a => a.ASNId).Contains(b.Id)).ToList();
            //判断ASN  是否可以转入库单
            //1 ，判断状态是否正确
            if (entityASN.Where(a => a.ASNStatus != (int)ASNStatusEnum.新增 && a.ASNStatus != (int)ASNStatusEnum.部分转入库单).Count() > 0)
            {
                entityASN.Where(a => a.ASNStatus != (int)ASNStatusEnum.新增 && a.ASNStatus != (int)ASNStatusEnum.部分转入库单).ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternReceiptNumber,
                        SystemOrder = b.ASNNumber,
                        Type = b.ReceiptType,
                        Msg = "当前状态不允许操作"
                    });
                });
                response.Code = StatusCode.Error;
                response.Msg = "当前状态不允许操作";
                return response;
            }
            //try
            //{
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSASN, WMSReceipt>()
                 .AddTransform<string>(a => a == null ? "" : a)
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
                 .AddTransform<string>(a => a == null ? "" : a)
                 //自定义投影，将ASN明细 ID 投影到入库明细表中
                 .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))
                 //默认转入同等数量的入库单(部分转入库，取传入的数量)
                 .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => request.Where(d => d.Id == c.Id).Select(d => d.ForReceiptQty).First()))
                 .ForMember(a => a.ReceiptQty, opt => opt.MapFrom(c => 0))
                 //添加创建人为当前用户
                 .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                 //忽略需要转换的字段
                 .ForMember(a => a.Id, opt => opt.Ignore())
                 .ForMember(a => a.Updator, opt => opt.Ignore())
                 .ForMember(a => a.UpdateTime, opt => opt.Ignore())
                 ;
            });
            var mapper = new Mapper(config);
            receipts = mapper.Map<List<WMSReceipt>>(entityASN);
            //针对性补充一些数据
            foreach (var item in receipts)
            {
                foreach (var itemDetail in item.Details)
                {
                    itemDetail.BatchCode = request.Where(d => d.Id == itemDetail.ASNDetailId).Select(d => d.BatchCode).First();
                }
            }
            return await Save(receipts, entityASN);
        }

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="receipts"></param>
        /// <param name="entityASN"></param>
        /// <returns></returns>
        private async Task<Response<List<OrderStatusDto>>> Save(List<WMSReceipt> receipts, List<WMSASN> entityASN)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
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

                //判断是不是存在不需要转入库单的明细
                if (item.Details.Where(a => a.ReceivedQty > 0).Count() == 0)
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = entityASN.First().ExternReceiptNumber,
                        SystemOrder = entityASN.First().ASNNumber,
                        Type = entityASN.First().ReceiptType,
                        Msg = "没有可转入库单的明细"
                    });
                    response.Code = StatusCode.Error;
                    response.Msg = "有无需转入库单的预入库单";
                }
            });
            if (response.Data != null && response.Data.Count > 0)
            {
                return response;
            }
            if (receipts.Count > 0)
            {
                receipts.ForEach(item =>
                {
                    item.Details = item.Details.Where(a => a.ReceivedQty > 0).ToList();
                });
                //插入入库单
                ////开始插入订单（排除数量为0的明细行）
                await _repReceipt.Context.InsertNav(receipts).Include(it => it.Details).ExecuteCommandAsync();

                //await _repASN.UpdateAsync(it => new WMSASN() { ASNStatus = (int)ASNStatusEnum.转入库单, Updator = _userManager.Account, UpdateTime = DateTime.Now }, it => entityASN.Select(b => b.Id).Contains(it.Id));
                var asnList = await _repASNDetail.AsQueryable().Where(a => receipts.Select(b => b.ASNId).Contains(a.ASNId)).ToListAsync();
                asnList.ForEach(e =>
                {
                    e.ReceivedQty += receipts.Where(v => v.ASNId == e.ASNId).First().Details.Where(c => c.ASNDetailId == e.Id).Sum(g => g.ReceivedQty);
                    e.Updator = _userManager.Account;
                    e.UpdateTime = DateTime.Now;

                });
                await _repASNDetail.UpdateRangeAsync(asnList);
                //_repASNDetail.Update(e);
                //判断还有没有明细行订单数量和入库数量不一致的，如果有，则更新状态为部分转入库单
                if (asnList.Where(a => a.ReceivedQty != a.ExpectedQty).Count() > 0)
                {
                    await _repASN.Context.Updateable<WMSASN>()
                     .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.部分转入库单)
                     .Where(p => entityASN.Select(b => b.Id).Contains(p.Id))
                     .ExecuteCommandAsync();
                }
                else
                {
                    await _repASN.Context.Updateable<WMSASN>()
                      .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.转入库单)
                      .Where(p => entityASN.Select(b => b.Id).Contains(p.Id))
                      .ExecuteCommandAsync();
                }
                await SaveRFID(receipts, entityASN);
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
            return response;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 创建RFID打印序列
        /// </summary>
        /// <param name="receipts"></param>
        /// <param name="entityASN"></param>
        /// <returns></returns>
        private async Task<Response<List<OrderStatusDto>>> SaveRFID(List<WMSReceipt> receipts, List<WMSASN> entityASN)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };


            //获取需要生成RFID序列的入库单明细信息
            var rfidReceiptDetails = _repReceiptDetail.AsQueryable()
                .Where(a => receipts.Select(b => b.Id).Contains(a.ReceiptId)
                 && SqlFunc.Subqueryable<WMSProduct>().Where(c => c.SKU == a.SKU && c.IsRFID == 1 && c.CustomerId == a.CustomerId).Any()
            ).ToList();

            //获取product
            var products = _repProduct.AsQueryable().Where(a => a.IsRFID == 1 && rfidReceiptDetails.Select(b => b.SKU).Contains(a.SKU)).ToList();

            //构建RFID序列容器
            List<WMSRFIDInfo> rfidInfos = new List<WMSRFIDInfo>();
            //根据入库单号生成RFID打印序列号
            //SKU唯一序列号根据SKUID+客户编码+唯一序列+三位随机数+随机数%10
            foreach (var item in rfidReceiptDetails)
            {
                for (var i = 0; i < item.ReceivedQty; i++)
                {
                    var skuId = products.Where(a => a.SKU == item.SKU).First().Id;
                    var customerCode = item.CustomerId;
                    var warehouseCode = item.WarehouseId;
                    long uniqueCode;
                    RedisCacheHelper.IncrementValue("RFID_UNIQUE_CODE", out uniqueCode);
                    var randomCode = new Random(Guid.NewGuid().GetHashCode()).Next(100, 999);
                    var rfidCode = skuId.ToString().PadLeft(7, '0') + "" + customerCode.ToString().PadLeft(3, '0') + "" + warehouseCode.ToString().PadLeft(3, '0') + "" + uniqueCode.ToString().PadLeft(7, '0') + "" + randomCode + "" + (randomCode % 10).ToString();
                    var rfidInfo = item.Adapt<WMSRFIDInfo>();
                    rfidInfo.ReceiptDetailId = item.Id;
                    rfidInfo.Id = 0;
                    rfidInfo.Creator = _userManager.Account;
                    rfidInfo.CreationTime = DateTime.Now;
                    rfidInfo.ReceiptPerson = _userManager.Account;
                    rfidInfo.ReceiptTime = DateTime.Now;
                    rfidInfo.Qty = 1;
                    rfidInfo.Status = 1;
                    rfidInfo.Sequence = rfidCode;
                    rfidInfo.RFID = rfidCode;
                    rfidInfo.Link = "";
                    rfidInfo.Remark = "";
                    rfidInfos.Add(rfidInfo);
                }
            }
            //插入入库单
            ////开始插入订单（排除数量为0的明细行）
            if (rfidInfos.Count > 0)
            {
                await _repRFIDInfo.InsertRangeAsync(rfidInfos);
            }
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
        }
    }
}


