//using Abp.Domain.Repositories;
//using MyProject.ASNCore;
//using MyProject.ASNCore.DomainService;
//using MyProject.InventoryCore;
//using MyProject.ReceiptCore.DomainService;
//using MyProject.ReceiptCore.Enumerate;
//using MyProject.ReceiptCore.Interface;
//using MyProject.ReceiptCore.Strategy;
//using MyProject.ReceiptReceivingCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Admin.NET.Application.Factory
//{
//    public class ReceiptReturnFactory
//    {
//        public static IReceiptReturnInterface ReturnReceipt(
//            IRepository<WMS_ASN, long> wms_asnRepository,
//            IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
//            IRepository<WMS_Receipt, long> wms_receiptRepository,
//            IRepository<WMS_ReceiptDetail, long> wms_receiptdetailRepository,long CustomerId)
//        {
//            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
//            switch (CustomerId)
//            {
//                case (long)ReceiptEnum.ReceiptExportDefault:
//                    return new ReceiptReturnDefaultStrategy(wms_asnRepository, wms_asndetailRepository,wms_receiptRepository, wms_receiptdetailRepository);
//                default:
//                    return new ReceiptReturnDefaultStrategy(wms_asnRepository, wms_asndetailRepository, wms_receiptRepository, wms_receiptdetailRepository);
//            }
//            //return new ASNDefaultStrategy();
//        }

//    }
//}
