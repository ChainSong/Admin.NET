namespace Admin.NET.Core.Entity;

/// <summary>
/// ASN质检点数主表
/// </summary>
[SugarTable("WMS_ASNCountQuantity","ASN质检点数主表")]
public class WMSASNCountQuantity : ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }


    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long ASNId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string ASNNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string ASNCountQuantityNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ExternReceiptNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
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
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ExpectDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int ASNCountQuantityStatus { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string ReceiptType { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Contact { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ContactInfo { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CompleteTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Po { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? So { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }
    
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
    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }
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

    [Navigate(NavigateType.OneToMany, nameof(WMSASNCountQuantityDetail.ASNCountQuantityId))]
    public virtual List<WMSASNCountQuantityDetail> Details { get; set; }

}
