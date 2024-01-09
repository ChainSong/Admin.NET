using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMSInventoryReport基础输入参数
    /// </summary>
    public class WMSInventoryReportBaseInput
    {
        /// <summary>
        /// InboundQty
        /// </summary>
        public virtual double? InboundQty { get; set; }
        
        /// <summary>
        /// OutboundQty
        /// </summary>
        public virtual double? OutboundQty { get; set; }
        
        /// <summary>
        /// AvailableInventory
        /// </summary>
        public virtual double? AvailableInventory { get; set; }
        
        /// <summary>
        /// FreezeInventory
        /// </summary>
        public virtual double? FreezeInventory { get; set; }
        
        /// <summary>
        /// OccupyInventory
        /// </summary>
        public virtual double? OccupyInventory { get; set; }
        
        /// <summary>
        /// AdjustInventory
        /// </summary>
        public virtual double? AdjustInventory { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string? SKU { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public virtual double? Qty { get; set; }
        
        /// <summary>
        /// InventoryType
        /// </summary>
        public virtual string? InventoryType { get; set; }
        
        /// <summary>
        /// InventoryStatus
        /// </summary>
        public virtual int? InventoryStatus { get; set; }
        
        /// <summary>
        /// InventoryDate
        /// </summary>
        public virtual DateTime? InventoryDate { get; set; }
        
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
        
    }

    /// <summary>
    /// WMSInventoryReport分页查询输入参数
    /// </summary>
    public class WMSInventoryReportInput : BasePageInput
    {
        /// <summary>
        /// InboundQty
        /// </summary>
        public double? InboundQty { get; set; }
        
        /// <summary>
        /// OutboundQty
        /// </summary>
        public double? OutboundQty { get; set; }
        
        /// <summary>
        /// AvailableInventory
        /// </summary>
        public double? AvailableInventory { get; set; }
        
        /// <summary>
        /// FreezeInventory
        /// </summary>
        public double? FreezeInventory { get; set; }
        
        /// <summary>
        /// OccupyInventory
        /// </summary>
        public double? OccupyInventory { get; set; }
        
        /// <summary>
        /// AdjustInventory
        /// </summary>
        public double? AdjustInventory { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public string? SKU { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public double? Qty { get; set; }
        
        /// <summary>
        /// InventoryType
        /// </summary>
        public string? InventoryType { get; set; }
        
        /// <summary>
        /// InventoryStatus
        /// </summary>
        public int? InventoryStatus { get; set; }
        
        /// <summary>
        /// InventoryDate
        /// </summary>
        public DateTime? InventoryDate { get; set; }
        
        /// <summary>
         /// InventoryDate范围
         /// </summary>
         public List<DateTime?> InventoryDateRange { get; set; } 
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
        
    }

    /// <summary>
    /// WMSInventoryReport增加输入参数
    /// </summary>
    public class AddWMSInventoryReportInput : WMSInventoryReportBaseInput
    {
    }

    /// <summary>
    /// WMSInventoryReport删除输入参数
    /// </summary>
    public class DeleteWMSInventoryReportInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMSInventoryReport更新输入参数
    /// </summary>
    public class UpdateWMSInventoryReportInput : WMSInventoryReportBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// WMSInventoryReport主键查询输入参数
    /// </summary>
    public class QueryByIdWMSInventoryReportInput : DeleteWMSInventoryReportInput
    {

    }
