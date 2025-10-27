// 麻省理工学院许可证

// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995

// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。

// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Const;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Dto;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Interface;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Factory;
using Admin.NET.Application.Enumerate;
using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Identity;

namespace Admin.NET.Application.Service.wMsRFAdjust;
/// <summary>
/// RFAdjustmentMove服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 101)]
public class WMSRFAdjustMoveService : IDynamicApiController, ITransient
{
    #region 依赖注入
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;
    private readonly SqlSugarRepository<WMSLocation> _repLocation;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SysCacheService _sysCacheService;
    private readonly UserManager _userManager;
    private readonly WMSAdjustmentService _wmsAdjustmentService;
    private readonly SqlSugarRepository<WMSAdjustment> _repAdjustment;
    public WMSRFAdjustMoveService(
        SqlSugarRepository<WMSInventoryUsable> inventoryRepo,
        SqlSugarRepository<WMSCustomer> customerRepo,
        SqlSugarRepository<WMSWarehouse> warehouseRepo,
        SqlSugarRepository<WMSLocation> locationRepo,
        SqlSugarRepository<WMSProduct> productRepo,
        SysCacheService sysCacheService,
        SqlSugarRepository<WMSAdjustment> repAdjustment,
        UserManager userManager,
        WMSAdjustmentService wmsAdjustmentService)
    {
        _repInventoryUsable = inventoryRepo;
        _repCustomer = customerRepo;
        _repWarehouse = warehouseRepo;
        _repLocation = locationRepo;
        _repProduct = productRepo;
        _sysCacheService = sysCacheService;
        _userManager = userManager;
        _wmsAdjustmentService = wmsAdjustmentService;
        _repAdjustment = repAdjustment;
    }
    #endregion

    [HttpPost]
    [ApiDescriptionSettings(Name = "AdjustMoveCheckScan")]
    public async Task<WMSRFAdjustMoveResponse> AdjustMoveCheckScan(WMSRFAdjustMoveInput input)
    {
        var typeEnum = Enum.TryParse<AdjustmentTypeEnum>(input.Type, out var parsedType)
       ? parsedType
       : throw new ArgumentException($"无效的操作类型：{input.Type}");

        IWMSRFAdjustMoveInterface factory = WMSRFAdjustMoveFactory.AddMove(typeEnum);
        factory.Init(
            _repInventoryUsable,
            _repCustomer,
            _repWarehouse,
            _repLocation,
            _repProduct,
            _sysCacheService,
            _userManager,
           _wmsAdjustmentService,
           _repAdjustment
            );
        var response = await factory.CheckScanValue(input);
        return response;
    }


    [HttpPost]
    [ApiDescriptionSettings(Name = "AdjustMove")]
    public async Task<WMSRFAdjustMoveResponse> AdjustMove(WMSRFAdjustMoveCompleteInput input)
    {
        WMSRFAdjustMoveResponse response = new WMSRFAdjustMoveResponse();
        var typeEnum = Enum.TryParse<AdjustmentTypeEnum>(input.Type, out var parsedType)
       ? parsedType
       : throw new ArgumentException($"无效的操作类型：{input.Type}");

        IWMSRFAdjustMoveInterface factory = WMSRFAdjustMoveFactory.AddMove(typeEnum);
        factory.Init(
            _repInventoryUsable,
            _repCustomer,
            _repWarehouse,
            _repLocation,
            _repProduct,
            _sysCacheService,
            _userManager,
           _wmsAdjustmentService,
           _repAdjustment
            );

        List<long> Ids = new List<long>() { input.Id };
        
        response.response = await factory.CompleteAddjustmentMove(Ids);

        return response;
    }
}
