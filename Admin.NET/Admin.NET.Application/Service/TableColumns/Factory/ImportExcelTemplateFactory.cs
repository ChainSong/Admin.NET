
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application
{
    public class ImportExcelTemplateFactory
    {

        public static IImportExcelTemplateInterface ImportExcelTemplate(long CustomerId, string TableName)
        {
            
            //string aaa = Enum.GetName(typeof(ASNEnum), ASNEnum.ASNExportDefault);
            switch (TableName)
            {
                case "WMS_ASN":
                    return new ImportExcelTemplateASNDefault();
                case "WMS_PreOrder":

                    return new ImportExcelTemplatePreOrderDefault();

                case "WMS_Adjustment":
                    return new ImportExcelTemplateAdjustmentDefault();

                case "WMS_Product":
                    return new ImportExcelTemplateProductDefault();

                case "WMS_ProductBom":
                    return new ImportExcelTemplateProductBomDefault();

                default:
                    return new ImportExcelTemplateASNDefault();
            }
        }
    }
}
