namespace Admin.NET.Application;

    /// <summary>
    /// WMSRFReceiptAcquisition输出参数
    /// </summary>
    public class WMSRFReceiptAcquisitionDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// ASNId
        /// </summary>
        public long ASNId { get; set; }
        
        /// <summary>
        /// ASNNumber
        /// </summary>
        public string ASNNumber { get; set; }



    /// <summary>
    /// 
    /// </summary> 
    public long ReceiptDetailId { get; set; }
    
    /// <summary>
    /// 收货单号，系统自动生成
    /// </summary>
    public string ReceiptNumber { get; set; }
        
        /// <summary>
        /// ExternReceiptNumber
        /// </summary>
        public string? ExternReceiptNumber { get; set; }
        
        /// <summary>
        /// CustomerId
        /// </summary>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// CustomerName
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long? WarehouseId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// SKU
        /// </summary>
        public string SKU { get; set; }
        
        /// <summary>
        /// Lot
        /// </summary>
        public string Lot { get; set; }
        
        /// <summary>
        /// ProductionDate
        /// </summary>
        public object? ProductionDate { get; set; }
        
        /// <summary>
        /// ExpirationDate
        /// </summary>
        public object? ExpirationDate { get; set; }
        
        /// <summary>
        /// Creator
        /// </summary>
        public string Creator { get; set; }
        
        /// <summary>
        /// CreationTime
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
    }
