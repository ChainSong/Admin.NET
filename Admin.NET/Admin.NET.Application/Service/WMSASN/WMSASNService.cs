﻿using Admin.NET.Application.CommonCore.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSASN服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSASNService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASN> _rep;
    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;


    private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;

    public WMSASNService(SqlSugarRepository<WMSASN> rep, ISqlSugarClient db, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, SqlSugarRepository<WMSReceipt> repReceipt, SqlSugarRepository<WMSASNDetail> repASNDetail)
    {
        _rep = rep;
        _db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _userManager = userManager;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repTableColumns = repTableColumns;
        _repReceiptDetail = repReceiptDetail;
        _repReceipt = repReceipt;
        _repASNDetail = repASNDetail;
    }

    /// <summary>
    /// 分页查询WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSASNOutput>> Page(WMSASNInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ASNStatus > 0, u => u.ASNStatus == input.ASNStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptType), u => u.ReceiptType.Contains(input.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Contact), u => u.Contact.Contains(input.Contact.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ContactInfo), u => u.ContactInfo.Contains(input.ContactInfo.Trim()))
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
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str11), u => u.Str11.Contains(input.Str11.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str12), u => u.Str12.Contains(input.Str12.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str13), u => u.Str13.Contains(input.Str13.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str14), u => u.Str14.Contains(input.Str14.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str15), u => u.Str15.Contains(input.Str15.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str16), u => u.Str16.Contains(input.Str16.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str17), u => u.Str17.Contains(input.Str17.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str18), u => u.Str18.Contains(input.Str18.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str19), u => u.Str19.Contains(input.Str19.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str20), u => u.Str20.Contains(input.Str20.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)
                    .WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                    .WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)

                    .Select<WMSASNOutput>()
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
    /// 增加WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response<List<OrderStatusDto>>> Add(AddOrUpdateWMSASNInput input)
    {
        //Response<List<OrderStatusDto>> response= new Response<List<OrderStatusDto>>();
        //var entity = input.Adapt<WMSASN>();
        List<AddOrUpdateWMSASNInput> entityListDtos = new List<AddOrUpdateWMSASNInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IASNInterface factory = ASNFactory.AddOrUpdate(input.CustomerId);
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.AddStrategy(entityListDtos);
        //string asdasd = response.Result.Msg;
        //await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSASNInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response<List<OrderStatusDto>>> Update(AddOrUpdateWMSASNInput input)
    {
        List<AddOrUpdateWMSASNInput> entityListDtos = new List<AddOrUpdateWMSASNInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IASNInterface factory = ASNFactory.AddOrUpdate(input.CustomerId);
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.UpdateStrategy(entityListDtos);
        //var entity = input.Adapt<WMSASN>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSASN 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSASN> Get(long id)
    {
        //var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSASN列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSASNOutput>> List([FromQuery] WMSASNInput input)
    {
        return await _rep.AsQueryable().Select<WMSASNOutput>().ToListAsync();
    }


    /// <summary>
    /// 接收上传文件方法
    /// </summary>
    /// <param name="file">文件内容</param>
    /// <returns>文件名称</returns>
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> UploadExcelFile(IFormFile file)
    {
        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        string url = await ImprotExcel.WriteFile(file);
        var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
        //var aaaaa = ExcelData.GetData<DataSet>(url);
        //1根据用户的角色 解析出Excel
        IASNExcelInterface factoryExcel = ASNExcelFactory.ASNExcel();
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._userManager = _userManager;
        var data = factoryExcel.Strategy(dataExcel);
        var entityListDtos = data.Data.TableToList<AddOrUpdateWMSASNInput>();
        var entityDetailListDtos = data.Data.TableToList<WMSASNDetail>();

        //将散装的主表和明细表 组合到一起 
        List<AddOrUpdateWMSASNInput> ASNs = entityListDtos.GroupBy(x => x.ExternReceiptNumber).Select(x => x.First()).ToList();
        ASNs.ForEach(item =>
        {
            item.Details = entityDetailListDtos.Where(a => a.ExternReceiptNumber == item.ExternReceiptNumber).ToList();
        });

        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        var CustomerData = _repCustomerUser.AsQueryable().Where(a => a.CustomerName == entityListDtos.First().CustomerName).First();
        long CustomerId = 0;
        if (CustomerData != null)
        {
            CustomerId = CustomerData.CustomerId;
        }
        //long CustomerId = _wms_asnRepository.GetAll().Where(a => a.ASNNumber == entityListDtos.First().ASNNumber).FirstOrDefault().CustomerId;
        //使用简单工厂定制化修改和新增的方法
        IASNInterface factory = ASNFactory.AddOrUpdate(CustomerId);
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        var response = factory.AddStrategy(entityListDtos);
        return await response;





    }


    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ASNForReceipt")]
    public async Task<Response<List<OrderStatusDto>>> ASNForReceipt(List<long> input)
    {
        var customerData = _rep.AsQueryable().Where(a => a.Id == input.First()).First();
        long customerId = 0;
        if (customerData != null)
        {
            customerId = customerData.CustomerId;
        }
        //使用简单工厂定制化修改和新增的方法
        IASNForReceiptInterface factory = ASNForReceiptFactory.ASNForReceipt(customerId);
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repCustomerUser = _repCustomerUser;

        var response = factory.Strategy(input);
        return await response;
    }

}
