
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
    public class ASNForReceiptFactory
    {
        public static IASNForReceiptInterface ASNForReceipt(long CustomerId, string ReceiptType)
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);
            //Enum.TryParse(typeof(ReceiptTypeEnum), ReceiptType, out object _ReceiptType);
            ReceiptTypeEnum _ReceiptType = (ReceiptTypeEnum)Enum.Parse(typeof(ReceiptTypeEnum), ReceiptType, true);
            switch (_ReceiptType)
            {
                case ReceiptTypeEnum.虚拟入库:
                    return new ASNForReceiptDefaultStrategy();
                case ReceiptTypeEnum.收货入库:
                    return new ASNForReceiptDefaultStrategy();
                case ReceiptTypeEnum.通用入库:
                    return new ASNForReceiptDefaultStrategy();
                case ReceiptTypeEnum.其它入库:
                    return new ASNForReceiptHachStrategy();
                case ReceiptTypeEnum.采购入库:
                    return new ASNForReceiptDefaultStrategy();
                default:
                    return new ASNForReceiptDefaultStrategy();
            }
            //return new ASNDefaultStrategy();
        }
    }
}
