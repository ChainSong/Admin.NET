// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Enumerate;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;
using System.Text.RegularExpressions;
using Admin.NET.Application.Service;
using System.Web;

namespace Admin.NET.Application;
public class OrderPickRFStrategy : IOrderPickRFInterface
{
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> OrderPickTask(WMSRFPickTaskInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        var orderPickTask = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id)
            .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
            //.OrderBy(a => a.Location)
            .Select(a => new WMSRFPickTaskDetailOutput()
            {
                SKU = a.SKU,
                Qty = SqlFunc.AggregateSum(a.Qty),
                PickQty = SqlFunc.AggregateSum(a.PickQty),
                BatchCode = a.BatchCode,
                CustomerId = a.CustomerId,
                WarehouseId = a.WarehouseId,
                PickTaskNumber = a.PickTaskNumber,
                Location = a.Location,
                Order = (SqlFunc.AggregateSum(a.Qty) == SqlFunc.AggregateSum(a.PickQty) ? 99 : 1)
            }).ToListAsync();

        List<WMSRFPickTaskDetailOutput> pickTaskDetailOutputs = new List<WMSRFPickTaskDetailOutput>();
        //判断有没有缓存，没有就加一个
        if (!_sysCacheService.ExistKey("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + orderPickTask.First().PickTaskNumber))
        {
            var data = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id).ToListAsync();
            pickTaskDetailOutputs = data.Adapt(pickTaskDetailOutputs);
            _sysCacheService.Set("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + orderPickTask.First().PickTaskNumber, pickTaskDetailOutputs);
        }
        var pickTaskDetail = _sysCacheService.Get<List<WMSRFPickTaskDetailOutput>>("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + request.PickTaskNumber);


        response.Data = pickTaskDetail.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();
        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        //throw new NotImplementedException();
        return response;
    }
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanOrderPickTask(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };
        //判断扫描的是不是条形码（有两种条形码）
        if (!string.IsNullOrEmpty(request.ScanInput))
        {
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
                request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
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
            //request.CustomerId = receipt.CustomerId;
        }


        //判断扫描的是产品条码还是库位
        if (!string.IsNullOrEmpty(request.SKU))
        {
            var data = await ScanSKU(request);
            if (data.Code == StatusCode.Error)
            {
                response.Code = data.Code;
                response.Msg = data.Msg;
                return response;
            }
            else
            {
                return data;
            }
        }
        else
        {
            //判断扫描的是库位
            var CheckLocation = await _repLocation.GetListAsync(it => it.Location == request.ScanInput);
            if (CheckLocation != null && CheckLocation.Count > 0)
            {
                var data = await ScanLocation(request);
                if (data.Code == StatusCode.Error)
                {
                    response.Code = data.Code;
                    response.Msg = data.Msg;
                    return response;
                }
                else
                {
                    return data;
                    //_sysCacheService.Remove("RFReceiptReceivingScan:" + request.CustomerId + ":" + request.ReceiptNumber);
                }
            }
            else
            {
                response.Code = StatusCode.Error;
                response.Msg = "扫描数据无法识别";
                return response;
            }
        }
    }

    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanSKU(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        var orderPickTask = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id
           && a.SKU == request.SKU
           && (a.BatchCode == request.Lot || string.IsNullOrEmpty(a.BatchCode))
           && a.Location == request.Location
           && a.PickQty < a.Qty
        ).ToListAsync();

        if (orderPickTask != null && orderPickTask.Count > 0)
        {
            var pickTaskDetail = orderPickTask.First();

            pickTaskDetail.PickTime = DateTime.Now;
            pickTaskDetail.PickPersonnel = _userManager.Account;
            pickTaskDetail.PickStatus = (pickTaskDetail.PickQty + 1) == (pickTaskDetail.Qty) ? (int)PickTaskStatusEnum.拣货完成 : (int)PickTaskStatusEnum.拣货中;
            pickTaskDetail.PickQty += 1;
            //await _repPickTask.UpdateAsync(pickTaskDetail);
            await _repPickTask.Context.Updateable<WMSPickTask>()
            .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.拣货中)
            .Where(p => p.Id == pickTaskDetail.PickTaskId)
            .ExecuteCommandAsync();
            await _repPickTaskDetail.UpdateAsync(pickTaskDetail);
            //判断是不是需要更新订单状态
            var pickCount = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id && a.PickQty < a.Qty).CountAsync();
            if (pickCount == 0)
            {
                await _repPickTask.Context.Updateable<WMSPickTask>()
                .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                .Where(p => p.Id == pickTaskDetail.PickTaskId)
                .ExecuteCommandAsync();

                await _repPickTask.Context.Updateable<WMSPickTaskDetail>()
                .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.拣货完成)
                .Where(p => p.PickTaskId == pickTaskDetail.PickTaskId)
                .ExecuteCommandAsync();
            }
        }
        else
        {

            response.Code = StatusCode.Error;
            response.Msg = "该产品已经全部拣货完成";
            return response;
        }

        //pickTaskDetailScan.Add(pickTaskDetail.First());

        var pickTask = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id)
         .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
         //.OrderBy(a => a.Location)
         .Select(a => new WMSRFPickTaskDetailOutput()
         {
             SKU = a.SKU,
             Qty = SqlFunc.AggregateSum(a.Qty),
             PickQty = SqlFunc.AggregateSum(a.PickQty),
             BatchCode = a.BatchCode,
             CustomerId = a.CustomerId,
             WarehouseId = a.WarehouseId,
             PickTaskNumber = a.PickTaskNumber,
             Location = a.Location,
             Order = (SqlFunc.AggregateSum(a.Qty) == SqlFunc.AggregateSum(a.PickQty) ? 99 : 1)
         }).ToListAsync();
        foreach (var item in pickTask)
        {
            if (item.Location == request.ScanInput)
            {
                item.Order = 0;
            }
        }
        response.Data = pickTask.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();
        response.Code = StatusCode.Success;
        response.Msg = "SKU";
        //throw new NotImplementedException();
        return response;
    }



    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanLocation(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        var orderPickTask = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id)
           .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
           //.OrderBy(a => a.Location)
           .Select(a => new WMSRFPickTaskDetailOutput()
           {
               SKU = a.SKU,
               Qty = SqlFunc.AggregateSum(a.Qty),
               PickQty = SqlFunc.AggregateSum(a.PickQty),
               BatchCode = a.BatchCode,
               CustomerId = a.CustomerId,
               WarehouseId = a.WarehouseId,
               PickTaskNumber = a.PickTaskNumber,
               Location = a.Location,
               Order = (SqlFunc.AggregateSum(a.Qty) == SqlFunc.AggregateSum(a.PickQty) ? 99 : 1)
           }).ToListAsync();


        foreach (var item in orderPickTask)
        {
            if (item.Location == request.ScanInput)
            {
                item.Order = 0;
            }
        }
        //orderPickTask = orderPickTask.OrderBy(a => new { a.Order, a.Location }).ToList();
        List<WMSRFPickTaskDetailOutput> pickTaskDetailOutputs = new List<WMSRFPickTaskDetailOutput>();
        //判断有没有缓存，没有就加一个
        if (!_sysCacheService.ExistKey("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + orderPickTask.First().PickTaskNumber))
        {
            var data = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id).ToListAsync();
            pickTaskDetailOutputs = data.Adapt(pickTaskDetailOutputs);
            _sysCacheService.Set("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + orderPickTask.First().PickTaskNumber, pickTaskDetailOutputs);
        }
        //var pickTaskDetail = _sysCacheService.Get<List<WMSRFPickTaskDetailOutput>>("RFPickTask:" + orderPickTask.First().CustomerId + ":" + orderPickTask.First().WarehouseId + ":" + request.PickTaskNumber);


        response.Data = orderPickTask.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();

        response.Code = StatusCode.Success;
        response.Msg = "Location";
        //throw new NotImplementedException();
        return response;
    }
}
