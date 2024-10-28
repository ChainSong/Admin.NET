// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class StockCheckQueryByDynamicLocationStrategy : IStockCheckInterface
{
    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }

    public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
    public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
    public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }
    public async Task<Response<List<WMSInventoryUsable>>> GetStockInfo(WMSStockCheckInput input)
    {

        List<string> Locations = new List<string>();
        //查询完成时间为今天的入库库位
        //Locations=await _repReceiptReceiving.AsQueryable().Where(a => a.ReceiptDate == DateTime.Now.Date).Select(a => a.Location).Distinct().ToListAsync();

        Response<List<WMSInventoryUsable>> response = new Response<List<WMSInventoryUsable>>() { Data = new List<WMSInventoryUsable>() };
        var inventoryUsable = await _repInventoryUsable.AsQueryable().Where(a =>
                     a.CustomerId == input.CustomerId &&
                     a.WarehouseId == input.WarehouseId &&
                     a.CreationTime >= DateTime.Now
                     )
                     .GroupBy(a => new
                     {
                         a.Location,
                         a.Area,
                         a.SKU,
                         a.BatchCode,
                         a.BoxCode,
                         a.PoCode,
                         a.ExpirationDate,
                         a.ProductionDate,
                     })
                    .Select(a => new WMSInventoryUsable()
                    {
                        CustomerId = SqlFunc.AggregateMin(a.CustomerId),
                        CustomerName = SqlFunc.AggregateMin(a.CustomerName),
                        Area = a.Area,
                        Location = a.Location,
                        SKU = a.SKU,
                        BatchCode = a.BatchCode,
                        BoxCode = a.BoxCode,
                        PoCode = a.PoCode,
                        ExpirationDate = a.ExpirationDate,
                        ProductionDate = a.ProductionDate,
                        WarehouseId = SqlFunc.AggregateMin(a.WarehouseId),
                        WarehouseName = SqlFunc.AggregateMin(a.WarehouseName),
                        Qty = SqlFunc.AggregateSum(a.Qty),
                    })
                    .OrderBy(a => a.Location).ToListAsync();


        //await receipt.ForEachAsync(a =>
        //{
        //    response.Data.Add(new OrderStatusDto()
        //    {
        //        ExternOrder = a.ExternReceiptNumber,
        //        SystemOrder = a.ASNNumber,
        //        Type = a.ReceiptType,
        //        StatusCode = StatusCode.Success,
        //        Msg = "操作成功"
        //    });
        //});
        response.Data = inventoryUsable;
        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        //throw new NotImplementedException();
        return response;

    }


}
