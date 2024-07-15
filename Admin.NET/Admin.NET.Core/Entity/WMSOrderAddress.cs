namespace Admin.NET.Core.Entity;

/// <summary>
/// 出库地址信息
/// </summary>
[SugarTable("WMS_OrderAddress","出库地址信息")]
public class WMSOrderAddress : ITenantIdFilter
{
  

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
    public long PreOrderId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PreOrderNumber { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ExternOrderNumber { get; set; }
    
    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "联系人", Length = 50)]
    public string? Name { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CompanyName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? AddressTag { get; set; }
    
    /// <summary>
    /// 联系电话
    /// </summary>
    [SugarColumn(ColumnDescription = "联系电话", Length = 50)]
    public string? Phone { get; set; }
    
    /// <summary>
    /// 邮编
    /// </summary>
    [SugarColumn(ColumnDescription = "邮编", Length = 50)]
    public string? ZipCode { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? Province { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? City { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? Country { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? County { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? Address { get; set; }
    
    /// <summary>
    /// 是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求
    /// </summary>
    [SugarColumn(ColumnDescription = "是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求")]
    public int? IsSignBack { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? ExpressCompany { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ExpressNumber { get; set; }
    
    /// <summary>
    /// 付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付
    /// </summary>
    [SugarColumn(ColumnDescription = "付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付", Length = 50)]
    public string? PayMethod { get; set; }
    
    /// <summary>
    /// 快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取
    /// </summary>
    [SugarColumn(ColumnDescription = "快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取", Length = 50)]
    public string? IsOneselfPickup { get; set; }
    
    /// <summary>
    /// 快件产品类别表 https://open.sf-express.com/developSupport/734349?activeIndex=324604
    /// </summary>
    [SugarColumn(ColumnDescription = "快件产品类别表 https://open.sf-express.com/developSupport/734349?activeIndex=324604", Length = 50)]
    public string? ExpressTypeId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Creator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
 
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Updator { get; set; }


    /// <summary>
    /// 
    /// </summary>

    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id")]
    public long? TenantId { get; set; }
    
}
