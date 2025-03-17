using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// SysPresetQuery基础输入参数
    /// </summary>
    public class SysPresetQueryBaseInput
    {
        /// <summary>
        /// QueryName
        /// </summary>
        public virtual string? QueryName { get; set; }
        
        /// <summary>
        /// BusinessName
        /// </summary>
        public virtual string? BusinessName { get; set; }
        
        /// <summary>
        /// QueryForm
        /// </summary>
        public virtual string? QueryForm { get; set; }
        
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
    /// SysPresetQuery分页查询输入参数
    /// </summary>
    public class SysPresetQueryInput : BasePageInput
    {
        /// <summary>
        /// QueryName
        /// </summary>
        public string? QueryName { get; set; }
        
        /// <summary>
        /// BusinessName
        /// </summary>
        public string? BusinessName { get; set; }
        
        /// <summary>
        /// QueryForm
        /// </summary>
        public string? QueryForm { get; set; }
        
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
    /// SysPresetQuery增加输入参数
    /// </summary>
    public class AddSysPresetQueryInput : SysPresetQueryBaseInput
    {
    }

    /// <summary>
    /// SysPresetQuery删除输入参数
    /// </summary>
    public class DeleteSysPresetQueryInput : BaseIdInput
    {
    }

    /// <summary>
    /// SysPresetQuery更新输入参数
    /// </summary>
    public class UpdateSysPresetQueryInput : SysPresetQueryBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// SysPresetQuery主键查询输入参数
    /// </summary>
    public class QueryByIdSysPresetQueryInput : DeleteSysPresetQueryInput
    {

    }
