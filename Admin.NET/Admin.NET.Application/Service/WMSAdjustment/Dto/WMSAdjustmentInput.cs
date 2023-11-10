using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSAdjustment基础输入参数
/// </summary>
public class WMSAdjustmentBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    //public virtual long Id { get; set; }

    /// <summary>
    /// 外部单号
    /// </summary>
    public virtual string ExternNumber { get; set; }

    /// <summary>
    /// 调整单号
    /// </summary>
    public virtual string AdjustmentNumber { get; set; }
    
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
    /// 1新建 99 完成 -1 取消
    /// </summary>
    public virtual int AdjustmentStatus { get; set; }

    /// <summary>
    /// 调整单类型 1 移动 2 修改 3 冻结
    /// </summary>
    public virtual string AdjustmentType { get; set; }

    /// <summary>
    /// 调整原因
    /// </summary>
    public virtual string? AdjustmentReason { get; set; }

    /// <summary>
    /// AdjustmentTime
    /// </summary>
    public virtual DateTime? AdjustmentTime { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

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
/// WMSAdjustment分页查询输入参数
/// </summary>
public class WMSAdjustmentInput : BasePageInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 调整单号
    /// </summary>
    public string AdjustmentNumber { get; set; }

    /// <summary>
    /// 外部单号
    /// </summary>
    public virtual string ExternNumber { get; set; }

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
    /// 1新建 99 完成 -1 取消
    /// </summary>
    public int AdjustmentStatus { get; set; }

    /// <summary>
    /// 调整单类型 1 移动 2 修改 3 冻结
    /// </summary>
    public string AdjustmentType { get; set; }

    /// <summary>
    /// 调整原因
    /// </summary>
    public string? AdjustmentReason { get; set; }

    /// <summary>
    /// AdjustmentTime
    /// </summary>
    //public DateTime? AdjustmentTime { get; set; }

    /// <summary>
    /// AdjustmentTime范围
    /// </summary>
    public List<DateTime?> AdjustmentTime { get; set; }
    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

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
    //public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime1范围
    /// </summary>
    public List<DateTime?> DateTime1 { get; set; }
    /// <summary>
    /// DateTime2
    /// </summary>
    //public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime2范围
    /// </summary>
    public List<DateTime?> DateTime2 { get; set; }
    /// <summary>
    /// DateTime3
    /// </summary>
    //public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime3范围
    /// </summary>
    public List<DateTime?> DateTime3 { get; set; }
    /// <summary>
    /// DateTime4
    /// </summary>
    //public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime4范围
    /// </summary>
    public List<DateTime?> DateTime4 { get; set; }
    /// <summary>
    /// DateTime5
    /// </summary>
    //public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// DateTime5范围
    /// </summary>
    public List<DateTime?> DateTime5 { get; set; }
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
/// WMSAdjustment增加输入参数
/// </summary>
public class AddOrUpdateWMSAdjustmentInput : WMSAdjustmentBaseInput
{
    //AddOrUpdateWMSASNInput
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }
    public List<WMSAdjustmentDetail> Details { get; set; }
}

/// <summary>
/// WMSAdjustment删除输入参数
/// </summary>
public class DeleteWMSAdjustmentInput : BaseIdInput
{
}

/// <summary>
/// WMSAdjustment更新输入参数
/// </summary>
public class UpdateWMSAdjustmentInput : WMSAdjustmentBaseInput
{
}

/// <summary>
/// WMSAdjustment主键查询输入参数
/// </summary>
public class QueryByIdWMSAdjustmentInput : DeleteWMSAdjustmentInput
{

}
