// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Interface;
using Admin.NET.Common.ExpressInterfaceUtils;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Core;
using Admin.NET.Express;
using Admin.NET.Express.Enumerate;
using Admin.NET.Express.Interface;
using Admin.NET.Express.Strategy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Express.Strategy.SFExpress.Dto;
using NewLife.Serialization;
using Admin.NET.Application.Service.WMSExpress.Interface;
using NPOI.SS.Formula.PTG;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using Admin.NET.Application.Service.WMSExpress.Enumerate;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaGetWxaGameFrameResponse.Types.Data.Types.Frame.Types;
using Admin.NET.Application.Service.WMSExpress.Dto;

namespace Admin.NET.Application.Service.WMSExpress.Strategy;

/// <summary>
/// 顺丰快递
/// </summary>
public class SFExpressStrategy : IExpressInterface
{

    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }
    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }
    public UserManager _userManager { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SqlSugarRepository<WMSExpressFee> _repWMSExpressFee { get; set; }
    //public SqlSugarRepository<WMSExpressDelivery> _repExpressDelivery { get; set; }


    public SqlSugarRepository<WMSExpressDelivery> _repExpressDelivery { get; set; }
    public SqlSugarRepository<WMSExpressConfig> _repExpressConfig { get; set; }

    public SysCacheService _sysCacheService { get; set; }
    /// <summary>
    /// 获取快递信息（一箱一个快递单号）
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> GetExpressData(ScanPackageInput request)
    {
        Response response = new Response();
        //request.PackageNumber = "126370824143168";

        //获取包裹信息
        var package = _repPackage.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).First();
        //如果已经有了快递单号直接打印
        if (package != null && !string.IsNullOrEmpty(package.ExpressNumber))
        {
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            return response;
        }
        var packageDetail = _repPackageDetail.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber);

        //获取快递信息
        var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == package.CustomerId && a.WarehouseId == package.WarehouseId && a.Status == 1).First();
        if (getExpressConfig == null)
        {
            response.Msg = "请配置该客户的快递公司信息";
            response.Code = StatusCode.Error;
            return response;
        }


        List<SFCargodetail> sFCargodetails = new List<SFCargodetail>();
        packageDetail.ForEach(a =>
        {
            sFCargodetails.Add(new SFCargodetail { name = a.OrderNumber + "| &" + a.PreOrderNumber });
        });

        var senderContact = "";
        var senderMobile = "";
        var senderTel = "";
        //获取寄件人信息
        var sender = _repWarehouse.AsQueryable().Includes(a => a.Details).Where(a => a.Id == package.WarehouseId).First();
        if (sender.Details != null && sender.Details.Count > 0 && sender.Details.Where(a => a.CustomerId == request.CustomerId).Count() > 0)
        {
            var warehouseDetail = sender.Details.Where(a => a.CustomerId == request.CustomerId).First();
            senderContact = warehouseDetail.Contact;
            senderMobile = warehouseDetail.Phone;
            senderTel = warehouseDetail.Tel;

        }
        else
        {
            senderContact = sender.Contractor;
            senderMobile = sender.Mobile;
            senderTel = sender.Phone;
        }
        List<SFContactinfolist> sFContactinfolists = new List<SFContactinfolist>();
        sFContactinfolists.Add(new SFContactinfolist()
        {
            address = sender.Address,
            city = sender.City,
            contact = senderContact,
            contactType = 1,
            //country = "CN",
            county = sender.County,
            mobile = senderMobile,
            tel = senderTel,
            province = sender.Province,
            company = sender.Company,
        });
        //package.PreOrderNumber = "125840648167744";
        //获取收件人信息
        var receiver = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == package.PreOrderNumber).First();
        sFContactinfolists.Add(new SFContactinfolist()
        {
            address = receiver.Address,
            city = receiver.City,
            contact = receiver.Name,
            contactType = 2,
            county = receiver.County,
            mobile = receiver.Phone,
            tel = receiver.Phone,
            province = receiver.Province,
            //company = receiver.Company,
        });

        //string monthlyCard = getExpressConfig.MonthAccount;//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）

        //var detail = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == detail.First().PreOrderNumber);


        //SFExpressServiceStrategy strategy = new SFExpressServiceStrategy();
        SFExpressInput<SFRootobject> input = new SFExpressInput<SFRootobject>() { Data = new SFRootobject() };
        //判断客户需不需要回单
        //1,先判断基础配置需不需要回单
        var IsSignBack = getExpressConfig.IsSignBack == 1 ? 1 : 0;
        //基础配置不需要回单，则判断用户订单需不需要回单
        if (IsSignBack != 1)
        {
            IsSignBack = receiver.IsSignBack == 1 ? 1 : 0;
        }
        input.Data = new SFRootobject()
        {
            orderId = package.PackageNumber,
            monthlyCard = getExpressConfig.MonthAccount,//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）
            language = "zh-CN",
            payMethod = 1, //付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付
            parcelQty = 1,//包裹数，一个包裹对应一个运单号；若包裹数大于1，则返回一个母运单号和N-1个子运单号
            totalWeight = package.GrossWeight.Value,//订单货物总重量（郑州空港海关必填）， 若为子母件必填， 单位千克， 精确到小数点后3位，如果提供此值， 必须>0 (子母件需>6)
            isOneselfPickup = 1,//快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取
            customsInfo = new SFCustomsinfo(),
            isSignBack = IsSignBack,// 是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求
            expressTypeId = 2, //https://open.sf-express.com/developSupport/734349?activeIndex=324604
            //extraInfoList = ,
            cargoDetails = sFCargodetails,
            contactInfoList = sFContactinfolists,
            remark = package.ExternOrderNumber
        };
        input.Checkword = getExpressConfig.Checkword;
        input.Url = getExpressConfig.Url;
        input.PartnerId = getExpressConfig.PartnerId;
        input.ServiceCode = "EXP_RECE_CREATE_ORDER"; //下单方法
        //input.Data.contactInfoList[1].company = "_";
        string Express = ExpressApplication.GetExpress(input);

        Rootobject output = Express.ToJsonEntity<Rootobject>();
        Apiresultdata apiresultdata = output.apiResultData.ToJsonEntity<Apiresultdata>();
        if (apiresultdata == null)
        {
            response.Code = StatusCode.Error;
            response.Msg = output.apiErrorMsg;
            return response;
        }
        if (apiresultdata.success.ToUpper() == "false".ToUpper())
        {
            response.Code = StatusCode.Error;
            response.Msg = apiresultdata.errorMsg;
            return response;
        }
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WMSPackage, WMSExpressDelivery>()
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               .ForMember(a => a.PackageId, opt => opt.MapFrom(c => c.Id))
               .ForMember(a => a.Weight, opt => opt.MapFrom(c => c.GrossWeight))
               // 
               //.ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
               //添加
               //.ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber))
               //忽略
               .ForMember(a => a.UpdateTime, opt => opt.Ignore())
               .ForMember(a => a.Updator, opt => opt.Ignore());
            //.AddTransform<string>(a => a == null ? "" : a);

            cfg.CreateMap<Routelabeldata, WMSSFExpress>()
              //添加创建人为当前用户
              .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
              .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
            //添加

        });
        var mapper = new Mapper(config);
        var packageData = mapper.Map<WMSExpressDelivery>(package);
        var sfexpress = mapper.Map<WMSSFExpress>(apiresultdata.msgData.routeLabelInfo[0].routeLabelData);
        packageData.SenderCompany = sender.Company;
        packageData.SenderProvince = sender.Province;
        packageData.SenderCity = sender.City;
        packageData.SenderContact = sender.Contractor;
        packageData.SenderTel = sender.Phone;
        packageData.SenderCountry = sender.Country;
        packageData.SenderCounty = sender.County;
        packageData.SenderAddress = sender.Address;
        packageData.SenderPostCode = "";
        packageData.RecipientsProvince = receiver.Province;
        packageData.RecipientsCity = receiver.City;
        packageData.RecipientsCountry = receiver.Country;
        packageData.RecipientsCounty = receiver.County;
        //packageData.ExpressNumber = Waybillnoinfolist[sfexpressflag].waybillNo; //sfexpress.WaybillNo;
        packageData.WaybillOrder = 1;
        packageData.WaybillType = "1"; //sfexpress.waybillType;
        packageData.SumOrder = 1; //sfexpress.waybillType;
        packageData.RecipientsCompany = receiver.ExpressCompany;
        packageData.ExpressNumber = sfexpress.WaybillNo;
        packageData.RecipientsContact = receiver.Name;
        packageData.RecipientsTel = receiver.Phone;
        packageData.RecipientsAddress = receiver.Address;
        packageData.RecipientsPostCode = "";
        packageData.PrintTime = DateTime.Now;
        packageData.PrintPersonnel = _userManager.Account;

        ///计算预计的价格
        packageData.EstimatedPrice = GetExpressFee(packageData);


        //packageData.Details = sfexpress;
        //await 【【【【【【【【【【【【【【【=】、、、【、【】‘、
        await _repExpressDelivery.InsertAsync(packageData);
        await _repPackage.UpdateAsync(a => new WMSPackage { ExpressNumber = sfexpress.WaybillNo }, a => a.PackageNumber == package.PackageNumber);
        //await _db.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
        response.Msg = "成功";
        response.Code = StatusCode.Success;
        return response;

    }



    /// <summary>
    /// 获取快递信息（可以获取子母单）
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response> GetExpressDataList(ScanPackageInput request)
    {
        Response response = new Response();
        //request.PackageNumber = "126370824143168";

        //获取包裹信息(获取订单号)
        var packageOrder = _repPackage.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).First();

        //根据订单号获取所有未获取快递单的包裹
        var package = _repPackage.AsQueryable().Where(a => a.OrderNumber == packageOrder.OrderNumber && string.IsNullOrEmpty(a.ExpressNumber)).ToList();

        //如果已经有了快递单号直接打印
        if (package != null && !string.IsNullOrEmpty(packageOrder.ExpressNumber))
        {
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            return response;
        }
        var packageDetail = _repPackageDetail.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber);

        //获取快递信息
        var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == packageOrder.CustomerId && a.WarehouseId == packageOrder.WarehouseId && a.Status == 1).First();
        if (getExpressConfig == null)
        {
            response.Msg = "请配置该客户的快递公司信息";
            response.Code = StatusCode.Error;
            return response;
        }


        List<SFCargodetail> sFCargodetails = new List<SFCargodetail>();
        //packageDetail.ForEach(a =>
        //{
        sFCargodetails.Add(new SFCargodetail { name = packageDetail.First().PickTaskNumber.Replace('(', ' ').Replace(')', ' ').Replace('/', ' ') });
        //});

        var senderContact = "";
        var senderMobile = "";
        var senderTel = "";
        //获取寄件人信息
        var sender = _repWarehouse.AsQueryable().Includes(a => a.Details).Where(a => a.Id == packageOrder.WarehouseId).First();
        if (sender.Details != null && sender.Details.Count > 0 && sender.Details.Where(a => a.CustomerId == request.CustomerId).Count() > 0)
        {
            var warehouseDetail = sender.Details.Where(a => a.CustomerId == request.CustomerId).First();
            senderContact = warehouseDetail.Contact;
            senderMobile = warehouseDetail.Phone;
            senderTel = warehouseDetail.Tel;

        }
        else
        {
            senderContact = sender.Contractor;
            senderMobile = sender.Mobile;
            senderTel = sender.Phone;
        }
        List<SFContactinfolist> sFContactinfolists = new List<SFContactinfolist>();
        sFContactinfolists.Add(new SFContactinfolist()
        {
            address = sender.Address,
            city = sender.City,
            contact = senderContact,
            contactType = 1,
            //country = "CN",
            county = sender.County,
            mobile = senderMobile,
            tel = senderTel,
            province = sender.Province,
            company = sender.Company,
        });
        //package.PreOrderNumber = "125840648167744";
        //获取收件人信息
        var receiver = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == packageOrder.PreOrderNumber).First();
        sFContactinfolists.Add(new SFContactinfolist()
        {
            address = receiver.Address?.Replace('(', ' ').Replace(')', ' ').Replace('/', ' '),
            city = receiver.City,
            contact = receiver.Name,
            contactType = 2,
            county = receiver.County,
            mobile = receiver.Phone,
            tel = receiver.Phone,
            province = receiver.Province,
            //company = receiver.Company,
        });

        //string monthlyCard = getExpressConfig.MonthAccount;//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）

        //var detail = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == detail.First().PreOrderNumber);


        //SFExpressServiceStrategy strategy = new SFExpressServiceStrategy();
        SFExpressInput<SFRootobject> input = new SFExpressInput<SFRootobject>() { Data = new SFRootobject() };
        //判断客户需不需要回单
        //1,先判断基础配置需不需要回单
        var IsSignBack = getExpressConfig.IsSignBack == 1 ? 1 : 0;
        //基础配置不需要回单，则判断用户订单需不需要回单
        if (IsSignBack != 1)
        {
            IsSignBack = receiver.IsSignBack == 1 ? 1 : 0;
        }

        input.Data = new SFRootobject()
        {
            orderId = packageOrder.OrderId.ToString(),
            monthlyCard = getExpressConfig.MonthAccount,//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）
            language = "zh-CN",
            payMethod = 1, //付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付
            parcelQty = package.Count(),//包裹数，一个包裹对应一个运单号；若包裹数大于1，则返回一个母运单号和N-1个子运单号
            totalWeight = package.Sum(a => a.GrossWeight).Value,//订单货物总重量（郑州空港海关必填）， 若为子母件必填， 单位千克， 精确到小数点后3位，如果提供此值， 必须>0 (子母件需>6)
            isOneselfPickup = 0,//快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取
            customsInfo = new SFCustomsinfo(),
            isSignBack = IsSignBack,// 是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求
            expressTypeId = 2, //快件产品类别表 https://open.sf-express.com/developSupport/734349?activeIndex=324604
            //extraInfoList = ,
            cargoDetails = sFCargodetails,
            contactInfoList = sFContactinfolists,
            remark = packageOrder.ExternOrderNumber?.Replace('(', ' ').Replace(')', ' ').Replace('/', ' ')
        };

        input.Checkword = getExpressConfig.Checkword;
        input.Url = getExpressConfig.Url;
        input.PartnerId = getExpressConfig.PartnerId;
        input.ServiceCode = "EXP_RECE_CREATE_ORDER"; //下单方法
                                                     //判断已经申请过母单号，使用申请子单号的方法
        var packageParent = _repPackage.AsQueryable().Where(a => a.OrderNumber == packageOrder.OrderNumber && !string.IsNullOrEmpty(a.ExpressNumber)).ToList();
        if (packageParent.Count > 0)
        {
            input.ServiceCode = "EXP_RECE_GET_SUB_MAILNO"; //下单方法（获取子单号）
        }
        //input.Data.contactInfoList[1].company = "_";
        string Express = ExpressApplication.GetExpress(input);

        Rootobject output = Express.ToJsonEntity<Rootobject>();
        Apiresultdata apiresultdata = output.apiResultData.ToJsonEntity<Apiresultdata>();
        if (apiresultdata == null)
        {
            response.Code = StatusCode.Error;
            response.Msg = output.apiErrorMsg;
            return response;

        }
        if (apiresultdata.success.ToUpper() == "false".ToUpper())
        {
            response.Code = StatusCode.Error;
            response.Msg = apiresultdata.errorMsg;
            return response;
        }
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WMSPackage, WMSExpressDelivery>()
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               .ForMember(a => a.PackageId, opt => opt.MapFrom(c => c.Id))
               .ForMember(a => a.Weight, opt => opt.MapFrom(c => c.GrossWeight))
               // 
               //.ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
               //添加
               //.ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber))
               //忽略
               .ForMember(a => a.UpdateTime, opt => opt.Ignore())
               .ForMember(a => a.Updator, opt => opt.Ignore());
            //.AddTransform<string>(a => a == null ? "" : a);

            cfg.CreateMap<Routelabeldata, WMSSFExpress>()
              //添加创建人为当前用户
              .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
              .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
            //添加

        });
        var mapper = new Mapper(config);
        var packageData = mapper.Map<List<WMSExpressDelivery>>(package);
        var sfexpress = new WMSSFExpress();
        if (apiresultdata.msgData.routeLabelInfo != null)
        {
            sfexpress = mapper.Map<WMSSFExpress>(apiresultdata.msgData.routeLabelInfo[0].routeLabelData);
        }
        var Waybillnoinfolist = apiresultdata.msgData.waybillNoInfoList;
        int sfexpressflag = 0;
        foreach (var item in packageData)
        {
            //if(input.ServiceCode == "EXP_RECE_GET_SUB_MAILNO" && item.)
            item.SenderCompany = sender.Company;
            item.SenderProvince = sender.Province;
            item.SenderCity = sender.City;
            item.SenderContact = sender.Contractor;
            item.SenderTel = sender.Phone;
            item.SenderCountry = sender.Country;
            item.SenderCounty = sender.County;
            item.SenderAddress = sender.Address;
            item.SenderPostCode = "";
            item.RecipientsProvince = receiver.Province;
            item.RecipientsCity = receiver.City;
            item.RecipientsCountry = receiver.Country;
            item.RecipientsCounty = receiver.County;
            item.RecipientsCompany = receiver.ExpressCompany;
            item.ExpressNumber = Waybillnoinfolist[sfexpressflag].waybillNo; //sfexpress.WaybillNo;
            item.WaybillOrder = sfexpressflag;
            item.WaybillType = Waybillnoinfolist[sfexpressflag].waybillType.ToString(); //sfexpress.waybillType;
            item.SumOrder = Waybillnoinfolist.Length; //sfexpress.waybillType;
            item.RecipientsContact = receiver.Name;
            item.RecipientsTel = receiver.Phone;
            item.RecipientsAddress = receiver.Address;
            item.RecipientsPostCode = "";
            item.PrintTime = DateTime.Now;
            item.PrintPersonnel = _userManager.Account;
            item.EstimatedPrice = GetExpressFee(item);
            sfexpressflag++;
        }


        ///计算预计的价格

        //package.ForEach(a =>
        //{
        //    a.ExpressNumber = Waybillnoinfolist[sfexpressflag].waybillNo;
        //    sfexpressflag++;
        //});

        //packageData.Details = sfexpress;
        await _repExpressDelivery.InsertRangeAsync(packageData);
        foreach (var c in packageData)
        {
            await _repPackage.AsUpdateable()
                .SetColumns(a => a.ExpressNumber == c.ExpressNumber)
                .Where(a => a.PackageNumber == c.PackageNumber)
                .ExecuteCommandAsync();
            //await _repPackage.UpdateAsync(a => new WMSPackage { ExpressNumber = c.ExpressNumber }, a => a.PackageNumber == c.PackageNumber);
        }
        //  packageData.ForEach(c =>
        //{
        //    _repPackage.Update(a => new WMSPackage { ExpressNumber = c.ExpressNumber }, a => a.PackageNumber == c.PackageNumber);
        //});
        //await _repPackage.UpdateAsync(a => new WMSPackage { ExpressNumber = sfexpress.WaybillNo }, a => a.PackageNumber == package.PackageNumber);
        //await _db.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
        response.Msg = "成功";
        response.Code = StatusCode.Success;
        return response;

    }


    /// <summary>
    /// 获取快递信息（可以获取子母单）EXP_RECE_GET_SUB_MAILNO
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    //public async Task<Response> GetExpressDataList_Sub(ScanPackageInput request)
    //{
    //    Response response = new Response();
    //    //request.PackageNumber = "126370824143168";

    //    //获取包裹信息(获取订单号)
    //    var packageOrder = _repPackage.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).First();

    //    //根据订单号获取所有未获取快递单的包裹
    //    var package = _repPackage.AsQueryable().Where(a => a.OrderNumber == packageOrder.OrderNumber && string.IsNullOrEmpty(a.ExpressNumber)).ToList();

    //    //如果已经有了快递单号直接打印
    //    if (package != null && !string.IsNullOrEmpty(packageOrder.ExpressNumber))
    //    {
    //        response.Msg = "成功";
    //        response.Code = StatusCode.Success;
    //        return response;
    //    }
    //    var packageDetail = _repPackageDetail.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber);

    //    //获取快递信息
    //    var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == packageOrder.CustomerId && a.WarehouseId == packageOrder.WarehouseId && a.Status == 1).First();
    //    if (getExpressConfig == null)
    //    {
    //        response.Msg = "请配置该客户的快递公司信息";
    //        response.Code = StatusCode.Error;
    //        return response;
    //    }




    //    List<SFCargodetail> sFCargodetails = new List<SFCargodetail>();
    //    //packageDetail.ForEach(a =>
    //    //{
    //    sFCargodetails.Add(new SFCargodetail { name = packageDetail.First().PickTaskNumber.Replace('(', ' ').Replace(')', ' ').Replace('/', ' ') });
    //    //});

    //    var senderContact = "";
    //    var senderMobile = "";
    //    var senderTel = "";
    //    //获取寄件人信息
    //    var sender = _repWarehouse.AsQueryable().Includes(a => a.Details).Where(a => a.Id == packageOrder.WarehouseId).First();
    //    if (sender.Details != null && sender.Details.Count > 0 && sender.Details.Where(a => a.CustomerId == request.CustomerId).Count() > 0)
    //    {
    //        var warehouseDetail = sender.Details.Where(a => a.CustomerId == request.CustomerId).First();
    //        senderContact = warehouseDetail.Contact;
    //        senderMobile = warehouseDetail.Phone;
    //        senderTel = warehouseDetail.Tel;

    //    }
    //    else
    //    {
    //        senderContact = sender.Contractor;
    //        senderMobile = sender.Mobile;
    //        senderTel = sender.Phone;
    //    }
    //    List<SFContactinfolist> sFContactinfolists = new List<SFContactinfolist>();
    //    sFContactinfolists.Add(new SFContactinfolist()
    //    {
    //        address = sender.Address,
    //        city = sender.City,
    //        contact = senderContact,
    //        contactType = 1,
    //        //country = "CN",
    //        county = sender.County,
    //        mobile = senderMobile,
    //        tel = senderTel,
    //        province = sender.Province,
    //        company = sender.Company,
    //    });
    //    //package.PreOrderNumber = "125840648167744";
    //    //获取收件人信息
    //    var receiver = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == packageOrder.PreOrderNumber).First();
    //    sFContactinfolists.Add(new SFContactinfolist()
    //    {
    //        address = receiver.Address?.Replace('(', ' ').Replace(')', ' ').Replace('/', ' '),
    //        city = receiver.City,
    //        contact = receiver.Name,
    //        contactType = 2,
    //        county = receiver.County,
    //        mobile = receiver.Phone,
    //        tel = receiver.Phone,
    //        province = receiver.Province,
    //        //company = receiver.Company,
    //    });

    //    //string monthlyCard = getExpressConfig.MonthAccount;//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）

    //    //var detail = _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == detail.First().PreOrderNumber);


    //    //SFExpressServiceStrategy strategy = new SFExpressServiceStrategy();
    //    SFExpressInput<SFRootobject> input = new SFExpressInput<SFRootobject>() { Data = new SFRootobject() };
    //    //判断客户需不需要回单
    //    //1,先判断基础配置需不需要回单
    //    var IsSignBack = getExpressConfig.IsSignBack == 1 ? 1 : 0;
    //    //基础配置不需要回单，则判断用户订单需不需要回单
    //    if (IsSignBack != 1)
    //    {
    //        IsSignBack = receiver.IsSignBack == 1 ? 1 : 0;
    //    }

    //    input.Data = new SFRootobject()
    //    {
    //        orderId = packageOrder.PackageNumber,
    //        monthlyCard = getExpressConfig.MonthAccount,//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）
    //        language = "zh-CN",
    //        payMethod = 1, //付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付
    //        parcelQty = package.Count(),//包裹数，一个包裹对应一个运单号；若包裹数大于1，则返回一个母运单号和N-1个子运单号
    //        totalWeight = package.Sum(a => a.GrossWeight).Value,//订单货物总重量（郑州空港海关必填）， 若为子母件必填， 单位千克， 精确到小数点后3位，如果提供此值， 必须>0 (子母件需>6)
    //        isOneselfPickup = 0,//快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取
    //        customsInfo = new SFCustomsinfo(),
    //        isSignBack = IsSignBack,// 是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求
    //        expressTypeId = 2, //快件产品类别表 https://open.sf-express.com/developSupport/734349?activeIndex=324604
    //        //extraInfoList = ,
    //        cargoDetails = sFCargodetails,
    //        contactInfoList = sFContactinfolists,
    //        remark = packageOrder.ExternOrderNumber
    //    };

    //    input.Checkword = getExpressConfig.Checkword;
    //    input.Url = getExpressConfig.Url;
    //    input.PartnerId = getExpressConfig.PartnerId;
    //    //input.ServiceCode = "EXP_RECE_CREATE_ORDER"; //下单方法
    //    input.ServiceCode = "EXP_RECE_GET_SUB_MAILNO"; //下单方法（获取子单号）

    //    //input.Data.contactInfoList[1].company = "_";
    //    string Express = ExpressApplication.GetExpress(input);

    //    Rootobject output = Express.ToJsonEntity<Rootobject>();
    //    Apiresultdata apiresultdata = output.apiResultData.ToJsonEntity<Apiresultdata>();
    //    if (apiresultdata == null)
    //    {
    //        response.Code = StatusCode.Error;
    //        response.Msg = output.apiErrorMsg;
    //        return response;

    //    }
    //    if (apiresultdata.success.ToUpper() == "false".ToUpper())
    //    {
    //        response.Code = StatusCode.Error;
    //        response.Msg = apiresultdata.errorMsg;
    //        return response;
    //    }
    //    var config = new MapperConfiguration(cfg =>
    //    {
    //        cfg.CreateMap<WMSPackage, WMSExpressDelivery>()
    //           //添加创建人为当前用户
    //           .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
    //           .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
    //           .ForMember(a => a.PackageId, opt => opt.MapFrom(c => c.Id))
    //           .ForMember(a => a.Weight, opt => opt.MapFrom(c => c.GrossWeight))
    //           // 
    //           //.ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
    //           //添加
    //           //.ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber))
    //           //忽略
    //           .ForMember(a => a.UpdateTime, opt => opt.Ignore())
    //           .ForMember(a => a.Updator, opt => opt.Ignore());
    //        //.AddTransform<string>(a => a == null ? "" : a);

    //        cfg.CreateMap<Routelabeldata, WMSSFExpress>()
    //          //添加创建人为当前用户
    //          .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
    //          .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now));
    //        //添加

    //    });
    //    var mapper = new Mapper(config);
    //    var packageData = mapper.Map<List<WMSExpressDelivery>>(package);
    //    var sfexpress = mapper.Map<WMSSFExpress>(apiresultdata.msgData.routeLabelInfo[0].routeLabelData);
    //    var Waybillnoinfolist = apiresultdata.msgData.waybillNoInfoList;
    //    int sfexpressflag = 0;
    //    foreach (var item in packageData)
    //    {
    //        item.SenderCompany = sender.Company;
    //        item.SenderProvince = sender.Province;
    //        item.SenderCity = sender.City;
    //        item.SenderContact = sender.Contractor;
    //        item.SenderTel = sender.Phone;
    //        item.SenderCountry = sender.Country;
    //        item.SenderCounty = sender.County;
    //        item.SenderAddress = sender.Address;
    //        item.SenderPostCode = "";
    //        item.RecipientsProvince = receiver.Province;
    //        item.RecipientsCity = receiver.City;
    //        item.RecipientsCountry = receiver.Country;
    //        item.RecipientsCounty = receiver.County;
    //        item.RecipientsCompany = receiver.ExpressCompany;
    //        item.ExpressNumber = Waybillnoinfolist[sfexpressflag].waybillNo; //sfexpress.WaybillNo;
    //        item.WaybillOrder = sfexpressflag;
    //        item.WaybillType = Waybillnoinfolist[sfexpressflag].waybillType.ToString(); //sfexpress.waybillType;
    //        item.SumOrder = Waybillnoinfolist.Length; //sfexpress.waybillType;
    //        item.RecipientsContact = receiver.Name;
    //        item.RecipientsTel = receiver.Phone;
    //        item.RecipientsAddress = receiver.Address;
    //        item.RecipientsPostCode = "";
    //        item.PrintTime = DateTime.Now;
    //        item.PrintPersonnel = _userManager.Account;
    //        item.EstimatedPrice = GetExpressFee(item);
    //        sfexpressflag++;
    //    }


    //    ///计算预计的价格

    //    //package.ForEach(a =>
    //    //{
    //    //    a.ExpressNumber = Waybillnoinfolist[sfexpressflag].waybillNo;
    //    //    sfexpressflag++;
    //    //});

    //    //packageData.Details = sfexpress;
    //    await _repExpressDelivery.InsertRangeAsync(packageData);
    //    foreach (var c in packageData)
    //    {
    //        await _repPackage.AsUpdateable()
    //            .SetColumns(a => a.ExpressNumber == c.ExpressNumber)
    //            .Where(a => a.PackageNumber == c.PackageNumber)
    //            .ExecuteCommandAsync();
    //        //await _repPackage.UpdateAsync(a => new WMSPackage { ExpressNumber = c.ExpressNumber }, a => a.PackageNumber == c.PackageNumber);
    //    }
    //    //  packageData.ForEach(c =>
    //    //{
    //    //    _repPackage.Update(a => new WMSPackage { ExpressNumber = c.ExpressNumber }, a => a.PackageNumber == c.PackageNumber);
    //    //});
    //    //await _repPackage.UpdateAsync(a => new WMSPackage { ExpressNumber = sfexpress.WaybillNo }, a => a.PackageNumber == package.PackageNumber);
    //    //await _db.InsertNav(packageData).Include(a => a.Details).ExecuteCommandAsync();
    //    response.Msg = "成功";
    //    response.Code = StatusCode.Success;
    //    return response;

    //}


    private double GetExpressFee(WMSExpressDelivery packageData)
    {
        double Fee = 0;
        try
        {
            //获取首重
            double FirstWeight = packageData.Weight.Value;

            //获取费用配置
            WMSExpressFee wMSExpressFee = _repWMSExpressFee.AsQueryable().Where(a => a.ExpressCompany == packageData.ExpressCompany && a.Origin == packageData.SenderProvince && a.Destination == packageData.RecipientsProvince).First();
            if (wMSExpressFee != null)
            {
                if (FirstWeight > 1)
                {
                    Fee = wMSExpressFee.FirstWeight.Value + (FirstWeight - 1) * wMSExpressFee.AdditionalWeight.Value;
                }
                else
                {
                    Fee = wMSExpressFee.FirstWeight.Value;
                }


            }
        }
        catch (Exception)
        {

        }
        return Fee;
    }


    //private double GetExpressFeeList(List<WMSExpressDelivery> packageData, WMSPackage package)
    //{
    //    double Fee = 0;
    //    try
    //    {
    //        //获取首重
    //        double FirstWeight = package.GrossWeight.Value;

    //        //获取费用配置
    //        WMSExpressFee wMSExpressFee = _repWMSExpressFee.AsQueryable().Where(a => a.ExpressCompany == packageData.ExpressCompany && a.Origin == packageData.SenderProvince && a.Destination == packageData.RecipientsProvince).First();
    //        if (wMSExpressFee != null)
    //        {
    //            if (FirstWeight > 1)
    //            {
    //                Fee = wMSExpressFee.FirstWeight.Value + (FirstWeight - 1) * wMSExpressFee.AdditionalWeight.Value;
    //            }
    //            else
    //            {
    //                Fee = wMSExpressFee.FirstWeight.Value;
    //            }


    //        }
    //    }
    //    catch (Exception)
    //    {

    //    }
    //    return Fee;
    //}

    /// <summary>
    /// 获取快递配置信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<dynamic>> GetExpressConfig(ScanPackageInput request)
    {
        Response<dynamic> response = new Response<dynamic>();
        //request.PackageNumber = "126370824143168";
        SFExpressInput<SFRootobjectPrint> input = new SFExpressInput<SFRootobjectPrint>();

        //获取包裹信息
        var package = _repPackage.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).First();

        var expressConfig = _sysCacheService.Get<WMSExpressConfigOutput>("SFExpress_" + request.CustomerId + "_" + request.WarehouseId);
        var flag = _sysCacheService.Get<int>("SFExpress_" + request.CustomerId + "_" + request.WarehouseId + "_" + request.PackageNumber + "_flag");
        var flagCount = _sysCacheService.Set("SFExpress_" + request.CustomerId + "_" + request.WarehouseId + "_" + request.PackageNumber + "_flag", (flag + 1), new TimeSpan(1, 0, 0));

        //同一个快递单，获取打印快递超过5次，就清理token 就更新token (改为0  每次都获取)
        if (expressConfig != null && flag >= 0)
        {
            expressConfig.Token = "";
        }
        if (expressConfig != null && !string.IsNullOrEmpty(expressConfig.Token))
        {
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            response.Data = expressConfig;
            return response;

        }


        //获取快递信息
        var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == request.CustomerId && a.WarehouseId == request.WarehouseId && a.Status == 1).First();
        //var Express = _repExpressDelivery.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).FirstAsync();
        //WMSExpressConfigOutput wMSExpress = new WMSExpressConfigOutput();

        var wMSExpressConfig = getExpressConfig.Adapt<WMSExpressConfigOutput>();

        if (getExpressConfig != null)
        {
            input.Checkword = getExpressConfig.Checkword;
            input.Url = getExpressConfig.Url;
            input.UrlToken = getExpressConfig.UrlToken;
            //input.UrlToken = "https://sfapi.sf-express.com/oauth2/accessToken";
            input.Env = getExpressConfig.Env;
            input.PartnerId = getExpressConfig.PartnerId;
            //input.ServiceCode = "COM_RECE_CLOUD_PRINT_WAYBILLS"; //云打印方法COM_RECE_CLOUD_PRINT_WAYBILLS
            //input.Data = new SFRootobjectPrint();
            //input.Data.orderId = package.PackageNumber;
            //input.Data.monthlyCard = getExpressConfig.MonthAccount;//顺丰月结卡号 月结支付时传值，现结不需传值；沙箱联调可使用测试月结卡号7551234567（非正式，无须绑定，仅支持联调使用）
            //input.Data.language = "zh-CN";
            //input.Data.payMethod = 1; //付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付    


            //input.Data = new SFRootobjectPrint()
            //{
            //    templateCode = "fm_150_standard_HJSRJOEY88G9",
            //    version = "2.0",
            //    fileType = "pdf",
            //    sync = "true",
            //    documents = new List<Document>() { new Document() { masterWaybillNo = package.ExpressNumber } }
            //};

            string ResultData = ExpressApplication.TokenExpress(input);

            RootobjectPrint rootobjectPrint = ResultData.ToJsonEntity<RootobjectPrint>();
            wMSExpressConfig.Token = rootobjectPrint.accessToken;
            //RootobjectPrint_obj RootobjectResult = rootobjectPrint.apiResultData.ToJsonEntity<RootobjectPrint_obj>();
            //rootobjectPrint

            _sysCacheService.Set("SFExpress_" + request.CustomerId + "_" + request.WarehouseId, wMSExpressConfig, new TimeSpan(1, 0, 0));
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            response.Data = wMSExpressConfig;
            return response;
        }
        //var config = new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<WMSPackage, WMSExpressDelivery>()
        //       //添加创建人为当前用户
        //       .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
        //       .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
        //       .ForMember(a => a.PackageId, opt => opt.MapFrom(c => c.Id))
        //       // 
        //       //.ForMember(a => a.PackageStatus, opt => opt.MapFrom(c => PackageStatusEnum.完成))
        //       //添加
        //       //.ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber))
        //       //忽略
        //       .ForMember(a => a.UpdateTime, opt => opt.Ignore())
        //       .ForMember(a => a.Updator, opt => opt.Ignore());
        //    //.AddTransform<string>(a => a == null ? "" : a);

        //    //cfg.CreateMap<PackageData, WMSPackageDetail>()
        //    //  //添加创建人为当前用户
        //    //  .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
        //    //  .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
        //    //  //添加
        //    //  .ForMember(a => a.PackageNumber, opt => opt.MapFrom(c => packageNumber));
        //});
        //var mapper = new Mapper(config);
        //var packageData = mapper.Map<WMSExpressDelivery>(package);
        //packageData.SenderCompany = sender.Company;
        //packageData.SenderProvince = sender.Province;
        //packageData.SenderCity = sender.City;
        //packageData.SenderContact = sender.Contractor;
        //packageData.SenderTel = sender.Phone;
        //packageData.SenderCountry = sender.Country;
        //packageData.SenderAddress = sender.Address;
        //packageData.SenderPostCode = "";
        //packageData.RecipientsProvince = receiver.Province;
        //packageData.RecipientsCity = receiver.City;
        //packageData.RecipientsCountry = receiver.Country;
        //packageData.RecipientsCompany = receiver.ExpressCompany;
        //packageData.RecipientsContact = receiver.Name;
        //packageData.RecipientsTel = receiver.Phone;
        //packageData.RecipientsAddress = receiver.Address;
        //packageData.RecipientsPostCode = "";
        //packageData.PrintTime = DateTime.Now;
        //packageData.PrintPersonnel = _userManager.Account;

        //await _repExpressDelivery.InsertAsync(packageData);

        response.Msg = "快递单号不存在";
        response.Code = StatusCode.Error;
        return response;
    }

    /// <summary>
    /// 打印快递信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<dynamic>> PrintBatchExpressDataByPackageId(List<long> request)
    {
        Response<dynamic> response = new Response<dynamic>();
        //request.PackageNumber = "126370824143168";
        //获取包裹信息
        var package = await _repPackage.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToListAsync();
        var result = package.Adapt<List<PrintSFExpressDto>>();

        foreach (var item in package)
        {
            if (package != null && !string.IsNullOrEmpty(item.ExpressNumber))
            {
                //获取快递信息
                var getExpressDelivery = _repExpressDelivery.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == item.CustomerId && a.WarehouseId == item.WarehouseId && a.OrderNumber == item.OrderNumber && a.ExpressNumber == item.ExpressNumber).First();

                item.PrintNum = ((item.PrintNum ?? 0) + 1);
                item.PrintPersonnel = _userManager.Account;
                item.PrintTime = DateTime.Now;
                await _repPackage.UpdateAsync(item);
                result.Where(a => a.PackageNumber == item.PackageNumber).First().WaybillType = getExpressDelivery.WaybillType;
                result.Where(a => a.PackageNumber == item.PackageNumber).First().SumOrder = getExpressDelivery.SumOrder ?? 1;
            }
            else
            {
                response.Data = package;
                response.Msg = "有包裹未获取到快递单号";
                response.Code = StatusCode.Error;
                return response;
            }
        }
        response.Data = result;
        response.Msg = "成功";
        response.Code = StatusCode.Success;
        return response;


        ////获取快递信息
        //var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == package.CustomerId && a.WarehouseId == package.WarehouseId && a.Status == 1).First();

        //var Express = _repExpressDelivery.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).FirstAsync();

        //SFExpressInput<SFRootobjectPrint> input = new SFExpressInput<SFRootobjectPrint>();


        //if (Express.Result != null)
        //{
        //    input.Checkword = getExpressConfig.Checkword;
        //    input.Url = getExpressConfig.Url;
        //    input.PartnerId = getExpressConfig.PartnerId;
        //    input.ServiceCode = "COM_RECE_CLOUD_PRINT_WAYBILLS"; //云打印方法COM_RECE_CLOUD_PRINT_WAYBILLS
        //    input.Data = new SFRootobjectPrint()
        //    {
        //        templateCode = "fm_150_standard_HJSRJOEY88G9",
        //        version = "2.0",
        //        fileType = "pdf",
        //        sync = "true",
        //        documents = new List<Document>() { new Document() { masterWaybillNo = package.ExpressNumber } }
        //    };
        //    var ResultData = ExpressApplication.PrintExpress(input);

        //    RootobjectPrint rootobjectPrint = ResultData.ToJsonEntity<RootobjectPrint>();
        //    RootobjectPrint_obj RootobjectResult = rootobjectPrint.apiResultData.ToJsonEntity<RootobjectPrint_obj>();
        //    //rootobjectPrint
        //    response.Msg = "成功";
        //    response.Code = StatusCode.Success;
        //    response.Data = RootobjectResult.obj.files[0].token;
        //    return response;
        //}

        //response.Msg = "快递单号不存在";
        //response.Code = StatusCode.Error;
        //return response;

    }


    /// <summary>
    /// 打印快递信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<dynamic>> PrintExpressData(ScanPackageInput request)
    {
        Response<dynamic> response = new Response<dynamic>();
        //request.PackageNumber = "126370824143168";
        //获取包裹信息
        var package = _repPackage.AsQueryable().Includes(a => a.Details).Where(a => a.PackageNumber == request.PackageNumber).First();
        var result = package.Adapt<PrintSFExpressDto>();
        if (package != null && !string.IsNullOrEmpty(package.ExpressNumber))
        {
            //获取快递信息
            var getExpressDelivery = _repExpressDelivery.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == package.CustomerId && a.WarehouseId == package.WarehouseId && a.OrderNumber == package.OrderNumber && a.ExpressNumber == package.ExpressNumber).First();

            package.PrintNum = ((package.PrintNum ?? 0) + 1);
            package.PrintPersonnel = _userManager.Account;
            package.PrintTime = DateTime.Now;
            await _repPackage.UpdateAsync(package);
            result.WaybillType = getExpressDelivery.WaybillType;
            result.WaybillOrder = getExpressDelivery.WaybillOrder ?? 1;
            result.SumOrder = getExpressDelivery.SumOrder ?? 1;

            response.Data = result;
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            return response;
        }
        else
        {
            response.Data = package;
            response.Msg = "失败";
            response.Code = StatusCode.Error;
            return response;
        }

        //获取快递信息
        var getExpressConfig = _repExpressConfig.AsQueryable().Where(a => a.ExpressCompany == "顺丰快递" && a.CustomerId == package.CustomerId && a.WarehouseId == package.WarehouseId && a.Status == 1).First();

        var Express = _repExpressDelivery.AsQueryable().Where(a => a.PackageNumber == request.PackageNumber).FirstAsync();

        SFExpressInput<SFRootobjectPrint> input = new SFExpressInput<SFRootobjectPrint>();


        if (Express.Result != null)
        {
            input.Checkword = getExpressConfig.Checkword;
            input.Url = getExpressConfig.Url;
            input.PartnerId = getExpressConfig.PartnerId;
            input.ServiceCode = "COM_RECE_CLOUD_PRINT_WAYBILLS"; //云打印方法COM_RECE_CLOUD_PRINT_WAYBILLS
            input.Data = new SFRootobjectPrint()
            {
                templateCode = "fm_150_standard_HJSRJOEY88G9",
                version = "2.0",
                fileType = "pdf",
                sync = "true",
                documents = new List<Document>() { new Document() { masterWaybillNo = package.ExpressNumber } }
            };
            var ResultData = ExpressApplication.PrintExpress(input);

            RootobjectPrint rootobjectPrint = ResultData.ToJsonEntity<RootobjectPrint>();
            RootobjectPrint_obj RootobjectResult = rootobjectPrint.apiResultData.ToJsonEntity<RootobjectPrint_obj>();
            //rootobjectPrint
            response.Msg = "成功";
            response.Code = StatusCode.Success;
            response.Data = RootobjectResult.obj.files[0].token;
            return response;
        }

        response.Msg = "快递单号不存在";
        response.Code = StatusCode.Error;
        return response;

    }

}





