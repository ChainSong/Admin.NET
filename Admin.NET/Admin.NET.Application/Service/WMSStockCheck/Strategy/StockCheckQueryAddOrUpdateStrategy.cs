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
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class StockCheckQueryAddOrUpdateStrategy : IStockCheckAddOrUpdateInterface
{
    public SqlSugarRepository<WMSStockCheck> _rep { get; set; }
    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }

    public SqlSugarRepository<WMSStockCheckDetail> _repStockCheckDetail { get; set; }
    public SqlSugarRepository<WMSStockCheckDetailScan> _repStockCheckDetailScan { get; set; }

    public UserManager _userManager { get; set; }

    public async Task<Response<List<WMSInventoryUsable>>> AddOrUpdate(WMSStockCheckInput input)
    {
        Response<List<WMSInventoryUsable>> response = new Response<List<WMSInventoryUsable>>() { Data = new List<WMSInventoryUsable>() };

        var entity = input.Adapt<WMSStockCheck>();

        var StockCheckNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
        entity.StockCheckNumber = StockCheckNumber;

        entity.Creator = _userManager.Account;
        entity.StockCheckStatus = (int)StockCheckStatusEnum.新增;
        entity.CreationTime = DateTime.Now;

        entity.Details.ForEach(x =>
        {
            x.StockCheckNumber = StockCheckNumber;
            x.CustomerId = entity.CustomerId;
            x.CustomerName = entity.CustomerName;
            x.WarehouseId = entity.WarehouseId;
            x.WarehouseName = entity.WarehouseName;
            x.ExternNumber = entity.ExternNumber;
            x.Creator = _userManager.Account;
            x.CreationTime = DateTime.Now;
        });
        //await _rep.AsQueryable().Includes(a=>a.Details).(entity);
        await _rep.Context.InsertNav(entity).Include(a => a.Details).ExecuteCommandAsync();

        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        //throw new NotImplementedException();
        return response;

    }


}
