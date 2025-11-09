
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class PrintOrderFactory
    {
        public static IPrintOrderInterface PrintOrder(string workflow)
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (workflow)
            {
                //case (long)OutboundEnum.OutboundDefault:
                //    return new PrintOrderStrategy();
                default:
                    return new PrintOrderStrategy();
            }
        }
    }
}
