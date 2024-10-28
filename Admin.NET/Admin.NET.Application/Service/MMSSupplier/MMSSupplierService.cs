using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// MMSSupplier服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class MMSSupplierService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<MMSSupplier> _rep;

    private readonly SqlSugarRepository<SupplierUserMapping> _repSupplierUser;

    private readonly UserManager _userManager;

    public MMSSupplierService(SqlSugarRepository<MMSSupplier> rep, SqlSugarRepository<SupplierUserMapping> repSupplierUser, UserManager userManager)
    {
        _rep = rep;
        _repSupplierUser = repSupplierUser;
        _userManager = userManager;

    }

    /// <summary>
    /// 分页查询MMSSupplier
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<MMSSupplierOutput>> Page(MMSSupplierInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(input.ProjectId > 0, u => u.ProjectId == input.ProjectId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierCode), u => u.SupplierCode.Contains(input.SupplierCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierType), u => u.SupplierType.Contains(input.SupplierType.Trim()))
                    .WhereIF(input.SupplierStatus !=0, u => u.SupplierStatus == input.SupplierStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CreditLine), u => u.CreditLine.Contains(input.CreditLine.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Province), u => u.Province.Contains(input.Province.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.City), u => u.City.Contains(input.City.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Email), u => u.Email.Contains(input.Email.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LawPerson), u => u.LawPerson.Contains(input.LawPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PostCode), u => u.PostCode.Contains(input.PostCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Bank), u => u.Bank.Contains(input.Bank.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Account), u => u.Account.Contains(input.Account.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TaxId), u => u.TaxId.Contains(input.TaxId.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.InvoiceTitle), u => u.InvoiceTitle.Contains(input.InvoiceTitle.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Fax), u => u.Fax.Contains(input.Fax.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WebSite), u => u.WebSite.Contains(input.WebSite.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                     .Where(a => SqlFunc.Subqueryable<SupplierUserMapping>().Where(b => b.SupplierId == a.Id && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<MMSSupplierOutput>()
;

        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加MMSSupplier
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(AddMMSSupplierInput input)
    {
        var entity = input.Adapt<MMSSupplier>();
        await _rep.InsertAsync(entity);

        //给自己添加供应商权限
        SupplierUserMapping supplierUserMapping = new SupplierUserMapping();
        supplierUserMapping.Creator = _userManager.Account;
        supplierUserMapping.CreationTime = DateTime.Now;
        supplierUserMapping.UserId = _userManager.UserId;
        supplierUserMapping.Status = 1;
        supplierUserMapping.UserName = _userManager.Account;
        supplierUserMapping.SupplierId = _rep.AsQueryable().Where(a => a.SupplierName == input.SupplierName).First().Id;
        supplierUserMapping.SupplierName = input.SupplierName;
        await _repSupplierUser.DeleteAsync(a => a.UserId == supplierUserMapping.UserId && a.SupplierId == supplierUserMapping.SupplierId);
        await _repSupplierUser.InsertAsync(supplierUserMapping);
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }

    /// <summary>
    /// 删除MMSSupplier
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteMMSSupplierInput input)
    {
        var entity = input.Adapt<MMSSupplier>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新MMSSupplier
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response> Update(UpdateMMSSupplierInput input)
    {
        var entity = input.Adapt<MMSSupplier>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }


    /// <summary>
    ///  
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "All")]
    public async Task<List<MMSSupplierOutput>> All()
    {
        return await _rep.AsQueryable().Select<MMSSupplierOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取MMSSupplier 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<MMSSupplier> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取MMSSupplier列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<MMSSupplierOutput>> List([FromQuery] MMSSupplierInput input)
    {
        return await _rep.AsQueryable().Select<MMSSupplierOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectSupplier")]
    public async Task<List<SelectListItem>> SelectSupplier(dynamic input)
    {
        //获取可以使用的供应商权限
        var supplier = _repSupplierUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.SupplierName).ToList();
        return await _rep.AsQueryable().Where(a => supplier.Contains(a.SupplierName)).Select(a => new SelectListItem { Text = a.SupplierName, Value = a.Id.ToString() }).ToListAsync();
    }
}

