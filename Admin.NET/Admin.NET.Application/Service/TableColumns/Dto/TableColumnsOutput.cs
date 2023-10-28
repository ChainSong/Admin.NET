using Admin.NET.Core.Entity;
using System.Collections.Generic;

namespace Admin.NET.Application;

/// <summary>
/// 表管理输出参数
/// </summary>
public class TableColumnsOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ProjectId
    /// </summary>
    public long? ProjectId { get; set; }
    public long? TenantId { get; set; }

    /// <summary>
    /// RoleName
    /// </summary>
    public string? RoleName { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public long? CustomerId { get; set; }

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


    public string ColumnName
    {
        get
        {
            var resolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            return resolver.GetResolvedPropertyName(DbColumnName);
        }
    }
    public string RelationColumn
    {
        get
        {
            var resolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            return resolver.GetResolvedPropertyName(RelationDBColumn);
        }
    }

    /// <summary>
    /// IsKey
    /// </summary>
    public int IsKey { get; set; }

    /// <summary>
    /// IsSearchCondition
    /// </summary>
    public int IsSearchCondition { get; set; }

    /// <summary>
    /// IsHide
    /// </summary>
    public int IsHide { get; set; }

    /// <summary>
    /// IsReadOnly
    /// </summary>
    public int IsReadOnly { get; set; }

    /// <summary>
    /// IsShowInList
    /// </summary>
    public int IsShowInList { get; set; }

    /// <summary>
    /// IsImportColumn
    /// </summary>
    public int IsImportColumn { get; set; }

    /// <summary>
    /// IsDropDownList
    /// </summary>
    public int IsDropDownList { get; set; }

    /// <summary>
    /// IsCreate
    /// </summary>
    public int IsCreate { get; set; }

    /// <summary>
    /// IsUpdate
    /// </summary>
    public int IsUpdate { get; set; }

    /// <summary>
    /// SearchConditionOrder
    /// </summary>
    public int SearchConditionOrder { get; set; }

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
    public int Order { get; set; }

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
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }


    public virtual List<TableColumnsDetail> TableColumnsDetails { get; set; }=new List<TableColumnsDetail>();

}


