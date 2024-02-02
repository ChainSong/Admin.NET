
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;
using Furion.DistributedIDGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PreOrderDefaultStrategy(
            )
        {

        }

        //处理预出库单业务
        public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateWMSPreOrderInput> request)
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

            var asnCheck = _repPreOrder.AsQueryable().Where(a => request.Select(r => r.ExternOrderNumber + r.CustomerName).ToList().Contains(a.ExternOrderNumber + a.CustomerName));
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
            int LineNumber = 1;
            orderData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                var PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                item.PreOrderNumber = PreOrderNumber;
                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.DetailCount = item.Details.Sum(pd => pd.OrderQty);
                item.Details.ForEach(a =>
                {
                    a.PreOrderNumber = PreOrderNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Creator = _userManager.Account;
                    a.CreationTime = DateTime.Now;
                });

                item.OrderAddress.PreOrderNumber = item.PreOrderNumber;
                item.OrderAddress.ExternOrderNumber = item.ExternOrderNumber;
                item.OrderAddress.Creator = _userManager.Account;
                item.OrderAddress.CreationTime = DateTime.Now;
                item.OrderAddress.UpdateTime = DateTime.Now;
                LineNumber++;
            });

            //开始插入数据
            //await _repPreOrder.Context.InsertNav(orderData).Include(a => a.Details).ExecuteCommandAsync();
            await _repPreOrder.Context.InsertNav(orderData).Include(a => a.Details).Include(b=>b.OrderAddress).ExecuteCommandAsync();
            //_repPreOrder.Insert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            response.Msg = "添加成功";
            return response;

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
            //var asnCheck = _repPreOrder.AsQueryable().Where(a => request.Select(r => r.ExternOrderNumber).ToList().Contains(a.ExternOrderNumber));
            //if (asnCheck != null && asnCheck.ToList().Count > 0)
            //{
            //    asnCheck.ToList().ForEach(b =>
            //    {
            //        response.Data.Add(new OrderStatusDto()
            //        {
            //            ExternOrder = b.ExternOrderNumber,
            //            SystemOrder = b.PreOrderNumber,
            //            Type = b.OrderType,
            //            Msg = "订单已存在"
            //        });

            //    });
            //    response.Code = StatusCode.error;
            //    response.Msg = "订单异常";
            //    return response;
            //}




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
            int LineNumber = 1;
            orderData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                //var PreOrderNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //item.PreOrderNumber = PreOrderNumber;
                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.DetailCount = item.Details.Sum(pd => pd.OrderQty);
                item.Details.ForEach(a =>
                {
                    a.PreOrderNumber = item.PreOrderNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternOrderNumber = item.ExternOrderNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Updator = _userManager.Account;
                    //a.Creator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                });
                LineNumber++;
            });

            //开始插入数据
            await _repPreOrder.Context.UpdateNav(orderData).Include(a => a.Details).Include(a=>a.OrderAddress).ExecuteCommandAsync();
            //_repPreOrder.Insert(asnData, options => options.IncludeGraph = true);
            response.Code = StatusCode.Success;
            return response;

        }
    }
}
