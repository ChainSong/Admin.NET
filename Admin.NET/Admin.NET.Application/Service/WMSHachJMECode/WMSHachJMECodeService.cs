using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Common;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Admin.NET.Application;
/// <summary>
/// JME打印服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSHachJMECodeService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSHachJMECode> _rep;
    private readonly UserManager _userManager;
    public WMSHachJMECodeService(SqlSugarRepository<WMSHachJMECode> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询JME打印
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSHachJMECodeOutput>> Page(WMSHachJMECodeInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.JMECode), u => u.JMECode.Contains(input.JMECode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.QRCode), u => u.QRCode.Contains(input.QRCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Url), u => u.Url.Contains(input.Url.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.JMEData), u => u.JMEData.Contains(input.JMEData.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<WMSHachJMECodeOutput>()
;
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加JME打印
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSHachJMECodeInput input)
    {
        var entity = input.Adapt<WMSHachJMECode>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除JME打印
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSHachJMECodeInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //var entity = input.Adapt<JME打印>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新JME打印
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSHachJMECodeInput input)
    {
        var entity = input.Adapt<WMSHachJMECode>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取JME打印 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSHachJMECode> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取JME打印列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSHachJMECodeOutput>> List([FromQuery] WMSHachJMECodeInput input)
    {
        return await _rep.AsQueryable().Select<WMSHachJMECodeOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取JME打印列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpGet]
    [ApiDescriptionSettings(Name = "ImportExcel")]
    public async Task<Response> ImportExcel(IFormFile file)
    {
        try
        {
            Response response = new Response();
            IImporter Importer = new ExcelImporter();
            //FileDir是存储临时文件的目录，相对路径
            //private const string FileDir = "/File/ExcelTemp";
            string url = await ImprotExcel.WriteFile(file);
            //var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
            var Excelurl = ExcelData.GetFullPath(url);
            //1根据用户的角色 解析出Excel
            //IProductExcelInterface factoryExcel = ProductExcelFactory.ProductBomExcel();
            //factoryExcel._repTableColumns = _repTableColumns;
            //factoryExcel._userManager = _userManager;
            //var data = factoryExcel.Strategy(dataExcel);
            //var entityListDtos = data.Data.TableToList<AddOrUpdateProductBomInput>();
            var import = await Importer.Import<WMSHachJMECodeImportExport>(Excelurl);
            var HachJMECode = import.Data.Adapt<List<WMSHachJMECode>>();

            foreach (var item in HachJMECode)
            {
                item.Url = "https://oms.hachchina.com.cn/webapp/s.html?p=" + item.QRCode + "";
                item.CreationTime = DateTime.Now;
                item.Creator = _userManager.RealName;
            }

            //var entity = input.Adapt<WMSHachJMECode>();
            await _rep.InsertRangeAsync(HachJMECode);
            response.Code = StatusCode.Success;
            response.Msg = "导入成功";
            return response;

        }
        catch (Exception ex)
        {
            //return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = ex.Message };
            throw Oops.Oh(ex.Message);
            //throw Oops.Oh("该订单类型不支持部分转入库单");
        }
    }

    //[HttpPost]
    [ApiDescriptionSettings(Name = "ExportDemo")]
    public async Task<IActionResult> ExportDemo()
    {
        //var list = await _outboundService.GetOutboundData(input);
        IExcelExporter excelExporter = new ExcelExporter();
        var res = await excelExporter.ExportAsByteArray(new List<WMSHachJMECodeImportExport>());
        return new FileStreamResult(new MemoryStream(res), "application/octet-stream") { FileDownloadName = DateTime.Now.ToString("yyyyMMddHHmm") + "打印JME模板.xlsx" };

    }


    [HttpPost]
    [ApiDescriptionSettings(Name = "PrintJME")]
    public async Task<Response<PrintBase<List<WMSHachJMECode>>>> PrintJME(List<long> ids)
    {
        Response<PrintBase<List<WMSHachJMECode>>> response = new Response<PrintBase<List<WMSHachJMECode>>>();
        response.Data = new PrintBase<List<WMSHachJMECode>>();
        var entity = await _rep.AsQueryable().Where(u => ids.Contains(u.Id)).ToListAsync();
        foreach (var item in entity)
        {
            item.PrintNum = (item.PrintNum ?? 0) + 1;
        }
        //var entity = input.Adapt<JME打印>();
        await _rep.UpdateRangeAsync(entity);
        response.Data.Data = entity;
        response.Data.PrintTemplate = "JME打印";
        response.Code = StatusCode.Success;
        return response;

    }



}

