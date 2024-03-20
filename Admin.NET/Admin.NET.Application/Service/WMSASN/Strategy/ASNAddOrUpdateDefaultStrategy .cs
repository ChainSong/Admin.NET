
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
using Furion.DistributedIDGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Strategy
{
    public class ASNAddOrUpdateDefaultStrategy : IASNInterface
    {

        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

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

        public ASNAddOrUpdateDefaultStrategy()
        {
        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSASNInput> request)

        /// <summary>
        /// 默认方法不做任何处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateWMSASNInput> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();

            //判断是否有权限操作
            //先判断是否能操作客户
            var customerCheck = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.CustomerName).ToList().Contains(a.CustomerName)).ToList();
            if (customerCheck.GroupBy(a => a.CustomerName).Count() != request.GroupBy(a => a.CustomerName).Count())
            {
                if (request.Where(a => string.IsNullOrEmpty(a.CustomerName)).Count() > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "存在空行或者客户为空";
                    return response;
                }
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少客户操作权限";
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

            //1判断PreOrder 是否已经存在已有的订单

            var asnCheck = _repASN.AsQueryable().Where(a => request.Select(r => r.ExternReceiptNumber + r.CustomerName.ToString()).ToList().Contains(a.ExternReceiptNumber + a.CustomerName.ToString()));
            if (asnCheck != null && asnCheck.ToList().Count > 0)
            {
                asnCheck.ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternReceiptNumber,
                        SystemOrder = b.ASNNumber,
                        Type = b.ReceiptType,
                        StatusCode = StatusCode.Warning,
                        //StatusMsg = StatusCode.warning.ToString(),
                        Msg = "订单已存在"
                    });

                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }

            var asnData = request.Adapt<List<WMSASN>>();

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<WMS_ASNEditDto, WMS_ASN>()
            //       //添加创建人为当前用户
            //       .ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //       .ForMember(a => a.ASNDetails, opt => opt.MapFrom(c => c.ASNDetails))
            //       //添加库存状态为可用
            //       .ForMember(a => a.ASNStatus, opt => opt.MapFrom(c => ASNStatusEnum.新增))
            //       .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //       .ForMember(a => a.UpdateTime, opt => opt.Ignore())
            //       .ForMember(a => a.CompleteTime, opt => opt.Ignore());
            //    cfg.CreateMap<WMS_ASNDetailEditDto, WMS_ASNDetail>()
            //   //添加创建人为当前用户
            //   .ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //   .ForMember(a => a.UpdateTime, opt => opt.Ignore());

            //});

            //var mapper = new Mapper(config);

            // 添加更多配置(设置ID)
            //var shortid = ShortIDGen.NextID(new GenerationOptions
            //{
            //    UseNumbers = false, // 不包含数字
            //    UseSpecialCharacters = true, // 包含特殊符号
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});

            //var asnData = mapper.Map<List<WMS_ASN>>(request);
            int LineNumber = 1;
            asnData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

                var ASNNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                item.ASNNumber = ASNNumber;
                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.Creator = _userManager.Account;
                item.CreationTime = DateTime.Now;
                item.ASNStatus = (int)ASNStatusEnum.新增;
                item.Details.ForEach(a =>
                {
                    a.ASNNumber = ASNNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternReceiptNumber = item.ExternReceiptNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Creator = _userManager.Account;
                    a.CreationTime = DateTime.Now;
                });
                LineNumber++;
            });


            ////开始插入订单
            await _repASN.Context.InsertNav(asnData).Include(a => a.Details).ExecuteCommandAsync();

            asnData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ASNNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单新增成功"
                });

            });
            response.Code = StatusCode.Success;
            response.Msg = "订单新增成功";
            return response;

        }

        public async Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateWMSASNInput> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //判断是否有权限操作
            //先判断是否能操作客户
            var customerCheck = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.CustomerName).ToList().Contains(a.CustomerName)).ToList();
            if (customerCheck.GroupBy(a => a.CustomerName).Count() != request.GroupBy(a => a.CustomerName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少客户操作权限";
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

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单 判断订单状态能不能修改

            var asnCheck = _repASN.AsQueryable().Where(a => request.Select(r => r.ExternReceiptNumber).ToList().Contains(a.ExternReceiptNumber));
            if (asnCheck != null && asnCheck.ToList().Count > 0)
            {
                asnCheck.ToList().ForEach(b =>
                {
                    //判断订单状态
                    if (b.ASNStatus > (int)ASNStatusEnum.新增)
                        response.Data.Add(new OrderStatusDto()
                        {
                            ExternOrder = b.ExternReceiptNumber,
                            SystemOrder = b.ASNNumber,
                            Type = b.ReceiptType,
                            StatusCode = StatusCode.Warning,
                            //StatusMsg = StatusCode.warning.ToString(),
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

            var asnData = request.Adapt<List<WMSASN>>();



            //var asnData = mapper.Map<List<WMS_ASN>>(request);
            int LineNumber = 1;
            asnData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                //var ASNNumber = ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //item.ASNNumber = ASNNumber;

                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.Updator = _userManager.Account;
                item.UpdateTime = DateTime.Now;
                item.ASNStatus = (int)ASNStatusEnum.新增;

                item.Details.ForEach(a =>
                {
                    //判断该行是新增的
                    if (a.Id == 0)
                    {
                        a.Creator= _userManager.Account;
                        a.CreationTime= DateTime.Now;
                    }
                    a.ASNNumber = item.ASNNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternReceiptNumber = item.ExternReceiptNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Updator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                });
                LineNumber++;
            });


            ////开始插入订单
            await _repASN.Context.UpdateNav(asnData).Include(a => a.Details).ExecuteCommandAsync();

            asnData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ASNNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单修改成功"
                });

            });
            response.Code = StatusCode.Success;
            response.Msg = "订单修改成功";
            return response;

        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSASNInput> request)
        //{
        //    throw new NotImplementedException();
        //}

        //Response<List<OrderStatusDto>> IASNInterface.Strategy()
        //{

        //    throw new NotImplementedException();
        //}
    }
}
