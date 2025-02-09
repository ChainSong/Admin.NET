﻿using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Admin.NET.Application;
/// <summary>
/// WMSStockCheck服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFStockCheckService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSStockCheck> _rep;
    private readonly SqlSugarRepository<WMSStockCheckDetail> _repStockCheckDetail;
    private readonly SqlSugarRepository<WMSStockCheckDetailScan> _repStockCheckDetailScan;
    private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    private readonly SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving;
    private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly UserManager _userManager;
    public WMSRFStockCheckService(SqlSugarRepository<WMSStockCheck> rep, SqlSugarRepository<WMSInventoryUsable> repInventoryUsable, SqlSugarRepository<WMSReceiptReceiving> repReceiptReceiving, SqlSugarRepository<WMSStockCheckDetail> repStockCheckDetail, SqlSugarRepository<WMSStockCheckDetailScan> repStockCheckDetailScan, UserManager userManager)
    {
        _rep = rep;
        _repInventoryUsable = repInventoryUsable;
        _repReceiptReceiving = repReceiptReceiving;
        _repStockCheckDetail = repStockCheckDetail;
        _repStockCheckDetailScan = repStockCheckDetailScan;
        _userManager = userManager;
  
    }

    /// <summary>
    /// 分页查询WMSStockCheck
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSRFStockCheckOutput>> Page(WMSRFStockCheckInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.Id > 0, u => u.Id == input.Id)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.StockCheckNumber), u => u.StockCheckNumber.Contains(input.StockCheckNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternNumber), u => u.ExternNumber.Contains(input.ExternNumber.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseId), u => u.WarehouseId.Contains(input.WarehouseId.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.StockCheckType), u => u.StockCheckType.Contains(input.StockCheckType.Trim()))
                    .WhereIF(input.StockCheckStatus != 0, u => u.StockCheckStatus == input.StockCheckStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ToCheckUser), u => u.ToCheckUser.Contains(input.ToCheckUser.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ToCheckAccount), u => u.ToCheckAccount.Contains(input.ToCheckAccount.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Remark), u => u.Remark.Contains(input.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Is_Difference), u => u.Is_Difference.Contains(input.Is_Difference.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Is_Deal), u => u.Is_Deal.Contains(input.Is_Deal.Trim()))
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
                    .Select<WMSRFStockCheckOutput>();
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }



}

