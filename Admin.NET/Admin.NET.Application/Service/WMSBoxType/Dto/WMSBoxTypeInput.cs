using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMSBoxType基础输入参数
    /// </summary>
    public class WMSBoxTypeBaseInput
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
        /// BoxCode
        /// </summary>
        public virtual string? BoxCode { get; set; }
        
        /// <summary>
        /// BoxType
        /// </summary>
        public virtual string? BoxType { get; set; }
        
        /// <summary>
        /// Length
        /// </summary>
        public virtual double? Length { get; set; }
        
        /// <summary>
        /// Width
        /// </summary>
        public virtual double? Width { get; set; }
        
        /// <summary>
        /// Height
        /// </summary>
        public virtual double? Height { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public virtual double? Volume { get; set; }
        
        /// <summary>
        /// NetWeight
        /// </summary>
        public virtual double? NetWeight { get; set; }
        
        /// <summary>
        /// GrossWeight
        /// </summary>
        public virtual double? GrossWeight { get; set; }
        
        /// <summary>
        /// BoxStatus
        /// </summary>
        public virtual int? BoxStatus { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        public virtual string? Remark { get; set; }
        
        /// <summary>
        /// Str1
        /// </summary>
        public virtual string? Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public virtual string? Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public virtual string? Str3 { get; set; }
        
        /// <summary>
        /// Str4
        /// </summary>
        public virtual string? Str4 { get; set; }
        
        /// <summary>
        /// Str5
        /// </summary>
        public virtual string? Str5 { get; set; }
        
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
    /// WMSBoxType分页查询输入参数
    /// </summary>
    public class WMSBoxTypeInput : BasePageInput
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
        /// BoxCode
        /// </summary>
        public string? BoxCode { get; set; }
        
        /// <summary>
        /// BoxType
        /// </summary>
        public string? BoxType { get; set; }
        
        /// <summary>
        /// Length
        /// </summary>
        public double? Length { get; set; }
        
        /// <summary>
        /// Width
        /// </summary>
        public double? Width { get; set; }
        
        /// <summary>
        /// Height
        /// </summary>
        public double? Height { get; set; }
        
        /// <summary>
        /// Volume
        /// </summary>
        public double? Volume { get; set; }
        
        /// <summary>
        /// NetWeight
        /// </summary>
        public double? NetWeight { get; set; }
        
        /// <summary>
        /// GrossWeight
        /// </summary>
        public double? GrossWeight { get; set; }
        
        /// <summary>
        /// BoxStatus
        /// </summary>
        public int? BoxStatus { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        public string? Remark { get; set; }
        
        /// <summary>
        /// Str1
        /// </summary>
        public string? Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public string? Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public string? Str3 { get; set; }
        
        /// <summary>
        /// Str4
        /// </summary>
        public string? Str4 { get; set; }
        
        /// <summary>
        /// Str5
        /// </summary>
        public string? Str5 { get; set; }
        
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
    /// WMSBoxType增加输入参数
    /// </summary>
    public class AddWMSBoxTypeInput : WMSBoxTypeBaseInput
    {
    }

    /// <summary>
    /// WMSBoxType删除输入参数
    /// </summary>
    public class DeleteWMSBoxTypeInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMSBoxType更新输入参数
    /// </summary>
    public class UpdateWMSBoxTypeInput : WMSBoxTypeBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// WMSBoxType主键查询输入参数
    /// </summary>
    public class QueryByIdWMSBoxTypeInput : DeleteWMSBoxTypeInput
    {

    }
