// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要版本的软件中必须包括上述版权声明和本许可声明。
//
// 软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// RF单次拣货记录
/// </summary>
public class RFSinglePickRecord
{
    /// <summary>
    /// 记录ID（GUID）
    /// </summary>
    public string RecordId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// 拣货任务ID
    /// </summary>
    public long PickTaskId { get; set; }

    /// <summary>
    /// 拣货任务号
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 外部订单号
    /// </summary>
    public string ExternOrderNumber { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public string SKU { get; set; }

    /// <summary>
    /// UPC
    /// </summary>
    public string UPC { get; set; }

    /// <summary>
    /// 货品名称
    /// </summary>
    public string GoodsName { get; set; }

    /// <summary>
    /// 货品类型
    /// </summary>
    public string GoodsType { get; set; }

    /// <summary>
    /// 单位编码
    /// </summary>
    public string UnitCode { get; set; }

    /// <summary>
    /// 所有者
    /// </summary>
    public string Onwer { get; set; }

    /// <summary>
    /// 箱编码
    /// </summary>
    public string BoxCode { get; set; }

    /// <summary>
    /// 托盘编码
    /// </summary>
    public string TrayCode { get; set; }

    /// <summary>
    /// 批次编码
    /// </summary>
    public string BatchCode { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string LotCode { get; set; }

    /// <summary>
    /// PO编码
    /// </summary>
    public string PoCode { get; set; }

    /// <summary>
    /// 应拣数量
    /// </summary>
    public double Qty { get; set; }

    /// <summary>
    /// 本次拣货数量（每次扫描都是1）
    /// </summary>
    public double PickQty { get; set; } = 1;

    /// <summary>
    /// 库位
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// 区域
    /// </summary>
    public string Area { get; set; }

    /// <summary>
    /// 生产日期
    /// </summary>
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 有效期
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// 拣货时间
    /// </summary>
    public DateTime PickTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 拣货人
    /// </summary>
    public string PickPersonnel { get; set; }

    /// <summary>
    /// 拣货箱号
    /// </summary>
    public string PickBoxNumber { get; set; }

    /// <summary>
    /// 是否已包装
    /// </summary>
    public bool IsPackaged { get; set; } = false;

    /// <summary>
    /// 客户ID
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// 仓库ID
    /// </summary>
    public long WarehouseId { get; set; }

    /// <summary>
    /// 仓库名称
    /// </summary>
    public string WarehouseName { get; set; }
}
