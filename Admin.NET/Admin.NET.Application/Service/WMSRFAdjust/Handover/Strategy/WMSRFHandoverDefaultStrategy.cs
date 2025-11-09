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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service.WMSRFAdjust.Handover.Enumerate;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Factory;
using Furion.FriendlyException;
using Admin.NET.Application.Service.WMSRFAdjust.Move.Dto;
using Admin.NET.Core.Service;
using Microsoft.AspNetCore.Identity;
using SqlSugar;

namespace Admin.NET.Application;
public class WMSRFHandoverDefaultStrategy : IWMSRFHandoverInterface
{
    private SqlSugarRepository<WMSOrder> _repOrder;
    private SqlSugarRepository<WMSPackage> _repPackage;
    private SqlSugarRepository<WMSHandover> _repHandover;
    private SysCacheService _cacheService;
    private UserManager _userManager;
    private readonly TimeSpan timeSpan = TimeSpan.FromHours(72);
    public void Init(
        SqlSugarRepository<WMSOrder> repOrder,
        SqlSugarRepository<WMSPackage> repPackage,
        SqlSugarRepository<WMSHandover> repHandover,
        SysCacheService cacheService,
        UserManager userManager
      )
    {
        _repOrder = repOrder;
        _repPackage = repPackage;
        _repHandover = repHandover;
        _cacheService = cacheService;
        _userManager = userManager;
    }
    public WMSRFHandoverDefaultStrategy()
    {
    }

    #region 查询待交接的订单列表：已包装未出库的订单
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetPendingHandoverOrder")]
    public async Task<wMsRFPendingHandoverResponse> PendingHandoverOrder(wMsRFScanPackageInput input)
    {
        wMsRFPendingHandoverResponse response = new wMsRFPendingHandoverResponse();
        try
        {
            var list = await _repOrder.AsQueryable()
                .Where(a => a.OrderStatus == 60)
                .Where(a => SqlFunc.Subqueryable<WMSPackage>()
                .Where(p => p.PackageStatus == 99)
                .Where(p => p.ExternOrderNumber == a.ExternOrderNumber)
                .Where(p => p.OrderId == a.Id).Any())
                .WhereIF(!string.IsNullOrEmpty(input.ExternOrderNumber), a => a.ExternOrderNumber == input.ExternOrderNumber)
                .ToListAsync();
            response.orders = list.Adapt<List<wMsRFPendingHandoverOrder>>();
        }
        catch (Exception ex)
        {

            throw;
        }
        return response;
    }
    #endregion

    #region 扫描箱号
    public async Task<wMsRFPendingHandoverResponse> ScanPackageNumber(wMsRFScanPackageInput input)
    {
        wMsRFPendingHandoverResponse response = new wMsRFPendingHandoverResponse();
        if (string.IsNullOrEmpty(input.PackageNumber))
        {
            throw Oops.Oh("扫描的箱号不能为空");
        }

        #region 验证扫描的箱号
        var package = await _repPackage.AsQueryable()
            .Where(p => p.PackageNumber == input.PackageNumber)
            .Where(p => p.OrderId == input.OrderId)
            .Where(p => p.ExternOrderNumber == input.ExternOrderNumber)
            .Where(p => p.PackageStatus == 99)
            .FirstAsync();
        if (package == null)
        {
            throw Oops.Oh("查询箱号信息失败！请检查");
        }
        #endregion

        #region 缓存操作信息
        //如果操作单号为空则生成新的操作单号
        if (string.IsNullOrEmpty(input.OpSerialNumber))
            input.OpSerialNumber = $"{Guid.NewGuid():N}".Substring(0, 5) + DateTime.Now.ToString("MMddyyyy");

        var cacheKey = $"{_userManager.Account}_WMSRF_Handover_{input.OpSerialNumber}";

        #endregion
        // 获取缓存操作信息
        var packageList = _cacheService.Get<List<WMSPackage>>(cacheKey) ?? new List<WMSPackage>();
        // 检查是否已扫描过该箱号
        var existingPackage = packageList.FirstOrDefault(p => p.PackageNumber == input.PackageNumber);
        if (existingPackage != null)
        {
            throw Oops.Oh("该箱号已扫描，请勿重复扫描");
        }
        #region 新扫描 - 添加到List
        // 添加到列表
        packageList.Add(package);
        // 更新缓存
        UpdateCache(cacheKey, packageList);
        #endregion
        // 设置返回信息
        response.Result = "Success";
        response.Message = "扫描完成";
        response.SerialNumber = input.OpSerialNumber;
        var opInfoed = _cacheService.Get<List<WMSPackage>>(cacheKey);
        response.packages = opInfoed;
        return response;
    }
    #endregion

