namespace Admin.NET.Application;

    /// <summary>
    /// WMSPackageLable输出参数
    /// </summary>
    public class WMSPackageLableDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// OrderId
        /// </summary>
        public long OrderId { get; set; }
        
        /// <summary>
        /// PreOrderNumber
        /// </summary>
        public string? PreOrderNumber { get; set; }
        
        /// <summary>
        /// 包装单号
        /// </summary>
        public string PackageNumber { get; set; }
        
        /// <summary>
        /// 出库单号
        /// </summary>
        public string OrderNumber { get; set; }
        
        /// <summary>
        /// 出库外部编号
        /// </summary>
        public string ExternOrderNumber { get; set; }
        
        /// <summary>
        /// 货主ID
        /// </summary>
        public long CustomerId { get; set; }
        
        /// <summary>
        /// 货主名称
        /// </summary>
        public string CustomerName { get; set; }
        
        /// <summary>
        /// WarehouseId
        /// </summary>
        public long? WarehouseId { get; set; }
        
        /// <summary>
        /// 出库仓库
        /// </summary>
        public string? WarehouseName { get; set; }
        
        /// <summary>
        /// 包装类型
        /// </summary>
        public string? PackageType { get; set; }
        
        /// <summary>
        /// 长
        /// </summary>
        public double? Length { get; set; }
        
        /// <summary>
        /// 宽
        /// </summary>
        public double? Width { get; set; }
        
        /// <summary>
        /// 高
        /// </summary>
        public double? Height { get; set; }
        
        /// <summary>
        /// 净重
        /// </summary>
        public double? NetWeight { get; set; }
        
        /// <summary>
        /// 毛重
        /// </summary>
        public double? GrossWeight { get; set; }
        
        /// <summary>
        /// 快递公司
        /// </summary>
        public string? ExpressCompany { get; set; }
        
        /// <summary>
        /// 快递单号
        /// </summary>
        public string? ExpressNumber { get; set; }
        
        /// <summary>
        /// 序号
        /// </summary>
        public string? SerialNumber { get; set; }
        
        /// <summary>
        /// 打印次数
        /// </summary>
        public int? PrintNum { get; set; }
        
        /// <summary>
        /// 打印人
        /// </summary>
        public string? PrintPersonnel { get; set; }
        
        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime? PrintTime { get; set; }
        
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
        
        /// <summary>
        /// Remark
        /// </summary>
        public string? Remark { get; set; }
        
    }
