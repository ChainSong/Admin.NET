
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ReceiptReturnFactory
    {
        public static IReceiptReturnInterface ReturnReceipt(SysWorkFlow workFlow, string ReceiptType)
        {
            string customName = ""
;            //判断是不是有定制化的流程
            if (workFlow != null)
            {
                var customWorkFlow = workFlow.SysWorkFlowSteps.Where(p => p.StepName == InboundWorkFlowConst.Workflow_ReceiptReturn).ToList();
                if (customWorkFlow.Count > 0)
                {
                    customName = customWorkFlow[0].Remark;
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
