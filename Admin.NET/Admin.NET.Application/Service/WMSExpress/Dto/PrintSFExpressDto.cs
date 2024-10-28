// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSExpress.Dto;
public class PrintSFExpressDto
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

    public long PickTaskId { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public long OrderId { get; set; }

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
    /// 出库外部编号
    /// </summary> 
    public string PickTaskNumber { get; set; }



    /// <summary>
    /// 
    /// </summary>

    public string? PreOrderNumber { get; set; }


    /// <summary>
    /// 货主ID
    /// </summary> 
    public long CustomerId { get; set; }

    /// <summary>
    /// 货主名称
    /// </summary> 
    public string CustomerName { get; set; }

    /// <summary>
    /// 
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
    /// 打印时间
    /// </summary> 

    public int? PrintNum { get; set; }

    /// <summary>
    /// 打印时间
    /// </summary>

    public string? PrintPersonnel { get; set; }


    /// <summary>
    /// 打印时间
    /// </summary>

    public DateTime? PrintTime { get; set; }
    public string WaybillType { get; set; }
    //默认是子单}
    public int SumOrder { get; set; }


    /// <summary>
    /// 
    /// </summary> 

    public int PackageStatus { get; set; }

    /// <summary>
    /// 包装时间
    /// </summary> 
    public DateTime? PackageTime { get; set; }

    /// <summary>
    /// 明细数量
    /// </summary>

    public double? DetailCount { get; set; }

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

    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public string? Updator { get; set; }

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

    /// <summary>
    /// 
    /// </summary>
    public int? Int4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Int5 { get; set; }


    public List<WMSPackageDetail> Details { get; set; }
}
