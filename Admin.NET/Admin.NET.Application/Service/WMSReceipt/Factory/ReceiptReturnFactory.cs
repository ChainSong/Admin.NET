
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ReceiptReturnFactory
    {
        public static IReceiptReturnInterface ReturnReceipt(long CustomerId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                case (long)ReceiptEnum.ReceiptExportDefault:
                    return new ReceiptReturnDefaultStrategy();
                default:
                    return new ReceiptReturnDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }

    }
}
