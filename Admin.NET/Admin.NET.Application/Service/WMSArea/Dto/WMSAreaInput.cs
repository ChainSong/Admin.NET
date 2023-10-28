using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 库区管理基础输入参数
    /// </summary>
    public class WMSAreaBaseInput
    {
        /// <summary>
        /// WarehouseId
        /// </summary>
        public virtual long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public virtual string? WarehouseName { get; set; }
        
        /// <summary>
        /// AreaName
        /// </summary>
        public virtual string? AreaName { get; set; }
        
        /// <summary>
        /// AreaStatus
        /// </summary>
        public virtual int? AreaStatus { get; set; }
        
        /// <summary>
        /// AreaType
        /// </summary>
        public virtual string? AreaType { get; set; }
        
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
        
        /// <summary>
        /// Updator
        /// </summary>
        public virtual string? Updator { get; set; }
        
    }

    /// <summary>
    /// 库区管理分页查询输入参数
    /// </summary>
    public class WMSAreaInput : BasePageInput
    {
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// AreaName
        /// </summary>
        public string? AreaName { get; set; }
        
        /// <summary>
        /// AreaStatus
        /// </summary>
        public int? AreaStatus { get; set; }
        
        /// <summary>
        /// AreaType
        /// </summary>
        public string? AreaType { get; set; }
        
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
        /// <summary>
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }

    /// <summary>
    /// 库区管理增加输入参数
    /// </summary>
    public class AddWMSAreaInput : WMSAreaBaseInput
    {
    }

    /// <summary>
    /// 库区管理删除输入参数
    /// </summary>
    public class DeleteWMSAreaInput : BaseIdInput
    {
    }

    /// <summary>
    /// 库区管理更新输入参数
    /// </summary>
    public class UpdateWMSAreaInput : WMSAreaBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// 库区管理主键查询输入参数
    /// </summary>
    public class QueryByIdWMS_AreaInput : DeleteWMSAreaInput
    {

    }
