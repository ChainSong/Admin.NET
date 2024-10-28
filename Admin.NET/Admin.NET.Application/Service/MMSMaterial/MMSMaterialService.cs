using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service;
using Admin.NET.Common.ExcelCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XAct;

namespace Admin.NET.Application;
/// <summary>
/// MMSMaterial服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class MMSMaterialService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<MMSMaterial> _rep;

    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;

    //private readonly ISqlSugarClient _db;
    //private readonly SqlSugarRepository<WMSCustomer> _repCustomer;





    //private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
    private readonly SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail;
    private readonly SqlSugarRepository<MMSSupplier> _repSupplier;
    private readonly SqlSugarRepository<SupplierUserMapping> _repSupplierUser;

    public MMSMaterialService(SqlSugarRepository<MMSMaterial> rep, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<TableColumnsDetail> repTableColumnsDetail, SqlSugarRepository<MMSSupplier> repSupplier, SqlSugarRepository<SupplierUserMapping> repSupplierUser)
    {
        _rep = rep;
        _userManager = userManager;
        _repTableColumns = repTableColumns;
        _repTableColumnsDetail = repTableColumnsDetail;
        _repSupplier = repSupplier;
        _repSupplierUser = repSupplierUser;
        //_repWarehouseUser = repWarehouseUser;
    }


    /// <summary>
    /// 分页查询MMSMaterial
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<MMSMaterialOutput>> Page(MMSMaterialInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.SupplierId > 0, u => u.SupplierId == input.SupplierId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(input.ProductStatus != 0, u => u.ProductStatus == input.ProductStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKUClassification), u => u.SKUClassification.Contains(input.SKUClassification.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKULevel), u => u.SKULevel.Contains(input.SKULevel.Trim()))
                    .WhereIF(input.SuperId > 0, u => u.SuperId == input.SuperId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKUGroup), u => u.SKUGroup.Contains(input.SKUGroup.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ManufacturerSKU), u => u.ManufacturerSKU.Contains(input.ManufacturerSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RetailSKU), u => u.RetailSKU.Contains(input.RetailSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReplaceSKU), u => u.ReplaceSKU.Contains(input.ReplaceSKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxGroup), u => u.BoxGroup.Contains(input.BoxGroup.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Country), u => u.Country.Contains(input.Country.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Manufacturer), u => u.Manufacturer.Contains(input.Manufacturer.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DangerCode), u => u.DangerCode.Contains(input.DangerCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Cost), u => u.Cost.Contains(input.Cost.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Color), u => u.Color.Contains(input.Color.Trim()))
                    .WhereIF(input.ExpirationDate > 0, u => u.ExpirationDate == input.ExpirationDate)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
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

                    .Select<MMSMaterialOutput>()
