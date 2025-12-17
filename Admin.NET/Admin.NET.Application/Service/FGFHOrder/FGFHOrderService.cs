using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// 福光出库回传服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class FGFHOrderService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<FGFHOrder> _rep;
    public FGFHOrderService(SqlSugarRepository<FGFHOrder> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询福光出库回传
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<FGFHOrderOutput>> Page(FGFHOrderInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.OutStockMID > 0, u => u.OutStockMID == input.OutStockMID)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OutStockNo), u => u.OutStockNo.Contains(input.OutStockNo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OutStockNumber), u => u.OutStockNumber.Contains(input.OutStockNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OutStockType), u => u.OutStockType.Contains(input.OutStockType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DownloadURL), u => u.DownloadURL.Contains(input.DownloadURL.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ContractNo), u => u.ContractNo.Contains(input.ContractNo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Addressee), u => u.Addressee.Contains(input.Addressee.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Phone), u => u.Phone.Contains(input.Phone.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Address), u => u.Address.Contains(input.Address.Trim()))
                    .WhereIF(input.IsReturn != null, u => u.IsReturn == input.IsReturn)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Reason), u => u.Reason.Contains(input.Reason.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)

                    .Select<FGFHOrderOutput>()
;
        if (input.OutStockDateRange != null && input.OutStockDateRange.Count > 0)
        {
            DateTime? start = input.OutStockDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.OutStockDate > start);
            if (input.OutStockDateRange.Count > 1 && input.OutStockDateRange[1].HasValue)
            {
                var end = input.OutStockDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.OutStockDate < end);
            }
        }
        if (input.ReturnDateRange != null && input.ReturnDateRange.Count > 0)
        {
            DateTime? start = input.ReturnDateRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReturnDate > start);
            if (input.ReturnDateRange.Count > 1 && input.ReturnDateRange[1].HasValue)
            {
                var end = input.ReturnDateRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReturnDate < end);
            }
        }
        if (input.Datetime1Range != null && input.Datetime1Range.Count > 0)
        {
            DateTime? start = input.Datetime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.Datetime1 > start);
            if (input.Datetime1Range.Count > 1 && input.Datetime1Range[1].HasValue)
            {
                var end = input.Datetime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.Datetime1 < end);
            }
        }
        if (input.Datetime2Range != null && input.Datetime2Range.Count > 0)
        {
            DateTime? start = input.Datetime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.Datetime2 > start);
            if (input.Datetime2Range.Count > 1 && input.Datetime2Range[1].HasValue)
            {
                var end = input.Datetime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.Datetime2 < end);
            }
        }
        if (input.Datetime3Range != null && input.Datetime3Range.Count > 0)
        {
            DateTime? start = input.Datetime3Range[0];
            query = query.WhereIF(start.HasValue, u => u.Datetime3 > start);
            if (input.Datetime3Range.Count > 1 && input.Datetime3Range[1].HasValue)
            {
                var end = input.Datetime3Range[1].Value.AddDays(1);
                query = query.Where(u => u.Datetime3 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加福光出库回传
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddFGFHOrderInput input)
    {
        var entity = input.Adapt<FGFHOrder>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除福光出库回传
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteFGFHOrderInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.ID) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        entity.IsReturn = 0;
        //var entity = input.Adapt<FGFHOrder>();
        await _rep.UpdateAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新福光出库回传
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateFGFHOrderInput input)
    {
        var entity = input.Adapt<FGFHOrder>();
        entity.IsReturn = 0;
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取福光出库回传 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<FGFHOrder> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取福光出库回传列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<FGFHOrderOutput>> List([FromQuery] FGFHOrderInput input)
    {
        return await _rep.AsQueryable().Select<FGFHOrderOutput>().ToListAsync();
    }





}

