
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;

namespace Admin.NET.Application.Strategy
{
    public class ReceiptExportDefaultStrategy : IReceiptExcelInterface
    {


        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public ReceiptExportDefaultStrategy(
        )
        {
        }

        //默认方法不做任何处理
        public Response<DataTable> Export(List<long> request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_Receipt");
            var detailTableColumn = GetColumns("WMS_ReceiptDetail");
            var receiptData = _repReceipt.AsQueryable().Includes(a=>a.Details).Where(a => request.Contains(a.Id)).ToList();
             

            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();

            //1，构建主表需要的信息
            headerTableColumn.ForEach(a =>
          {
              if (a.IsImportColumn == 1)
              {
                  dc = dt.Columns.Add(a.DisplayName, typeof(string));
              }
          });
            //2.构建明细需要的信息
            detailTableColumn.ForEach(a =>
          {
              if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
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
            receiptData.ForEach(a =>
            {
               
                Type receiptType = a.GetType();
                a.Details.ForEach(c =>
                {
                    DataRow row = dt.NewRow();
                    Type receiptDetailType = c.GetType();
                    headerTableColumn.ForEach(h =>
                    {
                        if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                        {
                            PropertyInfo property = receiptType.GetProperty(h.DbColumnName);
                            //如果该字段有下拉选项，则值取下拉选项中的值
                            if (h.tableColumnsDetails.Count() > 0)
                            {
                                var val = property.GetValue(a);
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
                                row[h.DisplayName] = property.GetValue(a);
                            }

                        }
                    });

                    detailTableColumn.ForEach(d =>
                      {
                          if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                          {
                              PropertyInfo property = receiptDetailType.GetProperty(d.DbColumnName);
                              row[d.DisplayName] = property.GetValue(c);

                          }
                      });
                    dt.Rows.Add(row);
                });
            

            });
            response.Data = dt;
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
                  Type = a.Type,
                  //由于框架约定大于配置， 数据库的字段首字母小写
                  //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
                  DbColumnName = a.DbColumnName,
                  IsImportColumn = a.IsImportColumn,
                  tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId==a.TenantId).ToList()
                  //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
                  //.Select()
              }).ToList();
        }


    }

    //public class PropertyValue<T>
    //{
    //    private static ConcurrentDictionary<string, MemberGetDelegate> _memberGetDelegate = new ConcurrentDictionary<string, MemberGetDelegate>();
    //    delegate object MemberGetDelegate(T obj);
    //    public PropertyValue(T obj)
    //    {
    //        Target = obj;
    //    }
    //    public T Target { get; private set; }
    //    public object Get(string name)
    //    {
    //        MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
    //        return memberGet(Target);
    //    }
    //    private MemberGetDelegate BuildDelegate(string name)
    //    {
    //        Type type = typeof(T);
    //        PropertyInfo property = type.GetProperty(name);
    //        return (MemberGetDelegate)Delegate.CreateDelegate(typeof(MemberGetDelegate), property.GetGetMethod());
    //    }
    //}
}