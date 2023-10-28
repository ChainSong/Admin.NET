
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ASNExcelFactory
    {

        //private     ISqlSugarClient _db { get; set; }

        public static IASNExcelInterface ASNExcel()
        {
            return new ASNExcelDefaultStrategy();
            //return new ASNDefaultStrategy();
        }
    }
}