    #region 提交箱号
    public async Task<wMsRFPendingHandoverResponse> SubmitHandover(wMsRFSubmitHandoverInput input)
    {
        wMsRFPendingHandoverResponse response = new wMsRFPendingHandoverResponse();
        if (!input.palletInfo.Width.HasValue||!input.palletInfo.Length.HasValue||!input.palletInfo.height.HasValue 
            || !input.palletInfo.Volume.HasValue || !input.palletInfo.Weight.HasValue)
        {
            throw Oops.Oh("请填写完整的箱子尺寸和重量信息");
        }

        var cacheKey = $"{_userManager.Account}_WMSRF_Handover_{input.OpSerialNumber}";

        List<WMSHandover> wMSHandovers = new List<WMSHandover>();

        string PalletNumber = GeneratePalletNumber();

        foreach (var item in input.packages)
        {
            wMSHandovers.Add(new WMSHandover
            {
                OrderId= item.OrderId,
                PackageId = item.Id,
                PalletNumber = PalletNumber,
                HandoverNumber = $"HO{DateTime.Now:yyyyMMddHHmmssfff}",
                PickTaskId = item.PickTaskId,
                PickTaskNumber = item.PickTaskNumber,
                PackageNumber=item.PackageNumber,
                PreOrderNumber=item.PreOrderNumber,
                OrderNumber = item.OrderNumber,
                ExternOrderNumber = item.ExternOrderNumber,
                CustomerId = item.CustomerId,
                CustomerName = item.CustomerName,
                WarehouseId = item.WarehouseId,
                WarehouseName = item.WarehouseName,
                HandoverType= "RF交接",
                Length=input.palletInfo.Length,
                Width = input.palletInfo.Width,
                Volume = input.palletInfo.Volume,
                Height = input.palletInfo.height,
                NetWeight = input.palletInfo.Weight,
                GrossWeight = input.palletInfo.Weight,
                ExpressNumber = item.ExpressNumber,
                ExpressCompany = item.ExpressCompany,
                SerialNumber = item.SerialNumber,
                IsHandovered=1,
                Handoveror=_userManager.RealName,
                HandoverTime= DateTime.Now,
                HandoverStatus=1,
                DetailCount=item.DetailCount,
                Creator=_userManager.RealName,
                CreationTime=DateTime.Now,
                TenantId= 1300000000001
            });
        }

        var result = await _repHandover.InsertRangeAsync(wMSHandovers);
        var upOrder= await _repOrder.AsUpdateable()
            .SetColumns(a => new WMSOrder
            {
                OrderStatus = 80
            })
            .Where(a => a.Id == input.packages.First().OrderId)
            .ExecuteCommandAsync();
        if (result)
        {
            // 新增移库单成功之后就清除缓存
            _cacheService.Remove(cacheKey);
            response.Result = "Success";
            response.Message = "绑托完成";
            response.SerialNumber = input.OpSerialNumber;
        }
        else
        {
            response.Result = "Faild";
            response.Message = "绑托失败";
            response.SerialNumber = input.OpSerialNumber;
        }
       
        return response;
    }
    #endregion
    #region
    public string GeneratePalletNumber()
    {
        var GUId = $"{Guid.NewGuid():N}".Substring(0, 8);
        return $"PL{DateTime.Now:yyyyMMddHHmmssfff_}_{GUId}";
    }
    #region 获取已扫描箱号列表
    public List<WMSPackage> GetScannedPackages(string opSerialNumber)
    {
        var cacheKey = $"{_userManager.Account}_WMSRFHandover_{opSerialNumber}";
        return _cacheService.Get<List<WMSPackage>>(cacheKey) ?? new List<WMSPackage>();
    }
    #endregion
    private void UpdateCache(string cacheKey, List<WMSPackage> output)
    {
        _cacheService.Set(cacheKey, output, timeSpan);
    }
 

    #region 清空扫描缓存
    public void ClearScanCache(string opSerialNumber)
    {
        var cacheKey = $"{_userManager.Account}_WMSRFHandover_{opSerialNumber}";
        _cacheService.Remove(cacheKey);
    }
    #endregion
    #endregion
}
