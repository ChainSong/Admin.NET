
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application
{
    public class ImportExcelTemplateRFIDHach : ImportExcelTemplateStrategy
    {


        //public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
        //public UserManager _userManager { get; set; }
        static List<string> _tableNames = new List<string>() { "RFID" };


        public ImportExcelTemplateRFIDHach(
            //ITable_ColumnsManager table_ColumnsManager,
            //      ITable_ColumnsDetailManager table_ColumnsDetailManager
            ) : base(_tableNames)
        {
            //_table_ColumnsManager = table_ColumnsManager;
            //_table_ColumnsDetailManager = table_ColumnsDetailManager;
        }


        public override Response<byte[]> Strategy(long CustomerId, long TenantId)
        {
            Response<byte[]> response = new Response<byte[]>();

            WMSRFIDImportExport wMS_RFIDImportExport = new WMSRFIDImportExport();
            IExporter exporter = new ExcelExporter();
            var result = exporter.ExportAsByteArray(new List<WMSRFIDImportExport>());

            response.Code = StatusCode.Success;
            response.Data = result.Result;
            response.Msg = "导出成功";
            return response;

        }
    }
}
