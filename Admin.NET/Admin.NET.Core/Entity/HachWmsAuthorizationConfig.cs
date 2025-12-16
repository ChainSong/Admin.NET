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


[SugarTable("hach_wms_authorization_config", "哈希接口身份认证配置表")]
[IncreTable]
public class HachWmsAuthorizationConfig : EntityTenant
{
    /// <summary>
    /// 雪花id
    /// </summary>
    [SugarColumn(ColumnDescription = "雪花id", IsOnlyIgnoreUpdate = true, IsIdentity = true, IsPrimaryKey = true)]
    public override long Id { get; set; }
    [SugarColumn(ColumnDescription = "用户id", IsOnlyIgnoreUpdate = true)]
    public long AppId { get; set; }
    [SugarColumn(ColumnDescription = "秘钥", IsOnlyIgnoreUpdate = true)]
    public string AppSecret { get; set; }
    [SugarColumn(ColumnDescription = "类型", IsOnlyIgnoreUpdate = true)]
    public string? Type { get; set; }
    [SugarColumn(ColumnDescription = "接口名称", IsOnlyIgnoreUpdate = true)]
    public string InterFaceName { get; set; }
    [SugarColumn(ColumnDescription = "客户名称", IsOnlyIgnoreUpdate = true)]
    public string CustomerName { get; set; }
    [SugarColumn(ColumnDescription = "客户ID", IsOnlyIgnoreUpdate = true)]
    public long? CustomerId { get; set; }
    [SugarColumn(ColumnDescription = "仓库Id", IsOnlyIgnoreUpdate = true)]
    public long? WarehouseId { get; set; }
    [SugarColumn(ColumnDescription = "仓库名称", IsOnlyIgnoreUpdate = true)]
    public string WarehouseName { get; set; }

    [SugarColumn(ColumnDescription = "仓库代码", IsOnlyIgnoreUpdate = true)]
    public string WarehouseCode { get; set; }
    [SugarColumn(ColumnDescription = "状态", IsOnlyIgnoreUpdate = true)]
    public bool Status { get; set; } = true;

    [SugarColumn(ColumnDescription = "")]
    public int? int1 { get; set; }
}
