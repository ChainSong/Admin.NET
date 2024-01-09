namespace Admin.NET.Core.Entity;

/// <summary>
/// 出库地址信息
/// </summary>
[SugarTable("WMS_OrderAddress","出库地址信息")]
[IncreTableAttribute]
public class WMSOrderAddress : ITenantIdFilter
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
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "单位名称", Length = 50)]
    public string? CompanyName { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    [SugarColumn(ColumnDescription = "地址标签", Length = 50)]
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
    /// 国家
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? Country { get; set; }
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
    /// 区县
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 20)]
    public string? County { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? Address { get; set; }
    
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
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Creator { get; set; } = "";
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string Updator { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }
    
}
