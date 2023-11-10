using Admin.NET.Application.CommonCore.EnumCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// 库区管理服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSAreaService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSArea> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    public WMSAreaService(SqlSugarRepository<WMSArea> rep, UserManager userManager, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser)
    {
        _rep = rep;
        _userManager = userManager;
        _repWarehouseUser = repWarehouseUser;
    }

    /// <summary>
    /// 分页查询库区管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSAreaOutput>> Page(WMSAreaInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AreaName), u => u.AreaName.Contains(input.AreaName.Trim()))
                    .WhereIF(input.AreaStatus > 0, u => u.AreaStatus == input.AreaStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.AreaType), u => u.AreaType.Contains(input.AreaType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<WMSAreaOutput>()
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
    /// 增加库区管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task<Response> Add(AddWMSAreaInput input)
    {
        var entity = input.Adapt<WMSArea>();
        await _rep.InsertAsync(entity);

        //添加之前先校验有没有
        var data = _rep.AsQueryable().Where(a => a.AreaName == entity.AreaName && a.WarehouseId == entity.WarehouseId);
        if (data.Count() > 0)
        {
            return new Response() { Code = StatusCode.Error, Msg = "库区已经存在" };
        }
        else
        {
            await _rep.InsertAsync(entity);
        }
        return new Response() { Code = StatusCode.Success, Msg = "添加成功" };

    }

    /// <summary>
    /// 删除库区管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSAreaInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新库区管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSAreaInput input)
    {
        var entity = input.Adapt<WMSArea>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 获取库区管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSArea> Get(long Id)
    {
        return await _rep.GetFirstAsync(u => u.Id == Id);
    }

    /// <summary>
    /// 获取库区管理列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSAreaOutput>> List([FromQuery] WMSAreaInput input)
    {
        return await _rep.AsQueryable().Select<WMSAreaOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSWarehouse列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SelectArea")]
    public async Task<List<SelectListItem>> SelectArea(dynamic input)
    {
        try
        {


            //获取可以使用的仓库权限
            var warehouse = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId).Select(a => a.WarehouseName).ToList();
            //是否选择了仓库
            string WarehouseName = input.objData.warehouseName;
            string AreaName = input.objData.areaName;
            //选择了仓库就推荐库位{
            if (!string.IsNullOrEmpty(WarehouseName))
            {
                return await _rep.AsQueryable().Where(a => warehouse.Contains(a.WarehouseName) && a.WarehouseName == WarehouseName)
                    .WhereIF(!string.IsNullOrEmpty(AreaName), a => a.AreaName.Contains(AreaName)).Select(a => new SelectListItem { Text = a.AreaName, Value = a.Id.ToString() }).Distinct().ToListAsync();
            }
            else
            {
                return new List<SelectListItem>();
            }
        }
        catch (Exception)
        {

            return new List<SelectListItem>();
        }
    }



}

