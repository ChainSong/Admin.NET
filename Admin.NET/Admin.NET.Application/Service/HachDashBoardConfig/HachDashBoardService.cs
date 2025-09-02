// 麻省理工学院许可证
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Const;
using Admin.NET.Application.Service.HachDashBoardConfig.Dto;
using Admin.NET.Common.AMap;
using Admin.NET.Common.AMap.Response;
using Admin.NET.Common.LocalLog;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Minio.DataModel;
using MongoDB.Driver.Linq;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ServiceStack;
using ServiceStack.Messaging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using static Aliyun.OSS.Model.SelectObjectRequestModel.OutputFormatModel;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ScanProductAddV2Request.Types.Product.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaSecOrderUploadCombinedShippingInfoRequest.Types.SubOrder.Types;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.CreateRefundDomesticRefundRequest.Types.Amount.Types;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.QueryMarketingMemberCardOpenCardsResponse.Types.Card.Types;
namespace Admin.NET.Application;
/// <summary>
/// HACH大屏
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1)]

public class HachDashBoardService : IDynamicApiController, ITransient
{

    #region 注入依赖
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
    private readonly SqlSugarRepository<WMSHachCustomerMapping> _repHachCustomerMapping;
    public static string logFilePath = @"C:\HachLogs\DashBoard_Logs\OrderDashBoard.log";
    public HachDashBoardService( 
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
        SqlSugarRepository<WMSOrderAddress> repOrderAddress,
        SqlSugarRepository<WMSHachCustomerMapping> repHachCustomerMapping
        )
    {
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
        _repHachCustomerMapping = repHachCustomerMapping;
    }
    #endregion

    LocalLog Logger = new LocalLog();

    #region 下拉筛选条件数据

