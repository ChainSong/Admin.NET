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
using ServiceStack.Script;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static Aliyun.OSS.Model.SelectObjectRequestModel.OutputFormatModel;
using Utilities;
using Tea.Utils;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.QueryMarketingMemberCardOpenCardsResponse.Types.Card.Types;

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
    private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;
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
        SqlSugarRepository<WMSInventoryUsable> repInventoryUsable,
        SqlSugarRepository<WMSOrderAddress> repOrderAddress
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
        _repOrderAddress = repOrderAddress;
    }
    #endregion

    #region 下拉筛选条件数据

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
    public async Task<MonthVSLastChartOneOutput> GetChartsOneData(ChartsInput input)
    {
        MonthVSLastChartOneOutput monthVSLastOutput = new MonthVSLastChartOneOutput();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        ////月库存金额趋势图VS去年 当月没到关账日不展示数据
        //monthVSLastOutput.MonthVSLast = await GetMonthVSLast(input, priceMap);
        ////及时库存
        //monthVSLastOutput.TodayPriceOfInventory = await GetInventoryUsableGroupBySkuByToday(input, DateTime.Today, priceMap);
        ////当月库存出库产品总金额
        //monthVSLastOutput.CurrentMonthPriceOfOB = await GetCurrentOrderGroupBySKUByAmount(input, priceMap);
        ////库销比
        //monthVSLastOutput.CurrentMonthPriceReceiptVSOB = await GetInventoryToSellPercent(input, priceMap);
        ////月累计对比去年
        //monthVSLastOutput.MonthlyCumulativeAmountVSLast = await GetCumulativeAmountVSLast(input, priceMap);
        ////获取过去三个月累计库存
        //monthVSLastOutput.CumulativeAmountVSLastThreeMonth = await GetCumulativeAmountVSLastThreeMonth(input, priceMap);
        return monthVSLastOutput;
    }

    #region 屏二
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetChartsTwoData")]
    public async Task<MonthVSLastChartTwoOutput> GetChartsTwoData(ChartsInput input)
    {
        MonthVSLastChartTwoOutput outputs = new MonthVSLastChartTwoOutput();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
        //月出库金额趋势 vs 去年
        outputs.MonthlyObAmount = await GetMonthlyOBAmountVSLast(input, priceMap);
        //月出库金额趋势 vs 去年
        outputs.MonthlyObQty = await GetMonthlyOBQtyVSLast(input, priceMap);
        //YTD 累积出库产品Minor金额 (Top5)
        outputs.MonthlyCumulativeObAmount = await GetMonthlyCumulativeObAmount(input, priceMap);
        //月累计发货金额最高的省份
        outputs.MonthlyCumulativeShipmentAmount = await GetMonthlyCumulativeShipmentAmount(input, priceMap);
        //当年月份新增用户数量
        outputs.MonthlyNewUserTrend = await GetMonthlyNewUserTrend(input, priceMap);
        //当年月份新增用户数量
        outputs.MonthlyCumulativeNewUserTrend = await GetMonthlyCumulativeNewUserTrend(input, priceMap);
        return outputs;
    }
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
        try
        {
            //没选中省份 那就返回空list
            if (string.IsNullOrEmpty(input.OBProvince))
                return outputs;

            // 商品价格字典缓存
            var priceMap = await GetProductPriceMap();
            var orderData = await GetObList(input);

            if (orderData != null && orderData.Count > 0)
            {
                // 计算每个省份的不同供应商的总金额
                outputs.oBProvinceGroupbyProvince = await GetObProvinceDataGByCustomer(orderData, priceMap);
                // 计算每个省份的总金额
                outputs.oBProvince = await GetObDataGByProvince(orderData, priceMap);
            }
        }
        catch (Exception ex)
        {
            throw;
        }
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
        //测试性能
        var SQL = _repInventoryUsableSnapshot.AsQueryable()
            .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
            .Where(a => a.InventoryTime >= lastAccountDate.EndDate && a.InventoryTime <= Convert.ToDateTime(lastAccountDate.EndDate).AddDays(1).AddSeconds(-1))
            .GroupBy(a => new { a.CustomerId, a.CustomerName, a.SKU })
            .Select(a => new { a.CustomerId, a.CustomerName, a.SKU, Qty = SqlFunc.AggregateSum(a.Qty) }).ToSqlString();

        var lastMonthInventory = await _repInventoryUsableSnapshot.AsQueryable()
            .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
            .Where(a => a.InventoryTime >= lastAccountDate.EndDate && a.InventoryTime <= Convert.ToDateTime(lastAccountDate.EndDate).AddDays(1).AddSeconds(-1))
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
    /// 根据sku获取目标时间范围内出库总金额--账期内
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetCurrentOrderGroupBySKUByAmount")]
    public async Task<List<ChartIndex>> GetCurrentOrderGroupBySKUByAmount(ChartsInput input)
    {
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
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
            //.Take(input.Take)
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
        skuAmounts = skuAmounts.OrderByDescending(a => a.Yseries).ToList();
        return skuAmounts;
    }

    /// <summary>
    /// 根据sku获取目标时间范围内出库总金额--账期外
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetCurrentOrderGroupBySKU(ChartsInput input, DateTime StartDate, DateTime? EndDate, List<CustomerProductPriceMapping> priceMap)
    {
        // 3. 计算各SKU金额
        var skuAmounts = new List<ChartIndex>();

        // 2. 查询当月出库数据（按SKU分组）
        var skuData = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                      .Where(o => o.Id == d.OrderId &&
                                  o.OrderStatus == 99 &&
                                  o.CreationTime >= StartDate &&
                                  o.CreationTime <= EndDate)
                      .Any())
            .WhereIF(input.CustomerId.HasValue, d => d.CustomerId == input.CustomerId)
            .GroupBy(d => d.SKU)
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
            foreach (var item in priceMap)
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

            var sql = _repASNDetail.AsQueryable()
                    .Where(d => SqlFunc.Subqueryable<WMSASN>()
                    .Where(o => o.Id == d.ASNId &&
                    o.ASNStatus != 90 &&
                                            o.CreationTime >= date.Date && // 精确到当天
                                            o.CreationTime < date.Date.AddDays(1)) // 精确到当天
                                .Any()).ToSqlString();
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
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetInventoryUsableGroupBySkuByToday")]
    public async Task<List<ChartIndex>> GetInventoryUsableGroupBySkuByToday(ChartsInput input)
    {
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        DateTime today = new DateTime();
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
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

    /// 获取月的出库总金额
    /// </summary>
    /// <param name="today">当前日期</param>
    /// <param name="priceMap">商品价格字典</param>
    /// <returns>按天汇总的入库总金额</returns>
    private async Task<Dictionary<string, double>> GetOrderGroupMonthAmount(ChartsInput input, WMSHachAccountDate TargetData, List<CustomerProductPriceMapping> priceMap)
    {
        // 获取当天的入库数据
        var todayData = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId &&
                                    o.OrderStatus == 99 &&
                                    o.CreationTime >= TargetData.StartDate && // 精确到当天
                                    o.CreationTime < Convert.ToDateTime(TargetData.EndDate).AddDays(1).AddSeconds(-1)) // 精确到当天
                        .Any())
            .WhereIF(input.CustomerId.HasValue, d => d.CustomerId == input.CustomerId)
            .ToListAsync();

        // 计算当天的总金额
        var totalAmount = CalculateTotalAmount(
            todayData.Select(x => (x.CustomerId.GetValueOrDefault(), x.CustomerName, x.SKU, (double)x.AllocatedQty)),
            priceMap
        );

        // 正确返回Dictionary
        return new Dictionary<string, double>
               {
                   { Convert.ToDateTime(TargetData.StartDate).ToString("yyyy-MM"), totalAmount }
               };
    }

    /// <summary>
    /// 月出库数量趋势 vs 去年
    /// </summary>
    /// <param name="TargetData"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<Dictionary<string, double>> GetOrderGroupMonthQty(ChartsInput input, WMSHachAccountDate TargetData, List<CustomerProductPriceMapping> priceMap)
    {
        // 获取当天的入库数据
        var TotalQty = await _repOrderDetail.AsQueryable()
            .Where(d => SqlFunc.Subqueryable<WMSOrder>()
                        .Where(o => o.Id == d.OrderId &&
                                    o.OrderStatus == 99 &&
                                    o.CreationTime >= TargetData.StartDate && // 精确到当天
                                    o.CreationTime < Convert.ToDateTime(TargetData.EndDate).AddDays(1).AddSeconds(-1)) // 精确到当天
                        .Any())
            .WhereIF(input.CustomerId.HasValue, d => d.CustomerId == input.CustomerId)
            .SumAsync(d => d.OrderQty);

        return new Dictionary<string, double>
               {
                   { Convert.ToDateTime(TargetData.StartDate).ToString("yyyy-MM"), TotalQty }
               };
    }
    /// <summary>
    /// 生成指定月份的所有日期数组
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public DateTime[] GetAllDatesInMonth(int year, int month)
    {
        // 获取该月的总天数（自动处理闰年）
        int daysInMonth = DateTime.DaysInMonth(year, month);

        // 使用 LINQ 生成从 1 到 daysInMonth 的所有日期
        return Enumerable.Range(1, daysInMonth)
                        .Select(day => new DateTime(year, month, day))
                        .ToArray();
    }
    #endregion

    #region 大屏一
    /// <summary>
    /// 月库存金额趋势图VS去年
    /// </summary>
    /// <returns></returns>
    /// 当月没到关账日不展示数据
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthVSLast")]
    public async Task<MonthVSLast> GetMonthVSLast(ChartsInput input)
    {
        MonthVSLast monthVSLastOutput = new MonthVSLast();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();
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
    /// 库销比
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetInventoryToSellPercent")]
    public async Task<List<ChartIndex>> GetInventoryToSellPercent(ChartsInput input)
    {
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();

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
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetCumulativeAmountVSLast")]
    public async Task<List<ChartIndex>> GetCumulativeAmountVSLast(ChartsInput input)
    {
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();

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
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetCumulativeAmountVSLastThreeMonth")]
    public async Task<List<ChartIndex>> GetCumulativeAmountVSLastThreeMonth(ChartsInput input)
    {
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        // 商品价格字典缓存
        var priceMap = await GetProductPriceMap();

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

    #region 大屏二
    /// <summary>
    /// 月出库金额趋势 vs 去年
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<MonthVSLast> GetMonthlyOBAmountVSLast(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        MonthVSLast monthVSLast = new MonthVSLast();
        var TargetDate = Convert.ToDateTime(input.Month);
        var currentMonth = new DateTime(TargetDate.Year, TargetDate.Month, 1);
        var lastYear = TargetDate.Year - 1;

        var currentYearMonths = GetPastMonthsExcludingCurrent(TargetDate);

        var lastYearMonths = currentYearMonths
            .Select(m => Convert.ToDateTime(m).AddYears(-1))
            .ToList();

        List<ChartIndex> CurrentYearList = new List<ChartIndex>();
        //获取今年目标月份数据
        foreach (var month in currentYearMonths)
        {
            var monthDate = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);
            if (accountDate == null)
            {
                CurrentYearList.Add(new ChartIndex
                {
                    Xseries = month,
                    Yseries = 0
                });
            }
            else
            {
                Dictionary<string, double> currentMonthData = await GetOrderGroupMonthAmount(input, accountDate, priceMap);
                if (currentMonthData.TryGetValue(month, out double value))
                {
                    CurrentYearList.Add(new ChartIndex
                    {
                        Xseries = month,
                        Yseries = value // 安全获取的 double 值
                    });
                }
            }
        }

        List<ChartIndex> LastYearList = new List<ChartIndex>();
        //获取今年目标月份数据
        foreach (var month in lastYearMonths)
        {
            var monthDate = DateTime.ParseExact(month.ToString("yyyy-MM"), "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);
            if (accountDate == null)
            {
                LastYearList.Add(new ChartIndex
                {
                    Xseries = month.ToString("yyyy-MM"),
                    Yseries = 0
                });
            }
            else
            {
                Dictionary<string, double> LastYearData = await GetOrderGroupMonthAmount(input, accountDate, priceMap);
                if (LastYearData.TryGetValue(month.ToString("yyyy-MM"), out double value))
                {
                    LastYearList.Add(new ChartIndex
                    {
                        Xseries = month.ToString("yyyy-MM"),
                        Yseries = value // 安全获取的 double 值
                    });
                }
            }
        }
        monthVSLast.CurrentYear = CurrentYearList;
        monthVSLast.LastYear = LastYearList;
        return monthVSLast;
    }

    /// <summary>
    /// 月出库数量趋势 vs 去年
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<MonthVSLast> GetMonthlyOBQtyVSLast(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        MonthVSLast monthVSLast = new MonthVSLast();
        var TargetDate = Convert.ToDateTime(input.Month);
        var currentMonth = new DateTime(TargetDate.Year, TargetDate.Month, 1);
        var lastYear = TargetDate.Year - 1;

        var currentYearMonths = GetPastMonthsExcludingCurrent(TargetDate);

        var lastYearMonths = currentYearMonths
            .Select(m => Convert.ToDateTime(m).AddYears(-1))
            .ToList();

        List<ChartIndex> CurrentYearList = new List<ChartIndex>();
        //获取今年目标月份数据
        foreach (var month in currentYearMonths)
        {
            var monthDate = DateTime.ParseExact(month, "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);
            if (accountDate == null)
            {
                CurrentYearList.Add(new ChartIndex
                {
                    Xseries = month,
                    Yseries = 0
                });
            }
            else
            {
                Dictionary<string, double> currentMonthData = await GetOrderGroupMonthQty(input, accountDate, priceMap);
                if (currentMonthData.TryGetValue(month, out double value))
                {
                    CurrentYearList.Add(new ChartIndex
                    {
                        Xseries = month,
                        Yseries = value // 安全获取的 double 值
                    });
                }
                else
                {
                    CurrentYearList.Add(new ChartIndex
                    {
                        Xseries = month,
                        Yseries = 0
                    });
                }
            }
        }

        List<ChartIndex> LastYearList = new List<ChartIndex>();
        //获取上一年年目标月份数据
        foreach (var month in lastYearMonths)
        {
            var monthDate = DateTime.ParseExact(month.ToString("yyyy-MM"), "yyyy-MM", CultureInfo.InvariantCulture);
            var accountDate = await GetAccountByDate(monthDate);
            if (accountDate == null)
            {
                LastYearList.Add(new ChartIndex
                {
                    Xseries = month.ToString("yyyy-MM"),
                    Yseries = 0
                });
            }
            else
            {
                Dictionary<string, double> LastYearData = await GetOrderGroupMonthQty(input, accountDate, priceMap);
                if (LastYearData.TryGetValue(month.ToString("yyyy-MM"), out double value))
                {
                    LastYearList.Add(new ChartIndex
                    {
                        Xseries = month.ToString("yyyy-MM"),
                        Yseries = value // 安全获取的 double 值
                    });
                }
                else
                {
                    LastYearList.Add(new ChartIndex
                    {
                        Xseries = month.ToString("yyyy-MM"),
                        Yseries = 0
                    });
                }
            }
        }
        monthVSLast.CurrentYear = CurrentYearList;
        monthVSLast.LastYear = LastYearList;
        return monthVSLast;
    }

    /// <summary>
    /// YTD 累积出库产品Minor金额 (Top5)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<MonthVSLast> GetMonthlyCumulativeObAmount(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        MonthVSLast outputs = new MonthVSLast();

        // 获取当前年份的第一天 00:00:00
        DateTime firstDay = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0);
        // 获取当前月份的天数（自动处理闰年）
        int days = input.Month.HasValue ? DateTime.DaysInMonth(Convert.ToDateTime(input.Month).Year, Convert.ToDateTime(input.Month).Month) : DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); ;
        // 获取当前年份的最后一天 23:59:59
        DateTime lastDay = input.Month.HasValue ? new DateTime(Convert.ToDateTime(input.Month).Year, Convert.ToDateTime(input.Month).Month, days, 31, 23, 59, 59) : new DateTime(DateTime.Now.Year, 12, 31, 23, 59, 59);


        outputs.CurrentYear = await GetCurrentOrderGroupBySKU(input, firstDay, lastDay, priceMap);
        outputs.LastYear = await GetCurrentOrderGroupBySKU(input, firstDay.AddYears(-1), lastDay.AddYears(-1), priceMap);

        return outputs;
    }

    /// <summary>xx
    /// 月累计发货金额最高的省份 饼图  top5 and else 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetMonthlyCumulativeShipmentAmount(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        List<ChartIndex> chartIndices = new List<ChartIndex>();
        var orderData = await GetObList(input);
        var orderDataByProvince = await GetObDataGByProvince(orderData, priceMap);
        orderDataByProvince = orderDataByProvince.OrderByDescending(a => a.Amount).ToList();

        // 获取前 5 条数据
        var top5 = orderDataByProvince.Take(5).ToList();

        // 将前 5 条数据添加到返回结果中
        foreach (var item in top5)
        {
            chartIndices.Add(new ChartIndex
            {
                Xseries = item.ObProvince,  // 省份名
                Yseries = item.Amount      // 对应的金额
            });
        }

        // 计算剩余数据的总和并归类为 "其它"
        var others = orderDataByProvince.Skip(5).ToList();
        var totalAmountForOthers = others.Sum(a => a.Amount);

        // 将 "其它" 的数据加入列表
        if (totalAmountForOthers > 0)
        {
            chartIndices.Add(new ChartIndex
            {
                Xseries = "其它",  // 归类为 "其它"
                Yseries = totalAmountForOthers
            });
        }

        return chartIndices;
    }
    /// <summary>
    /// 当年所有月份新增用户数量 根据省份来分组
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    /// 如果1月份的用户 在2月或者后面的月份出现了 那么后面的月份的count减去对应的用户数量
    private async Task<List<ChartIndex>> GetMonthlyNewUserTrend(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        List<ChartIndex> chartIndices = new List<ChartIndex>();
        DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);

        // 如果传入的月份有值，则使用传入月份的第一天
        if (input.Month.HasValue)
        {
            firstDayOfMonth = Convert.ToDateTime(input.Month);
        }

        // 生成当年每个月的第一天的日期
        var dateList = Enumerable.Range(1, 12) // 1~12
                     .Select(month => new DateTime(firstDayOfMonth.Year, month, 1))
                     .ToArray();

        // 用于存储每个月的新增用户数量（按省份统计）
        Dictionary<string, Dictionary<string, double>> userCountByProvincePerMonth = new Dictionary<string, Dictionary<string, double>>();

        // 记录每个用户的首次出现月份（UserId -> FirstMonth）
        Dictionary<string, int> userFirstMonth = new Dictionary<string, int>();

        // 遍历每个月份，获取每月新增用户
        var addressDataTaskList = dateList.Select(async item =>
        {
            var addressData = await _repOrderAddress.AsQueryable()
                .Where(a => a.CreationTime >= item && a.CreationTime < item.AddMonths(1))
                .ToListAsync();

            var monthlyUserCountByProvince = new Dictionary<string, double>();

            foreach (var address in addressData)
            {
                var userId = address.Name;
                var province = address.Province;

                if (!userFirstMonth.ContainsKey(userId)) // 新用户
                {
                    userFirstMonth[userId] = item.Month;

                    if (!monthlyUserCountByProvince.ContainsKey(province))
                    {
                        monthlyUserCountByProvince[province] = 0;
                    }
                    monthlyUserCountByProvince[province]++;
                }
                else // 已出现过的用户
                {
                    var firstMonth = userFirstMonth[userId];
                    if (firstMonth < item.Month) // 用户首次出现在之前的某个月份
                    {
                        continue;  // 跳过重复用户
                    }
                }
            }

            // 将该月按省份统计的用户数存入字典
            userCountByProvincePerMonth[item.ToString("yyyy-MM")] = monthlyUserCountByProvince;
        }).ToList();

        // 等待所有异步任务完成
        await Task.WhenAll(addressDataTaskList);

        // 聚合结果，按月份返回新增用户数量，不需要省份
        var monthUserCount = userCountByProvincePerMonth
            .ToDictionary(month => month.Key, month => month.Value.Values.Sum());

        // 设置最终结果集
        chartIndices.AddRange(monthUserCount.Select(month =>
            new ChartIndex
            {
                Xseries = month.Key,  // 取月份（键）
                Yseries = month.Value // 取新增用户数量（值）
            })
        );

        return chartIndices;
    }
    /// <summary>
    /// 当年月份累计新增用户数量 根据省份来分组
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    /// 如果1月份的用户 在2月或者后面的月份出现了 那么后面的月份的count减去对应的用户数量
    ///但是数据是 1月 跟前面的对比  然后是1月跟2月 跟前面的对比 然后是1月跟3月跟前面的对比 以此类推
    private async Task<List<ChartIndex>> GetMonthlyCumulativeNewUserTrend(ChartsInput input, List<CustomerProductPriceMapping> priceMap)
    {
        List<ChartIndex> chartIndices = new List<ChartIndex>();
        DateTime firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);

        // 如果传入的月份有值，则使用传入月份的第一天
        if (input.Month.HasValue)
        {
            firstDayOfMonth = Convert.ToDateTime(input.Month);
        }

        // 生成当年每个月的第一天的日期
        var dateList = Enumerable.Range(1, 12) // 1~12
                     .Select(month => new DateTime(firstDayOfMonth.Year, month, 1))
                     .ToArray();

        // 用于存储每个月的新增用户数量（按省份统计）
        Dictionary<string, Dictionary<string, double>> userCountByProvincePerMonth = new Dictionary<string, Dictionary<string, double>>();

        // 记录每个用户的首次出现月份（UserId -> FirstMonth）
        Dictionary<string, int> userFirstMonth = new Dictionary<string, int>();

        // 遍历每个月份，获取每月新增用户
        var addressDataTaskList = dateList.Select(async item =>
        {
            var addressData = await _repOrderAddress.AsQueryable()
                .Where(a => a.CreationTime >= item && a.CreationTime < item.AddMonths(1))
                .ToListAsync();

            var monthlyUserCountByProvince = new Dictionary<string, double>();

            foreach (var address in addressData)
            {
                var userId = address.Name;
                var province = address.Province;

                if (!userFirstMonth.ContainsKey(userId)) // 新用户
                {
                    userFirstMonth[userId] = item.Month;

                    if (!monthlyUserCountByProvince.ContainsKey(province))
                    {
                        monthlyUserCountByProvince[province] = 0;
                    }
                    monthlyUserCountByProvince[province]++;
                }
                else // 已出现过的用户
                {
                    var firstMonth = userFirstMonth[userId];
                    if (firstMonth < item.Month) // 用户首次出现在之前的某个月份
                    {
                        continue;  // 跳过重复用户
                    }
                }
            }

            // 将该月按省份统计的用户数存入字典
            userCountByProvincePerMonth[item.ToString("yyyy-MM")] = monthlyUserCountByProvince;
        }).ToList();

        // 等待所有异步任务完成
        await Task.WhenAll(addressDataTaskList);

        // 累计每个月的用户数量
        Dictionary<string, double> cumulativeMonthUserCount = new Dictionary<string, double>();
        double cumulativeUserCount = 0;

        foreach (var month in userCountByProvincePerMonth)
        {
            cumulativeUserCount += month.Value.Values.Sum(); // 累计每月新增用户数量
            cumulativeMonthUserCount[month.Key] = cumulativeUserCount; // 按月记录累计的用户数
        }

        // 设置最终结果集
        chartIndices.AddRange(cumulativeMonthUserCount.Select(month =>
            new ChartIndex
            {
                Xseries = month.Key,  // 取月份（键）
                Yseries = month.Value // 取累计新增用户数量（值）
            })
        );

        return chartIndices;
    }
    #endregion

    #region 大屏三
    /// <summary>
    /// 获取出库数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<OBProvinceList>> GetObList(ChartsInput input)
    {
        var orderData = await _repOrder.AsQueryable()
             .LeftJoin<WMSOrderAddress>((o, oa) => o.PreOrderId == oa.PreOrderId)
             .LeftJoin<WMSOrderDetail>((o, oa, od) => o.Id == od.OrderId)
             .Where((o, oa, od) => o.OrderStatus == 99)
             .WhereIF(input.Month.HasValue, (o, oa, od) => o.CreationTime >= DateTime.Parse(input.Month + "-01"))
             .WhereIF(input.Month.HasValue, (o, oa, od) => o.CreationTime <= DateTime.Parse(input.Month + "-01").AddMonths(1).AddTicks(-1))
             .WhereIF(input.CustomerId.HasValue, (o, oa, od) => o.CustomerId == input.CustomerId)
             .WhereIF(!string.IsNullOrEmpty(input.OBProvince), (o, oa, od) => oa.Province.Contains(input.OBProvince))
             .GroupBy((o, oa, od) => new { oa.Province, od.SKU, o.CustomerName, o.CustomerId })
             .Select((o, oa, od) => new OBProvinceList
             {
                 ObProvince = oa.Province,
                 Customer = o.CustomerName,
                 CustomerId = o.CustomerId,
                 Sku = od.SKU,
                 Qty = SqlFunc.AggregateSum(od.OrderQty),
             })
             .ToListAsync();
        return orderData;
    }

    /// <summary>
    /// 根据出库省份获取总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<List<OBProvince>> GetObDataGByProvince(List<OBProvinceList> data, List<CustomerProductPriceMapping> priceMap)
    {
        List<OBProvince> oBProvinces = new List<OBProvince>();
        var provinceGroupss = data.GroupBy(x => new { x.ObProvince });
        foreach (var provinceGroup in provinceGroupss)
        {
            oBProvinces.Add(new OBProvince
            {
                ObProvince = provinceGroup.Key.ObProvince,
                Qty = provinceGroup.Sum(x => x.Qty),
                Amount = CalculateTotalAmount(
                    provinceGroup.Select(x => (x.CustomerId.GetValueOrDefault(), x.Customer, x.Sku, x.Qty.GetValueOrDefault())),
                    priceMap),
            });
        }
        return oBProvinces;
    }

    /// <summary>
    /// 根据出库省份获取总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<List<OBProvinceGroupbyWhere>> GetObProvinceDataGByCustomer(List<OBProvinceList> data, List<CustomerProductPriceMapping> priceMap)
    {
        List<OBProvinceGroupbyWhere> oBProvinces = new List<OBProvinceGroupbyWhere>();
        var provinceGroups = data.GroupBy(x => x.ObProvince);
        foreach (var provinceGroup in provinceGroups)
        {
            oBProvinces.Add(new OBProvinceGroupbyWhere
            {
                ObProvince = provinceGroup.Key,
                Qty = provinceGroup.Sum(x => x.Qty),
                Amount = CalculateTotalAmount(
                provinceGroup.Select(x => (x.CustomerId.GetValueOrDefault(), x.Customer, x.Sku, x.Qty.GetValueOrDefault())),
                priceMap),
                Customer = provinceGroup.Select(x => x.Customer).FirstOrDefault(),
                CustomerId = provinceGroup.Select(x => x.CustomerId).FirstOrDefault(),
            });
        }
        return oBProvinces;
    }
    #endregion


    #endregion
}
