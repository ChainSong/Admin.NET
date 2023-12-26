
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Strategy;
using Admin.NET.Application.Strategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Factory
{
    public static class MReceiptExcelFactory
    {
        /// <summary>
        /// 导出入库单
        /// </summary>
        /// <returns></returns>
        public static IMReceiptExcelInterface Export()
        {
            //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);

            return new MReceiptExportDefaultStrategy();
            //return new ASNDefaultStrategy();
        }

        /// <summary>
        /// 导出上架模板
        /// </summary>
        /// <returns></returns>
        //public static IReceiptReceivingExcelInterface ExportReceiptReceiving()
        //{
        //    //string RoleName = Enum.GetName(typeof(ReceiptEnum), ReceiptEnum.ReceiptExportDefault);

        //    return new ReceiptReceivingExcelDefaultStrategy();
        //    //return new ASNDefaultStrategy();
        //}
    }
}
