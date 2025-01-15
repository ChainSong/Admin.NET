namespace Admin.NET.Core.Entity;

/// <summary>
/// 质检点数明细表
/// </summary>
public class WMSASNCountQuantityDetailDto 
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long? TenantId { get; set; }

    public virtual string? ScanInput { get; set; }
    public virtual string? ExpirationDate { get; set; }

    
    /// <summary>
    /// 雪花Id
    /// </summary>
    public virtual long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long ASNCountQuantityId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public long ASNId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public long ASNDetailId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public string ASNNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? ExternReceiptNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public string LineNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public string SKU { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? UPC { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? GoodsType { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? GoodsName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? BoxCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? TrayCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? BatchCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? LotCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? PoCode { get; set; }
    public string? SoCode { get; set; }
    public string? SnCode { get; set; }

    /// <summary>
    /// 
    /// </summary>


    public double Weight { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public double Volume { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public double ExpectedQty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public double ReceivedQty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public double Qty { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? UnitCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Onwer { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    //public DateTime? ProductionDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    //public DateTime? ExpirationDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
   
    public string? Remark { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public string Creator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    
    public DateTime CreationTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Updator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str5 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str6 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str8 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str9 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str10 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str11 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str12 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str13 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str14 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str15 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str16 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str17 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str18 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str19 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public string? Str20 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? DateTime1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? DateTime2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? DateTime3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? DateTime4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? DateTime5 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public int? Int1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public int? Int2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public int? Int3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public int? Int4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    
    public int? Int5 { get; set; }
    

    
}
