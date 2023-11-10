using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSInventory服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSInventoryUsableService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSInventoryUsable> _rep;
    public WMSInventoryUsableService(SqlSugarRepository<WMSInventoryUsable> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询WMSInventory
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSInventoryUsableOutput>> Page(WMSInventoryUsableInput input)
    {
        var query= _rep.AsQueryable() 
                    .WhereIF(input.ReceiptDetailId>0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptReceivingId>0, u => u.ReceiptReceivingId == input.ReceiptReceivingId)
                    .WhereIF(input.CustomerId>0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId>0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UPC), u => u.UPC.Contains(input.UPC.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(input.InventoryStatus>0, u => u.InventoryStatus == input.InventoryStatus)
                    .WhereIF(input.SuperId>0, u => u.SuperId == input.SuperId)
                    .WhereIF(input.RelatedId>0, u => u.RelatedId == input.RelatedId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UnitCode), u => u.UnitCode.Contains(input.UnitCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Onwer), u => u.Onwer.Contains(input.Onwer.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxCode), u => u.BoxCode.Contains(input.BoxCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TrayCode), u => u.TrayCode.Contains(input.TrayCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BatchCode), u => u.BatchCode.Contains(input.BatchCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(input.Int1>0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2>0, u => u.Int2 == input.Int2)

                    .Select<WMSInventoryUsableOutput>()
;
        if(input.ProductionDate != null && input.ProductionDate.Count >0)
        {
                DateTime? start= input.ProductionDate[0]; 
                query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
                if (input.ProductionDate.Count >1 && input.ProductionDate[1].HasValue)
                {
                    var end = input.ProductionDate[1].Value.AddDays(1);
                    query = query.Where(u => u.ProductionDate < end);
                }
        } 
        if(input.ExpirationDate != null && input.ExpirationDate.Count >0)
        {
                DateTime? start= input.ExpirationDate[0]; 
                query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
                if (input.ExpirationDate.Count >1 && input.ExpirationDate[1].HasValue)
                {
                    var end = input.ExpirationDate[1].Value.AddDays(1);
                    query = query.Where(u => u.ExpirationDate < end);
                }
        } 
        if(input.InventoryTime != null && input.InventoryTime.Count >0)
        {
                DateTime? start= input.InventoryTime[0]; 
                query = query.WhereIF(start.HasValue, u => u.InventoryTime > start);
                if (input.InventoryTime.Count >1 && input.InventoryTime[1].HasValue)
                {
                    var end = input.InventoryTime[1].Value.AddDays(1);
                    query = query.Where(u => u.InventoryTime < end);
                }
        } 
        if(input.CreationTime != null && input.CreationTime.Count >0)
        {
                DateTime? start= input.CreationTime[0]; 
                query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
                if (input.CreationTime.Count >1 && input.CreationTime[1].HasValue)
                {
                    var end = input.CreationTime[1].Value.AddDays(1);
                    query = query.Where(u => u.CreationTime < end);
                }
        } 
        if(input.DateTime1 != null && input.DateTime1.Count >0)
        {
                DateTime? start= input.DateTime1[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
                if (input.DateTime1.Count >1 && input.DateTime1[1].HasValue)
                {
                    var end = input.DateTime1[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime1 < end);
                }
        } 
        if(input.DateTime2 != null && input.DateTime2.Count >0)
        {
                DateTime? start= input.DateTime2[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
                if (input.DateTime2.Count >1 && input.DateTime2[1].HasValue)
                {
                    var end = input.DateTime2[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime2 < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSInventory
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSInventoryUsableInput input)
    {
        var entity = input.Adapt<WMSInventoryUsable>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSInventory
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteWMSInventoryUsableInput input)
    //{
    //    var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
    //    await _rep.Delete(entity);   //假删除
    //}

    /// <summary>
    /// 更新WMSInventory
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSInventoryUsableInput input)
    {
        var entity = input.Adapt<WMSInventoryUsable>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取WMSInventory 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSInventoryUsable> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSInventory列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSInventoryUsableOutput>> List([FromQuery] WMSInventoryUsableInput input)
    {
        return await _rep.AsQueryable().Select<WMSInventoryUsableOutput>().ToListAsync();
    }





}

