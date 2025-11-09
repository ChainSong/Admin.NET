// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum; 
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class HandoverExcelDefaultStrategy : IHandoverExcelInterface
{


    public SqlSugarRepository<WMSHandover> _repHandover { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public UserManager _userManager { get; set; }

    //public ISqlSugarClient _db { get; set; }



    public HandoverExcelDefaultStrategy(

    )
    {

    }

    //默认方法不做任何处理
    public Response<DataTable> Strategy(dynamic request)
    {
        Response<DataTable> response = new Response<DataTable>();
         
        var headerTableColumn = GetColumns("WMS_Handover");
        //DataTable dt = new DataTable();
        //DataColumn dc = new DataColumn();

        ////1，构建主表需要的信息
        //headerTableColumn.ForEach(a =>
        //{
        //    if (a.IsImportColumn == 1)
        //    {
        //        dc = dt.Columns.Add(a.DisplayName, typeof(string));
        //    }
        //});
        ////2.构建明细需要的信息
        //detailTableColumn.ForEach(a =>
        //{
        //    if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
        //    {
        //        dc = dt.Columns.Add(a.DisplayName, typeof(string));
        //    }
        //});

        //循环datatable
        for (int i = 0; i < request.Columns.Count; i++)
        {
            //获取datatable的标头
            var s = request.Columns[i].ColumnName;
            var Column = headerTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
            //if (Column == null)
            //{
            //    Column = detailTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
            //}

            if (Column == null)
            {
                continue;
            }
            //判断标头与key是否相等
            if (s.Equals(Column.DisplayName))
            {
                //相等替换掉原来的表头
                request.Columns[i].ColumnName = Column.DbColumnName;
            }

        }
        response.Data = request;
        response.Code = StatusCode.Success;
        //throw new NotImplementedException();
        return response;
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
              IsImportColumn = a.IsImportColumn

          }).ToList();
    }
}
