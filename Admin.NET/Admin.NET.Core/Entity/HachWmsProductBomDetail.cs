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

[SugarTable("hach_wms_product_bom_detail", "产品Bom数据对接明细表")]
public class HachWmsProductBomDetail : EntityTenant
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
    public long BomId { get; set; }
    /// <summary>
    /// 组织id
    /// </summary>
    [SugarColumn(ColumnDescription = "组织id", IsNullable = false)]
    public long OrganizationId { get; set; }
    /// <summary>
    /// 组织编码
    /// </summary>
    [SugarColumn(ColumnDescription = "组织编码", Length = 255)]
    public string? OrganizationCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", IsNullable = false)]
    public long AssemblyItemId { get; set; }
    /// <summary>
    /// BOM描述
    /// </summary>
    [SugarColumn(ColumnDescription = "BOM描述", IsNullable = false, Length = 255)]
    public string? BomDescription { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public string? ComponentSeq { get; set; } = "";
    [SugarColumn(ColumnDescription = "", IsNullable = false, Length = 255)]
    public string ComponentItem { get; set; }
    [SugarColumn(ColumnDescription = "", IsNullable = false, Length = 255)]
    public string ComponentDesc { get; set; } = "";
    [SugarColumn(ColumnDescription = "", IsNullable = false, Length = 255)]
    public string ComponentUom { get; set; } = "";
    [SugarColumn(ColumnDescription = "", IsNullable = false)]
    public long ComponentQuantity { get; set; }
    [SugarColumn(ColumnDescription = "", IsNullable = false)]
    public DateTime DateFrom { get; set; } = DateTime.Now;
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTo { get; set; } = DateTime.Now;
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute1 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute2 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute3 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute4 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute5 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute6 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute7 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute8 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute9 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string? Attribute10 { get; set; }
}
