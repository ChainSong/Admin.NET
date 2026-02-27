// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application;
public class WMSOrderDetailDto
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long? TenantId { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }



    /// <summary>
    /// 
    /// </summary>
    public long PreOrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long PreOrderDetailId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string PreOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExternOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? LineNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SKU { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? UPC { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? GoodsType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double OrderQty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double AllocatedQty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? BoxCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? TrayCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? BatchCode { get; set; }



    /// <summary>
    /// 
    /// </summary>
    public string? LotCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PoCode { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public double? Weight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double? Volume { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public string? UnitCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Onwer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

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
    public string? Updator { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? UpdateTime { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public string? Remark { get; set; }

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
    public string? Str6 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str7 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str8 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str9 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str10 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str11 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str12 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str13 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str14 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str15 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str16 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str17 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str18 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str19 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Str20 { get; set; }

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
    public DateTime? DateTime4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? DateTime5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Int2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Int3 { get; set; }
    public int? Int4 { get; set; }
    public int? Int5 { get; set; }
    public int Sequence { get; set; }

    /// <summary>
    /// 箱号
    /// </summary>
    public string? BoxNumber { get; set; }

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompleteTime { get; set; }
    /// <summary>
    /// Job总箱数
    /// </summary>
    public long? JobTotalQty { get; set; }
    /// <summary>
    /// 组合数量
    /// </summary>
    public long? CombinationQty { get; set; }
}
