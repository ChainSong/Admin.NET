using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSRFIDInfo服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFIDInfoService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSRFIDInfo> _rep;
    public WMSRFIDInfoService(SqlSugarRepository<WMSRFIDInfo> rep)
    {
        _rep = rep;
    }

    /// <summary>
    /// 分页查询WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFIDInfoOutput>> Page(WMSRFIDInfoInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(input.ASNDetailId > 0, u => u.ASNDetailId == input.ASNDetailId)
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptId > 0, u => u.ReceiptId == input.ReceiptId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptPerson), u => u.ReceiptPerson.Contains(input.ReceiptPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(input.PreOrderId > 0, u => u.PreOrderId == input.PreOrderId)
                    .WhereIF(input.PreOrderDetailId > 0, u => u.PreOrderDetailId == input.PreOrderDetailId)
                    .WhereIF(input.OrderDetailId > 0, u => u.OrderDetailId == input.OrderDetailId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderPerson), u => u.OrderPerson.Contains(input.OrderPerson.Trim()))
                    .WhereIF(input.Status > 0, u => u.Status == input.Status)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Sequence), u => u.Sequence.Contains(input.Sequence.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RFID), u => u.RFID.Contains(input.RFID.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Link), u => u.Link.Contains(input.Link.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPerson), u => u.PrintPerson.Contains(input.PrintPerson.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)

                    .Select<WMSRFIDInfoOutput>()
;
        if (input.ReceiptTimeRange != null && input.ReceiptTimeRange.Count > 0)
        {
            DateTime? start = input.ReceiptTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.ReceiptTime > start);
            if (input.ReceiptTimeRange.Count > 1 && input.ReceiptTimeRange[1].HasValue)
            {
                var end = input.ReceiptTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.ReceiptTime < end);
            }
        }
        if (input.OrderTimeRange != null && input.OrderTimeRange.Count > 0)
        {
            DateTime? start = input.OrderTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.OrderTime > start);
            if (input.OrderTimeRange.Count > 1 && input.OrderTimeRange[1].HasValue)
            {
                var end = input.OrderTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.OrderTime < end);
            }
        }
        if (input.PrintTimeRange != null && input.PrintTimeRange.Count > 0)
        {
            DateTime? start = input.PrintTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime > start);
            if (input.PrintTimeRange.Count > 1 && input.PrintTimeRange[1].HasValue)
            {
                var end = input.PrintTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
            }
        }
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
        if (input.DateTime1Range != null && input.DateTime1Range.Count > 0)
        {
            DateTime? start = input.DateTime1Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1Range.Count > 1 && input.DateTime1Range[1].HasValue)
            {
                var end = input.DateTime1Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2Range != null && input.DateTime2Range.Count > 0)
        {
            DateTime? start = input.DateTime2Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2Range.Count > 1 && input.DateTime2Range[1].HasValue)
            {
                var end = input.DateTime2Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSRFIDInfoInput input)
    {
        var entity = input.Adapt<WMSRFIDInfo>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    //[HttpPost]
    //[ApiDescriptionSettings(Name = "Delete")]
    //public async Task Delete(DeleteWMSRFIDInfoInput input)
    //{
    //    //var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
    //  //var entity = input.Adapt<WMSRFIDInfo>();
    //    await _rep.FakeDeleteAsync(entity);   //假删除
    //}

    /// <summary>
    /// 更新WMSRFIDInfo
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSRFIDInfoInput input)
    {
        var entity = input.Adapt<WMSRFIDInfo>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSRFIDInfo> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSRFIDInfo列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSRFIDInfoOutput>> List([FromQuery] WMSRFIDInfoInput input)
    {
        return await _rep.AsQueryable().Select<WMSRFIDInfoOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSRFIDInfo 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "QueryByReceiptId")]
    public async Task<Response<List<WMSRFIDInfo>>> GetPrinrRFIDInfoByReceiptId(List<long> receiptId)
    {
        Response<List<WMSRFIDInfo>> response = new Response<List<WMSRFIDInfo>>();
        var entity = await _rep.AsQueryable().Where(u => receiptId.Contains(u.ReceiptId.Value)).ToListAsync();
        response.Data = entity;
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
    }


}

