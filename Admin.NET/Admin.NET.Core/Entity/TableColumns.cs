namespace Admin.NET.Core.Entity;

/// <summary>
/// 自定义字段
/// </summary>
[SugarTable("TableColumns", "TableColumns")]
[IncreTableAttribute]
public class TableColumns : ITenantIdFilter
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
    //[SugarColumn(ColumnDescription = "")]
    //public long TenantId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    //[SugarColumn(ColumnDescription = "")]
    public long ProjectId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? RoleName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    //[SugarColumn(ColumnDescription = "")]
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? TableName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? TableNameCH { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? DisplayName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? DbColumnName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsKey { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsSearchCondition { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsHide { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsReadOnly { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsShowInList { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsImportColumn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsDropDownList { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsCreate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int IsUpdate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int SearchConditionOrder { get; set; }
    
    /// <summary>
    /// 验证
    /// </summary>
    [SugarColumn(ColumnDescription = "验证", Length = 50)]
    public string? Validation { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Group { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Type { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Characteristic { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int Order { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Associated { get; set; }
    
    /// <summary>
    /// 精确
    /// </summary>
    [SugarColumn(ColumnDescription = "精确")]
    public int? Precision { get; set; }
    
    /// <summary>
    /// 步骤  台阶
    /// </summary>
    [SugarColumn(ColumnDescription = "步骤  台阶")]
    public double? Step { get; set; }
    
    /// <summary>
    /// 最大值
    /// </summary>
    [SugarColumn(ColumnDescription = "最大值")]
    public double? Max { get; set; }
    
    /// <summary>
    /// 最小值
    /// </summary>
    [SugarColumn(ColumnDescription = "最小值")]
    public double? Min { get; set; }
    
    /// <summary>
    /// 默认值
    /// </summary>
    [SugarColumn(ColumnDescription = "默认值", Length = 50)]
    public string? Default { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Link { get; set; }
    
    /// <summary>
    /// 关联
    /// </summary>
    [SugarColumn(ColumnDescription = "关联", Length = 50)]
    public string? RelationDBColumn { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? ForView { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(TableColumnsDetail.Associated),nameof(Associated))]
    public virtual List<TableColumnsDetail> tableColumnsDetails { get; set; }


    //public List<WMSOrderAllocation> Allocation { get; set; }

}
