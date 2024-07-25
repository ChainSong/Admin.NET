// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Service.WMSInventoryReport.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CardCreateRequest.Types.GrouponCard.Types.Base.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECMerchantAddFreightTemplateRequest.Types.FreightTemplate.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ChannelsECWarehouseGetResponse.Types;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateContainerServiceRequest.Types;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using System.Reflection.Emit;
using Admin.NET.Application.Service.WMSInventoryReport.Dto;
using Admin.NET.Application.Dtos.Enum;
using System.Data;
using System.Reflection;

namespace Admin.NET.Application.Service.WMSInventoryReport.Strategy;
public class InventoryStrategy : IInvrntoryInterface
{
    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }

    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

    public async Task<SqlSugarPagedList<WMSInventoryUsableOutput>> InvrntoryData(WMSInventoryUsableInput input)
    {

        Response<WMSInventoryUsableOutput> response = new Response<WMSInventoryUsableOutput>();

        var query = _repInventoryUsable.AsQueryable()
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptReceivingId > 0, u => u.ReceiptReceivingId == input.ReceiptReceivingId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UPC), u => u.UPC.Contains(input.UPC.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(input.InventoryStatus > 0, u => u.InventoryStatus == input.InventoryStatus)
                    .WhereIF(input.SuperId > 0, u => u.SuperId == input.SuperId)
                    .WhereIF(input.RelatedId > 0, u => u.RelatedId == input.RelatedId)
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
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
        //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
        //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSInventoryUsableOutput>()
;
        if (input.ProductionDate != null && input.ProductionDate.Count > 0)
        {
            DateTime? start = input.ProductionDate[0];
            query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
            if (input.ProductionDate.Count > 1 && input.ProductionDate[1].HasValue)
            {
                var end = input.ProductionDate[1].Value.AddDays(1);
                query = query.Where(u => u.ProductionDate < end);
            }
        }
        if (input.ExpirationDate != null && input.ExpirationDate.Count > 0)
        {
            DateTime? start = input.ExpirationDate[0];
            query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
            if (input.ExpirationDate.Count > 1 && input.ExpirationDate[1].HasValue)
            {
                var end = input.ExpirationDate[1].Value.AddDays(1);
                query = query.Where(u => u.ExpirationDate < end);
            }
        }
        if (input.InventoryTime != null && input.InventoryTime.Count > 0)
        {
            DateTime? start = input.InventoryTime[0];
            query = query.WhereIF(start.HasValue, u => u.InventoryTime > start);
            if (input.InventoryTime.Count > 1 && input.InventoryTime[1].HasValue)
            {
                var end = input.InventoryTime[1].Value.AddDays(1);
                query = query.Where(u => u.InventoryTime < end);
            }
        }
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
        if (input.DateTime1 != null && input.DateTime1.Count > 0)
        {
            DateTime? start = input.DateTime1[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
            {
                var end = input.DateTime1[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2 != null && input.DateTime2.Count > 0)
        {
            DateTime? start = input.DateTime2[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
            {
                var end = input.DateTime2[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
        //throw new NotImplementedException();


    }


    public async Task<SqlSugarPagedList<WMSInventoryUsableReport>> InvrntoryDataPage(WMSInventoryUsableInput input)
    {

        SqlSugarPagedList<WMSInventoryUsableReport> response = new SqlSugarPagedList<WMSInventoryUsableReport>();
        var query = _repInventoryUsable.AsQueryable()
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptReceivingId > 0, u => u.ReceiptReceivingId == input.ReceiptReceivingId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UPC), u => u.UPC.Contains(input.UPC.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(input.InventoryStatus > 0, u => u.InventoryStatus == input.InventoryStatus)
                    .WhereIF(input.SuperId > 0, u => u.SuperId == input.SuperId)
                    .WhereIF(input.RelatedId > 0, u => u.RelatedId == input.RelatedId)
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
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                     //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                     //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                     .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                     .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                     .GroupBy(a => new
                     {
                         a.CustomerId
                        ,
                         a.CustomerName
                        ,
                         a.WarehouseId
                        ,
                         a.WarehouseName
                        ,
                         a.Area
                        ,
                         a.Location
                        ,
                         a.SKU
                        ,
                         a.UPC
                        ,
                         a.GoodsType
                        ,
                         a.InventoryStatus
                        ,
                         a.SuperId
                        ,
                         a.RelatedId
                        ,
                         a.GoodsName
                        ,
                         a.UnitCode
                        ,
                         a.Onwer
                        ,
                         a.BoxCode
                        ,
                         a.TrayCode
                        ,
                         a.BatchCode
                        ,
                         a.LotCode
                        ,
                         a.PoCode
                        ,
                         a.Weight
                        ,
                         a.Volume
                        ,
                         a.ProductionDate
                        ,
                         a.ExpirationDate
                        ,
                         a.Remark
                     })
                    .Select(a => new WMSInventoryUsableReport
                    {
                        CustomerId = a.CustomerId
                     ,
                        CustomerName = a.CustomerName
                     ,
                        WarehouseId = a.WarehouseId
                     ,
                        WarehouseName = a.WarehouseName
                     ,
                        Area = a.Area
                     ,
                        Location = a.Location
                     ,
                        SKU = a.SKU
                     ,
                        UPC = a.UPC
                     ,
                        GoodsType = a.GoodsType
                     ,
                        InventoryStatus = a.InventoryStatus
                     ,
                        SuperId = a.SuperId
                     ,
                        RelatedId = a.RelatedId
                     ,
                        GoodsName = a.GoodsName
                     ,
                        UnitCode = a.UnitCode
                     ,
                        Onwer = a.Onwer
                     ,
                        BoxCode = a.BoxCode
                     ,
                        TrayCode = a.TrayCode
                     ,
                        BatchCode = a.BatchCode
                     ,
                        LotCode = a.LotCode
                     ,
                        PoCode = a.PoCode
                     ,
                        Weight = a.Weight
                     ,
                        Volume = a.Volume
                     ,
                        Qty = SqlFunc.AggregateSum(a.Qty)
                     ,
                        ProductionDate = a.ProductionDate
                     ,
                        ExpirationDate = a.ExpirationDate
                     ,
                        Remark = a.Remark,
                        //,
                        //Id = SqlFunc.AggregateMax(a.Id)

                    })
                   ;

        ;
        if (input.ProductionDate != null && input.ProductionDate.Count > 0)
        {
            DateTime? start = input.ProductionDate[0];
            query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
            if (input.ProductionDate.Count > 1 && input.ProductionDate[1].HasValue)
            {
                var end = input.ProductionDate[1].Value.AddDays(1);
                query = query.Where(u => u.ProductionDate < end);
            }
        }
        if (input.ExpirationDate != null && input.ExpirationDate.Count > 0)
        {
            DateTime? start = input.ExpirationDate[0];
            query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
            if (input.ExpirationDate.Count > 1 && input.ExpirationDate[1].HasValue)
            {
                var end = input.ExpirationDate[1].Value.AddDays(1);
                query = query.Where(u => u.ExpirationDate < end);
            }
        }
        //if (input.InventoryTime != null && input.InventoryTime.Count > 0)
        //{
        //    DateTime? start = input.InventoryTime[0];
        //    query = query.WhereIF(start.HasValue, u => u.InventoryTime > start);
        //    if (input.InventoryTime.Count > 1 && input.InventoryTime[1].HasValue)
        //    {
        //        var end = input.InventoryTime[1].Value.AddDays(1);
        //        query = query.Where(u => u.InventoryTime < end);
        //    }
        //}
        //if (input.CreationTime != null && input.CreationTime.Count > 0)
        //{
        //    DateTime? start = input.CreationTime[0];
        //    query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
        //    if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
        //    {
        //        var end = input.CreationTime[1].Value.AddDays(1);
        //        query = query.Where(u => u.CreationTime < end);
        //    }
        //}
        //if (input.DateTime1 != null && input.DateTime1.Count > 0)
        //{
        //    DateTime? start = input.DateTime1[0];
        //    query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
        //    if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
        //    {
        //        var end = input.DateTime1[1].Value.AddDays(1);
        //        query = query.Where(u => u.DateTime1 < end);
        //    }
        //}
        //if (input.DateTime2 != null && input.DateTime2.Count > 0)
        //{
        //    DateTime? start = input.DateTime2[0];
        //    query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
        //    if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
        //    {
        //        var end = input.DateTime2[1].Value.AddDays(1);
        //        query = query.Where(u => u.DateTime2 < end);
        //    }
        //}

        //input.Field = "Location";
        //query = query.OrderBuilder(input);
        //try
        //{

        //var count = query.Count();
        //response.Items = await query.Take((input.PageSize - 1) * input.Page).Skip(input.PageSize).ToListAsync();
        //response.Page = input.Page;
        //response.PageSize = input.PageSize;
        //response.Total = count;
        //response.TotalPages =(int)Math.Ceiling(count/(input.PageSize + 0.00));
        //return response;
        return await query.ToPagedListAsync(input.Page, input.PageSize);
        //throw new NotImplementedException();\
        //}
        //catch (Exception asdasd)
        //{

        //    throw;
        //}


    }




    public Response<DataTable> InvrntoryDataExport(WMSInventoryUsableInput input)
    {

        Response<DataTable> response = new Response<DataTable>();
        var query = _repInventoryUsable.AsQueryable()
                    .WhereIF(input.ReceiptDetailId > 0, u => u.ReceiptDetailId == input.ReceiptDetailId)
                    .WhereIF(input.ReceiptReceivingId > 0, u => u.ReceiptReceivingId == input.ReceiptReceivingId)
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    .WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Area), u => u.Area.Contains(input.Area.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Location), u => u.Location.Contains(input.Location.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.SKU), u => u.SKU.Contains(input.SKU.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UPC), u => u.UPC.Contains(input.UPC.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.GoodsType), u => u.GoodsType.Contains(input.GoodsType.Trim()))
                    .WhereIF(input.InventoryStatus > 0, u => u.InventoryStatus == input.InventoryStatus)
                    .WhereIF(input.SuperId > 0, u => u.SuperId == input.SuperId)
                    .WhereIF(input.RelatedId > 0, u => u.RelatedId == input.RelatedId)
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
                    .WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    .WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
        //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
        //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                     .GroupBy(a => new
                     {
                         a.CustomerId
                        ,
                         a.CustomerName
                        ,
                         a.WarehouseId
                        ,
                         a.WarehouseName
                        ,
                         a.Area
                        ,
                         a.Location
                        ,
                         a.SKU
                        ,
                         a.UPC
                        ,
                         a.GoodsType
                        ,
                         a.InventoryStatus
                        ,
                         a.SuperId
                        ,
                         a.RelatedId
                        ,
                         a.GoodsName
                        ,
                         a.UnitCode
                        ,
                         a.Onwer
                        ,
                         a.BoxCode
                        ,
                         a.TrayCode
                        ,
                         a.BatchCode
                        ,
                         a.LotCode
                        ,
                         a.PoCode
                        ,
                         a.Weight
                        ,
                         a.Volume
                        ,
                         a.ProductionDate
                        ,
                         a.ExpirationDate
                        ,
                         a.Remark
                     })
                    .Select(a => new WMSInventoryUsableReport
                    {
                        CustomerId = a.CustomerId
                     ,
                        CustomerName = a.CustomerName
                     ,
                        WarehouseId = a.WarehouseId
                     ,
                        WarehouseName = a.WarehouseName
                     ,
                        Area = a.Area
                     ,
                        Location = a.Location
                     ,
                        SKU = a.SKU
                     ,
                        UPC = a.UPC
                     ,
                        GoodsType = a.GoodsType
                     ,
                        InventoryStatus = a.InventoryStatus
                     ,
                        SuperId = a.SuperId
                     ,
                        RelatedId = a.RelatedId
                     ,
                        GoodsName = a.GoodsName
                     ,
                        UnitCode = a.UnitCode
                     ,
                        Onwer = a.Onwer
                     ,
                        BoxCode = a.BoxCode
                     ,
                        TrayCode = a.TrayCode
                     ,
                        BatchCode = a.BatchCode
                     ,
                        LotCode = a.LotCode
                     ,
                        PoCode = a.PoCode
                     ,
                        Weight = a.Weight
                     ,
                        Volume = a.Volume
                     ,
                        Qty = SqlFunc.AggregateSum(a.Qty)
                     ,
                        ProductionDate = a.ProductionDate
                     ,
                        ExpirationDate = a.ExpirationDate
                     ,
                        Remark = a.Remark,
                        //,
                        //Id = SqlFunc.AggregateMax(a.Id)

                    })
                   ;

        ;
        if (input.ProductionDate != null && input.ProductionDate.Count > 0)
        {
            DateTime? start = input.ProductionDate[0];
            query = query.WhereIF(start.HasValue, u => u.ProductionDate > start);
            if (input.ProductionDate.Count > 1 && input.ProductionDate[1].HasValue)
            {
                var end = input.ProductionDate[1].Value.AddDays(1);
                query = query.Where(u => u.ProductionDate < end);
            }
        }
        if (input.ExpirationDate != null && input.ExpirationDate.Count > 0)
        {
            DateTime? start = input.ExpirationDate[0];
            query = query.WhereIF(start.HasValue, u => u.ExpirationDate > start);
            if (input.ExpirationDate.Count > 1 && input.ExpirationDate[1].HasValue)
            {
                var end = input.ExpirationDate[1].Value.AddDays(1);
                query = query.Where(u => u.ExpirationDate < end);
            }
        }

        var tableColumn = GetColumns("WMS_Inventory_Usable_Report");
        //var detailTableColumn = GetColumns("WMS_ReceiptReceiving");
        var inventoryData = query.ToListAsync();
        //var receiptData = _repReceipt.AsQueryable().Includes(a => a.Details).Includes(b => b.ReceiptReceivings).Where(a => request.Contains(a.Id)).ToListAsync();
        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        //1，构建主表需要的信息
        tableColumn.ForEach(a =>
        {
            if (a.IsImportColumn == 1)
            {
                dc = dt.Columns.Add(a.DisplayName, typeof(string));
            }
        });
        //2.构建明细需要的信息
        //detailTableColumn.ForEach(a =>
        //{
        //    if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
        //    {
        //        dc = dt.Columns.Add(a.DisplayName, typeof(string));
        //    }
        //});

        //塞数据
        inventoryData.Result.ForEach(a =>
        {

            //判断是采用上架表数据，还是使用入库明细数据(上架表有数据，就采用上架表数据，否则采用入库明细数据)
            //采用入库明细数据需要推荐上架库位
            Type receiptType = a.GetType();


            DataRow row = dt.NewRow();
            try
            {

                tableColumn.ForEach(h =>
                {
                    if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                    {
                        PropertyInfo property = receiptType.GetProperty(h.DbColumnName);
                        //PropertyInfo property = preOrderType.GetProperty(h.DbColumnName);
                        //如果该字段有下拉选项，则值取下拉选项中的值
                        if (h.tableColumnsDetails.Count() > 0)
                        {
                            var val = property.GetValue(a);
                            TableColumnsDetail data = new TableColumnsDetail();
                            if (val is int)
                            {
                                data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                            }
                            else
                            {
                                data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val)).FirstOrDefault();

                            }
                            //var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                            if (data != null)
                            {
                                row[h.DisplayName] = data.Name;
                            }
                            else
                            {
                                row[h.DisplayName] = "";
                            }
                        }
                        else
                        {
                            row[h.DisplayName] = property.GetValue(a);
                        }

                        //if (property != null)
                        //{
                        //    var val = property.GetValue(a);
                        //    TableColumnsDetail data = new TableColumnsDetail();
                        //    if (val is int)
                        //    {
                        //        data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                        //    }
                        //    else
                        //    {
                        //        data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val)).FirstOrDefault();

                        //    }
                        //    // 判断是下拉列表 就取下拉列表的数据
                        //    //if (h.Type == "DropDownListInt")
                        //    //{
                        //    //    try
                        //    //    {
                        //    //        row[h.DisplayName] = _repTableColumnsDetail.AsQueryable().Where(q => q.Associated == h.Associated &&q.CodeInt== property.GetValue(a)).First().Name;
                        //    //    }
                        //    //    catch (Exception e)
                        //    //    {
                        //    //        row[h.DisplayName] = property.GetValue(a);
                        //    //    }

                        //    //}
                        //    //else
                        //    //{
                        //    //    row[h.DisplayName] = property.GetValue(a);
                        //    //}
                        //}
                        //else
                        //{

                        //    row[h.DisplayName] = "";

                        //}
                    }
                });
            }
            catch (Exception e)
            {

            }
            dt.Rows.Add(row);



        });
        response.Data = dt;

        //response.Data = await query.ToListAsync();
        response.Code = StatusCode.Success;
        return response;
        //throw new NotImplementedException();\
        //}
        //catch (Exception asdasd)
        //{

        //    throw;
        //}


    }


    private List<TableColumns> GetColumns(string TableName)
    {
        return _repTableColumns.AsQueryable()
           .Where(a => a.TableName == TableName &&
             a.TenantId == _userManager.TenantId &&
             a.IsImportColumn == 1
           )
          .Select(a => new TableColumns
          {
              DisplayName = a.DisplayName,
              //由于框架约定大于配置， 数据库的字段首字母小写
              //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
              DbColumnName = a.DbColumnName,
              Type = a.Type,
              IsImportColumn = a.IsImportColumn,
              Validation = a.Validation,
              tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
              //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
              //.Select()
          }).ToList();
    }
}
