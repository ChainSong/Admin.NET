namespace Admin.NET.Core.Entity;

/// <summary>
/// 工作流
/// </summary>
[SugarTable("SysWorkFlow","工作流")]
public class SysWorkFlow  //: EntityBase
{




    /// <summary>
    /// 
    /// </summary>
    //[Required]
    [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true, ColumnDescription = "主键")]
    //[SugarColumn(IsIdentity = true, ColumnDescription = "", IsPrimaryKey = true)]
    public long Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    //[Required]
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string WorkName { get; set; }



    /// <summary>
    /// 
    /// </summary>
    //[Required]
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string WorkTable { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? WorkTableName { get; set; }


    /// <summary>
    /// 
    /// </summary>
    //[Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string WorkType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = -1)]
    public string? NodeConfig { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = -1)]
    public string? LineConfig { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Weight { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreateDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? CreateID { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 30)]
    public string? Creator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public bool? Enable { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 30)]
    public string? Modifier { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ModifyDate { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? ModifyID { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? AuditingEdit { get; set; }

    //[Navigate(NavigateType.OneToMany, nameof(SysWorkFlowStep.Id), nameof(Associated))]

    [Navigate(NavigateType.OneToMany, nameof(SysWorkFlowStep.WorkFlowId))]
    public List<SysWorkFlowStep> SysWorkFlowSteps { get; set; }
 


    
}
