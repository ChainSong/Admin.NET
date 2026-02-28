// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要版本的软件中必须包括上述版权声明和本许可声明。
//
// 软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Admin.NET.Application.Service;
using Admin.NET.Application.Enumerate;

namespace Admin.NET.Application;
/// <summary>
/// RF拣货策略（每次扫描即拣货一次，存入Redis）
/// </summary>
public class OrderPickWithRedisStrategy : IOrderPickRFInterface
{
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    /// <summary>
    /// 获取拣货任务明细（初始化拣货任务时调用）
    /// </summary>
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> OrderPickTask(WMSRFPickTaskInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        // 从数据库获取拣货明细
        var orderPickTask = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskId == request.Id)
            .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
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

        // 从缓存获取已拣货记录
        string cacheKey = GetCacheKey(request.CustomerId, request.WarehouseId, request.PickTaskNumber);
        var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

        // 计算实际拣货数量（从Redis统计）
        var pickQtyDict = pickedRecords
            .Where(r => !r.IsPackaged)
            .GroupBy(r => new { r.SKU, r.BatchCode, r.Location })
            .ToDictionary(g => g.Key, g => g.Sum(r => r.PickQty));

        // 更新拣货数量
        foreach (var item in orderPickTask)
        {
            var key = new { item.SKU, item.BatchCode, item.Location };
            if (pickQtyDict.ContainsKey(key))
            {
                item.PickQty = pickQtyDict[key];
                item.Order = (item.Qty == item.PickQty) ? 99 : 1;
            }
        }

        response.Data = orderPickTask.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();
        response.Code = StatusCode.Success;
        response.Msg = "操作成功";

        return response;
    }

    /// <summary>
    /// 扫描拣货（每次扫描即拣货一次，存入Redis）
    /// </summary>
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanOrderPickTask(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        // 解析扫描输入
        if (!string.IsNullOrEmpty(request.ScanInput))
        {
            ParseScanInput(request);
        }

        // 判断是扫描SKU还是库位
        if (!string.IsNullOrEmpty(request.SKU))
        {
            return await ScanSKU(request);
        }
        else if (!string.IsNullOrEmpty(request.Location))
        {
            return await ScanLocation(request);
        }
        else
        {
            response.Code = StatusCode.Error;
            response.Msg = "扫描数据无法识别";
            return response;
        }
    }

    /// <summary>
    /// 解析扫描输入
    /// </summary>
    private void ParseScanInput(WMSRFPickTaskDetailInput request)
    {
        if (string.IsNullOrEmpty(request.ScanInput))
            return;

        // 条形码格式：SKU|LOT|EXP
        if (request.ScanInput.Split(' ').Length > 1 || request.ScanInput.Split('|').Length > 1)
        {
            string skuRegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)";
            string lotRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d{4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
            string expirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";

            MatchCollection matchesSKU = Regex.Matches(request.ScanInput, skuRegex);
            request.SKU = matchesSKU.Count > 0 ? matchesSKU[0].Value : "";

            MatchCollection matchesExpirationDateRegex = Regex.Matches(request.ScanInput, expirationDateRegex);
            request.ExpirationDate = matchesExpirationDateRegex.Count > 0 ? matchesExpirationDateRegex[0].Value : "";

            MatchCollection matchesLOT = Regex.Matches(request.ScanInput, lotRegex);
            request.Lot = matchesLOT.Count > 0 ? matchesLOT[0].Value : "";
        }
        // HTTP 二维码
        else if (request.ScanInput.Contains("http"))
        {
            Uri uri = new Uri(request.ScanInput);
            var collection = HttpUtility.ParseQueryString(uri.Query);
            var p = collection["p"];
            if (p != null && p.Length > 0)
            {
                request.SKU = collection["p"].Split(':')[1];
            }
        }
        // 直接扫描产品条码
        else
        {
            var checkProduct = _repProduct.AsQueryable().Where(m => m.SKU == request.ScanInput).First();
            if (checkProduct != null && !string.IsNullOrEmpty(checkProduct.SKU))
            {
                request.SKU = checkProduct.SKU;
            }
        }
    }

    /// <summary>
    /// 扫描SKU拣货
    /// </summary>
    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanSKU(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        // 从数据库查询拣货明细
        var pickTaskDetails = await _repPickTaskDetail.AsQueryable()
            .Where(a => a.PickTaskId == request.Id
                && a.SKU == request.SKU
                && ((a.BatchCode == request.Lot || string.IsNullOrEmpty(a.BatchCode)) || string.IsNullOrEmpty(request.Lot))
                && a.Location == request.Location)
            .ToListAsync();

        if (pickTaskDetails == null || pickTaskDetails.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "该产品不存在于拣货任务中";
            return response;
        }

        var pickTaskDetail = pickTaskDetails.First();

        // 获取缓存中的已拣货记录
        string cacheKey = GetCacheKey(request.CustomerId, request.WarehouseId, request.PickTaskNumber);
        var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

        // 计算该SKU+批次+库位的已拣货数量（未包装的）
        var currentPickedQty = pickedRecords
            .Where(r => r.SKU == request.SKU
                && r.BatchCode == pickTaskDetail.BatchCode
                && r.Location == request.Location
                && !r.IsPackaged)
            .Sum(r => r.PickQty);

        // 检查是否已经拣货完成
        if (currentPickedQty >= pickTaskDetail.Qty)
        {
            response.Code = StatusCode.Error;
            response.Msg = "该产品已经全部拣货完成";
            return response;
        }

        // 创建拣货记录（每次扫描=拣货1次）
        var pickRecord = new RFSinglePickRecord
        {
            RecordId = Guid.NewGuid().ToString(),
            Id = pickTaskDetail.Id,
            PickTaskId = pickTaskDetail.PickTaskId,
            PickTaskNumber = request.PickTaskNumber,
            OrderId = pickTaskDetail.OrderId,
            OrderNumber = pickTaskDetail.OrderNumber,
            PreOrderNumber = pickTaskDetail.PreOrderNumber,
            ExternOrderNumber = pickTaskDetail.ExternOrderNumber,
            SKU = pickTaskDetail.SKU,
            UPC = pickTaskDetail.UPC,
            GoodsName = pickTaskDetail.GoodsName,
            GoodsType = pickTaskDetail.GoodsType,
            UnitCode = pickTaskDetail.UnitCode,
            Onwer = pickTaskDetail.Onwer,
            BoxCode = pickTaskDetail.BoxCode,
            TrayCode = pickTaskDetail.TrayCode,
            BatchCode = pickTaskDetail.BatchCode,
            LotCode = pickTaskDetail.LotCode,
            PoCode = pickTaskDetail.PoCode,
            Qty = pickTaskDetail.Qty,
            PickQty = 1, // 每次扫描只拣货1个
            Location = pickTaskDetail.Location,
            Area = pickTaskDetail.Area,
            ProductionDate = pickTaskDetail.ProductionDate,
            ExpirationDate = pickTaskDetail.ExpirationDate,
            PickTime = DateTime.Now,
            PickPersonnel = _userManager.Account,
            PickBoxNumber = "",
            IsPackaged = false,
            CustomerId = pickTaskDetail.CustomerId,
            CustomerName = pickTaskDetail.CustomerName,
            WarehouseId = pickTaskDetail.WarehouseId,
            WarehouseName = pickTaskDetail.WarehouseName
        };

        // 添加到缓存
        pickedRecords.Add(pickRecord);
        _sysCacheService.Set(cacheKey, pickedRecords);

        // 更新拣货任务状态
        await _repPickTask.Context.Updateable<WMSPickTask>()
            .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.拣货中)
            .Where(p => p.Id == request.Id)
            .ExecuteCommandAsync();

        // 返回拣货明细列表
        return await GetPickDetailListAsync(request);
    }

    /// <summary>
    /// 扫描库位
    /// </summary>
    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanLocation(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>() { Data = new List<WMSRFPickTaskDetailOutput>() };

        // 验证库位
        var checkLocation = await _repLocation.GetListAsync(it => it.Location == request.ScanInput);
        if (checkLocation == null || checkLocation.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "库位不存在";
            return response;
        }

        // 返回该库位的拣货明细
        return await GetPickDetailListAsync(request);
    }

    /// <summary>
    /// 获取拣货明细列表
    /// </summary>
    private async Task<Response<List<WMSRFPickTaskDetailOutput>>> GetPickDetailListAsync(WMSRFPickTaskDetailInput request)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>> { Data = new List<WMSRFPickTaskDetailOutput>() };

        // 从数据库获取拣货明细
        var pickTaskDetails = await _repPickTaskDetail.AsQueryable()
            .Where(a => a.PickTaskId == request.Id)
            .ToListAsync();

        // 从缓存获取已拣货记录
        string cacheKey = GetCacheKey(request.CustomerId, request.WarehouseId, request.PickTaskNumber);
        var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

        // 计算实际拣货数量（从Redis统计）
        var pickQtyDict = pickedRecords
            .Where(r => !r.IsPackaged)
            .GroupBy(r => new { r.SKU, r.BatchCode, r.Location })
            .ToDictionary(g => g.Key, g => g.Sum(r => r.PickQty));

        // 转换为输出格式
        var outputList = pickTaskDetails
            .GroupBy(a => new { a.SKU, a.BatchCode, a.CustomerId, a.WarehouseId, a.PickTaskNumber, a.Location })
            .Select(a => new WMSRFPickTaskDetailOutput
            {
                SKU = a.Key.SKU,
                Qty = a.Sum(x => x.Qty),
                PickQty = 0,
                BatchCode = a.Key.BatchCode,
                CustomerId = a.Key.CustomerId,
                WarehouseId = a.Key.WarehouseId,
                PickTaskNumber = a.Key.PickTaskNumber,
                Location = a.Key.Location,
                Order = 1
            })
            .ToList();

        // 更新拣货数量
        foreach (var item in outputList)
        {
            var key = new { item.SKU, item.BatchCode, item.Location };
            if (pickQtyDict.ContainsKey(key))
            {
                item.PickQty = pickQtyDict[key];
                item.Order = (item.Qty == item.PickQty) ? 99 : 1;
            }
        }

        // 标记当前位置
        foreach (var item in outputList)
        {
            if (item.Location == request.ScanInput)
            {
                item.Order = 0;
            }
        }

        response.Data = outputList.OrderBy(a => a.Order).ThenBy(a => a.Location).ToList();
        response.Code = StatusCode.Success;
        response.Msg = "拣货成功";

        return response;
    }

    /// <summary>
    /// 获取缓存Key
    /// </summary>
    private string GetCacheKey(long customerId, long warehouseId, string pickTaskNumber)
    {
        return $"RFSinglePick:{customerId}:{warehouseId}:{pickTaskNumber}";
    }
}
