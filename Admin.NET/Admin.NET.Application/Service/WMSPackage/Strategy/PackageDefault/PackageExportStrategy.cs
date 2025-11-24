// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
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
using System.Reflection;

namespace Admin.NET.Application.Service;
public class PackageExportStrategy : IPackageExportInterface
{


    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
    //包装
    public SqlSugarRepository<Core.Entity.WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
    public UserManager _userManager { get; set; }


    public static List<string> _tableNames { get; set; } = new List<string>() { "WMS_Order", "WMS_OrderAddress", "WMS_Package", "WMS_PackageDetail" };


    public PackageExportStrategy() //: base( _repTableColumns, _userManager)
    {

    }

    //默认方法不做任何处理
    public Response<DataTable> Export(WMSPackageInput request)
    {
        Response<DataTable> response = new Response<DataTable>();
        //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
        //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

        var tableColumn = GetExportColumns();
        //var addressTableColumn = GetColumns("WMS_OrderAddress");
        //var headerTableColumn = GetColumns("WMS_Package");
        //var detailTableColumn = GetColumns("WMS_PackageDetail");

        var orders = _repOrder.AsQueryable().Includes(a => a.OrderAddress).Where(a => request.Ids.Contains(a.Id)).ToList();
        var package = _repPackage.AsQueryable().Includes(a => a.Details).Where(a => request.Ids.Contains(a.OrderId)).ToList();



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

        var table = from a in orders join b in package on a.Id equals b.OrderId select new { a, b };


        //2.构建明细需要的信息
        //detailTableColumn.ForEach(a =>
        //{
        //    if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
        //    {
        //        dc = dt.Columns.Add(a.DisplayName, typeof(string));
        //    }
        //});
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

        foreach (var item in table)
        {
            DataRow row = dt.NewRow();
            Type orderType = item.a.GetType();
            Type packageType = item.b.GetType();


            tableColumn.ForEach(h =>
            {
                if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                {
                    PropertyInfo property = orderType.GetProperty(h.DbColumnName);
                    if (property == null)
                    {
                        property = packageType.GetProperty(h.DbColumnName);
                    }
                    //如果该字段有下拉选项，则值取下拉选项中的值
                    if (h.tableColumnsDetails.Count() > 0)
                    {
                        var val = property.GetValue(item.a);
                        var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
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
                        row[h.DisplayName] = property.GetValue(item.a);
                    }

                }
            });
            dt.Rows.Add(row);
        }


        //塞数据
        //package.ForEach(a =>
        //{

        //    Type receiptType = a.GetType();
        //    a.Details.ForEach(c =>
        //    {
        //        DataRow row = dt.NewRow();
        //        Type detailType = c.GetType();
        //        tableColumn.ForEach(h =>
        //        {
        //            if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
        //            {
        //                PropertyInfo property = receiptType.GetProperty(h.DbColumnName);
        //                //如果该字段有下拉选项，则值取下拉选项中的值
        //                if (h.tableColumnsDetails.Count() > 0)
        //                {
        //                    var val = property.GetValue(a);
        //                    var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
        //                    if (data != null)
        //                    {
        //                        row[h.DisplayName] = data.Name;
        //                    }
        //                    else
        //                    {
        //                        row[h.DisplayName] = "";
        //                    }
        //                }
        //                else
        //                {
        //                    row[h.DisplayName] = property.GetValue(a);
        //                }

        //            }
        //        });

        //        //tableColumn.ForEach(d =>
        //        //{
        //        //    if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
        //        //    {
        //        //        PropertyInfo property = detailType.GetProperty(d.DbColumnName);
        //        //        row[d.DisplayName] = property.GetValue(c);

        //        //    }
        //        //});
        //        dt.Rows.Add(row);
        //    });


        //});
        response.Data = dt;
        response.Code = StatusCode.Success;
        //throw new NotImplementedException();
        return response;
    }

    //默认方法不做任何处理
    public Response<DataTable> ExportPackage(WMSPackageInput request)
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
        sql.Append("where WMS_Package.Id in (" + string.Join(",", request.Ids) + ")");
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
              a.IsCreate == 1
            )
            .GroupBy(a => new { a.DbColumnName, a.Associated, a.IsImportColumn, a.DisplayName, a.Type, a.IsCreate, a.Validation, a.TenantId })
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
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).Distinct().ToList();


    }
}

