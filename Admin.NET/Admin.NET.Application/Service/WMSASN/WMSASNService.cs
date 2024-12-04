using Admin.NET.Common.ExcelCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
using Elasticsearch.Net;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Http;
using Nest;
using NewLife.Net;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using Admin.NET.Express;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.IO;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service;
using Admin.NET.Application.Enumerate;

namespace Admin.NET.Application;
/// <summary>
/// WMSASN服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSASNService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASN> _rep;
    private readonly SqlSugarRepository<WMSASNDetail> _repASNDetail;


    //private readonly ISqlSugarClient _db;
    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysWorkFlow> _repWorkFlow;
    private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;
    private readonly SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo;
    private readonly SqlSugarRepository<SysWorkFlowStep> _repWorkFlowStep;

    public WMSASNService(SqlSugarRepository<WMSASN> rep, SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, SqlSugarRepository<WMSReceipt> repReceipt, SqlSugarRepository<WMSASNDetail> repASNDetail, SqlSugarRepository<SysWorkFlow> repWorkFlow, SqlSugarRepository<WMSProduct> repProduct, SqlSugarRepository<WMSRFIDInfo> repRFIDInfo, SqlSugarRepository<SysWorkFlowStep> repWorkFlowStep)
    {
        _rep = rep;
        //_db = db;
        _repCustomer = repCustomer;
        _repCustomerUser = repCustomerUser;
        _userManager = userManager;
        _repWarehouseUser = repWarehouseUser;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repTableColumns = repTableColumns;
        _repReceiptDetail = repReceiptDetail;
        _repReceipt = repReceipt;
        _repASNDetail = repASNDetail;
        _repWorkFlow = repWorkFlow;
        _repProduct = repProduct;
        _repRFIDInfo = repRFIDInfo;
        _repWorkFlowStep = repWorkFlowStep;
    }

    /// <summary>
    /// 分页查询WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("分页查询WMSASN")]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSASNOutput>> Page(WMSASNInput input)
    {
        var query = _rep.AsQueryable()
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.ASNStatus != 0, u => u.ASNStatus == input.ASNStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptType), u => u.ReceiptType.Contains(input.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Po), u => u.Po.Contains(input.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.So), u => u.So.Contains(input.So.Trim()))
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
                    //.Where(a=>_repCustomerUser.AsQueryable().Where(b=>b.CustomerId==a.CustomerId).Count()>0)
                    //.Where(a=>_repWarehouseUser.AsQueryable().Where(b=>b.WarehouseId==a.WarehouseId).Count()>0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSASNOutput>()
