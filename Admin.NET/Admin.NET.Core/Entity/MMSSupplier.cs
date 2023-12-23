namespace Admin.NET.Core.Entity;

/// <summary>
/// 供应商管理
/// </summary>
[SugarTable("MMS_Supplier","供应商管理")]
[IncreTableAttribute]
public class MMSSupplier :  ITenantIdFilter
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
    public long ProjectId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? SupplierCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? SupplierName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Description { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? SupplierType { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int SupplierStatus { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CreditLine { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Province { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? City { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Address { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Email { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? LawPerson { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PostCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Bank { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Account { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? TaxId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? InvoiceTitle { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Fax { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? WebSite { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Updator { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }

}
