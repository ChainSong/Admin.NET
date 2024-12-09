using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSPickTask基础输入参数
/// </summary>
public class WMSPickTaskRFIDInput
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

    public List<string> RFIDs { get; set; }

}

 