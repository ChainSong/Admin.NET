//using Abp.Domain.Repositories;
//using MyProject.ASNCore;
//using MyProject.ASNCore.DomainService;
//using MyProject.InventoryCore;
//using MyProject.ReceiptCore.DomainService;
//using MyProject.ReceiptCore.Enumerate;
//using MyProject.ReceiptCore.Interface;
//using MyProject.ReceiptCore.Strategy;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyProject.ReceiptCore.Factory
//{
//    public class ReceiptFactory
//    {
//        public static IReceiptInterface ASNForReceipt(IRepository<WMS_ASN, long> wms_asnRepository, IRepository<WMS_ASNDetail, long> wms_asndetailRepository, IRepository<WMS_Receipt, long> wms_receiptRepository, long CustomerId)
//        {
//            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
//            switch (CustomerId)
//            {
//                case (long)ReceiptEnum.ReceiptExportDefault:
//                    return new ReceiptDefaultStrategy(wms_asnRepository, wms_asndetailRepository, wms_receiptRepository);
//                default:
//                    return new ReceiptDefaultStrategy(wms_asnRepository, wms_asndetailRepository, wms_receiptRepository);
//            }
//            //return new ASNDefaultStrategy();
//        }

//    }
//}
