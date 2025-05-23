﻿// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using Admin.NET.Common;
using Admin.NET.Core.Entity;
using Newtonsoft.Json;
using RulesEngine.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class OrderReturnFactory
{
    public static IOrderReturnInterface OrderReturn( string workFlow)
        {
        //ReceiptTypeEnum _ReceiptType = (ReceiptTypeEnum)Enum.Parse(typeof(ReceiptTypeEnum), ReceiptType, true);

        string workFlowName = ""
;            //判断是不是有定制化的流程
        //if (workFlow != null)
        //{
        //    var customWorkFlow = workFlow.SysWorkFlowSteps.Where(p => p.StepName == OutboundWorkFlowConst.Workflow_OrderReturn).ToList();
        //    if (customWorkFlow.Count > 0)
        //    {
        //        //判断有没有子流程
        //        if (!string.IsNullOrEmpty(customWorkFlow[0].Filters))
        //        {
        //            //将customWorkFlow[0].Filters 反序列化成List<SysWorkFlowFieldDto>
        //            List<SysWorkFlowFieldDto> sysWorkFlowFieldDtos = JsonConvert.DeserializeObject<List<SysWorkFlowFieldDto>>(customWorkFlow[0].Filters);
        //            workFlowName = sysWorkFlowFieldDtos.Where(p => p.Field == orderType).Select(p => p.Value).FirstOrDefault("");
        //        }
        //        else
        //        {
        //            workFlowName = customWorkFlow[0].Remark;
        //        }
        //    }

        //}

        switch (workFlowName)
        {
            case "Hach":
                return new OrderReturnHachStrategy();
            default:
                return new OrderReturnStrategy();
        }
        //return new OrderReturnStrategy();
        //    default:
        //        return new AutomatedAllocationDefaultStrategy();
        //}
    }

}