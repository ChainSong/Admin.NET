// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。


using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Common.SnowflakeCommon;
using AutoMapper;

namespace Admin.NET.Application.Service;
public class ProductAddOrUpdateStrategy : IProductInterface
{

    //数据库实例
    //ISqlSugarClient _db { get; set; }
    //用户仓储
    public UserManager _userManager { get; set; }
    //asn仓储
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

    //客户用户关系仓储
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    //仓库用户关系仓储
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateProductInput> request)
    {

        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

        //开始校验数据
        List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();

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
        //var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
        //if (warehouseCheck.GroupBy(a => a.WarehouseName).Count() != request.GroupBy(a => a.WarehouseName).Count())
        //{
        //    response.Code = StatusCode.Error;
        //    response.Msg = "用户缺少仓库操作权限";
        //    return response;
        //}

        //1判断PreOrder 是否已经存在已有的订单

        var productCheck = _repProduct.AsQueryable().Where(a => request.Select(r => r.SKU + r.CustomerName.ToString()).ToList().Contains(a.SKU + a.CustomerName.ToString()));
        if (productCheck != null && productCheck.ToList().Count > 0)
        {
            productCheck.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.CustomerName,
                    SystemOrder = b.SKU,
                    Type = b.GoodsType,
                    StatusCode = StatusCode.Warning,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "SKU已存在"
                });

            });
            if (response.Data.Count > 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "SKU已存在";
                return response;
            }
        }

        //var productData = request.Adapt<List<WMSProduct>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddOrUpdateProductInput, WMSProduct>()
                //将为Null的字段设置为"" () 
                .AddTransform<string>(a => a == null ? "" : a)
                .AddTransform<int?>(a => a == null ? 0 : a)
                .AddTransform<long?>(a => a == null ? 0 : a)

               .ForMember(a => a.ProductStatus, opt => opt.MapFrom(c => (int)ProductStatusEnum.新增))
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               //.ForMember(a => a.ASNDetails, opt => opt.MapFrom(c => c.ASNDetails))
               //添加库存状态为可用
               //.ForMember(a => a.ASNStatus, opt => opt.MapFrom(c => ASNStatusEnum.新增))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
            //.ForMember(a => a.UpdateTime, opt => opt.Ignore())
            //.ForMember(a => a., opt => opt.Ignore());
            //cfg.CreateMap<WMS_ASNDetailEditDto, WMS_ASNDetail>()
            ////添加创建人为当前用户
            //.ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //.ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //.ForMember(a => a.UpdateTime, opt => opt.Ignore());

        });

        var mapper = new Mapper(config);

        // 添加更多配置(设置ID)
        //var shortid = ShortIDGen.NextID(new GenerationOptions
        //{
        //    UseNumbers = false, // 不包含数字
        //    UseSpecialCharacters = true, // 包含特殊符号
        //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
        //});

        var productData = mapper.Map<List<WMSProduct>>(request);
        var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == request.First().CustomerName).First().CustomerId;
        //int LineNumber = 1;
        productData.ForEach(item =>
        {

            //var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

            //var ASNNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //ShortIDGen.NextID(new GenerationOptions
            //{
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            //item.ASNNumber = ASNNumber;
            item.CustomerId = CustomerId;
            //item.WarehouseId = WarehouseId;
            //item.Creator = _userManager.Account;
            //item.CreationTime = DateTime.Now;
            //item.ASNStatus = (int)ASNStatusEnum.新增;

        });


        ////开始插入订单
        await _repProduct.InsertRangeAsync(productData);
        //await _repProduct.Context.InsertNav(productData).ExecuteCommandAsync();

        productData.ToList().ForEach(b =>
        {
            response.Data.Add(new OrderStatusDto()
            {
                ExternOrder = b.CustomerName,
                SystemOrder = b.SKU,
                Type = b.GoodsType,
                StatusCode = StatusCode.Success,
                //StatusMsg = StatusCode.warning.ToString(),
                Msg = "SKU新增成功"
            });

        });
        response.Code = StatusCode.Success;
        response.Msg = "SKU新增成功";
        return response;
    }
    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateProductInput> request)
    {
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

        //开始校验数据
        List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();

        //判断是否有权限操作
        //先判断是否能操作客户
        var customerCheck = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.CustomerName).ToList().Contains(a.CustomerName)).ToList();
        if (customerCheck.GroupBy(a => a.CustomerName).Count() != request.GroupBy(a => a.CustomerName).Count())
        {
            response.Code = StatusCode.Error;
            response.Msg = "缺少客户操作权限";
            return response;
        }

        //先判断是否能操作仓库
        //var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
        //if (warehouseCheck.GroupBy(a => a.WarehouseName).Count() != request.GroupBy(a => a.WarehouseName).Count())
        //{
        //    response.Code = StatusCode.Error;
        //    response.Msg = "用户缺少仓库操作权限";
        //    return response;
        //}

        //1判断PreOrder 是否已经存在已有的订单

        //var asnCheck = _repProduct.AsQueryable().Where(a => request.Select(r => r.SKU + r.CustomerName.ToString()).ToList().Contains(a.SKU + a.CustomerName.ToString()));
        //if (asnCheck != null && asnCheck.ToList().Count > 0)
        //{
        //    asnCheck.ToList().ForEach(b =>
        //    {
        //        response.Data.Add(new OrderStatusDto()
        //        {
        //            ExternOrder = b.CustomerName,
        //            SystemOrder = b.SKU,
        //            Type = b.GoodsType,
        //            StatusCode = StatusCode.Warning,
        //            //StatusMsg = StatusCode.warning.ToString(),
        //            Msg = "SKU已存在"
        //        });

        //    });
        //    if (response.Data.Count > 0)
        //    {
        //        response.Code = StatusCode.Error;
        //        response.Msg = "SKU已存在";
        //        return response;
        //    }
        //}

        //var productData = request.Adapt<List<WMSProduct>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddOrUpdateProductInput, WMSProduct>()
                //将为Null的字段设置为"" () 
                .AddTransform<string>(a => a == null ? "" : a)
                .AddTransform<int?>(a => a == null ? 0 : a)
                .AddTransform<long?>(a => a == null ? 0 : a)
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               //.ForMember(a => a.ASNDetails, opt => opt.MapFrom(c => c.ASNDetails))
               //添加库存状态为可用
               //.ForMember(a => a.ASNStatus, opt => opt.MapFrom(c => ASNStatusEnum.新增))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
            //.ForMember(a => a.UpdateTime, opt => opt.Ignore())
            //.ForMember(a => a., opt => opt.Ignore());
            //cfg.CreateMap<WMS_ASNDetailEditDto, WMS_ASNDetail>()
            ////添加创建人为当前用户
            //.ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
            //.ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
            //.ForMember(a => a.UpdateTime, opt => opt.Ignore());

        });

        var mapper = new Mapper(config);

        // 添加更多配置(设置ID)
        //var shortid = ShortIDGen.NextID(new GenerationOptions
        //{
        //    UseNumbers = false, // 不包含数字
        //    UseSpecialCharacters = true, // 包含特殊符号
        //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
        //});

        var productData = mapper.Map<List<WMSProduct>>(request);
        var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == request.First().CustomerName).First().CustomerId;
        //int LineNumber = 1;
        productData.ForEach(item =>
        {

            //var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

            //var ASNNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //ShortIDGen.NextID(new GenerationOptions
            //{
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            //item.ASNNumber = ASNNumber;
            item.CustomerId = CustomerId;
            //item.WarehouseId = WarehouseId;
            //item.Creator = _userManager.Account;
            //item.CreationTime = DateTime.Now;
            //item.ASNStatus = (int)ASNStatusEnum.新增;

        });


        ////开始插入订单
        await _repProduct.InsertOrUpdateAsync(productData);
        //await _repProduct.Context.InsertNav(productData).ExecuteCommandAsync();

        productData.ToList().ForEach(b =>
        {
            response.Data.Add(new OrderStatusDto()
            {
                ExternOrder = b.CustomerName,
                SystemOrder = b.SKU,
                Type = b.GoodsType,
                StatusCode = StatusCode.Success,
                //StatusMsg = StatusCode.warning.ToString(),
                Msg = "SKU修改成功"
            });

        });
        response.Code = StatusCode.Success;
        response.Msg = "SKU修改成功";
        return response;
    }
}
