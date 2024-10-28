namespace Admin.NET.Core.Entity;

/// <summary>
/// 库存盘点主表
/// </summary>
[SugarTable("WMS_StockCheck", "库存盘点主表")]
public class WMSStockCheck : ITenantIdFilter
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
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? StockCheckNumber { get; set; }

    /// <summary>
    /// 外部单号
    /// </summary>
    [SugarColumn(ColumnDescription = "外部单号", Length = 50)]
    public string? ExternNumber { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "客户ID")]
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "客户名称", Length = 50)]
    public string CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public DateTime StockCheckDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? StockCheckType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? StockCheckStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ToCheckUser { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ToCheckAccount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [SugarColumn(ColumnDescription = "备注", Length = 50)]
    public string? Remark { get; set; }

    /// <summary>
    /// 是否存在差异(默认0 1为无差异 0为存在差异)
    /// </summary>
    [SugarColumn(ColumnDescription = "是否存在差异(默认0 1为无差异 0为存在差异)", Length = 50)]
    public string? Is_Difference { get; set; }

    /// <summary>
    /// 差异是否处理（X，Y ）
    /// </summary>
    [SugarColumn(ColumnDescription = "差异是否处理（X，Y ）", Length = 50)]
    public string? Is_Deal { get; set; }

    /// <summary>
    /// 差异是否处理（X，Y ）
    /// </summary>
    [SugarColumn(ColumnDescription = "是否盲盘", Length = 50)]
    public string? Is_Blind { get; set; }



    /// <summary>
    /// 创建人
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "创建人", Length = 50)]
    public string Creator { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "创建时间")]
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 更新人
    /// </summary>
    [SugarColumn(ColumnDescription = "更新人", Length = 50)]
    public string? Updator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str6 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str7 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str8 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str9 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str10 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int5 { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(WMSStockCheckDetail.StockCheckId))]
    public List<WMSStockCheckDetail> Details { get; set; }

}
