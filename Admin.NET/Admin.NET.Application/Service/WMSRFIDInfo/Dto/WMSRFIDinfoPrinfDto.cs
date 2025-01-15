namespace Admin.NET.Application;

/// <summary>
/// WMSRFIDInfo输出参数
/// </summary>
public class WMSRFIDinfoPrinfDto
{
 
    /// <summary>
    /// ReceiptNumber
    /// </summary>
    public string? ReceiptNumber { get; set; }

    /// <summary>
    /// ExternReceiptNumber
    /// </summary>
    public string? ExternReceiptNumber { get; set; }


    /// <summary>
    /// SKU
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public string? GoodsType { get; set; }
   

    /// <summary>
    /// Qty
    /// </summary>
    public double? Qty { get; set; }

    /// <summary>
    /// Sequence
    /// </summary>
    public string? Sequence { get; set; }

    /// <summary>
    /// RFID
    /// </summary>
    public string? RFID { get; set; }

    /// <summary>
    /// Link
    /// </summary>
    public string? Link { get; set; }

    public string BatchCode { get; set; }

    public string PoCode { get; set; }

    public string SoCode { get; set; }

    public string SnCode { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

  

}
