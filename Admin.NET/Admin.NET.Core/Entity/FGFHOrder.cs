namespace Admin.NET.Core.Entity;

/// <summary>
/// 福光出库对接表
/// </summary>
[SugarTable("FGFH_Order", "FGFH_Order")]
public class FGFHOrder
{


    /// <summary>
    /// 雪花Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public long OutStockMID { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string OutStockNo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? OutStockNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "")]
    public DateTime OutStockDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string OutStockType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 300)]
    public string? DownloadURL { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? ContractNo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? Addressee { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Phone { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Address { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? IsReturn { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? ReturnDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Reason { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? Datetime1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? Datetime2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? Datetime3 { get; set; }

}
