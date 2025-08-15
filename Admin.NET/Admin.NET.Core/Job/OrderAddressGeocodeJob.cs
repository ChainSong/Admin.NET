// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Common.AMap;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Mapster;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Job;

/// <summary>
/// 通过高德API解析出库地址JOB
/// </summary>

[JobDetail("OrderAddressGeocodeJob", Description = "高德API解析出库地址", GroupName = "default", Concurrent = false)]
//[Period(24 * 60 * 60 * 1000, TriggerId = "OrderAddressGeocodeJob", Description = "高德API解析出库地址")]
[Daily(TriggerId = "OrderAddressGeocodeJob", Description = "高德API解析出库地址")]
public class OrderAddressGeocodeJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    public OrderAddressGeocodeJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();

            var orderAddressRepo = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<WMSOrderAddress>>();
            var mappingRepo = scope.ServiceProvider.GetRequiredService<SqlSugarRepository<WMSOrderAddressGaoDeMapping>>();
            var userManager = scope.ServiceProvider.GetService<UserManager>();
            var aMap = new AMap();

            var pendingList = await GetPendingAddresses(orderAddressRepo, mappingRepo);
            if (pendingList == null || pendingList.Count == 0) return;

            var updatedList = new List<WMSOrderAddress>();
            var mappingList = new List<WMSOrderAddressGaoDeMapping>();

            foreach (var item in pendingList)
            {

                if (string.IsNullOrEmpty(item.Address)) continue;
                var geo = await aMap.RequestGeoCode(item.Address, item.City);
                if (geo!=null && geo.status=="0")
                {
                    continue;
                }
                var geoPOI = await aMap.RequestGeoCodePOI(item.Address);
                var geocode = geo?.geocodes?.FirstOrDefault();
                var geocodePOI = geoPOI?.Pois?.FirstOrDefault();

                if (geocode == null) continue;

                item.Province = geocode.province;
                item.City = geocode.city;
                item.CompanyName = geocodePOI?.Name;
                await UpdateAddressInfo(orderAddressRepo, item);
                var mapping = new WMSOrderAddressGaoDeMapping
                {
                    OrderAddressId = item.Id,
                    CompanyName = item.CompanyName,
                    IsConnected = true,
                    TenantId = 1300000000001,
                };
                await mappingRepo.InsertAsync(mapping);
                await Task.Delay(3000); // 间隔 3 秒
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task<List<WMSOrderAddress>> GetPendingAddresses(
       SqlSugarRepository<WMSOrderAddress> orderRepo,
       SqlSugarRepository<WMSOrderAddressGaoDeMapping> mapRepo)
    {
        var connectedIds = await mapRepo.AsQueryable()
            .Select(x => x.OrderAddressId)
            .ToListAsync();
        return await orderRepo.AsQueryable()
            .Where(x => !connectedIds.Contains(x.Id))
            .ToListAsync();
    }
 
    private async Task UpdateAddressInfo(SqlSugarRepository<WMSOrderAddress> repo, WMSOrderAddress info)
    {
        await repo.AsUpdateable(info)
            .UpdateColumns(x => new { x.Province, x.City })
            .WhereColumns(x => x.Id)
            .ExecuteCommandAsync();
    }
}
