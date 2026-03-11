namespace Admin.NET.Application;

    /// <summary>
    /// WMS_RFReceiptAcquisition输出参数
    /// </summary>
    public class WMSRFReceiptAcquisitionOutput
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
       /// ReceiptDetailId
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
       /// SN
       /// </summary>
       public string SN { get; set; }
    
       /// <summary>
       /// Qty
       /// </summary>
       public double Qty { get; set; }
    
       /// <summary>
       /// ProductionDate
       /// </summary>
       public DateTime? ProductionDate { get; set; }
    
       /// <summary>
       /// ExpirationDate
       /// </summary>
       public DateTime? ExpirationDate { get; set; }
    
       /// <summary>
       /// ReceiptAcquisitionStatus
       /// </summary>
       public int ReceiptAcquisitionStatus { get; set; }
    
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
    
       /// <summary>
       /// 类型：SN序列号，AFC防伪码
       /// </summary>
       public string? Type { get; set; }
    
    }
 

