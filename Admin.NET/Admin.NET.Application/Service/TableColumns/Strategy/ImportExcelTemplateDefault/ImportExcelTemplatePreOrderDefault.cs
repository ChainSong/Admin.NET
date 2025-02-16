﻿
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;

namespace Admin.NET.Application
{
    public class ImportExcelTemplatePreOrderDefault : ImportExcelTemplateStrategy
    {


        //public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
        //public UserManager _userManager { get; set; }

        static List<string> _tableNames = new List<string>() { "WMS_PreOrder", "WMS_PreOrderDetail", "WMS_OrderAddress" };
        public ImportExcelTemplatePreOrderDefault(
            //ITable_ColumnsManager table_ColumnsManager,
            //      ITable_ColumnsDetailManager table_ColumnsDetailManager
            ):base(_tableNames)
        {
            //_table_ColumnsManager = table_ColumnsManager;
            //_table_ColumnsDetailManager = table_ColumnsDetailManager;
        }


        public override Response<byte[]> Strategy(long CustomerId, long TenantId)
        {
            Response<byte[]> response = new Response<byte[]>();
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


            var orderAddress = _repTableColumns.AsQueryable().Where(a => a.TableName == "WMS_OrderAddress" &&
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
            //3.构建地址信息
            foreach (var item in orderAddress.ToList())
            {
                if (item.IsImportColumn == 1 && !dt.Columns.Contains(item.DisplayName))
                {
                    dc = dt.Columns.Add(item.DisplayName, typeof(string));
                }
            }
            IExporter exporter = new ExcelExporter();
            var result = exporter.ExportAsByteArray<DataTable>(dt);

            response.Code = StatusCode.Success;
            response.Data = result.Result;
            return response;

        }
    }
}
