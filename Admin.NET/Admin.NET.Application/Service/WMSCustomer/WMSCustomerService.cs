using Admin.NET.Common.EnumCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// Customer服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSCustomerService : IDynamicApiController, ITransient
{

    private readonly SqlSugarRepository<WMSCustomer> _rep;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly ISqlSugarClient _db;
    private readonly UserManager _userManager;
    public WMSCustomerService(SqlSugarRepository<WMSCustomer> rep, ISqlSugarClient db, SqlSugarRepository<CustomerUserMapping> repCustomerUser, UserManager userManager)
    {
        _rep = rep;
        _db = db;
        _repCustomerUser = repCustomerUser;
        _userManager = userManager;
    }

    /// <summary>
    /// 分页查询Customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSCustomerOutput>> Page(WMSCustomerInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ProjectId > 0, u => u.ProjectId == input.ProjectId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerCode), u => u.CustomerCode.Contains(input.CustomerCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerType), u => u.CustomerType.Contains(input.CustomerType.Trim()))
                    .WhereIF(input.CustomerStatus > 0, u => u.CustomerStatus == input.CustomerStatus)
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
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.Id).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.Id).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)

                    .Select<WMSCustomerOutput>()
;
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
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加Customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(WMSCustomer input)
    {
        var entity = input.Adapt<WMSCustomer>();
        await _db.InsertNav(entity).Include(a => a.Details).ExecuteCommandAsync();


        //给自己添加客户权限
        CustomerUserMapping customerUserMapping = new CustomerUserMapping();
        customerUserMapping.Creator = _userManager.Account;
        customerUserMapping.CreationTime = DateTime.Now;
        customerUserMapping.UserId = _userManager.UserId;
        customerUserMapping.Status = 1;
        customerUserMapping.UserName = _userManager.Account;
        customerUserMapping.CustomerId = _rep.AsQueryable().Where(a => a.CustomerName == input.CustomerName).First().Id;
        customerUserMapping.CustomerName = input.CustomerName;
        await _repCustomerUser.DeleteAsync(a => a.UserId == customerUserMapping.UserId && a.CustomerId == customerUserMapping.CustomerId);
        await _repCustomerUser.InsertAsync(customerUserMapping);
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };

    }

    /// <summary>
    /// 删除Customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSCustomerInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);
    }

    /// <summary>
    /// 更新Customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response> Updata(UpdateWMSCustomerInput input)
    {
        var entity = input.Adapt<WMSCustomer>();
        await _db.UpdateNav(entity).Include(a => a.Details).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }

    /// <summary>
    /// 获取Customer
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSCustomer> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();

        return entity;
    }

    /// <summary>
    /// 获取Customer列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSCustomerOutput>> List([FromQuery] WMSCustomerInput input)
    {
        return await _rep.AsQueryable().Select<WMSCustomerOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSCustomer列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "All")]
    public async Task<List<WMSCustomerOutput>> All()
    {
        return await _rep.AsQueryable().Select<WMSCustomerOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectCustomer")]
    public async Task<List<SelectListItem>> SelectCustomer(dynamic input)
    {
        //获取可以使用的仓库权限
        var warehouse = _repCustomerUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.CustomerName).ToList();
        return await _rep.AsQueryable().Where(a => warehouse.Contains(a.CustomerName)).Select(a => new SelectListItem { Text = a.CustomerName, Value = a.Id.ToString() }).ToListAsync();
    }

}

