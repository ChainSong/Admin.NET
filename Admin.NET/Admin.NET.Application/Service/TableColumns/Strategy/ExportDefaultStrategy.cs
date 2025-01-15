// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using System.Data;
using AngleSharp.Html.Dom;

namespace Admin.NET.Application;
public class ExportDefaultStrategy : IExportDefaultInterface
{

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
    public UserManager _userManager { get; set; }


    public List<string> _tableNames { get; set; }

    public ExportDefaultStrategy(List<string> tableNames, SqlSugarRepository<TableColumns> repTableColumns, UserManager userManager)
    {
        this._tableNames = tableNames;
        this._repTableColumns = repTableColumns;
        this._userManager = userManager;

    }



    /// <summary>
    /// 导出
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public virtual Response<DataTable> Export(List<long> request)
    {
        Response<DataTable> response = new Response<DataTable>();
        //CreateOrUpdateWMS_ReceiptInput orders = new CreateOrUpdateWMS_ReceiptInput();
        //orders.WMS_Receipts = new List<WMSReceiptEditDto>();
        //var orderDatas = _repASN.AsQueryable()
        //    //.LeftJoin<WMSASNDetail>（）
        //    .LeftJoin<WMSASNDetail>((a,b)=>a.Id==b.ASNId).Where(a => request.Contains(a.Id)).Select((a,b) => new
        //    {
        //        a,
        //        b
        //    }).ToList();

        //var headerTableColumn = GetColumns("WMS_ASN");
        //var detailTableColumn = GetColumns("WMS_ASNDetail");

        List<TableColumns> tableColumnList = new List<TableColumns>();
        foreach (var item in _tableNames)
        {
            tableColumnList.AddRange(GetColumns(item));
        }
        //var orderData = _repASN.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();


        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        //1，构建主表需要的信息
        tableColumnList.ForEach(a =>
        {
            if (a.IsImportColumn == 1)
            {
                dc = dt.Columns.Add(a.DisplayName, typeof(string));
            }
        }); 
        response.Data = dt;
        response.Code = StatusCode.Success;
        //throw new NotImplementedException();
        return response;
    }

    public List<TableColumns> GetColumns(string TableName)
    {
      try
      {


            var aaa = _repTableColumns.AsQueryable()
            .Where(a => a.TableName == TableName &&
              a.TenantId == _userManager.TenantId &&
              a.IsImportColumn == 1
            );

            var aaass = _repTableColumns.AsQueryable()
            .Where(a => a.TableName == TableName &&
              a.TenantId == _userManager.TenantId &&
              a.IsImportColumn == 1
            )
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).ToList();


            return _repTableColumns.AsQueryable()
            .Where(a => a.TableName == TableName &&
              a.TenantId == _userManager.TenantId &&
              a.IsImportColumn == 1
            )
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               //tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).ToList();
        }
      catch (Exception ex)
      {
          throw ex;
      }
    }
}
