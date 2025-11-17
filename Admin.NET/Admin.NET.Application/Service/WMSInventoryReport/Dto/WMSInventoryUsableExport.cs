// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSInventoryReport.Dto;
public class WMSInventoryUsableExport
{
    [ExporterHeader(displayName:"客户名称")]
    public string? CustomerName { get; set; }
    [ExporterHeader(displayName: "仓库名称")]
    public string? WarehouseName { get; set; }
    [ExporterHeader(displayName: "库区名称")]
    public string? AreaName { get; set; }
    [ExporterHeader(displayName: "库位名称")]
    public string? LocationName { get; set; }
    [ExporterHeader(displayName: "PO")]
    public string? PoCode { get; set; }
    [ExporterHeader(displayName: "SKU")]
    public string? SKU { get; set; }
    [ExporterHeader(displayName: "数量")]
    public double? Qty { get; set; }
    [ExporterHeader(displayName: "所属")]
    public string? Onwer { get; set; }
    /// <summary>
    /// 入库时间
    /// </summary>
    [ExporterHeader(displayName: "入库时间")]
    public string? CreateTime { get; set; }
    [ExporterHeader(displayName: "单位")]
    public string? Unit { get; set; }

    [ExporterHeader(displayName: "批次")]
    public string? BatchNumber { get; set; }

    [ExporterHeader(displayName: "产品类型")]
    public string? GoodsType { get; set; }
    [ExporterHeader(displayName: "库存状态")]
    public string? InventoryStatus { get; set; }

}
