using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSStockCheck基础输入参数
/// </summary>
public class WMSStockCheckBaseInput
{
    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// StockCheckNumber
    /// </summary>
    public virtual string? StockCheckNumber { get; set; }

    /// <summary>
    /// 外部单号
    /// </summary>
    public virtual string? ExternNumber { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public virtual long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public virtual string CustomerName { get; set; }

    /// <summary>
    /// WarehouseId
    /// </summary>
    public virtual string WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public virtual string WarehouseName { get; set; }

    /// <summary>
    /// StockCheckDate
    /// </summary>
    public virtual DateTime StockCheckDate { get; set; }

    /// <summary>
    /// StockCheckType
    /// </summary>
    public virtual string? StockCheckType { get; set; }

    /// <summary>
    /// StockCheckStatus
    /// </summary>
    public virtual int? StockCheckStatus { get; set; }

    /// <summary>
    /// ToCheckUser
    /// </summary>
    public virtual string? ToCheckUser { get; set; }

    /// <summary>
    /// ToCheckAccount
    /// </summary>
    public virtual string? ToCheckAccount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public virtual string? Remark { get; set; }

    /// <summary>
    /// 是否存在差异(默认0 1为无差异 0为存在差异)
    /// </summary>
    public virtual string? Is_Difference { get; set; }

    /// <summary>
    /// 差异是否处理（X，Y ）
    /// </summary>
    public virtual string? Is_Deal { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public virtual string Creator { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime CreationTime { get; set; }

    /// <summary>
    /// 更新人
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

    /// <summary>
    /// Int1
    /// </summary>
    public virtual int? Int1 { get; set; }

    /// <summary>
    /// Int2
    /// </summary>
    public virtual int? Int2 { get; set; }

    /// <summary>
    /// Int3
    /// </summary>
    public virtual int? Int3 { get; set; }

    /// <summary>
    /// Int4
    /// </summary>
    public virtual int? Int4 { get; set; }

    /// <summary>
    /// Int5
    /// </summary>
    public virtual int? Int5 { get; set; }

}

/// <summary>
/// WMSStockCheck分页查询输入参数
/// </summary>
public class WMSStockCheckInput : BasePageInput
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// StockCheckNumber
    /// </summary>
    public string? StockCheckNumber { get; set; }

    /// <summary>
    /// 外部单号
    /// </summary>
    public string? ExternNumber { get; set; }

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
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
    /// StockCheckDate
    /// </summary>
    public DateTime StockCheckDate { get; set; }

    /// <summary>
    /// StockCheckDate范围
    /// </summary>
    public List<DateTime?> StockCheckDateRange { get; set; }
    /// <summary>
    /// StockCheckType
    /// </summary>
    public string? StockCheckType { get; set; }

    /// <summary>
    /// StockCheckStatus
    /// </summary>
    public int? StockCheckStatus { get; set; }

    /// <summary>
    /// ToCheckUser
    /// </summary>
    public string? ToCheckUser { get; set; }

    /// <summary>
    /// ToCheckAccount
    /// </summary>
    public string? ToCheckAccount { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 是否存在差异(默认0 1为无差异 0为存在差异)
    /// </summary>
    public string? Is_Difference { get; set; }

    /// <summary>
    /// 差异是否处理（X，Y ）
    /// </summary>
    public string? Is_Deal { get; set; }

    public string? Is_Blind { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 创建时间范围
    /// </summary>
    public List<DateTime?> CreationTimeRange { get; set; }
    /// <summary>
    /// 更新人
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


    public string Area { get; set; }
    public string Location { get; set; }
    public string SKU { get; set; }
    public List<string> OrderTypeList { get; set; }

    public List<WMSStockCheckDetailDto> Details { get; set; }


}

/// <summary>
/// WMSStockCheck增加输入参数
/// </summary>
public class AddWMSStockCheckInput : WMSStockCheckBaseInput
{

}

/// <summary>
/// WMSStockCheck删除输入参数
/// </summary>
public class DeleteWMSStockCheckInput : BaseIdInput
{
}

/// <summary>
/// WMSStockCheck更新输入参数
/// </summary>
public class UpdateWMSStockCheckInput : WMSStockCheckBaseInput
{
}

/// <summary>
/// WMSStockCheck主键查询输入参数
/// </summary>
public class QueryByIdWMSStockCheckInput : DeleteWMSStockCheckInput
{

}
