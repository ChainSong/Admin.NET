// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。


using Admin.NET.Common;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core.Do;
using Admin.NET.Core.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System.Data;
using System.Security.AccessControl;
//using XAct.Messages;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;

namespace Admin.NET.Core;

/// <summary>
/// 清理在线用户作业任务
/// </summary>
[JobDetail("CreateASNJob", Description = "创建ASN", GroupName = "default", Concurrent = false)]
[Daily(TriggerId = "CreateASNJob", Description = "创建ASN")]
//[Daily(TriggerId = "AutomatedAllocationJobJob", Description = "自动分配")]
//[PeriodSeconds(2, TriggerId = "AutomatedAllocationJobJob", Description = "清理在自动分配线用户", MaxNumberOfRuns = 1, RunOnStart = true)]

//[PeriodSeconds(1, TriggerId = "trigger_onlineUser", Description = "清理在线用户", MaxNumberOfRuns = 1, RunOnStart = true)]
public class CreateASNJob : IJob
{
    private readonly IServiceProvider _serviceProvider;




    public CreateASNJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        return;

        using var serviceScope = _serviceProvider.CreateScope();

        // 获取指令仓储 
        var repWarehouse = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSWarehouse>>();
        var repInstruction = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSInstruction>>();
        var repSysUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysUser>>();
        var repSysNotice = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNotice>>();
        var repSysNoticeUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNoticeUser>>();
        var repSysTenant = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysTenant>>();
        var noticeServic = serviceScope.ServiceProvider.GetService<SysNoticeService>();
        var repOrder = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSOrder>>();
        var repOMSASNHeader = serviceScope.ServiceProvider.GetService<SqlSugarRepository<OMSASNHeader>>();
        var repOMSASNDetail = serviceScope.ServiceProvider.GetService<SqlSugarRepository<OMSASNDetail>>();
        var repASN = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSASN>>();
        var repCustomer = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSCustomer>>();
        var repWorkFlow = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysWorkFlow>>();
        var repCustomerUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<CustomerUserMapping>>();
        var repWarehouseUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WarehouseUserMapping>>();
        var repProduct = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSProduct>>();
        //var repASN = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSASN>>();

        //获取为对接到系统表中的前50个订单
        var orderdatas = repOMSASNHeader.AsQueryable().Includes(a => a.Details).Where(a => a.IsUsed == 0).Take(50).ToList();
        //获取订单中的客户信息
        var customers = repCustomer.AsQueryable().Where(a => orderdatas.Select(b => b.customerName).ToList().Contains(a.Description)).ToList();


        List<WMSASN> asns = new List<WMSASN>();
        //var config = new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<OMSASNHeader, WMSASN>()
        //     .AddTransform<string>(a => a == null ? "" : a)
        //     //自定义投影，将ASN ID 投影到入库表中
        //     .ForMember(a => a.CustomerId, opt => opt.MapFrom(c => c.orderNumber))
        //     .ForMember(a => a.ExternReceiptNumber, opt => opt.MapFrom(c => c.orderNumber))
        //     //添加创建人为当前用户
        //     .ForMember(a => a.Creator, opt => opt.MapFrom(c =>"API"))

        //     //忽略需要转换的字段
        //     .ForMember(a => a.Id, opt => opt.Ignore())
        //     .ForMember(a => a.Updator, opt => opt.Ignore())
        //     .ForMember(a => a.UpdateTime, opt => opt.Ignore());

        //    cfg.CreateMap<WMSASNDetail, WMSReceiptDetail>()
        //     .AddTransform<string>(a => a == null ? "" : a)
        //     //自定义投影，将ASN明细 ID 投影到入库明细表中
        //     .ForMember(a => a.ASNDetailId, opt => opt.MapFrom(c => c.Id))

        //     //忽略需要转换的字段
        //     .ForMember(a => a.Id, opt => opt.Ignore())
        //     .ForMember(a => a.Updator, opt => opt.Ignore())
        //    .ForMember(a => a.UpdateTime, opt => opt.Ignore())
        //     ;
        //});
        //var mapper = new Mapper(config);


