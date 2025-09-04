using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using FluentEmail.Core;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using StackExchange.Profiling.Internal;
using System.Collections.Generic;
using System.Linq;
using XAct;
//using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;

namespace Admin.NET.Application;
/// <summary>
/// WMSWarehouse服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSWarehouseService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSWarehouse> _rep;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<WMSShelf> _repShelf;

    private readonly UserManager _userManager;
    public WMSWarehouseService(SqlSugarRepository<WMSWarehouse> rep, UserManager userManager, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<WMSShelf> repShelf)
    {
        _rep = rep;
        _userManager = userManager;
        _repWarehouseUser = repWarehouseUser;
        _repShelf = repShelf;
    }

    /// <summary>
    /// 分页查询WMSWarehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSWarehouseOutput>> Page(WMSWarehouseInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ProjectId > 0, u => u.ProjectId == input.ProjectId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(input.WarehouseStatus != 0, u => u.WarehouseStatus == input.WarehouseStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseType), u => u.WarehouseType.Contains(input.WarehouseType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Description), u => u.Description.Contains(input.Description.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Company), u => u.Company.Contains(input.Company.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Province), u => u.Province.Contains(input.Province.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.City), u => u.City.Contains(input.City.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Contractor), u => u.Contractor.Contains(input.Contractor.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ContractorAddress), u => u.ContractorAddress.Contains(input.ContractorAddress.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Mobile), u => u.Mobile.Contains(input.Mobile.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Fax), u => u.Fax.Contains(input.Fax.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Email), u => u.Email.Contains(input.Email.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.Id).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.Id && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSWarehouseOutput>()
