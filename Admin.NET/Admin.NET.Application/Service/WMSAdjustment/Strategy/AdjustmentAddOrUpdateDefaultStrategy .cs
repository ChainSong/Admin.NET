
using Admin.NET.Application.CommonCore.EnumCommon;
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
    public class AdjustmentAddOrUpdateDefaultStrategy : IAdjustmentInterface
    {

        //注入数据库实例
        public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        //注入Adjustment仓储

        public SqlSugarRepository<WMSAdjustment> _repAdjustment { get; set; }
        //注入AdjustmentDetail仓储
        public SqlSugarRepository<WMSAdjustmentDetail> _repAdjustmentDetail { get; set; }
        //注入仓库关系仓储

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public AdjustmentAddOrUpdateDefaultStrategy()
        {
        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSAdjustmentInput> request)

        /// <summary>
        /// 默认方法不做任何处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateWMSAdjustmentInput> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单

            var AdjustmentCheck = _repAdjustment.AsQueryable().Where(a => request.Select(r => r.ExternNumber).ToList().Contains(a.ExternNumber));
            if (AdjustmentCheck != null && AdjustmentCheck.ToList().Count > 0)
            {
                AdjustmentCheck.ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternNumber,
                        SystemOrder = b.AdjustmentNumber,
                        Type = b.AdjustmentType,
                        StatusCode = StatusCode.Warning,
                        //StatusMsg = StatusCode.warning.ToString(),
                        Msg = "订单已存在"
                    });

                });
                response.Code = StatusCode.Error;
                response.Msg = "订单异常";
                return response;
            }

            var AdjustmentData = request.Adapt<List<WMSAdjustment>>();

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<WMS_AdjustmentEditDto, WMS_Adjustment>()
            //       //添加创建人为当前用户
            //       .ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //       .ForMember(a => a.AdjustmentDetails, opt => opt.MapFrom(c => c.AdjustmentDetails))
            //       //添加库存状态为可用
            //       .ForMember(a => a.AdjustmentStatus, opt => opt.MapFrom(c => AdjustmentStatusEnum.新增))
            //       .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //       .ForMember(a => a.UpdateTime, opt => opt.Ignore())
            //       .ForMember(a => a.CompleteTime, opt => opt.Ignore());
            //    cfg.CreateMap<WMS_AdjustmentDetailEditDto, WMS_AdjustmentDetail>()
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

            //var AdjustmentData = mapper.Map<List<WMS_Adjustment>>(request);
            int LineNumber = 1;
            AdjustmentData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

                var AdjustmentNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                item.AdjustmentNumber = AdjustmentNumber;
                item.ExternNumber = item.ExternNumber;
                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.Creator = _userManager.Account;
                item.CreationTime = DateTime.Now;
                item.AdjustmentStatus = (int)AdjustmentStatusEnum.新增;
                item.Details.ForEach(a =>
                {
                    a.AdjustmentNumber = AdjustmentNumber;
                    item.ExternNumber = item.ExternNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.AdjustmentNumber = item.AdjustmentNumber;
                    a.Creator = _userManager.Account;
                    a.CreationTime = DateTime.Now;
                });
                LineNumber++;
            });

            ////开始插入订单
            await _db.InsertNav(AdjustmentData).Include(a => a.Details).ExecuteCommandAsync();


            AdjustmentData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternNumber,
                    SystemOrder = b.AdjustmentNumber,
                    Type = b.AdjustmentType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单新增成功"
                });

            });
            response.Code = StatusCode.Success;
            return response;

        }

        public async Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateWMSAdjustmentInput> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单 判断订单状态

            var AdjustmentCheck = _repAdjustment.AsQueryable().Where(a => request.Select(r => r.ExternNumber).ToList().Contains(a.ExternNumber));
            if (AdjustmentCheck != null && AdjustmentCheck.ToList().Count > 0)
            {
                AdjustmentCheck.ToList().ForEach(b =>
                {
                    if (b.AdjustmentStatus > (int)AdjustmentStatusEnum.新增)
                        response.Data.Add(new OrderStatusDto()
                        {
                            ExternOrder = b.ExternNumber,
                            SystemOrder = b.AdjustmentNumber,
                            Type = b.AdjustmentType,
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

            var AdjustmentData = request.Adapt<List<WMSAdjustment>>();



            //var AdjustmentData = mapper.Map<List<WMS_Adjustment>>(request);
            int LineNumber = 1;
            AdjustmentData.ForEach(item =>
            {
                var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                //var AdjustmentNumber = ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //item.AdjustmentNumber = AdjustmentNumber;
                item.CustomerId = CustomerId;
                item.WarehouseId = WarehouseId;
                item.Updator = _userManager.Account;
                item.UpdateTime = DateTime.Now;
                item.AdjustmentStatus = (int)AdjustmentStatusEnum.新增;
                item.Details.ForEach(a =>
                {
                    a.AdjustmentNumber = item.AdjustmentNumber;
                    a.ExternNumber = item.ExternNumber;
                    a.CustomerId = CustomerId;
                    a.CustomerName = item.CustomerName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.AdjustmentNumber = item.AdjustmentNumber;
                    a.Updator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                });
                LineNumber++;
            });


            ////开始插入订单
            await _db.UpdateNav(AdjustmentData).Include(a => a.Details).ExecuteCommandAsync();

            AdjustmentData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternNumber,
                    SystemOrder = b.AdjustmentNumber,
                    Type = b.AdjustmentType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单修改成功"
                });

            });
            response.Code = StatusCode.Success;
            return response;

        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSAdjustmentInput> request)
        //{
        //    throw new NotImplementedException();
        //}

        //Response<List<OrderStatusDto>> IAdjustmentInterface.Strategy()
        //{

        //    throw new NotImplementedException();
        //}
    }
}
