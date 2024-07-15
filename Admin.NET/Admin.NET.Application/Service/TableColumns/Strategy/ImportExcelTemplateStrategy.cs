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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Furion.FriendlyException;

namespace Admin.NET.Application.Service;
public class ImportExcelTemplateStrategy : IImportExcelTemplateInterface
{

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
    public UserManager _userManager { get; set; }


    public List<string> _tableNames { get; set; }



    public ImportExcelTemplateStrategy(List<string> tableNames)
    {
        _tableNames = tableNames;
    }



    public virtual Response<DataTable> Strategy(long CustomerId, long TenantId)
    {
        try
        {

            Response<DataTable> response = new Response<DataTable>();

            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();

            //foreach (var item in _tableNames)
            //{
            var data = _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == TenantId &&
              a.IsImportColumn == 1
            )
             .Select(a => new TableColumns
             {
                 DisplayName = a.DisplayName,
                 //由于框架约定大于配置， 数据库的字段首字母小写
                 //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
                 DbColumnName = a.DbColumnName,
                 IsImportColumn = a.IsImportColumn
             }).OrderBy(a => new { a.TableName, a.Order});

            foreach (var field in data.ToList())
            {
                if (field.IsImportColumn == 1)
                {
                    dc = dt.Columns.Add(field.DisplayName, typeof(string));
                }
            }
            //}

            // var header = _repTableColumns.AsQueryable()
            // .Where(a => a.TableName == "WMS_Product" &&
            //   a.TenantId == TenantId &&
            //   a.IsImportColumn == 1
            // )
            //.Select(a => new TableColumns
            //{
            //    DisplayName = a.DisplayName,
            //    //由于框架约定大于配置， 数据库的字段首字母小写
            //    //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
            //    DbColumnName = a.DbColumnName,
            //    IsImportColumn = a.IsImportColumn
            //}).OrderBy(a => a.Order);
            // var detail = _repTableColumns.AsQueryable().Where(a => a.TableName == "WMS_ASNDetail" &&
            //   a.TenantId == TenantId &&
            //   a.IsImportColumn == 1
            // )
            //.Select(a => new TableColumns
            //{
            //    DisplayName = a.DisplayName,
            //    //由于框架约定大于配置， 数据库的字段首字母小写
            //    //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
            //    DbColumnName = a.DbColumnName,
            //    IsImportColumn = a.IsImportColumn
            //}).OrderBy(a=>a.Order);
            //DataTable dt = new DataTable();
            //DataColumn dc = new DataColumn();

            ////1，构建主表需要的信息
            //foreach (var item in header.ToList())
            //{
            //    if (item.IsImportColumn == 1)
            //    {
            //        dc = dt.Columns.Add(item.DisplayName, typeof(string));
            //    }
            //}
            //2.构建明细需要的信息
            //foreach (var item in detail.ToList())
            //{
            //    if (item.IsImportColumn == 1 && !dt.Columns.Contains(item.DisplayName))
            //    {
            //        dc = dt.Columns.Add(item.DisplayName, typeof(string));
            //    }
            //}
            response.Code = StatusCode.Success;
            response.Data = dt;
            return response;
        }
        catch (Exception ex)
        {
            throw Oops.Oh(ex.Message);  //
        }

    }
}
