
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
    public class ASNForReceiptFactory
    {
        public static IASNForReceiptInterface ASNForReceipt(long CustomerId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                case (long)ASNEnum.ASNForReceiptDefault:
                    return new ASNForReceiptDefaultStrategy();
                default:
                    return new ASNForReceiptDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
    }
}
