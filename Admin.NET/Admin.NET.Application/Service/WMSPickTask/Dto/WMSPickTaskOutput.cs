using Admin.NET.Core.Entity;
using System.Collections.Generic;

namespace Admin.NET.Application;

/// <summary>
/// WMSPickTask输出参数
/// </summary>
public class WMSPickTaskOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public string CustomerName { get; set; }

    public string OrderNumber { get; set; }
    public string ExternOrderNumber { get; set; }

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
    public int PickStatus { get; set; }

    /// <summary>
    /// PickType
    /// </summary>
    public string PickType { get; set; }
    public string Remark { get; set; }

    /// <summary>
    /// StartTime
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// EndTime
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public int PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public DateTime? PrintTime { get; set; }

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
    /// DateTime2
    /// </summary>
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime4
    /// </summary>
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime5
    /// </summary>
    public DateTime? DateTime5 { get; set; }

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


    //[Navigate(NavigateType.OneToMany, nameof(WMSPickTaskDetail.PickTaskId))]
    public List<WMSPickTaskDetail> Details { get; set; }

    //[Navigate(NavigateType.OneToOne, nameof(WMSOrderAddress.PreOrderId), nameof(PreOrderId))]
    //[Navigate()]
    public WMSOrderAddress OrderAddress { get; set; }

}


