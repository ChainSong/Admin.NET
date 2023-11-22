namespace Admin.NET.Application;

    /// <summary>
    /// WMSLowCode输出参数
    /// </summary>
    public class WMSLowCodeDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get; set; }
        
        /// <summary>
        /// UICode
        /// </summary>
        public string? UICode { get; set; }
        
        /// <summary>
        /// UIType
        /// </summary>
        public string? UIType { get; set; }
        
        /// <summary>
        /// DataSource
        /// </summary>
        public string? DataSource { get; set; }
        
        /// <summary>
        /// Creator
        /// </summary>
        public string? Creator { get; set; }
        
        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime? CreationTime { get; set; }
        
    }
