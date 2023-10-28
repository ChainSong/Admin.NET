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
//    public class ReceiptInventoryFactory
//    {
//        public static IReceiptInventoryInterface AddInventory(IRepository<WMS_Receipt, long> wms_receiptRepository,
//               IRepository<WMS_ReceiptReceiving, long> wms_receiptreceivingRepository,
//                 IRepository<WMS_ReceiptDetail, long> wms_receiptdetailRepository,
//                 IRepository<WMS_Inventory_Usable, long> wms_inventory_usableRepository,
//                 IRepository<WMS_ASN, long> wms_asnRepository, IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
//                 long CustomerId)
//        {
//             //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
//            switch (CustomerId)
//            {
//                case (long)ReceiptEnum.ReceiptExportDefault:
//                    return new ReceiptInventoryDefaultStrategy(wms_asnRepository, wms_asndetailRepository,wms_receiptRepository, wms_receiptdetailRepository, wms_receiptreceivingRepository, wms_inventory_usableRepository);
//                default:
//                    return new ReceiptInventoryDefaultStrategy(wms_asnRepository, wms_asndetailRepository,wms_receiptRepository, wms_receiptdetailRepository, wms_receiptreceivingRepository, wms_inventory_usableRepository);
//            }
//            //return new ASNDefaultStrategy();
//        }

//    }
//}
