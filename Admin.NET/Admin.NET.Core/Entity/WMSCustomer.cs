namespace Admin.NET.Core.Entity;

/// <summary>
/// 客户表
/// </summary>
[SugarTable("WMS_Customer", "客户表")]
[IncreTableAttribute]
public class WMSCustomer : IDeletedFilter, ITenantIdFilter
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

    [SugarColumn(ColumnDescription = "")]
    public long ProjectId { get; set; }

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
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CustomerType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int CustomerStatus { get; set; }

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

    ///// <summary>
    ///// 创建者Id
    ///// </summary>
    //[SugarColumn(ColumnDescription = "创建者Id", IsOnlyIgnoreUpdate = true)]
    //public virtual long? CreateUserId { get; set; }

    ///// <summary>
    ///// 修改者Id
    ///// </summary>
    //[SugarColumn(ColumnDescription = "修改者Id", IsOnlyIgnoreInsert = true)]
    //public virtual long? UpdateUserId { get; set; }

    /// <summary>
    /// 软删除
    /// </summary>
    [SugarColumn(ColumnDescription = "软删除")]
    public virtual bool IsDelete { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    //[SugarColumn(ColumnDescription = "", Length = 50)]

    [Navigate(NavigateType.OneToMany, nameof(WMSCustomerDetail.CustomerId))]

    public virtual List<WMSCustomerDetail> Details { get; set; }


    [Navigate(NavigateType.OneToOne, nameof(Id), nameof(WMSCustomerConfig.CustomerId))]
    public WMSCustomerConfig  CustomerConfig { get; set; }

    //[Navigate(NavigateType.OneToOne, nameof(Id), nameof(WMSOrderAddress.PreOrderId))]
    //public WMSOrderAddress OrderAddress { get; set; }
}
