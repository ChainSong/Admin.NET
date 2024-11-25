using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 入库点数基础输入参数
    /// </summary>
    public class WMSASNReceiptDetailBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }
        
        /// <summary>
        /// ASNId
        /// </summary>
        public virtual long ASNId { get; set; }
        
        /// <summary>
        /// ASNDetailId
        /// </summary>
        public virtual long ASNDetailId { get; set; }
        
        /// <summary>
        /// ASNNumber
        /// </summary>
        public virtual string ASNNumber { get; set; }
        
        /// <summary>
        /// ExternReceiptNumber
        /// </summary>
        public virtual string? ExternReceiptNumber { get; set; }
        
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
        /// LineNumber
        /// </summary>
        public virtual string LineNumber { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        
        /// <summary>
        /// UPC
        /// </summary>
        public virtual string? UPC { get; set; }
        
        /// <summary>
        /// GoodsType
        /// </summary>
        public virtual string? GoodsType { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public virtual string? GoodsName { get; set; }
        
        /// <summary>
        /// BoxCode
        /// </summary>
        public virtual string? BoxCode { get; set; }
        
        /// <summary>
        /// TrayCode
        /// </summary>
        public virtual string? TrayCode { get; set; }
        
        /// <summary>
        /// BatchCode
        /// </summary>
        public virtual string? BatchCode { get; set; }
        
        /// <summary>
        /// LotCode
        /// </summary>
        public virtual string? LotCode { get; set; }
        
        /// <summary>
        /// PoCode
        /// </summary>
        public virtual string? PoCode { get; set; }
        
        /// <summary>
        /// Weight
        /// </summary>
        public virtual double Weight { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public virtual double Volume { get; set; }
        
        /// <summary>
        /// ExpectedQty
        /// </summary>
        public virtual double ExpectedQty { get; set; }
        
        /// <summary>
        /// ReceivedQty
        /// </summary>
        public virtual double ReceivedQty { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public virtual double Qty { get; set; }
        
        /// <summary>
        /// UnitCode
        /// </summary>
        public virtual string? UnitCode { get; set; }
        
        /// <summary>
        /// Onwer
        /// </summary>
        public virtual string? Onwer { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public virtual DateTime? ProductionDate { get; set; }
        
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public virtual DateTime? ExpirationDate { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string? Remark { get; set; }
        
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
    /// 入库点数分页查询输入参数
    /// </summary>
    public class WMSASNReceiptDetailInput : BasePageInput
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
        /// ASNDetailId
        /// </summary>
        public long ASNDetailId { get; set; }
        
        /// <summary>
        /// ASNNumber
        /// </summary>
        public string ASNNumber { get; set; }
        
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
        public long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// LineNumber
        /// </summary>
        public string LineNumber { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// UPC
        /// </summary>
        public string? UPC { get; set; }
        
        /// <summary>
        /// GoodsType
        /// </summary>
        public string? GoodsType { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public string? GoodsName { get; set; }
        
        /// <summary>
        /// BoxCode
        /// </summary>
        public string? BoxCode { get; set; }
        
        /// <summary>
        /// TrayCode
        /// </summary>
        public string? TrayCode { get; set; }
        
        /// <summary>
        /// BatchCode
        /// </summary>
        public string? BatchCode { get; set; }
        
        /// <summary>
        /// LotCode
        /// </summary>
        public string? LotCode { get; set; }
        
        /// <summary>
        /// PoCode
        /// </summary>
        public string? PoCode { get; set; }
        
        /// <summary>
        /// Weight
        /// </summary>
        public double Weight { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public double Volume { get; set; }
        
        /// <summary>
        /// ExpectedQty
        /// </summary>
        public double ExpectedQty { get; set; }
        
        /// <summary>
        /// ReceivedQty
        /// </summary>
        public double ReceivedQty { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public double Qty { get; set; }
        
        /// <summary>
        /// UnitCode
        /// </summary>
        public string? UnitCode { get; set; }
        
        /// <summary>
        /// Onwer
        /// </summary>
        public string? Onwer { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public DateTime? ProductionDate { get; set; }
        
        /// <summary>
         /// ProductionDate范围
         /// </summary>
         public List<DateTime?> ProductionDateRange { get; set; } 
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public DateTime? ExpirationDate { get; set; }
        
        /// <summary>
         /// ExpirationDate范围
         /// </summary>
         public List<DateTime?> ExpirationDateRange { get; set; } 
        /// <summary>
        /// Remark
        /// </summary>
        public string? Remark { get; set; }
        
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
    /// 入库点数增加输入参数
    /// </summary>
    public class AddWMSASNReceiptDetailInput : WMSASNReceiptDetailBaseInput
    {
    }

    /// <summary>
    /// 入库点数删除输入参数
    /// </summary>
    public class DeleteWMSASNReceiptDetailInput : BaseIdInput
    {
    }

    /// <summary>
    /// 入库点数更新输入参数
    /// </summary>
    public class UpdateWMSASNReceiptDetailInput : WMSASNReceiptDetailBaseInput
    {
    }

    /// <summary>
    /// 入库点数主键查询输入参数
    /// </summary>
    public class QueryByIdWMSASNReceiptDetailInput : DeleteWMSASNReceiptDetailInput
    {

    }
