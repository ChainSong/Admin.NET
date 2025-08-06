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

public class ChartsInput
{
    public DateTime? Month { get; set; }
    public long? CustomerId { get; set; }
    public string? OBProvince { get; set; }
}

/// <summary>
/// 汇总金额项
/// </summary>
public class SumItemOutput
{
    /// <summary>
    /// 上个月库存总金额 Last month's total inventory amount
    /// </summary> 
    public long? LastMonthAmount { get; set; }

    /// <summary>
    /// 目前库存总金额  Total inventory amount for this month
    /// </summary>
    public long? CurrentMonthAmount { get;set; }

    /// <summary>
    /// 当月目标库存总金额 Target inventory total amount
    /// </summary>
    public long? CurrentTargetAmount { get; set; }

    /// <summary>
    /// 目前库存总金额VS目标库存总金额 Current total inventory amount vs. target total inventory amount
    /// </summary>
    public long? CMonthVSTargetAmount { get; set; }

    /// <summary>
    /// YTD出库/入库
    /// </summary>
    public long? YTDOrderVSASNAmount { get; set; }

    /// <summary>
    /// 当月入库总金额
    /// </summary>
    public long? CurrentReceiptAmount { get; set; }

    /// <summary>
    /// 当月出库总金额
    /// </summary>
    public long? CurrentOrderAmount { get; set; }
}
