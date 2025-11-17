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
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Const;
using Admin.NET.Application.Service.WMSRFAdjust.Factory;   
using Admin.NET.Application.Service.WMSRFAdjust.Interface;
using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Identity;
using Admin.NET.Application.Service.WMSRFHandover.Interface;
using Admin.NET.Application.Service.WMSRFHandover.Factory;
using Admin.NET.Application.Service.WMSRFHandover.Dto;
using Admin.NET.Application.Service.WMSRFHandover.Enumerate;

namespace Admin.NET.Application.Service.WMSRFHandover;
/// <summary>
/// RFHandover服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 102)]
public class WMSRFHandoverService : IDynamicApiController, ITransient
{
    #region 依赖注入
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<WMSHandover> _repHandover;
    private readonly SysCacheService _cacheService;
    private readonly UserManager _userManager;

    public WMSRFHandoverService(SqlSugarRepository<WMSOrder> repOrder,
        SqlSugarRepository<WMSPackage> repPackage,
        SqlSugarRepository<WMSHandover> repHandover,
        SysCacheService cacheService,
        UserManager userManager)
    {
        _repOrder = repOrder;
        _repPackage = repPackage;
        _repHandover = repHandover;
        _cacheService = cacheService;
        _userManager = userManager;
    }
    #endregion

    [HttpPost]
    [ApiDescriptionSettings(Name = "GetPendingHandoverOrder")]
    #region 查询待交接的订单列表：已包装未出库的订单
    public async Task<wMsRFPendingHandoverResponse> PendingHandoverOrder(wMsRFScanPackageInput input)
    {
        var typeEnum = Enum.TryParse<HandoverEnum>(input.Type, out var parsedType)
           ? parsedType
           : throw new ArgumentException($"无效的操作类型：{input.Type}");
        IWMSRFHandoverInterface factory = WMSRFHandoverFactory.AddHandover(typeEnum);
        factory.Init(
            _repOrder,
            _repPackage,
            _repHandover,
            _cacheService,
            _userManager);
        var response = await factory.PendingHandoverOrder(input);
        return response;
    }
    #endregion

    [HttpPost]
    [ApiDescriptionSettings(Name = "ScanPackageNumber")]
    #region 扫描箱号
    public async Task<wMsRFPendingHandoverResponse> ScanPackageNumber(wMsRFScanPackageInput input)
    {
        var typeEnum = Enum.TryParse<HandoverEnum>(input.Type, out var parsedType)
           ? parsedType
           : throw new ArgumentException($"无效的操作类型：{input.Type}");
        IWMSRFHandoverInterface factory = WMSRFHandoverFactory.AddHandover(typeEnum);
        factory.Init(
            _repOrder,
            _repPackage,
            _repHandover,
            _cacheService,
            _userManager);
        var response = await factory.ScanPackageNumber(input);
        return response;
    }
    #endregion


    [HttpPost]
    [ApiDescriptionSettings(Name = "SubmitHandover")]
    #region 扫描箱号
    public async Task<wMsRFPendingHandoverResponse> SubmitHandover(wMsRFSubmitHandoverInput input)
    {
        var typeEnum = Enum.TryParse<HandoverEnum>(input.Type, out var parsedType)
           ? parsedType
           : throw new ArgumentException($"无效的操作类型：{input.Type}");
        IWMSRFHandoverInterface factory = WMSRFHandoverFactory.AddHandover(typeEnum);
        factory.Init(
            _repOrder,
            _repPackage,
            _repHandover,
            _cacheService,
            _userManager);
        var response = await factory.SubmitHandover(input);
        return response;
    }
    #endregion
}