    #region 获取物料下拉列表
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "SelectProductList")]
    public async Task<List<SelectItem>> SelectProductList(List<long> CustomerIds)
    {
        try
        {
            var customerIdStr = string.Empty;
            if (CustomerIds != null && CustomerIds.Count > 0)
            {
                customerIdStr = string.Join(",", CustomerIds);
            }
            string Sql = "SELECT  [Id] AS [Id] , [SKU] AS [Value] , [GoodsName] AS [Label]  FROM [WMS_Product] (nolock) " +
                " WHERE ( [ProductStatus] = 1 )  AND  ([CustomerId] IN (" + customerIdStr + ")) ";
            return _repProduct.Context.Ado.GetDataTable(Sql).TableToList<SelectItem>();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion

    #region 获取客户下拉列表
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "SelectCustomerList")]
    public async Task<List<SelectItem>> SelectCustomerList(List<long?> CustomerIds)
    {
        string sqlWhereSql = string.Empty;

        if (CustomerIds != null && CustomerIds.Count > 0)
        {
            sqlWhereSql = "and customerid in (" + string.Join(",", CustomerIds) + ")";
        }
        string Sql = "select Id,CustomerId as Value,CustomerName as Label from WMS_Hach_Customer_Mapping" +
            " where type='HachDashBoard' " + sqlWhereSql + "";
        return _repHachCustomerMapping.Context.Ado.GetDataTable(Sql).TableToList<SelectItem>();
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

    #region  汇总Tab项数据

    /// <summary>
    /// 汇总Tab项数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetSumItemData")]
    public async Task<SumItemOutput> GetSumItemData(ChartsInput input)
    {
        SumItemOutput itemOutput = new SumItemOutput();

        #region 上个月库存金额
        try
        {
            itemOutput.LastMonthAmount = await GetInventoryUsableSnapshotByAccountDate(input);
        }
        catch (Exception ex)
        {
            itemOutput.LastMonthAmount = 0;
        }
        #endregion

        #region 目前库存金额
        try
        {
            itemOutput.CurrentMonthAmount = await GetInventoryUsableByToday(input);
        }
        catch (Exception ex)
        {
            itemOutput.CurrentMonthAmount = 0;
        }
        #endregion

        #region 库存目标金额
        try
        {
            itemOutput.CurrentTargetAmount = await GetTargetAmountByMonth(input);
        }
        catch (Exception ex)
        {
            itemOutput.CurrentTargetAmount = 0;
        }
        #endregion 

        //当前库存金额-库存目标金额
        itemOutput.CMonthVSTargetAmount = itemOutput.CurrentMonthAmount - itemOutput.CurrentTargetAmount;

        #region 当月入库总金额
        try
        {
            itemOutput.CurrentReceiptAmount = await GetCurrentReceiptAmount(input);
        }
        catch (Exception ex)
        {
            itemOutput.CurrentReceiptAmount = 0;
        }
        #endregion

        #region 当月出库总金额
        try
        {
            itemOutput.CurrentOrderAmount = await GetCurrentOrderAmount(input);
        }
        catch (Exception ex)
        {
            itemOutput.CurrentOrderAmount = 0;
        }
        #endregion

        //YTD出库 / YTD入库
        itemOutput.YTDOrderVSASNAmount = itemOutput.CurrentReceiptAmount == 0 ? 0 : (float)Math.Round((decimal)(itemOutput.CurrentOrderAmount / itemOutput.CurrentReceiptAmount) * 100, 3);

        return itemOutput;
    }

    #region 大屏汇总
    /// <summary>
    /// 获取月份 获取 库存快照表数据
    /// 汇总tab项 第一个label
    /// </summary>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<double?> GetInventoryUsableSnapshotByAccountDate(ChartsInput input)
    {
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }
        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
        string lastMonthString = targetDate.AddMonths(-1).ToString("yyyyMM");
        //查询目标日期上个月的库存信息
        string Sql = " SELECT SUM(i.[Qty] * p.[Price])FROM [WMS_Inventory_Usable_Snapshot] i WITH (NOLOCK) " +
                    " INNER JOIN [wms_product] p WITH (NOLOCK) ON i.[CustomerId] = p.[CustomerId]  AND i.[SKU] = p.[SKU] " +
                    " WHERE 1=1  AND  CONVERT(VARCHAR(10),i.[InventorySnapshotTime], 120) = ( select CONVERT(VARCHAR(10),EndDate, 120)  from WMS_HachAccountDate  " +
                    " where AccountDate =  '" + lastMonthString + "' )" +
                    " AND i.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")";
        try
        {
            var result = await _repInventoryUsableSnapshot.Context.Ado
                .GetScalarAsync(Sql);
            // 更安全的null检查和类型转换
            if (result == null || result == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToDouble(result);
        }
        catch (Exception ex)
        {
            Logger.LogMessage("查询大屏汇总1报错" + ex.Message, logFilePath);
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }
    }

    /// <summary>
    /// 获取当天 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<double?> GetInventoryUsableByToday(ChartsInput input)
    {
        try
        {

            #region 当月查询库存表数据
            //如果月份没有值 就是默认就是当天 如果有值并且是当月  那就查询库存表的数据
            if (!input.Month.HasValue || (input.Month.HasValue && Convert.ToDateTime(input.Month).ToString("yyyy-MM").Equals(Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM"))))
            {
                var sqlWhereSql = string.Empty;
                if (input.CustomerId.HasValue && input.CustomerId > 0)
                {
                    sqlWhereSql = "and customerId = " + input.CustomerId + "";
                }

                string query = "SELECT SUM(i.[Qty] * p.[Price]) AS [GrandTotalValue] FROM  [WMS_Inventory_Usable] i WITH (NOLOCK)" +
                     "INNER JOIN [wms_product] p WITH (NOLOCK)   ON i.[CustomerId] = p.[CustomerId]   AND i.[SKU] = p.[SKU]" +
                     "WHERE  i.[InventoryStatus] = 1 AND i.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping " +
                     "WHERE type='HachDashBoard' " + sqlWhereSql + ")";
                try
                {
                    var result = await _repInventoryUsable.Context.Ado
                        .GetScalarAsync(query);
                    // 更安全的null检查和类型转换
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToDouble(result);
                }
                catch (Exception ex)
                {
                    throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
                }
            }
            #endregion

            #region 历史月份查询目标月份关账日期的结束日
            else
            {
                var sqlWhereSql = string.Empty;
                if (input.CustomerId.HasValue && input.CustomerId > 0)
                {
                    sqlWhereSql = "and customerId = " + input.CustomerId + "";
                }
                //获取历史月份关账日的库存快照数据
                string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
                DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
                string lastMonthString = targetDate.ToString("yyyyMM");
                string Sql = " SELECT SUM(i.[Qty] * p.[Price])FROM [WMS_Inventory_Usable_Snapshot] i WITH (NOLOCK) " +
                            " INNER JOIN [wms_product] p WITH (NOLOCK) ON i.[CustomerId] = p.[CustomerId]  AND i.[SKU] = p.[SKU] " +
                            " WHERE 1=1  AND  CONVERT(VARCHAR(10),i.[InventorySnapshotTime], 120) = ( select CONVERT(VARCHAR(10),EndDate, 120)  from WMS_HachAccountDate  " +
                            " where AccountDate  =  '" + lastMonthString + "' )  " +
                            " AND i.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")";
                try
                {
                    var result = await _repInventoryUsableSnapshot.Context.Ado
                        .GetScalarAsync(Sql);
                    // 更安全的null检查和类型转换
                    if (result == null || result == DBNull.Value)
                    {
                        return 0;
                    }
                    return Convert.ToDouble(result);
                }
                catch (Exception ex)
                {
                    throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 获取当月库存目标金额
    /// </summary>
    /// <param name="currentMonthString"></param>
    /// <returns></returns>
    private async Task<double?> GetTargetAmountByMonth(ChartsInput input)
    {

        var sqlWhereSql = string.Empty;

        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        string query = " SELECT  SUM( CAST([PlanKRMB] AS MONEY)) AS [PlanKRMB]  FROM [WMS_HachTagretKRMB] " +
                       " WHERE 1=1 AND CONVERT(VARCHAR(7), TRY_CONVERT(DATE, [Month] + '-01'), 120) = '" + (input.Month.HasValue ? input.Month.Value.ToString("yyyy-MM") : DateTime.Today.ToString("yyyy-MM")) + "' " +
                       "AND  customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard'  " + sqlWhereSql + ")";
        try
        {
            var result = await _repHachTagretKRMB.Context.Ado
                .GetScalarAsync(query);
            // 更安全的null检查和类型转换
            if (result == null || result == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToDouble(result);
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }
    }

    /// <summary>
    /// 获取当月入库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double?> GetCurrentReceiptAmount(ChartsInput input)
    {
        try
        {
            double? Amount = 0;
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and customerId = " + input.CustomerId + "";
            }

            string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
            DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
            string lastMonthString = targetDate.ToString("yyyyMM");

            string query = "SELECT SUM(d.[ReceivedQty] * p.[Price]) AS [TotalAmount]  FROM [WMS_ASNDetail] d WITH (NOLOCK) " +
                "INNER JOIN [wms_product] p WITH (NOLOCK)  ON d.[CustomerId] = p.[CustomerId]  AND d.[SKU] = p.[SKU] " +
                "INNER JOIN [WMS_ASN] o WITH (NOLOCK)  ON o.[Id] = d.[ASNId] AND o.[ASNStatus] <> 90  " +
                "INNER JOIN WMS_HachAccountDate h WITH (NOLOCK) ON AccountDate = '" + lastMonthString + "' " +
                "AND o.[CreationTime] >= h.StartDate AND o.[CreationTime] <= h.EndDate " +
                "WHERE 1=1 AND d.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")";
            try
            {
                var result = await _repASNDetail.Context.Ado
                    .GetScalarAsync(query);
                // 更安全的null检查和类型转换
                if (result == null || result == DBNull.Value)
                {

                    return 0;
                }
                Amount = Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
            }
            return Amount;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 获取当月出库总金额
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    private async Task<double?> GetCurrentOrderAmount(ChartsInput input)
    {
        try
        {
            double? Amount = 0;
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and customerId = " + input.CustomerId + "";
            }
            string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
            DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
            string lastMonthString = targetDate.ToString("yyyyMM");
            string query = " SELECT SUM(od.OrderQty * p.[Price]) AS [TotalAmount] " +
                " FROM WMS_OrderDetail od WITH (NOLOCK)  " +
                " INNER JOIN [wms_product] p WITH (NOLOCK)  ON od.[CustomerId] = p.[CustomerId]  AND od.[SKU] = p.[SKU] " +
                " INNER JOIN WMS_Order o WITH (NOLOCK)  ON o.[Id] = od.OrderId  AND o.OrderStatus = 99 " +
                "INNER JOIN WMS_HachAccountDate h WITH (NOLOCK)  ON AccountDate = '" + lastMonthString + "' " +
                "AND o.[CreationTime] >= h.StartDate  AND o.[CreationTime] <= h.EndDate  " +
                "WHERE 1=1 AND o.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")";

            try
            {
                var result = await _repOrderDetail.Context.Ado
                    .GetScalarAsync(query);
                // 更安全的null检查和类型转换
                if (result == null || result == DBNull.Value)
                {
                    return 0;
                }
                Amount = Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
            }
            return Amount;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion

    #endregion

    #region 大屏一

    /// <summary>
    /// 目标月上一月库存金额趋势图VS去年
    /// </summary>
    /// <returns></returns>
    /// 当月没到关账日不展示数据
    #region 屏幕一 上面第一张折线图
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
        //只查询到上个月
        input.Month = Convert.ToDateTime(input.Month).AddMonths(-1);
        monthVSLastOutput.CurrentYear = await GetInventoryUsableSnapshotListByTargetDate(input);
        //查去年
        input.Month = Convert.ToDateTime(input.Month).AddYears(-1);
        monthVSLastOutput.LastYear = await GetInventoryUsableSnapshotListByTargetDate(input);
        return monthVSLastOutput;
    }

    /// <summary>
    /// 根据目标时间区间获取数据
    /// </summary>
    /// <param name="input"></param>
    /// <param name="lastAccountDate"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetInventoryUsableSnapshotListByTargetDate(ChartsInput input)
    {
        List<ChartIndex> result = new List<ChartIndex>();
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);

        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = targetDate.Year + "01";
        string lastMonthString = targetDate.ToString("yyyyMM");

        string query = " WITH Periods AS (SELECT h.AccountDate, h.StartDate, h.EndDate" +
                       " FROM dbo.WMS_HachAccountDate AS h WITH(NOLOCK)" +
                       " WHERE h.AccountDate BETWEEN '" + startMonthString + "' AND '" + lastMonthString + "')," +
                       " MonthlyData AS(SELECT pds.AccountDate," +
                       " SUM(i.Qty* pr.Price) AS MonthlyAmount FROM dbo.WMS_Inventory_Usable_Snapshot AS i WITH(NOLOCK)" +
                       " INNER JOIN Periods AS pds ON CAST(i.InventorySnapshotTime AS DATE) = CAST(pds.EndDate AS DATE)" +
                       " INNER JOIN dbo.WMS_Product AS pr WITH(NOLOCK) ON i.CustomerId = pr.CustomerId AND i.SKU = pr.SKU" +
                       " WHERE i.CustomerId IN(" +
                       " SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WITH (NOLOCK) WHERE cm.[Type] = 'HachDashBoard' "+sqlWhereSql+")" +
                       " GROUP BY pds.AccountDate)" +
                       " SELECT right(CAST(p.AccountDate AS VARCHAR(6)),2) AS Xseries," +
                       " COALESCE(md.MonthlyAmount, 0) AS Yseries FROM Periods p" +
                       " LEFT JOIN MonthlyData md ON md.AccountDate = p.AccountDate ORDER BY p.AccountDate;";
        try
        {
            result = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(query).TableToList<ChartIndex>();
            // 转换月份数字为英文名称
            foreach (var item in result)
            {
                item.Xseries = ConvertMonthNumberToName(item.Xseries);
            }
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }

        return result;
    }
    #endregion

    /// <summary>
    /// 获取当天 获取 库存表数据
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="today"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    #region 屏幕一  上面第二张图柱状图
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetInventoryUsableGroupBySkuByToday")]
    public async Task<List<ChartIndex>> GetInventoryUsableGroupBySkuByToday(ChartsInput input)
    {
        List<ChartIndex> result = new List<ChartIndex>();
        List<ChartIndex> Finallyresult = new List<ChartIndex>();

        try
        {
            #region 当月查询库存表数据
            //如果月份没有值 就是默认就是当天 如果有值并且是当月  那就查询库存表的数据
            if (!input.Month.HasValue || (input.Month.HasValue && Convert.ToDateTime(input.Month).ToString("yyyy-MM").Equals(Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM"))))
            {
                var sqlWhereSql = string.Empty;
                if (input.CustomerId.HasValue && input.CustomerId > 0)
                {
                    sqlWhereSql = "and customerId = " + input.CustomerId + "";
                }
                string sql = "SELECT COALESCE(NULLIF(p.[str2], ''),i.sku) as Xseries,SUM(i.[Qty] * p.[Price]) AS Yseries FROM  [WMS_Inventory_Usable] i WITH (NOLOCK)" +
                               " INNER JOIN [wms_product] p WITH (NOLOCK)   ON i.[CustomerId] = p.[CustomerId]   AND i.[SKU] = p.[SKU]" +
                               " WHERE  i.[InventoryStatus] = 1 AND i.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")" +
                               " group by COALESCE(NULLIF(p.[str2], ''),i.sku)  order by Yseries desc";
                try
                {
                    result = _repInventoryUsable.Context.Ado.GetDataTable(sql).TableToList<ChartIndex>();

                    return result;
                }
                catch (Exception ex)
                {
                    throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
                }
            }
            #endregion

            #region 历史月份查询目标月份关账日期的结束日
            else
            {
                var sqlWhereSql = string.Empty;
                if (input.CustomerId.HasValue && input.CustomerId > 0)
                {
                    sqlWhereSql = "and i.customerId = " + input.CustomerId + "";
                }
                string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
                DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
                string lastMonthString = targetDate.ToString("yyyyMM");
                //查询目标日期上个月的库存信息
                string Sql = " WITH Period AS (SELECT AccountDate, EndDate FROM dbo.WMS_HachAccountDate WITH (NOLOCK) " +
                             " WHERE AccountDate = '"+ lastMonthString + "' )" +
                             " SELECT COALESCE(NULLIF(p.str2, ''), i.SKU) AS Xseries, SUM(i.Qty * ISNULL(p.Price, 0))AS Yseries" +
                             " FROM dbo.WMS_Inventory_Usable_Snapshot AS i WITH(NOLOCK) JOIN dbo.WMS_Product AS p WITH(NOLOCK)" +
                             " ON p.CustomerId = i.CustomerId AND p.SKU = i.SKU JOIN Period pr" +
                             " ON CAST(i.InventorySnapshotTime AS DATE) = CAST(pr.EndDate AS DATE)" +
                             " WHERE i.CustomerId IN(SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WITH (NOLOCK) " +
                             " WHERE cm.[Type] = 'HachDashBoard'  " + sqlWhereSql+")" +
                             " GROUP BY COALESCE(NULLIF(p.str2, ''), i.SKU) ORDER BY Yseries DESC; ";
                try
                {
                    result = _repInventoryUsable.Context.Ado.GetDataTable(Sql).TableToList<ChartIndex>();
                    // 更安全的null检查和类型转换

                    return result;
                }
                catch (Exception ex)
                {
                    throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    /// <summary>
    /// 根据sku获取目标时间范围内出库总金额--账期内
    /// </summary>
    /// <param name="firstDayOfMonth"></param>
    /// <param name="lastDayOfMonth"></param>
    /// <returns></returns>
    #region  屏幕一  上面第三张图柱状图
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetCurrentOrderGroupBySKUByAmount")]
    public async Task<List<ChartIndex>> GetCurrentOrderGroupBySKUByAmount(ChartsInput input)
    {
        List < ChartIndex > skuAmounts = new List<ChartIndex>();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }

        var sqlWhereSql = string.Empty;

        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }
        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
        string lastMonthString = targetDate.ToString("yyyyMM");
        string query = " WITH Period AS (SELECT AccountDate, StartDate, EndDate FROM dbo.WMS_HachAccountDate WITH (NOLOCK) WHERE AccountDate = '"+ lastMonthString + "')"+
                       " SELECT COALESCE(NULLIF(p.Str2, ''), d.SKU) AS Xseries, SUM(d.OrderQty * ISNULL(p.Price, 0)) AS Yseries FROM dbo.WMS_OrderDetail AS d"+
                       " JOIN dbo.WMS_Order AS o ON d.OrderId = o.Id JOIN dbo.WMS_Product AS p ON p.SKU = d.SKU AND p.CustomerId = d.CustomerId"+
                       " JOIN Period h ON o.CreationTime >= h.StartDate AND o.CreationTime <  DATEADD(DAY, 1, h.EndDate)   "+
                       " WHERE o.OrderStatus = 99 " +
                       " AND o.CustomerId IN ( SELECT CustomerId FROM dbo.WMS_Hach_Customer_Mapping " +
                       " WHERE [Type] = 'HachDashBoard' "+ sqlWhereSql + ") "+
                       " GROUP BY COALESCE(NULLIF(p.Str2, ''), d.SKU) ORDER BY Yseries DESC;";
        try
        {
            skuAmounts = _repOrderDetail.Context.Ado
                .GetDataTable(query).TableToList<ChartIndex>();
            // 更安全的null检查和类型转换
            if (skuAmounts == null || skuAmounts.Count == 0)
            {
                return skuAmounts;
            }
            skuAmounts = skuAmounts.OrderByDescending(a => a.Yseries).ToList();
            return skuAmounts;
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }
    }
    #endregion

    /// <summary>
    /// 获取年初至今出入库金额比率
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    #region 屏幕一 下面第一张图折线图
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetYearToDateInOutAmountRatio")]
    public async Task<MonthVSLast> GetYearToDateInOutAmountRatio(ChartsInput input)
    {
        MonthVSLast result = new MonthVSLast();

        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }

        DateTime? CurrentYearStartDate = new DateTime(Convert.ToDateTime(input.Month).Year, 1, 1);
        DateTime? CurrentYearEndDate = input.Month;

        DateTime? PerviousYearStartDate = Convert.ToDateTime(CurrentYearStartDate).AddYears(-1);
        DateTime? PerviousYearEndDate = Convert.ToDateTime(CurrentYearEndDate).AddYears(-1);

        // 生成月份范围
        var months = GenerateMonthsFromDateRange(CurrentYearStartDate.GetValueOrDefault(), CurrentYearEndDate.GetValueOrDefault());
        var lastYearMonths = GenerateMonthsFromDateRange(PerviousYearStartDate.GetValueOrDefault(), PerviousYearEndDate.GetValueOrDefault());

        result.CurrentYear = await GetOrderVSAsnSalesByTargetMonth(input, CurrentYearStartDate, CurrentYearEndDate);
        result.LastYear = await GetOrderVSAsnSalesByTargetMonth(input, PerviousYearStartDate, PerviousYearEndDate);
        List<ChartIndex> LastYear = new List<ChartIndex>();

        if (result.LastYear == null || result.LastYear.Count == 0)
        {
            result.LastYear = MergeWithAllMonths(result.LastYear, lastYearMonths);
        }
        if (result.CurrentYear == null || result.CurrentYear.Count == 0)
        {
            result.CurrentYear = MergeWithAllMonths(result.CurrentYear, months);
        }

        return result;
    }

    /// <summary>
    /// 获取目标月份每天的库销比
    /// </summary>
    /// <param name="input"></param>
    /// <param name="today"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetOrderVSAsnSalesByTargetMonth(ChartsInput input, DateTime? StartDate, DateTime? EndDate)
    {
        var skuAmounts = new List<ChartIndex>();

        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);

        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = targetDate.Year + "01";

        string lastMonthString = targetDate.ToString("yyyyMM");

        string Sql = " WITH Periods AS (SELECT h.AccountDate, h.StartDate, h.EndDate " +
                     " FROM dbo.WMS_HachAccountDate AS h WITH (NOLOCK) " +
                     " WHERE h.AccountDate BETWEEN '"+ startMonthString + "' AND '"+ lastMonthString + "'),"+
                     " Outbound AS (SELECT pds.AccountDate,SUM(d.OrderQty * ISNULL(p.Price, 0)) AS OutboundAmount " +
                     " FROM dbo.WMS_OrderDetail AS d WITH (NOLOCK) "+
                     " JOIN dbo.WMS_Order AS o WITH (NOLOCK) ON d.OrderId = o.Id "+
                     " JOIN dbo.WMS_Product AS p WITH (NOLOCK) ON p.SKU = d.SKU AND p.CustomerId = d.CustomerId "+
                     " JOIN Periods AS pds ON  o.CreationTime >= pds.StartDate " +
                     " AND o.CreationTime <  DATEADD(DAY, 1, pds.EndDate)  "+
                     " WHERE o.OrderStatus = 99 " +
                     " AND o.CustomerId IN  ( SELECT CustomerId FROM dbo.WMS_Hach_Customer_Mapping WHERE [Type] = 'HachDashBoard' "+ sqlWhereSql + ") "+
                     " GROUP BY pds.AccountDate),"+
                     " Inbound AS (SELECT pds.AccountDate,SUM(ad.ExpectedQty * ISNULL(p.Price, 0)) AS InboundAmount " +
                     " FROM dbo.WMS_ASNDetail AS ad WITH (NOLOCK) "+
                     " JOIN dbo.WMS_ASN AS a   WITH (NOLOCK) ON ad.ASNId = a.Id "+
                     " JOIN dbo.WMS_Product AS p WITH (NOLOCK) ON p.SKU = ad.SKU AND p.CustomerId = ad.CustomerId "+
                     " JOIN Periods AS pds ON  a.CreationTime >= pds.StartDate AND a.CreationTime <  DATEADD(DAY, 1, pds.EndDate)"+
                     " WHERE a.ASNStatus <> 90 " +
                     " AND a.CustomerId IN (SELECT CustomerId FROM dbo.WMS_Hach_Customer_Mapping WHERE [Type] = 'HachDashBoard' "+ sqlWhereSql + ") " +
                     " GROUP BY pds.AccountDate)"+
                     " SELECT RIGHT(CAST(p.AccountDate AS VARCHAR(6)), 2) AS Xseries, "+
                     " CASE WHEN COALESCE(i.InboundAmount, 0) = 0 " +
                     " THEN 0 ELSE ROUND(COALESCE(o.OutboundAmount, 0) * 1.0 / i.InboundAmount, 4)"+
                     " END AS Yseries FROM Periods p"+
                     " LEFT JOIN Outbound o ON o.AccountDate = p.AccountDate"+
                     " LEFT JOIN Inbound  i ON i.AccountDate = p.AccountDate"+
                     " ORDER BY p.AccountDate;";
        try
        {
            skuAmounts = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(Sql).TableToList<ChartIndex>();
            // 更安全的null检查和类型转换

            // 转换月份数字为英文名称
            foreach (var item in skuAmounts)
            {
                item.Xseries = ConvertMonthNumberToName(item.Xseries);
            }
            return skuAmounts;
        }
        catch (Exception ex)
        {
            Logger.LogMessage("获取目标月份每天的库销比报错了：" + ex.Message, logFilePath);
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }
    }

    #endregion

    /// <summary>
    ///获取累计出入库金额比率
    /// </summary>
    /// <returns></returns>
    #region 屏幕一 下面第二张图折线图
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetCumulativeInOutAmountRatio")]
    public async Task<MonthVSLast> GetCumulativeInOutAmountRatio(ChartsInput input)
    {
        try
        {
            MonthVSLast result = new MonthVSLast();

            if (!input.Month.HasValue)
            {
                input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
            }

            DateTime? CurrentYearStartDate = new DateTime(Convert.ToDateTime(input.Month).Year, 1, 1);
            DateTime? CurrentYearEndDate = input.Month;

            DateTime? PerviousYearStartDate = Convert.ToDateTime(CurrentYearStartDate).AddYears(-1);
            DateTime? PerviousYearEndDate = Convert.ToDateTime(CurrentYearEndDate).AddYears(-1);

            // 生成月份范围
            var months = GenerateMonthsFromDateRange(CurrentYearStartDate.GetValueOrDefault(), CurrentYearEndDate.GetValueOrDefault());
            var lastYearMonths = GenerateMonthsFromDateRange(PerviousYearStartDate.GetValueOrDefault(), PerviousYearEndDate.GetValueOrDefault());

            result.CurrentYear = await GetOrderVSAsnAccumulatedMonthlySalesByTargetMonth(input, CurrentYearStartDate, CurrentYearEndDate);
            result.LastYear = await GetOrderVSAsnAccumulatedMonthlySalesByTargetMonth(input, PerviousYearStartDate, PerviousYearEndDate);
            List<ChartIndex> LastYear = new List<ChartIndex>();

            if (result.LastYear == null || result.LastYear.Count == 0)
            {
                result.LastYear = MergeWithAllMonths(result.LastYear, lastYearMonths);
            }
            if (result.CurrentYear == null || result.CurrentYear.Count == 0)
            {
                result.CurrentYear = MergeWithAllMonths(result.CurrentYear, months);
            }
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 获取目标年月份逐月累计库销比
    /// </summary>
    /// <param name="input"></param>
    /// <param name="today"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetOrderVSAsnAccumulatedMonthlySalesByTargetMonth(ChartsInput input, DateTime? StartDate, DateTime? EndDate)
    {
        var skuAmounts = new List<ChartIndex>();
        var skuAmount2s = new List<ChartIndex>();

        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = " and customerId = " + input.CustomerId + "";
        }
 
        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = StartDate.HasValue ? StartDate.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        string lastMonthString = EndDate.HasValue ? EndDate.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");

        string Sql = " WITH Periods AS ( SELECT h.AccountDate, h.StartDate, h.EndDate FROM dbo.WMS_HachAccountDate AS h WITH (NOLOCK)"+
                     " WHERE h.AccountDate BETWEEN '"+ startMonthString + "' AND '"+ lastMonthString + "'), "+
                     " Outbound AS (SELECT pds.AccountDate, SUM(d.OrderQty * ISNULL(p.Price, 0)) AS OutboundAmount "+
                     " FROM dbo.WMS_OrderDetail AS d WITH (NOLOCK) "+
                     " JOIN dbo.WMS_Order AS o  WITH (NOLOCK) ON d.OrderId = o.Id "+
                     " JOIN dbo.WMS_Product AS p WITH (NOLOCK) ON p.SKU = d.SKU AND p.CustomerId = d.CustomerId "+
                     " JOIN Periods AS pds ON  o.CreationTime >= pds.StartDate AND o.CreationTime <  DATEADD(DAY, 1, pds.EndDate) "+
                     " WHERE o.OrderStatus = 99 AND o.CustomerId IN (SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WITH (NOLOCK) "+
                     " WHERE cm.[Type] = 'HachDashBoard' "+ sqlWhereSql + ") GROUP BY pds.AccountDate),"+
                     " Inbound AS (  SELECT pds.AccountDate, SUM(ad.ExpectedQty * ISNULL(p.Price, 0)) AS InboundAmount FROM dbo.WMS_ASNDetail AS ad WITH (NOLOCK)"+
                     " JOIN dbo.WMS_ASN AS a   WITH (NOLOCK) ON ad.ASNId = a.Id "+
                     " JOIN dbo.WMS_Product AS p WITH (NOLOCK) ON p.SKU = ad.SKU AND p.CustomerId = ad.CustomerId "+
                     " JOIN Periods AS pds ON  a.CreationTime >= pds.StartDate AND a.CreationTime <  DATEADD(DAY, 1, pds.EndDate) "+
                     " WHERE a.ASNStatus <> 90 "+
                     " AND a.CustomerId IN (SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WITH (NOLOCK) "+
                     " WHERE cm.[Type] = 'HachDashBoard' "+ sqlWhereSql + ") GROUP BY pds.AccountDate),"+
                     " AllPeriods AS ( SELECT AccountDate FROM Periods),"+
                     " CumulativeOutbound AS ( SELECT ap.AccountDate, SUM(COALESCE(o.OutboundAmount, 0)) OVER (ORDER BY ap.AccountDate) AS CumulativeOutboundAmount "+
                     " FROM AllPeriods ap LEFT JOIN Outbound o ON o.AccountDate = ap.AccountDate),"+
                     " CumulativeInbound AS (SELECT ap.AccountDate,SUM(COALESCE(i.InboundAmount, 0)) OVER (ORDER BY ap.AccountDate) AS CumulativeInboundAmount "+
                     " FROM AllPeriods ap LEFT JOIN Inbound i ON i.AccountDate = ap.AccountDate) "+
                     " SELECT RIGHT(CAST(ap.AccountDate AS VARCHAR(6)), 2) AS Xseries,"+
                     " CASE WHEN COALESCE(ci.CumulativeInboundAmount, 0) = 0 THEN 0 "+
                     " ELSE ROUND(COALESCE(co.CumulativeOutboundAmount, 0) * 1.0 / ci.CumulativeInboundAmount, 4) "+
                     " END AS Yseries FROM AllPeriods ap "+
                     " LEFT JOIN CumulativeOutbound co ON co.AccountDate = ap.AccountDate "+
                     " LEFT JOIN CumulativeInbound  ci ON ci.AccountDate = ap.AccountDate "+
                     " ORDER BY ap.AccountDate; ";
        try
        {
            skuAmounts = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(Sql).TableToList<ChartIndex>();
            // 转换月份数字为英文名称
            foreach (var item in skuAmounts)
            {
                item.Xseries = ConvertMonthNumberToName(item.Xseries);
            }
            return skuAmounts;
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }
    }

    #endregion

    /// <summary>
    /// 获取过去三个月累计库存,当年不计算当月 跟去年对比
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    #region 屏幕一 下面第三张图折线图
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetRollingThreeMonthInOutRatio")]
    public async Task<MonthVSLast> GetRollingThreeMonthInOutRatio(ChartsInput input)
    {
        try
        {
            MonthVSLast result = new MonthVSLast();

            if (!input.Month.HasValue)
            {
                input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
            }
            DateTime? CurrentYearEndDate = Convert.ToDateTime(input.Month.Value.AddMonths(-1));
            DateTime? CurrentYearStartDate = CurrentYearEndDate.HasValue
                ? new DateTime(Convert.ToDateTime(input.Month).AddMonths(-3).Year, Convert.ToDateTime(input.Month).AddMonths(-3).Month, 1)
                : (DateTime?)null;

            DateTime? PerviousYearStartDate = Convert.ToDateTime(CurrentYearStartDate).AddYears(-1);
            DateTime? PerviousYearEndDate = Convert.ToDateTime(CurrentYearEndDate).AddYears(-1);

            // 生成月份范围
            var months = GenerateMonthsFromDateRange(CurrentYearStartDate.GetValueOrDefault(), CurrentYearEndDate.GetValueOrDefault());
            var lastYearMonths = GenerateMonthsFromDateRange(PerviousYearStartDate.GetValueOrDefault(), PerviousYearEndDate.GetValueOrDefault());

            result.CurrentYear = await GetOrderVSAsnAccumulatedMonthlySalesByTargetMonth(input, CurrentYearStartDate, CurrentYearEndDate);
            result.LastYear = await GetOrderVSAsnAccumulatedMonthlySalesByTargetMonth(input, PerviousYearStartDate, PerviousYearEndDate);
            List<ChartIndex> LastYear = new List<ChartIndex>();

            if (result.LastYear == null || result.LastYear.Count == 0)
            {
                result.LastYear = MergeWithAllMonths(result.LastYear, lastYearMonths);
            }
            if (result.CurrentYear == null || result.CurrentYear.Count == 0)
            {
                result.CurrentYear = MergeWithAllMonths(result.CurrentYear, months);
            }
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
        //try
        //{
        //    List<ChartIndex> result = new List<ChartIndex>();
        //    if (!input.Month.HasValue)
        //    {
        //        input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        //    }
        //    int Year = Convert.ToDateTime(input.Month).Year;
        //    int Month = Convert.ToDateTime(input.Month).AddMonths(-1).Month;
        //    int startMonth = Math.Max(1, Month - 2);
        //    result = await GetInventoryUsableGroupByMonth(input, Year, startMonth, Month);
        //    return result;
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
    }
    #endregion

    #endregion

    #region 大屏二

    #region 大屏二 上面第一张折线图
    /// <summary>
    /// 月出库金额趋势 vs 去年
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyOBAmountVSLast")]
    public async Task<MonthVSLast> GetMonthlyOBAmountVSLast(ChartsInput input)
    {
        MonthVSLast result = new MonthVSLast();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        //查到上个月的
        input.Month = input.Month.Value.AddMonths(-1);
        result.CurrentYear = await GetOrderTotalAmountByTagetMonthly(input);
        //查到去年的的
        input.Month = input.Month.Value.AddYears(-1);
        result.LastYear = await GetOrderTotalAmountByTagetMonthly(input);

        return result;
    }
    /// <summary>
    /// 根据目标月份 获取 年初到尾的出库总金额数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetOrderTotalAmountByTagetMonthly(ChartsInput input)
    {
        List<ChartIndex> result = new List<ChartIndex>();
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);

        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = targetDate.Year + "01";

        // 如果您确实需要上个月，可以这样（但根据您的描述，可能不需要）
        string lastMonthString = targetDate.ToString("yyyyMM");

        string query = " WITH AllAccountMonths AS (" +
                       " SELECT AccountDate,StartDate,EndDate FROM WMS_HachAccountDate  WHERE AccountDate BETWEEN '" + startMonthString + "' AND '" + lastMonthString + "')," +
                       " OrderData AS (SELECT h.AccountDate AS Xseries,  SUM(d.[OrderQty] * ISNULL(p.[Price], 0)) AS Yseries FROM [WMS_OrderDetail] d " +
                       " INNER JOIN [WMS_Order] o ON d.[OrderId] = o.[Id] " +
                       " INNER JOIN WMS_Product p ON d.[SKU] = p.[SKU] AND d.CustomerId = p.CustomerId " +
                       " INNER JOIN AllAccountMonths h  ON o.[CreationTime] >= h.StartDate  AND o.[CreationTime] <= h.EndDate " +
                       " WHERE o.[OrderStatus] = 99 " +
                       " AND o.customerId IN (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type = 'HachDashBoard' "+sqlWhereSql+") " +
                       " GROUP BY h.AccountDate)" +
                       " SELECT right(am.AccountDate,2) AS Xseries,ISNULL(od.Yseries, 0) AS Yseries FROM AllAccountMonths am  " +
                       " LEFT JOIN OrderData od ON am.AccountDate = od.Xseries " +
                       " ORDER BY am.AccountDate";
        try
        {
            result = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(query).TableToList<ChartIndex>();
            // 转换月份数字为英文名称
            foreach (var item in result)
            {
                item.Xseries = ConvertMonthNumberToName(item.Xseries);
            }
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }

        return result;
    }
    #endregion

    #region 大屏二 上面第二张折线图
    /// <summary>
    /// 月出库数量趋势 vs 去年
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyOBQtyVSLast")]
    public async Task<MonthVSLast> GetMonthlyOBQtyVSLast(ChartsInput input)
    {
        MonthVSLast result = new MonthVSLast();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        //查到上个月的
        input.Month = input.Month.Value.AddMonths(-1);
        result.CurrentYear = await GetOrderTotalQtyByTagetMonthly(input);
        //查到去年的的
        input.Month = input.Month.Value.AddYears(-1);
        result.LastYear = await GetOrderTotalQtyByTagetMonthly(input);

        return result;
    }

    /// <summary>
    /// 根据目标月份 获取 年初到尾的出库总金额数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private async Task<List<ChartIndex>> GetOrderTotalQtyByTagetMonthly(ChartsInput input)
    {
        List<ChartIndex> result = new List<ChartIndex>();
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);

        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = targetDate.Year + "01";

        // 如果您确实需要上个月，可以这样（但根据您的描述，可能不需要）
        string lastMonthString = targetDate.ToString("yyyyMM");

        string Sql = " WITH Periods AS ("+
                     " SELECT h.AccountDate,  h.StartDate,  h.EndDate FROM dbo.WMS_HachAccountDate AS h "+
                     " WHERE h.AccountDate BETWEEN '"+ startMonthString + "' AND '"+ lastMonthString + "'),"+
                     " OrderAgg AS (SELECT p.AccountDate,SUM(od.OrderQty) AS Yseries FROM Periods p"+
                     " INNER JOIN dbo.WMS_Order AS o ON  o.CreationTime >= p.StartDate"+
                     " AND o.CreationTime <  DATEADD(DAY, 1, p.EndDate)  "+
                     " AND o.OrderStatus = 99 AND o.CustomerId IN ("+
                     " SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WHERE cm.[Type] = 'HachDashBoard' "+sqlWhereSql+")"+
                     " INNER JOIN dbo.WMS_OrderDetail AS od ON od.OrderId = o.Id GROUP BY p.AccountDate )"+
                     " SELECT  Right(CAST(p.AccountDate AS VARCHAR(6)),2) AS Xseries, ISNULL(a.Yseries, 0)  AS Yseries"+
                     " FROM Periods p LEFT JOIN OrderAgg a ON a.AccountDate = p.AccountDate"+
                     " ORDER BY p.AccountDate;";
        try
        {
            result = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(Sql).TableToList<ChartIndex>();
            // 转换月份数字为英文名称
            foreach (var item in result)
            {
                item.Xseries = ConvertMonthNumberToName(item.Xseries);
            }
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }

        return result;
    }

    #endregion

    #region 大屏二 上面第三张柱状图
    /// <summary>
    /// YTD 累积出库产品Minor金额 (Top5)
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyCumulativeObAmount")]
    public async Task<List<ChartIndex>> GetMonthlyCumulativeObAmount(ChartsInput input)
    {
        List<ChartIndex> outputs = new List<ChartIndex>();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }
 
        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);

        // 获取该年份的第一个月（最开始的那个月）
        string startMonthString = targetDate.Year + "01";
        string lastMonthString = targetDate.ToString("yyyyMM");

        string query = " WITH Periods AS (SELECT AccountDate, StartDate, EndDate  FROM dbo.WMS_HachAccountDate "+
                        " WHERE AccountDate BETWEEN '"+ startMonthString + "' AND '"+ lastMonthString + "'),"+
                        " FilteredOrders AS (SELECT o.Id, o.CustomerId FROM dbo.WMS_Order AS o"+
                        " INNER JOIN Periods AS h "+
                        " ON  o.CreationTime >= h.StartDate AND o.CreationTime <  DATEADD(DAY, 1, h.EndDate) "+
                        " WHERE o.OrderStatus = 99 AND o.CustomerId IN ( "+
                        " SELECT cm.CustomerId FROM dbo.WMS_Hach_Customer_Mapping AS cm WHERE cm.[Type] = 'HachDashBoard' "+sqlWhereSql+")) "+
                        " SELECT COALESCE(NULLIF(p.Str2, ''), p.SKU) AS Xseries,SUM(d.AllocatedQty * ISNULL(p.Price, 0)) AS Yseries "+
                        " FROM FilteredOrders o "+
                        " INNER JOIN dbo.WMS_OrderDetail d ON d.OrderId = o.Id "+
                        " LEFT JOIN dbo.WMS_Product p ON p.SKU = d.SKU AND p.CustomerId = d.CustomerId "+
                        " GROUP BY COALESCE(NULLIF(p.Str2, ''), p.SKU)"+
                        " ORDER BY Yseries DESC;";
        try
        {
            outputs = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(query).TableToList<ChartIndex>();
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
        }

        return outputs;
    }
    #endregion

    #region 大屏二 下面第一张饼图
    /// <summary>xx
    /// 月累计发货金额最高的省份 饼图  top5 and else 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyCumulativeShipmentAmount")]
    public async Task<List<ChartIndex>> GetMonthlyCumulativeShipmentAmount(ChartsInput input)
    {
        List<ChartIndex> chartIndices = new List<ChartIndex>();
        if (!input.Month.HasValue)
        {
            input.Month = Convert.ToDateTime(DateTime.Today.ToString("yyyy-MM"));
        }
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }
        string monthString = input.Month.HasValue ? input.Month.Value.ToString("yyyyMM") : DateTime.Today.ToString("yyyyMM");
        DateTime targetDate = DateTime.ParseExact(monthString, "yyyyMM", CultureInfo.InvariantCulture);
        string lastMonthString = targetDate.ToString("yyyyMM");

        string query = "WITH ProvinceSummary AS (SELECT  ISNULL(oa.[Province], '未知省份') AS [ObProvince]," +
            " SUM(od.[OrderQty]) AS [Qty]  FROM [WMS_Order] o " +
            " INNER JOIN WMS_HachAccountDate h ON  o.[CreationTime] <= h.EndDate " +
            "  AND  AccountDate='"+ lastMonthString + "'" +
            " LEFT JOIN [WMS_OrderAddress] oa ON o.[PreOrderId] = oa.[PreOrderId] " +
            " LEFT JOIN [WMS_OrderDetail] od ON o.[Id] = od.[OrderId] " +
            " WHERE 1=1   AND o.customerId in (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")  " +
            " AND o.[OrderStatus] = 99 GROUP BY oa.[Province]), " +
            " ProvinceRank AS (SELECT [ObProvince],[Qty],ROW_NUMBER() OVER (ORDER BY [Qty] DESC) AS RankNum FROM ProvinceSummary) " +
            " SELECT CASE WHEN RankNum <= 5 THEN [ObProvince] ELSE '其它' END AS Xseries,SUM([Qty]) AS Yseries FROM ProvinceRank " +
            " GROUP BY CASE WHEN RankNum <= 5 THEN [ObProvince] ELSE '其它' END ORDER BY SUM([Qty]) DESC";
        try
        {
            chartIndices = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(query).TableToList<ChartIndex>();
        }
        catch (Exception ex)
        {
            throw; // 直接throw而不是throw ex以保留原始堆栈跟踪
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
    #endregion

    #region 大屏二 下面第二张表格
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyNewUserTrendTb")]
    public async Task<OBProvinceOutput> GetMonthlyNewUserTrendTb(ChartsInput input)
    {
        OBProvinceOutput oBProvince = new OBProvinceOutput();
        try
        {
            List<OBProvinceList> list = new List<OBProvinceList>();
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = " AND customerId = " + input.CustomerId.Value;
            }
            if (!input.Month.HasValue)
                input.Month = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));

            var targetMonth = DateTime.Parse(input.Month.Value.ToString("yyyy-MM-01"));

            string sql = $@"DECLARE @TargetMonth     DATE = @Month;
                      DECLARE @TargetYear      INT  = YEAR(@TargetMonth);
DECLARE @TargetMonthNum  INT  = MONTH(@TargetMonth);
DECLARE @StartAccountDate INT = @TargetYear * 100 + 1;
DECLARE @EndAccountDate   INT = @TargetYear * 100 + @TargetMonthNum;

;WITH Periods AS (
SELECT
h.AccountDate,
h.StartDate,
h.EndDate,
CAST(RIGHT(CAST(h.AccountDate AS CHAR(6)), 2) AS INT) AS MonthNum,
STUFF(CAST(h.AccountDate AS CHAR(6)), 5, 0, '-') AS MonthLabel
FROM dbo.WMS_HachAccountDate h WITH (NOLOCK)
WHERE h.AccountDate BETWEEN @StartAccountDate AND @EndAccountDate
),
AllowedCustomers AS (
SELECT /*DISTINCT*/ customerid AS CustomerId
FROM dbo.WMS_Hach_Customer_Mapping WITH (NOLOCK)
WHERE [type] = 'HachDashBoard'  {sqlWhereSql}
),
-- 为 SKU / Customer 选择一个稳定价格（若多条，取 MAX；你也可改为最新价）
ProductPrice AS (
SELECT CustomerId, SKU, MAX(Price) AS Price
FROM dbo.WMS_Product WITH (NOLOCK)
GROUP BY CustomerId, SKU
),
-- 统一生成客户标识（姓名+电话的归一化），并取最早出现月份
CustomerIdentifiers AS (
SELECT
REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')
+ '|' +
REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '') AS CustomerIdentifier,
MIN(h.AccountDate) AS FirstAccountDate
FROM dbo.WMS_OrderDetail o WITH (NOLOCK)
JOIN AllowedCustomers ac WITH (NOLOCK) ON ac.CustomerId = o.CustomerId
JOIN dbo.WMS_OrderAddress oa WITH (NOLOCK) ON oa.PreOrderId = o.PreOrderId
JOIN dbo.WMS_HachAccountDate h WITH (NOLOCK)
ON o.CreationTime >= h.StartDate AND o.CreationTime < DATEADD(DAY, 1, h.EndDate)
WHERE oa.Province IS NOT NULL AND oa.Province <> ''
AND oa.CompanyName IS NOT NULL AND oa.CompanyName <> ''
AND h.AccountDate BETWEEN @StartAccountDate AND @EndAccountDate
GROUP BY
REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')
+ '|' +
REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '')
),
-- 明细：用 HachAccountDate 的分期来做“按月”归档（完全可索引）
Detail AS (
SELECT
p.AccountDate,
p.MonthNum,
p.MonthLabel,
oa.Province      AS ObProvince,
oa.CompanyName,
oa.CompanyType,
o.CustomerId,
o.CustomerName,
-- 归一化标识，和上面完全一致
REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')
+ '|' +
REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '') AS CustomerIdentifier,
CAST(o.OrderQty AS BIGINT) AS OrderQty,
ISNULL(pp.Price, 0)        AS Price
FROM dbo.WMS_OrderDetail o WITH (NOLOCK)
JOIN AllowedCustomers ac WITH (NOLOCK) ON ac.CustomerId = o.CustomerId
JOIN dbo.WMS_OrderAddress oa WITH (NOLOCK) ON oa.PreOrderId = o.PreOrderId
JOIN dbo.WMS_HachAccountDate h WITH (NOLOCK)
ON o.CreationTime >= h.StartDate AND o.CreationTime < DATEADD(DAY, 1, h.EndDate)
JOIN Periods p ON p.AccountDate = h.AccountDate
LEFT JOIN ProductPrice pp
ON pp.CustomerId = o.CustomerId AND pp.SKU = o.SKU
WHERE oa.Province IS NOT NULL AND oa.Province <> ''
AND oa.CompanyName IS NOT NULL AND oa.CompanyName <> ''
),
-- 月聚合：一次性 SUM，不要再做相关子查询
Monthly AS (
SELECT
d.AccountDate,
d.MonthNum,
d.MonthLabel,
d.ObProvince,
d.CompanyName,
d.CompanyType,
d.CustomerId,
d.CustomerName,
d.CustomerIdentifier,
SUM(d.OrderQty)                          AS Qty,
SUM(d.OrderQty * ISNULL(d.Price,0.0))    AS Amount
FROM Detail d
-- 若你需要“累计到当月”的新客/金额，可在这里连 CustomerIdentifiers 并加条件
JOIN CustomerIdentifiers ci ON ci.CustomerIdentifier = d.CustomerIdentifier
GROUP BY
d.AccountDate, d.MonthNum, d.MonthLabel,
d.ObProvince, d.CompanyName, d.CompanyType,
d.CustomerId, d.CustomerName, d.CustomerIdentifier
)
SELECT TOP 200
m.MonthNum         AS Month,
m.MonthLabel,
m.CustomerId,
m.CustomerName,
m.ObProvince,
m.CompanyName,
m.CompanyType,
m.Qty,
CAST(m.Amount AS DECIMAL(18,2)) AS Amount
FROM Monthly m
ORDER BY m.AccountDate DESC, m.Amount DESC
OPTION (RECOMPILE);   ";

            // 4) 仅传 @Month 参数
            var dt = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(
                sql,
                new List<SugarParameter> {
        new SugarParameter("@Month", targetMonth)
                }
            );
            var result = dt.TableToList<OBProvinceList>();

            list = result
                .Select(x => new OBProvinceList
                {
                    Month = x.Month,
                    ObProvince = FormatProvinceName(x.ObProvince),
                    Amount = x.Amount,
                    Qty = x.Qty,
                    CustomerId = x.CustomerId,
                    CustomerName = x.CustomerName,
                    CompanyName = x.CompanyName,
                    CompanyType = x.CompanyType
                })
                .GroupBy(g => new
                {
                    g.Month,
                    g.CustomerId,
                    g.CustomerName,
                    g.ObProvince,
                    g.CompanyType,
                    g.CompanyName
                })
                .Select(g => new OBProvinceList
                {
                    Month = g.Key.Month,
                    CustomerId = g.Key.CustomerId,
                    CustomerName = g.Key.CustomerName,
                    ObProvince = g.Key.ObProvince,
                    Amount = g.Sum(z => z.Amount),
                    Qty = g.Sum(z => z.Qty),
                    CompanyType = g.Key.CompanyType,
                    CompanyName = g.Key.CompanyName
                })
                .OrderBy(x => x.Month)
                .ThenByDescending(x => x.Amount)
                .ToList();
            foreach (var item in list)
            {
                item.Month = ConvertMonthNumberToName(item.Month);
            }
            oBProvince.oBProvinceList = list;
            oBProvince.TotalQty = (long)list.Sum(a => a.Qty);
            return oBProvince;
        }
        catch (Exception ex)
        {
            return oBProvince;
            throw;
        }
    }

    #endregion

    #region 大屏二 下面第三张表格
    /// <summary>
    /// 当年月份累计新增用户数量 根据省份来分组
    /// </summary>
    /// <param name="input"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    /// 如果1月份的用户 在2月或者后面的月份出现了 那么后面的月份的count减去对应的用户数量
    ///但是数据是 1月 跟前面的对比  然后是1月跟2月 跟前面的对比 然后是1月跟3月跟前面的对比 以此类推

    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetMonthlyCumulativeNewUserTrendTb")]
    public async Task<OBProvinceOutput> GetMonthlyCumulativeNewUserTrendTb(ChartsInput input)
    {
        OBProvinceOutput oBProvinceOutput = new OBProvinceOutput();
        List<OBProvinceList> list = new List<OBProvinceList>();
        // 1) 标准化“最后月份”为该月第一天（yyyy-MM-01）
        if (!input.Month.HasValue)
            input.Month = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-01"));
        var targetMonth = DateTime.Parse(input.Month.Value.ToString("yyyy-MM-01"));

        // 2) 客户过滤片段（按你要求拼接）
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = " AND customerId = " + input.CustomerId.Value;
        }

        // 3) 主查询 SQL：计算每月窗口内的金额与“当月新增 Qty”
        var sql = $@"  DECLARE @TargetMonth     DATE = @Month;
                       DECLARE @TargetYear      INT  = YEAR(@TargetMonth);
                       DECLARE @TargetMonthNum  INT  = MONTH(@TargetMonth);
                       DECLARE @StartAccountDate INT = @TargetYear * 100 + 1;   
                       DECLARE @EndAccountDate   INT = @TargetYear * 100 + @TargetMonthNum; 
                       ;WITH Periods AS ( SELECT h.AccountDate,h.StartDate,h.EndDate,
                       CAST(left(CAST(h.AccountDate AS CHAR(6)), 4) AS varchar) AS MonthNum   FROM dbo.WMS_HachAccountDate AS h WITH (NOLOCK) 
                       WHERE h.AccountDate BETWEEN @StartAccountDate AND @EndAccountDate),
                       AllowedCustomers AS ( SELECT DISTINCT customerid AS CustomerId  FROM dbo.WMS_Hach_Customer_Mapping WITH (NOLOCK) WHERE [type] = 'HachDashBoard' {sqlWhereSql}),
                       CustomerIdentifiers AS ( SELECT 
                       REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')+ '|' + REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '')
                       AS CustomerIdentifier,MIN(h.AccountDate) AS FirstAccountDate  FROM dbo.WMS_OrderDetail AS o WITH (NOLOCK)
                       JOIN AllowedCustomers ac WITH (NOLOCK) ON ac.CustomerId = o.CustomerId
                       JOIN dbo.WMS_OrderAddress oa WITH (NOLOCK) ON oa.PreOrderId = o.PreOrderId
                        AND PROVINCE IS NOT NULL AND Province <>'' AND CompanyName IS NOT NULL AND CompanyName<>''
                       JOIN dbo.WMS_HachAccountDate h WITH (NOLOCK) ON o.CreationTime >= h.StartDate AND o.CreationTime < DATEADD(DAY, 1, h.EndDate)
                       GROUP BY  REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')+ '|' + REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '')
                       ),
                       MonthlyData AS (SELECT h.AccountDate,  oa.CompanyType,o.CustomerId,o.CustomerName,
                       REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')+ '|' + REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '')
                       AS CustomerIdentifier,
                       SUM(o.OrderQty) AS MonthlyOrderQty,SUM(o.OrderQty * ISNULL(pdt.Price, 0)) AS MonthlyAmount,ci.FirstAccountDate
                       FROM dbo.WMS_OrderDetail AS o WITH (NOLOCK)
                       JOIN AllowedCustomers ac WITH (NOLOCK) ON ac.CustomerId = o.CustomerId
                       JOIN dbo.WMS_OrderAddress oa WITH (NOLOCK) ON oa.PreOrderId = o.PreOrderId
                        AND PROVINCE IS NOT NULL AND Province <>'' AND CompanyName IS NOT NULL AND CompanyName<>''
                       JOIN dbo.WMS_Product pdt WITH (NOLOCK) ON pdt.SKU = o.SKU AND pdt.CustomerId = o.CustomerId
                       JOIN dbo.WMS_HachAccountDate h WITH (NOLOCK) ON o.CreationTime >= h.StartDate AND o.CreationTime < DATEADD(DAY, 1, h.EndDate)
                       INNER JOIN CustomerIdentifiers ci ON  REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')+ '|' + REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', '')
                       = ci.CustomerIdentifier
                       WHERE h.AccountDate BETWEEN @StartAccountDate AND @EndAccountDate
                       GROUP BY  h.AccountDate,oa.Province,oa.CompanyType,o.CustomerId,o.CustomerName,
                       REPLACE(REPLACE(REPLACE(UPPER(LTRIM(RTRIM(oa.name))), ',', ''), '，', ''), ' ', '')+ '|' + REPLACE(REPLACE(REPLACE(REPLACE(oa.phone, ' ', ''), '-', ''), '(', ''), ')', ''),
                       ci.FirstAccountDate),
                       MonthlySummary AS ( SELECT p.AccountDate,p.MonthNum,md.CustomerId,md.CustomerName,md.CompanyType,
                       -- 累计到当月的新增客户数量
                      Cast( COUNT(DISTINCT CASE WHEN md.FirstAccountDate <= p.AccountDate THEN md.CustomerIdentifier END) as bigint) AS Qty,
                       -- 累计到当月的新增客户金额
                       Cast(SUM(CASE WHEN md.FirstAccountDate <= p.AccountDate THEN md.MonthlyAmount ELSE 0 END) as bigint) AS Amount
                       FROM Periods p
                       INNER JOIN MonthlyData md ON md.AccountDate = p.AccountDate
                       GROUP BY p.AccountDate,p.MonthNum,md.CustomerId,md.CustomerName,md.CompanyType)
                       SELECT top 200 MonthNum as Month,CustomerId,CustomerName,CompanyType,Qty,Amount
                       FROM MonthlySummary
                       ORDER BY AccountDate DESC, Amount DESC;";
        try
        {
            var dt = _repInventoryUsableSnapshot.Context.Ado.GetDataTable(
                sql,
                new List<SugarParameter> {
                new SugarParameter("@Month", targetMonth)
                });

            var raw = dt.TableToList<OBProvinceList>();

            list = raw
             .Select(x => new OBProvinceList
             {
                 Month = x.Month,
                 ObProvince = FormatProvinceName(x.ObProvince),
                 Amount = x.Amount,
                 Qty = x.Qty,
                 CustomerId = x.CustomerId,
                 CustomerName = x.CustomerName,
                 CompanyType = x.CompanyType
             })
             .GroupBy(g => new
             {
                 g.Month,
                 g.CustomerId,
                 g.CustomerName,
                 g.ObProvince,
                 g.CompanyType,
             })
             .Select(g => new OBProvinceList
             {
                 Month = g.Key.Month,
                 CustomerId = g.Key.CustomerId,
                 CustomerName = g.Key.CustomerName,
                 ObProvince = g.Key.ObProvince,
                 Amount = g.Sum(z => z.Amount),
                 Qty = g.Sum(z => z.Qty),
                 CompanyType = g.Key.CompanyType,
             })
             .OrderBy(x => x.Month)
             .ThenByDescending(x => x.Amount)
             .ToList();
            foreach (var item in list)
            {
                item.Month=ConvertMonthNumberToName(item.Month);
            }
            oBProvinceOutput.oBProvinceList = list;
            oBProvinceOutput.TotalQty = (long)list.Sum(a => a.Qty);
        }
        catch
        {
            return oBProvinceOutput;
            throw; // 保留原始堆栈
        }
        return oBProvinceOutput;
    }

    #endregion

    #endregion

    #region 大屏三
    /// <summary>
    /// 屏三 根据省份获取总的出库金额
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetByOBProvince")]
    public async Task<List<OBProvince>> GetByOBProvince(ObChartsInput input)
    {
        List<OBProvince> outputs = new List<OBProvince>();
        try
        {
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and customerId = " + input.CustomerId + "";
            }

            var sqlWhereDate = string.Empty;
            var sqlWhereDateStr = string.Empty;

            if (input.StartDate.HasValue)
            {
                sqlWhereDateStr = "AND CONVERT(varchar(10),[o].[CreationTime] ,120)>='" + input.StartDate.Value.ToString("yyyy-MM-dd") + "' AND CONVERT(varchar(10),[o].[CreationTime] ,120) <= '" + input.EndDate.Value.ToString("yyyy-MM-dd") + "' ";
            }

            string Sql = "SELECT [oa].[Province] AS [ObProvince], SUM([od].[OrderQty] * ISNULL([p].[Price], 0)) AS [Amount] " +
                "FROM [WMS_Order] [o]  LEFT JOIN [WMS_OrderAddress] [oa] ON [o].[PreOrderId] = [oa].[PreOrderId] LEFT JOIN [WMS_OrderDetail] [od] " +
                "ON [o].[Id] = [od].[OrderId] LEFT JOIN [wms_product] [p] ON [od].[SKU] = [p].[sku] AND [o].[CustomerId] = [p].[customerid]  " +
                "WHERE 1=1 AND od.customerId IN (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")  " +
                "AND [o].[OrderStatus] = 99  " + sqlWhereDateStr + " " +
                //" "+ sqlWhereSql2 + "  " +
                "GROUP BY [oa].[Province] ORDER BY [Amount] DESC";
            var orderData = _repCustomer.Context.Ado.GetDataTable(Sql).TableToList<OBProvince>();

            if (orderData != null && orderData.Count > 0)
            {
                // 计算每个省份的总金额
                outputs = await GetObDataGByProvince(orderData);
            }
            else
            {
                // 如果结果集为null，循环省份数据并创建零值结果
                foreach (var province in ProvinceNameMap.Values)
                {
                    outputs.Add(new OBProvince
                    {
                        ObProvince = province,
                        Qty = 0,
                        Amount = 0
                    });
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return outputs;
    }
    [HttpPost]
    [AllowAnonymous]
    [ApiDescriptionSettings(Name = "GetByOBProvinceByProvince")]
    public async Task<List<OBProvinceGroupbyWhere>> GetByOBProvinceByProvince(ObChartsInput input)
    {
        List<OBProvinceGroupbyWhere> outputs = new List<OBProvinceGroupbyWhere>();
        if (string.IsNullOrEmpty(input.OBProvince))
        {
            return outputs;
        }
        var sqlWhereSql = string.Empty;
        if (input.CustomerId.HasValue && input.CustomerId > 0)
        {
            sqlWhereSql = "and customerId = " + input.CustomerId + "";
        }

        var sqlWhereSql2 = string.Empty;
        if (!string.IsNullOrEmpty(input.OBProvince))
        {
            sqlWhereSql2 = " and oa.province like '%" + input.OBProvince + "%' ";
        }
        var sqlWhereDate = string.Empty;
        var sqlWhereDateStr = string.Empty;

        if (input.StartDate.HasValue)
        {
            sqlWhereDateStr = "AND CONVERT(varchar(10),[o].[CreationTime] ,120)>='" + input.StartDate.Value.ToString("yyyy-MM-dd") + "' AND CONVERT(varchar(10),[o].[CreationTime] ,120) <= '" + input.EndDate.Value.ToString("yyyy-MM-dd") + "' ";
        }

        string Sql = "SELECT [oa].[Province] AS [ObProvince],o.CustomerId,o.CustomerName as Customer ,sum(od.OrderQty) as Qty,  SUM([od].[OrderQty] * ISNULL([p].[Price], 0)) AS [Amount] " +
            "FROM [WMS_Order] [o]  LEFT JOIN [WMS_OrderAddress] [oa] ON [o].[PreOrderId] = [oa].[PreOrderId] LEFT JOIN [WMS_OrderDetail] [od] " +
            "ON [o].[Id] = [od].[OrderId] LEFT JOIN [wms_product] [p] ON [od].[SKU] = [p].[sku] AND [o].[CustomerId] = [p].[customerid]  " +
            "WHERE 1=1 AND od.customerId IN (SELECT customerid FROM WMS_Hach_Customer_Mapping WHERE type='HachDashBoard' " + sqlWhereSql + ")  " +
            "AND [o].[OrderStatus] = 99  " + sqlWhereDateStr + " " +
            "" + sqlWhereSql2 + " GROUP BY [oa].[Province],o.CustomerName,o.CustomerId ORDER BY [Amount] DESC";
        outputs = _repCustomer.Context.Ado.GetDataTable(Sql).TableToList<OBProvinceGroupbyWhere>();

        if (outputs != null && outputs.Count > 0)
        {
            // 格式化输入数据并创建查找字典
            var formattedDataDict = outputs
                .Select(x => new OBProvinceGroupbyWhere
                {
                    ObProvince = FormatProvinceName(x.ObProvince),
                    Amount = x.Amount,
                    Qty = x.Qty,
                    Customer = x.Customer,
                    CustomerId = x.CustomerId
                })
                .GroupBy(a => a.Customer)
                .Select(g => new OBProvinceGroupbyWhere
                {
                    Customer = g.Key,
                    Amount = g.Sum(x => x.Amount),
                    Qty = g.Sum(x => x.Qty),
                    ObProvince = g.First().ObProvince,
                    CustomerId = g.First().CustomerId
                })
                .OrderByDescending(a => a.Amount)  // 按Amount降序排序
                .ToList();

            outputs = formattedDataDict;
        }
        return outputs;
    }
    #endregion

    #region 封装的公共方法

    #region tab汇总项封装方法
    // 根据日期范围生成月份列表
    private List<string> GenerateMonthsFromDateRange(DateTime startDate, DateTime endDate)
    {
        var months = new List<string>();
        var current = new DateTime(startDate.Year, startDate.Month, 1);

        while (current <= endDate)
        {
            months.Add(current.ToString("yyyy-MM"));
            current = current.AddMonths(1);
        }

        return months;
    }

    // 合并实际数据与所有月份，确保数据完整性
    private List<ChartIndex> MergeWithAllMonths(List<ChartIndex> actualData, List<string> allMonths)
    {
        List<ChartIndex> result = new List<ChartIndex>();

        if (allMonths == null || allMonths.Count == 0)
            return result;

        // 如果实际数据为空，创建全零数据
        if (actualData == null || actualData.Count == 0)
        {
            result = allMonths.Select(month => new ChartIndex
            {
                Xseries = month,
                Yseries = 0
            }).ToList();
        }

        return result;
    }
 
    #endregion

    #region 大屏三
    /// <summary>
    /// 根据出库省份获取总金额
    /// </summary>
    /// <param name="data"></param>
    /// <param name="priceMap"></param>
    /// <returns></returns>
    private async Task<List<OBProvince>> GetObDataGByProvince(List<OBProvince> data)
    {
        List<OBProvince> oBProvinces = new List<OBProvince>();

        // 获取所有省份名称
        var allProvinces = ProvinceNameMap.Values.ToList();

        // 格式化输入数据并创建查找字典
        var formattedDataDict = data
            .Select(x => new OBProvince
            {
                ObProvince = FormatProvinceName(x.ObProvince),
                Amount = x.Amount
            })
            .GroupBy(x => x.ObProvince)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Amount));

        // 创建结果列表，确保包含所有省份
        oBProvinces = allProvinces.Select(province => new OBProvince
        {
            ObProvince = province,
            Amount = formattedDataDict.TryGetValue(province, out var amount) ? amount : 0
        }).ToList();

        return oBProvinces;
    }

    #endregion
    /// <summary>
    /// 月份数字转换为名称  ->  01 ,02,03,04,05,06,07,08,09,10,11,12
    /// </summary>
    /// <param name="monthNumber"></param>
    /// <returns></returns>
    private string ConvertMonthNumberToName(string monthNumber)
    {
        // 处理单数字月份
        if (monthNumber.Length == 1)
        {
            monthNumber = "0" + monthNumber;
        }

        // 使用LINQ查找对应的月份名称
        var monthPair = MonthNameMap.FirstOrDefault(x => x.Value == monthNumber);
        return monthPair.Key ?? monthNumber; // 如果找不到，返回原始值
    }
    /// <summary>
    /// 月份名称映射表
    /// </summary>
    private static readonly Dictionary<string, string> MonthNameMap = new Dictionary<string, string>
        {
            { "Jan", "01" },
            { "Feb", "02" },
            { "Mar", "03" },
            { "Apr", "04" },
            { "May", "05" },
            { "Jun", "06" },
            { "Jul", "07" },
            { "Aug", "08" },
            { "Sep", "09" },
            { "Oct", "10" },
            { "Nov", "11" },
            { "Dec", "12" },
        };
    /// <summary>
    /// 省份名称映射表
    /// </summary>
    private static readonly Dictionary<string, string> ProvinceNameMap = new Dictionary<string, string>
        {
            { "北京市", "北京" },
            { "天津市", "天津" },
            { "上海市", "上海" },
            { "重庆市", "重庆" },
            { "香港特别行政区", "香港" },
            { "澳门特别行政区", "澳门" },
            { "内蒙古自治区", "内蒙古" },
            { "广西壮族自治区", "广西" },
            { "西藏自治区", "西藏" },
            { "宁夏回族自治区", "宁夏" },
            { "新疆维吾尔自治区", "新疆" },
            { "河北省", "河北" },
            { "山西省", "山西" },
            { "辽宁省", "辽宁" },
            { "吉林省", "吉林" },
            { "黑龙江省", "黑龙江" },
            { "江苏省", "江苏" },
            { "浙江省", "浙江" },
            { "安徽省", "安徽" },
            { "福建省", "福建" },
            { "江西省", "江西" },
            { "山东省", "山东" },
            { "河南省", "河南" },
            { "湖北省", "湖北" },
            { "湖南省", "湖南" },
            { "广东省", "广东" },
            { "海南省", "海南" },
            { "四川省", "四川" },
            { "贵州省", "贵州" },
            { "云南省", "云南" },
            { "陕西省", "陕西" },
            { "甘肃省", "甘肃" },
            { "青海省", "青海" },
            { "台湾省", "台湾" },
        };
    /// <summary>
    /// 格式化省份名称的函数
    /// </summary>
    /// <param name="provinceName"></param>
    /// <returns></returns>
    private string FormatProvinceName(string provinceName)
    {
        if (string.IsNullOrEmpty(provinceName)) return "";

        if (provinceName.Contains("山东"))
        {

        }
        // 使用映射表进行格式化，如果没有找到匹配，则返回原始名称
        // 处理映射表中定义的特殊名称
        if (ProvinceNameMap.ContainsKey(provinceName))
        {
            return ProvinceNameMap[provinceName];
        }
        // 替换省、自治区等后缀（“省”、“自治区”或“特别行政区”）
        return provinceName.Replace("省", "").Replace("自治区", "").Replace("特别行政区", "").Trim();
    }
    #endregion
}
