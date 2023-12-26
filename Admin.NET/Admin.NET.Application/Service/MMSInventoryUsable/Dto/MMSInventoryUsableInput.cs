using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// MMSInventoryUsable基础输入参数
    /// </summary>
    public class MMSInventoryUsableBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }
        
        /// <summary>
        /// ReceiptReceivingId
        /// </summary>
        public virtual long ReceiptReceivingId { get; set; }
        
        /// <summary>
        /// ReceiptReceivingDetailId
        /// </summary>
        public virtual long ReceiptReceivingDetailId { get; set; }
        
        /// <summary>
        /// SupplierId
        /// </summary>
        public virtual long SupplierId { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public virtual string SupplierName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public virtual long WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public virtual string WarehouseName { get; set; }
        
        /// <summary>
        /// Area
        /// </summary>
        public virtual string Area { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        public virtual string Location { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        
        /// <summary>
        /// UPC
        /// </summary>
        public virtual string UPC { get; set; }
        
        /// <summary>
        /// GoodsType
        /// </summary>
        public virtual string GoodsType { get; set; }
        
        /// <summary>
        /// InventoryStatus
        /// </summary>
        public virtual int InventoryStatus { get; set; }
        
        /// <summary>
        /// SuperId
        /// </summary>
        public virtual long SuperId { get; set; }
        
        /// <summary>
        /// RelatedId
        /// </summary>
        public virtual long RelatedId { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public virtual string GoodsName { get; set; }
        
        /// <summary>
        /// UnitCode
        /// </summary>
        public virtual string UnitCode { get; set; }
        
        /// <summary>
        /// Onwer
        /// </summary>
        public virtual string Onwer { get; set; }
        
        /// <summary>
        /// BoxCode
        /// </summary>
        public virtual string BoxCode { get; set; }
        
        /// <summary>
        /// TrayCode
        /// </summary>
        public virtual string TrayCode { get; set; }
        
        /// <summary>
        /// BatchCode
        /// </summary>
        public virtual string BatchCode { get; set; }
        
        /// <summary>
        /// LotCode
        /// </summary>
        public virtual string LotCode { get; set; }
        
        /// <summary>
        /// PoCode
        /// </summary>
        public virtual string PoCode { get; set; }
        
        /// <summary>
        /// Weight
        /// </summary>
        public virtual double Weight { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public virtual double Volume { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public virtual double Qty { get; set; }
        
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
        /// InventoryTime
        /// </summary>
        public virtual DateTime? InventoryTime { get; set; }
        
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
        public virtual string Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public virtual string Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public virtual string Str3 { get; set; }
        
        /// <summary>
        /// Str4
        /// </summary>
        public virtual string Str4 { get; set; }
        
        /// <summary>
        /// Str5
        /// </summary>
        public virtual string Str5 { get; set; }
        
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
        public virtual int Int1 { get; set; }
        
        /// <summary>
        /// Int2
        /// </summary>
        public virtual int Int2 { get; set; }
        
    }

    /// <summary>
    /// MMSInventoryUsable分页查询输入参数
    /// </summary>
    public class MMSInventoryUsableInput : BasePageInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// ReceiptReceivingId
        /// </summary>
        public long ReceiptReceivingId { get; set; }
        
        /// <summary>
        /// ReceiptReceivingDetailId
        /// </summary>
        public long ReceiptReceivingDetailId { get; set; }
        
        /// <summary>
        /// SupplierId
        /// </summary>
        public long SupplierId { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public string SupplierName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string WarehouseName { get; set; }
        
        /// <summary>
        /// Area
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// UPC
        /// </summary>
        public string UPC { get; set; }
        
        /// <summary>
        /// GoodsType
        /// </summary>
        public string GoodsType { get; set; }
        
        /// <summary>
        /// InventoryStatus
        /// </summary>
        public int InventoryStatus { get; set; }
        
        /// <summary>
        /// SuperId
        /// </summary>
        public long SuperId { get; set; }
        
        /// <summary>
        /// RelatedId
        /// </summary>
        public long RelatedId { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public string GoodsName { get; set; }
        
        /// <summary>
        /// UnitCode
        /// </summary>
        public string UnitCode { get; set; }
        
        /// <summary>
        /// Onwer
        /// </summary>
        public string Onwer { get; set; }
        
        /// <summary>
        /// BoxCode
        /// </summary>
        public string BoxCode { get; set; }
        
        /// <summary>
        /// TrayCode
        /// </summary>
        public string TrayCode { get; set; }
        
        /// <summary>
        /// BatchCode
        /// </summary>
        public string BatchCode { get; set; }
        
        /// <summary>
        /// LotCode
        /// </summary>
        public string LotCode { get; set; }
        
        /// <summary>
        /// PoCode
        /// </summary>
        public string PoCode { get; set; }
        
        /// <summary>
        /// Weight
        /// </summary>
        public double Weight { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public double Volume { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public double Qty { get; set; }
        
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
        /// InventoryTime
        /// </summary>
        public DateTime? InventoryTime { get; set; }
        
        /// <summary>
         /// InventoryTime范围
         /// </summary>
         public List<DateTime?> InventoryTimeRange { get; set; } 
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
        public string Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public string Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public string Str3 { get; set; }
        
        /// <summary>
        /// Str4
        /// </summary>
        public string Str4 { get; set; }
        
        /// <summary>
        /// Str5
        /// </summary>
        public string Str5 { get; set; }
        
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
        public int Int1 { get; set; }
        
        /// <summary>
        /// Int2
        /// </summary>
        public int Int2 { get; set; }
        
    }

    /// <summary>
    /// MMSInventoryUsable增加输入参数
    /// </summary>
    public class AddMMSInventoryUsableInput : MMSInventoryUsableBaseInput
    {
    }

    /// <summary>
    /// MMSInventoryUsable删除输入参数
    /// </summary>
    public class DeleteMMSInventoryUsableInput : BaseIdInput
    {
    }

    /// <summary>
    /// MMSInventoryUsable更新输入参数
    /// </summary>
    public class UpdateMMSInventoryUsableInput : MMSInventoryUsableBaseInput
    {
    }

    /// <summary>
    /// MMSInventoryUsable主键查询输入参数
    /// </summary>
    public class QueryByIdMMSInventoryUsableInput : DeleteMMSInventoryUsableInput
    {

    }