        foreach (var orderdata in orderdatas)
        {
            //客户信息
            var customer = customers.Where(a => a.Description == orderdata.customerName).FirstOrDefault();
            if (customer == null)
            {
                continue;
            }

            //构建ASN
            WMSASN asn = new WMSASN();
            //asn.ASNNumber= orderdata.orderNumber;
            asn.ExternReceiptNumber = orderdata.orderNumber;
            asn.CustomerId = customer.Id;
            asn.CustomerName = customer.CustomerName;
            asn.WarehouseId = 8;
            asn.WarehouseName = "上海松江仓";
            asn.ExpectDate = Convert.ToDateTime(orderdata.scheduleShippingDate);
            asn.ASNStatus = 1;
            asn.ReceiptType = "采购入库";
            asn.Creator = "API";
            asn.CreationTime = DateTime.Now;
            foreach (var detail in orderdata.Details)
            {
                WMSASNDetail asnDetail = new WMSASNDetail();
                //asnDetail.ASNNumber = orderdata.orderNumber;
                asnDetail.ExternReceiptNumber = asn.ExternReceiptNumber;
                asnDetail.CustomerId = asn.CustomerId;
                asnDetail.CustomerName = asn.CustomerName;
                asnDetail.WarehouseId = asn.WarehouseId;
                asnDetail.WarehouseName = asn.WarehouseName;
                asnDetail.SKU = detail.itemNumber;
                asnDetail.GoodsName = detail.itemDescription;
                asnDetail.ExpectedQty = Convert.ToInt32(detail.quantity);
                asnDetail.UnitCode = detail.uom;
                //asnDetail.ProductName = detail.ProductName;
                //asnDetail.ProductSpec = detail.ProductSpec;
                //asnDetail.ProductColor = detail.ProductColor;
                //asnDetail.ProductSize = detail.ProductSize;
                //asnDetail.ProductUnit = detail.ProductUnit;
                //asnDetail.ProductCount = detail.ProductCount;
                //asnDetail.ProductPrice = detail.ProductPrice;
                //asnDetail.ProductAmount = detail.ProductAmount;
                //asnDetail.ProductRemark = detail.ProductRemark;
                asnDetail.Creator = "API";
                asnDetail.CreationTime = DateTime.Now;
                asn.Details.Add(asnDetail);
            }
            asns.Add(asn);
        }


        //1判断ASN 是否已经存在已有的订单

        var asnCheck = repASN.AsQueryable().Where(a => asns.Select(r => r.ExternReceiptNumber).ToList().Contains(a.ExternReceiptNumber) && a.CustomerName == asns.First().CustomerName);
        if (asnCheck != null && asnCheck.ToList().Count > 0)
        {
            asnCheck.ToList().ForEach(b =>
            {
                asns.Remove(b);

            });
        }

        var asnData = asns.Adapt<List<WMSASN>>();

        asnData.ForEach(item =>
        {
            int LineNumber = 1;
            var customerId = repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
            var warehouseId = repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

            var ASNNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //ShortIDGen.NextID(new GenerationOptions
            //{
            //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            item.ASNNumber = ASNNumber;
            item.CustomerId = customerId;
            item.WarehouseId = warehouseId;
            item.Creator = "API";
            item.CreationTime = DateTime.Now;
            item.ASNStatus = 1;
            item.Details.ForEach(a =>
            {
                //获取产品信息
                //var productInfo = repProduct.AsQueryable()
                //   .Where(b => a.SKU == b.SKU && b.CustomerId == customerId)
                //   .First();
                ////校验产品信息
                //if (productInfo == null)
                //{
                //    response.Data.Add(new OrderStatusDto()
                //    {
                //        ExternOrder = item.ExternReceiptNumber,
                //        SystemOrder = item.ASNNumber,
                //        Type = item.ReceiptType,
                //        StatusCode = StatusCode.Error,
                //        //StatusMsg = StatusCode.warning.ToString(),
                //        Msg = "产品信息不存在"
                //    });
                //    return;
                //}
                a.ASNNumber = ASNNumber;
                a.CustomerId = customerId;
                a.CustomerName = item.CustomerName;
                a.WarehouseId = warehouseId;
                a.WarehouseName = item.WarehouseName;
                a.ExternReceiptNumber = item.ExternReceiptNumber;
                a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                a.Creator = "API";
                a.CreationTime = DateTime.Now;
                //a.GoodsName = item.GoodsName;
                LineNumber++;
            });

        });


        ////开始插入订单
        await repASN.Context.InsertNav(asnData).Include(a => a.Details).ExecuteCommandAsync();

    }



}