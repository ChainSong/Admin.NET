namespace Admin.NET.Application;

    /// <summary>
    /// WMSLocation输出参数
    /// </summary>
    public class WMSLocationDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
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
        public int? LocationStatus { get; set; }
        
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
        public DateTime? CreationTime { get; set; }
        
        /// <summary>
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }
