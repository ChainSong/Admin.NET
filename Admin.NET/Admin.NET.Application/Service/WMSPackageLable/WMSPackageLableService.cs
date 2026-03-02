using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSPackageLable服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPackageLableService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPackageLable> _rep;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSWarehouse> _repWarehouse;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SysWorkFlowService _repWorkFlowService;
    private readonly UserManager _userManager;


    public WMSPackageLableService(
        SqlSugarRepository<WMSPackageLable> rep,
        SqlSugarRepository<WMSOrder> repOrder,
        SqlSugarRepository<WMSWarehouse> repWarehouse,
        SqlSugarRepository<WMSCustomer> repCustomer,
        SqlSugarRepository<WMSPackage> repPackage, UserManager userManager,
        SysWorkFlowService repWorkFlowService)
    {
        _rep = rep;
        _repOrder = repOrder;
        _repWarehouse = repWarehouse;
        _repCustomer = repCustomer;
        _repPackage = repPackage;
        _repWorkFlowService = repWorkFlowService;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询WMSPackageLable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPackageLableOutput>> Page(WMSPackageLableInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.OrderId > 0, u => u.OrderId == input.OrderId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
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
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SerialNumber), u => u.SerialNumber.Contains(input.SerialNumber.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPersonnel), u => u.PrintPersonnel.Contains(input.PrintPersonnel.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))

                    .Select<WMSPackageLableOutput>()
