namespace Admin.NET.Core.Entity;

/// <summary>
/// 客户表
/// </summary>
[SugarTable("WMS_CustomerConfig", "客户配置表")]
[IncreTableAttribute]
public class WMSCustomerConfig : ITenantIdFilter
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
    /// 客户代码
    /// </summary>
    [SugarColumn(ColumnDescription = "CustomerId")]
    public long? CustomerId { get; set; }

    /// <summary>
    /// 客户代码
    /// </summary>
    [SugarColumn(ColumnDescription = "客户代码", Length = 50)]
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "客户名称", Length = 50)]
    public string? CustomerName { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "客户Logo", Length = 100)]
    public string? CustomerLogo { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "发货单模板", Length = 50)]
    public string? PrintShippingTemplate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    //[Required]
    //[SugarColumn(ColumnDescription = "")]
    //public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Updator { get; set; }


    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarColumn(ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true)]
    public virtual DateTime? CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", IsOnlyIgnoreInsert = true)]
    public virtual DateTime? UpdateTime { get; set; } 
}
