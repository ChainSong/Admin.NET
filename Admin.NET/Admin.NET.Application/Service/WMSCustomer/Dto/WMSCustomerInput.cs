using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// Customer基础输入参数
/// </summary>
public class WMSCustomerBaseInput
{

    public virtual long Id { get; set; }
    /// <summary>
    /// ProjectId
    /// </summary>

    public virtual long ProjectId { get; set; }

    /// <summary>
    /// 客户代码
    /// </summary>
    public virtual string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public virtual string? CustomerName { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// CustomerType
    /// </summary>
    public virtual string? CustomerType { get; set; }

    /// <summary>
    /// CustomerStatus
    /// </summary>
    public virtual int CustomerStatus { get; set; }

    /// <summary>
    /// CreditLine
    /// </summary>
    public virtual string? CreditLine { get; set; }

    /// <summary>
    /// Province
    /// </summary>
    public virtual string? Province { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public virtual string? City { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public virtual string? Address { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public virtual string? Email { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public virtual string? Phone { get; set; }

    /// <summary>
    /// LawPerson
    /// </summary>
    public virtual string? LawPerson { get; set; }

    /// <summary>
    /// PostCode
    /// </summary>
    public virtual string? PostCode { get; set; }

    /// <summary>
    /// Bank
    /// </summary>
    public virtual string? Bank { get; set; }

    /// <summary>
    /// Account
    /// </summary>
    public virtual string? Account { get; set; }

    /// <summary>
    /// TaxId
    /// </summary>
    public virtual string? TaxId { get; set; }

    /// <summary>
    /// InvoiceTitle
    /// </summary>
    public virtual string? InvoiceTitle { get; set; }

    /// <summary>
    /// Fax
    /// </summary>
    public virtual string? Fax { get; set; }

    /// <summary>
    /// WebSite
    /// </summary>
    public virtual string? WebSite { get; set; }

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

}

/// <summary>
/// Customer分页查询输入参数
/// </summary>
public class WMSCustomerInput : BasePageInput
{
    /// <summary>
    /// ProjectId
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 客户代码
    /// </summary>
    public string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// CustomerType
    /// </summary>
    public string? CustomerType { get; set; }

    /// <summary>
    /// CustomerStatus
    /// </summary>
    public int CustomerStatus { get; set; }

    /// <summary>
    /// CreditLine
    /// </summary>
    public string? CreditLine { get; set; }

    /// <summary>
    /// Province
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// LawPerson
    /// </summary>
    public string? LawPerson { get; set; }

    /// <summary>
    /// PostCode
    /// </summary>
    public string? PostCode { get; set; }

    /// <summary>
    /// Bank
    /// </summary>
    public string? Bank { get; set; }

    /// <summary>
    /// Account
    /// </summary>
    public string? Account { get; set; }

    /// <summary>
    /// TaxId
    /// </summary>
    public string? TaxId { get; set; }

    /// <summary>
    /// InvoiceTitle
    /// </summary>
    public string? InvoiceTitle { get; set; }

    /// <summary>
    /// Fax
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// WebSite
    /// </summary>
    public string? WebSite { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    //public List<DateTime?> CreationTime { get; set; }

    /// <summary>
    /// CreationTime范围
    /// </summary>
    public List<DateTime?> CreationTime { get; set; }
    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

    public virtual List<WMSCustomerDetail> Details { get; set; }

}

/// <summary>
/// Customer增加输入参数
/// </summary>
public class AddWMSCustomerInput : WMSCustomerBaseInput
{

}

/// <summary>
/// Customer删除输入参数
/// </summary>
public class DeleteWMSCustomerInput : BaseIdInput
{
    public virtual List<WMSCustomerDetail> Details { get; set; }
}

/// <summary>
/// Customer更新输入参数
/// </summary>
public class UpdateWMSCustomerInput  
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }


    /// <summary>
    /// ProjectId
    /// </summary>

    public virtual long ProjectId { get; set; }

    /// <summary>
    /// 客户代码
    /// </summary>
    public virtual string? CustomerCode { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public virtual string? CustomerName { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// CustomerType
    /// </summary>
    public virtual string? CustomerType { get; set; }

    /// <summary>
    /// CustomerStatus
    /// </summary>
    public virtual int CustomerStatus { get; set; }

    /// <summary>
    /// CreditLine
    /// </summary>
    public virtual string? CreditLine { get; set; }

    /// <summary>
    /// Province
    /// </summary>
    public virtual string? Province { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public virtual string? City { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public virtual string? Address { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public virtual string? Email { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public virtual string? Phone { get; set; }

    /// <summary>
    /// LawPerson
    /// </summary>
    public virtual string? LawPerson { get; set; }

    /// <summary>
    /// PostCode
    /// </summary>
    public virtual string? PostCode { get; set; }

    /// <summary>
    /// Bank
    /// </summary>
    public virtual string? Bank { get; set; }

    /// <summary>
    /// Account
    /// </summary>
    public virtual string? Account { get; set; }

    /// <summary>
    /// TaxId
    /// </summary>
    public virtual string? TaxId { get; set; }

    /// <summary>
    /// InvoiceTitle
    /// </summary>
    public virtual string? InvoiceTitle { get; set; }

    /// <summary>
    /// Fax
    /// </summary>
    public virtual string? Fax { get; set; }

    /// <summary>
    /// WebSite
    /// </summary>
    public virtual string? WebSite { get; set; }


    public virtual List<WMSCustomerDetail> Details { get; set; }
    public WMSCustomerConfig CustomerConfig { get; set; }
}

/// <summary>
/// Customer主键查询输入参数
/// </summary>
public class QueryByIdWMSCustomerInput : DeleteWMSCustomerInput
{

}
