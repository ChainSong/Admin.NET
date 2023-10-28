namespace Admin.NET.Core.Entity;

/// <summary>
/// 可用库存
/// </summary>
[SugarTable("WMS_Inventory_Usable", "可用库存")]
public class WMSInventoryUsable  
{

    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }

   

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long ReceiptDetailId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long ReceiptReceivingId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50,DefaultValue ="")]
    public string WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Area { get; set; }  

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Location { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string SKU { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string UPC { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string GoodsType { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public int InventoryStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long SuperId { get; set; }  

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long RelatedId { get; set; }  

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string GoodsName { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string UnitCode { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Onwer { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string BoxCode { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string TrayCode { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string BatchCode { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public double Qty { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>
 
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
 
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200, DefaultValue = "")]
    public string Remark { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>

    [SugarColumn(ColumnDescription = "", DefaultValue = "")]
    public DateTime? InventoryTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
  
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Creator { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>

    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Updator { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>

    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Str1 { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string Str2 { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string Str3 { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100, DefaultValue = "")]
    public string Str4 { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500, DefaultValue = "")]
    public string Str5 { get; set; } = "";

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int Int2 { get; set; }

}
