using Admin.NET.Application.CommonCore.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
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
/// WMSAdjustment服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSAdjustmentService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSAdjustment> _rep;
    private readonly SqlSugarRepository<WMSAdjustmentDetail> _repAdjustmentDetail;

    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;


    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;

    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;

    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;

    private readonly ISqlSugarClient _db;
    public WMSAdjustmentService(SqlSugarRepository<WMSAdjustment> rep, SqlSugarRepository<TableColumns> repTableColumns, UserManager userManager, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, ISqlSugarClient db, SqlSugarRepository<WMSAdjustmentDetail> repAdjustmentDetail)
    {
        _rep = rep;
        _repTableColumns = repTableColumns;
        _userManager = userManager;
        _repCustomer = repCustomer;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
        _db = db;
        _repAdjustmentDetail = repAdjustmentDetail;
    }

    /// <summary>
    /// 分页查询WMSAdjustment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSAdjustmentOutput>> Page(WMSAdjustmentInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustmentNumber), u => u.AdjustmentNumber.Contains(input.AdjustmentNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternNumber), u => u.ExternNumber.Contains(input.ExternNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.AdjustmentStatus > 0, u => u.AdjustmentStatus == input.AdjustmentStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustmentType), u => u.AdjustmentType.Contains(input.AdjustmentType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AdjustmentReason), u => u.AdjustmentReason.Contains(input.AdjustmentReason.Trim()))
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

                    .Select<WMSAdjustmentOutput>()
