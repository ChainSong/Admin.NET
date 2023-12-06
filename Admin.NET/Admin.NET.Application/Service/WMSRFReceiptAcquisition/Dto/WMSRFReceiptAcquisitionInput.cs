using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMSRFReceiptAcquisition基础输入参数
    /// </summary>
    public class WMSRFReceiptAcquisitionBaseInput
    {
        /// <summary>
        /// ASNId
        /// </summary>
        public virtual long? ASNId { get; set; }
        
        /// <summary>
        /// ASNNumber
        /// </summary>
        public virtual string ASNNumber { get; set; }
        
        /// <summary>
        /// 收货单号，系统自动生成
        /// </summary>
        public virtual string ReceiptNumber { get; set; }
        
        /// <summary>
        /// ExternReceiptNumber
        /// </summary>
        public virtual string? ExternReceiptNumber { get; set; }

    /// <summary>
    /// 
    /// </summary> 
    public long ReceiptDetailId { get; set; }

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
        /// SKU
        /// </summary>
        public virtual string SKU { get; set; }
        
        /// <summary>
        /// Lot
        /// </summary>
        public virtual string Lot { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public virtual string? ProductionDate { get; set; }
        
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public virtual string? ExpirationDate { get; set; }
        
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
        
    }

    /// <summary>
    /// WMSRFReceiptAcquisition分页查询输入参数
    /// </summary>
    public class WMSRFReceiptAcquisitionInput : BasePageInput
    {
        /// <summary>
        /// ASNId
        /// </summary>
        public long ASNId { get; set; }
        
        /// <summary>
        /// ASNNumber
        /// </summary>
        public string ASNNumber { get; set; }
        
        /// <summary>
        /// 收货单号，系统自动生成
        /// </summary>
        public string ReceiptNumber { get; set; }
        
        /// <summary>
        /// ExternReceiptNumber
        /// </summary>
        public string? ExternReceiptNumber { get; set; }

    /// <summary>
    /// 
    /// </summary> 
    public long ReceiptDetailId { get; set; }

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
        /// SKU
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// Lot
        /// </summary>
        public string Lot { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public object? ProductionDate { get; set; }
        
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public object? ExpirationDate { get; set; }
        
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
        
    }

    /// <summary>
    /// WMSRFReceiptAcquisition增加输入参数
    /// </summary>
    public class AddWMSRFReceiptAcquisitionInput : WMSRFReceiptAcquisitionBaseInput
    {
    }

    /// <summary>
    /// WMSRFReceiptAcquisition删除输入参数
    /// </summary>
    public class DeleteWMSRFReceiptAcquisitionInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMSRFReceiptAcquisition更新输入参数
    /// </summary>
    public class UpdateWMSRFReceiptAcquisitionInput : WMSRFReceiptAcquisitionBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// WMSRFReceiptAcquisition主键查询输入参数
    /// </summary>
    public class QueryByIdWMSRFReceiptAcquisitionInput : DeleteWMSRFReceiptAcquisitionInput
    {

    }
