using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.Factory;
using Admin.NET.Application.Service.WMSExpress.Factory;
using Admin.NET.Application.Service.WMSExpress.Interface;
using Admin.NET.Common;
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
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using NewLife.Reflection;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSPackage服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPackageService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPackage> _rep;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;

    private readonly SqlSugarRepository<WMSPickTask> _repPickTask;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;

    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;

    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly UserManager _userManager;
    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    private readonly SysWorkFlowService _repWorkFlowService;
    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }

    private readonly SysCacheService _sysCacheService;
    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }

    private readonly SqlSugarRepository<WMSExpressDelivery> _repExpressDelivery;
    private readonly SqlSugarRepository<WMSExpressConfig> _repExpressConfig;


    private readonly SqlSugarRepository<WMSRFPackageAcquisition> _repRFPackageAcquisition;

    //private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;


    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSExpressFee> _repWMSExpressFee;
    private readonly SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo;


    public WMSPackageService(SqlSugarRepository<WMSPackage> rep, SqlSugarRepository<WMSPickTask> repPickTask, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysCacheService sysCacheService, SqlSugarRepository<WMSExpressDelivery> repExpressDelivery, SqlSugarRepository<WMSOrderAddress> repOrderAddress, SqlSugarRepository<WMSWarehouse> repWarehouse, SqlSugarRepository<WMSExpressConfig> repExpressConfig, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSOrder> repOrder, SqlSugarRepository<WMSRFPackageAcquisition> repRFPackageAcquisition, SqlSugarRepository<WMSExpressFee> repWMSExpressFee, SqlSugarRepository<WMSRFIDInfo> repRFIDInfo, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<SysWorkFlow> repWorkFlow, SysWorkFlowService repWorkFlowService)
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
        _repWMSExpressFee = repWMSExpressFee;
        _repRFIDInfo = repRFIDInfo;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repWorkFlow = repWorkFlow;
        _repWorkFlowService = repWorkFlowService;
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
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
                    .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId.HasValue && input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PackageType), u => u.PackageType.Contains(input.PackageType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressCompany), u => u.ExpressCompany.Contains(input.ExpressCompany.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressNumber), u => u.ExpressNumber.Contains(input.ExpressNumber.Trim()))
                    .WhereIF(input.IsComposited > 0, u => u.IsComposited == input.IsComposited)
                    .WhereIF(input.IsHandovered > 0, u => u.IsHandovered == input.IsHandovered)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Handoveror), u => u.Handoveror.Contains(input.Handoveror.Trim()))
                    .WhereIF(input.PackageStatus.HasValue  && input.PackageStatus != 0, u => u.PackageStatus == input.PackageStatus)
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
        if (input.HandoverTime != null && input.HandoverTime.Count > 0)
        {
            DateTime? start = input.HandoverTime[0];
            query = query.WhereIF(start.HasValue, u => u.HandoverTime >= start);
            if (input.HandoverTime.Count > 1 && input.HandoverTime[1].HasValue)
            {
                var end = input.HandoverTime[1].Value.AddDays(1);
                query = query.Where(u => u.HandoverTime < end);
            }
        }
        if (input.PackageTime != null && input.PackageTime.Count > 0)
        {
            DateTime? start = input.PackageTime[0];
            query = query.WhereIF(start.HasValue, u => u.PackageTime >= start);
            if (input.PackageTime.Count > 1 && input.PackageTime[1].HasValue)
            {
                var end = input.PackageTime[1].Value.AddDays(1);
                query = query.Where(u => u.PackageTime < end);
            }
        }
        if (input.CreationTime != null && input.CreationTime.Count > 0)
        {
            DateTime? start = input.CreationTime[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime >= start);
            if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
            {
                var end = input.CreationTime[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1 != null && input.DateTime1.Count > 0)
        {
            DateTime? start = input.DateTime1[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 >= start);
            if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
            {
                var end = input.DateTime1[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2 != null && input.DateTime2.Count > 0)
        {
            DateTime? start = input.DateTime2[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 >= start);
            if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
            {
                var end = input.DateTime2[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3 != null && input.DateTime3.Count > 0)
        {
            DateTime? start = input.DateTime3[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 >= start);
            if (input.DateTime3.Count > 1 && input.DateTime3[1].HasValue)
            {
                var end = input.DateTime3[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4 != null && input.DateTime4.Count > 0)
        {
            DateTime? start = input.DateTime4[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 >= start);
            if (input.DateTime4.Count > 1 && input.DateTime4[1].HasValue)
            {
                var end = input.DateTime4[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5 != null && input.DateTime5.Count > 0)
        {
            DateTime? start = input.DateTime5[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 >= start);
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
    public async Task<Response> Update(UpdateWMSPackageInput input)
    {
        var entity = input.Adapt<WMSPackage>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return new Response() {  Code = StatusCode.Success, Msg = "更新成功" };
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
    [ApiDescriptionSettings(Name = "ScanPackageData")]
    //[Idempotent("ms", 500)]
    public async Task<Response<ScanPackageOutput>> ScanPackageData(ScanPackageInput input)
    {

        IPackageOperationInterface factory = PackageOperationFactory.PackageOperation("");
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

        IPackageOperationInterface factory = PackageOperationFactory.PackageOperation("");
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
        factory._repRFIDInfo = _repRFIDInfo;
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
    [ApiDescriptionSettings(Name = "PrintExpress")]
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
        factory._repWMSExpressFee = _repWMSExpressFee;

        //获取快递信息（包含快递单号）
        var data = await factory.GetExpressDataList(input);
        if (data.Code == StatusCode.Error)
        {
            response.Data = new OrderStatusDto()
            {

            };
            response.Code = StatusCode.Error;
            response.Msg = data.Msg;
            return response;

        }
        //获取打印信息
        return await factory.PrintExpressData(input);
        //获取Token 
        //return await factory.TokenExpressData(input);

        //response.Code = StatusCode.Error;
        //response.Msg = "失败";
        //return response;
    }



    /// <summary>
    /// 包装回退，清空重新扫描包装
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ResetPackageData")]
    public async Task<Response> ResetPackageData(ScanPackageInput input)
    {
        //1， 根据任务号删除包装信息
        //2， 修改任务号明细信息
        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            input.PickTaskNumber = input.Input;
        }
        //判断任务号是否为空
        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            return new Response() { Code = StatusCode.Error, Msg = "任务号不能为空" };
        }
        //获取任务号明细信息
        var pickTaskDetailList = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == input.PickTaskNumber).ToListAsync();
        if (pickTaskDetailList.Count > 1)
        {
            return new Response() { Code = StatusCode.Error, Msg = "找到多条任务号信息" };
        }
        //根据任务号获取包装信息
        var packageList = await _rep.AsQueryable().Where(a => a.PickTaskNumber == input.PickTaskNumber).ToListAsync();
        if (packageList.Count > 1)
        {
            //删除包装信息
            await _rep.DeleteAsync(packageList);
            //return new Response() { Code = StatusCode.Error, Msg = "找到多条包裹信息" };
        }

        //根据任务号获取包装明细信息
        var packageDetailList = await _repPackageDetail.AsQueryable().Where(a => a.PickTaskNumber == input.PickTaskNumber).ToListAsync();
        if (packageDetailList.Count > 1)
        {
            //删除包装信息
            await _repPackageDetail.DeleteAsync(packageDetailList);
            //return new Response() { Code = StatusCode.Error, Msg = "未找到包装信息" };
        }



        //修改任务号明细信息
        //var pickTaskDetailList = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == input.PickTaskNumber).ToListAsync();
        foreach (var item in pickTaskDetailList)
        {
            item.PickStatus = (int)PickTaskStatusEnum.拣货完成;
            //item.pa = 0;
        }
        _sysCacheService.Set(_userManager.Account + "_Package_" + input.PickTaskNumber, null);

        return new Response() { Code = StatusCode.Success, Msg = "清理成功" };
        //_sysCacheService.Get<List<PackageData>>(_userManager.Account + "_Package_" + request.PickTaskNumber);
    }


    /// <summary>
    /// 根据RFID 获取RFID 信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [ApiDescriptionSettings(Name = "GetRFIDInfo")]
    public async Task<Response<ScanPackageOutput>> GetRFIDInfo(ScanPackageRFIDInput input)
    {
     
        IPackageOperationInterface factory = PackageOperationFactory.PackageOperation("RFID");
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
        factory._repRFIDInfo = _repRFIDInfo;
        factory._repOrderDetail = _repOrderDetail;
        var response = await factory.VerifyPackage(input);
        return response;

    }



    /// <summary>
    /// 根据RFID 获取RFID 信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [ApiDescriptionSettings(Name = "PrintPackageList")]
    public async Task<Response<dynamic>> PrintPackageList(WMSPackageInput input)
    {
        Response<dynamic> response = new Response<dynamic>(); 

        var getPackageList = await _rep.AsQueryable().Where(a => input.Ids.Contains(a.Id)).ToListAsync();







        response.Data = new List<PackageData>();
        response.Data.aaa= "aaa";
        //使用简单工厂定制化修改和新增的方法
        //根据订单类型判断是否存在该流程
        //var workflow = await _repWorkFlow.AsQueryable()
        //   .Includes(a => a.SysWorkFlowSteps)
        //   .Where(a => a.WorkName == input.CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();
        //var workflow = await _repWorkFlowService.GetSystemWorkFlow(input.CustomerName, InboundWorkFlowConst.Workflow_Inbound, InboundWorkFlowConst.Workflow_ASN, input.ReceiptType);


     
        return response;

    }




    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ExportPackage")]
    public ActionResult ExportPackage(WMSPackageInput input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐

        //private const string FileDir = "/File/ExcelTemp";
        //string url = await ImprotExcel.WriteFile(file);
        //var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
        //var aaaaa = ExcelData.GetData<DataSet>(url);
        //1根据用户的角色 解析出Excel
        IPackageExportInterface factory = PackageExportFactory.PackageOperation("");

        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _repOrder;
        factory._repOrderDetail = _repOrderDetail;
        //factory._repInstruction = _repInstruction;
        //factory._repPreOrder = _repPreOrder;
        //factory._reppreOrderDetail = _repPreOrderDetail;
        //factory._repInventoryUsable = _repInventoryUsable;
        //factory._repPickTask = _repPickTask;
        //factory._repPickTaskDetail = _repPickTaskDetail;
        //factory._repOrderAllocation = _repOrderAllocation;
        factory._repPackage = _rep;
        factory._repPackageDetail = _repPackageDetail;



        var response = factory.ExportPackage(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "包装信息_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }


}

