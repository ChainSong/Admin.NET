using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// MMSInventoryUsable服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class MMSInventoryUsableService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<MMSInventoryUsable> _rep;
    public MMSInventoryUsableService(SqlSugarRepository<MMSInventoryUsable> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询MMSInventoryUsable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<MMSInventoryUsableOutput>> Page(MMSInventoryUsableInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.Id>0, u => u.Id == input.Id)
                    .WhereIF(input.ReceiptReceivingId>0, u => u.ReceiptReceivingId == input.ReceiptReceivingId)
                    .WhereIF(input.ReceiptReceivingDetailId>0, u => u.ReceiptReceivingDetailId == input.ReceiptReceivingDetailId)
                    .WhereIF(input.SupplierId>0, u => u.SupplierId == input.SupplierId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SupplierName), u => u.SupplierName.Contains(input.SupplierName.Trim()))
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
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LotCode), u => u.LotCode.Contains(input.LotCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PoCode), u => u.PoCode.Contains(input.PoCode.Trim()))
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

                    .Select<MMSInventoryUsableOutput>()
;
        if(input.ProductionDateRange != null && input.ProductionDateRange.Count >0)
        {
                DateTime? start= input.ProductionDateRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
                if (input.ProductionDateRange.Count >1 && input.ProductionDateRange[1].HasValue)
                {
                    var end = input.ProductionDateRange[1].Value.AddDays(1);
                    query = query.Where(u => u.ProductionDate < end);
                }
        } 
        if(input.ExpirationDateRange != null && input.ExpirationDateRange.Count >0)
        {
                DateTime? start= input.ExpirationDateRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
                if (input.ExpirationDateRange.Count >1 && input.ExpirationDateRange[1].HasValue)
                {
                    var end = input.ExpirationDateRange[1].Value.AddDays(1);
                    query = query.Where(u => u.ExpirationDate < end);
                }
        } 
        if(input.InventoryTimeRange != null && input.InventoryTimeRange.Count >0)
        {
                DateTime? start= input.InventoryTimeRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.InventoryTime > start);
                if (input.InventoryTimeRange.Count >1 && input.InventoryTimeRange[1].HasValue)
                {
                    var end = input.InventoryTimeRange[1].Value.AddDays(1);
                    query = query.Where(u => u.InventoryTime < end);
                }
        } 
        if(input.CreationTimeRange != null && input.CreationTimeRange.Count >0)
        {
                DateTime? start= input.CreationTimeRange[0]; 
                query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
                if (input.CreationTimeRange.Count >1 && input.CreationTimeRange[1].HasValue)
                {
                    var end = input.CreationTimeRange[1].Value.AddDays(1);
                    query = query.Where(u => u.CreationTime < end);
                }
        } 
        if(input.DateTime1Range != null && input.DateTime1Range.Count >0)
        {
                DateTime? start= input.DateTime1Range[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
                if (input.DateTime1Range.Count >1 && input.DateTime1Range[1].HasValue)
                {
                    var end = input.DateTime1Range[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime1 < end);
                }
        } 
        if(input.DateTime2Range != null && input.DateTime2Range.Count >0)
        {
                DateTime? start= input.DateTime2Range[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
                if (input.DateTime2Range.Count >1 && input.DateTime2Range[1].HasValue)
                {
                    var end = input.DateTime2Range[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime2 < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加MMSInventoryUsable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddMMSInventoryUsableInput input)
    {
        var entity = input.Adapt<MMSInventoryUsable>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除MMSInventoryUsable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteMMSInventoryUsableInput input)
    {
      var entity = input.Adapt<MMSInventoryUsable>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新MMSInventoryUsable
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateMMSInventoryUsableInput input)
    {
        var entity = input.Adapt<MMSInventoryUsable>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取MMSInventoryUsable 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<MMSInventoryUsable> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取MMSInventoryUsable列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<MMSInventoryUsableOutput>> List([FromQuery] MMSInventoryUsableInput input)
    {
        return await _rep.AsQueryable().Select<MMSInventoryUsableOutput>().ToListAsync();
    }





}