;


        if (input.ASNNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ASNNumber.IndexOf("\n") > 0)
            {
                numbers = input.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ASNNumber.IndexOf(',') > 0)
            {
                numbers = input.ASNNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (input.ASNNumber.IndexOf(' ') > 0)
            {
                numbers = input.ASNNumber.Split(' ').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => numbers.Contains(u.ASNNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()));
            }
        }

        if (input.ExternReceiptNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (input.ExternReceiptNumber.IndexOf("\n") > 0)
            {
                numbers = input.ExternReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (input.ExternReceiptNumber.IndexOf(',') > 0)
            {
                numbers = input.ExternReceiptNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (input.ExternReceiptNumber.IndexOf(' ') > 0)
            {
                numbers = input.ExternReceiptNumber.Split(' ').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => numbers.Contains(u.ExternReceiptNumber.Trim()));

            }
            else
            {
                query.WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()));
            }
        }

        if (input.ExpectDate != null && input.ExpectDate.Count > 0)
        {
            DateTime? start = input.ExpectDate[0];
            query = query.WhereIF(start.HasValue, u => u.ExpectDate > start);
            if (input.ExpectDate.Count > 1 && input.ExpectDate[1].HasValue)
            {
                var end = input.ExpectDate[1].Value.AddDays(1);
                query = query.Where(u => u.ExpectDate < end);
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
    /// 增加WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    [UnitOfWork]
    public async Task<Response<List<OrderStatusDto>>> Add(AddOrUpdateWMSASNInput input)
    {
        //Response<List<OrderStatusDto>> response= new Response<List<OrderStatusDto>>();
        //var entity = input.Adapt<WMSASN>();
        List<AddOrUpdateWMSASNInput> entityListDtos = new List<AddOrUpdateWMSASNInput>();
        entityListDtos.Add(input);

        ICheckColumnsDefaultInterface checkColumnsDefault = new CheckColumnDefaultStrategy();
        checkColumnsDefault._repTableColumns = _repTableColumns;
        checkColumnsDefault._userManager = _userManager;
        var result = await checkColumnsDefault.CheckColumns<AddOrUpdateWMSASNInput>(entityListDtos, "WMS_ASN");
        if (result.Code == StatusCode.Error)
        {
            return result;
        }
        //使用简单工厂定制化修改和新增的方法
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == input.CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();


        //使用简单工厂定制化修改和新增的方法
        IASNInterface factory = ASNFactory.AddOrUpdate(workflow, input.ReceiptType);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repProduct = _repProduct;
        //factory._userManager = _userManager;
        return await factory.AddStrategy(entityListDtos);
        //string asdasd = response.Result.Msg;
        //await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("删除WMSASN")]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSASNInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //删除
    }

    /// <summary>
    /// 删除WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("删除WMSASN")]
    [ApiDescriptionSettings(Name = "Cancel")]
    public async Task Cancel(DeleteWMSASNInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        entity.ASNStatus = (int)ASNStatusEnum.取消;
        await _rep.UpdateAsync(entity);   //删除
        //await _rep.DeleteAsync(entity);   //删除
    }

    /// <summary>
    /// 更新WMSASN
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [DisplayName("更新WMSASN")]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response<List<OrderStatusDto>>> Update(AddOrUpdateWMSASNInput input)
    {
        List<AddOrUpdateWMSASNInput> entityListDtos = new List<AddOrUpdateWMSASNInput>();
        entityListDtos.Add(input);

        ICheckColumnsDefaultInterface checkColumnsDefault = new CheckColumnDefaultStrategy();
        checkColumnsDefault._repTableColumns = _repTableColumns;
        checkColumnsDefault._userManager = _userManager;
        var result = await checkColumnsDefault.CheckColumns<AddOrUpdateWMSASNInput>(entityListDtos, "WMS_ASN");
        if (result.Code == StatusCode.Error)
        {
            return result;
        }
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == input.CustomerName+ InboundWorkFlowConst.Workflow_Inbound).FirstAsync();


        //使用简单工厂定制化修改和新增的方法
        IASNInterface factory = ASNFactory.AddOrUpdate(workflow, input.ReceiptType);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repProduct = _repProduct;
        //factory._userManager = _userManager;
        return await factory.UpdateStrategy(entityListDtos);
    }




    /// <summary>
    /// 获取WMSASN 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取WMSASN")]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSASN> Get(long id)
    {
        //var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSASN列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [DisplayName("获取WMSASN列表")]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSASNOutput>> List([FromQuery] WMSASNInput input)
    {
        return await _rep.AsQueryable().Select<WMSASNOutput>().ToListAsync();
    }


    /// <summary>
    /// 接收上传文件方法
    /// </summary>
    /// <param name="file">文件内容</param>
    /// <returns>文件名称</returns>
    [UnitOfWork]
    [DisplayName("接收上传文件方法")]
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
            IASNExcelInterface factoryExcel = ASNExcelFactory.ASNExcel();
            factoryExcel._repTableColumns = _repTableColumns;
            factoryExcel._userManager = _userManager;
            factoryExcel._repASN = _rep;
            var data = factoryExcel.Import(dataExcel);
            if (data.Code == StatusCode.Error)
            {
                Response<List<OrderStatusDto>> result = new Response<List<OrderStatusDto>>();
                result.Code = data.Code;
                result.Msg = data.Msg;
                result.Data = data.Result;
                return result;
            }
            var entityListDtos = data.Data.TableToList<AddOrUpdateWMSASNInput>();
            var entityDetailListDtos = data.Data.TableToList<WMSASNDetail>();

            //将散装的主表和明细表 组合到一起 
            List<AddOrUpdateWMSASNInput> ASNs = entityListDtos.GroupBy(x => x.ExternReceiptNumber).Select(x => x.First()).ToList();
            ASNs.ForEach(item =>
            {
                item.Details = entityDetailListDtos.Where(a => a.ExternReceiptNumber == item.ExternReceiptNumber).ToList();
            });

            //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
            var customerData = _repCustomerUser.AsQueryable().Where(a => a.CustomerName == entityListDtos.First().CustomerName).First();
            long customerId = 0;
            if (customerData != null)
            {
                customerId = customerData.CustomerId;
            }
            //long CustomerId = _wms_asnRepository.GetAll().Where(a => a.ASNNumber == entityListDtos.First().ASNNumber).FirstOrDefault().CustomerId;
            //使用简单工厂定制化修改和新增的方法
            //根据订单类型判断是否存在该流程
            var workflow = await _repWorkFlow.AsQueryable()
               .Includes(a => a.SysWorkFlowSteps)
               .Where(a => a.WorkName == ASNs.First().CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();


            //使用简单工厂定制化修改和新增的方法
            IASNInterface factory = ASNFactory.AddOrUpdate(workflow, ASNs.First().ReceiptType);
            //factory._db = _db;
            factory._userManager = _userManager;
            factory._repASN = _rep;
            factory._repCustomerUser = _repCustomerUser;
            factory._repWarehouseUser = _repWarehouseUser;
            factory._repProduct = _repProduct;
            var response = factory.AddStrategy(ASNs);
            return await response;
        }
        catch (Exception ex)
        {
            return new Response<List<OrderStatusDto>> { Code = StatusCode.Error, Msg = ex.Message };
            //throw Oops.Oh(ErrorCodeEnum.D1001, ex.Message);
        }
    }
    [DisplayName("ASN 转入库单")]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ASNForReceipt")]
    public async Task<Response<List<OrderStatusDto>>> ASNForReceipt(List<long> input)
    {
        var asnData = await _rep.AsQueryable().Where(a => input.Contains(a.Id)).ToListAsync();

        if (asnData == null || asnData.Count == 0)
        {
            throw Oops.Oh("订单不存在");
        }

        //验证客户和订单类型，一次只能选择一种订单类型
        if (asnData.Select(a => new { a.ReceiptType, a.CustomerName }).Distinct().Count() > 1)
        {
            throw Oops.Oh("只能操作同一种订单类型和客户");
        }

        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
            .Includes(a => a.SysWorkFlowSteps)
            .Where(a => a.WorkName == asnData.First().CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();


        //使用简单工厂定制化修改和新增的方法
        IASNForReceiptInterface factory = ASNForReceiptFactory.ASNForReceipt(workflow, asnData.First().ReceiptType);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repRFIDInfo = _repRFIDInfo;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repReceipt = _repReceipt;
        factory._repProduct = _repProduct;
        factory._repReceiptDetail = _repReceiptDetail;
        var response = factory.Strategy(input);
        return await response;
    }



    /// <summary>
    /// ASN 转入库单(部分)
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("ASN 转入库单(部分)")]
    [UnitOfWork]
    [ApiDescriptionSettings(Name = "ASNForReceiptPart")]
    public async Task<Response<List<OrderStatusDto>>> ASNForReceiptPart(List<WMSASNForReceiptDetailDto> input)
    {
        var asnData = _rep.AsQueryable().Where(a => a.Id == input.First().ASNId).First();
        //根据订单类型判断是否存在该流程
        var workflow = await _repWorkFlow.AsQueryable()
           .Includes(a => a.SysWorkFlowSteps)
           .Where(a => a.WorkName == asnData.CustomerName + InboundWorkFlowConst.Workflow_Inbound).FirstAsync();

        if (workflow == null || workflow.SysWorkFlowSteps.Where(a => a.StepName == InboundWorkFlowConst.Workflow_ASNForReceiptPart).Count() == 0)
        {
            throw Oops.Oh("该订单类型不支持部分转入库单");
        }

        //if (input == null || input.Count == 0)asnData
        //{
        //    throw Oops.Oh("订单不存在");
        //}

        ////验证客户和订单类型，一次只能选择一种订单类型
        //if (input.Select(a => new { a.ReceiptType, a.CustomerName }).Distinct().Count() > 1)
        //{
        //    throw Oops.Oh("只能操作同一种订单类型和客户");
        //}

        //long customerId = 0;
        //if (input != null)
        //{
        //    customerId = input.First().CustomerId;
        //}
        //使用简单工厂定制化修改和新增的方法
        IASNForReceiptInterface factory = ASNForReceiptFactory.ASNForReceipt(workflow, asnData.ReceiptType);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repASN = _rep;
        factory._repASNDetail = _repASNDetail;
        factory._repCustomerUser = _repCustomerUser;
        factory._repWarehouseUser = _repWarehouseUser;
        factory._repReceipt = _repReceipt;
        factory._repProduct = _repProduct;

        factory._repRFIDInfo = _repRFIDInfo;
        factory._repReceiptDetail = _repReceiptDetail;
        var response = factory.StrategyPart(input);
        return await response;
    }




    /// <summary>
    /// 导出预出库单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    [HttpPost]
    [DisplayName("导出预出库单")]
    [UnitOfWork]
    public ActionResult ExportASN(WMSASNExcelInput input)
    {
        //使用简单工厂定制化  /
        //不同的仓库存在不同的上架推荐库位的逻辑，这个地方按照实际的情况实现自己的业务逻辑，
        //默认：1，按照已有库存，且库存最小推荐
        //默认：2，没有库存，以前有库存
        //默认：3，随便推荐

        IASNExcelInterface factoryExcel = ASNExcelFactory.ASNExcel();
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._userManager = _userManager;
        factoryExcel._repASN = _rep;
        var response = factoryExcel.Export(input);
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "预入库_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }


}

