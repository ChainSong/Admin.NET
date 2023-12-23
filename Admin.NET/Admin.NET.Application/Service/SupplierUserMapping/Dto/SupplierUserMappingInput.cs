using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// SupplierUserMapping基础输入参数
    /// </summary>
    public class SupplierUserMappingBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }
        
        /// <summary>
        /// UserId
        /// </summary>
        public virtual long UserId { get; set; }
        
        /// <summary>
        /// UserName
        /// </summary>
        public virtual string UserName { get; set; }
        
        /// <summary>
        /// SupplierId
        /// </summary>
        public virtual long SupplierId { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public virtual string SupplierName { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public virtual int Status { get; set; }
        
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
        
    }

    /// <summary>
    /// SupplierUserMapping分页查询输入参数
    /// </summary>
    public class SupplierUserMappingInput : BasePageInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// UserId
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }
        
        /// <summary>
        /// SupplierId
        /// </summary>
        public long SupplierId { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public string SupplierName { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public int Status { get; set; }
        
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
        
    }

    /// <summary>
    /// SupplierUserMapping增加输入参数
    /// </summary>
    public class AddSupplierUserMappingInput : SupplierUserMappingBaseInput
    {
    }

    /// <summary>
    /// SupplierUserMapping删除输入参数
    /// </summary>
    public class DeleteSupplierUserMappingInput : BaseIdInput
    {
    }

    /// <summary>
    /// SupplierUserMapping更新输入参数
    /// </summary>
    public class UpdateSupplierUserMappingInput : SupplierUserMappingBaseInput
    {
    }

    /// <summary>
    /// SupplierUserMapping主键查询输入参数
    /// </summary>
    public class QueryByIdSupplierUserMappingInput : DeleteSupplierUserMappingInput
    {

    }
