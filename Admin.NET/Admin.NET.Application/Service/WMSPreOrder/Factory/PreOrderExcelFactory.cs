
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class PreOrderExcelFactory
    {
        public static IPreOrderExcelInterface GePreOrder()
        {

            return new PreOrderExcelDefaultStrategy();

            //return new ASNDefaultStrategy();
        }



    }
}
