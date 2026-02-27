// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;

namespace Admin.NET.Application;
/// <summary>
/// RF拣货包装输入参数
/// </summary>
public class RFPickAndPackageInput
{
    /// <summary>
    /// 拣货任务ID
    /// </summary>
    public long Id { get; set; }

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

    /// <summary>
    /// 拣货任务号
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNumber { get; set; }

    /// <summary>
    /// 外部订单号
    /// </summary>
    public string ExternOrderNumber { get; set; }

    /// <summary>
    /// 扫描输入
    /// </summary>
    public string ScanInput { get; set; }

    /// <summary>
    /// SKU
    /// </summary>
    public string SKU { get; set; }

    /// <summary>
    /// 批次号
    /// </summary>
    public string Lot { get; set; }

    /// <summary>
    /// 有效期
    /// </summary>
    public string ExpirationDate { get; set; }

    /// <summary>
    /// 库位
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// 箱号
    /// </summary>
    public string BoxNumber { get; set; }

    /// <summary>
    /// 操作类型：Pick=拣货, Package=包装
    /// </summary>
    public string OperationType { get; set; }
}