;
        if (input.AdjustmentTimeRange != null && input.AdjustmentTimeRange.Count > 0)
        {
            DateTime? start = input.AdjustmentTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.AdjustmentTime > start);
            if (input.AdjustmentTimeRange.Count > 1 && input.AdjustmentTimeRange[1].HasValue)
            {
                var end = input.AdjustmentTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.AdjustmentTime < end);
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
    /// 增加WMSAdjustment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<OrderStatusDto> Add(AddOrUpdateWMSAdjustmentInput input)
    {
        List<AddOrUpdateWMSAdjustmentInput> entityListDtos = new List<AddOrUpdateWMSAdjustmentInput>();
        entityListDtos.Add(input);
        //long CustomerId = _wms_PreOrderRepository.GetAll().Where(a => a.PreOrderNumber == entityListDtos.First().PreOrderNumber).FirstOrDefault().CustomerId;
        //使用简单工厂定制化修改和新增的方法
        IAdjustmentInterface factory = AdjustmentFactory.AddOrUpdate(input.CustomerId);
        factory._repAdjustment = _rep;
        factory._repAdjustmentDetail = _repAdjustmentDetail;
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._repTableColumns = _repTableColumns;
        //factory._repTableColumnsDetail = _repTableColumnsDetail;
        var response = await factory.AddStrategy(entityListDtos);
        //var entity = input.Adapt<WMSAdjustment>();
        //await _rep.InsertAsync(entity);
        return response.Data.First();
    }

    /// <summary>
    /// 删除WMSAdjustment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSAdjustmentInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);

        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSAdjustment
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<OrderStatusDto> Update(AddOrUpdateWMSAdjustmentInput input)
    {

        List<AddOrUpdateWMSAdjustmentInput> entityListDtos = new List<AddOrUpdateWMSAdjustmentInput>();
        entityListDtos.Add(input);
        //long CustomerId = _wms_PreOrderRepository.GetAll().Where(a => a.PreOrderNumber == entityListDtos.First().PreOrderNumber).FirstOrDefault().CustomerId;
        //使用简单工厂定制化修改和新增的方法
        IAdjustmentInterface factory = AdjustmentFactory.AddOrUpdate(input.CustomerId);
        factory._repAdjustment = _rep;
        factory._repAdjustmentDetail = _repAdjustmentDetail;
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._repTableColumns = _repTableColumns;
        //factory._repTableColumnsDetail = _repTableColumnsDetail;
        var response = await factory.UpdateStrategy(entityListDtos);
        //var entity = input.Adapt<WMSAdjustment>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return response.Data.First();
    }




    /// <summary>
    /// 获取WMSAdjustment 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSAdjustment> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSAdjustment列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSAdjustmentOutput>> List([FromQuery] WMSAdjustmentInput input)
    {
        return await _rep.AsQueryable().Select<WMSAdjustmentOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取WMSAdjustment列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Confirm")]
    public async Task<Response<List<OrderStatusDto>>> Confirm(List<long> input)
    {
        //获取订单的客户以及订单的类型

        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        var OrderData = _rep.AsQueryable().Where(a => input.Contains(a.Id)).First();
        long customerId = 0;
        string adjustmentType = "";
        if (OrderData != null)
        {
            customerId = OrderData.CustomerId;
            adjustmentType = OrderData.AdjustmentType;
        }
        //long CustomerId = _wms_PreOrderRepository.GetAll().Where(a => a.PreOrderNumber == entityListDtos.First().PreOrderNumber).FirstOrDefault().CustomerId;
        //使用简单工厂定制化修改和新增的方法
        IAdjustmentConfirmInterface factory = AdjustmentConfirmFactory.Confirm(customerId, adjustmentType);
        factory._repAdjustment = _rep;
        factory._repAdjustmentDetail = _repAdjustmentDetail;
        factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        //factory._repTableColumns = _repTableColumns;
        //factory._repTableColumnsDetail = _repTableColumnsDetail;
        var response = await factory.Strategy(input);
        return response;
        //return await _rep.AsQueryable().Select<WMSAdjustmentOutput>().ToListAsync();
    }

    /// <summary>
    /// 接收上传文件方法
    /// </summary>
    /// <param name="file">文件内容</param>
    /// <param name="Status">提交状态，第一次提交，可能存在校验提示， 用户选择忽略提示可以使用</param>
    /// <returns>文件名称</returns>
    [UnitOfWork]
    public async Task<List<OrderStatusDto>> UploadExcelFile(IFormFile file)
    {

        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        string url = await ImprotExcel.WriteFile(file);
        var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
        //var aaaaa = ExcelData.GetData<DataSet>(url);
        //1根据用户的角色 解析出Excel
        IAdjustmentExcelInterface factoryExcel = AdjustmentExcelFactory.AdjustmentExcel();


        factoryExcel._userManager = _userManager;
        factoryExcel._repTableColumns = _repTableColumns;

        var data = factoryExcel.Strategy(dataExcel);
        var entityListDtos = data.Data.TableToList<AddOrUpdateWMSAdjustmentInput>();
        var entityDetailListDtos = data.Data.TableToList<WMSAdjustmentDetail>();
        if (entityListDtos.Count > 0)
        {
            //将散装的主表和明细表 组合到一起 
            List<AddOrUpdateWMSAdjustmentInput> Adjustment = entityListDtos.GroupBy(x => x.AdjustmentNumber).Select(x => x.First()).ToList();
            Adjustment.ForEach(item =>
            {
                item.Details = entityDetailListDtos.Where(a => a.AdjustmentNumber == item.AdjustmentNumber).ToList();
            });

            //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
            var customerData = _repCustomerUser.AsQueryable().Where(a => a.CustomerName == entityListDtos.First().CustomerName).First();
            long customerId = 0;
            if (customerData != null)
            {
                customerId = customerData.CustomerId;
            }
            //long CustomerId = _wms_PreOrderRepository.GetAll().Where(a => a.PreOrderNumber == entityListDtos.First().PreOrderNumber).FirstOrDefault().CustomerId;
            //使用简单工厂定制化修改和新增的方法
            IAdjustmentInterface factory = AdjustmentFactory.AddOrUpdate(customerId);
            factory._repAdjustment = _rep;
            factory._repAdjustmentDetail = _repAdjustmentDetail;
            factory._db = _db;
            factory._userManager = _userManager;
            factory._repCustomerUser = _repCustomerUser;
            factory._repWarehouseUser = _repWarehouseUser;
            //factory._repTableColumns = _repTableColumns;
            //factory._repTableColumnsDetail = _repTableColumnsDetail;
            var response = factory.AddStrategy(Adjustment);

            return response.Result.Data;
        }
        else
        {
            return new List<OrderStatusDto>();
        }


    }



}

