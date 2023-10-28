
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public static class ReceiptReceivingExcelFactory
    {
        public static IReceiptReceivingExcelInterface GetReceipt()
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);

            return new ReceiptReceivingExcelDefaultStrategy();
            //return new ASNDefaultStrategy();
        }
    }
}
