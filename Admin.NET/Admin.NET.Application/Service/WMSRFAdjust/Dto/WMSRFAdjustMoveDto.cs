// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSRFAdjust.Dto;
public class WMSRFAdjustMoveInput
{
    /// <summary>
    /// 客户Id
    /// </summary>
    public long? CustomerId { get; set; }
    /// <summary>
    /// 仓库Id
    /// </summary>
    public long? WarehouseId { get; set; }
    /// <summary>
    /// 扫描的数据
    /// </summary>
    public string ScanValue { get; set; }
    /// <summary>
    /// 操作序列号
    /// </summary>
    public string OpSerialNumber { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public string Type { get; set; }
}
public class WMSAddAdjustRFMoveInput : WMSRFAdjustMoveInput
{
    public WMSRFAdjustMove detail { get; set; }
}

public class WMSRFAdjustMove
{
    /// <summary>
    /// 扫描的数据
    /// </summary>
    public string FromLocation { get; set; }
    public string SKU { get; set; }
    public long? Qty { get; set; }
    public string ToLocation { get; set; }
}

public class WMSRFAdjustMoveResponse
{
    public string Result { get; set; }
    public string Message { get; set; }
    public string SerialNumber { get; set; }
    public long AdjustmentId { get; set; }
    public List<WMSRFAdjustMove> outputs { get; set; } = new List<WMSRFAdjustMove>();
    public Response<List<OrderStatusDto>> response { get; set; } = new Response<List<OrderStatusDto>>();
}

public class WMSRFAdjustAddResponse : OrderStatusDto
{
    public long AdjustmentId { get; set; }
}

public class WMSRFAdjustMoveCompleteInput
{
    public long Id { get; set; }
    /// <summary>
    /// 客户Id
    /// </summary>
    public long? CustomerId { get; set; }
    /// <summary>
    /// 仓库Id
    /// </summary>
    public long? WarehouseId { get; set; }
    /// <summary>
    /// 扫描的数据
    /// </summary>
    public string ScanValue { get; set; }
    /// <summary>
    /// 操作序列号
    /// </summary>
    public string OpSerialNumber { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public string Type { get; set; }
}