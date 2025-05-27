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

namespace Admin.NET.Application.Service.WMSInventoryReport.Dto;
public class WMSInventoryUsableReport
{
    //public long Id { get; set; }
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
    /// Area
    /// </summary>
    public string Area { get; set; }

    /// <summary>
    /// Location
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public string SKU { get; set; }

    /// <summary>
    /// UPC
    /// </summary>
    public string UPC { get; set; }

    /// <summary>
    /// GoodsType
    /// </summary>
    public string GoodsType { get; set; }

    /// <summary>
    /// InventoryStatus
    /// </summary>
    public int InventoryStatus { get; set; }

    /// <summary>
    /// SuperId
    /// </summary>
    public long SuperId { get; set; }

    /// <summary>
    /// RelatedId
    /// </summary>
    public long RelatedId { get; set; }

    /// <summary>
    /// GoodsName
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// UnitCode
    /// </summary>
    public string UnitCode { get; set; }

    /// <summary>
    /// Onwer
    /// </summary>
    public string Onwer { get; set; }

    /// <summary>
    /// BoxCode
    /// </summary>
    public string BoxCode { get; set; }

    /// <summary>
    /// TrayCode
    /// </summary>
    public string TrayCode { get; set; }

    /// <summary>
    /// BatchCode
    /// </summary>
    public string BatchCode { get; set; }




    public string LotCode { get; set; }

    public string PoCode { get; set; }


    public double Weight { get; set; }

    public double Volume { get; set; }

    /// <summary>
    /// Qty
    /// </summary>
    public double Qty { get; set; }

    /// <summary>
    /// ProductionDate
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// ExpirationDate
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string Remark { get; set; }
    public DateTime? InventoryTime { get; set; }

    
}
