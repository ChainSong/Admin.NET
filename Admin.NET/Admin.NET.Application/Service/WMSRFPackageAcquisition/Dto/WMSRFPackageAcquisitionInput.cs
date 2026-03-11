using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMS_RFPackageAcquisition基础输入参数
    /// </summary>
    public class WMSRFPackageAcquisitionBaseInput
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
        /// PickTaskId
        /// </summary>
        public virtual long? PickTaskId { get; set; }
        
        /// <summary>
        /// PickTaskNumber
        /// </summary>
        public virtual string? PickTaskNumber { get; set; }
        
        /// <summary>
        /// PreOrderNumber
        /// </summary>
        public virtual string? PreOrderNumber { get; set; }
        
        /// <summary>
        /// OrderNumber
        /// </summary>
        public virtual string? OrderNumber { get; set; }
        
        /// <summary>
        /// ExternOrderNumber
        /// </summary>
        public virtual string? ExternOrderNumber { get; set; }
        
        /// <summary>
        /// PackageNumber
        /// </summary>
        public virtual string? PackageNumber { get; set; }
        
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
        /// Lot
        /// </summary>
        public virtual string? Lot { get; set; }
        
        /// <summary>
        /// SN
        /// </summary>
        public virtual string? SN { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public virtual double? Qty { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public virtual DateTime? ProductionDate { get; set; }
        
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public virtual DateTime? ExpirationDate { get; set; }
        
        /// <summary>
        /// ReceiptAcquisitionStatus
        /// </summary>
        public virtual int? ReceiptAcquisitionStatus { get; set; }
        
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
        /// Type
        /// </summary>
        public virtual string? Type { get; set; }
        
    }

    /// <summary>
    /// WMS_RFPackageAcquisition分页查询输入参数
    /// </summary>
    public class WMSRFPackageAcquisitionInput : BasePageInput
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
        /// PickTaskId
        /// </summary>
        public long? PickTaskId { get; set; }
        
        /// <summary>
        /// PickTaskNumber
        /// </summary>
        public string? PickTaskNumber { get; set; }
        
        /// <summary>
        /// PreOrderNumber
        /// </summary>
        public string? PreOrderNumber { get; set; }
        
        /// <summary>
        /// OrderNumber
        /// </summary>
        public string? OrderNumber { get; set; }
        
        /// <summary>
        /// ExternOrderNumber
        /// </summary>
        public string? ExternOrderNumber { get; set; }
        
        /// <summary>
        /// PackageNumber
        /// </summary>
        public string? PackageNumber { get; set; }
        
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
        /// Lot
        /// </summary>
        public string? Lot { get; set; }
        
        /// <summary>
        /// SN
        /// </summary>
        public string? SN { get; set; }
        
        /// <summary>
        /// Qty
        /// </summary>
        public double? Qty { get; set; }
        
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
        /// ReceiptAcquisitionStatus
        /// </summary>
        public int? ReceiptAcquisitionStatus { get; set; }
        
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
        /// Type
        /// </summary>
        public string? Type { get; set; }
        
    }

    /// <summary>
    /// WMS_RFPackageAcquisition增加输入参数
    /// </summary>
    public class AddWMSRFPackageAcquisitionInput : WMSRFPackageAcquisitionBaseInput
    {
    }

    /// <summary>
    /// WMS_RFPackageAcquisition删除输入参数
    /// </summary>
    public class DeleteWMSRFPackageAcquisitionInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMS_RFPackageAcquisition更新输入参数
    /// </summary>
    public class UpdateWMSRFPackageAcquisitionInput : WMSRFPackageAcquisitionBaseInput
    {
    }

    /// <summary>
    /// WMS_RFPackageAcquisition主键查询输入参数
    /// </summary>
    public class QueryByIdWMSRFPackageAcquisitionInput : DeleteWMSRFPackageAcquisitionInput
    {

    }
