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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service.WMSRFReceiptReceiving.Dto;
using System.Text.RegularExpressions;
using Admin.NET.Core.Service;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;
using AutoMapper;
using Admin.NET.Application.Dtos.Enum;
using Aliyun.OSS;
using Nest;
using NPOI.SS.Formula.Functions;
using SharpCompress.Common;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECMerchantAddFreightTemplateRequest.Types.FreightTemplate.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateContainerServiceRequest.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Reflection.Emit;
using FastExpressionCompiler;

namespace Admin.NET.Application;
public class RFReceiptReceivingDefaultStrategy : IRFReceiptReceivingInterface
{
    public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
    public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
    public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }
    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
    public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

    public async Task<Response<RFReceiptReceivingOutput>> RFReceiptReceivingSave(RFReceiptReceivingInput request, WMSReceipt receipt)
    {
        //判断扫描的是不是条形码（有两种条形码）
        if (!string.IsNullOrEmpty(request.ScanInput))
        {
            //var skuInfo = request.Input.Split('|');
            if (request.ScanInput.Split(' ').Length > 1 || request.ScanInput.Split('|').Length > 1)
            {

                string SKURegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)"; // 正则表达式匹配英文字符或数字
                string LOTRegex = @"(?<=\|LOT)[^|\]+|(?<==\d{3}|=\d{4}|=\d({4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
                string ExpirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";
                MatchCollection matchesSKU = Regex.Matches(request.ScanInput, SKURegex);
                request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";

                MatchCollection matchesExpirationDateRegex = Regex.Matches(request.ScanInput, ExpirationDateRegex);
                request.ExpirationDate = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";
                MatchCollection matchesLOT = Regex.Matches(request.ScanInput, LOTRegex);
                request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
                request.SKU = request.SKU;
                request.CustomerId = receipt.CustomerId;

            };
        }
        //获取加载需要上架的订单
        var OrderData = _sysCacheService.Get<List<WMSReceiptDetail>>("RFReceiptReceivingOrder:" + receipt.CustomerId + ":" + request.ReceiptNumber);
        if (OrderData == null || OrderData.Count == 0)
        {
            OrderData = await _repReceiptDetail.GetListAsync(it => it.ReceiptNumber == receipt.ReceiptNumber && it.CustomerId == receipt.CustomerId);
        }
        //response.Data.PickTaskNumber = request.PickTaskNumber;
        //response.Data.Weight = request.Weight;
        //response.Data.SKU = request.SKU;
        //response.Data.Input = request.Input;
        //response.Data.SN = request.SN;
        //response.Data.Lot = request.Lot;
        //response.Data.AcquisitionData = request.AcquisitionData;
        //判断扫描的是产品条码还是库位
        //if (!string.IsNullOrEmpty(request.SKU))
        //{
        //  return  await ScanSKU(request, OrderData);

        //}
        //else {
        //    return await ScanLocation(request, OrderData);
        //}

        return new Response<RFReceiptReceivingOutput>();
    }

    //扫描SKU 注意区分有唯一编码和没有唯一编码
    private async Task<Response> ScanSKU(RFReceiptReceivingInput request, List<WMSReceiptDetail> OrderData)
    {
        Response response = new Response();
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WMSReceiptDetail, WMSReceiptReceiving>()
               //添加创建人为当前用户
               .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))

               .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
               .ForMember(a => a.Id, opt => opt.Ignore())
               //将为Null的字段设置为"" () 
               .AddTransform<string>(a => a == null ? "" : a)
               //将为Null的字段设置为"" () 
               .AddTransform<int?>(a => a == null ? 0 : a)
               //将为Null的字段设置为"" () 
               .AddTransform<double?>(a => a == null ? 0 : a);
        });


        var mapper = new Mapper(config);

        //第一步判断上架数量是否足够
        var orderCheckData = _sysCacheService.Get<List<WMSReceiptReceiving>>("RFReceiptReceivingScan:" + request.CustomerId + ":" + request.ReceiptNumber);

        //缓存没有数据，说明还没上架
        if (orderCheckData == null || orderCheckData.Count == 0)
        {
            orderCheckData = new List<WMSReceiptReceiving>();
            //OrderCheckData =await  _repReceiptDetail.GetListAsync(it => it.ReceiptNumber == request.ReceiptNumber && it.CustomerId == request.CustomerId);
        }
        //判断需不需要上架，数量是否足够
        if (OrderData.Where(it => it.SKU == request.SKU && it.ReceivedQty > it.ReceiptQty).Count() >= 0)
        {
            //可继续上架

            var receiptReceiving = OrderData.Where(it => it.SKU == request.SKU && it.ReceivedQty > it.ReceiptQty).First();
            receiptReceiving.ReceivedQty = receiptReceiving.ReceivedQty + 1;
            var packageData = mapper.Map<WMSReceiptReceiving>(receiptReceiving);
            packageData.ReceivedQty = 1;
            orderCheckData.Add(packageData);
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "上架数量足够";
            return response;
        }
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
    }


    //扫描SKU 注意区分有唯一编码和没有唯一编码
    private async Task<Response> ScanLocation(RFReceiptReceivingInput request, List<WMSReceiptDetail> orderData)
    {
        Response response = new Response();
        //判断是不是可用的库存
        var checkLocation = await _repLocation.GetListAsync(it => it.Location == request.ScanInput);
        if (checkLocation != null && checkLocation.Count > 0)
        {
            var orderCheckData = _sysCacheService.Get<List<WMSReceiptReceiving>>("RFReceiptReceivingScan:" + request.CustomerId + ":" + request.ReceiptNumber);
            if (orderCheckData != null && orderCheckData.Count > 0)
            {
                //判断需不需要上架，数量是否足够
                //将自己扫描的数据存入缓存，等待上架，补充库位信息
                orderCheckData.Where(it => string.IsNullOrEmpty(it.Location) && it.Creator == _userManager.Account).ToList().ForEach(it => it.Location = request.ScanInput);
                return await AddReceiptReceiving(orderCheckData, orderData);
            }
            else
            {
                response.Code = StatusCode.Error;
                response.Msg = "请扫描产品条码";
                return response;
            }
        }
        else
        {
            //库位不存在，提示错误信息
            response.Code = StatusCode.Error;
            response.Msg = "库位不存在";
            return response;
        }


    }

    private async Task<Response> AddReceiptReceiving(List<WMSReceiptReceiving> request, List<WMSReceiptDetail> OrderData)
    {
        Response response = new Response();
        //判断是不是可用的库存
        //var checkLocation = await _repLocation.GetListAsync(it => it.Location == request.ScanInput);
        //if (checkLocation != null && checkLocation.Count > 0)
        //{
        //var orderCheckData = _sysCacheService.Get<List<WMSReceiptReceiving>>("RFReceiptReceivingScan:" + request.CustomerId + ":" + request.ReceiptNumber);
        //判断数量是否足够上架
        if (request != null && OrderData.Count > 0 && OrderData.Where(it => it.ReceivedQty != it.ReceiptQty).Count() == 0)
        {
            //判断需不需要上架，数量是否足够
            //将自己扫描的数据存入缓存，等待上架，补充库位信息
            //构建正式上架数据（同类型数据合并）
            var receiptReceiving = request.GroupBy(r => new
            {

                r.ASNDetailId
                ,
                r.ReceiptId
                ,
                r.ReceiptDetailId
                ,
                r.ReceiptNumber
                ,
                r.ExternReceiptNumber
                ,
                r.ASNNumber
                ,
                r.CustomerId
                ,
                r.CustomerName
                ,
                r.WarehouseId
                ,
                r.WarehouseName
                ,
                r.ReceiptReceivingStatus
                ,
                r.GoodsStatus
                ,
                r.LineNumber
                ,
                r.SKU
                ,
                r.UPC
                ,
                r.GoodsType
                ,
                r.GoodsName
                ,
                r.BoxCode
                ,
                r.TrayCode
                ,
                r.BatchCode
                ,
                r.LotCode
                ,
                r.PoCode
                ,
                r.Weight
                ,
                r.Volume
                ,
                r.UnitCode
                ,
                r.Onwer
                ,
                r.Area
                ,
                r.Location
                ,
                r.ProductionDate
                ,
                r.ExpirationDate
                ,
                r.Remark
                ,
                r.Creator
                ,
                r.CreationTime
                ,
                r.Updator
                ,
                r.UpdateTime
                ,
                r.Str1
                ,
                r.Str2
                ,
                r.Str3
                ,
                r.Str4
                ,
                r.Str5
                ,
                r.Str6
                ,
                r.Str7
                ,
                r.Str8
                ,
                r.Str9
                ,
                r.Str10
                ,
                r.Str11
                ,
                r.Str12
                ,
                r.Str13
                ,
                r.Str14
                ,
                r.Str15
                ,
                r.Str16
                ,
                r.Str17
                ,
                r.Str18
                ,
                r.Str19
                ,
                r.Str20
                ,
                r.DateTime1
                ,
                r.DateTime2
                ,
                r.DateTime3
                ,
                r.DateTime4
                ,
                r.DateTime5
                ,
                r.Int1
                ,
                r.Int2
                ,
                r.Int3
                ,
                r.Int4
                ,
                r.Int5
                ,
                r.TenantId
            }).Select(a => new WMSReceiptReceiving
            {

                ASNDetailId = a.Key.ASNDetailId
                ,
                ReceiptId = a.Key.ReceiptId
                ,
                ReceiptDetailId = a.Key.ReceiptDetailId
                ,
                ReceiptNumber = a.Key.ReceiptNumber
                ,
                ExternReceiptNumber = a.Key.ExternReceiptNumber
                ,
                ASNNumber = a.Key.ASNNumber
                ,
                CustomerId = a.Key.CustomerId
                ,
                CustomerName = a.Key.CustomerName
                ,
                WarehouseId = a.Key.WarehouseId
                ,
                WarehouseName = a.Key.WarehouseName
                ,
                ReceiptReceivingStatus = a.Key.ReceiptReceivingStatus
                ,
                GoodsStatus = a.Key.GoodsStatus
                ,
                LineNumber = a.Key.LineNumber
                ,
                SKU = a.Key.SKU
                ,
                UPC = a.Key.UPC
                ,
                GoodsType = a.Key.GoodsType
                ,
                GoodsName = a.Key.GoodsName
                ,
                BoxCode = a.Key.BoxCode
                ,
                TrayCode = a.Key.TrayCode
                ,
                BatchCode = a.Key.BatchCode
                ,
                LotCode = a.Key.LotCode
                ,
                PoCode = a.Key.PoCode
                ,
                Weight = a.Key.Weight
                ,
                Volume = a.Key.Volume
                ,
                ReceivedQty = a.Sum(b => b.ReceivedQty)
                ,
                UnitCode = a.Key.UnitCode
                ,
                Onwer = a.Key.Onwer
                ,
                Area = a.Key.Area
                ,
                Location = a.Key.Location
                ,
                ProductionDate = a.Key.ProductionDate
                ,
                ExpirationDate = a.Key.ExpirationDate
                ,
                Remark = a.Key.Remark
                ,
                Creator = a.Key.Creator
                ,
                CreationTime = a.Key.CreationTime
                ,
                Updator = a.Key.Updator
                ,
                UpdateTime = a.Key.UpdateTime
                ,
                Str1 = a.Key.Str1
                ,
                Str2 = a.Key.Str2
                ,
                Str3 = a.Key.Str3
                ,
                Str4 = a.Key.Str4
                ,
                Str5 = a.Key.Str5
                ,
                Str6 = a.Key.Str6
                ,
                Str7 = a.Key.Str7
                ,
                Str8 = a.Key.Str8
                ,
                Str9 = a.Key.Str9
                ,
                Str10 = a.Key.Str10
                ,
                Str11 = a.Key.Str11
                ,
                Str12 = a.Key.Str12
                ,
                Str13 = a.Key.Str13
                ,
                Str14 = a.Key.Str14
                ,
                Str15 = a.Key.Str15
                ,
                Str16 = a.Key.Str16
                ,
                Str17 = a.Key.Str17
                ,
                Str18 = a.Key.Str18
                ,
                Str19 = a.Key.Str19
                ,
                Str20 = a.Key.Str20
                ,
                DateTime1 = a.Key.DateTime1
                ,
                DateTime2 = a.Key.DateTime2
                ,
                DateTime3 = a.Key.DateTime3
                ,
                DateTime4 = a.Key.DateTime4
                ,
                DateTime5 = a.Key.DateTime5
                ,
                Int1 = a.Key.Int1
                ,
                Int2 = a.Key.Int2
                ,
                Int3 = a.Key.Int3
                ,
                Int4 = a.Key.Int4
                ,
                Int5 = a.Key.Int5
                ,
                TenantId = a.Key.TenantId
            }).ToList();

            await _repReceiptReceiving.InsertRangeAsync(receiptReceiving);
            _repReceipt.Context.Updateable<WMSReceipt>()
                   .SetColumns(p => p.ReceiptStatus == (int)ReceiptStatusEnum.上架)
                   .Where(p => OrderData.Select(c => c.ReceiptId).Contains(p.Id))
                   .ExecuteCommand();
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "请扫描产品条码";
            return response;
        }
        //}
        //else
        //{
        //    //库位不存在，提示错误信息
        //    response.Code = StatusCode.Error;
        //    response.Msg = "库位不存在";
        //    return response;
        //}

        return response;
    }
}
