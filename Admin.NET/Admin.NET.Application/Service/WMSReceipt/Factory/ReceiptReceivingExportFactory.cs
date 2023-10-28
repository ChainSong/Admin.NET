
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ReceiptReceivingExportFactory
    {
        public static IReceiptReceivingExportInterface GetReceiptReceiving(
                  )
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);

            return new ReceiptReceivingExportDefaultStrategy();

            //return new ASNDefaultStrategy();
        }

    }
}
