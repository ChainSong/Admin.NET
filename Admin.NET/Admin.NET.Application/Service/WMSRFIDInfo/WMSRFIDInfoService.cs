using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Common.Utility;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ServiceStack.Text;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSRFIDInfo服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFIDInfoService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSRFIDInfo> _rep;
    private readonly UserManager _userManager;

    public WMSRFIDInfoService(SqlSugarRepository<WMSRFIDInfo> rep, UserManager userManager)
    {
        _rep = rep;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFIDInfoOutput>> Page(WMSRFIDInfoInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(input.ASNDetailId > 0, u => u.ASNDetailId == input.ASNDetailId)
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptId > 0, u => u.ReceiptId == input.ReceiptId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptPerson), u => u.ReceiptPerson.Contains(input.ReceiptPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.PreOrderId > 0, u => u.PreOrderId == input.PreOrderId)
                    .WhereIF(input.PreOrderDetailId > 0, u => u.PreOrderDetailId == input.PreOrderDetailId)
                    .WhereIF(input.OrderDetailId > 0, u => u.OrderDetailId == input.OrderDetailId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderPerson), u => u.OrderPerson.Contains(input.OrderPerson.Trim()))
                    .WhereIF(input.Status > 0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PoCode), u => u.PoCode.Contains(input.PoCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SoCode), u => u.SoCode.Contains(input.SoCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SnCode), u => u.SnCode.Contains(input.SnCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Sequence), u => u.Sequence.Contains(input.Sequence.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RFID), u => u.RFID.Contains(input.RFID.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Link), u => u.Link.Contains(input.Link.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPerson), u => u.PrintPerson.Contains(input.PrintPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                     .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSRFIDInfoOutput>()
;
        if (input.ReceiptTimeRange != null && input.ReceiptTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime >= start);
            if (input.ReceiptTimeRange.Count > 1 && input.ReceiptTimeRange[1].HasValue)
            {
                var end = input.ReceiptTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
            }
        }
        if (input.OrderTimeRange != null && input.OrderTimeRange.Count > 0)
        {
            DateTime? start = input.OrderTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.OrderTime >= start);
            if (input.OrderTimeRange.Count > 1 && input.OrderTimeRange[1].HasValue)
            {
                var end = input.OrderTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OrderTime < end);
            }
        }
        if (input.PrintTimeRange != null && input.PrintTimeRange.Count > 0)
        {
            DateTime? start = input.PrintTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime >= start);
            if (input.PrintTimeRange.Count > 1 && input.PrintTimeRange[1].HasValue)
            {
                var end = input.PrintTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
            }
        }
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime >= start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1Range != null && input.DateTime1Range.Count > 0)
        {
            DateTime? start = input.DateTime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 >= start);
            if (input.DateTime1Range.Count > 1 && input.DateTime1Range[1].HasValue)
            {
                var end = input.DateTime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2Range != null && input.DateTime2Range.Count > 0)
        {
            DateTime? start = input.DateTime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 >= start);
            if (input.DateTime2Range.Count > 1 && input.DateTime2Range[1].HasValue)
            {
                var end = input.DateTime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSRFIDInfoInput input)
    {
        var entity = input.Adapt<WMSRFIDInfo>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteWMSRFIDInfoInput input)
    //{
    //    //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
    //  //var entity = input.Adapt<WMSRFIDInfo>();
    //    await _rep.FakeDeleteAsync(entity);   //假删除
    //}

    /// <summary>
    /// 更新WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSRFIDInfoInput input)
    {
        var entity = input.Adapt<WMSRFIDInfo>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    /// <summary>
    /// 更新WMSRFIDInfo(SN)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UpdateSNCode")]
    public async Task<Response> UpdateSNCode(IFormFile file)
    {
        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        string path = await ImprotExcel.WriteFile(file);

        var dataExcel = await ExcelData.ExcelToEntityCollection<WMSRFIDImportImport>(path, null, true);
        List<WMSRFIDImportImport> _RFIDImportImports = new List<WMSRFIDImportImport>();
        _RFIDImportImports = dataExcel.Data.Adapt<List<WMSRFIDImportImport>>();

        //先校验验证码是否已经存在
        var checkentity = await _rep.AsQueryable().Where(u => _RFIDImportImports.Select(a => a.SnCode).Contains(u.SnCode)).ToListAsync();
        if (checkentity.Count > 0)
        {
            return new Response() { Code = StatusCode.Error, Msg = "校验码码已经存在，请检查！" };
        }
        //按照导入的查询出来
        //然后把序列号一个一个赋值上去
        var getentity = await _rep.AsQueryable().Where(u => _RFIDImportImports.Select(a => a.ExternReceiptNumber).Contains(u.ExternReceiptNumber)).ToListAsync();
        foreach (var item in _RFIDImportImports)
        {
            if (getentity.Where(u => u.ExternReceiptNumber == item.ExternReceiptNumber && u.PoCode == item.PoCode && u.SKU == item.SKU && string.IsNullOrEmpty(u.SnCode)).Count() > 0)
            {
                getentity.Where(u => u.ExternReceiptNumber == item.ExternReceiptNumber && u.PoCode == item.PoCode && u.SKU == item.SKU && string.IsNullOrEmpty(u.SnCode)).First().SnCode = item.SnCode;
                //getentity.Where(u => u.ExternReceiptNumber == item.ExternReceiptNumber && u.PoCode == item.PoCode && u.SKU == item.SKU && string.IsNullOrEmpty(u.SnCode)).First() = item.SnCode;
            }
        }
        //或者批量更新
        //_rep.Update(u => new WMSRFIDInfo { SNCode = u.SNCode }, u => import.Where(x => x.RFID == u.RFID).Select(x => x.SNCode));
        //var entity = input.Adapt<WMSRFIDInfo>();
        await _rep.AsUpdateable(getentity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

        return new Response() { Code = StatusCode.Success, Msg = "更新成功" }; ;
    }


    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSRFIDInfo> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSRFIDInfo列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSRFIDInfoOutput>> List([FromQuery] WMSRFIDInfoInput input)
    {
        return await _rep.AsQueryable().Select<WMSRFIDInfoOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryByReceiptId")]
    public async Task<Response<List<WMSRFIDinfoPrinfDto>>> GetPrinrRFIDInfoByReceiptId(List<long> receiptIds)
    {
        Response<List<WMSRFIDinfoPrinfDto>> response = new Response<List<WMSRFIDinfoPrinfDto>>() { Data = new List<WMSRFIDinfoPrinfDto>() };
        var entity = await _rep.AsQueryable().Where(u => receiptIds.Contains(u.ReceiptId.Value)).ToListAsync();
        entity.ForEach(u =>
        {
            u.Link = "https://oms.hachchina.com.cn/webapp/s.html?p=" + u.SnCode + ":" + u.SKU + "&code=" + u.RFID;
        });
        response.Data = entity.Adapt<List<WMSRFIDinfoPrinfDto>>();
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
    }



    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryById")]
    public async Task<Response<List<WMSRFIDinfoPrinfDto>>> GetPrinrRFIDInfoById(List<long> ids)
    {
        Response<List<WMSRFIDinfoPrinfDto>> response = new Response<List<WMSRFIDinfoPrinfDto>>() { Data = new List<WMSRFIDinfoPrinfDto>() };
        var entity = await _rep.AsQueryable().Where(u => ids.Contains(u.Id)).ToListAsync();

        await _rep.Context.Updateable<WMSRFIDInfo>()
       .SetColumns(p => p.PrintTime == DateTime.Now)
       .SetColumns(p => p.PrintPerson == _userManager.Account)
       .SetColumns(p => p.PrintNum == p.PrintNum + 1)
      .Where(u => ids.Contains(u.Id))
       .ExecuteCommandAsync();

        //await _rep.UpdateAsync(a => a.PrintTime = DateTime.Now, a.).Where(u => ids.Contains(u.Id)).ToListAsync();
        entity.ForEach(u =>
        {

            //u.Link = "https://omstest.hachultra.com.cn:8081/webapp/s3.html?p="+ u.SnCode + ":"+u.SKU;
            //u.Link = "https://oms.hachchina.com.cn/webapp/s.html?p=" + u.SnCode + ":" + u.SKU + "&code=" + u.RFID;
            u.Link = "https://oms.hachchina.com.cn/webapp/s.html?p=" + u.SnCode + ":" + u.SKU + "&code=" + u.RFID;
        });
        response.Data = entity.Adapt<List<WMSRFIDinfoPrinfDto>>();
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
    }



    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryRFID")]
    public async Task<Response<Dictionary<string, string>>> GetRFID(string rfid)
    {
        //string aaa, bbb;
        //aaa = "cF/oxVfhjnF4OCg4XOsva9GmiGa4CbXaqeqQviNcG+A=";
        //aaa = AESCryption.Encrypt(rfid);
        rfid = AESCryption.Decrypt(rfid);
        Response<Dictionary<string, string>> response = new Response<Dictionary<string, string>>();
        //校验位 以24位为基准，多于24位则截取前24位，少于24位则反馈错误信息
        if (string.IsNullOrEmpty(rfid) || rfid.Length < 24)
        {
            throw Oops.Oh(ErrorCodeEnum.D1002);
        }
        if (string.IsNullOrEmpty(rfid) || rfid.Length > 24)
        {
            //截取前24位
            rfid = rfid.Substring(0, 24);
        }
        var entity = await _rep.AsQueryable().Where(u => u.RFID == rfid).OrderByDescending(u => u.Id).FirstAsync();
        if (entity == null || string.IsNullOrEmpty(entity.RFID))
        {
            response.Code = StatusCode.Error;
            response.Msg = "不存在的RFID";
            return response;

        }
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        response.Data = new Dictionary<string, string>();
        response.Data.Add("RFID", entity.RFID);
        response.Data.Add("SKU", entity.SKU);
        response.Data.Add("合同单号", entity.PoCode);
        response.Data.Add("入库时间", entity.ReceiptTime.ToString());
        response.Data.Add("出库时间", entity.OrderTime.ToString());
        return response;
    }




    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryRFIDInfo")]
    public async Task<Response<Dictionary<string, string>>> QueryRFID(WMSRFIDInfoDto rfids)
    {
        //string rfid = rfids.RFID;
        string rfid = AESCryption.Decrypt(rfids.RFID);
        Response<Dictionary<string, string>> response = new Response<Dictionary<string, string>>();
        //校验位 以24位为基准，多于24位则截取前24位，少于24位则反馈错误信息
        if (string.IsNullOrEmpty(rfid) || rfid.Length < 24)
        {
            throw Oops.Oh(ErrorCodeEnum.D1002);
        }
        if (string.IsNullOrEmpty(rfid) || rfid.Length > 24)
        {
            //截取前24位
            rfid = rfid.Substring(0, 24);
        }
        var entity = await _rep.AsQueryable().Where(u => u.RFID == rfid).OrderByDescending(u => u.Id).FirstAsync();
        if (entity == null || string.IsNullOrEmpty(entity.RFID))
        {
            response.Code = StatusCode.Error;
            response.Msg = "不存在的RFID";
            return response;

        }
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        response.Data = new Dictionary<string, string>();
        response.Data.Add("RFID", entity.RFID);
        response.Data.Add("SKU", entity.SKU);
        response.Data.Add("合同单号", entity.PoCode);
        response.Data.Add("入库时间", entity.ReceiptTime.ToString());
        response.Data.Add("出库时间", entity.OrderTime.ToString());
        return response;
    }





    /// <summary>
    /// 分页查询WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ExportRFID")]
    public ActionResult ExportRFID(WMSRFIDInfoInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(input.ASNDetailId > 0, u => u.ASNDetailId == input.ASNDetailId)
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptId > 0, u => u.ReceiptId == input.ReceiptId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptPerson), u => u.ReceiptPerson.Contains(input.ReceiptPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.PreOrderId > 0, u => u.PreOrderId == input.PreOrderId)
                    .WhereIF(input.PreOrderDetailId > 0, u => u.PreOrderDetailId == input.PreOrderDetailId)
                    .WhereIF(input.OrderDetailId > 0, u => u.OrderDetailId == input.OrderDetailId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderPerson), u => u.OrderPerson.Contains(input.OrderPerson.Trim()))
                    .WhereIF(input.Status > 0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PoCode), u => u.PoCode.Contains(input.PoCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SoCode), u => u.SoCode.Contains(input.SoCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SnCode), u => u.SnCode.Contains(input.SnCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Sequence), u => u.Sequence.Contains(input.Sequence.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RFID), u => u.RFID.Contains(input.RFID.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Link), u => u.Link.Contains(input.Link.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPerson), u => u.PrintPerson.Contains(input.PrintPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                       .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSRFIDInfoOutput>()
;
        if (input.ReceiptTimeRange != null && input.ReceiptTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime >= start);
            if (input.ReceiptTimeRange.Count > 1 && input.ReceiptTimeRange[1].HasValue)
            {
                var end = input.ReceiptTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
            }
        }
        if (input.OrderTimeRange != null && input.OrderTimeRange.Count > 0)
        {
            DateTime? start = input.OrderTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.OrderTime >= start);
            if (input.OrderTimeRange.Count > 1 && input.OrderTimeRange[1].HasValue)
            {
                var end = input.OrderTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OrderTime < end);
            }
        }
        if (input.PrintTimeRange != null && input.PrintTimeRange.Count > 0)
        {
            DateTime? start = input.PrintTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime >= start);
            if (input.PrintTimeRange.Count > 1 && input.PrintTimeRange[1].HasValue)
            {
                var end = input.PrintTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
            }
        }
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime >= start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1Range != null && input.DateTime1Range.Count > 0)
        {
            DateTime? start = input.DateTime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 >= start);
            if (input.DateTime1Range.Count > 1 && input.DateTime1Range[1].HasValue)
            {
                var end = input.DateTime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2Range != null && input.DateTime2Range.Count > 0)
        {
            DateTime? start = input.DateTime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 >= start);
            if (input.DateTime2Range.Count > 1 && input.DateTime2Range[1].HasValue)
            {
                var end = input.DateTime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        var wMSRFIDs = query.OrderBuilder(input).ToList();

        //类型转换
        var wMSRFIDInfos = wMSRFIDs.Adapt<List<WMSRFIDImportExport>>();
        //var wMSRFIDInfos = wMSRFIDs.MapTo<List<WMSRFIDInfoOutput>>();

        //return query.ToListAsync();
        //return await query.ToListAsync();
        //IASNExcelInterface factoryExcel = ASNExcelFactory.ASNExcel();
        //factoryExcel._repTableColumns = _repTableColumns;
        //factoryExcel._userManager = _userManager;
        //factoryExcel._repASN = _rep;
        //var response = factoryExcel.Export(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<WMSRFIDImportExport>(wMSRFIDInfos);
        var fs = new MemoryStream(result.Result);
        ////return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "RFID_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };

        //return null;
    }

}

