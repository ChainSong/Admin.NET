
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class AutomatedAllocationFactory
    {
        public static IAutomatedAllocationInterface AutomatedAllocation()
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            //switch (CustomerId)
            //{
            //    case (long)OutboundEnum.OutboundDefault:
                    return new AutomatedAllocationDefaultStrategy();
            //    default:
            //        return new AutomatedAllocationDefaultStrategy();
            //}
        }
    }
}
