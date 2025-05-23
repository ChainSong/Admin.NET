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

namespace Admin.NET.Common;
public class InboundWorkFlowConst
{
    /// <summary>
    /// 入库
    /// </summary>
    public const string Workflow_Inbound = "入库";
    /// <summary>
    /// 预入库单
    /// </summary>
    public const string Workflow_ASN = "预入库单";
    /// <summary>
    /// 入库单
    /// </summary>
    public const string Workflow_Receipt = "入库单";
    /// <summary>
    /// 上架单
    /// </summary>
    public const string Workflow_ReceiptReceiving = "上架单";
    /// <summary>
    /// 转入库单
    /// </summary>
    public const string Workflow_ASNForReceipt = "转入库单";
    /// <summary>
    /// 全部转入库单
    /// </summary>
    public const string Workflow_ASNForReceiptALL = "全部转入库单";
    /// <summary>
    /// 部分转入库单
    /// </summary>
    public const string Workflow_ASNForReceiptPart = "部分转入库单";
    /// <summary>
    /// 点数
    /// </summary>
    public const string Workflow_CountQuantity = "点数";
    /// <summary>
    /// 打印RFID
    /// </summary>
    public const string Workflow_PintRFID = "打印RFID";

    public const string Workflow_CreateRFID = "创建RFID";
    /// <summary>
    /// 库存
    /// </summary>
    public const string Workflow_Inventory = "库存";

    /// <summary>
    /// 入库单回退
    /// </summary>
    public const string Workflow_ReceiptReturn = "入库单回退";

    /// <summary>
    /// 上架单回退
    /// </summary>
    public const string Workflow_ReceiptReceivingReturn = "上架单回退";
}
