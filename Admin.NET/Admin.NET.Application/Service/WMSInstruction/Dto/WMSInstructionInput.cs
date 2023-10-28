using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMSInstruction基础输入参数
    /// </summary>
    public class WMSInstructionBaseInput
    {
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
        /// TableName
        /// </summary>
        public virtual string? TableName { get; set; }
        
        /// <summary>
        /// InstructionType
        /// </summary>
        public virtual string? InstructionType { get; set; }
        
        /// <summary>
        /// BusinessType
        /// </summary>
        public virtual string? BusinessType { get; set; }
        
        /// <summary>
        /// OperationId
        /// </summary>
        public virtual long? OperationId { get; set; }
        
        /// <summary>
        /// InstructionStatus
        /// </summary>
        public virtual int? InstructionStatus { get; set; }
        
        /// <summary>
        /// InstructionTaskNo
        /// </summary>
        public virtual string? InstructionTaskNo { get; set; }
        
        /// <summary>
        /// InstructionPriority
        /// </summary>
        public virtual int? InstructionPriority { get; set; }
        
        /// <summary>
        /// Message
        /// </summary>
        public virtual string? Message { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string? Remark { get; set; }
        
        /// <summary>
        /// Creator
        /// </summary>
        public virtual string? Creator { get; set; }
        
        /// <summary>
        /// CreationTime
        /// </summary>
        public virtual DateTime? CreationTime { get; set; }
        
    }

    /// <summary>
    /// WMSInstruction分页查询输入参数
    /// </summary>
    public class WMSInstructionInput : BasePageInput
    {
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
        /// TableName
        /// </summary>
        public string? TableName { get; set; }
        
        /// <summary>
        /// InstructionType
        /// </summary>
        public string? InstructionType { get; set; }
        
        /// <summary>
        /// BusinessType
        /// </summary>
        public string? BusinessType { get; set; }
        
        /// <summary>
        /// OperationId
        /// </summary>
        public long? OperationId { get; set; }
        
        /// <summary>
        /// InstructionStatus
        /// </summary>
        public int? InstructionStatus { get; set; }
        
        /// <summary>
        /// InstructionTaskNo
        /// </summary>
        public string? InstructionTaskNo { get; set; }
        
        /// <summary>
        /// InstructionPriority
        /// </summary>
        public int? InstructionPriority { get; set; }
        
        /// <summary>
        /// Message
        /// </summary>
        public string? Message { get; set; }
        
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
         /// CreationTime范围
         /// </summary>
         public List<DateTime?> CreationTimeRange { get; set; } 
    }

    /// <summary>
    /// WMSInstruction增加输入参数
    /// </summary>
    public class AddWMSInstructionInput : WMSInstructionBaseInput
    {
    }

    /// <summary>
    /// WMSInstruction删除输入参数
    /// </summary>
    public class DeleteWMSInstructionInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMSInstruction更新输入参数
    /// </summary>
    public class UpdateWMSInstructionInput : WMSInstructionBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// WMSInstruction主键查询输入参数
    /// </summary>
    public class QueryByIdWMSInstructionInput : DeleteWMSInstructionInput
    {

    }
