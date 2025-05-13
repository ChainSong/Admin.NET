// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using System.Reflection;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;

namespace Admin.NET.Application;
public class OrderExportDefaultStrategy :  IOrderExcelInterface //ExportBaseStrategy
{

    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

    public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public   UserManager _userManager { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public   SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }

    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }

    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }


    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }

    //= new List<string>() { "WMS_Order", "WMS_OrderAddress", "WMS_Package", "WMS_PackageDetail" };


    public OrderExportDefaultStrategy()// : base()
    {

    }
    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Response<DataTable> Export(WMSOrderExcellInput request)
    {
        Response<DataTable> response = new Response<DataTable>();
        //CreateOrUpdateWMS_ReceiptInput orders = new CreateOrUpdateWMS_ReceiptInput();
        //orders.WMS_Receipts = new List<WMSReceiptEditDto>();
        //_tableNames.Add("WMS_Order");
        //_tableNames.Add("WMS_OrderDetail"); 
        var headerTableColumn = GetExportColumns("WMS_Order");
        var detailTableColumn = GetExportColumns("WMS_OrderDetail");
        var orderData = _repOrder.AsQueryable().Includes(a => a.Details)
                     .WhereIF(request.PreOrderId > 0, u => u.PreOrderId == request.PreOrderId)
                    .WhereIF(request.CustomerId.HasValue && request.CustomerId > 0, u => u.CustomerId == request.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.CustomerName), u => u.CustomerName.Contains(request.CustomerName.Trim()))
                    .WhereIF(request.WarehouseId.HasValue && request.WarehouseId > 0, u => u.WarehouseId == request.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.WarehouseName), u => u.WarehouseName.Contains(request.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.OrderType), u => u.OrderType.Contains(request.OrderType.Trim()))
                    .WhereIF(request.OrderStatus.HasValue && request.OrderStatus != 0, u => u.OrderStatus == request.OrderStatus)
                     .WhereIF(!string.IsNullOrWhiteSpace(request.Po), u => u.Po.Contains(request.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.So), u => u.So.Contains(request.So.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Creator), u => u.Creator.Contains(request.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Updator), u => u.Updator.Contains(request.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Remark), u => u.Remark.Contains(request.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str1), u => u.Str1.Contains(request.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str2), u => u.Str2.Contains(request.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str3), u => u.Str3.Contains(request.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str4), u => u.Str4.Contains(request.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str5), u => u.Str5.Contains(request.Str5.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str6), u => u.Str6.Contains(request.Str6.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str7), u => u.Str7.Contains(request.Str7.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str8), u => u.Str8.Contains(request.Str8.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str9), u => u.Str9.Contains(request.Str9.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str10), u => u.Str10.Contains(request.Str10.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str11), u => u.Str11.Contains(request.Str11.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str12), u => u.Str12.Contains(request.Str12.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str13), u => u.Str13.Contains(request.Str13.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str14), u => u.Str14.Contains(request.Str14.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str15), u => u.Str15.Contains(request.Str15.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str16), u => u.Str16.Contains(request.Str16.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str17), u => u.Str17.Contains(request.Str17.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str18), u => u.Str18.Contains(request.Str18.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str19), u => u.Str19.Contains(request.Str19.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str20), u => u.Str20.Contains(request.Str20.Trim()))
                    .WhereIF(request.Int1 > 0, u => u.Int1 == request.Int1)
                    .WhereIF(request.Int2 > 0, u => u.Int2 == request.Int2)
                    .WhereIF(request.Int3 > 0, u => u.Int3 == request.Int3)
                    .WhereIF(request.Int4 > 0, u => u.Int4 == request.Int4)
                    .WhereIF(request.Int5 > 0, u => u.Int5 == request.Int5)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                      .WhereIF(request.Ids != null && request.Ids.Count > 0, u => request.Ids.Contains(u.Id))
                    .Select<WMSOrder>();



        if (request.PreOrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (request.PreOrderNumber.IndexOf("\n") > 0)
            {
                numbers = request.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (request.PreOrderNumber.IndexOf(',') > 0)
            {
                numbers = request.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.PreOrderNumber), u => numbers.Contains(u.PreOrderNumber.Trim()));

            }
            else
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.PreOrderNumber), u => u.PreOrderNumber.Contains(request.PreOrderNumber.Trim()));
            }
        }

        if (request.OrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (request.OrderNumber.IndexOf("\n") > 0)
            {
                numbers = request.OrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (request.OrderNumber.IndexOf(',') > 0)
            {
                numbers = request.OrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.OrderNumber), u => numbers.Contains(u.OrderNumber.Trim()));

            }
            else
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.OrderNumber), u => u.OrderNumber.Contains(request.OrderNumber.Trim()));
            }
        }
        if (request.ExternOrderNumber != null)
        {
            IEnumerable<string> numbers = Enumerable.Empty<string>();
            if (request.ExternOrderNumber.IndexOf("\n") > 0)
            {
                numbers = request.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
            }
            if (request.ExternOrderNumber.IndexOf(',') > 0)
            {
                numbers = request.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
            }
            if (numbers != null && numbers.Any())
            {
                numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
            }
            if (numbers != null && numbers.Any())
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternOrderNumber), u => numbers.Contains(u.ExternOrderNumber.Trim()));

            }
            else
            {
                orderData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternOrderNumber), u => u.ExternOrderNumber.Contains(request.ExternOrderNumber.Trim()));
            }
        }
        if (request.OrderTime != null && request.OrderTime.Count > 0)
        {
            DateTime? start = request.OrderTime[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.OrderTime >= start);
            if (request.OrderTime.Count > 1 && request.OrderTime[1].HasValue)
            {
                var end = request.OrderTime[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.OrderTime < end);
            }
        }
        if (request.CompleteTime != null && request.CompleteTime.Count > 0)
        {
            DateTime? start = request.CompleteTime[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.CompleteTime >= start);
            if (request.CompleteTime.Count > 1 && request.CompleteTime[1].HasValue)
            {
                var end = request.CompleteTime[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.CompleteTime < end);
            }
        }
        if (request.CreationTime != null && request.CreationTime.Count > 0)
        {
            DateTime? start = request.CreationTime[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.CreationTime >= start);
            if (request.CreationTime.Count > 1 && request.CreationTime[1].HasValue)
            {
                var end = request.CreationTime[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.CreationTime < end);
            }
        }
        if (request.DateTime1 != null && request.DateTime1.Count > 0)
        {
            DateTime? start = request.DateTime1[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.DateTime1 >= start);
            if (request.DateTime1.Count > 1 && request.DateTime1[1].HasValue)
            {
                var end = request.DateTime1[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.DateTime1 < end);
            }
        }
        if (request.DateTime2 != null && request.DateTime2.Count > 0)
        {
            DateTime? start = request.DateTime2[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.DateTime2 >= start);
            if (request.DateTime2.Count > 1 && request.DateTime2[1].HasValue)
            {
                var end = request.DateTime2[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.DateTime2 < end);
            }
        }
        if (request.DateTime3 != null && request.DateTime3.Count > 0)
        {
            DateTime? start = request.DateTime3[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.DateTime3 >= start);
            if (request.DateTime3.Count > 1 && request.DateTime3[1].HasValue)
            {
                var end = request.DateTime3[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.DateTime3 < end);
            }
        }
        if (request.DateTime4 != null && request.DateTime4.Count > 0)
        {
            DateTime? start = request.DateTime4[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.DateTime4 >= start);
            if (request.DateTime4.Count > 1 && request.DateTime4[1].HasValue)
            {
                var end = request.DateTime4[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.DateTime4 < end);
            }
        }
        if (request.DateTime5 != null && request.DateTime5.Count > 0)
        {
            DateTime? start = request.DateTime5[0];
            orderData = orderData.WhereIF(start.HasValue, u => u.DateTime5 >= start);
            if (request.DateTime5.Count > 1 && request.DateTime5[1].HasValue)
            {
                var end = request.DateTime5[1].Value.AddDays(1);
                orderData = orderData.Where(u => u.DateTime5 < end);
            }
        }

        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        //1，构建主表需要的信息
        headerTableColumn.ForEach(a =>
        {
            if (a.IsImportColumn == 1 || a.IsKey == 1)
            {
                dc = dt.Columns.Add(a.DisplayName, typeof(string));
            }
        });
        //2.构建明细需要的信息
        detailTableColumn.ForEach(a =>
        {
            if ((a.IsImportColumn == 1 || a.IsKey == 1) && !dt.Columns.Contains(a.DisplayName))
            {
                dc = dt.Columns.Add(a.DisplayName, typeof(string));
            }
        });
        ////1，构建主表需要的信息
        //foreach (var item in )
        //{
        //    if (item.IsImportColumn == 1)
        //    {
        //        dc = dt.Columns.Add(item.DisplayName, typeof(string));
        //    }
        //}
        ////2.构建明细需要的信息
        //foreach (var item in detailTableColumn)
        //{
        //    if (item.IsImportColumn == 1 && !dt.Columns.Contains(item.DisplayName))
        //    {
        //        dc = dt.Columns.Add(item.DisplayName, typeof(string));
        //    }
        //}


        //塞数据
        orderData.ForEach(a =>
        {

            Type orderType = a.GetType();
            a.Details.ForEach(c =>
            {
                DataRow row = dt.NewRow();
                Type orderDetailType = c.GetType();
                headerTableColumn.ForEach(h =>
                {
                    if ((h.IsImportColumn == 1 || h.IsKey == 1) && dt.Columns.Contains(h.DisplayName))
                    {
                        PropertyInfo property = orderType.GetProperty(h.DbColumnName);
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

                    }
                });

                detailTableColumn.ForEach(d =>
                {
                    if ((d.IsImportColumn == 1 || d.IsKey == 1) && dt.Columns.Contains(d.DisplayName))
                    {
                        PropertyInfo property = orderDetailType.GetProperty(d.DbColumnName);
                        row[d.DisplayName] = property.GetValue(c);

                    }
                });

                dt.Rows.Add(row);
            });


        });
        response.Data = dt;
        response.Code = StatusCode.Success;
        return response;
    }



    //默认方法不做任何处理
    public Response<DataTable> ExportPackage(List<long> request)
    {
        Response<DataTable> response = new Response<DataTable>();
        var tableColumn = GetExportColumns("WMS_OrderAddress", "WMS_Package", "WMS_PackageDetail");

        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT ");
        foreach (var item in tableColumn)
        {
            sql.Append(item.TableName + "." + item.DbColumnName + " as '" + item.DisplayName + "',");
        }
        sql.Remove(sql.Length - 1, 1);
        sql.Append(@"from   WMS_Package  
            left join WMS_PackageDetail
            on WMS_Package.Id=WMS_PackageDetail.PackageId 
            left join WMS_OrderAddress
            on WMS_Package.PreOrderNumber=WMS_OrderAddress.PreOrderNumber
           ");
        sql.Append("where WMS_Package.OrderId in (" + string.Join(",", request) + ")");
        var data = _repOrder.Context.Ado.GetDataTable(sql.ToString());
        response.Data = data;
        response.Code = StatusCode.Success;
        return response;

    }




    public virtual List<TableColumns> GetExportColumns(params string[] _tableNames)
    {
        var tenantId = _userManager.TenantId;
        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == tenantId &&
              (a.IsCreate == 1 || a.IsKey == 1)
            )
            .GroupBy(a => new { a.DbColumnName, a.Associated, a.IsImportColumn, a.DisplayName, a.IsKey,a.Type, a.IsCreate, a.Validation, a.TenantId })
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               TableName = SqlFunc.AggregateMax(a.TableName),
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               IsCreate = a.IsCreate,
               IsKey = a.IsKey,
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).Distinct().ToList();



    }

    public virtual List<TableColumns> GetImportColumns(params string[] _tableNames)
    {

        var tenantId = _userManager.TenantId;
        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == tenantId &&
              (a.IsImportColumn == 1 || a.IsKey == 1)
            )
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               IsKey = a.IsKey,
               TableName = a.TableName,
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).Distinct().ToList();

    }

}