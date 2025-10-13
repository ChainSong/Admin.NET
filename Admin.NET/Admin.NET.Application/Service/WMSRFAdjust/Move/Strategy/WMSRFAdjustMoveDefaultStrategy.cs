// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Service.WMSRFAdjust.Move.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Dto;
using StackExchange.Profiling.Internal;
using Admin.NET.Core.Service;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;

namespace Admin.NET.Application.Service.WMSRFAdjust.Move.Strategy;
public class WMSRFAdjustMoveDefaultStrategy: IWMSRFAdjustMoveInterface
{
    private SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    private SqlSugarRepository<WMSCustomer> _repCustomer;
    private SqlSugarRepository<WMSWarehouse> _repWarehouse;
    private SqlSugarRepository<WMSLocation> _repLocation;
    private SqlSugarRepository<WMSProduct> _repProduct;
    private SysCacheService _sysCacheService;
    private UserManager _userManager;

    private readonly TimeSpan timeSpan = TimeSpan.FromHours(72);

    public void Init(
        SqlSugarRepository<WMSInventoryUsable> inventoryRepo,
        SqlSugarRepository<WMSCustomer> customerRepo,
        SqlSugarRepository<WMSWarehouse> warehouseRepo,
        SqlSugarRepository<WMSLocation> locationRepo,
        SqlSugarRepository<WMSProduct> productRepo,
        SysCacheService cacheService,
        UserManager userManager)
    {
        _repInventoryUsable = inventoryRepo;
        _repCustomer = customerRepo;
        _repWarehouse = warehouseRepo;
        _repLocation = locationRepo;
        _repProduct = productRepo;
        _sysCacheService = cacheService;
        _userManager = userManager;
    }
    public WMSRFAdjustMoveDefaultStrategy()
    {
    }
    /// <summary>
    /// 验证扫描值
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WMSRFAdjustMoveResponse> CheckScanValue(WMSRFAdjustMoveInput input)
    {
        var response = new WMSRFAdjustMoveResponse();
        var output = new WMSRFAdjustMoveOutput();

        #region 入参校验
        var missingFields = new List<string>();
        if (!input.CustomerId.HasValue) missingFields.Add("客户");
        if (!input.WarehouseId.HasValue) missingFields.Add("仓库");
        if (string.IsNullOrWhiteSpace(input.ScanValue)) missingFields.Add("扫描值");

        if (missingFields.Any())
        {
            response.Result = "Fail";
            response.Message = $"以下数据不能为空：{string.Join("、", missingFields)}";
            return response;
        }
        #endregion

        //如果操作单号为空则生成新的操作单号
        if (string.IsNullOrEmpty(input.OpSerialNumber))
            input.OpSerialNumber = $"{Guid.NewGuid():N}".Substring(0, 5) + DateTime.Now.ToString("MMddyyyy");

        var cacheKey = $"{_userManager.Account}_WMSRFAdjust_Move_{input.OpSerialNumber}";

        // 获取缓存操作信息
        var opInfo = _sysCacheService.Get<WMSRFAdjustMoveOutput>(cacheKey);

        #region 校验扫描值
        //不需要跟 customerId 和 warehouseId 进行关联校验
        var FormLocation = await _repLocation.AsQueryable()
            .Where(x => x.Location == input.ScanValue)
            .Where(x => x.LocationStatus == (int)LocationStatusEnum.可用)
            .Select(x => new
            {
                Data = x.Location,
                DataType = "Location"
            })
            .FirstAsync();

        var SKU = await _repProduct.AsQueryable()
        .Where(x => x.SKU == input.ScanValue)
        .Where(x => x.ProductStatus == (int)ProductStatusEnum.新增)
        .Select(x => new
        {
            Data = x.SKU,
            DataType = "SKU"
        })
        .FirstAsync();

        // 没查到任何数据
        if (FormLocation == null && SKU == null)
        {
            response.Result = "Fail";
            response.Message = $"未在库位或产品中找到扫描值 [{input.ScanValue}]";
            return response;
        }
        #endregion

        #region 已扫描
        if (opInfo != null)
        {
            if (FormLocation!=null)
            {
                //原库位为空  扫描原库位
                if (string.IsNullOrEmpty(opInfo.FromLocation))
                {
                    output.FromLocation = FormLocation.Data;
                    _sysCacheService.Set(_userManager.Account + "WMSRFAdjust_Move" + input.OpSerialNumber, output, timeSpan);

                    return new WMSRFAdjustMoveResponse
                    {
                        Result = "Success",
                        Message = "原库位扫描成功，请继续扫描SKU",
                        SerialNumber = input.OpSerialNumber
                    };
                }
                //扫描目标库位
                if (string.IsNullOrEmpty(opInfo.ToLocation))
                {
                    if (string.IsNullOrEmpty(opInfo.SKU))
                    {
                        return new WMSRFAdjustMoveResponse
                        {
                            Result = "Success",
                            Message = "请先扫描SKU",
                            SerialNumber = input.OpSerialNumber
                        };
                    }

                    opInfo.ToLocation = FormLocation.Data;
                    _sysCacheService.Set(cacheKey, opInfo, timeSpan);
 
                    //扫描完目标库位之后就开始建单

                    // 完成移库后清除缓存
                    _sysCacheService.Remove(cacheKey);

                    return new WMSRFAdjustMoveResponse
                    {
                        Result = "Success",
                        Message = "目标库位扫描完成，移动成功！",
                        SerialNumber = input.OpSerialNumber
                    };
                }
            }
            if (SKU!=null)
            {
                if (string.IsNullOrEmpty(opInfo.FromLocation))
                {
                    return new WMSRFAdjustMoveResponse
                    {
                        Result = "Success",
                        Message = "请先扫描原库位",
                        SerialNumber = input.OpSerialNumber
                    };
                }

                if (string.IsNullOrEmpty(opInfo.SKU))
                {
                    opInfo.SKU = SKU.Data;
                    opInfo.Qty = (opInfo.Qty ?? 0) + 1; // 避免空引用
                    _sysCacheService.Set(cacheKey, opInfo, timeSpan);

                    return new WMSRFAdjustMoveResponse
                    {
                        Result = "Success",
                        Message = $"SKU [{SKU.Data}] 扫描成功，请继续扫描或者扫描目标库位",
                        SerialNumber = input.OpSerialNumber
                    };
                }
            }

        }
        #endregion

        #region 新扫描（无缓存）
        if (opInfo == null)
        {
            if (FormLocation != null)
            {
                output.FromLocation = FormLocation.Data;
                _sysCacheService.Set(cacheKey, output, timeSpan);

                return new WMSRFAdjustMoveResponse
                {
                    Result = "Success",
                    Message = "原库位扫描成功，请继续扫描SKU",
                    SerialNumber = input.OpSerialNumber
                };
            }

            if (SKU != null)
            {
                return new WMSRFAdjustMoveResponse
                {
                    Result = "Fail",
                    Message = "请先扫描原库位",
                    SerialNumber = input.OpSerialNumber
                };
            }
        }
        #endregion

        // 默认成功兜底
        response.Result = "Success";
        response.Message = "扫描完成";
        response.SerialNumber = input.OpSerialNumber;
        var opInfoed = _sysCacheService.Get<WMSRFAdjustMoveOutput>(cacheKey);
        response.outputs.Add(opInfoed);
        return response;
    }
    /// <summary>
    /// 新增移库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WMSRFAdjustMoveResponse> AddAdjustmentMove(WMSRFAdjustMoveInput input)
    {
        WMSRFAdjustMoveResponse response = new WMSRFAdjustMoveResponse();

        return response;
    }
}
