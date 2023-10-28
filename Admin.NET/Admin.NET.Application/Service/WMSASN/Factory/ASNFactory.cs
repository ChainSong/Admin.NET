
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Strategy;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public class ASNFactory
    {
        //public static ISqlSugarClient _db { get; set; }
        //public static UserManager _userManager { get; set; }
        public static IASNInterface AddOrUpdate(long CustomerId)
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (CustomerId)
            {
                case (long)ASNExcelEnum.ASNExportDefault:
                    //ASNDefaultStrategy aSNDefaultStrategy = new ASNDefaultStrategy();
                    //aSNDefaultStrategy._db = _db;
                    //aSNDefaultStrategy._userManager = _userManager;
                    return  new ASNAddOrUpdateDefaultStrategy();
                default:
                    return new ASNAddOrUpdateDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
        //public static IASNInterface UpdateASN(long CustomerId)
        //{
        //    //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
        //    switch (CustomerId)
        //    {
        //        case (long)ASNExcelEnum.ASNExportDefault:
        //            //ASNDefaultStrategy aSNDefaultStrategy = new ASNDefaultStrategy();
        //            //aSNDefaultStrategy._db = _db;
        //            //aSNDefaultStrategy._userManager = _userManager;
        //            return new ASNAddDefaultStrategy();
        //        default:
        //            return new ASNAddDefaultStrategy();
        //    }
        //    //return new ASNDefaultStrategy();
        //}
        //public CreateOrUpdateWMS_ASNInput _createOrUpdateWMS { get; set; }

        //public BaseASNData(CreateOrUpdateWMS_ASNInput createOrUpdateWMS)
        //{
        //    this._createOrUpdateWMS = createOrUpdateWMS;
        //}

        //public virtual void CustomerDefinedSettledTransData(ref string message)
        //{

        //}

        //public Response<CreateOrUpdateWMS_ASNInput> Hub(string className)
        //{

        //}
    }
}
