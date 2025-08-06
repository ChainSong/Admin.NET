// 麻省理工学院许可证
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Core.Entity;
/// <summary>
/// 可用库存快照
/// </summary>
[SugarTable("WMS_Inventory_Usable_Snapshot", "可用库存快照")]
public class WMSInventoryUsableSnapshot : ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }

    [SugarColumn(ColumnName = "InventoryId")]
    public long InventoryId { get; set; }

    [SugarColumn(ColumnName = "ReceiptDetailId")]
    public long ReceiptDetailId { get; set; }

    [SugarColumn(ColumnName = "ReceiptReceivingId")]
    public long ReceiptReceivingId { get; set; }

    [SugarColumn(ColumnName = "CustomerId")]
    public long CustomerId { get; set; }

    [SugarColumn(ColumnName = "CustomerName")]
    public string CustomerName { get; set; }

    [SugarColumn(ColumnName = "WarehouseId")]
    public long WarehouseId { get; set; }

    [SugarColumn(ColumnName = "WarehouseName")]
    public string WarehouseName { get; set; }

    [SugarColumn(ColumnName = "Area")]
    public string Area { get; set; }

    [SugarColumn(ColumnName = "Location")]
    public string Location { get; set; }

    [SugarColumn(ColumnName = "SKU")]
    public string SKU { get; set; }

    [SugarColumn(ColumnName = "UPC")]
    public string UPC { get; set; }

    [SugarColumn(ColumnName = "GoodsType")]
    public string GoodsType { get; set; }

    [SugarColumn(ColumnName = "InventoryStatus")]
    public int InventoryStatus { get; set; }

    [SugarColumn(ColumnName = "SuperId")]
    public long SuperId { get; set; }

    [SugarColumn(ColumnName = "RelatedId")]
    public long RelatedId { get; set; }

    [SugarColumn(ColumnName = "GoodsName")]
    public string GoodsName { get; set; }

    [SugarColumn(ColumnName = "UnitCode")]
    public string UnitCode { get; set; }

    [SugarColumn(ColumnName = "Onwer")]
    public string Onwer { get; set; }

    [SugarColumn(ColumnName = "BoxCode")]
    public string BoxCode { get; set; }

    [SugarColumn(ColumnName = "TrayCode")]
    public string TrayCode { get; set; }

    [SugarColumn(ColumnName = "BatchCode")]
    public string BatchCode { get; set; }

    [SugarColumn(ColumnName = "LotCode", IsNullable = true)]
    public string LotCode { get; set; }

    [SugarColumn(ColumnName = "PoCode", IsNullable = true)]
    public string PoCode { get; set; }

    [SugarColumn(ColumnName = "Weight")]
    public double Weight { get; set; }

    [SugarColumn(ColumnName = "Volume")]
    public double Volume { get; set; }

    [SugarColumn(ColumnName = "Qty")]
    public double Qty { get; set; }

    [SugarColumn(ColumnName = "ProductionDate", IsNullable = true)]
    public DateTime? ProductionDate { get; set; }

    [SugarColumn(ColumnName = "ExpirationDate", IsNullable = true)]
    public DateTime? ExpirationDate { get; set; }

    [SugarColumn(ColumnName = "Remark")]
    public string Remark { get; set; }

    [SugarColumn(ColumnName = "InventoryTime", IsNullable = true)]
    public DateTime? InventoryTime { get; set; }

    [SugarColumn(ColumnName = "InventorySnapshotTime", IsNullable = true)]
    public DateTime? InventorySnapshotTime { get; set; }

    [SugarColumn(ColumnName = "Creator")]
    public string Creator { get; set; }

    [SugarColumn(ColumnName = "CreationTime", IsNullable = true)]
    public DateTime? CreationTime { get; set; }

    [SugarColumn(ColumnName = "Updator")]
    public string Updator { get; set; }

    [SugarColumn(ColumnName = "UpdateTime", IsNullable = true)]
    public DateTime? UpdateTime { get; set; }

    [SugarColumn(ColumnName = "Str1")]
    public string Str1 { get; set; }

    [SugarColumn(ColumnName = "Str2")]
    public string Str2 { get; set; }

    [SugarColumn(ColumnName = "Str3")]
    public string Str3 { get; set; }

    [SugarColumn(ColumnName = "Str4")]
    public string Str4 { get; set; }

    [SugarColumn(ColumnName = "Str5")]
    public string Str5 { get; set; }

    [SugarColumn(ColumnName = "DateTime1", IsNullable = true)]
    public DateTime? DateTime1 { get; set; }

    [SugarColumn(ColumnName = "DateTime2", IsNullable = true)]
    public DateTime? DateTime2 { get; set; }

    [SugarColumn(ColumnName = "Int1")]
    public int Int1 { get; set; }

    [SugarColumn(ColumnName = "Int2")]
    public int Int2 { get; set; }
}
