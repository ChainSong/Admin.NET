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

public class SumTabInput
{
    public long? CustomerId { get; set; }
}
public class ChartsInput
{
    public DateTime? Month { get; set; }
    public long? CustomerId { get; set; }
    public string? OBProvince { get; set; }
    public int Take { get; set; } = 100;
}
public class ObChartsInput: ChartsInput
{
    public DateTime? StartDate{ get; set; }
    public DateTime? EndDate { get; set; }
}
public class ProvinceInput
{
    //只支持单个关键词语搜索关键词支持：行政区名称、citycode、adcode
    //例如，在 subdistrict = 2，搜索省份（例如山东），能够显示市（例如济南），区（例如历下区）
    public string? Keywords { get; set; }
    //设置显示下级行政区级数（行政区级别包括：国家、省/直辖市、市、区/县4个级别）
    //可选值：0、1、2、3
    //0：不返回下级行政区；
    //1：返回下一级行政区；
    //2：返回下两级行政区；
    //3：返回下三级行政区；
    public string? Subdistrict { get; set; }
    // 此项控制行政区信息中返回行政区边界坐标点； 可选值：base、all;
    //base:不返回行政区边界坐标点；
    //all:只返回当前查询 district 的边界值，不返回子节点的边界值；
    public string? Extensions { get; set; }
}
/// <summary>
/// 汇总金额项
/// </summary>
public class SumItemOutput
{
    /// <summary>
    /// 上个月库存总金额 Last month's total inventory amount
    /// </summary> 
    public double? LastMonthAmount { get; set; }

    /// <summary>
    /// 目前库存总金额  Total inventory amount for this month
    /// </summary>
    public double? CurrentMonthAmount { get;set; }

    /// <summary>
    /// 当月目标库存总金额 Target inventory total amount
    /// </summary>
    public double? CurrentTargetAmount { get; set; }

    /// <summary>
    /// 目前库存总金额VS目标库存总金额 Current total inventory amount vs. target total inventory amount
    /// </summary>
    public double? CMonthVSTargetAmount { get; set; }

    /// <summary>
    /// YTD出库/入库
    /// </summary>
    public double? YTDOrderVSASNAmount { get; set; }

    /// <summary>
    /// 当月入库总金额
    /// </summary>
    public double? CurrentReceiptAmount { get; set; }

    /// <summary>
    /// 当月出库总金额
    /// </summary>
    public double? CurrentOrderAmount { get; set; }
}
public class SelectItem
{
    public long? Id { get; set; }
    public string? Label { get; set; }
    public string? Value { get; set; }
}

