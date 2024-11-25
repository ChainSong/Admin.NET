using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSProduct基础输入参数
/// </summary>
public class WMSProductBaseInput
{
    /// <summary>
    /// CustomerId
    /// </summary>
    public virtual long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public virtual string? CustomerName { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public virtual string SKU { get; set; }

    /// <summary>
    /// ProductStatus
    /// </summary>
    public virtual int ProductStatus { get; set; }

    /// <summary>
    /// GoodsName
    /// </summary>
    public virtual string GoodsName { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public virtual string? GoodsType { get; set; }

    /// <summary>
    /// SKUClassification
    /// </summary>
    public virtual string? SKUClassification { get; set; }

    /// <summary>
    /// SKULevel
    /// </summary>
    public virtual string? SKULevel { get; set; }

    /// <summary>
    /// SuperId
    /// </summary>
    public virtual long SuperId { get; set; }

    /// <summary>
    /// SKUGroup
    /// </summary>
    public virtual string? SKUGroup { get; set; }

    /// <summary>
    /// ManufacturerSKU
    /// </summary>
    public virtual string? ManufacturerSKU { get; set; }

    /// <summary>
    /// RetailSKU
    /// </summary>
    public virtual string? RetailSKU { get; set; }

    /// <summary>
    /// ReplaceSKU
    /// </summary>
    public virtual string? ReplaceSKU { get; set; }

    /// <summary>
    /// BoxGroup
    /// </summary>
    public virtual string? BoxGroup { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public virtual string? Country { get; set; }

    /// <summary>
    /// Manufacturer
    /// </summary>
    public virtual string? Manufacturer { get; set; }

    /// <summary>
    /// DangerCode
    /// </summary>
    public virtual string? DangerCode { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    public virtual double Volume { get; set; }

    /// <summary>
    /// StandardVolume
    /// </summary>
    public virtual double StandardVolume { get; set; }

    /// <summary>
    /// Weight
    /// </summary>
    public virtual double Weight { get; set; }

    /// <summary>
    /// StandardWeight
    /// </summary>
    public virtual double StandardWeight { get; set; }

    /// <summary>
    /// NetWeight
    /// </summary>
    public virtual double NetWeight { get; set; }

    /// <summary>
    /// StandardNetWeight
    /// </summary>
    public virtual double StandardNetWeight { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public virtual double Price { get; set; }

    /// <summary>
    /// ActualPrice
    /// </summary>
    public virtual double ActualPrice { get; set; }

    /// <summary>
    /// Cost
    /// </summary>
    public virtual string? Cost { get; set; }

    /// <summary>
    /// Color
    /// </summary>
    public virtual string? Color { get; set; }

    /// <summary>
    /// Length
    /// </summary>
    public virtual double Length { get; set; }

    /// <summary>
    /// Wide
    /// </summary>
    public virtual double Wide { get; set; }

    /// <summary>
    /// High
    /// </summary>
    public virtual double High { get; set; }

    /// <summary>
    /// ExpirationDate
    /// </summary>
    public virtual int ExpirationDate { get; set; }

    public int IsNFC { get; set; }
    /// <summary>
    /// 是否RFID
    /// </summary>
    public int IsRFID { get; set; }
    /// <summary>
    ///  是否质检
    /// </summary>
    public int IsQC { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime? CreationTime { get; set; }

    /// <summary>
    /// Str1
    /// </summary>
    public virtual string? Str1 { get; set; }

    /// <summary>
    /// Str2
    /// </summary>
    public virtual string? Str2 { get; set; }

    /// <summary>
    /// Str3
    /// </summary>
    public virtual string? Str3 { get; set; }

    /// <summary>
    /// Str4
    /// </summary>
    public virtual string? Str4 { get; set; }

    /// <summary>
    /// Str5
    /// </summary>
    public virtual string? Str5 { get; set; }

    /// <summary>
    /// Str6
    /// </summary>
    public virtual string? Str6 { get; set; }

    /// <summary>
    /// Str7
    /// </summary>
    public virtual string? Str7 { get; set; }

    /// <summary>
    /// Str8
    /// </summary>
    public virtual string? Str8 { get; set; }

    /// <summary>
    /// Str9
    /// </summary>
    public virtual string? Str9 { get; set; }

    /// <summary>
    /// Str10
    /// </summary>
    public virtual string? Str10 { get; set; }

    /// <summary>
    /// Str11
    /// </summary>
    public virtual string? Str11 { get; set; }

    /// <summary>
    /// Str12
    /// </summary>
    public virtual string? Str12 { get; set; }

    /// <summary>
    /// Str13
    /// </summary>
    public virtual string? Str13 { get; set; }

    /// <summary>
    /// Str14
    /// </summary>
    public virtual string? Str14 { get; set; }

    /// <summary>
    /// Str15
    /// </summary>
    public virtual string? Str15 { get; set; }

    /// <summary>
    /// Str16
    /// </summary>
    public virtual string? Str16 { get; set; }

    /// <summary>
    /// Str17
    /// </summary>
    public virtual string? Str17 { get; set; }

    /// <summary>
    /// Str18
    /// </summary>
    public virtual string? Str18 { get; set; }

    /// <summary>
    /// Str19
    /// </summary>
    public virtual string? Str19 { get; set; }

    /// <summary>
    /// Str20
    /// </summary>
    public virtual string? Str20 { get; set; }

    /// <summary>
    /// DateTime1
    /// </summary>
    public virtual object? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public virtual object? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public virtual object? DateTime3 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public virtual int Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public virtual int Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public virtual int Int3 { get; set; }

}

/// <summary>
/// WMSProduct分页查询输入参数
/// </summary>
public class WMSProductInput : BasePageInput
{
    /// <summary>
    /// CustomerId
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public string SKU { get; set; }

    /// <summary>
    /// ProductStatus
    /// </summary>
    public int ProductStatus { get; set; }

    /// <summary>
    /// GoodsName
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public string? GoodsType { get; set; }

    /// <summary>
    /// SKUClassification
    /// </summary>
    public string? SKUClassification { get; set; }

    /// <summary>
    /// SKULevel
    /// </summary>
    public string? SKULevel { get; set; }

    /// <summary>
    /// SuperId
    /// </summary>
    public long SuperId { get; set; }

    /// <summary>
    /// SKUGroup
    /// </summary>
    public string? SKUGroup { get; set; }

    /// <summary>
    /// ManufacturerSKU
    /// </summary>
    public string? ManufacturerSKU { get; set; }

    /// <summary>
    /// RetailSKU
    /// </summary>
    public string? RetailSKU { get; set; }

    /// <summary>
    /// ReplaceSKU
    /// </summary>
    public string? ReplaceSKU { get; set; }

    /// <summary>
    /// BoxGroup
    /// </summary>
    public string? BoxGroup { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Manufacturer
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// DangerCode
    /// </summary>
    public string? DangerCode { get; set; }

    /// <summary>
    /// Volume
    /// </summary>
    public double Volume { get; set; }

    /// <summary>
    /// StandardVolume
    /// </summary>
    public double StandardVolume { get; set; }

    /// <summary>
    /// Weight
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// StandardWeight
    /// </summary>
    public double StandardWeight { get; set; }

    /// <summary>
    /// NetWeight
    /// </summary>
    public double NetWeight { get; set; }

    /// <summary>
    /// StandardNetWeight
    /// </summary>
    public double StandardNetWeight { get; set; }

    /// <summary>
    /// Price
    /// </summary>
    public double Price { get; set; }

    /// <summary>
    /// ActualPrice
    /// </summary>
    public double ActualPrice { get; set; }

    /// <summary>
    /// Cost
    /// </summary>
    public string? Cost { get; set; }

    /// <summary>
    /// Color
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Length
    /// </summary>
    public double Length { get; set; }

    /// <summary>
    /// Wide
    /// </summary>
    public double Wide { get; set; }

    /// <summary>
    /// High
    /// </summary>
    public double High { get; set; }

    /// <summary>
    /// ExpirationDate
    /// </summary>
    public int ExpirationDate { get; set; }

    public int IsNFC { get; set; }
    /// <summary>
    /// 是否RFID
    /// </summary>
    public int IsRFID { get; set; }
    /// <summary>
    ///  是否质检
    /// </summary>
    public int IsQC { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    //public DateTime? CreationTime { get; set; }

    /// <summary>
    /// CreationTime范围
    /// </summary>
    public List<DateTime?> CreationTime { get; set; }
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
    public object? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public object? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public object? DateTime3 { get; set; }

    /// <summary>
    /// Int1
    /// </summary>
    public int Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public int Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public int Int3 { get; set; }

}

/// <summary>
/// WMSProduct增加输入参数
/// </summary>
public class AddOrUpdateProductInput : WMSProductBaseInput
{
    public long? Id { get; set; }
}

/// <summary>
/// WMSProduct删除输入参数
/// </summary>
public class DeleteWMSProductInput : BaseIdInput
{
}

/// <summary>
/// WMSProduct更新输入参数
/// </summary>
public class UpdateWMSProductInput : WMSProductBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSProduct主键查询输入参数
/// </summary>
public class QueryByIdWMSProductInput : DeleteWMSProductInput
{

}
