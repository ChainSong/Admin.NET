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
              //上个月的库存金额
              GetInventoryUsableSnapshotByAccountDate(today.AddMonths(-1), priceMap),
              //这天的库存金额
              GetInventoryUsableByToday(today, priceMap),
              //获取当月库存目标金额
              GetTargetAmountByMonth(currentMonthString),
              //获取YTD ORDER VS YTD ASN
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
        itemOutput.CurrentReceiptAmount = Convert.ToInt64(await GetCurrentReceiptAmount(today, priceMap));
        itemOutput.CurrentOrderAmount = Convert.ToInt64(await GetCurrentOrderAmount(today, priceMap));
        return itemOutput;
    }

    #region 屏一
    /// <summary>
    /// 月库存金额趋势图VS去年
    /// </summary>
    /// <returns></returns>
    /// 当月没到关账日不展示数据
    public async Task<MonthVSLastOutput> GetMonthVSLast(ChartsInput input)
    {
        MonthVSLastOutput monthVSLastOutput = new MonthVSLastOutput();

        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();

        #region 获取去年的月份数据
        //获取去年所有月份的账期
        var ALLLastMonth = Enumerable.Range(1, 12) // 生成1-12月
        .Select(month => new DateTime(DateTime.Today.Year - 1, month, 1))
        .ToList();
        Dictionary<long, long> LastYear = new Dictionary<long, long>();
        foreach (var item in ALLLastMonth)
        {
            //获取目标月份账期起始日，结束日
            var TargetMonth = await GetAccountByDate(item);
            var LastData = await GetInventoryUsableSnapshotByAccountDate(TargetMonth.StartDate.GetValueOrDefault(), priceMap);
            LastYear.Add(TargetMonth.Month.GetValueOrDefault(), LastData);
        }
        #endregion

        #region 获取今年的月份数据
        //获取去年所有月份的账期
        var ALLCurrentMonth = Enumerable.Range(1, 12) // 生成1-12月
        .Select(month => new DateTime(DateTime.Today.Year - 1, month, 1))
        .ToList();
        Dictionary<long, long> CurrentYear = new Dictionary<long, long>();
        foreach (var item in ALLCurrentMonth)
        {
            //获取目标月份账期起始日，结束日
            var TargetMonth = await GetAccountByDate(item);
            var CurrentData = await GetInventoryUsableSnapshotByAccountDate(TargetMonth.StartDate.GetValueOrDefault(), priceMap);
            CurrentYear.Add(TargetMonth.Month.GetValueOrDefault(), CurrentData);
        }
        #endregion

        monthVSLastOutput.LastYear = LastYear;
        monthVSLastOutput.CurrentYear = CurrentYear;

        return monthVSLastOutput;
    }

    /// <summary>
    /// 及时库存
    /// </summary>
    /// <returns></returns>
    public async Task<MonthVSLastOutput> GetProductMinorPrice(ChartsInput input)
    {
        MonthVSLastOutput monthVSLastOutput = new MonthVSLastOutput();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        var data = await GetInventoryUsableGroupBySkuByToday(DateTime.Today, priceMap);
        monthVSLastOutput.TodayPriceOfInventory = data;
        return monthVSLastOutput;
    }

    /// <summary>
    /// 当月库存出库产品总金额
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<MonthVSLastOutput> GetObPriceByMonth(ChartsInput input)
    {
        MonthVSLastOutput monthVSLastOutput = new MonthVSLastOutput();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        var data = await GetCurrentOrderGroupBySKUByAmount(DateTime.Today, priceMap);
        monthVSLastOutput.TodayPriceOfInventory = data;
        return monthVSLastOutput;
    }

    /// <summary>
    /// 库销比
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<MonthVSLastOutput> GetInventoryToSellPercent(ChartsInput input)
    {
        MonthVSLastOutput monthVSLastOutput = new MonthVSLastOutput();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        var receiptData = await GetReceiptGroupByDateAmount(DateTime.Today, priceMap);
        var OrderData = await GetOrderGroupByDateAmount(DateTime.Today, priceMap);

        // 创建字典来存储每天的比率
        Dictionary<DateTime, string> dailyPercentages = new Dictionary<DateTime, string>();

        // 遍历所有日期，并计算入库与出库的比率
        foreach (var receipt in receiptData)
        {
            // 获取当天的入库金额
            var receiptAmount = receipt.Value;

            // 获取当天的出库金额
            if (OrderData.ContainsKey(receipt.Key))
            {
                var orderAmount = OrderData[receipt.Key];

                // 如果出库金额为零，避免除以零
                if (orderAmount == 0)
                {
                    dailyPercentages[receipt.Key] = "0%";
                }
                else
                {
                    // 计算入库与出库的比率并转换为百分比
                    var percentage = (receiptAmount / (double)orderAmount) * 100;

                    // 格式化为 2 位小数的百分比
                    dailyPercentages[receipt.Key] = $"{percentage:F2}%";
                }
            }
            else
            {
                // 如果出库数据不存在
                dailyPercentages[receipt.Key] = "0%";
            }
        }

        // 将日比率数据存储到返回对象
        monthVSLastOutput.CurrentMonthPriceOfOB = dailyPercentages;

        return monthVSLastOutput;
    }
    #endregion

    #region 屏二
    #endregion

    #region 屏三
    #endregion

    #region 封装的公共方法

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

    /// <summary>
    /// 获取月份 获取 库存快照表数据
    /// </summary>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetInventoryUsableSnapshotByAccountDate(DateTime today, Dictionary<string, double> priceMap)
    {
        var lastAccountDate = await GetAccountByDate(today);

        // 如果查询不到记录或EndDate大于今天，直接返回0
        if (lastAccountDate == null ||
         (lastAccountDate.EndDate.HasValue && lastAccountDate.EndDate.Value > today))
        {
            return 0;
        }

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

    /// <summary>
    /// 获取月份 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetInventoryUsableByAmountDate(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);

        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= AccountDate.StartDate && a.InventoryTime <= AccountDate.EndDate)
            .GroupBy(a => a.SKU)
            .Select(a => new { a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        return CalculateTotalAmount(
            currentInventory.Select(x => (x.SKU, (double)x.Qty)),
            priceMap
        );
    }
    /// <summary>
    /// 获取当天 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetInventoryUsableByToday(DateTime today, Dictionary<string, double> priceMap)
    {
        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= today && a.InventoryTime <= today)
            .GroupBy(a => a.SKU)
            .Select(a => new { a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        //获取所有总金额
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
    private async Task<long> GetTargetAmountByMonth(string currentMonthString)
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
    private async Task<double> GetCurrentReceiptAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);

        var data = await _repASNDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSASN>()
                        .Where(o => o.Id == d.ASNId &&
                                    o.ASNStatus != 90 &&
                                    o.CreationTime >= AccountDate.StartDate &&
                                    o.CreationTime <= AccountDate.EndDate)
                        .Any())
            .ToListAsync();

        return CalculateTotalAmount(
         data.Select(x => (x.SKU, (double)x.ExpectedQty)),
         priceMap
     );
    }

    /// <summary>
    /// 获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double> GetCurrentOrderAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);
        var data = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId &&
                                    o.OrderStatus == 99 &&
                                    o.CreationTime >= AccountDate.StartDate &&
                                    o.CreationTime <= AccountDate.EndDate)
                        .Any())
            .ToListAsync();

        return CalculateTotalAmount(
      data.Select(x => (x.SKU, (double)x.AllocatedQty)),
      priceMap
  );
    }

    /// <summary>
    /// 根据sku获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetCurrentOrderGroupBySKUByAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);
        // 2. 查询当月出库数据（按SKU分组）
        var skuData = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                      .Where(o => o.Id == d.OrderId &&
                                  o.OrderStatus == 99 &&
                                  o.CreationTime >= AccountDate.StartDate &&
                                  o.CreationTime <= AccountDate.EndDate)
                      .Any())
            .GroupBy(d => d.SKU)
            .Select(d => new
            {
                SKU = d.SKU,
                TotalQty = SqlFunc.AggregateSum(d.AllocatedQty)
            })
            .ToListAsync();

        // 3. 计算各SKU金额
        var skuAmounts = new Dictionary<string, double>();

        foreach (var item in skuData)
        {
            if (priceMap.TryGetValue(item.SKU, out double price))
            {
                double amount = item.TotalQty * price;
                skuAmounts.Add(item.SKU, amount);
            }
            else
            {
                // 记录缺失价格的SKU
                skuAmounts.Add(item.SKU, 0);
            }
        }
        return skuAmounts;
    }
 
    /// 获取当月每天的入库总金额
    /// </summary>
    /// <param name="today">当前日期</param>
    /// <param name="priceMap">商品价格字典</param>
    /// <returns>按天汇总的入库总金额</returns>
    private async Task<Dictionary<DateTime, long>> GetReceiptGroupByDateAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);
        Dictionary<DateTime, long> dailyAmount = new Dictionary<DateTime, long>();
        if (AccountDate == null) 
            return dailyAmount;
        // 遍历当月的每一天
        for (var date =Convert.ToDateTime(AccountDate.StartDate); date <= AccountDate.EndDate; date =Convert.ToDateTime(AccountDate.StartDate).AddDays(1))
        {
            // 获取当天的入库数据
            var todayData = await _repASNDetail.AsQueryable()
                .Where(d => SqlFunc.Subqueryable<WMSASN>()
                            .Where(o => o.Id == d.ASNId &&
                                        o.ASNStatus != 90 &&
                                        o.CreationTime >= date.Date && // 精确到当天
                                        o.CreationTime < date.Date.AddDays(1)) // 精确到当天
                            .Any())
                .ToListAsync();

            // 计算当天的总金额
            var totalAmount = CalculateTotalAmount(
                todayData.Select(x => (x.SKU, (double)x.ExpectedQty)),
                priceMap
            );

            // 将每天的入库总金额加入字典
            dailyAmount[date] = totalAmount;
        }
        return dailyAmount;
    }

    /// 获取当月每天的出库总金额
    /// </summary>
    /// <param name="today">当前日期</param>
    /// <param name="priceMap">商品价格字典</param>
    /// <returns>按天汇总的入库总金额</returns>
    private async Task<Dictionary<DateTime, long>> GetOrderGroupByDateAmount(DateTime today, Dictionary<string, double> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);
        Dictionary<DateTime, long> dailyAmount = new Dictionary<DateTime, long>();
        if (AccountDate == null)
            return dailyAmount;
        // 遍历当月的每一天
        for (var date = Convert.ToDateTime(AccountDate.StartDate); date <= AccountDate.EndDate; date = Convert.ToDateTime(AccountDate.StartDate).AddDays(1))
        {
            // 获取当天的入库数据
            var todayData = await _repOrderDetail.AsQueryable()
                .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                            .Where(o => o.Id == d.OrderId &&
                                        o.OrderStatus == 99 &&
                                        o.CreationTime >= date.Date && // 精确到当天
                                        o.CreationTime < date.Date.AddDays(1)) // 精确到当天
                            .Any())
                .ToListAsync();

            // 计算当天的总金额
            var totalAmount = CalculateTotalAmount(
                todayData.Select(x => (x.SKU, (double)x.AllocatedQty)),
                priceMap
            );
            // 将每天的入库总金额加入字典
            dailyAmount[date] = totalAmount;
        }
        return dailyAmount;
    }
    /// <summary>
    /// 获取今年已经过去的所有月份（不包括当月）
    /// </summary>
    /// <returns></returns>
    public List<string> GetPastMonthsExcludingCurrent()
    {
        var today = DateTime.Today;
        var currentYear = today.Year;
        var currentMonth = today.Month;

        var pastMonths = new List<string>();

        // 从1月开始到上个月结束
        for (int month = 1; month < currentMonth; month++)
        {
            // 格式化为"yyyy-MM"（例如：2025-01）
            var monthStr = new DateTime(currentYear, month, 1).ToString("yyyy-MM");
            pastMonths.Add(monthStr);
        }

        return pastMonths;
    }

    /// <summary>
    /// 获取指定年份已过去月份的范围（始终排除该年份的当月）
    /// </summary>
    /// <returns></returns>
    public static (DateTime startDate, DateTime endDate) GetPastMonthsRangeExcludingCurrentMonth(
           int year,
           DateTime? currentDate = null)
    {
        var today = currentDate ?? DateTime.Today;
        bool isCurrentYear = (year == today.Year);

        // 最小月份始终是1月1日
        DateTime startDate = new DateTime(year, 1, 1);

        // 计算最大月份最后一天
        DateTime endDate = isCurrentYear && today.Month > 1
            ? new DateTime(year, today.Month, 1).AddDays(-1)  // 当前年的上个月最后一天
            : new DateTime(year, 12, 31);                    // 非当前年或当前年1月时返回全年

        return (startDate, endDate);
    }
    /// <summary>
    /// 根据月份获取账期
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private async Task<WMSHachAccountDate> GetAccountByDate(DateTime date)
    {
        return await _repHachAccountDate.AsQueryable()
            .Where(x => x.StartDate.HasValue &&
                        x.StartDate.Value.Year == date.Year &&
                        x.StartDate.Value.Month == date.Month)
            .FirstAsync();
    }

    /// <summary>
    /// 获取当天 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetInventoryUsableGroupBySkuByToday(DateTime today, Dictionary<string, double> priceMap)
    {
        var result = new Dictionary<string, double>();

        try
        {
            // 1. 获取当天库存数据（按SKU分组汇总）
            var inventoryData = await _repInventoryUsable.AsQueryable()
                .Where(a => a.InventoryStatus == 1)
                .Where(a => a.InventoryTime >= today.Date && a.InventoryTime <= today.Date.AddDays(1).AddSeconds(-1))
                .GroupBy(a => a.SKU)
                .Select(a => new
                {
                    SKU = a.SKU,
                    Qty = SqlFunc.AggregateSum(a.Qty)
                })
                .ToListAsync();

            // 2. 计算每个SKU的金额
            foreach (var item in inventoryData)
            {
                if (priceMap.TryGetValue(item.SKU, out var price))
                {
                    result[item.SKU] = item.Qty * price;
                }
                else
                {
                    // 未找到价格的SKU记为0
                    result[item.SKU] = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
        return result;
    }
    #endregion
}
