using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// 表管理基础输入参数
/// </summary>
public class TableColumnsBaseInput
{



    /// <summary>
    /// TenantId
    /// </summary>
    public virtual long? TenantId { get; set; }


    /// <summary>
    /// ProjectId
    /// </summary>
    public virtual long? ProjectId { get; set; }

    /// <summary>
    /// RoleName
    /// </summary>
    public virtual string? RoleName { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public virtual long? CustomerId { get; set; }

    /// <summary>
    /// TableName
    /// </summary>
    public virtual string? TableName { get; set; }

    /// <summary>
    /// TableNameCH
    /// </summary>
    public virtual string? TableNameCH { get; set; }

    /// <summary>
    /// DisplayName
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// DbColumnName
    /// </summary>
    public virtual string? DbColumnName { get; set; }

    /// <summary>
    /// IsKey
    /// </summary>
    public virtual int IsKey { get; set; }

    /// <summary>
    /// IsSearchCondition
    /// </summary>
    public virtual int IsSearchCondition { get; set; }

    /// <summary>
    /// IsHide
    /// </summary>
    public virtual int IsHide { get; set; }

    /// <summary>
    /// IsReadOnly
    /// </summary>
    public virtual int IsReadOnly { get; set; }

    /// <summary>
    /// IsShowInList
    /// </summary>
    public virtual int IsShowInList { get; set; }

    /// <summary>
    /// IsImportColumn
    /// </summary>
    public virtual int IsImportColumn { get; set; }

    /// <summary>
    /// IsDropDownList
    /// </summary>
    public virtual int IsDropDownList { get; set; }

    /// <summary>
    /// IsCreate
    /// </summary>
    public virtual int IsCreate { get; set; }

    /// <summary>
    /// IsUpdate
    /// </summary>
    public virtual int IsUpdate { get; set; }

    /// <summary>
    /// SearchConditionOrder
    /// </summary>
    public virtual int SearchConditionOrder { get; set; }

    /// <summary>
    /// 验证
    /// </summary>
    public virtual string? Validation { get; set; }

    /// <summary>
    /// Group
    /// </summary>
    public virtual string? Group { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    public virtual string? Type { get; set; }

    /// <summary>
    /// Characteristic
    /// </summary>
    public virtual string? Characteristic { get; set; }

    /// <summary>
    /// Order
    /// </summary>
    public virtual int Order { get; set; }

    /// <summary>
    /// Associated
    /// </summary>
    public virtual string? Associated { get; set; }

    /// <summary>
    /// 精确
    /// </summary>
    public virtual int? Precision { get; set; }

    /// <summary>
    /// 步骤  台阶
    /// </summary>
    public virtual double? Step { get; set; }

    /// <summary>
    /// 最大值
    /// </summary>
    public virtual double? Max { get; set; }

    /// <summary>
    /// 最小值
    /// </summary>
    public virtual double? Min { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public virtual string? Default { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    public virtual string? Link { get; set; }

    /// <summary>
    /// 关联
    /// </summary>
    public virtual string? RelationDBColumn { get; set; }

    /// <summary>
    /// ForView
    /// </summary>
    public virtual int? ForView { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime CreationTime { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public virtual string? Updator { get; set; }

}

/// <summary>
/// 表管理分页查询输入参数
/// </summary>
public class TableColumnsInput : BasePageInput
{


    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; } = 0;

    /// <summary>
    /// TenantId
    /// </summary>
    public long TenantId { get; set; } = 0;


    /// <summary>
    /// ProjectId
    /// </summary>
    public long ProjectId { get; set; } = 0;

    /// <summary>
    /// RoleName
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public long CustomerId { get; set; } = 0;

    /// <summary>
    /// TableName
    /// </summary>
    public string? TableName { get; set; }

    /// <summary>
    /// TableNameCH
    /// </summary>
    public string? TableNameCH { get; set; }

    /// <summary>
    /// DisplayName
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// DbColumnName
    /// </summary>
    public string? DbColumnName { get; set; }

    /// <summary>
    /// IsKey
    /// </summary>
    public int IsKey { get; set; } = 0;

    /// <summary>
    /// IsSearchCondition
    /// </summary>
    public int IsSearchCondition { get; set; } = 0;

    /// <summary>
    /// IsHide
    /// </summary>
    public int IsHide { get; set; } = 0;

    /// <summary>
    /// IsReadOnly
    /// </summary>
    public int IsReadOnly { get; set; } = 0;

    /// <summary>
    /// IsShowInList
    /// </summary>
    public int IsShowInList { get; set; } = 0;

    /// <summary>
    /// IsImportColumn
    /// </summary>
    public int IsImportColumn { get; set; } = 0;

    /// <summary>
    /// IsDropDownList
    /// </summary>
    public int IsDropDownList { get; set; } = 0;

    /// <summary>
    /// IsCreate
    /// </summary>
    public int IsCreate { get; set; } = 0;

    /// <summary>
    /// IsUpdate
    /// </summary>
    public int IsUpdate { get; set; } = 0;

    /// <summary>
    /// SearchConditionOrder
    /// </summary>
    public int SearchConditionOrder { get; set; } = 0;

    /// <summary>
    /// 验证
    /// </summary>
    public string? Validation { get; set; }

    /// <summary>
    /// Group
    /// </summary>
    public string? Group { get; set; }

    /// <summary>
    /// Type
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Characteristic
    /// </summary>
    public string? Characteristic { get; set; }

    /// <summary>
    /// Order
    /// </summary>
   new public int Order { get; set; } = 0;

    /// <summary>
    /// Associated
    /// </summary>
    public string? Associated { get; set; }

    /// <summary>
    /// 精确
    /// </summary>
    public int? Precision { get; set; }

    /// <summary>
    /// 步骤  台阶
    /// </summary>
    public double? Step { get; set; }

    /// <summary>
    /// 最大值
    /// </summary>
    public double? Max { get; set; }

    /// <summary>
    /// 最小值
    /// </summary>
    public double? Min { get; set; }

    /// <summary>
    /// 默认值
    /// </summary>
    public string? Default { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// 关联
    /// </summary>
    public string? RelationDBColumn { get; set; }

    /// <summary>
    /// ForView
    /// </summary>
    public int? ForView { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime[]? CreationTime { get; set; }

    /// <summary>
    /// CreationTime范围
    /// </summary>
    public DateTime[]? CreationTimeRange { get; set; }
    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

}

/// <summary>
/// 表管理增加输入参数
/// </summary>
public class AddTableColumnsInput : TableColumnsBaseInput
{
}

/// <summary>
/// 表管理删除输入参数
/// </summary>
public class DeleteTableColumnsInput : BaseIdInput
{
}

/// <summary>
/// 表管理更新输入参数
/// </summary>
public class UpdateTableColumnsInput : TableColumnsBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    //[Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

    public  List<TableColumnsDetail> tableColumnsDetails { get; set; } = new List<TableColumnsDetail>();

}

/// <summary>
/// 表管理主键查询输入参数
/// </summary>
public class QueryByIdTableColumnsInput : DeleteTableColumnsInput
{

}
