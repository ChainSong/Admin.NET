

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
    public class MReceiptInventoryFactory
    {
        public static IMReceiptInventoryInterface AddInventory(long CustomerId)
        {
             //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (CustomerId)
            {
                case (long)ReceiptEnum.ReceiptExportDefault:
                    return new MReceiptInventoryDefaultStrategy();
                default:
                    return new MReceiptInventoryDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }

    }
}
