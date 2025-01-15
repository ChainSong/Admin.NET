using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Identity;
using Nest;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// WMSOrder服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSOrder> _rep;
    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;

    //private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    //private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    //private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSInventoryUsed> _repInventoryUsed;

    private readonly SqlSugarRepository<WMSInstruction> _repInstruction;
    private readonly SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation;

    private readonly SqlSugarRepository<WMSPickTask> _repPickTask;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;


    private readonly SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail;
    private readonly SqlSugarRepository<WMSPreOrder> _repPreOrder;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;

    //private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;


    public WMSOrderService(SqlSugarRepository<WMSOrder> rep, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<WMSInventoryUsable> repInventoryUsable, ISqlSugarClient db, UserManager userManager, SqlSugarRepository<WMSInventoryUsed> repInventoryUsed, SqlSugarRepository<WMSInstruction> repInstruction, SqlSugarRepository<WMSOrderAllocation> repOrderAllocation, SqlSugarRepository<WMSPickTask> repPickTask, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WMSPreOrderDetail> repPreOrderDetail, SqlSugarRepository<WMSPreOrder> repPreOrder, SqlSugarRepository<WMSWarehouse> repWarehouse, SqlSugarRepository<WMSPackage> repPackage, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SqlSugarRepository<SysWorkFlow> repWorkFlow)
    {
        _rep = rep;
        _repOrderDetail = repOrderDetail;
        //_repReceipt = repReceipt;
        //_repReceiptDetail = repReceiptDetail;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repInventoryUsable = repInventoryUsable;
        //_db = db;
        _userManager = userManager;
        _repInventoryUsed = repInventoryUsed;
        _repInstruction = repInstruction;
        _repOrderAllocation = repOrderAllocation;
        _repPickTask = repPickTask;
        _repPickTaskDetail = repPickTaskDetail;
        _repPreOrderDetail = repPreOrderDetail;
        _repPreOrder = repPreOrder;
        _repWarehouse = repWarehouse;
        _repPackage = repPackage;
        _repPackageDetail = repPackageDetail;
        _repWorkFlow = repWorkFlow;

    }

    /// <summary>
    /// 分页查询WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSOrderOutput>> Page(WMSOrderInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.PreOrderId > 0, u => u.PreOrderId == input.PreOrderId)

                    .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId.HasValue && input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderType), u => u.OrderType.Contains(input.OrderType.Trim()))
                    .WhereIF(input.OrderStatus.HasValue && input.OrderStatus != 0, u => u.OrderStatus == input.OrderStatus)
                     .WhereIF(!string.IsNullOrWhiteSpace(input.Po), u => u.Po.Contains(input.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.So), u => u.So.Contains(input.So.Trim()))
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
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSOrderOutput>();



        if (input.PreOrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.PreOrderNumber.IndexOf("\n") > 0)
            {
                numbers = input.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.PreOrderNumber.IndexOf(',') > 0)
            {
                numbers = input.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => numbers.Contains(u.PreOrderNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()));
            }
        }

        if (input.OrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.OrderNumber.IndexOf("\n") > 0)
            {
                numbers = input.OrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.OrderNumber.IndexOf(',') > 0)
            {
                numbers = input.OrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => numbers.Contains(u.OrderNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()));
            }
        }
        if (input.ExternOrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ExternOrderNumber.IndexOf("\n") > 0)
            {
                numbers = input.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ExternOrderNumber.IndexOf(',') > 0)
            {
                numbers = input.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => numbers.Contains(u.ExternOrderNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()));
            }
        }
        if (input.OrderTime != null && input.OrderTime.Count > 0)
        {
            DateTime? start = input.OrderTime[0];
            query = query.WhereIF(start.HasValue, u => u.OrderTime >= start);
            if (input.OrderTime.Count > 1 && input.OrderTime[1].HasValue)
            {
                var end = input.OrderTime[1].Value.AddDays(1);
                query = query.Where(u => u.OrderTime < end);
            }
        }
        if (input.CompleteTime != null && input.CompleteTime.Count > 0)
        {
            DateTime? start = input.CompleteTime[0];
            query = query.WhereIF(start.HasValue, u => u.CompleteTime >= start);
            if (input.CompleteTime.Count > 1 && input.CompleteTime[1].HasValue)
            {
                var end = input.CompleteTime[1].Value.AddDays(1);
                query = query.Where(u => u.CompleteTime < end);
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
    /// 增加WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSOrderInput input)
    {
        var entity = input.Adapt<WMSOrder>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> Delete(DeleteWMSOrderInput input)
    {
        //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //await _rep.DeleteAsync(entity);   //假删除
        //根据id 获取订单信息
        var order = await _rep.AsQueryable().Where(a => input.Id == (a.Id)).FirstAsync();
        //使用简单工厂定制化修改和新增的方法
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == order.CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();



        //使用简单工厂定制化  /
        List<DeleteWMSOrderInput> request = new List<DeleteWMSOrderInput>();
        request.Add(input);
        IOrderReturnInterface factory = OrderReturnFactory.OrderReturn(workflow, order.OrderType);

        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        factory._repPreOrder = _repPreOrder;
        factory._repPreOrderDetail = _repPreOrderDetail;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repOrderAllocation = _repOrderAllocation;



        //factory._repTableColumns = _repTableInventoryUsed;
        return await factory.OrderReturn(request);
    }

    /// <summary>
    /// 更新WMSOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSOrderInput input)
    {
        var entity = input.Adapt<WMSOrder>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSOrder 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSOrder> Get(long id)
    {
        var entity = await _rep.AsQueryable()
           .Includes(a => a.Details)
           .Includes(a => a.OrderAddress)
           .Includes(a => a.Allocation)
           .Where(u => u.Id == id).FirstAsync();
        return entity;
        //var entity = await _rep.AsQueryable()
        //    .Includes(a => a.Details)
        //    .Includes(a => a.OrderAddress)
        //    .Includes(a => a.Allocation)
        //    .Where(u => u.Id == id).FirstAsync();
        //return entity;
    }

    /// <summary>
    /// 获取WMSOrder列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSOrderOutput>> List([FromQuery] WMSOrderInput input)
    {
        return await _rep.AsQueryable().Select<WMSOrderOutput>().ToListAsync();
    }



    [HttpPost]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> AutomatedAllocation(List<long> input)
    {
        //使用简单工厂定制化  / 

        IAutomatedAllocationInterface factory = AutomatedAllocationFactory.AutomatedAllocation();

        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        var response = await factory.Strategy(input);
        return response;
    }



    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "CreatePickTask")]
    public async Task<Response<List<OrderStatusDto>>> CreatePickTask(List<long> input)
    {

        //根据id 获取订单信息
        var order = await _rep.AsQueryable().Where(a => input.Contains(a.Id)).ToListAsync();
        //只允许同客户，同仓库同订单类型的订单进行拣货任务创建
        if (order.GroupBy(a => new { a.CustomerId, a.WarehouseId, a.OrderType }).Count() > 1)
        {
            return new Response<List<OrderStatusDto>>()
            {
                Code = StatusCode.Error,
                Msg = "只能选择同客户，同仓库，同订单类型的订单进行拣货任务创建！"
            };
        }

        //使用简单工厂定制化修改和新增的方法
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == order.First().CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();


        //使用简单工厂定制化  / 
        IPickTaskInterface factory = PickTaskFactory.PickTask(workflow, order.First().OrderType);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        var response = await factory.Strategy(input);
        return response;

    }


    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "CompleteOrder")]
    public async Task<Response<List<OrderStatusDto>>> CompleteOrder(List<long> input)
    {
        //使用简单工厂定制化  / 
        IOrderInterface factory = OrderFactory.CompleteOrder();
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repPreOrder = _repPreOrder;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        factory._repOrderAllocation = _repOrderAllocation;
        factory._repInventoryUsable = _repInventoryUsable;

        var response = await factory.CompleteOrder(input);
        return response;

    }



    /// <summary>
    /// 导出预出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ExportOrder")]
    public ActionResult ExportOrder(WMSOrderExcellInput input)
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
        IOrderExcelInterface factory = OrderExcelFactory.Export();

        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        factory._repPreOrder = _repPreOrder;
        factory._reppreOrderDetail = _repPreOrderDetail;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repOrderAllocation = _repOrderAllocation;
        factory._repPackage = _repPackage;
        factory._repPackageDetail = _repPackageDetail;



        var response = factory.Export(input);
        IExporter exporter = new ExcelExporter();
       
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "出库单_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }




    /// <summary>
    /// 导出预出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ExportPackage")]
    public ActionResult ExportPackage(List<long> input)
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
        IOrderExcelInterface factory = OrderExcelFactory.Export();

        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        factory._repPreOrder = _repPreOrder;
        factory._reppreOrderDetail = _repPreOrderDetail;
        factory._repInventoryUsable = _repInventoryUsable;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repOrderAllocation = _repOrderAllocation;
        factory._repPackage = _repPackage;
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


    /// <summary>
    /// 打印发货单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    public async Task<List<WMSOrderPrintDto>> PrintShippingList(List<long> input)
    {
        //使用PrintShippingList类种的打印方法  

        IPrintOrderInterface factory = PrintOrderFactory.PrintOrder();
        factory._userManager = _userManager;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _rep;
        factory._repOrderDetail = _repOrderDetail;
        factory._repInstruction = _repInstruction;
        factory._repPreOrder = _repPreOrder;
        factory._reppreOrderDetail = _repPreOrderDetail;
        factory._repPickTask = _repPickTask;
        factory._repPickTaskDetail = _repPickTaskDetail;
        factory._repOrderAllocation = _repOrderAllocation;
        factory._repWarehouse = _repWarehouse;
        factory._repCustomer = _repCustomer;



        var response = await factory.PrintShippingList(input);
        if (response.Code == StatusCode.Success)
        {
            return response.Data;
        }
        //return response;
        return response.Data;
    }



    /// <summary>
    /// 获取运输位置信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpGet]
    [ApiDescriptionSettings(Name = "GetOrderLocation")]
    public async Task<Response<List<string>>> GetOrderLocation(long id)
    {
        Response<List<string>> response = new Response<List<string>>();
        //获取位置信息
        //1，仓库的位置信息
        //2，订单的收件人位置信息
        //3，RFID终端位置信息 
        //查询出订单的信息
        var order = await _rep.AsQueryable().Includes(a => a.OrderAddress).Where(a => a.Id == id).FirstAsync();
        //根据订单中的仓库获取仓库的位置信息
        var warehouse = await _repWarehouse.AsQueryable().Where(a => a.Id == order.WarehouseId).FirstAsync();
        response.Data = new List<string>();
        response.Data.Add(warehouse.Address);
        response.Data.Add(order.OrderAddress.Province+"," + order.OrderAddress.City +","+ order.OrderAddress.Address);
        response.Code = StatusCode.Success;
        response.Msg = "获取位置信息成功";
        return response;
    }

}

