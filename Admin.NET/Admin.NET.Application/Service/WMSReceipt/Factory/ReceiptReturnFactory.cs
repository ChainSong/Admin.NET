
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using Admin.NET.Common;
using Admin.NET.Core.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ReceiptReturnFactory
    {
        public static IReceiptReturnInterface ReturnReceipt(SysWorkFlow workFlow, string receiptType)
        {

            string customName = ""
;            //判断是不是有定制化的流程
            if (workFlow != null)
            {
                var customWorkFlow = workFlow.SysWorkFlowSteps.Where(p => p.StepName == InboundWorkFlowConst.Workflow_ReceiptReturn).ToList();
                if (customWorkFlow.Count > 0)
                {
                    //判断有没有子流程
                    if (!string.IsNullOrEmpty(customWorkFlow[0].Filters))
                    {
                        //将customWorkFlow[0].Filters 反序列化成List<SysWorkFlowFieldDto>
                        List<SysWorkFlowFieldDto> sysWorkFlowFieldDtos = JsonConvert.DeserializeObject<List<SysWorkFlowFieldDto>>(customWorkFlow[0].Filters);
                        customName = sysWorkFlowFieldDtos.Where(p => p.Field == receiptType).Select(p => p.Value).FirstOrDefault("");
                    }
                    else
                    {
                        customName = customWorkFlow[0].Remark;
                    }
                }

            }


            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (customName)
            {
                case "Hach":
                    return new ReceiptReturnHachStrategy();
                default:
                    return new ReceiptReturnDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }

    }
}
