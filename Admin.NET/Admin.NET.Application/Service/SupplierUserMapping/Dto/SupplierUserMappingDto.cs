namespace Admin.NET.Application;

    /// <summary>
    /// SupplierUserMapping输出参数
    /// </summary>
    public class SupplierUserMappingDto
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
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }
