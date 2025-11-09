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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;

namespace Admin.NET.Application.Service;
public class LocationAddOrUpdateStrategy : ILocationInterface
{


    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }
    public SqlSugarRepository<WMSArea> _repArea { get; set; }
    public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
    public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

    public UserManager _userManager { get; set; }

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public LocationAddOrUpdateStrategy(
    )
    {
    }

    //默认方法不做任何处理
    public Task<Response<List<OrderStatusDto>>> Export(List<WMSLocationImportExport> request)
    {


        return Task.FromResult(new Response<List<OrderStatusDto>>());
    }

    public async Task<Response<List<OrderStatusDto>>> Import(List<WMSLocationImportExport> request)
    {
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();
        response.Data = new List<OrderStatusDto>();
        var entity = request.Adapt<List<WMSLocation>>();
        //根据导入的库位信息获取仓库信息
        var warehouse = await _repWarehouse.AsQueryable().Where(a => a.WarehouseName == entity.First().WarehouseName).ToListAsync();
        if (warehouse == null || warehouse.Count() == 0)
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "仓库不存在" };
        }
        //entity.WarehouseName = warehouse.First().WarehouseName;
        //entity.WarehouseId = warehouse.First().Id;
        //根据导入的库位信息获取库区信息
        var area = await _repArea.AsQueryable().Where(a => a.AreaName == entity.First().AreaName).ToListAsync();
        if (area == null || area.Count() == 0)
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "库区不存在" };
        }
        entity.ForEach(a =>
        {
            a.WarehouseName = warehouse.First().WarehouseName;
            a.WarehouseId = warehouse.First().Id;
            a.AreaName = area.First().AreaName;
            a.AreaId = area.First().Id;
            a.LocationType = "1";
            a.LocationStatus = 1;
        });
        //entity.AreaName = area.First().AreaName;
        //entity.AreaId = area.First().Id;
        //根据导入的库位信息获取库位类型信息
        var data =  _repLocation.AsQueryable().Where(a => request.Select(b => b.Location).Contains(a.Location) && a.WarehouseName == entity.First().WarehouseName);
        if (data.Count() > 0)
        {
            data.ToList().ForEach(a =>
            {

                response.Data.Add(new OrderStatusDto() { SystemOrder = a.Location, ExternOrder= a.Location, StatusCode = StatusCode.Error, Msg = "库位已经存在" });
            });
            response.Code = StatusCode.Error;
            response.Msg = "库位已经存在";
            return response;

        }
        else
        {
            await _repLocation.InsertRangeAsync(entity);
            response.Code = StatusCode.Success;
            response.Msg = "导入成功";
            return response;
        }
    }
}
