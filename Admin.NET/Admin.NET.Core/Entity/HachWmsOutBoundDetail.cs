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

namespace Admin.NET.Core.Entity;

[SugarTable("hach_wms_outBound_detail", "出库对接明细数据表")]
public class HachWmsOutBoundDetail : EntityTenant
{
    /// <summary>
    /// 雪花id
    /// </summary>
    [SugarColumn(ColumnDescription = "雪花id", IsOnlyIgnoreUpdate = true, IsIdentity = true, IsPrimaryKey = true)]
    public override long Id { get; set; }
    /// <summary>
    /// 主表Id
    /// </summary>
    [SugarColumn(ColumnDescription = "主表Id", IsNullable = false)]
    public long OutBoundId { get; set; }
    [SugarColumn(ColumnDescription = "发运Id", IsNullable = false)]
    public long DeliveryDetailId { get; set; }
    [SugarColumn(ColumnDescription = "行id", IsNullable = false)]
    public long LineId { get; set; }
    [SugarColumn(ColumnDescription = "行号", IsNullable = false)]
    public string LineNumber { get; set; }
    [SugarColumn(ColumnDescription = "商品id", IsNullable = false)]
    public long ItemId { get; set; }
    [SugarColumn(ColumnDescription = "商品编码", IsNullable = false)]
    public string ItemNumber { get; set; }
    [SugarColumn(ColumnDescription = "商品描述", IsNullable = false)]
    public string ItemDescription { get; set; }
    [SugarColumn(ColumnDescription = "账套ID", IsNullable = false)]
    public long OrganizationId { get; set; }
    [SugarColumn(ColumnDescription = "账套编码", IsNullable = false)]
    public string OrganizationCode { get; set; }
    [SugarColumn(ColumnDescription = "数量", IsNullable = false)]
    public string Quantity { get; set; }
    [SugarColumn(ColumnDescription = "单位", IsNullable = false)]
    public string Uom { get; set; }
    [SugarColumn(ColumnDescription = "父阶商品编码", IsNullable = false)]
    public string? ParentItemNumber { get; set; }
    [SugarColumn(ColumnDescription = "父阶商品id")]
    public int? ParentItemId { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 240)]
    public string? Attribute1 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 240)]
    public string? Attribute2 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 240)]
    public string? Attribute3 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 240)]
    public string? Attribute4 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 240)]
    public string? Attribute5 { get; set; }
}
