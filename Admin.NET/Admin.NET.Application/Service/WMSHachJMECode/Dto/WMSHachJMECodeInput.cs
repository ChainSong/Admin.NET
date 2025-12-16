using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// JME打印基础输入参数
    /// </summary>
    public class WMSHachJMECodeBaseInput
    {
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string? SKU { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public virtual string? GoodsName { get; set; }
        
        /// <summary>
        /// JMECode
        /// </summary>
        public virtual string? JMECode { get; set; }
        
        /// <summary>
        /// QRCode
        /// </summary>
        public virtual string? QRCode { get; set; }
        
        /// <summary>
        /// Url
        /// </summary>
        public virtual string? Url { get; set; }
        
        /// <summary>
        /// JMEData
        /// </summary>
        public virtual string? JMEData { get; set; }
        
        /// <summary>
        /// PrintNum
        /// </summary>
        public virtual int? PrintNum { get; set; }
        
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
    /// JME打印分页查询输入参数
    /// </summary>
    public class WMSHachJMECodeInput : BasePageInput
    {
        /// <summary>
        /// SKU
        /// </summary>
        public string? SKU { get; set; }
        
        /// <summary>
        /// GoodsName
        /// </summary>
        public string? GoodsName { get; set; }
        
        /// <summary>
        /// JMECode
        /// </summary>
        public string? JMECode { get; set; }
        
        /// <summary>
        /// QRCode
        /// </summary>
        public string? QRCode { get; set; }
        
        /// <summary>
        /// Url
        /// </summary>
        public string? Url { get; set; }
        
        /// <summary>
        /// JMEData
        /// </summary>
        public string? JMEData { get; set; }
        
        /// <summary>
        /// PrintNum
        /// </summary>
        public int? PrintNum { get; set; }
        
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
    /// JME打印增加输入参数
    /// </summary>
    public class AddWMSHachJMECodeInput : WMSHachJMECodeBaseInput
    {
    }

    /// <summary>
    /// JME打印删除输入参数
    /// </summary>
    public class DeleteWMSHachJMECodeInput : BaseIdInput
    {
    }

    /// <summary>
    /// JME打印更新输入参数
    /// </summary>
    public class UpdateWMSHachJMECodeInput : WMSHachJMECodeBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// JME打印主键查询输入参数
    /// </summary>
    public class QueryByIdWMSHachJMECodeInput : DeleteWMSHachJMECodeInput
    {

    }
