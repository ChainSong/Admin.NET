// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Application.Dtos.Enum;
using System.Text.RegularExpressions;
using Aliyun.OSS;
using NPOI.SS.Formula.Functions;
using SharpCompress.Common;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;
using System.Reflection.Emit;
using Admin.NET.Core.Service;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;
using System.Web;

namespace Admin.NET.Application.Service;
public class ASNCountQuantityRFStrategy : ASNCountQuantityRFInterface
{
    //数据库实例
    //ISqlSugarClient _db { get; set; }
    //用户仓储
    public UserManager _userManager { get; set; }
    //asn仓储
    public SqlSugarRepository<WMSASN> _repASN { get; set; }
    //ASNDetail 仓储
    public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
    public SqlSugarRepository<WMSASNCountQuantity> _repASNCountQuantity { get; set; }
    public SqlSugarRepository<WMSASNCountQuantityDetail> _repASNCountQuantityDetail { get; set; }

    //客户用户关系仓储
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    //产品仓储
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

    //仓库用户关系仓储
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }



    /// <summary>
    /// 创建点数质检单
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Response<List<WMSASNCountQuantityDetailDto>>> ScanAddStrategy(WMSASNCountQuantityDetailDto request)
    {
        Response<List<WMSASNCountQuantityDetailDto>> response = new Response<List<WMSASNCountQuantityDetailDto>>() { Data = new List<WMSASNCountQuantityDetailDto>() };
        //判断扫描的是不是条形码（有两种条形码）
        if (!string.IsNullOrEmpty(request.ScanInput))
        {
            //var skuInfo = request.Input.Split('|');
            //var skuInfo = request.Input.Split('|');
            if (request.ScanInput.Split(' ').Length > 1 || request.ScanInput.Split('|').Length > 1)
            {

                string skuRegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)"; // 正则表达式匹配英文字符或数字
                string lotRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d{4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
                string expirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";
                MatchCollection matchesSKU = Regex.Matches(request.ScanInput, skuRegex);
                request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";

                MatchCollection matchesExpirationDateRegex = Regex.Matches(request.ScanInput, expirationDateRegex);
                request.ExpirationDate = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";
                MatchCollection matchesLOT = Regex.Matches(request.ScanInput, lotRegex);
                request.BatchCode = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
                request.SKU = request.SKU;

            }
            //扫描的是HTTP 二维码，那么从中解析SKU
            else if (request.ScanInput.Contains("http"))
            {

                Uri uri = new Uri(request.ScanInput);
                var collection = HttpUtility.ParseQueryString(uri.Query);
                var p = collection["p"];
                if (p.Count() > 0)
                {
                    request.SKU = collection["p"].Split(':')[1];
                    request.SnCode = collection["p"].Split(':')[0];
                }
            }
            else
            {
                //判断是不是不需要解析，直接扫描的产品条码
                var checkProduct = _repProduct.AsQueryable().Where(m => m.SKU == request.ScanInput).First();
                if (checkProduct != null || !string.IsNullOrEmpty(checkProduct.SKU))
                {
                    request.SKU = checkProduct.SKU;
                }
            }
            //request.CustomerId = request.CustomerId;
            //request.CustomerId = receipt.CustomerId;
        }



        //获取ASN明细中的同产品信息，补全基本信息
        //此处不补全ID等详细信息
        var asnDetailList = await _repASNDetail.AsQueryable().Where(m => m.ASNId == request.ASNId).ToListAsync();
        if (asnDetailList != null && asnDetailList.Count > 0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WMSASNDetail, WMSASNCountQuantityDetail>()
                   //添加创建人为当前用户
                   //.ForMember(a => a.Creator, opt => opt.MapFrom(c => abpSession.UserName))
                   //.ForMember(a => a.ASNDetails, opt => opt.MapFrom(c => c.ASNDetails))
                   //添加库存状态为可用
                   //.ForMember(a => a.ASNId, opt => opt.MapFrom(c => c.Id))
                   //.ForMember(a => a.ASNCountQuantityStatus, opt => opt.MapFrom(c => ASNCountQuantityStatusEnum.新增))
                   .ForMember(a => a.Creator, opt => opt.MapFrom(c => _userManager.Account))
                   .ForMember(a => a.CreationTime, opt => opt.MapFrom(c => DateTime.Now))
                   .ForMember(a => a.LineNumber, opt => opt.MapFrom(c => 0))
                   .ForMember(a => a.UPC, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.BoxCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.TrayCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.BatchCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.LotCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.PoCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.Weight, opt => opt.MapFrom(c => 0))
                   .ForMember(a => a.Volume, opt => opt.MapFrom(c => 0))
                   .ForMember(a => a.ExpectedQty, opt => opt.MapFrom(c => 0))
                   .ForMember(a => a.ReceivedQty, opt => opt.MapFrom(c => 0))
                   //.ForMember(a => a.Qty, opt => opt.MapFrom(c => 0))
                   .ForMember(a => a.UnitCode, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.Onwer, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.Remark, opt => opt.MapFrom(c => ""))
                   //.ForMember(a => a.Updator, opt => opt.MapFrom(c => ""))
                   //.ForMember(a => a.UpdateTime, opt => opt.MapFrom(c => ""))
                   .ForMember(a => a.Updator, opt => opt.Ignore());

            });
            //校验SKU 是否存在订单中

            var checkProduct = await _repASNDetail.AsQueryable().Where(m => m.ASNId == request.ASNId && m.SKU == request.SKU).ToListAsync();
            if (checkProduct == null && checkProduct.Count == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "该SKU不存在订单中";
            }
            var chechQty = await _repASNCountQuantityDetail.AsQueryable().Where(m => m.ASNId == request.ASNId && m.SKU == request.SKU).SumAsync(a => a.Qty);
            if (chechQty + 1 > checkProduct.Sum(a => a.ExpectedQty))
            {
                response.Code = StatusCode.Error;
                response.Msg = "该SKU数量已超过订单中数量";
            }

            var mapper = new Mapper(config);
            string goodsName = await _repProduct.AsQueryable().Where(m => m.SKU == request.SKU).Select(m => m.GoodsName).FirstAsync();
            //使用Mapper将ASN信息转为ASNCountQuantity信息
            var wMSASNCountQuantityDetail = mapper.Map<WMSASNCountQuantityDetail>(asnDetailList.First());
            //WMSASNCountQuantityDetail wMSASNCountQuantityDetail = new WMSASNCountQuantityDetail();
            wMSASNCountQuantityDetail.ASNCountQuantityId = request.Id;
            wMSASNCountQuantityDetail.ASNDetailId = 0;
            wMSASNCountQuantityDetail.Qty = 1;
            wMSASNCountQuantityDetail.ASNId = request.ASNId;
            wMSASNCountQuantityDetail.BatchCode = request.BatchCode;
            wMSASNCountQuantityDetail.ExpirationDate = string.IsNullOrEmpty(request.ExpirationDate) ? null : DateTime.Parse(request.ExpirationDate);
            //wMSASNCountQuantityDetail.Input = request.ScanInput;
            wMSASNCountQuantityDetail.LotCode = "";
            //wMSASNCountQuantityDetail.Quantity = request.Quantity;
            wMSASNCountQuantityDetail.SKU = request.SKU;
            wMSASNCountQuantityDetail.SnCode = request.SnCode;
            wMSASNCountQuantityDetail.GoodsName = goodsName;
            //wMSASNCountQuantityDetail.Creator = request.Creator;
            //wMSASNCountQuantityDetail.CreationTime = request.CreationTime;
            //wMSASNCountQuantityDetail.Id = SnowFlakeHelper.GetSnowInstance().NextId();
            await _repASNCountQuantityDetail.InsertAsync(wMSASNCountQuantityDetail);
            //response.Data.Add(Mapper.Map<WMSRFPickTaskDetailOutput>(wMSASNCountQuantityDetail));

            var responseList = await _repASNCountQuantityDetail.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(request.BatchCode), m => m.BatchCode.Contains(request.BatchCode))
                .Where(m => m.ASNId == request.ASNId
            && m.SKU == request.SKU
            ).GroupBy(a => new { a.ExternReceiptNumber, a.ASNNumber, a.SKU, a.BatchCode, a.ASNId }).Select(a => new WMSASNCountQuantityDetailDto()
            {
                SKU = a.SKU,
                ASNId = a.ASNId,
                ASNNumber = a.ASNNumber,
                ExternReceiptNumber = a.ExternReceiptNumber,
                BatchCode = a.BatchCode,
                ExpectedQty = SqlFunc.Subqueryable<WMSASNDetail>().Where(c => c.SKU == a.SKU && c.ASNId == a.ASNId).Max(a => a.ExpectedQty),
                Qty = SqlFunc.AggregateSum(a.Qty),
            }).ToListAsync();
            response.Data = responseList;
            response.Code = StatusCode.Success;
            response.Msg = "新增点数质检单成功";
        }
        return response;
    }
}
