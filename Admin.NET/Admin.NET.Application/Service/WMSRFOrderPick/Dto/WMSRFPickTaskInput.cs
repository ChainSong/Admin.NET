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

namespace Admin.NET.Application;
public class WMSRFPickTaskInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string WarehouseName { get; set; }

    /// <summary>
    /// PickTaskNumber
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// PickStatus
    /// </summary>
    public int PickStatus { get; set; }

    public string OrderNumber { get; set; }
    public string ExternOrderNumber { get; set; }

    public string ScanInput { get; set; }

    /// <summary>
    /// PickType
    /// </summary>
    public string PickType { get; set; }

    /// <summary>
    /// StartTime
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// EndTime
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public int PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintPersonnel
    /// </summary>
    public string? PrintPersonnel { get; set; }

    /// <summary>
    /// PickPlanPersonnel
    /// </summary>
    public string? PickPlanPersonnel { get; set; }

    /// <summary>
    /// DetailQty
    /// </summary>
    public double DetailQty { get; set; }

    /// <summary>
    /// DetailKindsQty
    /// </summary>
    public int DetailKindsQty { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

    /// <summary>
    /// Str1
    /// </summary>
    public string? Str1 { get; set; }

    /// <summary>
    /// Str2
    /// </summary>
    public string? Str2 { get; set; }

    /// <summary>
    /// Str3
    /// </summary>
    public string? Str3 { get; set; }

    /// <summary>
    /// Str4
    /// </summary>
    public string? Str4 { get; set; }

    /// <summary>
    /// Str5
    /// </summary>
    public string? Str5 { get; set; }

    /// <summary>
    /// Str6
    /// </summary>
    public string? Str6 { get; set; }

    /// <summary>
    /// Str7
    /// </summary>
    public string? Str7 { get; set; }

    /// <summary>
    /// Str8
    /// </summary>
    public string? Str8 { get; set; }

    /// <summary>
    /// Str9
    /// </summary>
    public string? Str9 { get; set; }

    /// <summary>
    /// Str10
    /// </summary>
    public string? Str10 { get; set; }

    /// <summary>
    /// DateTime1
    /// </summary>
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime4
    /// </summary>
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime5
    /// </summary>
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public int? Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public int? Int3 { get; set; }

    /// <summary>
    /// Int4
    /// </summary>
    public int? Int4 { get; set; }

    /// <summary>
    /// Int5
    /// </summary>
    public int? Int5 { get; set; }
}
