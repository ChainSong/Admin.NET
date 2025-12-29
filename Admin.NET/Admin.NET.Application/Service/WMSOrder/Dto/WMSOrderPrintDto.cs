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

namespace Admin.NET.Application.Service;
/// <summary>
/// WMSOrder输出参数
/// </summary>
public class WMSOrderPrintDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// PreOrderId
    /// </summary>
    public long? PreOrderId { get; set; }

    /// <summary>
    /// PreOrderNumber
    /// </summary>
    public string? PreOrderNumber { get; set; }

    /// <summary>
    /// OrderNumber
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// ExternOrderNumber
    /// </summary>
    public string ExternOrderNumber { get; set; }

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
    public long? WarehouseId { get; set; }

    /// <summary>
    /// WarehouseName
    /// </summary>
    public string WarehouseName { get; set; }

    /// <summary>
    /// OrderType
    /// </summary>
    public string OrderType { get; set; }

    /// <summary>
    /// OrderStatus
    /// </summary>
    public int OrderStatus { get; set; }

    /// <summary>
    /// OrderTime
    /// </summary>
    public DateTime OrderTime { get; set; }

    /// <summary>
    /// CompleteTime
    /// </summary>
    public DateTime? CompleteTime { get; set; }

    /// <summary>
    /// DetailCount
    /// </summary>
    public double? DetailCount { get; set; }

    /// <summary>
    /// Creator
    /// </summary>
    public string? Creator { get; set; }

    /// <summary>
    /// CreationTime
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public string? Po { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public string? So { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public string? Dn { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PL { get; set; }

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


    //[Navigate(NavigateType.OneToMany, nameof(WMSOrderDetail.OrderId))]
    public List<WMSOrderDetailDto> Details { get; set; }
    public WMSWarehouse Warehouse { get; set; }
    public WMSCustomer Customer { get; set; }
    public WMSCustomerConfig CustomerConfig { get; set; }
    public WMSCustomerDetail CustomerDetail { get; set; }
    public HachWmsOutBound outBound { get; set; }

    //[Navigate(NavigateType.OneToMany, nameof(WMSOrderAllocation.OrderId))]
    //public List<WMSOrderAllocation> Allocation { get; set; }

    //[Navigate(NavigateType.OneToOne, nameof(WMSOrderAddress.PreOrderId), nameof(PreOrderId))]
    public WMSOrderAddress OrderAddress { get; set; }

    //----------------------------------打印相关字段------------------------------

    /// <summary>
    /// 打印人
    /// </summary>
    public string? PrintPerson { get; set; }

    /// <summary>
    /// 打印时间
    /// </summary>
    public string? PrintDataTime { get; set; }

    /// <summary>
    /// 图片Logo路径
    /// </summary>
    public string? ImgLogoUrl { get; set; }

}
public class WMSOrderJobPrintDto: WMSOrder
{
    public WMSCustomer Customer { get; set; } = new WMSCustomer();
    public WMSWarehouse Warehouse { get; set; } = new WMSWarehouse();
    public WMSCustomerConfig CustomerConfig { get; set; } = new WMSCustomerConfig();
    public WMSCustomerDetail CustomerDetail { get; set; } = new WMSCustomerDetail();
    public WMSOrderAddress OrderAddress { get; set; } = new WMSOrderAddress();
    public WMSOrderPrintJobPackageDto PackageInfo { get; set; } = new WMSOrderPrintJobPackageDto();
    public HachWmsOutBound outBound { get; set; }= new HachWmsOutBound();
    public WMSOrderPrintCustomerInfo wMSOrderPrintCustomerInfo { get; set; } = new WMSOrderPrintCustomerInfo();
    public List<WMSOrderPrintDetail> JobDetails { get; set; } = new List<WMSOrderPrintDetail>();

}
public class WMSOrderPrintJobPackageDto
{
    public long Id { get; set; }
    public long OId { get; set; }
    public string ExternOrderNumber { get; set; }
    public string OrderNumber { get; set; }
    public string PackageNumber { get; set; }
    public string ExpressNumber { get; set; }
    public string ExpressCompany { get; set; }
    public List<WMSOrderPrintJobPackageDetailDto> Details { get; set; }
}
public class WMSOrderPrintJobPackageDetailDto  
{
    public int Sequence { get; set; }
    public long PackageId { get; set; }
    public string PackageNumber { get; set; }
    public DateTime? CompleteTime { get; set; }
    public string PoCode { get; set; }
    public string BoxCode { get; set; }
    public string SKU { get; set; }
    public double AllocatedQty { get; set; }
    public double CombinationQty { get; set; }
    public string Onwer { get; set; }
    public double JobTotalQty { get; set; }
}


public class WMSOrderPackageDto
{
    public long TotalCount { get; set; }
    public List<WMSPackage> package { get; set; } = new List<WMSPackage>();
}
