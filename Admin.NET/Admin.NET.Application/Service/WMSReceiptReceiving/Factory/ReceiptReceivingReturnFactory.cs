
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using MyProAdmin.NET.Applicationject.ReceiptReceivingCore.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Admin.NET.Application.ReceiptReceivingCore.Factory
{
    public static class ReceiptReceivingReturnFactory
    {
        public static IReceiptReceivingReturnInterface ReturnReceiptReceiving(long CustomerId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                default:
                    return new ReceiptReceivingReturnDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
    }
}
