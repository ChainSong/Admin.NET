// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSReport.Dto;
public class WMSRFPackageAcquisitionInput: BasePageInput
{
    public virtual long Id { get; set; }
    public long OrderId { get; set; }
    public long PickTaskId { get; set; }
    public string PickTaskNumber { get; set; }
    public string? PreOrderNumber { get; set; }
    public string? OrderNumber { get; set; }
    public string? ExternOrderNumber { get; set; }
    public string? PackageNumber { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }
    public long? WarehouseId { get; set; }
    public string? WarehouseName { get; set; }
    public string? Type { get; set; }
    public string SKU { get; set; }
    public string Lot { get; set; }
    public string? SN { get; set; }
     
}


public class WMSRFPackageAcquisitionExport
{
    [ExporterHeader(displayName: "拣货任务号")]
    public string PickTaskNumber { get; set; }
    [ExporterHeader(displayName: "预出库单号")]
    public string? PreOrderNumber { get; set; }
    [ExporterHeader(displayName: "出库单号")]
    public string? OrderNumber { get; set; }
    [ExporterHeader(displayName: "外部单号")]
    public string? ExternOrderNumber { get; set; }
    [ExporterHeader(displayName: "包装单号")]
    public string? PackageNumber { get; set; }
    [ExporterHeader(displayName: "客户名称")]
    public string CustomerName { get; set; }
    [ExporterHeader(displayName: "仓库名称")]
    public string? WarehouseName { get; set; }
    [ExporterHeader(displayName: "类型")]
    public string? Type { get; set; }
    [ExporterHeader(displayName: "SKU")]
    public string SKU { get; set; }
    [ExporterHeader(displayName: "Lot")]
    public string Lot { get; set; }
    [ExporterHeader(displayName: "SN码")]
    public string? SN { get; set; }
    [ExporterHeader(displayName: "数量")]
    public double? Qty { get; set; }
    [ExporterHeader(displayName: "生产日期")]
    public DateTime? ProductionDate { get; set; }
    [ExporterHeader(displayName: "过期日期")]
    public DateTime? ExpirationDate { get; set; }
    [ExporterHeader(displayName: "创建用户")]
    public string Creator { get; set; }
    [ExporterHeader(displayName: "创建时间")]
    public DateTime CreationTime { get; set; }
    [ExporterHeader(displayName: "更新用户")]
    public string? Updator { get; set; }
    [ExporterHeader(displayName: "更新时间")]
    public DateTime? UpdateTime { get; set; }
}