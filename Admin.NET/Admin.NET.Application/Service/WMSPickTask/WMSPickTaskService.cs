using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Aliyun.OSS;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Nest;
using Org.BouncyCastle.Asn1.Cmp;
using System.Collections.Generic;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECLeagueHeadSupplierOrderGetResponse.Types.CommssionOrder.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECMerchantAddFreightTemplateRequest.Types.FreightTemplate.Types;
using System.Reflection.Emit;

namespace Admin.NET.Application;
/// <summary>
/// WMSPickTask服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSPickTaskService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPickTask> _rep;
    
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;


    private readonly UserManager _userManager;
    private readonly ISqlSugarClient _db;
    public WMSPickTaskService(SqlSugarRepository<WMSPickTask> rep, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser)
    {
        _rep = rep;
        _userManager = userManager;
        _db = db;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
    }

    /// <summary>
    /// 分页查询WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(input.PickStatus > 0, u => u.PickStatus == input.PickStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickType), u => u.PickType.Contains(input.PickType.Trim()))
                    .WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PrintPersonnel), u => u.PrintPersonnel.Contains(input.PrintPersonnel.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickPlanPersonnel), u => u.PickPlanPersonnel.Contains(input.PickPlanPersonnel.Trim()))
                    .WhereIF(input.DetailQty > 0, u => u.DetailQty == input.DetailQty)
                    .WhereIF(input.DetailKindsQty > 0, u => u.DetailKindsQty == input.DetailKindsQty)
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
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    .WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)
                    .WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                    .WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)

                    .Select<WMSPickTaskOutput>()
;
        if (input.StartTimeRange != null && input.StartTimeRange.Count > 0)
        {
            DateTime? start = input.StartTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.StartTime > start);
            if (input.StartTimeRange.Count > 1 && input.StartTimeRange[1].HasValue)
            {
                var end = input.StartTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.StartTime < end);
            }
        }
        if (input.EndTimeRange != null && input.EndTimeRange.Count > 0)
        {
            DateTime? start = input.EndTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.EndTime > start);
            if (input.EndTimeRange.Count > 1 && input.EndTimeRange[1].HasValue)
            {
                var end = input.EndTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.EndTime < end);
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
        if (input.DateTime3Range != null && input.DateTime3Range.Count > 0)
        {
            DateTime? start = input.DateTime3Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
            if (input.DateTime3Range.Count > 1 && input.DateTime3Range[1].HasValue)
            {
                var end = input.DateTime3Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4Range != null && input.DateTime4Range.Count > 0)
        {
            DateTime? start = input.DateTime4Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
            if (input.DateTime4Range.Count > 1 && input.DateTime4Range[1].HasValue)
            {
                var end = input.DateTime4Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5Range != null && input.DateTime5Range.Count > 0)
        {
            DateTime? start = input.DateTime5Range[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
            if (input.DateTime5Range.Count > 1 && input.DateTime5Range[1].HasValue)
            {
                var end = input.DateTime5Range[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSPickTaskInput input)
    {
        var entity = input.Adapt<WMSPickTask>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSPickTaskInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);
        //await _rep.FakeDeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSPickTaskInput input)
    {
        var entity = input.Adapt<WMSPickTask>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSPickTask> Get(long id)
    {
        var entity = await _rep.AsQueryable().Includes(a => a.Details).Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSPickTask列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSPickTaskOutput>> List([FromQuery] WMSPickTaskInput input)
    {
        return await _rep.AsQueryable().Select<WMSPickTaskOutput>().ToListAsync();
    }


    /// <summary>
    /// 获取WMSPickTask 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "AddPrintLog")]
    public async Task AddPrintLog(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        entity.PrintNum += 1;
        entity.PrintPersonnel = _userManager.Account;
        entity.PrintTime = DateTime.Now;
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();

    }




}
