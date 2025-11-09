using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSHandover基础输入参数
/// </summary>
public class WMSHandoverBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// OrderId
    /// </summary>
    public virtual long? OrderId { get; set; }

    /// <summary>
    /// PackageId
    /// </summary>
    public virtual long PackageId { get; set; }

    /// <summary>
    /// PickTaskId
    /// </summary>
    public virtual long PickTaskId { get; set; }

    /// <summary>
    /// PalletNumber
    /// </summary>
    public virtual string? PalletNumber { get; set; }
    /// <summary>
    /// HandoverNumber
    /// </summary>
    public string HandoverNumber { get; set; }
    /// <summary>
    /// PickTaskNumber
    /// </summary>
    public virtual string PickTaskNumber { get; set; }

    /// <summary>
    /// PreOrderNumber
    /// </summary>
    public virtual string? PreOrderNumber { get; set; }

    /// <summary>
    /// PackageNumber
    /// </summary>
    public virtual string PackageNumber { get; set; }

    /// <summary>
    /// OrderNumber
    /// </summary>
    public virtual string OrderNumber { get; set; }

    /// <summary>
    /// ExternOrderNumber
    /// </summary>
    public virtual string ExternOrderNumber { get; set; }

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
    public virtual long? WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string? WarehouseName { get; set; }

    /// <summary>
    /// HandoverType
    /// </summary>
    public virtual string? HandoverType { get; set; }

    /// <summary>
    /// Length
    /// </summary>
    public virtual double? Length { get; set; }

    /// <summary>
    /// Width
    /// </summary>
    public virtual double? Width { get; set; }

    /// <summary>
    /// Height
    /// </summary>
    public virtual double? Height { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    public virtual double? Volume { get; set; }

    /// <summary>
    /// NetWeight
    /// </summary>
    public virtual double? NetWeight { get; set; }

    /// <summary>
    /// GrossWeight
    /// </summary>
    public virtual double? GrossWeight { get; set; }

    /// <summary>
    /// ExpressCompany
    /// </summary>
    public virtual string? ExpressCompany { get; set; }

    /// <summary>
    /// ExpressNumber
    /// </summary>
    public virtual string? ExpressNumber { get; set; }

    /// <summary>
    /// SerialNumber
    /// </summary>
    public virtual string? SerialNumber { get; set; }

    /// <summary>
    /// IsComposited
    /// </summary>
    public virtual int? IsComposited { get; set; }

    /// <summary>
    /// IsHandovered
    /// </summary>
    public virtual int? IsHandovered { get; set; }

    /// <summary>
    /// Handoveror
    /// </summary>
    public virtual string? Handoveror { get; set; }

    /// <summary>
    /// HandoverTime
    /// </summary>
    public virtual DateTime? HandoverTime { get; set; }

    /// <summary>
    /// HandoverStatus
    /// </summary>
    public virtual int HandoverStatus { get; set; }

    /// <summary>
    /// DetailCount
    /// </summary>
    public virtual double? DetailCount { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public virtual int? PrintNum { get; set; }

    /// <summary>
    /// PrintPersonnel
    /// </summary>
    public virtual string? PrintPersonnel { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public virtual DateTime? PrintTime { get; set; }

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
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

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
    /// Str11
    /// </summary>
    public virtual string? Str11 { get; set; }

    /// <summary>
    /// Str12
    /// </summary>
    public virtual string? Str12 { get; set; }

    /// <summary>
    /// Str13
    /// </summary>
    public virtual string? Str13 { get; set; }

    /// <summary>
    /// Str14
    /// </summary>
    public virtual string? Str14 { get; set; }

    /// <summary>
    /// Str15
    /// </summary>
    public virtual string? Str15 { get; set; }

    /// <summary>
    /// Str16
    /// </summary>
    public virtual string? Str16 { get; set; }

    /// <summary>
    /// Str17
    /// </summary>
    public virtual string? Str17 { get; set; }

    /// <summary>
    /// Str18
    /// </summary>
    public virtual string? Str18 { get; set; }

    /// <summary>
    /// Str19
    /// </summary>
    public virtual string? Str19 { get; set; }

    /// <summary>
    /// Str20
    /// </summary>
    public virtual string? Str20 { get; set; }

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
/// WMSHandover分页查询输入参数
/// </summary>
public class WMSHandoverInput : BasePageInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// OrderId
    /// </summary>
    public long? OrderId { get; set; }

    /// <summary>
    /// PackageId
    /// </summary>
    public long PackageId { get; set; }

    /// <summary>
    /// PickTaskId
    /// </summary>
    public long PickTaskId { get; set; }

    /// <summary>
    /// PalletNumber
    /// </summary>
    public string? PalletNumber { get; set; }

    /// <summary>
    /// PickTaskNumber
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// HandoverNumber
    /// </summary>
    public string HandoverNumber { get; set; }

    /// <summary>
    /// PreOrderNumber
    /// </summary>
    public string? PreOrderNumber { get; set; }

    /// <summary>
    /// PackageNumber
    /// </summary>
    public string PackageNumber { get; set; }

    /// <summary>
    /// OrderNumber
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// ExternOrderNumber
    /// </summary>
    public string ExternOrderNumber { get; set; }

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
    public long? WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// HandoverType
    /// </summary>
    public string? HandoverType { get; set; }

    /// <summary>
    /// Length
    /// </summary>
    public double? Length { get; set; }

    /// <summary>
    /// Width
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    /// Height
    /// </summary>
    public double? Height { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    public double? Volume { get; set; }

    /// <summary>
    /// NetWeight
    /// </summary>
    public double? NetWeight { get; set; }

    /// <summary>
    /// GrossWeight
    /// </summary>
    public double? GrossWeight { get; set; }

    /// <summary>
    /// ExpressCompany
    /// </summary>
    public string? ExpressCompany { get; set; }

    /// <summary>
    /// ExpressNumber
    /// </summary>
    public string? ExpressNumber { get; set; }

    /// <summary>
    /// SerialNumber
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// IsComposited
    /// </summary>
    public int? IsComposited { get; set; }

    /// <summary>
    /// IsHandovered
    /// </summary>
    public int? IsHandovered { get; set; }

    /// <summary>
    /// Handoveror
    /// </summary>
    public string? Handoveror { get; set; }

    /// <summary>
    /// HandoverTime
    /// </summary>
    public DateTime? HandoverTime { get; set; }

    /// <summary>
    /// HandoverTime范围
    /// </summary>
    public List<DateTime?> HandoverTimeRange { get; set; }
    /// <summary>
    /// HandoverStatus
    /// </summary>
    public int HandoverStatus { get; set; }

    /// <summary>
    /// DetailCount
    /// </summary>
    public double? DetailCount { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public int? PrintNum { get; set; }

    /// <summary>
    /// PrintPersonnel
    /// </summary>
    public string? PrintPersonnel { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintTime范围
    /// </summary>
    public List<DateTime?> PrintTimeRange { get; set; }
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
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

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
    /// Str11
    /// </summary>
    public string? Str11 { get; set; }

    /// <summary>
    /// Str12
    /// </summary>
    public string? Str12 { get; set; }

    /// <summary>
    /// Str13
    /// </summary>
    public string? Str13 { get; set; }

    /// <summary>
    /// Str14
    /// </summary>
    public string? Str14 { get; set; }

    /// <summary>
    /// Str15
    /// </summary>
    public string? Str15 { get; set; }

    /// <summary>
    /// Str16
    /// </summary>
    public string? Str16 { get; set; }

    /// <summary>
    /// Str17
    /// </summary>
    public string? Str17 { get; set; }

    /// <summary>
    /// Str18
    /// </summary>
    public string? Str18 { get; set; }

    /// <summary>
    /// Str19
    /// </summary>
    public string? Str19 { get; set; }

    /// <summary>
    /// Str20
    /// </summary>
    public string? Str20 { get; set; }

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
/// WMSHandover增加输入参数
/// </summary>
public class AddWMSHandoverInput : WMSHandoverBaseInput
{
}

/// <summary>
/// WMSHandover删除输入参数
/// </summary>
public class DeleteWMSHandoverInput : BaseIdInput
{
}

/// <summary>
/// WMSHandover更新输入参数
/// </summary>
public class UpdateWMSHandoverInput : WMSHandoverBaseInput
{
}

/// <summary>
/// WMSHandover主键查询输入参数
/// </summary>
public class QueryByIdWMSHandoverInput : DeleteWMSHandoverInput
{

}
