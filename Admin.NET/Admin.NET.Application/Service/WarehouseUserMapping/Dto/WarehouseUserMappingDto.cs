namespace Admin.NET.Application;

    /// <summary>
    /// 仓库用户关系输出参数
    /// </summary>
    public class WarehouseUserMappingDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
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
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }
