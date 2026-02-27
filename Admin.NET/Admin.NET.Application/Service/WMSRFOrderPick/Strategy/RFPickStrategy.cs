//// 麻省理工学院许可证
////
//// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
////
//// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要版本的软件中必须包括上述版权声明和本许可声明。
////
//// 软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
//// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

//using Admin.NET.Application.Dtos;
//using Admin.NET.Application.Dtos.Enum;
//using Admin.NET.Application.Service;
//using Admin.NET.Core;
//using Admin.NET.Core.Entity;
//using Admin.NET.Core.Service;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web;

//namespace Admin.NET.Application;
///// <summary>
///// RF拣货策略
///// </summary>
//public class RFPickStrategy : IOrderPickRFInterface
//{
//    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
//    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
//    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
//    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
//    public UserManager _userManager { get; set; }
//    public SysCacheService _sysCacheService { get; set; }
//    public SqlSugarRepository<Admin.NET.Core.Entity.WMSOrder> _repOrder { get; set; }
//    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
//    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
//    public SqlSugarRepository<Admin.NET.Core.Entity.WMSPackage> _repPackage { get; set; }
//    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

//    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> Process(RFPickAndPackageInput request)
//    {
//        // 解析扫描输入
//        if (!string.IsNullOrEmpty(request.ScanInput))
//        {
//            ParseScanInput(request);
//        }

//        // 判断是扫描SKU还是库位
//        if (!string.IsNullOrEmpty(request.SKU))
//        {
//            return await ScanSKU(request);
//        }
//        else if (!string.IsNullOrEmpty(request.Location))
//        {
//            return await ScanLocation(request);
//        }
//        else
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "扫描数据无法识别",
//                Data = null
//            };
//        }
//    }

//    /// <summary>
//    /// 解析扫描输入
//    /// </summary>
//    private void ParseScanInput(RFPickAndPackageInput request)
//    {
//        if (string.IsNullOrEmpty(request.ScanInput))
//            return;

//        // 条形码格式：SKU|LOT|EXP
//        if (request.ScanInput.Split(' ').Length > 1 || request.ScanInput.Split('|').Length > 1)
//        {
//            string skuRegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)";
//            string lotRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d{4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
//            string expirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";

//            MatchCollection matchesSKU = Regex.Matches(request.ScanInput, skuRegex);
//            request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";

//            MatchCollection matchesExpirationDateRegex = Regex.Matches(request.ScanInput, expirationDateRegex);
//            request.ExpirationDate = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";

//            MatchCollection matchesLOT = Regex.Matches(request.ScanInput, lotRegex);
//            request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
//        }
//        // HTTP 二维码
//        else if (request.ScanInput.Contains("http"))
//        {
//            Uri uri = new Uri(request.ScanInput);
//            var collection = HttpUtility.ParseQueryString(uri.Query);
//            var p = collection["p"];
//            if (p != null && p.Length > 0)
//            {
//                request.SKU = collection["p"].Split(':')[1];
//            }
//        }
//        // 直接扫描产品条码
//        else
//        {
//            var checkProduct = _repProduct.AsQueryable().Where(m => m.SKU == request.ScanInput).First();
//            if (checkProduct != null && !string.IsNullOrEmpty(checkProduct.SKU))
//            {
//                request.SKU = checkProduct.SKU;
//            }
//        }
//    }

//    /// <summary>
//    /// 扫描SKU拣货
//    /// </summary>
//    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanSKU(RFPickAndPackageInput request)
//    {
//        // 从数据库查询拣货明细
//        var pickTaskDetails = await _repPickTaskDetail.AsQueryable()
//            .Where(a => a.PickTaskId == request.Id
//                && a.SKU == request.SKU
//                && ((a.BatchCode == request.Lot || string.IsNullOrEmpty(a.BatchCode)) || string.IsNullOrEmpty(request.Lot))
//                && a.Location == request.Location
//                && a.PickQty < a.Qty)
//            .ToListAsync();

//        if (pickTaskDetails == null || pickTaskDetails.Count == 0)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "该产品已经全部拣货完成或不存在",
//                Data = null
//            };
//        }

//        var pickTaskDetail = pickTaskDetails.First();

//        // 更新数据库
//        pickTaskDetail.PickTime = DateTime.Now;
//        pickTaskDetail.PickPersonnel = _userManager.Account;
//        pickTaskDetail.PickStatus = (pickTaskDetail.PickQty + 1) == pickTaskDetail.Qty ? (int)PickTaskStatusEnum.拣货完成 : (int)PickTaskStatusEnum.拣货中;
//        pickTaskDetail.PickQty += 1;
//        pickTaskDetail.PickBoxNumber = request.BoxNumber;
//        await _repPickTaskDetail.UpdateAsync(pickTaskDetail);

//        // 更新拣货任务状态
//        await _repPickTask.Context.Updateable<WMSPickTask>()
//            .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.拣货中)
//            .Where(p => p.Id == pickTaskDetail.PickTaskId)
//            .ExecuteCommandAsync();

