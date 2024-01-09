using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSPickTask基础输入参数
/// </summary>
public class WMSPickTaskBaseInput
{
    /// <summary>
    /// CustomerId
    /// </summary>
    public virtual long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public virtual string CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public virtual long WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string WarehouseName { get; set; }

    /// <summary>
    /// PickTaskNumber
    /// </summary>
    public virtual string PickTaskNumber { get; set; }

    /// <summary>
    /// PickStatus
    /// </summary>
    public virtual int PickStatus { get; set; }
    public string OrderNumber { get; set; }
    public string ExternOrderNumber { get; set; }

    /// <summary>
    /// PickType
    /// </summary>
    public virtual string PickType { get; set; }

    /// <summary>
    /// StartTime
    /// </summary>
    public virtual DateTime? StartTime { get; set; }

    /// <summary>
    /// EndTime
    /// </summary>
    public virtual DateTime? EndTime { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public virtual int PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public virtual DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintPersonnel
    /// </summary>
    public virtual string? PrintPersonnel { get; set; }

    /// <summary>
    /// PickPlanPersonnel
    /// </summary>
    public virtual string? PickPlanPersonnel { get; set; }

    /// <summary>
    /// DetailQty
    /// </summary>
    public virtual double DetailQty { get; set; }

    /// <summary>
    /// DetailKindsQty
    /// </summary>
    public virtual int DetailKindsQty { get; set; }

    public virtual string PickContainer { get; set; }


    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime? CreationTime { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public virtual string? Updator { get; set; }

    /// <summary>
    /// Str1
    /// </summary>
    public virtual string? Str1 { get; set; }

    /// <summary>
    /// Str2
    /// </summary>
    public virtual string? Str2 { get; set; }

    /// <summary>
    /// Str3
    /// </summary>
    public virtual string? Str3 { get; set; }

    /// <summary>
    /// Str4
    /// </summary>
    public virtual string? Str4 { get; set; }

    /// <summary>
    /// Str5
    /// </summary>
    public virtual string? Str5 { get; set; }

    /// <summary>
    /// Str6
    /// </summary>
    public virtual string? Str6 { get; set; }

    /// <summary>
    /// Str7
    /// </summary>
    public virtual string? Str7 { get; set; }

    /// <summary>
    /// Str8
    /// </summary>
    public virtual string? Str8 { get; set; }

    /// <summary>
    /// Str9
    /// </summary>
    public virtual string? Str9 { get; set; }

    /// <summary>
    /// Str10
    /// </summary>
    public virtual string? Str10 { get; set; }

    /// <summary>
    /// DateTime1
    /// </summary>
    public virtual DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public virtual DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public virtual DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime4
    /// </summary>
    public virtual DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime5
    /// </summary>
    public virtual DateTime? DateTime5 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public virtual int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public virtual int? Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public virtual int? Int3 { get; set; }

    /// <summary>
    /// Int4
    /// </summary>
    public virtual int? Int4 { get; set; }

    /// <summary>
    /// Int5
    /// </summary>
    public virtual int? Int5 { get; set; }

}

/// <summary>
/// WMSPickTask分页查询输入参数
/// </summary>
public class WMSPickTaskInput : BasePageInput
{
    /// <summary>
    /// CustomerId
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string WarehouseName { get; set; }

    /// <summary>
    /// PickTaskNumber
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// PickStatus
    /// </summary>
    public int? PickStatus { get; set; }

    /// <summary>
    /// PickType
    /// </summary>
    public string PickType { get; set; }

    /// <summary>
    /// StartTime
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// StartTime范围
    /// </summary>
    public List<DateTime?> StartTimeRange { get; set; }
    /// <summary>
    /// EndTime
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// EndTime范围
    /// </summary>
    public List<DateTime?> EndTimeRange { get; set; }
    /// <summary>
    /// PrintNum
    /// </summary>
    public int PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintTime范围
    /// </summary>
    public List<DateTime?> PrintTimeRange { get; set; }
    /// <summary>
    /// PrintPersonnel
    /// </summary>
    public string? PrintPersonnel { get; set; }

    /// <summary>
    /// PickPlanPersonnel
    /// </summary>
    public string? PickPlanPersonnel { get; set; }

    /// <summary>
    /// DetailQty
    /// </summary>
    public int DetailQty { get; set; }

    /// <summary>
    /// DetailKindsQty
    /// </summary>
    public int DetailKindsQty { get; set; }

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
    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

    /// <summary>
    /// Str1
    /// </summary>
    public string? Str1 { get; set; }

    /// <summary>
    /// Str2
    /// </summary>
    public string? Str2 { get; set; }

    /// <summary>
    /// Str3
    /// </summary>
    public string? Str3 { get; set; }

    /// <summary>
    /// Str4
    /// </summary>
    public string? Str4 { get; set; }

    /// <summary>
    /// Str5
    /// </summary>
    public string? Str5 { get; set; }

    /// <summary>
    /// Str6
    /// </summary>
    public string? Str6 { get; set; }

    /// <summary>
    /// Str7
    /// </summary>
    public string? Str7 { get; set; }

    /// <summary>
    /// Str8
    /// </summary>
    public string? Str8 { get; set; }

    /// <summary>
    /// Str9
    /// </summary>
    public string? Str9 { get; set; }

    /// <summary>
    /// Str10
    /// </summary>
    public string? Str10 { get; set; }

    /// <summary>
    /// DateTime1
    /// </summary>
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime1范围
    /// </summary>
    public List<DateTime?> DateTime1Range { get; set; }
    /// <summary>
    /// DateTime2
    /// </summary>
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime2范围
    /// </summary>
    public List<DateTime?> DateTime2Range { get; set; }
    /// <summary>
    /// DateTime3
    /// </summary>
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime3范围
    /// </summary>
    public List<DateTime?> DateTime3Range { get; set; }
    /// <summary>
    /// DateTime4
    /// </summary>
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime4范围
    /// </summary>
    public List<DateTime?> DateTime4Range { get; set; }
    /// <summary>
    /// DateTime5
    /// </summary>
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// DateTime5范围
    /// </summary>
    public List<DateTime?> DateTime5Range { get; set; }
    /// <summary>
    /// Int1
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public int? Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public int? Int3 { get; set; }

    /// <summary>
    /// Int4
    /// </summary>
    public int? Int4 { get; set; }

    /// <summary>
    /// Int5
    /// </summary>
    public int? Int5 { get; set; }

}

/// <summary>
/// WMSPickTask增加输入参数
/// </summary>
public class AddWMSPickTaskInput : WMSPickTaskBaseInput
{
}

/// <summary>
/// WMSPickTask删除输入参数
/// </summary>
public class DeleteWMSPickTaskInput : BaseIdInput
{
}

/// <summary>
/// WMSPickTask更新输入参数
/// </summary>
public class UpdateWMSPickTaskInput : WMSPickTaskBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSPickTask主键查询输入参数
/// </summary>
public class QueryByIdWMSPickTaskInput : DeleteWMSPickTaskInput
{

}
