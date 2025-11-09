using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.Factory;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Aliyun.OSS;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.RemoteRequest;
using Nest;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Crypto;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using XAct;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECLeagueHeadSupplierOrderGetResponse.Types.CommssionOrder.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECMerchantAddFreightTemplateRequest.Types.FreightTemplate.Types;

namespace Admin.NET.Application.Service;
/// <summary>
/// WMSPickTask服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPickTaskService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPickTask> _rep;
    private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;


    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;

    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;
    private readonly SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo;

    //private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    //private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;


    private readonly UserManager _userManager;
    //private readonly ISqlSugarClient _db;
    private readonly SysCacheService _sysCacheService;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SysWorkFlowService _repWorkFlowService;
    public WMSPickTaskService(SqlSugarRepository<WMSPickTask> rep, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<Admin.NET.Core.Entity.WMSOrder> repOrder, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<Admin.NET.Core.Entity.WMSPackage> repPackage, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysCacheService sysCacheService, SqlSugarRepository<WMSRFIDInfo> repRFIDInfo, SqlSugarRepository<WMSOrderAddress> repOrderAddress, SqlSugarRepository<WMSProduct> repProduct, SysWorkFlowService repWorkFlowService, SqlSugarRepository<WMSOrderDetail> repOrderDetail)
    {
        _rep = rep;
        _userManager = userManager;
        //_db = db;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
        _repOrder = repOrder;
        _repOrderDetail = repOrderDetail;
        _repPickTaskDetail = repPickTaskDetail;
        _repPackage = repPackage;
        _repPackageDetail = repPackageDetail;
        _sysCacheService = sysCacheService;
        _repRFIDInfo = repRFIDInfo;
        _repOrderAddress = repOrderAddress;
        _repProduct = repProduct;
        _repWorkFlowService = repWorkFlowService;
    }

    /// <summary>
    /// 分页查询WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId.HasValue && input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(input.PickStatus.HasValue && input.PickStatus != 0, u => u.PickStatus == input.PickStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickType), u => u.PickType.Contains(input.PickType.Trim()))
                    .WhereIF(input.PrintNum != null, u => u.PrintNum == input.PrintNum)
                    //.WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPersonnel), u => u.PrintPersonnel.Contains(input.PrintPersonnel.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickPlanPersonnel), u => u.PickPlanPersonnel.Contains(input.PickPlanPersonnel.Trim()))
                    .WhereIF(input.DetailQty > 0, u => u.DetailQty == input.DetailQty)
                    .WhereIF(input.DetailKindsQty > 0, u => u.DetailKindsQty == input.DetailKindsQty)
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
                    .WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                    .WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSPickTaskOutput>()