//        // 同步到Redis缓存
//        await SyncToCacheAsync(pickTaskDetail, request);

//        // 返回拣货明细列表
//        return await GetPickDetailListAsync(request);
//    }

//    /// <summary>
//    /// 扫描库位
//    /// </summary>
//    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanLocation(RFPickAndPackageInput request)
//    {
//        // 验证库位
//        var checkLocation = await _repLocation.GetListAsync(it => it.Location == request.ScanInput);
//        if (checkLocation == null || checkLocation.Count == 0)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "库位不存在",
//                Data = null
//            };
//        }

//        // 返回该库位的拣货明细
//        return await GetPickDetailListAsync(request);
//    }

//    /// <summary>
//    /// 同步到缓存
//    /// </summary>
//    private async Task SyncToCacheAsync(WMSPickTaskDetail detail, RFPickAndPackageInput request)
//    {
//        string cacheKey = GetCacheKey(request.CustomerId, request.WarehouseId, request.PickTaskNumber);
//        var cachedData = _sysCacheService.Get<List<RFPickCacheData>>(cacheKey);

//        if (cachedData == null)
//        {
//            cachedData = new List<RFPickCacheData>();
//        }

//        // 查找或创建缓存记录
//        var cacheItem = cachedData.FirstOrDefault(a =>
//            a.SKU == detail.SKU &&
//            a.BatchCode == detail.BatchCode &&
//            a.Location == detail.Location);

//        if (cacheItem != null)
//        {
//            // 更新缓存记录
//            cacheItem.PickQty = detail.PickQty;
//            cacheItem.PickStatus = detail.PickStatus;
//            cacheItem.PickTime = detail.PickTime;
//            cacheItem.PickPersonnel = detail.PickPersonnel;
//            cacheItem.PickBoxNumber = detail.PickBoxNumber;
//        }
//        else
//        {
//            // 创建新的缓存记录
//            cachedData.Add(new RFPickCacheData
//            {
//                PickTaskId = detail.PickTaskId,
//                OrderId = detail.OrderId,
//                OrderNumber = detail.OrderNumber,
//                ExternOrderNumber = detail.ExternOrderNumber,
//                SKU = detail.SKU,
//                UPC = detail.UPC,
//                GoodsName = detail.GoodsName,
//                GoodsType = detail.GoodsType,
//                UnitCode = detail.UnitCode,
//                Onwer = detail.Onwer,
//                BoxCode = detail.BoxCode,
//                TrayCode = detail.TrayCode,
//                BatchCode = detail.BatchCode,
//                LotCode = detail.LotCode,
//                PoCode = detail.PoCode,
//                Qty = detail.Qty,
//                PickQty = detail.PickQty,
//                Location = detail.Location,
//                Area = detail.Area,
//                ProductionDate = detail.ProductionDate,
//                ExpirationDate = detail.ExpirationDate,
//                PickStatus = detail.PickStatus,
//                PickTime = detail.PickTime,
//                PickPersonnel = detail.PickPersonnel,
//                PickBoxNumber = detail.PickBoxNumber,
//                IsPackaged = false
//            });
//        }

//        // 更新缓存
//        _sysCacheService.Set(cacheKey, cachedData);
//    }

//    /// <summary>
//    /// 获取拣货明细列表
//    /// </summary>
//    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> GetPickDetailListAsync(RFPickAndPackageInput request)
//    {
//        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>> { Data = new List<WMSRFPickTaskDetailOutput>() };

//        // 从数据库获取拣货明细
//        var pickTaskDetails = await _repPickTaskDetail.AsQueryable()
//            .Where(a => a.PickTaskId == request.Id)
//            .ToListAsync();

//        // 转换为输出格式
//        var outputList = pickTaskDetails
//            .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
//            .Select(a => new WMSRFPickTaskDetailOutput
//            {
//                SKU = a.Key.SKU,
//                Qty = a.Sum(x => x.Qty),
//                PickQty = a.Sum(x => x.PickQty),
//                BatchCode = a.Key.BatchCode,
//                CustomerId = a.Key.CustomerId,
//                WarehouseId = a.Key.WarehouseId,
//                PickTaskNumber = a.Key.PickTaskNumber,
//                Location = a.Key.Location,
//                Order = (a.Sum(x => x.Qty) == a.Sum(x => x.PickQty)) ? 99 : 1
//            })
//            .ToList();

//        // 标记当前位置
//        foreach (var item in outputList)
//        {
//            if (item.Location == request.ScanInput)
//            {
//                item.Order = 0;
//            }
//        }

//        response.Data = outputList.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();
//        response.Code = StatusCode.Success;
//        response.Msg = "拣货成功";

//        return response;
//    }

//    /// <summary>
//    /// 获取缓存Key
//    /// </summary>
//    private string GetCacheKey(long customerId, long warehouseId, string pickTaskNumber)
//    {
//        return $"RFPickAndPackage:{customerId}:{warehouseId}:{pickTaskNumber}";
//    }
//}
