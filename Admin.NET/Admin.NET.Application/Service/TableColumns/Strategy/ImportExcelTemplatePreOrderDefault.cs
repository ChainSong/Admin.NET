
using Admin.NET.Application.CommonCore.EnumCommon;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application
{
    public class ImportExcelTemplatePreOrderDefault : IImportExcelTemplate
    {


        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
        public UserManager _userManager { get; set; }


        public ImportExcelTemplatePreOrderDefault(
            //ITable_ColumnsManager table_ColumnsManager,
            //      ITable_ColumnsDetailManager table_ColumnsDetailManager
            )
        {
            //_table_ColumnsManager = table_ColumnsManager;
            //_table_ColumnsDetailManager = table_ColumnsDetailManager;
        }


        public Response<DataTable> Strategy(long CustomerId, long TenantId)
        {


            Response<DataTable> response = new Response<DataTable>();

            var header = _repTableColumns.AsQueryable()
            .Where(a => a.TableName == "WMS_PreOrder" &&
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
           });
            var detail = _repTableColumns.AsQueryable().Where(a => a.TableName == "WMS_PreOrderDetail" &&
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
           });
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();

            //1，构建主表需要的信息
            foreach (var item in header.ToList())
            {
                if (item.IsImportColumn == 1)
                {
                    dc = dt.Columns.Add(item.DisplayName, typeof(string));
                }
            }
            //2.构建明细需要的信息
            foreach (var item in detail.ToList())
            {
                if (item.IsImportColumn == 1 && !dt.Columns.Contains(item.DisplayName))
                {
                    dc = dt.Columns.Add(item.DisplayName, typeof(string));
                }
            }
            response.Code = StatusCode.Success;
            response.Data = dt;
            return response;

        }
    }
}
