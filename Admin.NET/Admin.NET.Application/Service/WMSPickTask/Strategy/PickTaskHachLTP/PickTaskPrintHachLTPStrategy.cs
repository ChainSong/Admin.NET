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
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using FastExpressionCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;

namespace Admin.NET.Application.Service;
public class PickTaskPrintHachLTPStrategy : IPrintPickTaskInterface
{
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SysWorkFlowService _repWorkFlowService { get; set; }

    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }

    public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    public async Task<Response<PrintBase<List<WMSPickTaskOutput>>>> PickTaskPtint(List<long> ids)
    {


        Response<PrintBase<List<WMSPickTaskOutput>>> response = new Response<PrintBase<List<WMSPickTaskOutput>>>() { Data = new PrintBase<List<WMSPickTaskOutput>>() };
        //使用PrintShippingList类种的打印

        var entity = await _repPickTask.AsQueryable().Includes(a => a.Details).Where(u => ids.Contains(u.Id)).ToListAsync();
        response.Data.Data = new List<WMSPickTaskOutput>();
        response.Data.Data = entity.Adapt<List<WMSPickTaskOutput>>();
        //根据拣货单获取订单类型
        var orderEntity = await _repOrder.AsQueryable().Where(u => u.Id == response.Data.Data.First().Details.First().OrderId).ToListAsync();

        foreach (var item in response.Data.Data)
        {
            var order = await _repOrder.AsQueryable().Includes(a => a.Details).Where(a => a.OrderNumber == item.OrderNumber).FirstAsync();
            var orderadrress = await _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == item.Details.First().PreOrderNumber).FirstAsync();
            var product = await _repProduct.AsQueryable().Where(a => item.Details.Select(b => b.SKU).Contains(a.SKU) && a.CustomerId == item.CustomerId).ToListAsync();
            item.Details = item.Details.GroupBy(a => new { a.SKU, a.GoodsName, a.GoodsType, a.CustomerId, a.Area, a.Location, a.PoCode, a.BatchCode, a.PickTaskNumber, a.PickTaskId }).Select(a => new WMSPickTaskDetailOutput
            {
                SKU = a.Key.SKU,
                GoodsName = a.Key.GoodsName,
                GoodsType = a.Key.GoodsType,
                Area = a.Key.Area,
                Location = a.Key.Location,
                BatchCode = a.Key.BatchCode,
                PickTaskNumber = a.Key.PickTaskNumber,
                PickTaskId = a.Key.PickTaskId,
                Qty = a.Sum(b => b.Qty),
                IsSN = Convert.ToBoolean(product.Where(b => b.SKU == a.Key.SKU && b.CustomerId == a.Key.CustomerId).First().IsSN).ToString(),
                CN805 = Convert.ToBoolean(order.Details.Where(b => b.SKU == a.Key.SKU && b.PoCode == a.Key.PoCode && b.CustomerId == a.Key.CustomerId).First()?.PoCode.Contains("CN805")).ToString()
            }).OrderBy(a => a.Location).ToList();
            item.PrintTime = DateTime.Now;
            item.OrderAddress = orderadrress;
            item.Remark = order.Remark;
        }


        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        //throw new NotImplementedException();
        return response;
    }
}