;
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
    /// 增加MMSMaterial
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response<List<OrderStatusDto>>> Add(AddOrUpdateMaterialInput input)
    {
        //Response<List<OrderStatusDto>> response= new Response<List<OrderStatusDto>>();
        //var entity = input.Adapt<WMSASN>();
        List<AddOrUpdateMaterialInput> entityListDtos = new List<AddOrUpdateMaterialInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IMaterialInterface factory = MaterialFactory.AddOrUpdate(input.SupplierId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repMaterial = _rep;
        factory._repSupplierUser = _repSupplierUser;

        //factory._repASNDetail = _repASNDetail;
        //factory._repCustomerUser = _repCustomerUser;
        //factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.AddStrategy(entityListDtos);
        //var entity = input.Adapt<MMSMaterial>();
        //await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除MMSMaterial
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteMMSMaterialInput input)
    {
        var entity = input.Adapt<MMSMaterial>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新MMSMaterial
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response<List<OrderStatusDto>>> Update(AddOrUpdateMaterialInput input)
    {

        List<AddOrUpdateMaterialInput> entityListDtos = new List<AddOrUpdateMaterialInput>();
        entityListDtos.Add(input);
        //使用简单工厂定制化修改和新增的方法
        IMaterialInterface factory = MaterialFactory.AddOrUpdate(input.SupplierId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repMaterial = _rep;
        factory._repSupplierUser = _repSupplierUser;

        //factory._repASNDetail = _repASNDetail;
        //factory._repCustomerUser = _repCustomerUser;
        //factory._repWarehouseUser = _repWarehouseUser;
        //factory._userManager = _userManager;
        return await factory.UpdateStrategy(entityListDtos);
        //var entity = input.Adapt<MMSMaterial>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取MMSMaterial 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<MMSMaterial> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取MMSMaterial列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<MMSMaterialOutput>> List([FromQuery] MMSMaterialInput input)
    {
        return await _rep.AsQueryable().Select<MMSMaterialOutput>().ToListAsync();
    }




    /// <summary>
    /// 获取SKU列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectMaterialSKU")]
    public async Task<List<SelectListItem>> SelectMaterialSKU(dynamic input)
    {

        string sku = input.inputData;
        if (!string.IsNullOrEmpty(sku) && sku.Length > 3)
        {
            long supplierId = input.whereData.supplierId;
            //获取可以使用的仓库权限
            var supplier = _repSupplierUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.SupplierId).ToList();
            //string sku = objData.inputData;
            //long customerId = objData.objData.CustomerId;
            return await _rep.AsQueryable().Where(a => supplier.Contains(a.SupplierId) && a.SupplierId == supplierId && a.SKU.Contains(sku)).Select(a => new SelectListItem { Text = a.SKU, Value = a.GoodsName.ToString() }).Distinct().ToListAsync();
        }
        else
        {
            return new List<SelectListItem>();
        }

    }




    /// <summary>
    /// UploadExcelFile
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UploadExcelFile")]
    public async Task<Response<List<OrderStatusDto>>> UploadExcelFile(IFormFile file)
    {

        //FileDir是存储临时文件的目录，相对路径
        //private const string FileDir = "/File/ExcelTemp";
        string url = await ImprotExcel.WriteFile(file);
        var dataExcel = ExcelData.ExcelToDataTable(url, null, true);
        //var aaaaa = ExcelData.GetData<DataSet>(url);
        //1根据用户的角色 解析出Excel
        IMaterialExcelInterface factoryExcel = MaterialExcelFactory.MaterialExcel();
        factoryExcel._repTableColumns = _repTableColumns;
        factoryExcel._userManager = _userManager;
        var data = factoryExcel.Strategy(dataExcel);
        var entityListDtos = data.Data.TableToList<AddOrUpdateMaterialInput>();
        //var entityDetailListDtos = data.Data.TableToList<WMSASNDetail>();

        //将散装的主表和明细表 组合到一起 
        //List<WMSProduct> ASNs = entityListDtos.GroupBy(x => x.ExternReceiptNumber).Select(x => x.First()).ToList();
        //ASNs.ForEach(item =>
        //{
        //    item.Details = entityDetailListDtos.Where(a => a.ExternReceiptNumber == item.ExternReceiptNumber).ToList();
        //});

        //获取需要导入的客户，根据客户调用不同的配置方法(根据系统单号获取)
        //var CustomerData = entityListDtos.First();
        var SupplierData = _repSupplierUser.AsQueryable().Where(a => a.SupplierName == entityListDtos.First().SupplierName).First();
        long SupplierId = 0;
        if (SupplierData != null)
        {
            SupplierId = SupplierData.SupplierId;
        }
        //long CustomerId = _wms_asnRepository.GetAll().Where(a => a.ASNNumber == entityListDtos.First().ASNNumber).FirstOrDefault().CustomerId;
        //使用简单工厂定制化修改和新增的方法
        IMaterialInterface factory = MaterialFactory.AddOrUpdate(SupplierId);
        //factory._db = _db;
        factory._userManager = _userManager;
        factory._repMaterial = _rep;
        factory._repCustomerUser = _repCustomerUser;
        //factory._repWarehouseUser = _repWarehouseUser;
        var response = factory.AddStrategy(entityListDtos);
        return await response;

    }


}

