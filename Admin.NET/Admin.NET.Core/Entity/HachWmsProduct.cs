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

[SugarTable("HachWms_Product", "产品主数据对接表")]
public class HachWmsProduct : EntityTenant
{
    /// <summary>
    /// 雪花id
    /// </summary>
    [SugarColumn(ColumnDescription = "雪花id", IsOnlyIgnoreUpdate = true)]
    public override long Id { get; set; }
    /// <summary>
    /// 组织ID
    /// </summary>
    [SugarColumn(ColumnDescription = "组织ID", Length = 11, IsNullable = false)]
    public int OrganizationId { get; set; }
    /// <summary>
    /// 组织编码
    /// </summary>
    [SugarColumn(ColumnDescription = "组织编码", Length = 50, IsNullable = false)]
    public string OrganizationCode { get; set; }
    /// <summary>
    /// 产品ID
    /// </summary>
    [SugarColumn(ColumnDescription = "产品ID", Length = 11, IsNullable = false)]
    public int InventoryItemId { get; set; }
    /// <summary>
    /// 产品代码
    /// </summary>
    [SugarColumn(ColumnDescription = "产品代码", Length = 50, IsNullable = false)]
    public string ItemNumber { get; set; }
    /// <summary>
    /// 产品单位
    /// </summary>
    [SugarColumn(ColumnDescription = "产品单位", Length = 50, IsNullable = false)]
    public string PrimaryUomCode { get; set; }
    /// <summary>
    /// 英文描述
    /// </summary>
    [SugarColumn(ColumnDescription = "英文描述", Length = 240)]
    public string DescriptionEn { get; set; }
    /// <summary>
    /// 中文描述
    /// </summary>
    [SugarColumn(ColumnDescription = "中文描述", Length = 400 )]
    public string DescriptionZhs { get; set; }
    /// <summary>
    /// 长中文描述
    /// </summary>
    [SugarColumn(ColumnDescription = "长中文描述", Length = 4000)]
    public string LongDescriptionZhs { get; set; }
    /// <summary>
    /// 长英文描述
    /// </summary>
    [SugarColumn(ColumnDescription = "长英文描述", Length = 4000)]
    public string LongDescriptionEn { get; set; }
    /// <summary>
    /// 是否BOM
    /// </summary>
    [SugarColumn(ColumnDescription = "是否BOM", Length = 20)]
    public string BomFlag { get; set; }
    /// <summary>
    /// 产品类别
    /// </summary>
    [SugarColumn(ColumnDescription = "产品类别", Length = 255)]
    public string ItemType { get; set; }
    /// <summary>
    /// 产品状态
    /// </summary>
    [SugarColumn(ColumnDescription = "产品状态", Length = 255)]
    public string ItemStatus { get; set; }
    /// <summary>
    /// 制造件还是采购件
    /// </summary>
    [SugarColumn(ColumnDescription = "制造件还是采购件", Length = 255)]
    public string MakeOrBuy { get; set; }
    /// <summary>
    /// 危化品标识
    /// </summary>
    [SugarColumn(ColumnDescription = "危化品标识", Length = 255)]
    public string DangerousGoodsFlag { get; set; }
    /// <summary>
    /// 危险运输标识
    /// </summary>
    [SugarColumn(ColumnDescription = "危险运输标识", Length = 255)]
    public string HazardClass { get; set; }
    /// <summary>
    /// 品牌
    /// </summary>
    [SugarColumn(ColumnDescription = "品牌", Length = 255)]
    public string Brand { get; set; }
    /// <summary>
    /// 长英文描述
    /// </summary>
    [SugarColumn(ColumnDescription = "长英文描述", Length = 255)]
    public string CountryOfOrigin { get; set; }
    /// <summary>
    /// TSS编码，产品维度编码
    /// </summary>
    [SugarColumn(ColumnDescription = "TSS编码，产品维度编码", Length = 255)]
    public string TssCode { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string blockedDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 11)]
    public int LeadTime { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute1 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute2 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute3 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute4 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute5 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute6 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute7 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute8 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute9 { get; set; }
    [SugarColumn(ColumnDescription = "", Length = 255)]
    public string Attribute10 { get; set; }
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }
}
