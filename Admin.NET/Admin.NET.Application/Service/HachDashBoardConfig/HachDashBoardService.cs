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
using Admin.NET.Application.Service.HachDashBoardConfig.Dto;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Org.BouncyCastle.Asn1.X509;
using ServiceStack;

namespace Admin.NET.Application.Service.HachDashBoardConfig;

/// <summary>
/// HACH大屏
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1)]
public class HachDashBoardService : IDynamicApiController, ITransient
{

    #region 注入依赖
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSHachAccountDate> _repHachAccountDate;
    private readonly SqlSugarRepository<WMSHachTagretKRMB> _repHachTagretKRMB;
    private readonly SqlSugarRepository<WMSInventoryUsableSnapshot> _repInventoryUsableSnapshot;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;

    public HachDashBoardService(UserManager userManager,
        SqlSugarRepository<WMSHachAccountDate> repHachAccountDate,
        SqlSugarRepository<WMSInventoryUsableSnapshot> repInventoryUsableSnapshot,
        SqlSugarRepository<WMSHachTagretKRMB> repHachTagretKRMB,
        SqlSugarRepository<WMSProduct> repProduct,
        SqlSugarRepository<WMSCustomer> repCustomer,
        SqlSugarRepository<WMSWarehouse> repWarehouse,
        SqlSugarRepository<WMSOrder> repOrder,
        SqlSugarRepository<WMSOrderDetail> repOrderDetail,
        SqlSugarRepository<WMSASNDetail> repASNDetail
        )
    {
        _userManager = userManager;
        _repHachAccountDate = repHachAccountDate;
        _repInventoryUsableSnapshot = repInventoryUsableSnapshot;
        _repHachTagretKRMB = repHachTagretKRMB;
        _repProduct = repProduct;
        _repCustomer = repCustomer;
        _repWarehouse = repWarehouse;
        _repOrder = repOrder;
        _repOrderDetail = repOrderDetail;
        _repASNDetail = repASNDetail;
    }
    #endregion

    /// <summary>
    /// 汇总Tab项数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<SumItemOutput> GetSumItemData(WMSReceiptInput input)
    {
        SumItemOutput itemOutput = new SumItemOutput();
        var today = DateTime.Today;
        var (firstDayOfMonth, lastDayOfMonth) = GetMonthDateRange(today);
        var currentMonthString = today.ToString("yyyy-MM");

        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();

        // 并行获取数据
        var tasks = new List<Task<long>>
    {
        GetLastMonthAmount(today, priceMap),
        GetCurrentMonthAmount(firstDayOfMonth, today, priceMap),
        GetCurrentTargetAmount(currentMonthString),
        GetYTDOrderVSASNAmount()
    };

        // 等待所有任务完成
        var results = await Task.WhenAll(tasks);

        itemOutput.LastMonthAmount = results[0];
        itemOutput.CurrentMonthAmount = results[1];
        itemOutput.CurrentTargetAmount = results[2];
        itemOutput.YTDOrderVSASNAmount = results[3];

        // 计算差值
        itemOutput.CMonthVSTargetAmount = itemOutput.CurrentMonthAmount - itemOutput.CurrentTargetAmount;

        // 获取当月入库和出库金额
        itemOutput.CurrentReceiptAmount = Convert.ToInt64(await GetCurrentReceiptAmount(firstDayOfMonth, lastDayOfMonth));
        itemOutput.CurrentOrderAmount = Convert.ToInt64(await GetCurrentOrerAmount(firstDayOfMonth, lastDayOfMonth));

        return itemOutput;
    }

    #region 封装的公共方法
    /// <summary>
    /// 获取上月总金额
    /// </summary>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetLastMonthAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var lastAccountDate = await _repHachAccountDate.AsQueryable()
            .Where(x => x.StartDate.HasValue &&
                        x.StartDate.Value.Year == today.Year &&
                        x.StartDate.Value.Month == today.Month)
            .FirstAsync();

