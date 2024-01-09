namespace Admin.NET.Application;

    /// <summary>
    /// WMSInventoryReport输出参数
    /// </summary>
    public class WMSInventoryReportOutput
    {
       /// <summary>
       /// Id
       /// </summary>
       public long Id { get; set; }
    
       /// <summary>
       /// InboundQty
       /// </summary>
       public double? InboundQty { get; set; }
    
       /// <summary>
       /// OutboundQty
       /// </summary>
       public double? OutboundQty { get; set; }
    
       /// <summary>
       /// AvailableInventory
       /// </summary>
       public double? AvailableInventory { get; set; }
    
       /// <summary>
       /// FreezeInventory
       /// </summary>
       public double? FreezeInventory { get; set; }
    
       /// <summary>
       /// OccupyInventory
       /// </summary>
       public double? OccupyInventory { get; set; }
    
       /// <summary>
       /// AdjustInventory
       /// </summary>
       public double? AdjustInventory { get; set; }
    
       /// <summary>
       /// SKU
       /// </summary>
       public string? SKU { get; set; }
    
       /// <summary>
       /// Qty
       /// </summary>
       public double? Qty { get; set; }
    
       /// <summary>
       /// InventoryType
       /// </summary>
       public string? InventoryType { get; set; }
    
       /// <summary>
       /// InventoryStatus
       /// </summary>
       public int? InventoryStatus { get; set; }
    
       /// <summary>
       /// InventoryDate
       /// </summary>
       public DateTime? InventoryDate { get; set; }
    
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
       public long WarehouseId { get; set; }
    
       /// <summary>
       /// WarehouseName
       /// </summary>
       public string WarehouseName { get; set; }
    
    }
 

