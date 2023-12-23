using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// MMSSupplier基础输入参数
    /// </summary>
    public class MMSSupplierBaseInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual long Id { get; set; }
        
        /// <summary>
        /// ProjectId
        /// </summary>
        public virtual long ProjectId { get; set; }
        
        /// <summary>
        /// SupplierCode
        /// </summary>
        public virtual string? SupplierCode { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public virtual string? SupplierName { get; set; }
        
        /// <summary>
        /// Description
        /// </summary>
        public virtual string? Description { get; set; }
        
        /// <summary>
        /// SupplierType
        /// </summary>
        public virtual string? SupplierType { get; set; }
        
        /// <summary>
        /// SupplierStatus
        /// </summary>
        public virtual int SupplierStatus { get; set; }
        
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
        /// Updator
        /// </summary>
        public virtual string? Updator { get; set; }
        
        /// <summary>
        /// D
        /// </summary>
        public virtual DateTime? UpdateTime { get; set; }
        
    }

    /// <summary>
    /// MMSSupplier分页查询输入参数
    /// </summary>
    public class MMSSupplierInput : BasePageInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// ProjectId
        /// </summary>
        public long ProjectId { get; set; }
        
        /// <summary>
        /// SupplierCode
        /// </summary>
        public string? SupplierCode { get; set; }
        
        /// <summary>
        /// SupplierName
        /// </summary>
        public string? SupplierName { get; set; }
        
        /// <summary>
        /// Description
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// SupplierType
        /// </summary>
        public string? SupplierType { get; set; }
        
        /// <summary>
        /// SupplierStatus
        /// </summary>
        public int SupplierStatus { get; set; }
        
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
        /// Updator
        /// </summary>
        public string? Updator { get; set; }
        
        /// <summary>
        /// D
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        
        /// <summary>
         /// D范围
         /// </summary>
         public List<DateTime?> UpdateTimeRange { get; set; } 
    }

    /// <summary>
    /// MMSSupplier增加输入参数
    /// </summary>
    public class AddMMSSupplierInput : MMSSupplierBaseInput
    {
    }

    /// <summary>
    /// MMSSupplier删除输入参数
    /// </summary>
    public class DeleteMMSSupplierInput : BaseIdInput
    {
    }

    /// <summary>
    /// MMSSupplier更新输入参数
    /// </summary>
    public class UpdateMMSSupplierInput : MMSSupplierBaseInput
    {
    }

    /// <summary>
    /// MMSSupplier主键查询输入参数
    /// </summary>
    public class QueryByIdMMSSupplierInput : DeleteMMSSupplierInput
    {

    }
