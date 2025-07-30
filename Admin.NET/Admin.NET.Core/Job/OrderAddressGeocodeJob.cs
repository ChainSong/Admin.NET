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
/// 

[JobDetail("OrderAddressGeocodeJob", Description = "高德API解析出库地址", GroupName = "default", Concurrent = false)]
[Period(24 * 60 * 60 * 1000, TriggerId = "OrderAddressGeocodeJob", Description = "高德API解析出库地址")]
public class OrderAddressGeocodeJob : IJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;
    private readonly SqlSugarRepository<WMSOrderAddressGaoDeMapping> _repOrderAddressGaoDeMapping;
    private readonly AMap _aMap;//高德API
    public OrderAddressGeocodeJob(IServiceProvider serviceProvider, SqlSugarRepository<WMSOrderAddress> repOrderAddress
        , SqlSugarRepository<WMSOrderAddressGaoDeMapping> repOrderAddressGaoDeMapping, AMap aMap)
    {
        _serviceProvider = serviceProvider;
        _repOrderAddress = repOrderAddress;
        _repOrderAddressGaoDeMapping = repOrderAddressGaoDeMapping;
        _aMap = aMap;
    }
    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        List<WMSOrderAddress> Output = new List<WMSOrderAddress>();
        WMSOrderAddress orderAddress= new WMSOrderAddress();
        var data = await GetNeedToConnectData();
        if (data != null && data.Count > 0)
        {
            foreach (var item in data)
            {
                var Georesponse = await _aMap.RequestGeoCode(item.Address, item.City);

                if (Georesponse?.geocodes == null || Georesponse.geocodes.Count == 0)
                    continue;

                var ModifiedAddress = item.Adapt<WMSOrderAddress>();
                ModifiedAddress.Province = Georesponse.geocodes[0].province;
                ModifiedAddress.City = Georesponse.geocodes[0].city;
                Output.Add(ModifiedAddress);

                await Task.Delay(3000); // 间隔3秒
            }

            if (Output!=null && Output.Count>0)
            {
                await UpdateOrderAddress(Output);
                await AddGaodeOrderAddress(Output);
            }
        }
    }
    /// <summary>
    /// 获取需要对接高德解析地址的数据
    /// </summary>
    /// <returns></returns>
    public async Task<List<WMSOrderAddress>> GetNeedToConnectData()
    {
        //获取已经对接过的
        var ConnectedIds = await _repOrderAddressGaoDeMapping.AsQueryable()
            .Select(a => a.OrderAddressId)
            .ToListAsync();
        //返回未对接的
        return await _repOrderAddress.AsQueryable()
             .Where(x => !ConnectedIds.Contains(x.Id))
             .ToListAsync();
    }
    /// <summary>
    /// 修改订单地址 
    /// </summary>
    /// <returns></returns>
    public async Task UpdateOrderAddress(List<WMSOrderAddress> input)
    {
        if (input == null || input.Count == 0)
            return;
        await _repOrderAddress.AsUpdateable(input)
       .UpdateColumns(x => new { x.Province, x.City })  
       .WhereColumns(x => x.Id)                      
       .ExecuteCommandAsync();
    }
    /// <summary>
    /// 新增高德地址关联表
    /// </summary>
    /// <returns></returns>
    public async Task AddGaodeOrderAddress(List<WMSOrderAddress> input)
    {
        if (input == null || input.Count == 0)
            return;
        List<WMSOrderAddressGaoDeMapping> List = new List<WMSOrderAddressGaoDeMapping>();

        foreach (var item in input)
        {
            List.Add(new WMSOrderAddressGaoDeMapping
            {
                OrderAddressId = item.Id,
                CompanyName = item.CompanyName,
                IsConnected = true
            });
        }
        await _repOrderAddressGaoDeMapping.InsertRangeAsync(List);
    }
}
