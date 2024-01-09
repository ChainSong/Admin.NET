using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSExpressConfig基础输入参数
/// </summary>
public class WMSExpressConfigBaseInput
{
    /// <summary>
    /// CustomerId
    /// </summary>
    public virtual long CustomerId { get; set; }

    /// <summary>
    /// CustomerName
    /// </summary>
    public virtual string CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public virtual long WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string WarehouseName { get; set; }

    /// <summary>
    /// ExpressCode
    /// </summary>
    public virtual string? ExpressCode { get; set; }

    /// <summary>
    /// ExpressCompany
    /// </summary>
    public virtual string? ExpressCompany { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public virtual string? Url { get; set; }
    public virtual string? UrlToken { get; set; }
    

    /// <summary>
    /// AppKey
    /// </summary>
    public virtual string? AppKey { get; set; }

    /// <summary>
    /// CompanyCode
    /// </summary>
    public virtual string? CompanyCode { get; set; }


    /// <summary>
    /// 
    /// </summary> 
    public string? TemplateCode { get; set; }


    /// <summary>
    /// 
    /// </summary> 
    public string? Env { get; set; }

    /// <summary>
    /// Sign
    /// </summary>
    public virtual string? Sign { get; set; }

    public virtual string? CustomerCode { get; set; }


    /// <summary>
    /// CustomerCode
    /// </summary>
    //public virtual string? CustomerCode { get; set; }

    /// <summary>
    /// PartnerId
    /// </summary>
    public virtual string? PartnerId { get; set; }

    /// <summary>
    /// Checkword
    /// </summary>
    public virtual string? Checkword { get; set; }

    /// <summary>
    /// ClientId
    /// </summary>
    public virtual string? ClientId { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    public virtual string? Version { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public virtual string? Password { get; set; }

    /// <summary>
    /// MonthAccount
    /// </summary>
    public virtual string? MonthAccount { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public virtual int? Status { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual DateTime? CreationTime { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public virtual string? Updator { get; set; }

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
    public virtual DateTime? DateTime1 { get; set; }

    /// <summary>
    /// DateTime2
    /// </summary>
    public virtual DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime3
    /// </summary>
    public virtual DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime4
    /// </summary>
    public virtual DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime5
    /// </summary>
    public virtual DateTime? DateTime5 { get; set; }

}

/// <summary>
/// WMSExpressConfig分页查询输入参数
/// </summary>
public class WMSExpressConfigInput : BasePageInput
{
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

    /// <summary>
    /// ExpressCode
    /// </summary>
    public string? ExpressCode { get; set; }

    /// <summary>
    /// ExpressCompany
    /// </summary>
    public string? ExpressCompany { get; set; }

    /// <summary>
    /// Url
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// AppKey
    /// </summary>
    public string? AppKey { get; set; }

    /// <summary>
    /// CompanyCode
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// Sign
    /// </summary>
    public string? Sign { get; set; }

    /// <summary>
    /// CustomerCode
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// PartnerId
    /// </summary>
    public string? PartnerId { get; set; }

    /// <summary>
    /// Checkword
    /// </summary>
    public string? Checkword { get; set; }

    /// <summary>
    /// ClientId
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// Version
    /// </summary>
    public string? Version { get; set; }

    /// <summary>
    /// Password
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// MonthAccount
    /// </summary>
    public string? MonthAccount { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// CreationTime范围
    /// </summary>
    public List<DateTime?> CreationTimeRange { get; set; }
    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

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
    /// DateTime1范围
    /// </summary>
    public List<DateTime?> DateTime1Range { get; set; }
    /// <summary>
    /// DateTime2
    /// </summary>
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// DateTime2范围
    /// </summary>
    public List<DateTime?> DateTime2Range { get; set; }
    /// <summary>
    /// DateTime3
    /// </summary>
    public DateTime? DateTime3 { get; set; }

    /// <summary>
    /// DateTime3范围
    /// </summary>
    public List<DateTime?> DateTime3Range { get; set; }
    /// <summary>
    /// DateTime4
    /// </summary>
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// DateTime4范围
    /// </summary>
    public List<DateTime?> DateTime4Range { get; set; }
    /// <summary>
    /// DateTime5
    /// </summary>
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// DateTime5范围
    /// </summary>
    public List<DateTime?> DateTime5Range { get; set; }
}

/// <summary>
/// WMSExpressConfig增加输入参数
/// </summary>
public class AddWMSExpressConfigInput : WMSExpressConfigBaseInput
{
}

/// <summary>
/// WMSExpressConfig删除输入参数
/// </summary>
public class DeleteWMSExpressConfigInput : BaseIdInput
{
}

/// <summary>
/// WMSExpressConfig更新输入参数
/// </summary>
public class UpdateWMSExpressConfigInput : WMSExpressConfigBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSExpressConfig主键查询输入参数
/// </summary>
public class QueryByIdWMSExpressConfigInput : DeleteWMSExpressConfigInput
{

}
