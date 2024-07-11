using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service.WMSExpress.Factory;
using Admin.NET.Application.Service.WMSExpress.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Express.Enumerate;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using XAct.Library.Settings;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service.WMSExpress.Enumerate;

namespace Admin.NET.Application;
/// <summary>
/// WMSExpressConfig服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSExpressConfigService : IDynamicApiController, ITransient
{
    //private readonly SqlSugarRepository<WMSExpressConfig> _rep;

    //private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    //private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    //private readonly UserManager _userManager;
    //public WMSExpressConfigService(SqlSugarRepository<WMSExpressConfig> rep, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager)
    //{
    //    _rep = rep;
    //    _repCustomerUser = repCustomerUser;
    //    _repWarehouseUser = repWarehouseUser;
    //    _userManager = userManager;
    //}
    private readonly SqlSugarRepository<WMSExpressConfig> _rep;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;


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
    //private readonly SqlSugarRepository<WMSExpressConfig> _repExpressConfig;



    //private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;


    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;

    private readonly SqlSugarRepository<WMSExpressFee> _repWMSExpressFee;

    public WMSExpressConfigService(SqlSugarRepository<WMSExpressConfig> rep, SqlSugarRepository<WMSPackage> repPackage, SqlSugarRepository<WMSPickTask> repPickTask, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysCacheService sysCacheService, SqlSugarRepository<WMSExpressDelivery> repExpressDelivery, SqlSugarRepository<WMSOrderAddress> repOrderAddress, SqlSugarRepository<WMSWarehouse> repWarehouse, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSOrder> repOrder, SqlSugarRepository<WMSExpressFee> repWMSExpressFee)
    {
        _rep = rep;
        _repPackage = repPackage;
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
        //_repExpressConfig = repExpressConfig;
        _repOrderDetail = repOrderDetail;
        _repOrder = repOrder;
        _repWMSExpressFee = repWMSExpressFee;
    }


    /// <summary>
    /// 分页查询WMSExpressConfig
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSExpressConfigOutput>> Page(WMSExpressConfigInput input)
    {
        var query = _rep.AsQueryable()
                    //.WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    //.WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressCode), u => u.ExpressCode.Contains(input.ExpressCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressCompany), u => u.ExpressCompany.Contains(input.ExpressCompany.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Url), u => u.Url.Contains(input.Url.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.AppKey), u => u.AppKey.Contains(input.AppKey.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CompanyCode), u => u.CompanyCode.Contains(input.CompanyCode.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Sign), u => u.Sign.Contains(input.Sign.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), u => u.CustomerCode.Contains(input.CustomerCode.Trim()))
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSExpressConfigOutput>();
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSExpressConfig
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSExpressConfigInput input)
    {

        //判断是否存在同名配置
        var checkEntity = await _rep.GetFirstAsync(u => u.ExpressCompany == input.ExpressCompany && u.CustomerName == input.CustomerName && u.WarehouseName == input.WarehouseName);
        if (checkEntity != null)
        {
            throw Oops.Oh(ErrorCodeExpressConfigEnum.E0001);
        }
        var entity = input.Adapt<WMSExpressConfig>();

        //判断是否存在同名配置


        entity.Status = 1;
        entity.Creator = _userManager.Account;
        entity.CreationTime = DateTime.Now;
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSExpressConfig
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSExpressConfigInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        await _rep.DeleteAsync(entity);   //删除
    }

    /// <summary>
    /// 更新WMSExpressConfig
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSExpressConfigInput input)
    {
        var entity = input.Adapt<WMSExpressConfig>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSExpressConfig 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSExpressConfig> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }


    /// <summary>
    /// 获取WMSExpressConfig 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "AllExpress")]
    public async Task<List<WMSExpressConfig>> All()
    {
        var entity = await _rep.AsQueryable()
              .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
              .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
              .Select(a => new WMSExpressConfig { ExpressCompany = a.ExpressCompany }).Distinct()
              .ToListAsync();
        return entity;
    }


    /// <summary>
    /// 获取WMSExpressConfig列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSExpressConfigOutput>> List([FromQuery] WMSExpressConfigInput input)
    {
        return await _rep.AsQueryable().Select<WMSExpressConfigOutput>().ToListAsync();
    }



    /// <summary>
    /// 获取WMSPackage列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetExpressConfig")]
    public async Task<Response<dynamic>> GetExpressConfig(ScanPackageInput input)
    {
        Response<dynamic> response = new Response<dynamic>();
        IExpressInterface factory = ExpressFactory.GetExpress((ExpressEnum)Enum.Parse(typeof(ExpressEnum), input.ExpressCompany));
        //factory._repExpressDelivery = _rep;
        factory._repExpressConfig = _rep;
        factory._repPackage = _repPackage;
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
        factory._repWMSExpressFee = _repWMSExpressFee;

        //获取快递信息（包含快递单号）
        //var data = await factory.GetExpressData(input);
        //获取打印信息
        //var result = await factory.PrintExpressData(input);
        //获取Token 
        return await factory.GetExpressConfig(input);

        //response.Code = StatusCode.Error;
        //response.Msg = "失败";
        //return response;
    }



}

