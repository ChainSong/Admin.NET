using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSProduct基础输入参数
/// </summary>
public class AddOrUpdateProductBomInput
{

    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public long ProductId { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public string SKU { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public long ChildSKUId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? ChildSKU { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? UnitCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? ChildGoodsName { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public double Qty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public int Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public int Int2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    
    public int Int3 { get; set; }

}
