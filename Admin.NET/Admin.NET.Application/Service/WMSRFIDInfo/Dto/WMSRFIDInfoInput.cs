using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSRFIDInfo基础输入参数
/// </summary>
public class WMSRFIDInfoBaseInput
{
    /// <summary>
    /// ReceiptNumber
    /// </summary>
    public virtual string? ReceiptNumber { get; set; }

    /// <summary>
    /// ExternReceiptNumber
    /// </summary>
    public virtual string? ExternReceiptNumber { get; set; }

    /// <summary>
    /// ASNNumber
    /// </summary>
    public virtual string? ASNNumber { get; set; }

    /// <summary>
    /// ASNDetailId
    /// </summary>
    public virtual long? ASNDetailId { get; set; }

    /// <summary>
    /// ReceiptDetailId
    /// </summary>
    public virtual long? ReceiptDetailId { get; set; }

    /// <summary>
    /// ReceiptId
    /// </summary>
    public virtual long? ReceiptId { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public virtual long? CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public virtual string? CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public virtual long? WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string? WarehouseName { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public virtual string? SKU { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public virtual string? GoodsType { get; set; }

    /// <summary>
    /// ReceiptPerson
    /// </summary>
    public virtual string? ReceiptPerson { get; set; }

    /// <summary>
    /// ReceiptTime
    /// </summary>
    public virtual DateTime? ReceiptTime { get; set; }

    /// <summary>
    /// OrderNumber
    /// </summary>
    public virtual string? OrderNumber { get; set; }

    /// <summary>
    /// ExternOrderNumber
    /// </summary>
    public virtual string? ExternOrderNumber { get; set; }

    /// <summary>
    /// PreOrderId
    /// </summary>
    public virtual long? PreOrderId { get; set; }

    /// <summary>
    /// PreOrderDetailId
    /// </summary>
    public virtual long? PreOrderDetailId { get; set; }

    /// <summary>
    /// OrderDetailId
    /// </summary>
    public virtual long? OrderDetailId { get; set; }

    /// <summary>
    /// OrderPerson
    /// </summary>
    public virtual string? OrderPerson { get; set; }

    /// <summary>
    /// OrderTime
    /// </summary>
    public virtual DateTime? OrderTime { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public virtual int? Status { get; set; }

    /// <summary>
    /// Qty
    /// </summary>
    public virtual double? Qty { get; set; }

    /// <summary>
    /// Sequence
    /// </summary>
    public virtual string? Sequence { get; set; }

    /// <summary>
    /// RFID
    /// </summary>
    public virtual string? RFID { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    public virtual string? Link { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public virtual int? PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public virtual DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintPerson
    /// </summary>
    public virtual string? PrintPerson { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime CreationTime { get; set; }

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
    /// DateTime1
    /// </summary>
    public virtual DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public virtual DateTime? DateTime2 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public virtual int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public virtual int? Int2 { get; set; }

}

/// <summary>
/// WMSRFIDInfo分页查询输入参数
/// </summary>
public class WMSRFIDInfoInput : BasePageInput
{
    /// <summary>
    /// ReceiptNumber
    /// </summary>
    public string? ReceiptNumber { get; set; }

    /// <summary>
    /// ExternReceiptNumber
    /// </summary>
    public string? ExternReceiptNumber { get; set; }

    /// <summary>
    /// ASNNumber
    /// </summary>
    public string? ASNNumber { get; set; }

    /// <summary>
    /// ASNDetailId
    /// </summary>
    public long? ASNDetailId { get; set; }

    /// <summary>
    /// ReceiptDetailId
    /// </summary>
    public long? ReceiptDetailId { get; set; }

    /// <summary>
    /// ReceiptId
    /// </summary>
    public long? ReceiptId { get; set; }

    /// <summary>
    /// CustomerId
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public long? WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public string? GoodsType { get; set; }

    /// <summary>
    /// ReceiptPerson
    /// </summary>
    public string? ReceiptPerson { get; set; }

    /// <summary>
    /// ReceiptTime
    /// </summary>
    public DateTime? ReceiptTime { get; set; }

    /// <summary>
    /// ReceiptTime范围
    /// </summary>
    public List<DateTime?> ReceiptTimeRange { get; set; }
    /// <summary>
    /// OrderNumber
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// ExternOrderNumber
    /// </summary>
    public string? ExternOrderNumber { get; set; }

    /// <summary>
    /// PreOrderId
    /// </summary>
    public long? PreOrderId { get; set; }

    /// <summary>
    /// PreOrderDetailId
    /// </summary>
    public long? PreOrderDetailId { get; set; }

    /// <summary>
    /// OrderDetailId
    /// </summary>
    public long? OrderDetailId { get; set; }

    /// <summary>
    /// OrderPerson
    /// </summary>
    public string? OrderPerson { get; set; }

    /// <summary>
    /// OrderTime
    /// </summary>
    public DateTime? OrderTime { get; set; }

    /// <summary>
    /// OrderTime范围
    /// </summary>
    public List<DateTime?> OrderTimeRange { get; set; }
    /// <summary>
    /// Status
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Qty
    /// </summary>
    public double? Qty { get; set; }

    /// <summary>
    /// Sequence
    /// </summary>
    public string? Sequence { get; set; }

    /// <summary>
    /// RFID
    /// </summary>
    public string? RFID { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    public string? Link { get; set; }

    public string BatchCode { get; set; }

    public string PoCode { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// PrintNum
    /// </summary>
    public int? PrintNum { get; set; }

    /// <summary>
    /// PrintTime
    /// </summary>
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// PrintTime范围
    /// </summary>
    public List<DateTime?> PrintTimeRange { get; set; }
    /// <summary>
    /// PrintPerson
    /// </summary>
    public string? PrintPerson { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime CreationTime { get; set; }

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
    /// Int1
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public int? Int2 { get; set; }

}

/// <summary>
/// WMSRFIDInfo增加输入参数
/// </summary>
public class AddWMSRFIDInfoInput : WMSRFIDInfoBaseInput
{
}

/// <summary>
/// WMSRFIDInfo删除输入参数
/// </summary>
public class DeleteWMSRFIDInfoInput : BaseIdInput
{
}

/// <summary>
/// WMSRFIDInfo更新输入参数
/// </summary>
public class UpdateWMSRFIDInfoInput : WMSRFIDInfoBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSRFIDInfo主键查询输入参数
/// </summary>
public class QueryByIdWMSRFIDInfoInput : DeleteWMSRFIDInfoInput
{

}
