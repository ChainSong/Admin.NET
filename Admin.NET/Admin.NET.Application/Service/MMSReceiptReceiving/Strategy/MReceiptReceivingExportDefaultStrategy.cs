
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Data;
using System.Reflection;
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Interface;

namespace Admin.NET.Application.Strategy
{

    public class MReceiptReceivingExportDefaultStrategy : IMReceiptReceivingExcelInterface
    {
        //public ISqlSugarClient _db { get; set; }

        public SqlSugarRepository<MMSReceiptReceiving> _repReceiptReceiving { get; set; }
        public SqlSugarRepository<MMSReceipt> _repMReceipt { get; set; }
        public SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public SqlSugarRepository<MMSSupplier> _repSupplier { get; set; }
        public SqlSugarRepository<SupplierUserMapping> _repSupplierUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }

        public MReceiptReceivingExportDefaultStrategy(
        )
        {

        }

        //默认方法不做任何处理
        public Response<DataTable> Strategy(dynamic request)
        {

            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptOutput>();
            //var headerTableColumn = GetColumns("MMS_ReceiptReceiving");
            var detailTableColumn = GetColumns("MMS_ReceiptReceivingDetail");
            //var receiptData = _repMReceiptReceiving.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToListAsync();
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

            ////塞数据
            //receiptData.Result.ForEach(a =>
            //{

            //    Type receiptType = a.GetType();
            //    a.Details.ForEach(c =>
            //    {
            //        DataRow row = dt.NewRow();
            //        Type receiptReceivingType = c.GetType();
            //        headerTableColumn.ForEach(h =>
            //        {
            //            if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
            //            {
            //                PropertyInfo property = receiptType.GetProperty(h.DbColumnName);
            //                if (property != null)
            //                {
            //                    row[h.DisplayName] = property.GetValue(a);
            //                }
            //                else
            //                {
            //                    row[h.DisplayName] = "";
            //                }
            //            }
            //        });

            //        detailTableColumn.ForEach(d =>
            //        {
            //            if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
            //            {
            //                PropertyInfo property = receiptReceivingType.GetProperty(d.DbColumnName);
            //                if (property != null)
            //                {
            //                    row[d.DisplayName] = property.GetValue(c);
            //                }
            //                else
            //                {
            //                    row[d.DisplayName] = "";
            //                }

            //            }
            //        });
            //        dt.Rows.Add(row);
            //    });



            //});

            //循环datatable
            for (int i = 0; i < request.Columns.Count; i++)
            {
                //获取datatable的标头
                var s = request.Columns[i].ColumnName;
                var Column = detailTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
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
                  IsImportColumn = a.IsImportColumn,
                  Associated = a.Associated,
                  Type = a.Type
              }).ToList();
        }
    }
}