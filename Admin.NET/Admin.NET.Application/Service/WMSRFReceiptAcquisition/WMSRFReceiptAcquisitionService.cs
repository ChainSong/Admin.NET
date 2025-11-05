using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Microsoft.AspNetCore.Identity;
using NewLife;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Admin.NET.Application;
/// <summary>
/// WMSRFReceiptAcquisition服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFReceiptAcquisitionService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSRFReceiptAcquisition> _rep;

    private readonly SqlSugarRepository<WMSProduct> _repProduct;


    private readonly SqlSugarRepository<WMSReceipt> _repReceipt;

    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;

    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;

    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly UserManager _userManager;

    public WMSRFReceiptAcquisitionService(SqlSugarRepository<WMSRFReceiptAcquisition> rep, SqlSugarRepository<WMSReceipt> repReceipt, SqlSugarRepository<WMSReceiptDetail> repReceiptDetail, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<WMSProduct> repProduct, SqlSugarRepository<WMSCustomer> repCustomer)
    {
        _rep = rep;
        _repReceipt = repReceipt;
        _repReceiptDetail = repReceiptDetail;
        _repCustomerUser = repCustomerUser;
        _repWarehouseUser = repWarehouseUser;
        _userManager = userManager;
        _repProduct = repProduct;

    }

    /// <summary>
    /// 分页查询WMSRFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFReceiptAcquisitionOutput>> Page(WMSRFReceiptAcquisitionInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ASNId > 0, u => u.ASNId == input.ASNId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ASNNumber), u => u.ASNNumber.Contains(input.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ReceiptNumber), u => u.ReceiptNumber.Contains(input.ReceiptNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(input.ExternReceiptNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Lot), u => u.Lot.Contains(input.Lot.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<WMSRFReceiptAcquisitionOutput>()
;
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
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }


    /// <summary>
    /// 增加WMSRFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "All")]
    public async Task<List<WMSReceipt>> All(AddWMSRFReceiptAcquisitionInput input)
    {
        return await _repReceipt.AsQueryable()
               .WhereIF(!string.IsNullOrEmpty(input.ExternReceiptNumber), u => u.ExternReceiptNumber == input.ExternReceiptNumber)
               .WhereIF(!string.IsNullOrEmpty(input.ReceiptNumber), u => u.ReceiptNumber == input.ReceiptNumber)
               .Where(a => a.ReceiptStatus == (int)ReceiptStatusEnum.新增)
               .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
               .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
            .ToListAsync();
    }

    /// <summary>
    /// 增加WMSReceipt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "SaveAcquisitionData")]
    public async Task<Response> SaveAcquisitionData(AddWMSRFReceiptAcquisitionInput input)
    {
        try
        {


            Response response = new Response();
            var getReceipt = await _repReceipt.AsQueryable().Where(a => a.ReceiptNumber == input.ReceiptNumber).FirstAsync();
            input.ASNId = getReceipt.ASNId;
            input.ASNNumber = getReceipt.ASNNumber;
            input.ReceiptNumber = getReceipt.ReceiptNumber;
            input.CustomerId = getReceipt.CustomerId;
            input.CustomerName = getReceipt.CustomerName;
            input.WarehouseId = getReceipt.WarehouseId;
            input.ReceiptDetailId = getReceipt.Id;
            input.ReceiptAcquisitionStatus = (int)ReceiptAcquisitionStatusEnum.新增;
            input.WarehouseName = getReceipt.WarehouseName;
            //var dateStr = "";
            //if (input.ExpirationDate.Contains("EXP"))
            //{
            //    //获取前两位是日期
            //    var date = input.ExpirationDate.Replace("EXP", "").ToString().Substring(0, 2);
            //    //获取后两位是年份
            //    var year = input.ExpirationDate.Replace("EXP", "").ToString().Substring(input.ExpirationDate.Replace("EXP", "").Length - 2, 2);


            //}
            string pattern = @"([a-zA-Z]+)|([0-9]+)"; // 正则表达式匹配英文字符或数字

            string skuRegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)|^[^|][^\s:]+(?=\s|$)"; // 正则表达式匹配英文字符或数字
            string lotRegex = @"(?<=\|LOT)[^\|]+|(?<==\d{3}|=\d{4}|=\d{4}CN)[0-9]{5}\b|(?<=\s)[A-Z0-9]{1,5}\b";
            string expirationDateRegex = @"(?<=\|EXP)[^\|]+|(?<=\s)\d{6}\b";
            MatchCollection matches = Regex.Matches(input.ExpirationDate.Replace("EXP", ""), pattern);
            string dateStr = "";
            if (matches.Count > 2)
            {
                  dateStr = matches[2].Value + matches[1].Value + matches[0].Value;
            }
            //foreach (Match match in matches)
            //{
            //    Console.WriteLine(match.Value); // 输出匹配到的连续英文字符和数字  
            //}
            input.ExpirationDate = !string.IsNullOrEmpty(input.ExpirationDate) ? DateTime.Parse(dateStr).ToString() : null;
            input.Lot = !string.IsNullOrEmpty(input.Lot) ? input.Lot : "";
            input.SKU = input.SKU != "" ? input.SKU.Replace("ITM", "") : "";



            var entity = input.Adapt<WMSRFReceiptAcquisition>();
            if (!string.IsNullOrEmpty(entity.SN))
            {
                entity.Qty = 1;
            }
            else
            {
                entity.Qty = 0;
            }
            //var dasd=input.ExpirationDate.re
            //先删除已经存在的SN 
            await _rep.DeleteAsync(a => a.SKU == entity.SKU && a.SN == entity.SN && a.CustomerId == entity.CustomerId && !string.IsNullOrEmpty(a.SN));
            await _rep.InsertAsync(entity);
            await _repReceiptDetail.UpdateAsync(a => new WMSReceiptDetail { ExpirationDate = input.ExpirationDate.ToDateTime(), LotCode = input.Lot }, (a => a.SKU == input.SKU && a.ReceiptNumber == input.ReceiptNumber));
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
        }
        catch (Exception ex)
        {

            throw Oops.Oh(ex.Message);
        }
    }


    /// <summary>
    /// 增加WMSRFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddWMSRFReceiptAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFReceiptAcquisition>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除WMSRFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSRFReceiptAcquisitionInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新WMSRFReceiptAcquisition
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSRFReceiptAcquisitionInput input)
    {
        var entity = input.Adapt<WMSRFReceiptAcquisition>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSRFReceiptAcquisition 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSRFReceiptAcquisition> Get(long id)
    {
        var entity = await _rep.AsQueryable().Where(u => u.Id == id).FirstAsync();
        return entity;
    }

    /// <summary>
    /// 获取WMSRFReceiptAcquisition列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSRFReceiptAcquisitionOutput>> List([FromQuery] WMSRFReceiptAcquisitionInput input)
    {
        return await _rep.AsQueryable().Select<WMSRFReceiptAcquisitionOutput>().ToListAsync();
    }





}

