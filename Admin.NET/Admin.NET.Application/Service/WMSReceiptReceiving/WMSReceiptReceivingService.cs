using Admin.NET.Common.EnumCommon;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.ReceiptCore.Factory;
using Admin.NET.Application.ReceiptCore.Interface;
using Admin.NET.Application.ReceiptReceivingCore.Factory;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
//using MyProject.ReceiptReceivingCore.Dto;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSReceiptReceivingService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSReceipt> _rep;
    private readonly SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;

    private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSLocation> _repLocation;

    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;
    private readonly SqlSugarRepository<WMSASN> _repASN;

    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable;
    public WMSReceiptReceivingService(SqlSugarRepository<WMSReceipt> rep, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, ISqlSugarClient db, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSReceiptReceiving> repReceiptReceiving, SqlSugarRepository<WMSLocation> repLocation, SqlSugarRepository<WMSASNDetail> repASNDetail, SqlSugarRepository<WMSASN> repASN, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed, SqlSugarRepository<WMSInventoryUsable> repTableInventoryUsable)
    {
        _rep = rep;
        _repReceiptDetail = repReceiptDetail;
        _db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repReceiptReceiving = repReceiptReceiving;
        _repLocation = repLocation;
        _repASNDetail = repASNDetail;
        _repASN = repASN;
        _repTableInventoryUsed = repTableInventoryUsed;
        _repTableInventoryUsable = repTableInventoryUsable;
    }

    /// <summary>
    /// 分页查询WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSReceiptOutput>> Page(WMSReceiptInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ASNId > 0, u => u.ASNId == input.ASNId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ReceiptStatus > 0, u => u.ReceiptStatus == input.ReceiptStatus)
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
                    .Where(a => a.ReceiptStatus >= (int)ReceiptReceivingStatusEnum.上架)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)

                    .Select<WMSReceiptOutput>()
