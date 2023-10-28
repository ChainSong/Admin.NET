namespace Admin.NET.Application;

    /// <summary>
    /// 库区管理输出参数
    /// </summary>
    public class WMSAreaDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
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
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }
