﻿using Admin.NET.Core.Entity;
using System.Collections.Generic;

namespace Admin.NET.Application;

/// <summary>
/// 入库点数输出参数
/// </summary>
public class WMSASNCountQuantityOutput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// ASNId
    /// </summary>
    public long ASNId { get; set; }

    /// <summary>
    /// ASNNumber
    /// </summary>
    public string ASNNumber { get; set; }

    /// <summary>
    /// ASNCountQuantityNumber
    /// </summary>
    public string ASNCountQuantityNumber { get; set; }

    /// <summary>
    /// ExternReceiptNumber
    /// </summary>
    public string? ExternReceiptNumber { get; set; }

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
    /// ExpectDate
    /// </summary>
    public DateTime? ExpectDate { get; set; }

    /// <summary>
    /// ASNCountQuantityStatus
    /// </summary>
    public int ASNCountQuantityStatus { get; set; }

    /// <summary>
    /// ReceiptType
    /// </summary>
    public string ReceiptType { get; set; }

    /// <summary>
    /// Contact
    /// </summary>
    public string? Contact { get; set; }

    /// <summary>
    /// ContactInfo
    /// </summary>
    public string? ContactInfo { get; set; }

    /// <summary>
    /// CompleteTime
    /// </summary>
    public DateTime? CompleteTime { get; set; }

    /// <summary>
    /// Po
    /// </summary>
    public string? Po { get; set; }

    /// <summary>
    /// So
    /// </summary>
    public string? So { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

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

    public virtual List<WMSASNCountQuantityDetail> Details { get; set; }

}


