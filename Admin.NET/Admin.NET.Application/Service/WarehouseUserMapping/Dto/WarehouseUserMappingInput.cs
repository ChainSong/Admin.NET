using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 仓库用户关系基础输入参数
    /// </summary>
    public class WarehouseUserMappingBaseInput
    {
        /// <summary>
        /// UserId
        /// </summary>
        public virtual long? UserId { get; set; }
        
        /// <summary>
        /// UserName
        /// </summary>
        public virtual string? UserName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public virtual long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public virtual string? WarehouseName { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public virtual int? Status { get; set; }
        
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
    /// 仓库用户关系分页查询输入参数
    /// </summary>
    public class WarehouseUserMappingInput : BasePageInput
    {
        /// <summary>
        /// UserId
        /// </summary>
        public long? UserId { get; set; }
        
        /// <summary>
        /// UserName
        /// </summary>
        public string? UserName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// Status
        /// </summary>
        public int? Status { get; set; }
        
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
    /// 仓库用户关系增加输入参数
    /// </summary>
    public class AddWarehouseUserMappingInput : WarehouseUserMappingBaseInput
    {
    }

    /// <summary>
    /// 仓库用户关系删除输入参数
    /// </summary>
    public class DeleteWarehouseUserMappingInput : BaseIdInput
    {
    }

    /// <summary>
    /// 仓库用户关系更新输入参数
    /// </summary>
    public class UpdateWarehouseUserMappingInput : WarehouseUserMappingBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 仓库用户关系主键查询输入参数
    /// </summary>
    public class QueryByIdWarehouseUserMappingInput : DeleteWarehouseUserMappingInput
    {

    }
