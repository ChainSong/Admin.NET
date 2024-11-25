using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// 入库点数服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSASNReceiptDetailService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASNReceiptDetail> _rep;
    public WMSASNReceiptDetailService(SqlSugarRepository<WMSASNReceiptDetail> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSASNReceiptDetailOutput>> Page(WMSASNReceiptDetailInput input)
    {
        var query= _rep.AsQueryable()
                    .WhereIF(input.Id>0, u => u.Id == input.Id)
                    .WhereIF(input.ASNId>0, u => u.ASNId == input.ASNId)
                    .WhereIF(input.ASNDetailId>0, u => u.ASNDetailId == input.ASNDetailId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId>0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId>0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LineNumber), u => u.LineNumber.Contains(input.LineNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UPC), u => u.UPC.Contains(input.UPC.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsName), u => u.GoodsName.Contains(input.GoodsName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BoxCode), u => u.BoxCode.Contains(input.BoxCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TrayCode), u => u.TrayCode.Contains(input.TrayCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.BatchCode), u => u.BatchCode.Contains(input.BatchCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.LotCode), u => u.LotCode.Contains(input.LotCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PoCode), u => u.PoCode.Contains(input.PoCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UnitCode), u => u.UnitCode.Contains(input.UnitCode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Onwer), u => u.Onwer.Contains(input.Onwer.Trim()))
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
                    .WhereIF(input.Int1>0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2>0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3>0, u => u.Int3 == input.Int3)
                    .WhereIF(input.Int4>0, u => u.Int4 == input.Int4)
                    .WhereIF(input.Int5>0, u => u.Int5 == input.Int5)

                    .Select<WMSASNReceiptDetailOutput>()
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
        if(input.DateTime3Range != null && input.DateTime3Range.Count >0)
        {
                DateTime? start= input.DateTime3Range[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
                if (input.DateTime3Range.Count >1 && input.DateTime3Range[1].HasValue)
                {
                    var end = input.DateTime3Range[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime3 < end);
                }
        } 
        if(input.DateTime4Range != null && input.DateTime4Range.Count >0)
        {
                DateTime? start= input.DateTime4Range[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
                if (input.DateTime4Range.Count >1 && input.DateTime4Range[1].HasValue)
                {
                    var end = input.DateTime4Range[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime4 < end);
                }
        } 
        if(input.DateTime5Range != null && input.DateTime5Range.Count >0)
        {
                DateTime? start= input.DateTime5Range[0]; 
                query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
                if (input.DateTime5Range.Count >1 && input.DateTime5Range[1].HasValue)
                {
                    var end = input.DateTime5Range[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime5 < end);
                }
        } 
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSASNReceiptDetailInput input)
    {
        var entity = input.Adapt<WMSASNReceiptDetail>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSASNReceiptDetailInput input)
    {
      var entity = input.Adapt<WMSASNReceiptDetail>();
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新入库点数
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSASNReceiptDetailInput input)
    {
        var entity = input.Adapt<WMSASNReceiptDetail>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

 


      /// <summary>
    /// 获取入库点数 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSASNReceiptDetail> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取入库点数列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSASNReceiptDetailOutput>> List([FromQuery] WMSASNReceiptDetailInput input)
    {
        return await _rep.AsQueryable().Select<WMSASNReceiptDetailOutput>().ToListAsync();
    }





}

