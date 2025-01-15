using Admin.NET.Common.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
//using COSXML.Network;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using NewLife.Net;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.IO;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Service;
using System.Reflection;

namespace Admin.NET.Application;
/// <summary>
/// WMS_PreOrder服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPreOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPreOrder> _rep;
    private readonly SqlSugarRepository<WMSOrderAddress> _repOrderAddress;

    private readonly SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail;
    //private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;

    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;


    private readonly SqlSugarRepository<WMSOrderDetail> _repOrderDetail;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSPreOrderExtend> _repPreOrderExtend;
    private readonly SqlSugarRepository<UploadMappingLog> _repUploadMapping;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SqlSugarRepository<WMSProductBom> _repProductBom;
    private readonly SqlSugarRepository<WMSOrderDetailBom> _repOrderDetailBom;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    public WMSPreOrderService(SqlSugarRepository<WMSPreOrder> rep, SqlSugarRepository<WMSPreOrderDetail> reppreOrderDetail, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<WMSOrderDetail> repOrderDetail, SqlSugarRepository<WMSOrder> repOrder, SqlSugarRepository<WMSPreOrderExtend> repPreOrderExtend, SqlSugarRepository<UploadMappingLog> repUploadMapping, SqlSugarRepository<WMSOrderAddress> repOrderAddress, SqlSugarRepository<WMSProduct> repProduct, SqlSugarRepository<WMSProductBom> repProductBom, SqlSugarRepository<WMSOrderDetailBom> repOrderDetailBom, SqlSugarRepository<SysWorkFlow> repWorkFlow)
    {
        _rep = rep;
        _reppreOrderDetail = reppreOrderDetail;
        _userManager = userManager;
        //_db = db;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repOrderDetail = repOrderDetail;
        _repOrder = repOrder;
        _repPreOrderExtend = repPreOrderExtend;
        _repUploadMapping = repUploadMapping;
        _repOrderAddress = repOrderAddress;
        _repProduct = repProduct;
        _repProductBom = repProductBom;
        _repOrderDetailBom = repOrderDetailBom;
        _repWorkFlow = repWorkFlow;
    }



    /// <summary>
    /// 分页查询WMS_PreOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("分页查询WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPreOrderOutput>> Page(WMSPreOrderInput input)
    {
        var query = _rep.AsQueryable()
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.PreOrderNumber), u => u.PreOrderNumber.Contains(input.PreOrderNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.CustomerId.HasValue && input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId.HasValue && input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderType), u => u.OrderType.Contains(input.OrderType.Trim()))
                    .WhereIF(input.PreOrderStatus.HasValue && input.PreOrderStatus != 0, u => u.PreOrderStatus == input.PreOrderStatus)
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

                    .Select<WMSPreOrderOutput>()