;
        if (input.ReceiptTime != null && input.ReceiptTime.Count > 0)
        {
            DateTime? start = input.ReceiptTime[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime > start);
            if (input.ReceiptTime.Count > 1 && input.ReceiptTime[1].HasValue)
            {
                var end = input.ReceiptTime[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
            }
        }
        if (input.CompleteTime != null && input.CompleteTime.Count > 0)
        {
            DateTime? start = input.CompleteTime[0];
            query = query.WhereIF(start.HasValue, u => u.CompleteTime > start);
            if (input.CompleteTime.Count > 1 && input.CompleteTime[1].HasValue)
            {
                var end = input.CompleteTime[1].Value.AddDays(1);
                query = query.Where(u => u.CompleteTime < end);
            }
        }
        if (input.CreationTime != null && input.CreationTime.Count > 0)
        {
            DateTime? start = input.CreationTime[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
            {
                var end = input.CreationTime[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1 != null && input.DateTime1.Count > 0)
        {
            DateTime? start = input.DateTime1[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
            {
                var end = input.DateTime1[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2 != null && input.DateTime2.Count > 0)
        {
            DateTime? start = input.DateTime2[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
            {
                var end = input.DateTime2[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3 != null && input.DateTime3.Count > 0)
        {
            DateTime? start = input.DateTime3[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
            if (input.DateTime3.Count > 1 && input.DateTime3[1].HasValue)
            {
                var end = input.DateTime3[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4 != null && input.DateTime4.Count > 0)
        {
            DateTime? start = input.DateTime4[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
            if (input.DateTime4.Count > 1 && input.DateTime4[1].HasValue)
            {
                var end = input.DateTime4[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5 != null && input.DateTime5.Count > 0)
        {
            DateTime? start = input.DateTime5[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
            if (input.DateTime5.Count > 1 && input.DateTime5[1].HasValue)
            {
                var end = input.DateTime5[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSReceiptInput input)
    {
        var entity = input.Adapt<WMSReceipt>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSReceiptInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSReceiptInput input)
    {
        var entity = input.Adapt<WMSReceipt>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSReceipt 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSReceipt> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Includes(b => b.ReceiptReceivings).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSReceipt列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSReceiptOutput>> List([FromQuery] WMSReceiptInput input)
    {
        return await _rep.AsQueryable().Select<WMSReceiptOutput>().ToListAsync();
    }


    /*请在此扩展应用服务实现*/
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
        //1根据用户的角色 解析出Excel
        IReceiptReceivingExcelInterface factoryExcel = ReceiptReceivingExcelFactory.GetReceipt();
        factoryExcel._db = _db;
        factoryExcel._repReceipt = _rep;
        factoryExcel._repReceiptDetail = _repReceiptDetail;
        factoryExcel._repReceiptReceiving = _repReceiptReceiving;
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._userManager = _userManager;
        //factoryExcel._repTableColumnsDetail = _repTableColumnsDetail;
        var data = factoryExcel.Strategy(dataExcel);


        var entityListDtos = data.Data.TableToList<WMSReceiptReceiving>();
        //var entityListDtos = ObjectMapper.Map<List<WMS_ReceiptReceivingListDto>>(data.Data);

        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        var customer = _rep.AsQueryable().Where(a => a.ReceiptNumber == entityListDtos.First().ReceiptNumber).First();
        long customerId = 0;
        if (customer != null)
        {
            customerId = customer.CustomerId;
        }
        else
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "数据错误" };
        }
        //使用简单工厂定制化修改和新增的方法
        IReceiptReceivingInterface factory = ReceiptReceivingFactory.GetReceiptReceiving(customerId);
        factory._db = _db;
        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._repReceiptReceiving = _repReceiptReceiving;
        factory._repTableColumns = _repTableColumns;
        factory._repLocation = _repLocation;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;

        var response = await factory.Strategy(entityListDtos);

        return response;

    }


    /// <summary>
    /// 状态回退
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> ReturnReceiptReceiving(List<long> input)
    {
        //Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();
        //获取客户的id
        var customerData = _rep.AsQueryable().Where(a => a.Id == input[0]).First();
        long customerId = 0;
        if (customerData != null)
        {
            customerId = customerData.CustomerId;
        }
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，

        IReceiptReceivingReturnInterface factory = ReceiptReceivingReturnFactory.ReturnReceiptReceiving(customerId);
        factory._db = _db;
        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._repReceiptReceiving = _repReceiptReceiving;
        factory._repTableColumns = _repTableColumns;
        factory._repLocation = _repLocation;
        return await factory.Strategy(input);

        //return response;
    }


    /// <summary>
    ///【WMS_Receipt】通过id 加入库存
    /// </summary>
    //[AbpAuthorize(WMS_ReceiptPermissions.Node)]
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "AddInventory")]
    public async Task<Response<List<OrderStatusDto>>> AddInventory(List<long> input)
    {

        //var receipt = _rep.AsQueryable().Where(a => a.Id == input).FirstAsync();
        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>();

        //if (receipt.Result == null)
        //{
        //    response.Code = StatusCode.Error;
        //    response.Msg = "订单不存在";
        //    return response;
        //}

        //if (receipt.Result.ReceiptStatus != (int)ReceiptStatusEnum.上架)
        //{
        //    response.Code = StatusCode.Error;
        //    response.Msg = "请检查订单状态";
        //    response.Data.Add(new OrderStatusDto()
        //    {
        //        Id = receipt.Result.Id,
        //        ExternOrder = receipt.Result.ExternReceiptNumber,
        //        SystemOrder = receipt.Result.ReceiptNumber,
        //        StatusCode = StatusCode.Error,
        //        //StatusMsg = StatusCode.error.ToString(),
        //        Msg = "请检查订单状态"
        //    });

        //}
        //获取客户的id
        var customerData = _rep.AsQueryable().Where(a => a.Id == input[0]).First();
        long customerId = 0;
        if (customerData != null)
        {
            customerId = customerData.CustomerId;
        }
        //使用简单工厂定制化修改和新增的方法
        IReceiptInventoryInterface factory = ReceiptInventoryFactory.AddInventory(customerData.CustomerId);
        factory._db = _db;
        factory._repASN = _repASN;
        factory._repASNDetail = _repASNDetail;
        factory._repReceipt = _rep;
        factory._repReceiptDetail = _repReceiptDetail;
        factory._repReceiptReceiving = _repReceiptReceiving;
        factory._repTableColumns = _repTableColumns;
        factory._repTableInventoryUsable = _repTableInventoryUsable;
        factory._repTableInventoryUsed = _repTableInventoryUsed;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._userManager = _userManager;

        //List<long> ids = new List<long>();
        //ids.Add(input);
        response = await factory.Strategy(input);

        //var entity = await _wms_receiptRepository.GetAllIncluding(a => a.ReceiptReceivings).Where(b => b.Id == input.Id).FirstAsync();
        //var dto = ObjectMapper.Map<WMS_ReceiptListDto>(entity);
        return response;
    }
}

