using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.Factory;
using Admin.NET.Application.Service.WMSExpress.Factory;
using Admin.NET.Application.Service.WMSExpress.Interface;

using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Express;
using Admin.NET.Express.Enumerate;
using AngleSharp.Dom;
using AutoMapper;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using NewLife.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSPackage服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPackageService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPackage> _rep;


    private readonly SqlSugarRepository<WMSPickTask> _repPickTask;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;

    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;


    private readonly UserManager _userManager;
    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;
    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }

    private readonly SysCacheService _sysCacheService;
    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }

    private readonly SqlSugarRepository<WMSExpressDelivery> _repExpressDelivery;
    private readonly SqlSugarRepository<WMSExpressConfig> _repExpressConfig;


    private readonly SqlSugarRepository<WMSRFPackageAcquisition> _repRFPackageAcquisition;

    //private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;


    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;

    public WMSPackageService(SqlSugarRepository<WMSPackage> rep, SqlSugarRepository<WMSPickTask> repPickTask, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysCacheService sysCacheService, SqlSugarRepository<WMSExpressDelivery> repExpressDelivery, SqlSugarRepository<WMSOrderAddress> repOrderAddress, SqlSugarRepository<WMSWarehouse> repWarehouse, SqlSugarRepository<WMSExpressConfig> repExpressConfig, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSOrder> repOrder, SqlSugarRepository<WMSRFPackageAcquisition> repRFPackageAcquisition)
    {
        _rep = rep;
        _repPickTask = repPickTask;
        _repPickTaskDetail = repPickTaskDetail;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
        _userManager = userManager;
        //_db = db;
        _repPackageDetail = repPackageDetail;
        _sysCacheService = sysCacheService;
        _repExpressDelivery = repExpressDelivery;
        _repOrderAddress = repOrderAddress;
        _repWarehouse = repWarehouse;
        _repExpressConfig = repExpressConfig;
        _repOrderDetail = repOrderDetail;
        _repOrder = repOrder;
        _repRFPackageAcquisition = repRFPackageAcquisition;
    }

    /// <summary>
    /// 分页查询WMSPackage
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPackageOutput>> Page(WMSPackageInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.OrderId > 0, u => u.OrderId == input.OrderId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PackageNumber), u => u.PackageNumber.Contains(input.PackageNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PackageType), u => u.PackageType.Contains(input.PackageType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressCompany), u => u.ExpressCompany.Contains(input.ExpressCompany.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressNumber), u => u.ExpressNumber.Contains(input.ExpressNumber.Trim()))
                    .WhereIF(input.IsComposited > 0, u => u.IsComposited == input.IsComposited)
                    .WhereIF(input.IsHandovered > 0, u => u.IsHandovered == input.IsHandovered)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Handoveror), u => u.Handoveror.Contains(input.Handoveror.Trim()))
                    .WhereIF(input.PackageStatus > 0, u => u.PackageStatus == input.PackageStatus)
                    .WhereIF(input.DetailCount > 0, u => u.DetailCount == input.DetailCount)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
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
                      .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSPackageOutput>()
;
        if (input.HandoverTimeRange != null && input.HandoverTimeRange.Count > 0)
        {
            DateTime? start = input.HandoverTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.HandoverTime > start);
            if (input.HandoverTimeRange.Count > 1 && input.HandoverTimeRange[1].HasValue)
            {
                var end = input.HandoverTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.HandoverTime < end);
            }
        }
        if (input.PackageTimeRange != null && input.PackageTimeRange.Count > 0)
        {
            DateTime? start = input.PackageTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.PackageTime > start);
            if (input.PackageTimeRange.Count > 1 && input.PackageTimeRange[1].HasValue)
            {
                var end = input.PackageTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.PackageTime < end);
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
    /// 增加WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "All")]
    public async Task<List<WMSPackage>> All(WMSPackageInput input)
    {
        return await _rep.AsQueryable().Where(a => a.PickTaskNumber == input.PickTaskNumber).ToListAsync();
    }


    /// <summary>
    /// 增加WMSPackage
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSPackageInput input)
    {
        var entity = input.Adapt<WMSPackage>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSPackage
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSPackageInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSPackage
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSPackageInput input)
    {
        var entity = input.Adapt<WMSPackage>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSPackage 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSPackage> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSPackage列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSPackageOutput>> List([FromQuery] WMSPackageInput input)
    {
        return await _rep.AsQueryable().Select<WMSPackageOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取WMSPackage列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "scanPackageData")]
    public async Task<Response<ScanPackageOutput>> ScanPackageData(ScanPackageInput input)
    {

        IPackageOperationInterface factory = PackageOperationFactory.packageOperationFactory("");
        factory._repPackage = _rep;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repCustomerUser = _repCustomerUser;
        factory._repRFPackageAcquisition = _repRFPackageAcquisition;
        factory._userManager = _userManager;
        //factory._db = _db;
        factory._repPackageDetail = _repPackageDetail;
        factory._sysCacheService = _sysCacheService;
        factory._repOrder = _repOrder;
        factory._repOrderDetail = _repOrderDetail;
        var response = await factory.GetPackage(input);
        return response;

        //return await _rep.AsQueryable().Select<WMSPackageOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSPackage列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "AddPackage")]
    public async Task<Response<ScanPackageOutput>> AddPackageData(ScanPackageInput input)
    {

        IPackageOperationInterface factory = PackageOperationFactory.packageOperationFactory("");
        factory._repPackage = _rep;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repRFPackageAcquisition = _repRFPackageAcquisition;
        factory._repCustomerUser = _repCustomerUser;
        factory._userManager = _userManager;
        //factory._db = _db;
        factory._repPackageDetail = _repPackageDetail;
        factory._sysCacheService = _sysCacheService;
        factory._repOrder = _repOrder;
        factory._repOrderDetail = _repOrderDetail;
        var response = await factory.AddPackage(input);
        return response;

        //return await _rep.AsQueryable().Select<WMSPackageOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSPackage列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "printExpress")]
    public async Task<Response<dynamic>> PrintExpress(ScanPackageInput input)
    {
        Response<dynamic> response = new Response<dynamic>();
        IExpressInterface factory = ExpressFactory.GetExpress((ExpressEnum)Enum.Parse(typeof(ExpressEnum), input.ExpressCompany));
        factory._repPackage = _rep;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repOrderAddress = _repOrderAddress;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repCustomerUser = _repCustomerUser;
        factory._userManager = _userManager;
    
        //factory._db = _db;
        
        factory._repPackageDetail = _repPackageDetail;
        factory._sysCacheService = _sysCacheService;
        factory._repWarehouse = _repWarehouse;
        factory._repExpressDelivery = _repExpressDelivery;
        factory._repExpressConfig = _repExpressConfig;

        //获取快递信息（包含快递单号）
        var data = await factory.GetExpressData(input);
        //获取打印信息
        return await factory.PrintExpressData(input);
        //获取Token 
        //return await factory.TokenExpressData(input);

        //response.Code = StatusCode.Error;
        //response.Msg = "失败";
        //return response;
    }




}