        if (lastAccountDate != null)
        {
            var lastMonthInventory = await _repInventoryUsableSnapshot.AsQueryable()
                .Where(a => a.InventoryTime >= lastAccountDate.StartDate && a.InventoryTime <= lastAccountDate.EndDate)
                .GroupBy(a => a.SKU)
                .Select(a => new { a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
                .ToListAsync();

            return CalculateTotalAmount(
                lastMonthInventory.Select(x => (x.SKU, (double)x.Qty)),
                priceMap
            );
        }
        return 0;
    }

    /// <summary>
    /// 获取当月总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetCurrentMonthAmount(DateTime firstDayOfMonth, DateTime today, Dictionary<string, double> priceMap)
    {
        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= firstDayOfMonth && a.InventoryTime <= today)
            .GroupBy(a => a.SKU)
            .Select(a => new { a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        return CalculateTotalAmount(
            currentInventory.Select(x => (x.SKU, (double)x.Qty)),
            priceMap
        );
    }

    /// <summary>
    /// 获取当月库存目标金额
    /// </summary>
    /// <param name="currentMonthString"></param>
    /// <returns></returns>
    private async Task<long> GetCurrentTargetAmount(string currentMonthString)
    {
        var target = await _repHachTagretKRMB.AsQueryable()
            .Where(a => a.Month == currentMonthString)
            .GroupBy(a => 1)
            .Select(a => new { PlanKRMB = SqlFunc.AggregateSum(a.PlanKRMB) })
            .FirstAsync();
        return Convert.ToInt64(target?.PlanKRMB ?? 0);
    }

    /// <summary>
    /// 获取YTD ORDER VS YTD ASN
    /// </summary>
    /// <returns></returns>
    private async Task<long> GetYTDOrderVSASNAmount()
    {
        var totalOrderQty = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId && o.OrderStatus == 99)
                        .Any())
            .SumAsync(d => d.AllocatedQty);

        var totalASNQty = await _repASNDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSASN>()
                        .Where(o => o.Id == d.ASNId && o.ASNStatus != 90)
                        .Any())
            .SumAsync(d => d.ExpectedQty);

        return totalASNQty == 0
            ? 0
            : Convert.ToInt64(totalOrderQty / totalASNQty);
    }

    /// <summary>
    /// 获取当月入库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double> GetCurrentReceiptAmount(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
    {
        return await _repASNDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSASN>()
                        .Where(o => o.Id == d.ASNId &&
                                    o.ASNStatus != 90 &&
                                    o.CreationTime >= firstDayOfMonth &&
                                    o.CreationTime <= lastDayOfMonth)
                        .Any())
            .SumAsync(d => d.ExpectedQty);
    }

    /// <summary>
    /// 获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double> GetCurrentOrerAmount(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
    {
        return await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId &&
                                    o.OrderStatus == 99 &&
                                    o.CreationTime >= firstDayOfMonth &&
                                    o.CreationTime <= lastDayOfMonth)
                        .Any())
            .SumAsync(d => d.AllocatedQty);
    }

    /// <summary>
    /// 获取 SKU -> Price 的映射字典
    /// </summary>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetProductPriceMap()
    {
        var products = await _repProduct.AsQueryable()
            .Select(p => new { p.SKU, p.Price })
            .ToListAsync();

        return products.ToDictionary(p => p.SKU, p => p.Price);
    }

    /// <summary>
    /// 通过库存数量和价格表计算总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private long CalculateTotalAmount(IEnumerable<(string SKU, double Qty)> data, Dictionary<string, double> priceMap)
    {
        double total = 0;
        foreach (var item in data)
        {
            if (priceMap.TryGetValue(item.SKU, out var price))
            {
                total += item.Qty * price;
            }
        }
        return Convert.ToInt64(total);
    }
    /// <summary>
    /// 公共方法：获取月份的开始和结束日期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private (DateTime FirstDay, DateTime LastDay) GetMonthDateRange(DateTime date)
    {
        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        return (firstDayOfMonth, lastDayOfMonth);
    }

    #endregion

}
