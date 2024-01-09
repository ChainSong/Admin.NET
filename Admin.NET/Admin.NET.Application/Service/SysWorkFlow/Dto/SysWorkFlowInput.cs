using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// SysWorkFlow基础输入参数
/// </summary>
public class SysWorkFlowBaseInput
{
    /// <summary>
    /// WorkName
    /// </summary>
    public virtual string WorkName { get; set; }

    /// <summary>
    /// WorkTable
    /// </summary>
    public virtual string WorkTable { get; set; }

    /// <summary>
    /// WorkTableName
    /// </summary>
    public virtual string? WorkTableName { get; set; }

    /// <summary>
    /// NodeConfig
    /// </summary>
    public virtual string? NodeConfig { get; set; }

    /// <summary>
    /// LineConfig
    /// </summary>
    public virtual string? LineConfig { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// Weight
    /// </summary>
    public virtual int? Weight { get; set; }

    /// <summary>
    /// CreateDate
    /// </summary>
    public virtual DateTime? CreateDate { get; set; }

    /// <summary>
    /// CreateID
    /// </summary>
    public virtual int? CreateID { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// Enable
    /// </summary>
    public virtual bool? Enable { get; set; }

    /// <summary>
    /// Modifier
    /// </summary>
    public virtual string? Modifier { get; set; }

    /// <summary>
    /// ModifyDate
    /// </summary>
    public virtual DateTime? ModifyDate { get; set; }

    /// <summary>
    /// ModifyID
    /// </summary>
    public virtual int? ModifyID { get; set; }

    /// <summary>
    /// AuditingEdit
    /// </summary>
    public virtual int? AuditingEdit { get; set; }

}

/// <summary>
/// SysWorkFlow分页查询输入参数
/// </summary>
public class SysWorkFlowInput : BasePageInput
{
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
    /// CreateDate范围
    /// </summary>
    public List<DateTime?> CreateDateRange { get; set; }
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
    /// ModifyDate范围
    /// </summary>
    public List<DateTime?> ModifyDateRange { get; set; }
    /// <summary>
    /// ModifyID
    /// </summary>
    public int? ModifyID { get; set; }

    /// <summary>
    /// AuditingEdit
    /// </summary>
    public int? AuditingEdit { get; set; }

}

/// <summary>
/// SysWorkFlow增加输入参数
/// </summary>
public class AddSysWorkFlowInput //: SysWorkFlowBaseInput
{

    //public List<SysWorkFlow> MainData { get; set; }
    //public List<SysWorkFlowStep> DetailData { get; set; }
    public Dictionary<string, object> MainData { get; set; }
    public List<Dictionary<string, object>> DetailData { get; set; }
    public List<object> DelKeys { get; set; }

    /// <summary>
    /// 从前台传入的其他参数(自定义扩展可以使用)
    /// </summary>
    public object Extra { get; set; }
}

/// <summary>
/// SysWorkFlow删除输入参数
/// </summary>
public class DeleteSysWorkFlowInput : BaseIdInput
{
    /// <summary>
    /// WorkFlow_Id
    /// </summary>
    [Required(ErrorMessage = "WorkFlowId不能为空")]
    public long WorkFlowId { get; set; }

}

/// <summary>
/// SysWorkFlow更新输入参数
/// </summary>
public class UpdateSysWorkFlowInput : SysWorkFlowBaseInput
{
    /// <summary>
    /// WorkFlow_Id
    /// </summary>
    [Required(ErrorMessage = "WorkFlow_Id不能为空")]
    public long WorkFlowId { get; set; }

}

/// <summary>
/// SysWorkFlow主键查询输入参数
/// </summary>
public class QueryByIdSysWorkFlowInput : DeleteSysWorkFlowInput
{

}
