using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSWarehouse基础输入参数
/// </summary>
public class WMSWarehouseBaseInput
{
    /// <summary>
    /// ProjectId
    /// </summary>
    public virtual long ProjectId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string? WarehouseName { get; set; }

    /// <summary>
    /// WarehouseStatus
    /// </summary>
    public virtual int WarehouseStatus { get; set; }

    /// <summary>
    /// WarehouseType
    /// </summary>
    public virtual string? WarehouseType { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// Company
    /// </summary>
    public virtual string? Company { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public virtual string? Address { get; set; }

    /// <summary>
    /// Province
    /// </summary>
    public virtual string? Province { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public virtual string? City { get; set; }

    /// <summary>
    /// Country
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Contractor
    /// </summary>
    public virtual string? Contractor { get; set; }

    /// <summary>
    /// ContractorAddress
    /// </summary>
    public virtual string? ContractorAddress { get; set; }

    /// <summary>
    /// Mobile
    /// </summary>
    public virtual string? Mobile { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public virtual string? Phone { get; set; }

    /// <summary>
    /// Fax
    /// </summary>
    public virtual string? Fax { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public virtual string? Email { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public virtual string? Creator { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public virtual string? Updator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public virtual object CreationTime { get; set; }

}

/// <summary>
/// WMSWarehouse分页查询输入参数
/// </summary>
public class WMSWarehouseInput : BasePageInput
{
    /// <summary>
    /// ProjectId
    /// </summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// WarehouseStatus
    /// </summary>
    public int WarehouseStatus { get; set; }

    /// <summary>
    /// WarehouseType
    /// </summary>
    public string? WarehouseType { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Company
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Province
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// City
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// Contractor
    /// </summary>
    public string? Contractor { get; set; }

    /// <summary>
    /// ContractorAddress
    /// </summary>
    public string? ContractorAddress { get; set; }

    /// <summary>
    /// Mobile
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Fax
    /// </summary>
    public string? Fax { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// Updator
    /// </summary>
    public string? Updator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public List<DateTime?> CreationTime { get; set; }

}

/// <summary>
/// WMSWarehouse增加输入参数
/// </summary>
public class AddWMSWarehouseInput : WMSWarehouseBaseInput
{
}

/// <summary>
/// WMSWarehouse删除输入参数
/// </summary>
public class DeleteWMSWarehouseInput : BaseIdInput
{
}

/// <summary>
/// WMSWarehouse更新输入参数
/// </summary>
public class UpdateWMSWarehouseInput : WMSWarehouseBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    [Required(ErrorMessage = "Id不能为空")]
    public long Id { get; set; }

}

/// <summary>
/// WMSWarehouse主键查询输入参数
/// </summary>
public class QueryByIdWMSWarehouseInput : DeleteWMSWarehouseInput
{

}
