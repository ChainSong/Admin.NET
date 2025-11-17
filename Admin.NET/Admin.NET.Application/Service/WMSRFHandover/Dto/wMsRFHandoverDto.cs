// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service.WMSRFAdjust.Dto;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSRFHandover.Dto;
public class wMsRFPendingHandoverResponse
{
    public string Result { get; set; }
    public string Message { get; set; }
    public string SerialNumber { get; set; }
    public long AdjustmentId { get; set; }
    public List<WMSPackage> packages { get; set; } = new List<WMSPackage>();
    public List<wMsRFPendingHandoverOrder> orders { get; set; } = new List<wMsRFPendingHandoverOrder>();
    public Response<List<OrderStatusDto>> response { get; set; } = new Response<List<OrderStatusDto>>();
}

public class wMsRFPendingHandoverOrder : WMSOrder
{

}

public class wMsRFPendingHandoverOrderInput
{
    public string OrderNumber { get; set; }
    public string ExternOrderNumber { get; set; }
}

public class wMsRFScanPackageInput
{
    public string PackageNumber { get; set; }
    public long OrderId { get; set; }
    public string ExternOrderNumber { get; set; }
    public string Type { get; set; }
    public string OpSerialNumber { get; set; }

}

public class wMsRFSubmitHandoverInput
{
    public string PackageNumber { get; set; }
    public long OrderId { get; set; }
    public string ExternOrderNumber { get; set; }
    public string Type { get; set; }
    public string OpSerialNumber { get; set; }
    public PalletInfo palletInfo { get; set; }
    public List<WMSPackage> packages { get; set; } = new List<WMSPackage>();
}
public class PalletInfo
{
    public double? Width { get; set; }
    public double? Length { get; set; }
    public double? height { get; set; }
    public double? Weight { get; set; }
    public double? Volume { get; set; }
}