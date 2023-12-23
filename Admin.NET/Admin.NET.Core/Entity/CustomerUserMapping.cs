namespace Admin.NET.Core.Entity;

/// <summary>
/// 客户用户关系
/// </summary>
[SugarTable("Customer_User_Mapping","客户用户关系")]
public class CustomerUserMapping : ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }


    [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
    public long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long UserId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string UserName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int Status { get; set; }
    
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
    
}
