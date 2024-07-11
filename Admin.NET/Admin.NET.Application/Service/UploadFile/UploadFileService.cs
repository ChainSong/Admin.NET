//using Admin.NET.Application.Const;
//using Admin.NET.Application.Factory;
//using Admin.NET.Application.Interface;
//using Admin.NET.Common;
//using Admin.NET.Common.ExcelCommon;
//using Admin.NET.Core;
//using Admin.NET.Core.Entity;
//using Furion.DependencyInjection;
//using Furion.FriendlyException;
//using Magicodes.ExporterAndImporter.Core;
//using Magicodes.ExporterAndImporter.Excel;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;

//namespace Admin.NET.Application;
///// <summary>
///// 仓库用户关系服务
///// </summary>
//[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
//public class UploadFileService : IDynamicApiController, ITransient
//{
//    private readonly SqlSugarRepository<WarehouseUserMapping> _rep;
//    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;
//    private readonly UserManager _userManager;
//    public UploadFileService(SqlSugarRepository<WarehouseUserMapping> rep, SqlSugarRepository<WMSWarehouse> repWarehouse, UserManager userManager)
//    {
//        _rep = rep;
//        _repWarehouse = repWarehouse;
//        _userManager = userManager;
//    }


//    //[HttpPost] 
//    //public ActionResult ImageUpload(IFormFile file)
//    //{


//    //    //FileDir是存储临时文件的目录，相对路径
//    //    //private const string FileDir = "/File/ExcelTemp";
//    //    string url = await ImageUploadUtils.WriteFile(file,);
//    //    var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
//    //    //使用简单工厂定制化  /
//    //    //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
//    //    //默认：1，按照已有库存，且库存最小推荐
//    //    //默认：2，没有库存，以前有库存
//    //    //默认：3，随便推荐
//    //    IReceiptReceivingExportInterface factory = ReceiptReceivingExportFactory.GetReceiptReceiving();

      
//    //    //factory._repTableColumns = _repTableInventoryUsed;
//    //    var response = factory.Strategy(input);
//    //    IExporter exporter = new ExcelExporter();
//    //    var result = exporter.ExportAsByteArray<DataTable>(response.Data);
//    //    var fs = new MemoryStream(result.Result);
//    //    //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
//    //    return new FileStreamResult(fs, "application/octet-stream")
//    //    {
//    //        FileDownloadName = "上架单.xlsx" // 配置文件下载显示名
//    //    };
//    //}


//}

