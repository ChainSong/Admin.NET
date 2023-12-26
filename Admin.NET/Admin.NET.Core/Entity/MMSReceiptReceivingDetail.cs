namespace Admin.NET.Core.Entity;

/// <summary>
/// 物料上架明细表
/// </summary>
[SugarTable("MMS_ReceiptReceivingDetail","物料上架明细表")]
[IncreTableAttribute]
public class MMSReceiptReceivingDetail : ITenantIdFilter
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
    public virtual long ReceiptReceivingId { get; set; }


    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long ReceiptId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long ReceiptDetailId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string ReceiptNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string ReceiptReceivingNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PurchaseOrderNumber { get; set; }
    
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
    public long SupplierId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string SupplierName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string LineNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string SKU { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? UPC { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string GoodsType { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string GoodsName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? BoxCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? TrayCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? BatchCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? LotCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PoCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public double? Weight { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public double? Volume { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public double ExpectedQty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public double ReceivedQty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public double ReceiptQty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? UnitCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Onwer { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Area { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Location { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int GoodsStatus { get; set; }


    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ProductionDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ExpirationDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
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
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str11 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str12 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str13 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str14 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str15 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str16 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str17 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str18 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str19 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str20 { get; set; }
    
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
    
}
