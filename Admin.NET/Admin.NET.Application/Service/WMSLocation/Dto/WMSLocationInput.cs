using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// WMSLocation基础输入参数
    /// </summary>
    public class WMSLocationBaseInput
{
        /// <summary>
        /// WarehouseId
        /// </summary>
        public virtual long WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public virtual string WarehouseName { get; set; }
        
        /// <summary>
        /// AreaId
        /// </summary>
        public virtual long AreaId { get; set; }
        
        /// <summary>
        /// AreaName
        /// </summary>
        public virtual string AreaName { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        public virtual string Location { get; set; }
        
        /// <summary>
        /// LocationStatus
        /// </summary>
        public virtual int? LocationStatus { get; set; }
        
        /// <summary>
        /// LocationType
        /// </summary>
        public virtual string? LocationType { get; set; }
        
        /// <summary>
        /// Classification
        /// </summary>
        public virtual int? Classification { get; set; }
        
        /// <summary>
        /// Category
        /// </summary>
        public virtual int? Category { get; set; }
        
        /// <summary>
        /// ABCClassification
        /// </summary>
        public virtual string? ABCClassification { get; set; }
        
        /// <summary>
        /// IsMultiLot
        /// </summary>
        public virtual int? IsMultiLot { get; set; }
        
        /// <summary>
        /// IsMultiSKU
        /// </summary>
        public virtual int? IsMultiSKU { get; set; }
        
        /// <summary>
        /// LocationLevel
        /// </summary>
        public virtual string? LocationLevel { get; set; }
        
        /// <summary>
        /// GoodsPutOrder
        /// </summary>
        public virtual string? GoodsPutOrder { get; set; }
        
        /// <summary>
        /// GoodsPickOrder
        /// </summary>
        public virtual string? GoodsPickOrder { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public virtual string? SKU { get; set; }
        
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
    /// WMSLocation分页查询输入参数
    /// </summary>
    public class WMSLocationInput : BasePageInput
    {
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string WarehouseName { get; set; }
        
        /// <summary>
        /// AreaId
        /// </summary>
        public long AreaId { get; set; }
        
        /// <summary>
        /// AreaName
        /// </summary>
        public string AreaName { get; set; }
        
        /// <summary>
        /// Location
        /// </summary>
        public string Location { get; set; }
        
        /// <summary>
        /// LocationStatus
        /// </summary>
        public int LocationStatus { get; set; }
        
        /// <summary>
        /// LocationType
        /// </summary>
        public string? LocationType { get; set; }
        
        /// <summary>
        /// Classification
        /// </summary>
        public int? Classification { get; set; }
        
        /// <summary>
        /// Category
        /// </summary>
        public int? Category { get; set; }
        
        /// <summary>
        /// ABCClassification
        /// </summary>
        public string? ABCClassification { get; set; }
        
        /// <summary>
        /// IsMultiLot
        /// </summary>
        public int? IsMultiLot { get; set; }
        
        /// <summary>
        /// IsMultiSKU
        /// </summary>
        public int? IsMultiSKU { get; set; }
        
        /// <summary>
        /// LocationLevel
        /// </summary>
        public string? LocationLevel { get; set; }
        
        /// <summary>
        /// GoodsPutOrder
        /// </summary>
        public string? GoodsPutOrder { get; set; }
        
        /// <summary>
        /// GoodsPickOrder
        /// </summary>
        public string? GoodsPickOrder { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public string? SKU { get; set; }
        
        /// <summary>
        /// Creator
        /// </summary>
        public string? Creator { get; set; }
        
        /// <summary>
        /// CreationTime
        /// </summary>
        //public DateTime? CreationTime { get; set; }
        
        /// <summary>
         /// CreationTime范围
         /// </summary>
         public List<DateTime?> CreationTime { get; set; } 
        /// <summary>
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }

    /// <summary>
    /// WMSLocation增加输入参数
    /// </summary>
    public class AddWMSLocationInput : WMSLocationBaseInput
{
    }

    /// <summary>
    /// WMSLocation删除输入参数
    /// </summary>
    public class DeleteWMSLocationInput : BaseIdInput
    {
    }

    /// <summary>
    /// WMSLocation更新输入参数
    /// </summary>
    public class Update库位Input : WMSLocationBaseInput
{
        /// <summary>
        /// Id
        /// </summary>
        [Required(ErrorMessage = "Id不能为空")]
        public long Id { get; set; }
        
    }

    /// <summary>
    /// WMSLocation主键查询输入参数
    /// </summary>
    public class QueryByIdWMSLocationInput : DeleteWMSLocationInput
{

    }
