
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Common;
using Admin.NET.Common.AMap;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using NewLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Admin.NET.Application.Strategy
{
    public class PreOrderDefaultStrategy : IPreOrderInterface
    {
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }

        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        //注入产品仓储
        public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

        public PreOrderDefaultStrategy()
        {

        }

        AMap aMap = new AMap();
        //处理预出库单业务
        public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateWMSPreOrderInput> request)
        {
            //try
            //{
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

            //临时方法，在开会改bug (对hach 导入的电话号码做验证)
            // 正则表达式用于匹配电话号码或手机号码
            // 这里的正则表达式是一个示例，可能需要根据实际情况进行调整
            //string phoneStr = @"^[1]+[2,3,4,5,6,7,8,9]+\d{9}";
            //string telStr = @"^(\d{3,4}-)?\d{6,8}$";

            // 检查电话号码是否符合正则表达式
            //return ;

            foreach (var item in request)
            {

                #region 请求高德地图 地理编码API  返回省市区

                if (item.OrderAddress != null)
                {
                    try
                    {
                        //调用高德地图api
                        var Georesponse = await aMap.RequestGeoCode(item.OrderAddress.Province + item.OrderAddress.City + item.OrderAddress.County + item.OrderAddress.Address, item.OrderAddress.City);
                        if (Georesponse != null && Georesponse.status == "1")
                        {
                            item.OrderAddress.Province = Georesponse.geocodes[0].province;
                            item.OrderAddress.City = Georesponse.geocodes[0]?.city?.ToString() ?? item.OrderAddress.City;
                            //item.OrderAddress.County = Georesponse.geocodes[0].district;
                        }
                        else
                        {
                            //response.Data.Add(new OrderStatusDto()
                            //{
                            //    ExternOrder = item.ExternOrderNumber,
                            //    SystemOrder = item.PreOrderNumber,
                            //    Type = item.OrderType,
                            //    Msg = "获取省区市失败"
                            //});
                            //return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "获取省区市失败" };
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }

                #endregion
                //if (string.IsNullOrEmpty(item.OrderType))
                //{
                //    response.Data.Add(new OrderStatusDto()
                //    {
                //        ExternOrder = item.ExternOrderNumber,
                //        SystemOrder = item.PreOrderNumber,
                //        Type = item.OrderType,
                //        Msg = "缺少订单类型"
                //    });
                //}
                if (!string.IsNullOrEmpty(item.OrderAddress.Phone))
                {
                    item.OrderAddress.Phone = item.OrderAddress.Phone.Trim();
                    //if (Regex.IsMatch(item.OrderAddress.Phone, phoneStr) || Regex.IsMatch(item.OrderAddress.Phone, telStr))
                    //{
                    //    continue;
                    //}
                    if (PhoneNumberValidator.ValidatePhoneNumber(item.OrderAddress.Phone))
                    {
                        continue;
                    }
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = item.ExternOrderNumber,
                        SystemOrder = item.PreOrderNumber,
                        Type = item.OrderType,
                        Msg = "电话号码格式不正确"
                    });
                    //response.Code = StatusCode.Error;
                    //response.Msg = "电话号码格式不正确";
                    //return response;
                }


            }


            if (response.Data.Count > 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "订单异常";
                return response;
            }

            if (request.Select(a => a.CustomerName).Distinct().Count() > 1)
            {
                response.Code = StatusCode.Error;
                response.Msg = "一次只能导入一家客户";
                return response;
            }
            var asnCheck = _repPreOrder.AsQueryable().Where(a => request.Select(r => r.ExternOrderNumber).ToList().Contains(a.ExternOrderNumber) && a.CustomerName == request.First().CustomerName);
            if (asnCheck != null && asnCheck.ToList().Count > 0)
            {
                asnCheck.ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternOrderNumber,
                        SystemOrder = b.PreOrderNumber,
                        Type = b.OrderType,
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




            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddOrUpdateWMSPreOrderInput, WMSPreOrder>()
                   //添加创建人为当前用户
                   .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.Details, opt => opt.MapFrom(c => c.Details))
                   .ForMember(a => a.OrderAddress, opt => opt.MapFrom(c => c.OrderAddress))
                   .ForMember(a => a.Extends, opt => opt.MapFrom(c => c.Extends))
                   //添加库存状态为可用
                   .ForMember(a => a.PreOrderStatus, opt => opt.MapFrom(c => PreOrderStatusEnum.新增))

                   .ForMember(a => a.Updator, opt => opt.Ignore())
                   .ForMember(a => a.UpdateTime, opt => opt.Ignore())
                   .AddTransform<string>(a => a == null ? "" : a);


                // cfg.CreateMap<AddWMSPreOrderInput, WMSPreOrderDetail>()
                ////添加创建人为当前用户
                //.ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
                //.ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                //.ForMember(a => a.UpdateTime, opt => opt.Ignore());

            });
            //var idGen = new SequentialGuidIDGenerator();
            //var guid = idGen.Create();
            //// 更多参数
            //var idGen2 = new SequentialGuidIDGenerator();
            //var guid2 = idGen2.Create(new SequentialGuidSettings { LittleEndianBinary16Format = true });

            //var ReceiptNumber = ShortIDGen.NextID(new GenerationOptions
            //{
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            var mapper = new Mapper(config);

            var orderData = mapper.Map<List<WMSPreOrder>>(request);

            orderData.ForEach(async (item) =>
            {
                int lineNumber = 1;
                var customerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var warehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                var preOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                item.PreOrderNumber = preOrderNumber;
                item.CustomerId = customerId;
                item.WarehouseId = warehouseId;
                item.DetailCount = item.Details.Sum(pd => pd.OrderQty);
                item.Details.ForEach(a =>
                {
                    //获取产品信息
                    var productInfo = _repProduct.AsQueryable()
                       .Where(b => b.SKU == a.SKU && b.CustomerId == customerId)
                       .First();
                    //校验产品信息
                    if (productInfo == null)
                    {
                        response.Data.Add(new OrderStatusDto()
                        {
                            ExternOrder = item.ExternOrderNumber,
                            SystemOrder = item.PreOrderNumber,
                            Type = item.OrderType,
                            StatusCode = StatusCode.Error,
                            //StatusMsg = StatusCode.warning.ToString(),
                            Msg = a.SKU + "产品信息不存在"
                        });
                        return;
                    }

                    a.PreOrderNumber = preOrderNumber;
                    a.CustomerId = customerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = warehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.GoodsName = productInfo.GoodsName;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                    a.LineNumber = lineNumber.ToString().PadLeft(5, '0');
                    a.Creator = _userManager.Account;
                    a.CreationTime = DateTime.Now;
                    lineNumber++;
                });
                if (item.OrderAddress != null)
                {
                    //item.OrderAddress.ExternOrderNumber = null;
                    item.OrderAddress.PreOrderNumber = item.PreOrderNumber;
                    //item.OrderAddress.ExternOrderNumber = item.ExternOrderNumber;
                    item.OrderAddress.Creator = _userManager.Account;
                    item.OrderAddress.CreationTime = DateTime.Now;
                }
                if (item.Extends != null && item.Extends.Count>0)
                {
                    item.Extends[0].PreOrderNumber = item.PreOrderNumber;
                    item.Extends[0].ExternOrderNumber = item.ExternOrderNumber;
                    item.Extends[0].Creator = _userManager.Account;
                    item.Extends[0].CreationTime = DateTime.Now;
                }
                //item.OrderAddress.UpdateTime = DateTime.Now;
            });

            if (response.Data.Count > 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "订单异常";
                return response;
            }

            //开始插入数据
            //await _repPreOrder.Context.InsertNav(orderData).Include(a => a.Details).ExecuteCommandAsync();
            _repPreOrder.Context.InsertNav(orderData)
              .Include(a => a.Details)
              .Include(b => b.OrderAddress)
              .Include(b => b.Extends).ExecuteCommand();

            //    await _repPreOrder.InsertRangeAsync().
            //.Include(a => a.Details)
            //.Include(b => b.OrderAddress)
            //.Include(b => b.Extend).ExecuteCommandAsync();
            //_repPreOrder.Insert(asnData, options => options.IncludeGraph = true);


            orderData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternOrderNumber,
                    SystemOrder = b.PreOrderNumber,
                    Type = b.OrderType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单新增成功"
                });

            });
            response.Code = StatusCode.Success;
            response.Msg = "添加成功";
            return response;
            //}
            //catch (Exception er)
            //{

            //    throw Oops.Oh(er, "添加预出库单失败");
            //}

        }


        public async Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateWMSPreOrderInput> request)
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
            //var customerCheck = _repCustomerUser.AsQueryable().Where(a => request.Select(r => r.CustomerName).ToList().Contains(a.CustomerName)).ToList();
            //if (customerCheck.Count != request.GroupBy(a => a.CustomerName).Count())
            //{
            //    response.Code = StatusCode.Error;
            //    response.Msg = "用户缺少客户操作权限";
            //    return response;
            //}

            ////先判断是否能操作仓库
            //var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
            //if (warehouseCheck.Count != request.GroupBy(a => a.WarehouseName).Count())
            //{
            //    response.Code = StatusCode.Error;
            //    response.Msg = "用户缺少仓库操作权限";
            //    return response;
            //}
            var asnCheck = _repPreOrder.AsQueryable().Where(a => request.Select(r => r.ExternOrderNumber).ToList().Contains(a.ExternOrderNumber) &&
            a.PreOrderStatus > (int)PreOrderStatusEnum.新增);
            if (asnCheck != null && asnCheck.ToList().Count > 0)
            {
                asnCheck.ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternOrderNumber,
                        SystemOrder = b.PreOrderNumber,
                        Type = b.OrderType,
                        Msg = "订单状态异常"
                    });

                });
                response.Code = StatusCode.Error;
                response.Msg = "订单异常";
                return response;
            }




            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddOrUpdateWMSPreOrderInput, WMSPreOrder>()
                   //添加创建人为当前用户
                   .ForMember(a => a.Updator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.UpdateTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.Details, opt => opt.MapFrom(c => c.Details))
                   //添加库存状态为可用
                   .ForMember(a => a.PreOrderStatus, opt => opt.MapFrom(c => PreOrderStatusEnum.新增))

                   //.ForMember(a => a.Creator, opt => opt.Ignore())
                   .ForMember(a => a.UpdateTime, opt => opt.Ignore())

                   .AddTransform<string>(a => a == null ? "" : a);


                // cfg.CreateMap<AddWMSPreOrderInput, WMSPreOrderDetail>()
                ////添加创建人为当前用户
                //.ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
                //.ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                //.ForMember(a => a.UpdateTime, opt => opt.Ignore());

            });
            //var idGen = new SequentialGuidIDGenerator();
            //var guid = idGen.Create();
            //// 更多参数
            //var idGen2 = new SequentialGuidIDGenerator();
            //var guid2 = idGen2.Create(new SequentialGuidSettings { LittleEndianBinary16Format = true });

            //var ReceiptNumber = ShortIDGen.NextID(new GenerationOptions
            //{
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            var mapper = new Mapper(config);

            var orderData = mapper.Map<List<WMSPreOrder>>(request);

            orderData.ForEach(item =>
            {
                int lineNumber = 1;
                var customerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var warehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                //var PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //item.PreOrderNumber = PreOrderNumber;
                item.CustomerId = customerId;
                item.WarehouseId = warehouseId;
                item.DetailCount = item.Details.Sum(pd => pd.OrderQty);
                item.Details.ForEach(a =>
                {

                    //获取产品信息
                    var productInfo = _repProduct.AsQueryable()
                       .Where(b => b.SKU == a.SKU && b.CustomerId == customerId)
                       .First();
                    //校验产品信息
                    if (productInfo == null)
                    {
                        response.Data.Add(new OrderStatusDto()
                        {
                            ExternOrder = item.ExternOrderNumber,
                            SystemOrder = item.PreOrderNumber,
                            Type = item.OrderType,
                            StatusCode = StatusCode.Error,
                            //StatusMsg = StatusCode.warning.ToString(),
                            Msg = "产品信息不存在"
                        });
                        return;
                    }

                    a.PreOrderNumber = item.PreOrderNumber;
                    a.CustomerId = customerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = warehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.GoodsName = productInfo.GoodsName;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                    a.LineNumber = lineNumber.ToString().PadLeft(5, '0');
                    a.Updator = _userManager.Account;
                    //a.Creator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                    lineNumber++;
                });

                if (item.OrderAddress != null)
                {
                    item.OrderAddress.PreOrderNumber = item.PreOrderNumber;
                    item.OrderAddress.ExternOrderNumber = item.ExternOrderNumber;
                    item.OrderAddress.Updator = _userManager.Account;
                    item.OrderAddress.UpdateTime = DateTime.Now;
                }
                if (item.Extends != null)
                {
                    item.Extends[0].PreOrderNumber = item.PreOrderNumber;
                    item.Extends[0].ExternOrderNumber = item.ExternOrderNumber;
                    item.Extends[0].Updator = _userManager.Account;
                    item.Extends[0].UpdateTime = DateTime.Now;
                }

            });

            //开始插入数据
            //await _repPreOrder.Context.UpdateNav(orderData).Include(a => a.Details).Include(a => a.OrderAddress).ExecuteCommandAsync();
            await _repPreOrder.Context.UpdateNav(orderData)
              .Include(a => a.Details)
              .Include(b => b.OrderAddress)
              .Include(b => b.Extends)
              .ExecuteCommandAsync();

            orderData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternOrderNumber,
                    SystemOrder = b.PreOrderNumber,
                    Type = b.OrderType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单修改成功"
                });

            });
            //_repPreOrder.Insert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            return response;

        }
    }
}
