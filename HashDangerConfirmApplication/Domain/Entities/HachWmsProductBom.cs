// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlaApplication.Models;

namespace TaskPlaApplication.Domain.Entities;

[SugarTable("hach_wms_product_bom", "产品Bom数据对接主表")]
public class HachWmsProductBom : EntityBase
{
    /// <summary>
    /// 雪花id
    /// </summary>
    [SugarColumn(ColumnDescription = "雪花id", IsOnlyIgnoreUpdate = true, IsIdentity = true, IsPrimaryKey = true)]
    public override long Id { get; set; }
    /// <summary>
    /// 组织编码
    /// </summary>
    [SugarColumn(ColumnDescription = "产品编码", Length = 400)]
    public string? ItemNumber { get; set; }
    /// <summary>
    /// 单位编码
    /// </summary>
    [SugarColumn(ColumnDescription = "单位编码", Length = 255)]
    public string? UomCode { get; set; }

    [SugarColumn(ColumnDescription = "状态")]
    public bool Status { get; set; } = true;
    [SugarColumn(ColumnDescription = "接收时间", IsOnlyIgnoreUpdate = true)]
    public DateTime? ReceivingTime { get; set; } = DateTime.Now;
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(HachWmsProductBomDetail.BomId))]
    public virtual List<HachWmsProductBomDetail> items { get; set; }
}
