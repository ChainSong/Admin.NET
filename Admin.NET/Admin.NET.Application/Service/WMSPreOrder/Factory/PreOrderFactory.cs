
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
   public  class PreOrderFactory
    {
        public static IPreOrderInterface AddOrUpdate(long CustomerId)
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (CustomerId)
            {
                case (long)PreOrderEnum.PreOrderExportDefault:
                    return new PreOrderDefaultStrategy();
                default:
                    return new PreOrderDefaultStrategy();
            }
        }

    }
}
