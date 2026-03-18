using SqlSugar;
using System.ComponentModel.DataAnnotations;
using TaskPlaApplication.Models;

namespace TaskPlaApplication.Domain.Entities;

/// <summary>
/// 出库明细
/// </summary>
[SugarTable("WMS_OrderDetail","出库明细")]
public class WMSOrderDetail : ITenantIdFilter
{

    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }

    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long PreOrderId { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long PreOrderDetailId { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long OrderId { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string PreOrderNumber { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? OrderNumber { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? ExternOrderNumber { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long? CustomerId { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? CustomerName { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }
  
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? WarehouseName { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? LineNumber { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? SKU { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? UPC { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string GoodsName { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? GoodsType { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public double OrderQty { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public double AllocatedQty { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? BoxCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? TrayCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? BatchCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? LotCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PoCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50, DefaultValue = "")]
    public string? SoCode { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public double? Weight { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public double? Volume { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 100)]
    public string? UnitCode { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Onwer { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? ProductionDate { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? ExpirationDate { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Updator { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str1 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str2 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str3 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str4 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str5 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str6 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str7 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str8 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str9 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str10 { get; set; }
    
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str11 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str12 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str13 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str14 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str15 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str16 { get; set; }

    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str17 { get; set; }
   
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str18 { get; set; }
    

    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str19 { get; set; }
    
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str20 { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime1 { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime2 { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime3 { get; set; }
    

    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? DateTime5 { get; set; }
    
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
    public int? Int4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public int? Int5 { get; set; }
    
}