;



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
            if (input.PreOrderNumber.IndexOf(' ') > 0)
            {
                numbers = input.PreOrderNumber.Split(' ').Select(s => { return s.Trim(); });
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
            if (input.ExternOrderNumber.IndexOf(' ') > 0)
            {
                numbers = input.ExternOrderNumber.Split(' ').Select(s => { return s.Trim(); });
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
    /// 增加WMS_PreOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [DisplayName("增加WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response<List<OrderStatusDto>>> Add(AddOrUpdateWMSPreOrderInput input)
    {
        //TODO:新增前的逻辑判断，是否允许新增

        //构造传入集合
        List<AddOrUpdateWMSPreOrderInput> entityListDtos = new List<AddOrUpdateWMSPreOrderInput>();
        entityListDtos.Add(input);

        ICheckColumnsDefaultInterface checkColumnsDefault = new CheckColumnDefaultStrategy();
        checkColumnsDefault._repTableColumns = _repTableColumns;
        checkColumnsDefault._userManager = _userManager;
        var result = await checkColumnsDefault.CheckColumns<AddOrUpdateWMSPreOrderInput>(entityListDtos, "WMS_PreOrder");
        if (result.Code == StatusCode.Error)
        {
            return result;
        }

        //var asnData = _rep.AsQueryable().Where(a => a.Id == input.).First();
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == input.CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();


        //使用简单工厂定制化修改和新增的方法
        IPreOrderInterface factory = PreOrderFactory.AddOrUpdate(workflow, input.OrderType);
        factory._repPreOrder = _rep;
        factory._reppreOrderDetail = _reppreOrderDetail;
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repProduct = _repProduct;
        var response = await factory.AddStrategy(entityListDtos);
        return response;
        //var entity = ObjectMapper.Map<WMS_PreOrder>(input);
        ////调用领域服务
        //entity = await _wms_preorderManager.CreateAsync(entity);

        //var dto = ObjectMapper.Map<WMS_PreOrderEditDto>(entity);
        //response.Data.
        //var entity = input.Adapt<WMSPreOrder>();
        //await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMS_PreOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("删除WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSPreOrderInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 删除WMS_PreOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("删除WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Cancel")]
    public async Task Cancel(DeleteWMSPreOrderInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        entity.PreOrderStatus = (int)PreOrderStatusEnum.取消;
        await _rep.UpdateAsync(entity);   //
        //await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMS_PreOrder
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [UnitOfWork]
    [DisplayName("更新WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response<List<OrderStatusDto>>> Update(AddOrUpdateWMSPreOrderInput input)
    {


        List<AddOrUpdateWMSPreOrderInput> entityListDtos = new List<AddOrUpdateWMSPreOrderInput>();
        entityListDtos.Add(input);

        ICheckColumnsDefaultInterface checkColumnsDefault = new CheckColumnDefaultStrategy();
        checkColumnsDefault._repTableColumns = _repTableColumns;
        checkColumnsDefault._userManager = _userManager;
        var result = await checkColumnsDefault.CheckColumns<AddOrUpdateWMSPreOrderInput>(entityListDtos, "WMS_PreOrder");
        if (result.Code == StatusCode.Error)
        {
            return result;
        }

        //var asnData = _rep.AsQueryable().Where(a => a.Id == input.).First();
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == input.CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();

        //使用简单工厂定制化修改和新增的方法
        IPreOrderInterface factory = PreOrderFactory.AddOrUpdate(workflow, input.OrderType);
        factory._repPreOrder = _rep;
        factory._reppreOrderDetail = _reppreOrderDetail;
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repProduct = _repProduct;
        var response = await factory.UpdateStrategy(entityListDtos);
        return response;
        //var entity = input.Adapt<WMSPreOrder>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    [HttpPost]
    [DisplayName("导入PreOrder")]
    [ApiDescriptionSettings(Name = "UploadPreOrderFile")]
    public async Task<string> UploadPreOrderFile(IFormFile file)
    {


        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        var uploadInfo = await ImageUploadUtils.WriteFile(file, "UploadPreOrderFile");
        UploadMappingLog uploadMappingLog = uploadInfo.Adapt<UploadMappingLog>();
        uploadMappingLog.Creator = _userManager.Account;
        uploadMappingLog.CreationTime = DateTime.Now;
        uploadMappingLog.FileType = "PreOrderFile";
        //uploadMappingLog.Url = uploadInfo.Url;
        _repUploadMapping.Insert(uploadMappingLog);
        return uploadInfo.Url + "/" + uploadInfo.FileName;
    }


    /// <summary>
    /// 获取WMS_PreOrder 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取WMS_PreOrder")]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSPreOrder> Get(long id)
    {
        //var asd = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        var entity = await _rep.AsQueryable()
            .Includes(a => a.Details)
            .Includes(a => a.OrderAddress)
            .Includes(a => a.Extend)
            .Where(u => u.Id == id).FirstAsync();
        //var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMS_PreOrder列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取WMS_PreOrder列表")]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSPreOrderOutput>> List([FromQuery] WMSPreOrderInput input)
    {
        return await _rep.AsQueryable().Select<WMSPreOrderOutput>().ToListAsync();
    }



    /// <summary>
    /// 接收上传文件方法
    /// </summary>
    /// <param name="file">文件内容</param>
    /// <param name="Status">提交状态，第一次提交，可能存在校验提示， 用户选择忽略提示可以使用</param>
    /// <returns>文件名称</returns>
    [UnitOfWork]
    [DisplayName("接收上传文件方法")]
    [ApiDescriptionSettings(Name = "UploadExcelFile")]
    public async Task<Response<List<OrderStatusDto>>> UploadExcelFile(IFormFile file)
    {
        try
        {
            //FileDir是存储临时文件的目录，相对路径
            //private const string FileDir = "/File/ExcelTemp";
            string url = await ImprotExcel.WriteFile(file);
            var dataExcel = ExcelData.ExcelToDataTable(url, null, true);



            //var aaaaa = ExcelData.GetData<DataSet>(url);
            //1根据用户的角色 解析出Excel
            IPreOrderExcelInterface factoryExcel = PreOrderExcelFactory.GePreOrder();

            factoryExcel._repPreOrder = _rep;
            factoryExcel._repWarehouseUser = _repWarehouseUser;
            //factoryExcel._db = _db;
            factoryExcel._userManager = _userManager;

            factoryExcel._repCustomerUser = _repCustomerUser;
            factoryExcel._repWarehouseUser = _repWarehouseUser;
            factoryExcel._repTableColumns = _repTableColumns;
            factoryExcel._repTableColumnsDetail = _repTableColumnsDetail;


            var data = factoryExcel.Import(dataExcel);
            if (data.Code == StatusCode.Error)
            {
                Response<List<OrderStatusDto>> result = new Response<List<OrderStatusDto>>();
                result.Code = data.Code;
                result.Msg = data.Msg;
                result.Data = data.Result;
                return result;
            }
            var entityListDtos = data.Data.TableToList<AddOrUpdateWMSPreOrderInput>();
            var entityDetailListDtos = data.Data.TableToList<WMSPreOrderDetail>();
            var entityAddressListDtos = data.Data.TableToList<WMSOrderAddress>();

            //将散装的主表和明细表 组合到一起 
            List<AddOrUpdateWMSPreOrderInput> preOrders = entityListDtos.GroupBy(x => x.ExternOrderNumber).Select(x => x.First()).ToList();
            foreach (var item in preOrders)
            {
                item.Details = entityDetailListDtos.Where(a => a.ExternOrderNumber == item.ExternOrderNumber).ToList();
            }
            //preOrders.ForEach(item =>
            //{
            //    item.Details = entityDetailListDtos.Where(a => a.ExternOrderNumber == item.ExternOrderNumber).ToList();
            //});


            //将散装的地址表和主表组合到一起 
            //List<AddOrUpdateWMSPreOrderInput> preOrders = entityListDtos.GroupBy(x => x.ExternOrderNumber).Select(x => x.First()).ToList();
            foreach (var item in preOrders)
            {
                item.OrderAddress = entityAddressListDtos.Where(a => a.ExternOrderNumber == item.ExternOrderNumber).First();
            }
            //preOrders.ForEach(item =>
            //{
            //    item.OrderAddress = entityAddressListDtos.Where(a => a.ExternOrderNumber == item.ExternOrderNumber).First();
            //});

            //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
            var customerData = _repCustomerUser.AsQueryable().Where(a => a.CustomerName == entityListDtos.First().CustomerName).First();
            long customerId = 0;
            if (customerData != null)
            {
                customerId = customerData.CustomerId;
            }


            //根据订单类型判断是否存在该流程
            var workflow = await _repWorkFlow.AsQueryable()
               .Includes(a => a.SysWorkFlowSteps)
               .Where(a => a.WorkName == entityListDtos.First().CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();


            //long CustomerId = _wms_PreOrderRepository.GetAll().Where(a => a.PreOrderNumber == entityListDtos.First().PreOrderNumber).FirstOrDefault().CustomerId;
            //使用简单工厂定制化修改和新增的方法
            IPreOrderInterface factory = PreOrderFactory.AddOrUpdate(workflow, entityListDtos.First().OrderType);
            factory._repPreOrder = _rep;
            factory._reppreOrderDetail = _reppreOrderDetail;
            //factory._db = _db;
            factory._userManager = _userManager;
            factory._repCustomerUser = _repCustomerUser;
            factory._repWarehouseUser = _repWarehouseUser;
            factory._repTableColumns = _repTableColumns;
            factory._repTableColumnsDetail = _repTableColumnsDetail;
            factory._repProduct = _repProduct;
            var response = await factory.AddStrategy(preOrders);
            return response;

        }
        catch (Exception ex)
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = ex.Message };
            //throw Oops.Oh(ex.Message);
            //throw Oops.Oh("该订单类型不支持部分转入库单");
        }

    }


    //[Route("~/adonet/transaction")]
    /// <summary>
    /// 预出库单转出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "PreOrderForOrder")]
    [DisplayName("预出库单转出库单")]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> PreOrderForOrder(List<long> input)
    {
        //获取勾选的订单的客户
        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        var customerData = _rep.AsQueryable().Where(a => a.Id == input.First()).First();
        long customerId = 0;
        if (customerData != null)
        {
            customerId = customerData.CustomerId;
        }

        var orderData = _rep.AsQueryable().Where(a => input.Contains(a.Id)).First();

        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == orderData.CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();



        IPreOrderForOrderInterface factory = PreOrderForOrderFactory.PreOrderForOrder(workflow, orderData.OrderType);
        factory._repPreOrder = _rep;
        factory._reppreOrderDetail = _reppreOrderDetail;
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repTableColumns = _repTableColumns;
        factory._repTableColumnsDetail = _repTableColumnsDetail;
        factory._repOrder = _repOrder;
        factory._repOrderDetail = _repOrderDetail;
        factory._repProductBom = _repProductBom;
        factory._repOrderDetailBom = _repOrderDetailBom;
        var response = await factory.Strategy(input);
        return response;
    }




    /// <summary>
    /// 更新UpdateOrderAddress
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UpdateOrderAddress")]
    public async Task<Response> UpdateOrderAddress(WMSOrderAddressIntput input)
    {
        var entity = input.Adapt<WMSOrderAddress>();
        await _repOrderAddress.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "更新成功" };
    }



    /// <summary>
    /// 导出预出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [UnitOfWork]
    [DisplayName("导出预出库单")]
    public ActionResult ExportPreOrder(WMSPreOrderExcelInput input)
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
        IPreOrderExcelInterface factoryExcel = PreOrderExcelFactory.GePreOrder();

        factoryExcel._repPreOrder = _rep;
        factoryExcel._repWarehouseUser = _repWarehouseUser;
        //factoryExcel._db = _db;
        factoryExcel._userManager = _userManager;

        factoryExcel._repCustomerUser = _repCustomerUser;
        factoryExcel._repWarehouseUser = _repWarehouseUser;
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._repTableColumnsDetail = _repTableColumnsDetail;


        var response = factoryExcel.Export(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "预出库单_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }

}

