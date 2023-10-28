
using Admin.NET.Application.CommonCore.EnumCommon;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.ReceiptReceivingCore.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.ReceiptReceivingCore.Strategy
{
    public class ReceiptReceivingExcelDefaultStrategy : IReceiptReceivingExcelInterface
    {


        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public ISqlSugarClient _db { get; set; }

 

        public ReceiptReceivingExcelDefaultStrategy(

        )
        {

        }

        //默认方法不做任何处理
        public Response<DataTable> Strategy(dynamic request)
        {
            Response<DataTable> response = new Response<DataTable>();

            var headerTableColumn = GetColumns("WMS_Receipt");
            var detailTableColumn = GetColumns("WMS_ReceiptReceiving");
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
                if (Column == null)
                {
                    Column = detailTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
                }

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
}