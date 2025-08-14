// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.HachDashBoardConfig.Dto;
/// <summary>
/// 产品客户价格关系表
/// </summary>
public class CustomerProductPriceMapping
{
    public long? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? Sku { get; set; }
    public double? Price { get; set; }
    public double? Qty { get; set; }

}
/// <summary>
/// 第一张图
/// </summary>
public class MonthVSLastChartOneOutput
{
    //月库存金额趋势图VS去年
    public MonthVSLast MonthVSLast;
    //及时库存
    public List<ChartIndex> TodayPriceOfInventory { get; set; }
    //当月库存出库产品总金额
    public List<ChartIndex> CurrentMonthPriceOfOB { get; set; }
    //当月每天  入库/出库
    public List<ChartIndex> CurrentMonthPriceReceiptVSOB { get; set; }
    //当年按照  月累计 库存总金额  对比去年  
    public List<ChartIndex> MonthlyCumulativeAmountVSLast { get; set; }

    //当年当月前3月累计 库存总金额  对比去年 相同月份数据 
    public List<ChartIndex> CumulativeAmountVSLastThreeMonth { get; set; }
}
/// <summary>
/// 第二张图
/// </summary>
public class MonthVSLastChartTwoOutput
{
    /// <summary>
    /// 月出库金额趋势 vs 去年
    /// </summary>
    public MonthVSLast MonthlyObAmount { get; set; }
    /// <summary>
    /// 月出库数量趋势 vs 去年
    /// </summary>
    public MonthVSLast MonthlyObQty{ get; set; }
    /// <summary>
    /// YTD 累积出库产品Minor金额 (Top5): 需要提供
    /// </summary>
    public MonthVSLast MonthlyCumulativeObAmount{ get; set; }
    /// <summary>
    /// 月累计发货金额最高的省份
    /// </summary>
    /// TOP5  AND  ELSE
    public List<ChartIndex> MonthlyCumulativeShipmentAmount { get; set; }
    /// <summary>
    /// 每月新用户增长趋势
    /// </summary>
    public List<ChartIndex> MonthlyNewUserTrend { get; set; }
    /// <summary>
    /// 月累计新用户增长趋势
    /// </summary>
    public List<ChartIndex> MonthlyCumulativeNewUserTrend { get; set; }
}
/// <summary>
/// 第三张图
/// </summary>
public class OBProvinceOutput
{
    public List<OBProvince> oBProvince { get; set; }
    public List<OBProvinceGroupbyWhere> oBProvinceGroupbyProvince { get; set; }
}
/// <summary>
/// 目标年份与前一年
/// </summary>
public class MonthVSLast
{
    public List<ChartIndex> LastYear { get; set; }
    public List<ChartIndex> CurrentYear { get; set; }
}
/// <summary>
/// 图标的x 与 y
/// </summary>
public class ChartIndex
{
    public string Xseries { get; set; }
    public double? Yseries { get; set; }
}

public class OBProvinceList
{
    public string? ObProvince { get; set; }
    public string? Customer { get; set; }
    public string? Sku { get; set; }
    public long? CustomerId { get; set; }
    public double? Qty { get; set; }
}
/// <summary>
/// 计算省份的总金额，总数量
/// </summary>
public class OBProvince 
{
    public string ObProvince { get; set; }
    public double? Qty { get; set; }
    public long? Amount { get; set; }
}
/// <summary>
/// 根据条件计算省份每位客户的的总金额，总数量
/// </summary>
public class OBProvinceGroupbyWhere
{
    public string? ObProvince { get; set; }
    public string? Customer { get; set; }
    public long? CustomerId { get; set; }
    public double? Qty { get; set; }
    public double? Amount { get; set; }
}

public class OrderDetailTotal
{
    public string? SKU { get; set; }
    public double? TotalQty { get; set; }

}
public class OrderDetailTotalByMonth
{
    public string? Month { get; set; }
    public double? Qty { get; set; }
    public string? SKU { get; set; }
}

public class OrderDetailTotalAmount
{
    public string? Month { get; set; }
    public double? Price { get; set; }
}