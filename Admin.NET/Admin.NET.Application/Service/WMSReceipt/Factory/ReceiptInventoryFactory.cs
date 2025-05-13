
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ReceiptInventoryFactory
    {
        public static IReceiptInventoryInterface AddInventory()
        {
            return new ReceiptInvenrotyHachStrategy();
        }

    }
}
