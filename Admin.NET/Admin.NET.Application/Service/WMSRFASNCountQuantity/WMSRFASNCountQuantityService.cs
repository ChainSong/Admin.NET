using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using AutoMapper.Internal.Mappers;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.IO;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFASNCountQuantity : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASNCountQuantity> _rep;
    private readonly SqlSugarRepository<WMSASNCountQuantityDetail> _repASNCountQuantityDetail;
    private readonly SqlSugarRepository<WMSASN> _repASN;
    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;

    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    public WMSRFASNCountQuantity(SqlSugarRepository<WMSASNCountQuantity> rep, SqlSugarRepository<WMSASN> repASN, SqlSugarRepository<WMSASNDetail> repASNDetail, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, UserManager userManager, SqlSugarRepository<SysWorkFlow> repWorkFlow, SqlSugarRepository<WMSReceipt> repReceipt, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, SqlSugarRepository<WMSProduct> repProduct, SqlSugarRepository<WMSASNCountQuantityDetail> repASNCountQuantityDetail)
    {
        _rep = rep;
        _repASN = repASN;
        _repASNDetail = repASNDetail;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _userManager = userManager;
        _repWorkFlow = repWorkFlow;
        _repReceipt = repReceipt;
        _repReceiptDetail = repReceiptDetail;
        _repProduct = repProduct;
        _repASNCountQuantityDetail = repASNCountQuantityDetail;
    }

    /// <summary>
    /// 分页查询入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSASNCountQuantityOutput>> Page(WMSASNCountQuantityInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.ASNId > 0, u => u.ASNId == input.ASNId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNCountQuantityNumber), u => u.ASNCountQuantityNumber.Contains(input.ASNCountQuantityNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ASNCountQuantityStatus > 0, u => u.ASNCountQuantityStatus == input.ASNCountQuantityStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptType), u => u.ReceiptType.Contains(input.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Contact), u => u.Contact.Contains(input.Contact.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ContactInfo), u => u.ContactInfo.Contains(input.ContactInfo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Po), u => u.Po.Contains(input.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.So), u => u.So.Contains(input.So.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str6), u => u.Str6.Contains(input.Str6.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str7), u => u.Str7.Contains(input.Str7.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str8), u => u.Str8.Contains(input.Str8.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str9), u => u.Str9.Contains(input.Str9.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str10), u => u.Str10.Contains(input.Str10.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)

                    .Select<WMSASNCountQuantityOutput>()
;
        if (input.ExpectDateRange != null && input.ExpectDateRange.Count > 0)
        {
            DateTime? start = input.ExpectDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.ExpectDate > start);
            if (input.ExpectDateRange.Count > 1 && input.ExpectDateRange[1].HasValue)
            {
                var end = input.ExpectDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.ExpectDate < end);
            }
        }
        if (input.CompleteTimeRange != null && input.CompleteTimeRange.Count > 0)
        {
            DateTime? start = input.CompleteTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CompleteTime > start);
            if (input.CompleteTimeRange.Count > 1 && input.CompleteTimeRange[1].HasValue)
            {
                var end = input.CompleteTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CompleteTime < end);
            }
        }
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
        if (input.DateTime1Range != null && input.DateTime1Range.Count > 0)
        {
            DateTime? start = input.DateTime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1Range.Count > 1 && input.DateTime1Range[1].HasValue)
            {
                var end = input.DateTime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2Range != null && input.DateTime2Range.Count > 0)
        {
            DateTime? start = input.DateTime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2Range.Count > 1 && input.DateTime2Range[1].HasValue)
            {
                var end = input.DateTime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3Range != null && input.DateTime3Range.Count > 0)
        {
            DateTime? start = input.DateTime3Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
            if (input.DateTime3Range.Count > 1 && input.DateTime3Range[1].HasValue)
            {
                var end = input.DateTime3Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4Range != null && input.DateTime4Range.Count > 0)
        {
            DateTime? start = input.DateTime4Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
            if (input.DateTime4Range.Count > 1 && input.DateTime4Range[1].HasValue)
            {
                var end = input.DateTime4Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5Range != null && input.DateTime5Range.Count > 0)
        {
            DateTime? start = input.DateTime5Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
            if (input.DateTime5Range.Count > 1 && input.DateTime5Range[1].HasValue)
            {
                var end = input.DateTime5Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Add")]
    //public async Task<Response<OrderStatusDto>> Add(List<long> input)
    //{


    //    //使用简单工厂定制化修改和新增的方法
    //    ASNCountQuantityRFInterface factory = ASNCountQuantityRFFactory.AddOrUpdate(0);
    //    //factory._db = _db;
    //    factory._userManager = _userManager;
    //    factory._repASN = _repASN;
    //    factory._repASNCountQuantity = _rep;
    //    factory._repASNDetail = _repASNDetail;
    //    factory._repCustomerUser = _repCustomerUser;
    //    factory._repWarehouseUser = _repWarehouseUser;
    //    //factory._repReceipt = _repReceipt;
    //    //factory._repReceiptDetail = _repReceiptDetail;
    //    var response = await factory.AddStrategy(input);
    //    return response;
    //    //var entity = input.Adapt<WMSASNCountQuantity>();
    //    //await _rep.InsertAsync(entity);
    //}


    /// <summary>
    /// 增加入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ScanAdd")]
    public async Task<Response<List<WMSASNCountQuantityDetailDto>>> ScanAdd(WMSASNCountQuantityDetailDto input)
    {


        //使用简单工厂定制化修改和新增的方法
        ASNCountQuantityRFInterface factory = ASNCountQuantityRFFactory.AddOrUpdate(0);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _repASN;
        factory._repASNCountQuantity = _rep;
        factory._repASNCountQuantityDetail = _repASNCountQuantityDetail;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repProduct = _repProduct;
        //factory._repReceipt = _repReceipt;
        //factory._repReceiptDetail = _repReceiptDetail;
        var response = await factory.ScanAddStrategy(input);
        return response;
        //var entity = input.Adapt<WMSASNCountQuantity>();
        //await _rep.InsertAsync(entity);
    }
    /// <summary>
    /// 删除入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Clear")]
    public async Task<Response> Clear(WMSASNCountQuantityDetailDto input)
    {

        var entity = await _repASNCountQuantityDetail.AsQueryable()
               .WhereIF(!string.IsNullOrWhiteSpace(input.BatchCode), u => u.BatchCode.Contains(input.BatchCode.Trim()))
               .Where(a => a.ASNId == input.ASNId && a.SKU == input.SKU).ToListAsync();
        await _repASNCountQuantityDetail.DeleteAsync(entity);
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }


    /// <summary>
    /// 删除入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSASNCountQuantityInput input)
    {
        var entity = input.Adapt<WMSASNCountQuantity>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSASNCountQuantityInput input)
    {
        var entity = input.Adapt<WMSASNCountQuantity>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取入库点数 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSASNCountQuantity> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取入库点数列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSASNCountQuantityOutput>> List([FromQuery] WMSASNCountQuantityInput input)
    {
        return await _rep.AsQueryable().Select<WMSASNCountQuantityOutput>().ToListAsync();
    }





}

