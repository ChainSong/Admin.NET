

using Admin.NET.Application.ReceiptCore.Interface; 
using Admin.NET.Application.Strategy;
//using MyProject.ReceiptReceivingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.ReceiptCore.Factory
{
    public class ReceiptInventoryFactory
    {
        public static IReceiptInventoryInterface AddInventory(long CustomerId)
        {
             //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                case (long)ReceiptEnum.ReceiptExportDefault:
                    return new ReceiptInventoryDefaultStrategy();
                default:
                    return new ReceiptInventoryDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }

    }
}
