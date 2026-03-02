using SqlSugar;
using TaskPlaApplication.Models;

namespace TaskPlaApplication.Domain.Entities;
/// <summary>
/// 包装
/// </summary>
[SugarTable("WMS_Package","包装")]
public class WMSPackage : ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }


    /// <summary>
    /// Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }

    [SugarColumn(ColumnDescription = "")]
    public long PickTaskId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long OrderId { get; set; }
    
    /// <summary>
    /// 包装单号
    /// </summary>
    
    [SugarColumn(ColumnDescription = "包装单号", Length = 50)]
    public string PackageNumber { get; set; }
    
    /// <summary>
    /// 出库单号
    /// </summary>
    
    [SugarColumn(ColumnDescription = "出库单号", Length = 50)]
    public string OrderNumber { get; set; }
    
    /// <summary>
    /// 出库外部编号
    /// </summary>
    
    [SugarColumn(ColumnDescription = "出库外部编号", Length = 50)]
    public string ExternOrderNumber { get; set; }



    /// <summary>
    /// 出库外部编号
    /// </summary>
    
    [SugarColumn(ColumnDescription = "拣货任务号", Length = 50)]
    public string PickTaskNumber { get; set; }

    

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? PreOrderNumber { get; set; }


    /// <summary>
    /// 货主ID
    /// </summary>
    
    [SugarColumn(ColumnDescription = "货主ID")]
    public long CustomerId { get; set; }
    
    /// <summary>
    /// 货主名称
    /// </summary>
    
    [SugarColumn(ColumnDescription = "货主名称", Length = 50)]
    public string CustomerName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public long? WarehouseId { get; set; }
    
    /// <summary>
    /// 出库仓库
    /// </summary>
    [SugarColumn(ColumnDescription = "出库仓库", Length = 50)]
    public string? WarehouseName { get; set; }
    
    /// <summary>
    /// 包装类型
    /// </summary>
    [SugarColumn(ColumnDescription = "包装类型", Length = 50)]
    public string? PackageType { get; set; }
    
    /// <summary>
    /// 长
    /// </summary>
    [SugarColumn(ColumnDescription = "长")]
    public double? Length { get; set; }
    
    /// <summary>
    /// 宽
    /// </summary>
    [SugarColumn(ColumnDescription = "宽")]
    public double? Width { get; set; }
    
    /// <summary>
    /// 高
    /// </summary>
    [SugarColumn(ColumnDescription = "高")]
    public double? Height { get; set; }
    
    /// <summary>
    /// 净重
    /// </summary>
    [SugarColumn(ColumnDescription = "净重")]
    public double? NetWeight { get; set; }
    
    /// <summary>
    /// 毛重
    /// </summary>
    [SugarColumn(ColumnDescription = "毛重")]
    public double? GrossWeight { get; set; }
    
    /// <summary>
    /// 快递公司
    /// </summary>
    [SugarColumn(ColumnDescription = "快递公司", Length = 50)]
    public string? ExpressCompany { get; set; }
    
    /// <summary>
    /// 快递单号
    /// </summary>
    [SugarColumn(ColumnDescription = "快递单号", Length = 50)]
    public string? ExpressNumber { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    [SugarColumn(ColumnDescription = "序号", Length = 50)]
    public string? SerialNumber { get; set; }

    /// <summary>
    /// 是否主单
    /// </summary>
    [SugarColumn(ColumnDescription = "是否主单")]
    public int? IsComposited { get; set; }
    
    /// <summary>
    /// 是否交接
    /// </summary>
    [SugarColumn(ColumnDescription = "是否交接")]
    public int? IsHandovered { get; set; }
    
    /// <summary>
    /// 交接人
    /// </summary>
    [SugarColumn(ColumnDescription = "交接人", Length = 50)]
    public string? Handoveror { get; set; }
    
    /// <summary>
    /// 交接时间
    /// </summary>
    [SugarColumn(ColumnDescription = "交接时间")]
    public DateTime? HandoverTime { get; set; }
    

    /// <summary>
    /// 打印时间
    /// </summary>
    [SugarColumn(ColumnDescription = "打印次数")]

    public int? PrintNum { get; set; }

    /// <summary>
    /// 打印时间
    /// </summary>
    [SugarColumn(ColumnDescription = "打印人")]
    public string? PrintPersonnel { get; set; }


    /// <summary>
    /// 打印时间
    /// </summary>
    [SugarColumn(ColumnDescription = "打印时间")]
    public DateTime? PrintTime { get; set; }

    

    /// <summary>
    /// 
    /// </summary>
    
    [SugarColumn(ColumnDescription = "")]
    public int PackageStatus { get; set; }
    
    /// <summary>
    /// 包装时间
    /// </summary>
    [SugarColumn(ColumnDescription = "包装时间")]
    public DateTime? PackageTime { get; set; }
    
    /// <summary>
    /// 明细数量
    /// </summary>
    [SugarColumn(ColumnDescription = "明细数量")]
    public double? DetailCount { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Creator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "")]
    public DateTime? UpdateTime { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Updator { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Remark { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str1 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str2 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str4 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str5 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str6 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str7 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str8 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str9 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str10 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str11 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str12 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str13 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str14 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 50)]
    public string? Str15 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str16 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str17 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 200)]
    public string? Str18 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str19 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [SugarColumn(ColumnDescription = "", Length = 500)]
    public string? Str20 { get; set; }
    
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
    public DateTime? DateTime3 { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
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


    [Navigate(NavigateType.OneToMany, nameof(WMSPackageDetail.PackageId))]
    public List<WMSPackageDetail> Details { get; set; }

}
