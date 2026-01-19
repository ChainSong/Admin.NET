using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Factory;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Service.Enumerate;
using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
using Admin.NET.Common;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Admin.NET.Application;
/// <summary>
/// WMSHandover服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSHandoverService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSHandover> _rep;
    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSInstruction> _repInstruction;
    private readonly SqlSugarRepository<WMSPreOrder> _repPreOrder;
    private readonly SysWorkFlowService _repWorkFlowService;

    public WMSHandoverService(SqlSugarRepository<WMSHandover> rep,
        SqlSugarRepository<WMSPackage> repPackage,
        SqlSugarRepository<CustomerUserMapping> repCustomerUser,
        SqlSugarRepository<WarehouseUserMapping> repWarehouseUser,
        SqlSugarRepository<TableColumns> repTableColumns,
        UserManager userManager,
        SqlSugarRepository<WMSOrder> repOrder,
        SqlSugarRepository<WMSInstruction> repInstruction,
        SqlSugarRepository<WMSPreOrder> repPreOrder,
        SysWorkFlowService repWorkFlowService)
    {
        _rep = rep;
        _repPackage = repPackage;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _userManager = userManager;
        _repOrder = repOrder;
        _repInstruction = repInstruction;
        _repPreOrder = repPreOrder;
        _repWorkFlowService = repWorkFlowService;
    }

    /// <summary>
    /// 分页查询WMSHandover
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSHandoverOutput>> Page(WMSHandoverInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.OrderId > 0, u => u.OrderId == input.OrderId)
                    .WhereIF(input.PackageId > 0, u => u.PackageId == input.PackageId)
                    .WhereIF(input.PickTaskId > 0, u => u.PickTaskId == input.PickTaskId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PalletNumber), u => u.PalletNumber.Contains(input.PalletNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PackageNumber), u => u.PackageNumber.Contains(input.PackageNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.HandoverType), u => u.HandoverType.Contains(input.HandoverType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressCompany), u => u.ExpressCompany.Contains(input.ExpressCompany.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExpressNumber), u => u.ExpressNumber.Contains(input.ExpressNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SerialNumber), u => u.SerialNumber.Contains(input.SerialNumber.Trim()))
                    .WhereIF(input.IsComposited > 0, u => u.IsComposited == input.IsComposited)
                    .WhereIF(input.IsHandovered > 0, u => u.IsHandovered == input.IsHandovered)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Handoveror), u => u.Handoveror.Contains(input.Handoveror.Trim()))
                    .WhereIF(input.HandoverStatus > 0, u => u.HandoverStatus == input.HandoverStatus)
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPersonnel), u => u.PrintPersonnel.Contains(input.PrintPersonnel.Trim()))
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

                    .Select<WMSHandoverOutput>()
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
    /// 增加WMSHandover
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSHandoverInput input)
    {
        var entity = input.Adapt<WMSHandover>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSHandover
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(List<long> input)
    {
        //var entity = input.Adapt<WMSHandover>();
        await _rep.DeleteAsync(a => input.Contains(a.Id));
    }

    /// <summary>
    /// 更新WMSHandover
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSHandoverInput input)
    {
        var entity = input.Adapt<WMSHandover>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSHandover 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSHandover> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSHandover列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSHandoverOutput>> List([FromQuery] WMSHandoverInput input)
    {
        return await _rep.AsQueryable().Select<WMSHandoverOutput>().ToListAsync();
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
        try
        {

            //FileDir是存储临时文件的目录，相对路径
            //private const string FileDir = "/File/ExcelTemp";
            string url = await ImprotExcel.WriteFile(file);
            var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
            //1根据用户的角色 解析出Excel
            IHandoverExcelInterface factoryExcel = HandoverExcelFactory.Getfactory();
            //factoryExcel._db = _db;
            factoryExcel._repHandover = _rep;
            factoryExcel._repPackage = _repPackage;
            //factoryExcel._repTableColumns = _repTableColumns;
            factoryExcel._repTableColumns = _repTableColumns;
            factoryExcel._userManager = _userManager;
            //factoryExcel._repTableColumnsDetail = _repTableColumnsDetail;
            var data = factoryExcel.Strategy(dataExcel);
            var entityListDtos = data.Data.TableToList<WMSHandover>();
            var getPackage = await _repPackage.AsQueryable().Where(a => a.PackageNumber == entityListDtos.First().PackageNumber).FirstAsync();
            if (getPackage == null || string.IsNullOrEmpty(getPackage.PackageNumber))
            {
                return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "数据错误" };
            }
            //var entityListDtos = ObjectMapper.Map<List<WMS_ReceiptReceivingListDto>>(data.Data);

            //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
            //var customer = _rep.AsQueryable().Where(a => a.ReceiptNumber == entityListDtos.First().ReceiptNumber).First();
            //long customerId = 0;
            //if (customer != null)
            //{
            //    customerId = customer.CustomerId;
            //}
            //else
            //{
            //    return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "数据错误" };
            //}
            var workflow = await _repWorkFlowService.GetSystemWorkFlow(getPackage.CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Outbound_Handover, "");

            //使用简单工厂定制化修改和新增的方法
            IHandoverInterface factory = HandoverFactory.AddHandover(workflow);
            //factory._db = _db;
            factory._repHandover = _rep;
            factory._repPackage = _repPackage;
            factory._repTableColumns = _repTableColumns;
            factory._repCustomerUser = _repCustomerUser;
            factory._repWarehouseUser = _repWarehouseUser;
            factory._repOrder = _repOrder;
            factory._repPreOrder = _repPreOrder;
            factory._repInstruction = _repInstruction;
            factory._userManager = _userManager;
            var response = await factory.Strategy(entityListDtos);
            return response;
        }
        catch (Exception ex)
        {
            throw Oops.Oh(ex);
        }
    }



    /// <summary>
    /// 获取WMSHandover 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "HachHandover")]
    public async Task<Response> HachHandover(List<WMSOrder> orders)
    {

        Response response = new Response();
        //出库装箱回传判断DN 是不是都完成了。ND下的所有的so 都完成才可以插入出库装箱回传 (客户系统需要对接WMS)
        //让安琪将DN 字段对接到业务表中 STR1 可以通过dn 字段来判断是不是所有的dn 都已经完成，那么可以插入装箱信息
        //判断里面有哪些DN 
        //var checkDn = await _repOrder.AsQueryable().Where(a => orders.Select(b => b.Dn).Contains(a.Dn) && a.CustomerId == orders.First().CustomerId).GroupBy(a => new { a.Dn, a.CustomerId, a.CustomerName, a.WarehouseId, a.WarehouseName })
        //    .Select(a => new
        //    {
        //        a.Key.Dn,
        //        a.Key.CustomerId,
        //        a.Key.CustomerName,
        //        a.Key.WarehouseId,
        //        a.Key.WarehouseName,
        //        Id = a.Min(b => b.Id)
        //    });

        List<WMSInstruction> wMSInstructions = new List<WMSInstruction>();
        foreach (var item in orders)
        {

            //当已经完成包装的订单数量== 预报订单里面的数量 就回传
            var checkOrderDN = await _repOrder.AsQueryable().Where(a => a.Dn == item.Dn && item.CustomerId == item.CustomerId && a.OrderStatus >= (int)OrderStatusEnum.已包装).ToListAsync();

            var checkPreOrderDN = await _repPreOrder.AsQueryable().Where(a => a.Dn == item.Dn && item.CustomerId == item.CustomerId && a.PreOrderStatus != (int)PreOrderStatusEnum.取消).ToListAsync();

            if (checkOrderDN.Count == checkPreOrderDN.Count)
            {
                WMSInstruction wMSInstructionSNGRHach = new WMSInstruction();
                //wMSInstruction.OrderId = orderData[0].Id;
                wMSInstructionSNGRHach.InstructionStatus = (int)InstructionStatusEnum.新增;
                wMSInstructionSNGRHach.InstructionType = "出库装箱回传HachDG";
                wMSInstructionSNGRHach.BusinessType = "出库装箱回传HachDG";
                //wMSInstruction.InstructionTaskNo = DateTime.Now;
                wMSInstructionSNGRHach.CustomerId = item.CustomerId;
                wMSInstructionSNGRHach.CustomerName = item.CustomerName;
                wMSInstructionSNGRHach.WarehouseId = item.WarehouseId;
                wMSInstructionSNGRHach.WarehouseName = item.WarehouseName;
                wMSInstructionSNGRHach.OperationId = item.Id;
                wMSInstructionSNGRHach.OrderNumber = item.Dn ?? "";
                wMSInstructionSNGRHach.Creator = _userManager.Account;
                wMSInstructionSNGRHach.CreationTime = DateTime.Now;
                wMSInstructionSNGRHach.InstructionTaskNo = item.Dn ?? "";
                wMSInstructionSNGRHach.TableName = "WMS_Order";
                wMSInstructionSNGRHach.InstructionPriority = 4;
                wMSInstructionSNGRHach.Remark = "";
                //判断是否插入过一次
                var getInstruction = await _repInstruction.AsQueryable().Where(a => a.CustomerId == item.CustomerId && a.OrderNumber == item.Dn && a.BusinessType == "出库装箱回传HachDG").ToListAsync();
                if (getInstruction == null || getInstruction.Count == 0)
                {
                    if (!string.IsNullOrEmpty(item.Dn))
                    {
                        if (wMSInstructions.Where(a => a.OperationId == item.Id && a.InstructionType == "出库装箱回传HachDG").Count() == 0)
                        {
                            wMSInstructions.Add(wMSInstructionSNGRHach);
                        }
                    }
                }
            }
        }

        if (wMSInstructions.Count > 0)
        {
            await _repInstruction.InsertRangeAsync(wMSInstructions);
        }
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
    }

}

