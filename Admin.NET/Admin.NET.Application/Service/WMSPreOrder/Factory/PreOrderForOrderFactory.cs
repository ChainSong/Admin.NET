
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.ApplicationCore.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class PreOrderForOrderFactory
    {
        public static IPreOrderForOrderInterface PreOrderForOrder(long CustomerId)
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (CustomerId)
            {
                case (long)OutboundEnum.OutboundDefault:
                    return new PreOrderForOrderDefaultStrategy();
                default:
                    return new PreOrderForOrderDefaultStrategy();
            }
        }

    }
}
