
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Admin.NET.Applicationt.ReceiptReceivingCore.Factory
{
    public static class ReceiptReceivingFactory
    {
        public static IReceiptReceivingInterface GetReceiptReceiving(long CustomerId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                default:
                    return new ReceiptReceivingDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
    }
}
