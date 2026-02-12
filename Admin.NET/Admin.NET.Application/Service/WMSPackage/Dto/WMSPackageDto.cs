namespace Admin.NET.Application;

/// <summary>
/// WMSPackage输出参数
/// </summary>
public class WMSPackageDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// OrderId
    /// </summary>
    public long? OrderId { get; set; }

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
    ///  
    /// </summary>
    public string PreOrderNumber { get; set; }

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
    /// 是否主单
    /// </summary>
    public int? IsComposited { get; set; }

    /// <summary>
    /// 是否交接
    /// </summary>
    public int? IsHandovered { get; set; }

    /// <summary>
    /// 交接人
    /// </summary>
    public string? Handoveror { get; set; }

    /// <summary>
    /// 交接时间
    /// </summary>
    public DateTime? HandoverTime { get; set; }

    /// <summary>
    /// PackageStatus
    /// </summary>
    public int PackageStatus { get; set; }

    /// <summary>
    /// 包装时间
    /// </summary>
    public DateTime? PackageTime { get; set; }

     

    public int? PrintNum { get; set; }
     
    public string? PrintPersonnel { get; set; }

     
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// 明细数量
    /// </summary>
    public int? DetailCount { get; set; }

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

    /// <summary>
    /// Str1
    /// </summary>
    public string? Str1 { get; set; }

    /// <summary>
    /// Str2
    /// </summary>
    public string? Str2 { get; set; }

    /// <summary>
    /// Str3
    /// </summary>
    public string? Str3 { get; set; }

    /// <summary>
    /// Str4
    /// </summary>
    public string? Str4 { get; set; }

    /// <summary>
    /// Str5
    /// </summary>
    public string? Str5 { get; set; }

    /// <summary>
    /// Str6
    /// </summary>
    public string? Str6 { get; set; }

    /// <summary>
    /// Str7
    /// </summary>
    public string? Str7 { get; set; }

    /// <summary>
    /// Str8
    /// </summary>
    public string? Str8 { get; set; }

    /// <summary>
    /// Str9
    /// </summary>
    public string? Str9 { get; set; }

    /// <summary>
    /// Str10
    /// </summary>
    public string? Str10 { get; set; }

    /// <summary>
    /// Str11
    /// </summary>
    public string? Str11 { get; set; }

    /// <summary>
    /// Str12
    /// </summary>
    public string? Str12 { get; set; }

    /// <summary>
    /// Str13
    /// </summary>
    public string? Str13 { get; set; }

    /// <summary>
    /// Str14
    /// </summary>
    public string? Str14 { get; set; }

    /// <summary>
    /// Str15
    /// </summary>
    public string? Str15 { get; set; }

    /// <summary>
    /// Str16
    /// </summary>
    public string? Str16 { get; set; }

    /// <summary>
    /// Str17
    /// </summary>
    public string? Str17 { get; set; }

    /// <summary>
    /// Str18
    /// </summary>
    public string? Str18 { get; set; }

    /// <summary>
    /// Str19
    /// </summary>
    public string? Str19 { get; set; }

    /// <summary>
    /// Str20
    /// </summary>
    public string? Str20 { get; set; }

    /// <summary>
    /// DateTime1
    /// </summary>
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime4
    /// </summary>
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime5
    /// </summary>
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public int? Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public int? Int3 { get; set; }

    /// <summary>
    /// Int4
    /// </summary>
    public int? Int4 { get; set; }

    /// <summary>
    /// Int5
    /// </summary>
    public int? Int5 { get; set; }

}
