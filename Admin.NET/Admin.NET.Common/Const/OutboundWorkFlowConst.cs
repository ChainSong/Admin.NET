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

namespace Admin.NET.Common;
public class OutboundWorkFlowConst
{
    public const string Workflow_Outbound= "出库";
    public const string Workflow_PreOrder= "预出库单";
    public const string Workflow_PreOrder_Add = "新增预出库单";
    public const string Workflow_PreOrder_Add_Excel = "新增预出库单";
    public const string Workflow_PreOrder_Update = "修改预出库单";
    public const string Workflow_Order = "出库单"; 
    public const string Workflow_Print_Order = "打印发运单";
    public const string Workflow_Print_Job_Order = "JOB汇总清单";
    public const string Workflow_Pick = "拣货单";
    public const string Workflow_Print_Pick = "打印拣货单";
    public const string Workflow_PickTemplate = "拣货单模板";
    public const string Workflow_Complete = "完成出库";
    public const string Workflow_Package = "包装单";
    public const string Workflow_PreOrder_ForOrder = "转出库单";
    public const string Workflow_PreOrder_ForOrder_ALL = "全部转出库单";
    public const string Workflow_PreOrder_ForOrder_Part = "部分转出库单";
    public const string Workflow_Automated_Outbound = "自动分配";
    public const string Workflow_Order_Return = "出库单回退";
    public const string Workflow_Print_Package_List = "出库打印装箱清单";
    public const string Workflow_Print_DG_Package_List = "危险品仓出库打印装箱清单";
    //public const string Workflow_ReceiptReceiving = "上架单";
}
