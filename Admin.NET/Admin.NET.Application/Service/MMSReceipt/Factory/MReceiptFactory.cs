
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class MReceiptFactory
    {
        public static IMReceiptInterface AddOrUpdate(long SupplierId)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            switch (SupplierId)
            {
                //case (long)ReceiptEnum.ReceiptExportDefault:
                //    return new MReceiptDefaultStrategy();
                default:
                    return new MReceiptDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }

    }
}
