// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application;
public class WMSAdjustmentExportOutput
{
    /// <summary>
    /// 调整单号
    /// </summary>
    [ExporterHeader(displayName: "调整单号")]
    public string AdjustmentNumber { get; set; }
    /// <summary>
    /// 外部单号
    /// </summary>
    [ExporterHeader(displayName: "外部单号")]
    public virtual string ExternNumber { get; set; }
    /// <summary>
    /// CustomerName
    /// </summary>
    [ExporterHeader(displayName: "客户名称")]
    public string CustomerName { get; set; }
    /// <summary>
    /// WarehouseName
    /// </summary>
    [ExporterHeader(displayName: "仓库名称")]
    public string WarehouseName { get; set; }
    /// <summary>
    /// 1新建 99 完成 -1 取消
    /// </summary>
    [ExporterHeader(displayName: "调整单状态")]
    public string AdjustmentStatus { get; set; }
    /// <summary>
    /// 调整单类型 1 移动 2 修改 3 冻结
    /// </summary>
    [ExporterHeader(displayName: "调整单类型")]
    public string AdjustmentType { get; set; }
    /// <summary>
    /// 调整原因
    /// </summary>
    [ExporterHeader(displayName: "调整原因")]
    public string? AdjustmentReason { get; set; }
    /// <summary>
    /// AdjustmentTime
    /// </summary>
    [ExporterHeader(displayName: "调整时间")]
    public DateTime? AdjustmentTime { get; set; }
    [ExporterHeader(displayName: "SKU")]
    public string SKU { get; set; }
    [ExporterHeader(displayName: "UPC")]
    public string UPC { get; set; }
    [ExporterHeader(displayName: "托号")]
    public string? TrayCode { get; set; }
    [ExporterHeader(displayName: "批次号")]
    public string? BtachCode { get; set; }
    [ExporterHeader(displayName: "箱号")]
    public string? BoxCode { get; set; }
    [ExporterHeader(displayName: "SKU名称")]
    public string? GoodsName { get; set; }
    [ExporterHeader(displayName: "Lot号")]
    public string? LotCode { get; set; }
    [ExporterHeader(displayName: "Po号")]
    public string? PoCode { get; set; }
    [ExporterHeader(displayName: "So号")]
    public string? SoCode { get; set; }
    [ExporterHeader(displayName: "重量")]
    public double? Weight { get; set; }
    [ExporterHeader(displayName: "体积")]
    public double? Volume { get; set; }
    [ExporterHeader(displayName: "生产日期")]
    public DateTime? ProductionDate { get; set; }
    [ExporterHeader(displayName: "过期日期")]
    public DateTime? ExpirationDate { get; set; }
    [ExporterHeader(displayName: "原仓库")]
    public string? FromWarehouseName { get; set; }
    [ExporterHeader(displayName: "目标仓库")]
    public string? ToWarehouseName { get; set; }
    [ExporterHeader(displayName: "原库区")]
    public string? FromArea { get; set; }
    [ExporterHeader(displayName: "目标仓库")]
    public string? ToArea { get; set; }
    [ExporterHeader(displayName: "原库位")]
    public string? FromLocation { get; set; }
    [ExporterHeader(displayName: "目标库位")]
    public string? ToLocation { get; set; }
    [ExporterHeader(displayName: "原数量")]
    public float? FromQty { get; set; }
    [ExporterHeader(displayName: "目标调整数量")]
    public float? ToQty { get; set; }
    [ExporterHeader(displayName: "调整数量")]
    public float? Qty { get; set; }
    [ExporterHeader(displayName: "原所属")]
    public string? FromOnwer { get; set; }
    [ExporterHeader(displayName: "目标所属")]
    public string? ToOnwer { get; set; }
    [ExporterHeader(displayName: "原品级")]
    public string? FromGoodsType { get; set; }
    [ExporterHeader(displayName: "目标品级")]
    public string? ToGoodsType { get; set; }
    [ExporterHeader(displayName: "原单位")]
    public string? FromUnitCode { get; set; }
    [ExporterHeader(displayName: "目标单位")]
    public string? ToUnitCode { get; set; }
    [ExporterHeader(displayName: "创建时间")]
    public DateTime? CreationTime { get; set; }
    [ExporterHeader(displayName: "创建用户")]
    public string? Creator { get; set; }
    [ExporterHeader(displayName: "修改时间")]
    public DateTime? UpdateTime { get; set; }
    [ExporterHeader(displayName: "修改用户")]
    public string? Updator { get; set; }
}
