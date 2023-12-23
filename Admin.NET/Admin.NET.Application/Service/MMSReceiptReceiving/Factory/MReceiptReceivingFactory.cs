

using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Admin.NET.Applicationt.Factory
{
    public static class MReceiptReceivingFactory
    {
        public static IMReceiptReceivingInterface GetReceiptReceiving(long CustomerId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                default:
                    return new MReceiptReceivingDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
    }
}
