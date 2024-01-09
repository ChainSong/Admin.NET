namespace Admin.NET.Application;

/// <summary>
/// SysWorkFlow输出参数
/// </summary>
public class SysWorkFlowDto
{

    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// WorkFlow_Id
    /// </summary>
    public long WorkFlowId { get; set; }

    /// <summary>
    /// WorkName
    /// </summary>
    public string WorkName { get; set; }

    /// <summary>
    /// WorkTable
    /// </summary>
    public string WorkTable { get; set; }

    /// <summary>
    /// WorkTableName
    /// </summary>
    public string? WorkTableName { get; set; }

    /// <summary>
    /// NodeConfig
    /// </summary>
    public string? NodeConfig { get; set; }

    /// <summary>
    /// LineConfig
    /// </summary>
    public string? LineConfig { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Weight
    /// </summary>
    public int? Weight { get; set; }

    /// <summary>
    /// CreateDate
    /// </summary>
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// CreateID
    /// </summary>
    public int? CreateID { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// Enable
    /// </summary>
    public bool? Enable { get; set; }

    /// <summary>
    /// Modifier
    /// </summary>
    public string? Modifier { get; set; }

    /// <summary>
    /// ModifyDate
    /// </summary>
    public DateTime? ModifyDate { get; set; }

    /// <summary>
    /// ModifyID
    /// </summary>
    public int? ModifyID { get; set; }

    /// <summary>
    /// AuditingEdit
    /// </summary>
    public int? AuditingEdit { get; set; }

}