;
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSWarehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(AddWMSWarehouseInput input)
    {
        var entity = input.Adapt<WMSWarehouse>();


        //验证仓库编码是否存在
        if (await _rep.AsQueryable().AnyAsync(u => u.WarehouseName == input.WarehouseName))
        {
            return new Response() { Code = StatusCode.Error, Msg = "仓库编码已存在" };
        }

        if (entity.Details != null)
        {
            entity.Details.ForEach(a =>
            {
                a.WarehouseName = entity.WarehouseName;
                a.Creator = _userManager.Account;
                a.CreateTime = DateTime.Now;
            });
        }
        await _rep.Context.InsertNav(entity).Include(a => a.Details).ExecuteCommandAsync();

        //给自己添加仓库权限
        WarehouseUserMapping warehouseUserMapping = new WarehouseUserMapping();
        warehouseUserMapping.Creator = _userManager.Account;
        warehouseUserMapping.CreationTime = DateTime.Now;
        warehouseUserMapping.UserId = _userManager.UserId;
        warehouseUserMapping.Status = 1;
        warehouseUserMapping.UserName = _userManager.Account;
        warehouseUserMapping.WarehouseId = _rep.AsQueryable().Where(a => a.WarehouseName == input.WarehouseName).First().Id;
        warehouseUserMapping.WarehouseName = input.WarehouseName;
        //var WarehouseUserentity = input.Adapt<WarehouseUserMapping>();
        //WarehouseUserentity.ForEach(a =>
        //{

        //    a.Creator = _userManager.Account;
        //    a.CreationTime = DateTime.Now;
        //});
        await _repWarehouseUser.DeleteAsync(a => a.UserId == warehouseUserMapping.UserId && a.WarehouseId == warehouseUserMapping.WarehouseId);
        await _repWarehouseUser.InsertAsync(warehouseUserMapping);
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
    }

    /// <summary>
    /// 删除WMSWarehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSWarehouseInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //删除
    }

    /// <summary>
    /// 更新WMSWarehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task<Response> Update(UpdateWMSWarehouseInput input)
    {
        //return Task.Run( () =>
        //{
        var entity = input.Adapt<WMSWarehouse>();
        if (entity.Details != null)
        {
            entity.Details.ForEach(a =>
            {
                a.WarehouseName = entity.WarehouseName;
                a.Creator = _userManager.Account;
                a.CreateTime = DateTime.Now;
            });
        }
        await _rep.Context.UpdateNav(entity).Include(a => a.Details).ExecuteCommandAsync();
        return new Response() { Code = StatusCode.Success, Msg = "操作成功" };
        //});
    }

    /// <summary>
    /// 获取WMSWarehouse
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSWarehouse> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSWarehouseOutput>> List(WMSWarehouseInput input)
    {
        return await _rep.AsQueryable().Select<WMSWarehouseOutput>().ToListAsync();
    }

    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "All")]
    public async Task<List<WMSWarehouseOutput>> All()
    {
        return await _rep.AsQueryable().Select<WMSWarehouseOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectWarehouse")]
    public async Task<List<SelectListItem>> SelectWarehouse(dynamic input)
    {

        try
        {
            string warehouseInput = input.inputData;
            // 获取可以使用的仓库权限
            var warehouse = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.WarehouseName).ToList();
            if (!string.IsNullOrEmpty(warehouseInput))
            {
                return await _rep.AsQueryable().Where(a => warehouse.Contains(a.WarehouseName) && a.WarehouseName.Contains(warehouseInput)).Select(a => new SelectListItem { Text = a.WarehouseName, Value = a.Id.ToString() }).Distinct().ToListAsync();
                //return await _rep.AsQueryable().Where(a => customer.Contains(a.CustomerId) && a.CustomerId == customerId && a.SKU.Contains(sku)).Select(a => new SelectListItem { Text = a.SKU, Value = a.GoodsName.ToString() }).Distinct().Take(6).ToListAsync();
            }
            else
            {
                return await _rep.AsQueryable().Where(a => warehouse.Contains(a.WarehouseName)).Select(a => new SelectListItem { Text = a.WarehouseName, Value = a.Id.ToString() }).Distinct().ToListAsync();
            }
        }
        catch (Exception)
        {
            throw Oops.Oh("请选择仓库");
        }

    }

    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetShelf")]
    public async Task<List<WMSShelfMapDto>> GetShelf()
    {
        List<WMSShelfMapDto> mapDto = new List<WMSShelfMapDto>();

        var SheIf = await _repShelf.AsQueryable().Select<WMSShelf>().ToListAsync();
        mapDto = SheIf.GroupBy(a => new { a.ShelfNo, a.LocationX, a.LocationY, a.LocationZ, a.Direction }).Select(a => new WMSShelfMapDto
        {
            ShelfNo = a.Key.ShelfNo,
            x = a.Key.LocationX,
            y = a.Key.LocationY,
            z = a.Key.LocationZ,
            direction = a.Key.Direction,
        }).Distinct().ToList();

        foreach (var item in mapDto)
        {
            layers layers = new layers();
            var layersData = SheIf.Where(a => a.ShelfNo == item.ShelfNo).GroupBy(a => new
            {
                a.ShelfNo,
                a.SlotX,
                a.SlotY,
                a.SlotZ
            }).Select(a => new layers
            {
                slotX = a.Key.SlotX,
                slotY = a.Key.SlotY,
                slotZ = a.Key.SlotZ,
            }).Distinct().ToList();

            foreach (var itemlocation in layersData)
            {
                var locationData = SheIf.Where(a => a.ShelfNo == item.ShelfNo && a.SlotX == itemlocation.slotX && a.SlotZ == itemlocation.slotZ).GroupBy(a => new
                {
                    a.Location,
                    a.SlotY
                }).OrderBy(a => a.Key.SlotY).Select(a => new boxes
                {
                    code = a.Key.Location,
                    count = "10"
                }).Distinct().ToList();

                itemlocation.boxes = new List<List<boxes>>();
                var aaa = new List<boxes>();
                aaa.Add(locationData[0]);
                var bbb = new List<boxes>();
                bbb.Add(locationData[1]);
                itemlocation.boxes.Add(aaa);
                itemlocation.boxes.Add(bbb);

            }


            item.layers = layersData;

        }
        return mapDto;
    }

}

