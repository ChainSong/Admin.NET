namespace Admin.NET.Application;

    /// <summary>
    /// WMSWarehouse输出参数
    /// </summary>
    public class WMSWarehouseDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        
        /// <summary>
        /// WarehouseName
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// WarehouseStatus
        /// </summary>
        public int WarehouseStatus { get; set; }
        
        /// <summary>
        /// WarehouseType
        /// </summary>
        public string? WarehouseType { get; set; }
        
        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Company
        /// </summary>
        public string? Company { get; set; }
        
        /// <summary>
        /// Address
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// Province
        /// </summary>
        public string? Province { get; set; }
        
        /// <summary>
        /// City
        /// </summary>
        public string? City { get; set; }
        
        /// <summary>
        /// Contractor
        /// </summary>
        public string? Contractor { get; set; }
        
        /// <summary>
        /// ContractorAddress
        /// </summary>
        public string? ContractorAddress { get; set; }
        
        /// <summary>
        /// Mobile
        /// </summary>
        public string? Mobile { get; set; }
        
        /// <summary>
        /// Phone
        /// </summary>
        public string? Phone { get; set; }
        
        /// <summary>
        /// Fax
        /// </summary>
        public string? Fax { get; set; }
        
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }
        
        /// <summary>
        /// Remark
        /// </summary>
        public string? Remark { get; set; }
        
        /// <summary>
        /// Creator
        /// </summary>
        public string? Creator { get; set; }
        
        /// <summary>
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
        /// <summary>
        /// CreationTime
        /// </summary>
        public object CreationTime { get; set; }
        
    }
