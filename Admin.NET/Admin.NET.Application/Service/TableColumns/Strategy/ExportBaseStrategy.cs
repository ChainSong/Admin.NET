﻿// 麻省理工学院许可证
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
using XAct;

namespace Admin.NET.Application;
public class ExportBaseStrategy
{

    public virtual SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
    public virtual UserManager _userManager { get; set; }


    //public List<string> _tableNames { get; set; }

    public ExportBaseStrategy()
    {
        //this._tableNames = tableNames;
        //this._repTableColumns = repTableColumns;
        //this._userManager = userManager;

    }


    public virtual List<TableColumns> GetExportColumns(params string[] _tableNames)
    {
        var tenantId = _userManager.TenantId;
        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == tenantId &&
              a.IsCreate == 1
            )
            .GroupBy(a => new { a.DbColumnName, a.Associated,a.IsImportColumn, a.DisplayName, a.Type,a.IsCreate, a.Validation,  a.TenantId })
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
               IsCreate=a.IsCreate,
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
              a.IsImportColumn == 1
            )
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
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
