

using Admin.NET.Application.Interface;
using Admin.NET.Application.ReceiptCore.Interface;
using Admin.NET.Application.Strategy;
using Admin.NET.ApplicationCore.Strategy;

//using MyProject.ReceiptReceivingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.ReceiptCore.Factory
{
    public class ReceiptReceivingInventoryFactory
    {
        //public static IReceiptReceivingInventoryInterface AddInventory(long CustomerId)
        //{
        //     //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
        //    switch (CustomerId)
        //    {
        //        //case (long)ReceiptEnum.ReceiptExportDefault:
        //        //    return new ReceiptInventoryDefaultStrategy();
        //        default:
        //            return new ReceiptInventoryDefaultStrategy();
        //    }
        //    //return new ASNDefaultStrategy();
        //}

        public static IReceiptReceivingInventoryInterface AddInventory(string workFlow)
        {

            switch (workFlow)
            {
                case "HachDG":
                    return new ReceiptInventoryHachDGStrategy();
                default:
                    return new ReceiptInventoryDefaultStrategy();
            }
        }

    }
}