;
        if (input.PrintTimeRange != null && input.PrintTimeRange.Count > 0)
        {
            DateTime? start = input.PrintTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime > start);
            if (input.PrintTimeRange.Count > 1 && input.PrintTimeRange[1].HasValue)
            {
                var end = input.PrintTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
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
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSPackageLable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSPackageLableInput input)
    {
        // 验证参数
        if (string.IsNullOrWhiteSpace(input.PreOrderNumber))
        {
            throw Oops.Oh("订单号不能为空");
        }
        if (input.PrintNum == null || input.PrintNum <= 0)
        {
            throw Oops.Oh("打印数量必须大于0");
        }

        // 根据PreOrderNumber查询订单信息
        var order = await _repOrder.AsQueryable()
            .Where(o => o.PreOrderNumber == input.PreOrderNumber.Trim())
            .FirstAsync();

        if (order == null)
        {
            throw Oops.Oh($"未找到订单号为 {input.PreOrderNumber} 的订单");
        }

        // 根据打印数量生成多个打印信息
        var packageLables = new List<WMSPackageLable>();
        for (int i = 1; i <= input.PrintNum.Value; i++)
        {
            var entity = new WMSPackageLable
            {
                OrderId = order.Id,
                PreOrderNumber = order.PreOrderNumber,
                PackageNumber = $"{order.Dn ?? order.ExternOrderNumber}-{i.ToString().PadLeft(3, '0')}", // 包装单号加上序号
                OrderNumber = order.OrderNumber,
                ExternOrderNumber = order.ExternOrderNumber,
                CustomerId = order.CustomerId,
                CustomerName = order.CustomerName,
                WarehouseId = order.WarehouseId,
                WarehouseName = order.WarehouseName,
                PackageType = input.PackageType,
                Length = input.Length,
                Width = input.Width,
                Height = input.Height,
                NetWeight = input.NetWeight,
                GrossWeight = input.GrossWeight,
                ExpressCompany = input.ExpressCompany,
                ExpressNumber = input.ExpressNumber,
                SerialNumber = i.ToString(), // 序号加上序号
                PrintNum = 0,
                PrintPersonnel = input.PrintPersonnel,
                PrintTime = null,
                Creator = input.Creator,
                CreationTime = DateTime.Now,
                Updator = input.Updator,
                Remark = input.Remark
            };
            packageLables.Add(entity);
        }

        // 批量插入
        await _rep.InsertRangeAsync(packageLables);
    }




    /// <summary>
    /// 打印箱号
    /// </summary>
    /// <param name="input">订单ID列表</param>
    /// <returns></returns>
    public async Task<Response<PrintBase<List<WMSPackageLable>>>> PrintBoxNumber(List<long> input)
    {
        Response<PrintBase<List<WMSPackageLable>>> data = new Response<PrintBase<List<WMSPackageLable>>>();

        // 根据订单ID获取订单信息
        var orders = await _rep.AsQueryable()
                 //.Includes(a => a.Details)
                 //.Includes(a => a.OrderAddress)
                 .Where(u => input.Contains(u.Id)).ToListAsync();

        if (orders == null || orders.Count == 0)
        {
            data.Code = StatusCode.Error;
            data.Msg = "订单不存在";
            return data;
        }

        //if (orders(a => a.CustomerId).Distinct().Count() > 1)
        //{
        //    data.Code = StatusCode.Error;
        //    data.Msg = "请选择同一个客户的订单进行打印！";
        //    return data;
        //}

        // 获取仓库信息
        //WMSWarehouse warehouse = await _repWarehouse.AsQueryable().Where(o => o.Id == orders.First().WarehouseId).FirstAsync();

        //// 获取客户信息
        //WMSCustomer customer = await _repCustomer.AsQueryable()
        //         .Includes(a => a.Details)
        //         .Includes(a => a.CustomerConfig)
        //         .Where(o => o.Id == orders.First().CustomerId).FirstAsync();

        // 获取订单对应的包装信息
        var packageList = await _rep.AsQueryable()
                 //.Includes(a => a.Details)
                 .Where(p => input.Contains(p.Id))
                 .OrderBy(p => p.OrderId)
                 .ToListAsync();

        try
        {
            List<WMSPackageLable> orderPrintDtos = new List<WMSPackageLable>();
            orderPrintDtos = packageList.Adapt<List<WMSPackageLable>>();


            await _rep.Context.Updateable<WMSPackageLable>()
                .SetColumns(p => p.PrintPersonnel == _userManager.RealName)
                .SetColumns(p => p.PrintNum == (p.PrintNum ?? 0) + 1)
                .Where(p => input.Contains(p.OrderId))
                .ExecuteCommandAsync();

            // 为每个订单生成箱号
            //foreach (var order in orderPrintDtos)
            //{
            //    //order.Customer = customer;
            //    //order.Warehouse = warehouse;
            //    //order.CustomerConfig = customer.CustomerConfig;
            //    //order.CustomerDetail = customer.Details.FirstOrDefault();
            //    //if (order.OrderAddress != null)
            //    //{
            //    //    order.OrderAddress.Address = order.OrderAddress.Province + order.OrderAddress.City + order.OrderAddress.County + order.OrderAddress.Address;
            //    //}

            //    // 为每个明细生成箱号
            //    int boxSequence = 1;
            //    foreach (var detail in order.Details)
            //    {
            //        // 箱号规则：订单号-序列号
            //        detail.BoxNumber = string.IsNullOrEmpty(order.OrderNumber)
            //            ? $"ORDER-{order.Id}-{boxSequence}"
            //            : $"{order.OrderNumber}-{boxSequence}";

            //        boxSequence++;
            //    }

            //    // 如果订单没有明细，生成一个箱号
            //    if (order.Details == null || order.Details.Count == 0)
            //    {
            //        var defaultDetail = new WMSOrderDetailDto
            //        {
            //            BoxNumber = string.IsNullOrEmpty(order.OrderNumber)
            //                ? $"ORDER-{order.Id}-1"
            //                : $"{order.OrderNumber}-1"
            //        };
            //        order.Details = new List<WMSOrderDetailDto> { defaultDetail };
            //    }
            //}

            // 获取打印模板
            //var workflow = await _repWorkFlowService.GetSystemWorkFlow(orders.First().CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Package_Number, orders.First().OrderType);

            data.Data = new PrintBase<List<WMSPackageLable>>()
            {
                PrintTemplate = "前置打印箱号",
                Data = orderPrintDtos
            };
            data.Code = StatusCode.Success;
            data.Msg = "获取成功";
        }
        catch (Exception ex)
        {
            data.Code = StatusCode.Error;
            data.Msg = ex.Message;
        }
        return data;
    }


    /// <summary>
    /// 删除WMSPackageLable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSPackageLableInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        //var entity = input.Adapt<WMSPackageLable>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSPackageLable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSPackageLableInput input)
    {
        var entity = input.Adapt<WMSPackageLable>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSPackageLable 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSPackageLable> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSPackageLable列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSPackageLableOutput>> List([FromQuery] WMSPackageLableInput input)
    {
        return await _rep.AsQueryable().Select<WMSPackageLableOutput>().ToListAsync();
    }





}

