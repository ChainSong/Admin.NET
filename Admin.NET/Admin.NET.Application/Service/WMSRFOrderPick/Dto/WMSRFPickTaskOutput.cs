﻿// 麻省理工学院许可证
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
public class WMSRFPickTaskOutput
{
    /// <summary>
    /// SKU
    /// </summary>
    public string SKU { get; set; }
    /// <summary>
    /// BatchCode
    /// </summary>
    public string BatchCode { get; set; }

    /// <summary>
    /// 扫描输入
    /// </summary>
    public string ScanInput { get; set; }
    public string Lot { get; set; }
    public string SN { get; set; }
    public string ExpirationDate { get; set; }

    /// <summary>
    /// 收货单号，系统自动生成
    /// </summary>
    public string OrderNumber { get; set; }
    public string PickTaskNumber { get; set; }

    public long CustomerId { get; set; }

    /// <summary>
    /// 外部单号，由外部导入
    /// </summary>
    public string? ExternOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public double ExpectedQty { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>

    public double PickQty { get; set; } = 0;

    /// <summary>
    /// 
    /// </summary>

    public double OrderQty { get; set; } = 0;

    public double AllocationQty { get; set; } = 0;
    public double PackageQty { get; set; } = 0;


}
