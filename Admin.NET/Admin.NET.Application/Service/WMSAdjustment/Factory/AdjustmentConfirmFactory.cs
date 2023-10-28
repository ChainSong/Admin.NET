
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
    public class AdjustmentConfirmFactory
    {
        //public static ISqlSugarClient _db { get; set; }
        //public static UserManager _userManager { get; set; }
        public static IAdjustmentConfirmInterface Confirm(long CustomerId, string adjustmentType)
        {
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (CustomerId, Enum.Parse(typeof(AdjustmentTypeEnum), adjustmentType))
            {
                case (0, AdjustmentTypeEnum.库存冻结):
                    return new AdjustmentConfirmFrozenDefaultStrategy();
                case (0, AdjustmentTypeEnum.库存品级):
                    return new AdjustmentConfirmGoodsTypeDefaultStrategy();
                case (0, AdjustmentTypeEnum.库存数量):
                    return new AdjustmentConfirmQuantityDefaultStrategy();
                case (0, AdjustmentTypeEnum.库存解冻):
                    return new AdjustmentConfirmUnfreezeDefaultStrategy();
                case (0, AdjustmentTypeEnum.库存移动):
                    return new AdjustmentConfirmMoveDefaultStrategy();
                default:
                    switch (Enum.Parse(typeof(AdjustmentTypeEnum), adjustmentType))
                    {
                        case (AdjustmentTypeEnum.库存冻结):
                            return new AdjustmentConfirmFrozenDefaultStrategy();
                        case (AdjustmentTypeEnum.库存品级):
                            return new AdjustmentConfirmGoodsTypeDefaultStrategy();
                        case (AdjustmentTypeEnum.库存数量):
                            return new AdjustmentConfirmQuantityDefaultStrategy();
                        case (AdjustmentTypeEnum.库存解冻):
                            return new AdjustmentConfirmUnfreezeDefaultStrategy();
                        case (AdjustmentTypeEnum.库存移动):
                            return new AdjustmentConfirmMoveDefaultStrategy();
                    }
                    return new AdjustmentConfirmDefaultStrategy();
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
