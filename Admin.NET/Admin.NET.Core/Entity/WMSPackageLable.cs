namespace Admin.NET.Core.Entity;

/// <summary>
/// 包装信息前置打印
/// </summary>
[SugarTable("WMS_PackageLable", "包装信息前置打印")]
public class WMSPackageLable : ITenantIdFilter
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

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long OrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PreOrderNumber { get; set; }

    /// <summary>
    /// 包装单号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "包装单号", Length = 50)]
    public string PackageNumber { get; set; }

    /// <summary>
    /// 出库单号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "出库单号", Length = 50)]
    public string OrderNumber { get; set; }

    /// <summary>
    /// 出库外部编号
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "出库外部编号", Length = 50)]
    public string ExternOrderNumber { get; set; }

    /// <summary>
    /// 货主ID
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "货主ID")]
    public long CustomerId { get; set; }

    /// <summary>
    /// 货主名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "货主名称", Length = 50)]
    public string CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 出库仓库
    /// </summary>
    [SugarColumn(ColumnDescription = "出库仓库", Length = 50)]
    public string? WarehouseName { get; set; }

    /// <summary>
    /// 包装类型
    /// </summary>
    [SugarColumn(ColumnDescription = "包装类型", Length = 50)]
    public string? PackageType { get; set; }

    /// <summary>
    /// 长
    /// </summary>
    [SugarColumn(ColumnDescription = "长")]
    public double? Length { get; set; }

    /// <summary>
    /// 宽
    /// </summary>
    [SugarColumn(ColumnDescription = "宽")]
    public double? Width { get; set; }

    /// <summary>
    /// 高
    /// </summary>
    [SugarColumn(ColumnDescription = "高")]
    public double? Height { get; set; }

    /// <summary>
    /// 净重
    /// </summary>
    [SugarColumn(ColumnDescription = "净重")]
    public double? NetWeight { get; set; }

    /// <summary>
    /// 毛重
    /// </summary>
    [SugarColumn(ColumnDescription = "毛重")]
    public double? GrossWeight { get; set; }

    /// <summary>
    /// 快递公司
    /// </summary>
    [SugarColumn(ColumnDescription = "快递公司", Length = 50)]
    public string? ExpressCompany { get; set; }

    /// <summary>
    /// 快递单号
    /// </summary>
    [SugarColumn(ColumnDescription = "快递单号", Length = 50)]
    public string? ExpressNumber { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    [SugarColumn(ColumnDescription = "序号", Length = 50)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// 打印次数
    /// </summary>
    [SugarColumn(ColumnDescription = "打印次数")]
    public int? PrintNum { get; set; }

    /// <summary>
    /// 打印人
    /// </summary>
    [SugarColumn(ColumnDescription = "打印人", Length = 255)]
    public string? PrintPersonnel { get; set; }

    /// <summary>
    /// 打印时间
    /// </summary>
    [SugarColumn(ColumnDescription = "打印时间")]
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Updator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }

}
