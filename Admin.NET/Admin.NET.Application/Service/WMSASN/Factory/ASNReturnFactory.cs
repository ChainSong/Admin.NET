
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Admin.NET.Application.Factory
//{
//    public class ASNReturnFactory
//    {
//        public static IASNReturnInterface ASNReturn(IRepository<WMS_ASN, long> wms_asnRepository,
//            IRepository<WMS_ASNDetail, long> wms_asndetailRepository, IWarehouseUserMappingManager warehouseusermappingManager,
//            ICustomerUserMappingManager customerusermappingManager,long CustomerId)
//        {
//            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
//            switch (CustomerId)
//            {
//                case (long)ASNExcelEnum.ASNExportDefault:
//                    return new ASNReturnDefaultStrategy(wms_asnRepository, wms_asndetailRepository, warehouseusermappingManager, customerusermappingManager);
//                default:
//                    return new ASNReturnDefaultStrategy(wms_asnRepository, wms_asndetailRepository, warehouseusermappingManager, customerusermappingManager);
//            }
//            //return new ASNDefaultStrategy();
//        }

//        //public CreateOrUpdateWMS_ASNInput _createOrUpdateWMS { get; set; }

//        //public BaseASNData(CreateOrUpdateWMS_ASNInput createOrUpdateWMS)
//        //{
//        //    this._createOrUpdateWMS = createOrUpdateWMS;
//        //}

//        //public virtual void CustomerDefinedSettledTransData(ref string message)
//        //{

//        //}

//        //public Response<CreateOrUpdateWMS_ASNInput> Hub(string className)
//        //{

//        //}
//    }
//}
