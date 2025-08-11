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
using Elasticsearch.Net;
using Admin.NET.Common.AMap;
using Admin.NET.Common.AMap.Response;
using FastExpressionCompiler;
using XAct;

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

    #region 获取物料下拉列表
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "SelectProductList")]
    public async Task<List<SelectItem>> SelectProductList(List<long> CustomerIds)
    {
        return await _repProduct.AsQueryable()
            .Where(a => a.ProductStatus == 1)
            .WhereIF(CustomerIds.Count > 0 && CustomerIds.Any(), a => CustomerIds.Contains(a.CustomerId))
            .Select(a => new SelectItem
            {
                Id = a.Id,
                Value = a.SKU,
                Label = a.GoodsName
            })
            .ToListAsync();
    }
    #endregion

    #region 获取客户下拉列表
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "SelectCustomerList")]
    public async Task<List<SelectItem>> SelectCustomerList(List<long> CustomerIds)
    {
        return await _repCustomer.AsQueryable()
            .Where(a => a.CustomerStatus == 1)
            .WhereIF(CustomerIds.Count > 0 && CustomerIds.Any(), a => CustomerIds.Contains(a.Id))
            .Select(a => new SelectItem
            {
                Id = a.Id,
                Label = a.CustomerName
            })
            .ToListAsync();
    }
    #endregion

    #region 获取出库省份下拉列表
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "SelectOBProvinceList")]
    public async Task<List<SelectItem>> SelectOBProvinceList(ProvinceInput input)
    {
        AMap AMap = new AMap();
        List<SelectItem> selectItems = new List<SelectItem>();
        List<GeoDistrictCode> geoDistrictCodes = new List<GeoDistrictCode>();
        if (string.IsNullOrEmpty(input.Keywords)) return selectItems;
        var geoPOI = await AMap.RequestGeoCodeDistrict(input.Keywords, input.Subdistrict, input.Extensions);
        geoDistrictCodes = geoPOI?.districts ?? new List<GeoDistrictCode>();
        if (geoDistrictCodes != null && geoDistrictCodes.Count > 0)
        {
            foreach (var country in geoDistrictCodes)
            {
                foreach (var districts in country.Districts)
                {
                    selectItems.Add(new SelectItem
                    {
                        Value = districts.AdCode,
                        Label = districts.Name
                    });
                }
            }
        }
        return selectItems;
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
        ChartsInput input = new ChartsInput();
        SumItemOutput itemOutput = new SumItemOutput();
        var today = DateTime.Today;
        var currentMonthString = today.ToString("yyyy-MM");
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        //上个月的库存金额
        itemOutput.LastMonthAmount = await GetInventoryUsableSnapshotByAccountDate(input, today.AddMonths(-1), priceMap);
        //这天的库存金额
        itemOutput.CurrentMonthAmount = await GetInventoryUsableByToday(today, priceMap);
        //获取当月库存目标金额
        itemOutput.CurrentTargetAmount = await GetTargetAmountByMonth(currentMonthString);
        //获取YTD ORDER VS YTD ASN
        itemOutput.YTDOrderVSASNAmount = await GetYTDOrderVSASNAmount();
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
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        //月库存金额趋势图VS去年 当月没到关账日不展示数据
        monthVSLastOutput.MonthVSLast = await GetMonthVSLast(input, priceMap);
        //及时库存
        monthVSLastOutput.TodayPriceOfInventory = await GetProductMinorPrice(input, priceMap);
        //当月库存出库产品总金额
        monthVSLastOutput.CurrentMonthPriceOfOB = await GetObPriceByMonth(input, priceMap);
        //库销比
        monthVSLastOutput.CurrentMonthPriceReceiptVSOB = await GetInventoryToSellPercent(input, priceMap);
        //月累计对比去年
        monthVSLastOutput.MonthlyCumulativeAmountVSLast = await GetCumulativeAmountVSLast(input, priceMap);
        //获取过去三个月累计库存
        monthVSLastOutput.CumulativeAmountVSLastThreeMonth = await GetCumulativeAmountVSLastThreeMonth(input, priceMap);
        return monthVSLastOutput;
    }

    #region 屏二
    #endregion

    #region 屏三
    /// <summary>
    /// 屏三 根据省份获取出库数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetByOBProvince")]
    public async Task<OBProvinceOutput> GetByOBProvince(ChartsInput input)
    {
        OBProvinceOutput outputs = new OBProvinceOutput();
        List<OBProvinceGroupbyWhere> listByWhere = new List<OBProvinceGroupbyWhere>();
        List<OBProvince> totalList = new List<OBProvince>();
        try
        {
            //没选中省份 那就返回空list
            if (string.IsNullOrEmpty(input.OBProvince))
                return outputs;

            // 商品价格字典缓存
            var priceMap = await GetProductPriceMap();

            var orderData = await _repOrder.AsQueryable()
                .LeftJoin<WMSOrderAddress>((o, oa) => o.PreOrderId == oa.PreOrderId)
                .LeftJoin<WMSOrderDetail>((o, oa, od) => o.Id == od.OrderId)
                .Where((o, oa, od) => o.OrderStatus == 99)
                .WhereIF(input.Month.HasValue, (o, oa, od) => o.CreationTime >= DateTime.Parse(input.Month + "-01"))
                .WhereIF(input.Month.HasValue, (o, oa, od) => o.CreationTime <= DateTime.Parse(input.Month + "-01").AddMonths(1).AddTicks(-1))
                .WhereIF(input.CustomerId.HasValue, (o, oa, od) => o.CustomerId == input.CustomerId)
                .WhereIF(!string.IsNullOrEmpty(input.OBProvince), (o, oa, od) => oa.Province.Contains(input.OBProvince))
                .GroupBy((o, oa, od) => new { oa.Province, od.SKU, o.CustomerName, o.CustomerId })
                .Select((o, oa, od) => new
                {
                    ObProvince = oa.Province,
                    Customer = o.CustomerName,
                    CustomerId = o.CustomerId,
                    Sku = od.SKU,
                    Qty = SqlFunc.AggregateSum(od.AllocatedQty),
                })
                .ToListAsync();
            if (orderData != null && orderData.Count > 0)
            {
                // 计算每个省份的总金额
                var provinceGroups = orderData.GroupBy(x => x.ObProvince);
                foreach (var provinceGroup in provinceGroups)
                {
                    var provinceTotalData = new OBProvinceGroupbyWhere
                    {
                        ObProvince = provinceGroup.Key,
                        Qty = provinceGroup.Sum(x => x.Qty),
                        Amount = CalculateTotalAmount(
                        provinceGroup.Select(x => (x.CustomerId, x.Customer, x.Sku, x.Qty)),
                        priceMap),
                        Customer = provinceGroup.Select(x => x.Customer).FirstOrDefault(),
                        CustomerId = provinceGroup.Select(x => x.CustomerId).FirstOrDefault(),
                    };
                    totalList.Add(totalList);
                }

                // 计算每个省份的总金额
                var provinceGroupss = orderData.GroupBy(x => new { x.ObProvince });
                foreach (var provinceGroup in provinceGroupss)
                {
                    var provinceTotalData = new OBProvince
                    {
                        ObProvince = provinceGroup.Key.ObProvince,
                        Qty = provinceGroup.Sum(x => x.Qty),
                        Amount = CalculateTotalAmount(
                            provinceGroup.Select(x => (x.CustomerId, x.Customer, x.Sku, x.Qty)),
                            priceMap),
                    };
                    totalList.Add(provinceTotalData);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        outputs.oBProvince = totalList;
        outputs.oBProvinceGroupbyProvince = listByWhere;
        return outputs;
    }
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
                CustomerName = p.CustomerName,
                CustomerId = p.CustomerId,
                Sku = p.SKU,
                Price = p.Price
            })
            .ToListAsync();
    }

    /// <summary>
    /// 通过库存数量和价格表计算总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private long CalculateTotalAmount(IEnumerable<(long CustomerId, string CustomerName, string SKU, double Qty)> data, List<CustomerProductPriceMapping> priceMap)
    {
        double total = 0;
        foreach (var item in data)
        {
            var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU && p.CustomerId == item.CustomerId);
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
    private async Task<long> GetInventoryUsableSnapshotByAccountDate(ChartsInput input, DateTime today, List<CustomerProductPriceMapping> priceMap)
    {
        var lastAccountDate = await GetAccountByDate(today);

        // 如果查询不到记录或EndDate大于今天，直接返回0
        if (lastAccountDate == null)
        {
            return 0;
        }
        var lastMonthInventory = await _repInventoryUsableSnapshot.AsQueryable()
            .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
            .Where(a => a.InventoryTime >= lastAccountDate.StartDate && a.InventoryTime <= lastAccountDate.EndDate)
            .GroupBy(a => new { a.CustomerId, a.CustomerName, a.SKU })
            .Select(a => new { a.CustomerId, a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
            .ToListAsync();

        return CalculateTotalAmount(
            lastMonthInventory.Select(x => (x.CustomerId, x.CustomerName, x.SKU, (double)x.Qty)),
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
        // 如果查询不到记录或EndDate大于今天，直接返回0
        if (AccountDate == null)
        {
            return 0;
        }
        var currentInventory = await _repInventoryUsable.AsQueryable()
            .Where(a => a.InventoryStatus == 1)
            .Where(a => a.InventoryTime >= AccountDate.StartDate && a.InventoryTime <= AccountDate.EndDate)
            .GroupBy(a => a.SKU)
            .Select(a => new { a.CustomerId, a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
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
            .Where(a => a.InventoryTime >= today && a.InventoryTime <= today.AddDays(1).AddSeconds(-1))
            .GroupBy(a => new { a.SKU, a.CustomerId, a.CustomerName })
            .Select(a => new { a.CustomerId, a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) })
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
            .Select(a => new { PlanKRMB = SqlFunc.AggregateSum(SqlFunc.ToDecimal(a.PlanKRMB)) })
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

        if (AccountDate != null && AccountDate.EndDate < today)
        {
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

        return 0;
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
        if (AccountDate != null && AccountDate.EndDate < today)
        {
            var data = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId &&
                                    o.OrderStatus == 99 &&
                                    o.CreationTime >= AccountDate.StartDate &&
                                    o.CreationTime <= AccountDate.EndDate)
                        .Any())
            .ToListAsync();

            return CalculateTotalAmount(
          data.Select(x => (x.CustomerId.GetValueOrDefault(), x.CustomerName, x.SKU, (double)x.AllocatedQty)),
          priceMap
      );
        }
        return 0;
    }

    /// <summary>
    /// 根据sku获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetCurrentOrderGroupBySKUByAmount(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        var AccountDate = await GetAccountByDate(Convert.ToDateTime(input.Month));

        // 3. 计算各SKU金额
        var skuAmounts = new List<ChartIndex>();
        // 如果查询不到记录或EndDate大于今天，直接返回0
        if (AccountDate == null)
        {
            return skuAmounts;
        }
        // 2. 查询当月出库数据（按SKU分组）
        var skuData = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                      .Where(o => o.Id == d.OrderId &&
                                  o.OrderStatus == 99 &&
                                  o.CreationTime >= AccountDate.StartDate &&
                                  o.CreationTime <= AccountDate.EndDate)
                      .Any())
            .GroupBy(d => d.SKU)
            .Take(input.Take)
            .Select(d => new
            {
                SKU = d.SKU,
                TotalQty = SqlFunc.AggregateSum(d.AllocatedQty)
            })
            .ToListAsync();


        if (skuData != null && skuData.Count > 0)
        {
            foreach (var item in skuData)
            {
                var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU);
                skuAmounts.Add(new ChartIndex
                {
                    Xseries = item.SKU,
                    Yseries = priceEntry != null ? item.TotalQty * priceEntry.Price : 0
                });
            }
        }
        else
        {
            foreach (var item in priceMap.Take(input.Take))
            {
                skuAmounts.Add(new ChartIndex
                {
                    Xseries = item.Sku,
                    Yseries = 0
                });
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
        for (var date = Convert.ToDateTime(AccountDate.StartDate); date <= AccountDate.EndDate; date = date.AddDays(1))
        {
            //if (date.AddDays(1).AddSeconds(-1) > today.AddDays(1).AddSeconds(-1))
            //    continue;
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
        for (var date = Convert.ToDateTime(AccountDate.StartDate); date <= AccountDate.EndDate; date = date.AddDays(1))
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
    private List<string> GetPastMonthsExcludingCurrent(DateTime TargetDate)
    {
        var today = DateTime.Today;
        var currentYear = today.Year;
        var currentMonth = today.Month;

        var pastMonths = new List<string>();

        // 如果目标年份是当前年份，则只统计到上个月
        if (TargetDate.Year == currentYear)
        {
            for (int month = 1; month <= TargetDate.AddMonths(-1).Month; month++)
            {
                var monthStr = new DateTime(TargetDate.Year, month, 1).ToString("yyyy-MM");
                pastMonths.Add(monthStr);
            }
        }
        // 如果目标年份是过去的年份，则返回所有12个月
        else if (TargetDate.Year < currentYear)
        {
            var TargetMonth = 12;

            if (TargetDate.Month < currentMonth)
                if (TargetDate.Month < currentMonth)
                {
                    TargetMonth = Convert.ToInt32(TargetDate.Month);
                }
            for (int month = 1; month <= TargetMonth; month++)
            {
                var monthStr = new DateTime(TargetDate.Year, month, 1).ToString("yyyy-MM");
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
    private async Task<List<ChartIndex>> GetInventoryUsableGroupBySkuByToday(ChartsInput input, DateTime today, List<CustomerProductPriceMapping> priceMap)
    {
        var result = new List<ChartIndex>();
        try
        {
            var inventoryData1 = _repInventoryUsable.AsQueryable()
                .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .Where(a => a.InventoryStatus == 1)
                .Where(a => a.InventoryTime >= today.Date && a.InventoryTime <= today.Date.AddDays(1).AddSeconds(-1))
                .GroupBy(a => a.SKU)
                .Take(input.Take)
                .Select(a => new
                {
                    SKU = a.SKU,
                    Qty = SqlFunc.AggregateSum(a.Qty)
                })
                .ToSqlString();
            // 1. 获取当天库存数据（按SKU分组汇总）
            var inventoryData = await _repInventoryUsable.AsQueryable()
                .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
                .Where(a => a.InventoryStatus == 1)
                .Where(a => a.InventoryTime >= today.Date && a.InventoryTime <= today.Date.AddDays(1).AddSeconds(-1))
                .GroupBy(a => a.SKU)
                .Take(input.Take)
                .Select(a => new
                {
                    SKU = a.SKU,
                    Qty = SqlFunc.AggregateSum(a.Qty)
                })
                .ToListAsync();
            if (inventoryData != null && inventoryData.Count > 0)
            {
                // 2. 计算每个SKU的金额
                foreach (var item in inventoryData)
                {
                    var priceEntry = priceMap.FirstOrDefault(p => p.Sku == item.SKU);
                    result.Add(new ChartIndex
                    {
                        Xseries = item.SKU,
                        Yseries = priceEntry != null ? item.Qty * priceEntry.Price : 0
                    });
                }
            }
            else
            {
                foreach (var item in priceMap.Take(input.Take))
                {
                    result.Add(new ChartIndex
                    {
                        Xseries = item.Sku,
                        Yseries = 0
                    });
                }
            }

        }
        catch (Exception ex)
        {
        }
        result = result.OrderByDescending(a => a.Yseries).ToList();
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
                .GroupBy(a => new { a.SKU, a.CustomerId, a.CustomerName })
                .Select(a => new
                {
                    SKU = a.SKU,
                    Qty = SqlFunc.AggregateSum(a.Qty),
                    CustomerId = a.CustomerId,
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
        List<ChartIndex> LastYear = new List<ChartIndex>();
        foreach (var item in ALLLastMonth)
        {
            long LastData = 0;
            //获取目标月份账期起始日，结束日
            var TargetMonth = await GetAccountByDate(item);
            if (TargetMonth != null)
            {
                LastData = await GetInventoryUsableSnapshotByAccountDate(input, TargetMonth.StartDate.GetValueOrDefault(), priceMap);
                LastYear.Add(new ChartIndex
                {
                    Xseries = Convert.ToDateTime(TargetMonth.StartDate).ToString("MM"),
                    Yseries = LastData
                });
                continue;
            }
            LastYear.Add(new ChartIndex
            {
                Xseries = Convert.ToDateTime(item).ToString("MM"),
                Yseries = LastData
            });
        }
        #endregion

        #region 获取今年的月份数据
        //获取今年的月份数据
        var ALLCurrentMonth = Enumerable.Range(1, 12) // 生成1-12月
        .Select(month => new DateTime(DateTime.Today.Year, month, 1))
        .ToList();
        List<ChartIndex> CurrentYear = new List<ChartIndex>();
        foreach (var item in ALLCurrentMonth)
        {
            long CurrentData = 0;
            //获取目标月份账期起始日，结束日
            var TargetMonth = await GetAccountByDate(item);
            if (TargetMonth != null)
            {
                CurrentData = await GetInventoryUsableSnapshotByAccountDate(input, TargetMonth.StartDate.GetValueOrDefault(), priceMap);
                CurrentYear.Add(new ChartIndex
                {
                    Xseries = Convert.ToDateTime(TargetMonth.StartDate).ToString("MM"),
                    Yseries = CurrentData
                });
                continue;
            }
            CurrentYear.Add(new ChartIndex
            {
                Xseries = Convert.ToDateTime(item).ToString("MM"),
                Yseries = CurrentData
            });
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
    private async Task<List<ChartIndex>> GetProductMinorPrice(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        return await GetInventoryUsableGroupBySkuByToday(input, DateTime.Today, priceMap);
    }

    /// <summary>
    /// 当月库存出库产品总金额
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetObPriceByMonth(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        return await GetCurrentOrderGroupBySKUByAmount(input, priceMap);
    }

    /// <summary>
    /// 库销比
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetInventoryToSellPercent(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        var receiptData = await GetReceiptGroupByDateAmount(Convert.ToDateTime(input.Month), priceMap);
        var OrderData = await GetOrderGroupByDateAmount(Convert.ToDateTime(input.Month), priceMap);
        // 创建字典来存储每天的比率
        List<ChartIndex> dailyPercentages = new List<ChartIndex>();

        // 遍历所有日期，并计算入库与出库的比率
        foreach (var receipt in receiptData)
        {
            // 获取当天的入库金额
            var receiptAmount = receipt.Value;

            // 获取当天的出库金额
            if (OrderData.ContainsKey(receipt.Key))
            {
                var orderAmount = OrderData[receipt.Key];
                dailyPercentages.Add(new ChartIndex
                {
                    Xseries = Convert.ToString(receipt.Key),
                    Yseries = orderAmount == 0 ? 0 : (receiptAmount / (double)orderAmount) * 100
                });
            }
            else
            {
                dailyPercentages.Add(new ChartIndex
                {
                    Xseries = Convert.ToString(receipt.Key),
                    Yseries = 0
                });
            }
        }
        return dailyPercentages;
    }

    /// <summary>
    ///获取月累计库存,当年不计算当月 跟去年对比
    /// </summary>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetCumulativeAmountVSLast(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {

        var pivotMonth = Convert.ToDateTime(input.Month); // 例如 2025-08-01
        var currentYear = pivotMonth.Year;
        var lastYear = currentYear - 1;

        // 今年：从1月到“pivotMonth的上一个月”
        // 去年：从1月到“去年对应的上一个月”
        // 统一取“今年有效月份列表（不含当月）”，然后映射出去年的同月
        var currentYearMonths = GetPastMonthsExcludingCurrent(pivotMonth); // ["2025-01", ... , "2025-07"]
        var lastYearMonths = GetPastMonthsExcludingCurrent(pivotMonth.AddYears(-1)); // ["2025-01", ... , "2025-07"]

        // 准备累计Map：yyyy-MM -> decimal(累计金额)
        var currentCum = new Dictionary<string, decimal>();
        var lastCum = new Dictionary<string, decimal>();

        // 预先把每个月的“当月截止日”取出来，减少重复查询
        // 今年
        foreach (var month in currentYearMonths)
        {
            var monthDate = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);
            var end = accountDate?.EndDate ?? new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));

            // 今年累计：从当年1月1日 -> 当月截止
            // 注意：你的 GetInventoryUsableGroupBySkuByMonth 返回 long，这里转 decimal 参与计算
            var val = await GetInventoryUsableGroupBySkuByMonth(
                new DateTime(currentYear, 1, 1),
                end,
                priceMap
            );
            currentCum[month] = (decimal)val;
        }

        // 去年的对应同月（比如今年“2025-03”的同比基数是“2024-03”）
        foreach (var month in lastYearMonths)
        {
            var mm = month.Substring(5);           // "MM"
            var lyKey = $"{lastYear}-{mm}";        // "yyyy-MM" 去年同月
            var lyMonthDate = DateTime.ParseExact(lyKey, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(lyMonthDate);
            var end = accountDate?.EndDate ?? new DateTime(lyMonthDate.Year, lyMonthDate.Month, DateTime.DaysInMonth(lyMonthDate.Year, lyMonthDate.Month));

            var val = await GetInventoryUsableGroupBySkuByMonth(
                new DateTime(lastYear, 1, 1),
                end,
                priceMap
            );
            lastCum[lyKey] = (decimal)val;
        }

        // 计算同比：((今年累计 - 去年同期累计) / 去年同期累计) * 100
        var result = new List<ChartIndex>();
        foreach (var month in currentYearMonths)
        {
            var mm = month.Substring(5);       // "MM"
            var lyKey = $"{lastYear}-{mm}";

            currentCum.TryGetValue(month, out var curVal);
            lastCum.TryGetValue(lyKey, out var lyVal);

            long y;
            if (lyVal != 0m)
            {
                //同比增长率
                var growth = (Convert.ToDouble(curVal - lyVal)) / Convert.ToDouble(lyVal) * Convert.ToDouble(100m); // decimal 计算百分比
                                                                                                                    // 你目前的 Yseries 是 long，只能取整；若要保留两位小数建议把 ChartIndex.Yseries 改为 decimal
                y = (long)Math.Round(growth, 0, MidpointRounding.AwayFromZero);
            }
            else
            {
                y = 0; // 无基数或为0时同比显示0，避免除零
            }

            result.Add(new ChartIndex
            {
                Xseries = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture).ToString("MM"),
                Yseries = y
            });
        }

        return result;

    }

    /// <summary>
    /// 获取过去三个月累计库存,当年不计算当月 跟去年对比
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetCumulativeAmountVSLastThreeMonth(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        var today = Convert.ToDateTime(input.Month);
        var currentMonth = new DateTime(today.Year, today.Month, 1);
        var lastYear = today.Year - 1;

        var currentYearLastThreeMonths = Enumerable.Range(1, 3)
            .Select(i => currentMonth.AddMonths(-i))
            .ToList();

        var lastYearLastThreeMonths = currentYearLastThreeMonths
            .Select(m => m.AddYears(-1))
            .ToList();

        var lastYearData = new Dictionary<string, long>();
        var currentYearData = new Dictionary<string, long>();

        var result = new List<ChartIndex>();

        // 今年数据及同比计算
        foreach (var month in currentYearLastThreeMonths)
        {
            long value = 0;
            var monthKey = month.ToString("yyyy-MM");
            var accountDate = await GetAccountByDate(month);
            if (accountDate?.StartDate == null || accountDate?.EndDate == null)
            {
                currentYearData[monthKey] = 0;
            }
            else
            {
                value = await GetInventoryUsableGroupBySkuByMonth(
                    accountDate.StartDate.Value,
                    accountDate.EndDate.Value,
                    priceMap
                );
                currentYearData[monthKey] = value;
            }
        }

        // 去年数据及同比计算
        foreach (var month in lastYearLastThreeMonths)
        {
            long value = 0;
            var monthKey = month.ToString("yyyy-MM");
            var accountDate = await GetAccountByDate(month);
            if (accountDate?.StartDate == null || accountDate?.EndDate == null)
            {
                lastYearData[monthKey] = 0;
            }
            else
            {
                value = await GetInventoryUsableGroupBySkuByMonth(
                    accountDate.StartDate.Value,
                    accountDate.EndDate.Value,
                    priceMap
                );
                lastYearData[monthKey] = value;
            }
        }

        // 计算同比：((今年累计 - 去年同期累计) / 去年同期累计) * 100
        foreach (var month in currentYearLastThreeMonths)
        {
            var mm = month.ToString("MM");       // "MM"
            var lyKey = $"{mm}";

            currentYearData.TryGetValue(month.Year + "-" + mm, out var curVal);
            lastYearData.TryGetValue(month.AddYears(-1).Year + "-" + mm, out var lyVal);

            double y;
            if (lyVal > 0)
            {
                var growth = (Convert.ToDouble(curVal - lyVal)) / Convert.ToDouble(lyVal) * Convert.ToDouble(100m); // decimal 计算百分比
                                                                                                                    // 你目前的 Yseries 是 long，只能取整；若要保留两位小数建议把 ChartIndex.Yseries 改为 decimal
                y = Math.Round(growth, 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                y = 0; // 无基数或为0时同比显示0，避免除零
            }

            result.Add(new ChartIndex
            {
                Xseries = mm,
                Yseries = y
            });
        }
        // 确保按照月份升序排序
        result = result.OrderBy(a => a.Xseries).ToList();

        return result;
    }
    #endregion

    #endregion
}
