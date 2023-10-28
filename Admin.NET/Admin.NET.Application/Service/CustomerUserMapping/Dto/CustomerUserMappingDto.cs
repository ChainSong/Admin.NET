namespace Admin.NET.Application;

    /// <summary>
    /// 客户用户关系输出参数
    /// </summary>
    public class CustomerUserMappingDto
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
        /// CustomerId
        /// </summary>
        public long? CustomerId { get; set; }
        
        /// <summary>
        /// CustomerName
        /// </summary>
        public string? CustomerName { get; set; }
        
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
