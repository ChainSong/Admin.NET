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

//namespace Admin.NET.Application;
///// <summary>
///// RF包装策略
///// </summary>
//public class RFPackageStrategy : IOrderPickRFInterface
//{
//    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
//    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
//    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
//    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
//    public UserManager _userManager { get; set; }
//    public SysCacheService _sysCacheService { get; set; }
//    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
//    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
//    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
//    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
//    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

//    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> Process(RFPickAndPackageInput request)
//    {
//        // 验证输入
//        if (string.IsNullOrEmpty(request.PickTaskNumber))
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "拣货任务号不能为空",
//                Data = null
//            };
//        }

//        if (string.IsNullOrEmpty(request.BoxNumber))
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "箱号不能为空",
//                Data = null
//            };
//        }

//        // 获取拣货任务
//        var pickTask = await _repPickTask.AsQueryable()
//            .Where(a => a.PickTaskNumber == request.PickTaskNumber)
//            .FirstAsync();

//        if (pickTask == null)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "拣货任务不存在",
//                Data = null
//            };
//        }

//        // 从缓存获取拣货数据
//        string cacheKey = GetCacheKey(pickTask.CustomerId, pickTask.WarehouseId, request.PickTaskNumber);
//        var cachedData = _sysCacheService.Get<List<RFPickCacheData>>(cacheKey);

//        if (cachedData == null || cachedData.Count == 0)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "没有可包装的拣货数据",
//                Data = null
//            };
//        }

//        // 过滤出已拣货但未包装的数据
//        var itemsToPackage = cachedData
//            .Where(a => a.PickQty > 0 && !a.IsPackaged)
//            .ToList();

//        if (itemsToPackage.Count == 0)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = "没有已拣货但未包装的商品",
//                Data = null
//            };
//        }

//        try
//        {
//            // 创建包装记录
//            var firstOrderNumber = pickTask.OrderNumber?.Split(',')[0] ?? "";
//            var firstExternOrderNumber = pickTask.ExternOrderNumber?.Split(',')[0] ?? "";
//            var firstOrderId = itemsToPackage.First().OrderId;

//            var package = new WMSPackage
//            {
//                PickTaskId = pickTask.Id,
//                PickTaskNumber = pickTask.PickTaskNumber,
//                OrderId = firstOrderId,
//                OrderNumber = firstOrderNumber,
//                ExternOrderNumber = firstExternOrderNumber,
//                PackageNumber = request.BoxNumber,
//                CustomerId = pickTask.CustomerId,
//                CustomerName = pickTask.CustomerName,
//                WarehouseId = pickTask.WarehouseId,
//                WarehouseName = pickTask.WarehouseName,
//                PackageType = "标准箱",
//                PackageStatus = 1,
//                PackageTime = DateTime.Now,
//                Creator = _userManager.Account,
//                CreationTime = DateTime.Now,
//                DetailCount = itemsToPackage.Count
//            };

//            var packageId = await _repPackage.InsertReturnIdentityAsync(package);

//            // 创建包装明细记录
//            List<WMSPackageDetail> packageDetails = new List<WMSPackageDetail>();
//            foreach (var item in itemsToPackage)
//            {
//                packageDetails.Add(new WMSPackageDetail
//                {
//                    PickTaskId = item.PickTaskId,
//                    PackageId = packageId,
//                    OrderId = item.OrderId,
//                    OrderNumber = item.OrderNumber,
//                    ExternOrderNumber = item.ExternOrderNumber,
//                    PickTaskNumber = request.PickTaskNumber,
//                    PackageNumber = request.BoxNumber,
//                    CustomerId = pickTask.CustomerId,
//                    CustomerName = pickTask.CustomerName,
//                    WarehouseId = pickTask.WarehouseId,
//                    WarehouseName = pickTask.WarehouseName,
//                    SKU = item.SKU,
//                    UPC = item.UPC,
//                    GoodsName = item.GoodsName,
//                    GoodsType = item.GoodsType,
//                    UnitCode = item.UnitCode,
//                    Onwer = item.Onwer,
//                    BoxCode = item.BoxCode,
//                    TrayCode = item.TrayCode,
//                    BatchCode = item.BatchCode,
//                    LotCode = item.LotCode,
//                    PoCode = item.PoCode,
//                    Qty = item.PickQty,
//                    ProductionDate = item.ProductionDate,
//                    ExpirationDate = item.ExpirationDate,
//                    Creator = _userManager.Account,
//                    CreationTime = DateTime.Now
//                });

//                // 标记为已包装
//                item.IsPackaged = true;
//            }

//            await _repPackageDetail.InsertRangeAsync(packageDetails);

//            // 更新缓存
//            _sysCacheService.Set(cacheKey, cachedData);

//            // 检查是否所有商品都已包装
//            var allPackaged = cachedData.All(a => a.IsPackaged);
//            if (allPackaged)
//            {
//                // 更新拣货任务状态为包装完成
//                pickTask.PickStatus = (int)PickTaskStatusEnum.包装完成;
//                pickTask.EndTime = DateTime.Now;
//                await _repPickTask.UpdateAsync(pickTask);

//                // 清除缓存
//                _sysCacheService.Remove(cacheKey);
//            }

//            // 返回未包装的拣货明细（继续拣货）
//            var remainingItems = GetRemainingPickDetails(cachedData, request);

//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Success,
//                Msg = $"箱号 {request.BoxNumber} 包装完成，共包装 {itemsToPackage.Count} 条明细",
//                Data = remainingItems
//            };
//        }
//        catch (Exception ex)
//        {
//            return new Response<List<WMSRFPickTaskDetailOutput>>
//            {
//                Code = StatusCode.Error,
//                Msg = $"包装失败：{ex.Message}",
//                Data = null
//            };
//        }
//    }

//    /// <summary>
//    /// 获取剩余拣货明细
//    /// </summary>
//    private List<WMSRFPickTaskDetailOutput> GetRemainingPickDetails(List<RFPickCacheData> cachedData, RFPickAndPackageInput request)
//    {
//        return cachedData
//            .Where(a => !a.IsPackaged || (a.Qty > a.PickQty && !a.IsPackaged))
//            .Select(a => new WMSRFPickTaskDetailOutput
//            {
//                SKU = a.SKU,
//                Qty = a.Qty,
//                PickQty = a.PickQty,
//                BatchCode = a.BatchCode,
//                Location = a.Location,
//                Order = ((a.Qty - a.PickQty) <= 0) ? 99 : 1
//            })
//            .OrderBy(a => a.Order)
//            .ThenBy(a => a.Location)
//            .ToList();
//    }

//    /// <summary>
//    /// 获取缓存Key
//    /// </summary>
//    private string GetCacheKey(long customerId, long warehouseId, string pickTaskNumber)
//    {
//        return $"RFPickAndPackage:{customerId}:{warehouseId}:{pickTaskNumber}";
//    }
//}
