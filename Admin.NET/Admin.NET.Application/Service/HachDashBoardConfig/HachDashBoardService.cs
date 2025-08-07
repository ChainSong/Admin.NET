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
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Admin.NET.Application;

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
        SqlSugarRepository<WMSASNDetail> repASNDetail,
        SqlSugarRepository<WMSInventoryUsable> repInventoryUsable
        )
    {
        _userManager = userManager;
        _repHachAccountDate = repHachAccountDate;
        _repInventoryUsableSnapshot = repInventoryUsableSnapshot;
        _repInventoryUsable = repInventoryUsable;
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
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetSumItemData")]
    public async Task<SumItemOutput> GetSumItemData()
    {
        SumItemOutput itemOutput = new SumItemOutput();
        var today = DateTime.Today;
        var currentMonthString = today.ToString("yyyy-MM");
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        // 并行获取数据
        //var tasks = new List<Task<long>>
        //    {
        //      //GetInventoryUsableSnapshotByAccountDate(today.AddMonths(-1), priceMap),
        //      //GetInventoryUsableByToday(today, priceMap),
        //      GetTargetAmountByMonth(currentMonthString),
        //      GetYTDOrderVSASNAmount()
        //    };
        // 等待所有任务完成
        //var results = await Task.WhenAll(tasks);
        //上个月的库存金额
        itemOutput.LastMonthAmount = await GetInventoryUsableSnapshotByAccountDate(today.AddMonths(-1), priceMap);
        //这天的库存金额
        itemOutput.CurrentMonthAmount = await GetInventoryUsableByToday(today, priceMap);
        //获取当月库存目标金额
        itemOutput.CurrentTargetAmount = await GetTargetAmountByMonth(currentMonthString);
        //获取YTD ORDER VS YTD ASN
        itemOutput.YTDOrderVSASNAmount =await GetYTDOrderVSASNAmount();
        // 计算差值
        itemOutput.CMonthVSTargetAmount = itemOutput.CurrentMonthAmount - itemOutput.CurrentTargetAmount;
        // 获取当月入库和出库金额
        itemOutput.CurrentReceiptAmount = Convert.ToInt64(await GetCurrentReceiptAmount(today, priceMap));
        itemOutput.CurrentOrderAmount = Convert.ToInt64(await GetCurrentOrderAmount(today, priceMap));
        return itemOutput;
    }

    /// <summary>
    /// 屏一
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetChartsOneData")]
    public async Task<MonthVSLastOutput> GetChartsOneData(ChartsInput input)
    {
        MonthVSLastOutput monthVSLastOutput = new MonthVSLastOutput();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        //月库存金额趋势图VS去年 当月没到关账日不展示数据
        monthVSLastOutput.monthVSLast = await GetMonthVSLast(input, priceMap);
        //及时库存
        monthVSLastOutput.TodayPriceOfInventory = await GetProductMinorPrice(input, priceMap);
        //当月库存出库产品总金额
        monthVSLastOutput.CurrentMonthPriceOfOB = await GetObPriceByMonth(input, priceMap);
        //库销比
        monthVSLastOutput.CurrentMonthPriceReceiptVSOB = await GetInventoryToSellPercent(input, priceMap);
        //当年库存总金额vs去年库存总金额
        monthVSLastOutput.CumulativeAmountVSLastThreeMonth = await GetCumulativeAmountVSLastThreeMonth(input, priceMap);
        return monthVSLastOutput;
    }

    #region 屏二
    #endregion

    #region 屏三
    #endregion

    #region 封装的公共方法

    #region tab汇总项封装方法
    /// <summary>
    /// 获取 SKU -> Price 的映射字典
    /// </summary>
    /// <returns></returns>
    private async Task<List<CustomerProductPriceMapping>> GetProductPriceMap()
    {
        return await _repProduct.AsQueryable()
            .Select(p => new CustomerProductPriceMapping 
            {
               CustomerName=p.CustomerName,
               CustomerId=p.CustomerId,
               Sku=p.SKU,
               Price=p.Price
            })
            .ToListAsync();
    }

    /// <summary>
    /// 通过库存数量和价格表计算总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private long CalculateTotalAmount(IEnumerable<(long CustomerId,string CustomerName,string SKU, double Qty)> data, List<CustomerProductPriceMapping> priceMap)
    {
        double total = 0;
        foreach (var item in data)
        {
            var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU && p.CustomerId==item.CustomerId);
            if (priceEntry != null && priceEntry.Price.HasValue)
            {
                total += item.Qty * priceEntry.Price.Value;
            }
        }
        return Convert.ToInt64(total); // 向下取整，可换成 Math.Round(total)
    }
    /// <summary>
    /// 获取月份 获取 库存快照表数据
    /// </summary>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetInventoryUsableSnapshotByAccountDate(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
            .Select(a => new {a.CustomerId,a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        return CalculateTotalAmount(
            lastMonthInventory.Select(x => (x.CustomerId,x.CustomerName,x.SKU, (double)x.Qty)),
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
    private async Task<long> GetInventoryUsableByAmountDate(DateTime today, List<CustomerProductPriceMapping> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);

        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= AccountDate.StartDate && a.InventoryTime <= AccountDate.EndDate)
            .GroupBy(a => a.SKU)
            .Select(a => new {a.CustomerId,a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        return CalculateTotalAmount(
            currentInventory.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.Qty)),
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
    private async Task<long> GetInventoryUsableByToday(DateTime today, List<CustomerProductPriceMapping> priceMap)
    {
        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= today && a.InventoryTime <= today)
            .GroupBy(a => new { a.SKU,a.CustomerId,a.CustomerName})
            .Select(a => new { a.CustomerId,a.CustomerName,a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        //获取所有总金额
        return CalculateTotalAmount(
            currentInventory.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.Qty)),
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
            //.GroupBy(a => 1)
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
    private async Task<double> GetCurrentReceiptAmount(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
         data.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.ExpectedQty)),
         priceMap
     );
    }

    /// <summary>
    /// 获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double> GetCurrentOrderAmount(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
      data.Select(x => (x.CustomerId.GetValueOrDefault(), x.CustomerName,x.SKU, (double)x.AllocatedQty)),
      priceMap
  );
    }

    /// <summary>
    /// 根据sku获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetCurrentOrderGroupBySKUByAmount(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
            var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU);
            if (priceEntry!=null)
            {
                double amount =Convert.ToInt64(item.TotalQty * priceEntry.Price);
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
    private async Task<Dictionary<DateTime, long>> GetReceiptGroupByDateAmount(DateTime today, List<CustomerProductPriceMapping> priceMap)
    {
        var AccountDate = await GetAccountByDate(today);
        Dictionary<DateTime, long> dailyAmount = new Dictionary<DateTime, long>();
        if (AccountDate == null)
            return dailyAmount;
        // 遍历当月的每一天
        for (var date = Convert.ToDateTime(AccountDate.StartDate); date <= AccountDate.EndDate; date = Convert.ToDateTime(AccountDate.StartDate).AddDays(1))
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
                todayData.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.ExpectedQty)),
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
    private async Task<Dictionary<DateTime, long>> GetOrderGroupByDateAmount(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
                todayData.Select(x => (x.CustomerId.GetValueOrDefault(), x.CustomerName, x.SKU, (double)x.AllocatedQty)),
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
    private List<string> GetPastMonthsExcludingCurrent(int targetYear)
    {
        var today = DateTime.Today;
        var currentYear = today.Year;
        var currentMonth = today.Month;

        var pastMonths = new List<string>();

        // 如果目标年份是当前年份，则只统计到上个月
        if (targetYear == currentYear)
        {
            for (int month = 1; month <= currentMonth; month++)
            {
                var monthStr = new DateTime(targetYear, month, 1).ToString("yyyy-MM");
                pastMonths.Add(monthStr);
            }
        }
        // 如果目标年份是过去的年份，则返回所有12个月
        else if (targetYear < currentYear)
        {
            for (int month = 1; month <= 12; month++)
            {
                var monthStr = new DateTime(targetYear, month, 1).ToString("yyyy-MM");
                pastMonths.Add(monthStr);
            }
        }
        // 如果目标年份是未来年份，则返回空列表（因为还没有月份过去）
        else
        {
            return pastMonths; // 空列表
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
        // 该月第一天
        var startOfMonth = new DateTime(date.Year, date.Month, 1);
        // 下个月第一天（不包含）
        var endOfMonth = startOfMonth.AddMonths(1);
        return await _repHachAccountDate.AsQueryable()
            .Where(x => x.StartDate.HasValue &&
                        x.StartDate.Value >= startOfMonth &&
                        x.StartDate.Value < endOfMonth)
            .FirstAsync(); // 或 FirstOrDefaultAsync()
    }

    /// <summary>
    /// 获取当天 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetInventoryUsableGroupBySkuByToday(DateTime today, List<CustomerProductPriceMapping> priceMap)
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
                var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU);
                if (priceEntry!=null)
                {
                    result[item.SKU] =Convert.ToInt64(item.Qty * priceEntry.Price);
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
    /// <summary>
    /// 获取目标月份 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<long> GetInventoryUsableGroupBySkuByMonth(DateTime StartDate,
        DateTime EndDate, List<CustomerProductPriceMapping> priceMap)
    {
        try
        {
            // 1. 获取指定时间库存数据（按SKU分组汇总）
            var inventoryData = await _repInventoryUsableSnapshot.AsQueryable()
                .Where(a => a.InventoryStatus == 1)
                .Where(a => a.InventoryTime >= StartDate && a.InventoryTime <= EndDate)
                .GroupBy(a => a.SKU)
                .Select(a => new
                {
                    SKU = a.SKU,
                    Qty = SqlFunc.AggregateSum(a.Qty),
                    CustomerId= a.CustomerId,
                    CustomerName = a.CustomerName,
                })
                .ToListAsync();
            return CalculateTotalAmount(
         inventoryData.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.Qty)),
         priceMap
     );
        }
        catch (Exception ex)
        {
        }

        return 0;
    }
    #endregion

    #region 大屏一
    /// <summary>
    /// 月库存金额趋势图VS去年
    /// </summary>
    /// <returns></returns>
    /// 当月没到关账日不展示数据
    private async Task<MonthVSLast> GetMonthVSLast(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        MonthVSLast monthVSLastOutput = new MonthVSLast();

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
            LastYear.Add(Convert.ToInt64(Convert.ToDateTime(TargetMonth.StartDate).ToString("MM")), LastData);
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
            CurrentYear.Add(Convert.ToInt64(Convert.ToDateTime(TargetMonth.EndDate).ToString("MM")), CurrentData);
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
    private async Task<Dictionary<string, double>> GetProductMinorPrice(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        return await GetInventoryUsableGroupBySkuByToday(DateTime.Today, priceMap);
    }

    /// <summary>
    /// 当月库存出库产品总金额
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetObPriceByMonth(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        return await GetCurrentOrderGroupBySKUByAmount(DateTime.Today, priceMap);
    }

    /// <summary>
    /// 库销比
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<Dictionary<DateTime, string>> GetInventoryToSellPercent(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
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
        return dailyPercentages;
    }

    /// <summary>
    ///获取月累计库存,当年不计算当月 跟去年对比
    /// </summary>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetCumulativeAmountVSLast(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        var today = DateTime.Today;
        var currentYear = today.Year;
        var lastYear = currentYear - 1;

        // 获取两个年份的月份列表(当年不包含当前月)
        var lastYearMonths = GetPastMonthsExcludingCurrent(lastYear);
        var currentYearMonths = GetPastMonthsExcludingCurrent(currentYear);
        Dictionary<string, double> LastYearData = new Dictionary<string, double>();
        Dictionary<string, double> CurrentYearData = new Dictionary<string, double>();
        Dictionary<string, double> ComparisonResults = new Dictionary<string, double>();

        // 处理去年数据
        foreach (var month in lastYearMonths)
        {
            var monthDate = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);

            // 累计计算:从年度开始到当前月末
            var cumulativeValue = await GetInventoryUsableGroupBySkuByMonth(
                new DateTime(lastYear, 1, 1), accountDate.EndDate.GetValueOrDefault(), priceMap);

            LastYearData[month] = cumulativeValue;
        }

        // 处理今年数据
        foreach (var month in currentYearMonths)
        {
            var monthDate = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);

            // 累计计算:从年度开始到当前月末
            var cumulativeValue = await GetInventoryUsableGroupBySkuByMonth(
                new DateTime(currentYear, 1, 1), accountDate.EndDate.GetValueOrDefault(), priceMap);

            CurrentYearData[month] = cumulativeValue;

            // 计算同比(与去年同月比较)
            var lastYearMonth = $"{lastYear}-{month.Substring(5)}";
            if (LastYearData.TryGetValue(lastYearMonth, out var lastYearValue) && lastYearValue != 0)
            {
                var growthRate = (cumulativeValue - lastYearValue) / lastYearValue * 100;
                ComparisonResults[month] = Math.Round(growthRate, 2);
            }
            else
            {
                ComparisonResults[month] = 0; // 无对比数据或除零情况
            }
        }
        return ComparisonResults;
    }

    /// <summary>
    /// 获取过去三个月累计库存,当年不计算当月 跟去年对比
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetCumulativeAmountVSLastThreeMonth(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        var today = DateTime.Today;
        var currentMonth = new DateTime(today.Year, today.Month, 1);

        var currentYearLastThreeMonths = Enumerable.Range(1, 3)
            .Select(i => currentMonth.AddMonths(-i))
            .ToList();

        var lastYearLastThreeMonths = currentYearLastThreeMonths
            .Select(m => m.AddYears(-1))
            .ToList();

        var lastYearData = new Dictionary<string, double>();
        var currentYearData = new Dictionary<string, double>();
        var comparisonResults = new Dictionary<string, double>();

        // 去年数据
        foreach (var month in lastYearLastThreeMonths)
        {
            var accountDate = await GetAccountByDate(month);
            if (accountDate?.StartDate == null || accountDate?.EndDate == null)
                continue;

            var value = await GetInventoryUsableGroupBySkuByMonth(
                accountDate.StartDate.Value,
                accountDate.EndDate.Value,
                priceMap
            );
            lastYearData[month.ToString("yyyy-MM")] = value;
        }

        // 今年数据及同比计算
        foreach (var month in currentYearLastThreeMonths)
        {
            var accountDate = await GetAccountByDate(month);
            if (accountDate?.StartDate == null || accountDate?.EndDate == null)
                continue;

            var value = await GetInventoryUsableGroupBySkuByMonth(
                accountDate.StartDate.Value,
                accountDate.EndDate.Value,
                priceMap
            );
            var monthKey = month.ToString("yyyy-MM");
            currentYearData[monthKey] = value;

            var lastYearKey = month.AddYears(-1).ToString("yyyy-MM");
            if (lastYearData.TryGetValue(lastYearKey, out var lastYearValue) && lastYearValue != 0)
            {
                var growthRate = (value - lastYearValue) / lastYearValue * 100;
                comparisonResults[monthKey] = Math.Round(growthRate, 2);
            }
            else
            {
                comparisonResults[monthKey] = 0;
            }
        }

        return comparisonResults;
    }
    #endregion
    #endregion
}
