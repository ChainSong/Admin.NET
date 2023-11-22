using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSLowCode基础输入参数
/// </summary>
public class WMSLowCodeBaseInput
{
    /// <summary>
    /// Name
    /// </summary>
    public virtual string? MenuName { get; set; }

    /// <summary>
    /// UICode
    /// </summary>
    public virtual string? UICode { get; set; }

    public virtual string? SQLCode { get; set; }

    /// <summary>
    /// UIType
    /// </summary>
    public virtual string? UIType { get; set; }

    /// <summary>
    /// DataSource
    /// </summary>
    public virtual string? DataSource { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime? CreationTime { get; set; }

}

/// <summary>
/// WMSLowCode分页查询输入参数
/// </summary>
public class WMSLowCodeInput : BasePageInput
{
    /// <summary>
    /// Name
    /// </summary>
    public string? MenuName { get; set; }

    /// <summary>
    /// UICode
    /// </summary>
    public string? UICode { get; set; }
    public string? SQLCode { get; set; }

    /// <summary>
    /// UIType
    /// </summary>
    public string? UIType { get; set; }

    /// <summary>
    /// DataSource
    /// </summary>
    public string? DataSource { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// CreationTime范围
    /// </summary>
    public List<DateTime?> CreationTimeRange { get; set; }
}

/// <summary>
/// WMSLowCode增加输入参数
/// </summary>
public class AddWMSLowCodeInput : WMSLowCodeBaseInput
{
}

/// <summary>
/// WMSLowCode删除输入参数
/// </summary>
public class DeleteWMSLowCodeInput : BaseIdInput
{
    public string? MenuName { get; set; }
}

/// <summary>
/// WMSLowCode更新输入参数
/// </summary>
public class UpdateWMSLowCodeInput : WMSLowCodeBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSLowCode主键查询输入参数
/// </summary>
public class QueryByIdWMSLowCodeInput : DeleteWMSLowCodeInput
{

}