;
        if (input.StartTime != null && input.StartTime.Count > 0)
        {
            DateTime? start = input.StartTime[0];
            query = query.WhereIF(start.HasValue, u => u.StartTime >= start);
            if (input.StartTime.Count > 1 && input.StartTime[1].HasValue)
            {
                var end = input.StartTime[1].Value.AddDays(1);
                query = query.Where(u => u.StartTime < end);
            }
        }
        if (input.EndTime != null && input.EndTime.Count > 0)
        {
            DateTime? start = input.EndTime[0];
            query = query.WhereIF(start.HasValue, u => u.EndTime >= start);
            if (input.EndTime.Count > 1 && input.EndTime[1].HasValue)
            {
                var end = input.EndTime[1].Value.AddDays(1);
                query = query.Where(u => u.EndTime < end);
            }
        }
        if (input.PrintTime != null && input.PrintTime.Count > 0)
        {
            DateTime? start = input.PrintTime[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime >= start);
            if (input.PrintTime.Count > 1 && input.PrintTime[1].HasValue)
            {
                var end = input.PrintTime[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
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
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSPickTaskInput input)
    {
        var entity = input.Adapt<WMSPickTask>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    //[HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task<Response<List<OrderStatusDto>>> Delete(DeleteWMSPickTaskInput input)
    {
        //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.DeleteAsync(entity);
        //await _rep.FakeDeleteAsync(entity);   //假删除
        //var workflow = await _repWorkFlowService.GetSystemWorkFlow(orderData.CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_PreOrder_ForOrder_ALL, orderData.OrderType);

        List<DeleteWMSPickTaskInput> request = new List<DeleteWMSPickTaskInput>();
        request.Add(input);
        IPickTaskReturnInterface factory = PickTaskReturnFactory.PickTaskReturn("");

        factory._userManager = _userManager;
        factory._repPickTask = _rep;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repCustomerUser = _repCustomerUser;
        factory._sysCacheService = _sysCacheService;
        factory._repOrder = _repOrder;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repPackage = _repPackage;
        factory._repPackageDetail = _repPackageDetail;
        factory._repRFIDInfo = _repRFIDInfo;

        //factory._repTableColumns = _repTableInventoryUsed;
        return await factory.PickTaskReturn(request);
    }

    /// <summary>
    /// 更新WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(WMSPickTaskBaseInput input)
    {
        var entity = input.Adapt<WMSPickTask>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    /// <summary>
    /// 更新WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "WMSPickComplete")]
    public async Task WMSPickComplete(UpdateWMSPickTaskInput input)
    {
        //var entity = input.Adapt<WMSPickTask>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(a => a.Id == input.Id).FirstAsync();
        if (entity.PickStatus < (int)PickTaskStatusEnum.拣货完成)
        {
            entity.PickStatus = (int)PickTaskStatusEnum.拣货完成;
            entity.StartTime = DateTime.Now;
            entity.EndTime = DateTime.Now;
            entity.Details.ForEach(a =>
            {
                a.PickStatus = (int)PickTaskStatusEnum.拣货完成;
                a.PickQty = a.Qty;
                a.PickTime = DateTime.Now;
            });

            await _repOrder.UpdateAsync(a => new Admin.NET.Core.Entity.WMSOrder { OrderStatus = (int)OrderStatusEnum.已拣货 }, a => entity.Details.Select(b => b.OrderId).Contains(a.Id));
            //await _rep.UpdateAsync(entity).incIgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            await _rep.Context.UpdateNav(entity).Include(a => a.Details).ExecuteCommandAsync();
        }

    }



    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSPickTask> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }


    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "PrintPickTasks")]
    public async Task<Response<PrintBase<List<WMSPickTaskOutput>>>> PrintPickTasks(List<long> ids)
    {

        //Response<PrintBase<List<WMSOrderPrintDto>>> response = new Response<PrintBase<List<WMSOrderPrintDto>>>();
        //使用PrintShippingList类种的打印方法  



        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => ids.Contains(u.Id)).ToListAsync();

        var data = entity.Adapt<List<WMSPickTaskOutput>>();
        //根据拣货单获取订单类型
        var orderEntity = await _repOrder.AsQueryable().Where(u => u.Id == data.First().Details.First().OrderId).ToListAsync();

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(data.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Pick, orderEntity.First().OrderType);


        IPrintPickTaskInterface factory = PrintPickTaskFactory.PickTaskPrint(workflow);
        factory._userManager = _userManager;
        factory._repPickTask = _rep;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._sysCacheService = _sysCacheService;
        factory._repOrder = _repOrder;
        factory._repRFIDInfo = _repRFIDInfo;
        factory._repPackage = _repPackage;
        factory._repPackageDetail = _repPackageDetail;
        factory._repOrderAddress = _repOrderAddress;
        factory._repProduct = _repProduct;
        factory._repWorkFlowService = _repWorkFlowService;
        var response = await factory.PickTaskPtint(ids);

        if (response.Code == StatusCode.Success)
        {
            var workflowPickTemplate = await _repWorkFlowService.GetSystemWorkFlow(data.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_PickTemplate, orderEntity.First().OrderType);

            response.Data.PrintTemplate = workflowPickTemplate;
            //data.Data = response.Data.Data;
            //data.Code = StatusCode.Success;
            //data.Msg = "打印成功";
            //return data;
            return response;
        }
        return response;

    }
    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetPickTasks")]
    public async Task<List<WMSPickTaskOutput>> GetPickTasks(List<long> ids)
    {



        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => ids.Contains(u.Id)).ToListAsync();

        var data = entity.Adapt<List<WMSPickTaskOutput>>();
        //根据拣货单获取订单类型
        var orderEntity = await _repOrder.AsQueryable().Where(u => u.Id == data.First().Details.First().OrderId).ToListAsync();

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(data.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Pick, orderEntity.First().OrderType);


        if (entity.First().CustomerName == "哈希")
        {
            foreach (var item in data)
            {
                var order = await _repOrder.AsQueryable().Includes(a => a.Details).Where(a => a.OrderNumber == item.OrderNumber).FirstAsync();
                var orderadrress = await _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == item.Details.First().PreOrderNumber).FirstAsync();
                var product = await _repProduct.AsQueryable().Where(a => item.Details.Select(b => b.SKU).Contains(a.SKU) && a.CustomerId == item.CustomerId).ToListAsync();
                item.Details = item.Details.GroupBy(a => new { a.SKU, a.GoodsName, a.GoodsType, a.CustomerId, a.Area, a.Location, a.PoCode, a.BatchCode, a.PickTaskNumber, a.PickTaskId }).Select(a => new WMSPickTaskDetailOutput
                {
                    SKU = a.Key.SKU,
                    GoodsName = a.Key.GoodsName,
                    GoodsType = a.Key.GoodsType,
                    Area = a.Key.Area,
                    Location = a.Key.Location,
                    BatchCode = a.Key.BatchCode,
                    PickTaskNumber = a.Key.PickTaskNumber,
                    PickTaskId = a.Key.PickTaskId,
                    Qty = a.Sum(b => b.Qty),
                    IsSN = Convert.ToBoolean(product.Where(b => b.SKU == a.Key.SKU && b.CustomerId == a.Key.CustomerId).First().IsSN).ToString(),
                    CN805 = Convert.ToBoolean(order.Details.Where(b => b.SKU == a.Key.SKU && b.PoCode == a.Key.PoCode && b.CustomerId == a.Key.CustomerId).First()?.PoCode.Contains("CN805")).ToString()
                }).OrderBy(a => a.Location).ToList();

                item.PrintTime = DateTime.Now;
                item.OrderAddress = orderadrress;
                item.Remark = order.Remark;
            }
        }
        else
        {

            foreach (var item in data)
            {
                var order = await _repOrder.AsQueryable().Includes(a => a.Details).Where(a => a.OrderNumber == item.OrderNumber).FirstAsync();
                var orderadrress = await _repOrderAddress.AsQueryable().Where(a => a.PreOrderNumber == item.Details.First().PreOrderNumber).FirstAsync();
                var product = await _repProduct.AsQueryable().Where(a => item.Details.Select(b => b.SKU).Contains(a.SKU) && a.CustomerId == item.CustomerId).ToListAsync();
                item.Details = item.Details.GroupBy(a => new { a.SKU, a.GoodsName, a.GoodsType, a.CustomerId, a.Area, a.Location, a.BatchCode, a.PickTaskNumber, a.PickTaskId }).Select(a => new WMSPickTaskDetailOutput
                {
                    SKU = a.Key.SKU,
                    GoodsName = a.Key.GoodsName,
                    GoodsType = a.Key.GoodsType,
                    Area = a.Key.Area,
                    Location = a.Key.Location,
                    BatchCode = a.Key.BatchCode,
                    PickTaskNumber = a.Key.PickTaskNumber,
                    PickTaskId = a.Key.PickTaskId,
                    Qty = a.Sum(b => b.Qty),
                    IsSN = Convert.ToBoolean(product.Where(b => b.SKU == a.Key.SKU && b.CustomerId == a.Key.CustomerId).First().IsSN).ToString(),
                    CN805 = ""
                }).OrderBy(a => a.Location).ToList();

                item.PrintTime = DateTime.Now;
                item.OrderAddress = orderadrress;
                item.Remark = order.Remark;
            }
        }

        return data;
    }
    /// <summary>
    /// 获取WMSPickTask列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSPickTaskOutput>> List([FromQuery] WMSPickTaskInput input)
    {
        return await _rep.AsQueryable().Select<WMSPickTaskOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    ///// <param name="input"></param>
    ///// <returns></returns>
    //[HttpGet]
    //[ApiDescriptionSettings(Name = "AddPrintLog")]
    //public async Task AddPrintLog(long id)
    //{
    //    var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
    //    entity.PrintNum += 1;
    //    entity.PrintPersonnel = _userManager.Account;
    //    entity.PrintTime = DateTime.Now;
    //    await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    //}

    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "AddPrintLog")]
    public async Task AddPrintLog(List<long> ids)
    {
        var entity = _rep.AsQueryable().Where(u => ids.Contains(u.Id)).ToList();
        entity.ForEach(u =>
        {
            u.PrintNum += 1;
            u.PrintPersonnel = _userManager.Account;
            u.PrintTime = DateTime.Now;
        });
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


}
