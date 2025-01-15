
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
using Nest;

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
        public Response<DataTable> Export(WMSReceiptInput request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_Receipt");
            var detailTableColumn = GetColumns("WMS_ReceiptDetail");
            var receiptData = _repReceipt.AsQueryable().Includes(a => a.Details)
                  .WhereIF(request.ASNId > 0, u => u.ASNId == request.ASNId)
                    //.WhereIF(!string.IsNullOrWhiteSpace(request.ASNNumber), u => u.ASNNumber.Contains(request.ASNNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(request.ReceiptNumber), u => u.ReceiptNumber.Contains(request.ReceiptNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(request.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(request.ExternReceiptNumber.Trim()))
                    .WhereIF(request.CustomerId.HasValue && request.CustomerId > 0, u => u.CustomerId == request.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.CustomerName), u => u.CustomerName.Contains(request.CustomerName.Trim()))
                    .WhereIF(request.WarehouseId.HasValue && request.WarehouseId > 0, u => u.WarehouseId == request.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.WarehouseName), u => u.WarehouseName.Contains(request.WarehouseName.Trim()))
                    .WhereIF(request.ReceiptStatus.HasValue && request.ReceiptStatus != 0, u => u.ReceiptStatus == request.ReceiptStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Po), u => u.Po.Contains(request.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.So), u => u.So.Contains(request.So.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.ReceiptType), u => u.ReceiptType.Contains(request.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Contact), u => u.Contact.Contains(request.Contact.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.ContactInfo), u => u.ContactInfo.Contains(request.ContactInfo.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Remark), u => u.Remark.Contains(request.Remark.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Creator), u => u.Creator.Contains(request.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Updator), u => u.Updator.Contains(request.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str1), u => u.Str1.Contains(request.Str1.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str2), u => u.Str2.Contains(request.Str2.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str3), u => u.Str3.Contains(request.Str3.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str4), u => u.Str4.Contains(request.Str4.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str5), u => u.Str5.Contains(request.Str5.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str6), u => u.Str6.Contains(request.Str6.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str7), u => u.Str7.Contains(request.Str7.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str8), u => u.Str8.Contains(request.Str8.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str9), u => u.Str9.Contains(request.Str9.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str10), u => u.Str10.Contains(request.Str10.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str11), u => u.Str11.Contains(request.Str11.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str12), u => u.Str12.Contains(request.Str12.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str13), u => u.Str13.Contains(request.Str13.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str14), u => u.Str14.Contains(request.Str14.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str15), u => u.Str15.Contains(request.Str15.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str16), u => u.Str16.Contains(request.Str16.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str17), u => u.Str17.Contains(request.Str17.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str18), u => u.Str18.Contains(request.Str18.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str19), u => u.Str19.Contains(request.Str19.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Str20), u => u.Str20.Contains(request.Str20.Trim()))
                    .WhereIF(request.Int1 > 0, u => u.Int1 == request.Int1)
                    .WhereIF(request.Int2 > 0, u => u.Int2 == request.Int2)
                    .WhereIF(request.Int3 > 0, u => u.Int3 == request.Int3)
                    .WhereIF(request.Int4 > 0, u => u.Int4 == request.Int4)
                    .WhereIF(request.Int5 > 0, u => u.Int5 == request.Int5)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .WhereIF(request.Ids != null && request.Ids.Count > 0, u => request.Ids.Contains(u.Id))
                    .Select<WMSReceipt>();
            if (request.ASNNumber != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (request.ASNNumber.IndexOf("\n") > 0)
                {
                    numbers = request.ASNNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.ASNNumber.IndexOf(',') > 0)
                {
                    numbers = request.ASNNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (request.ASNNumber.IndexOf(' ') > 0)
                {
                    numbers = request.ASNNumber.Split(' ').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ASNNumber), u => numbers.Contains(u.ASNNumber.Trim()));

                }
                else
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ASNNumber), u => u.ASNNumber.Contains(request.ASNNumber.Trim()));
                }
            }

            if (request.ExternReceiptNumber != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (request.ExternReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = request.ExternReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.ExternReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = request.ExternReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (request.ExternReceiptNumber.IndexOf(' ') > 0)
                {
                    numbers = request.ExternReceiptNumber.Split(' ').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternReceiptNumber), u => numbers.Contains(u.ExternReceiptNumber.Trim()));

                }
                else
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(request.ExternReceiptNumber.Trim()));
                }
            }

            if (request.ReceiptNumber != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (request.ReceiptNumber.IndexOf("\n") > 0)
                {
                    numbers = request.ReceiptNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.ReceiptNumber.IndexOf(',') > 0)
                {
                    numbers = request.ReceiptNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (request.ReceiptNumber.IndexOf(' ') > 0)
                {
                    numbers = request.ReceiptNumber.Split(' ').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ReceiptNumber), u => numbers.Contains(u.ReceiptNumber.Trim()));

                }
                else
                {
                    receiptData.WhereIF(!string.IsNullOrWhiteSpace(request.ReceiptNumber), u => u.ReceiptNumber.Contains(request.ReceiptNumber.Trim()));
                }
            }

            if (request.ReceiptTime != null && request.ReceiptTime.Count > 0)
            {
                DateTime? start = request.ReceiptTime[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.ReceiptTime >= start);
                if (request.ReceiptTime.Count > 1 && request.ReceiptTime[1].HasValue)
                {
                    var end = request.ReceiptTime[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.ReceiptTime < end);
                }
            }
            if (request.CompleteTime != null && request.CompleteTime.Count > 0)
            {
                DateTime? start = request.CompleteTime[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.CompleteTime >= start);
                if (request.CompleteTime.Count > 1 && request.CompleteTime[1].HasValue)
                {
                    var end = request.CompleteTime[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.CompleteTime < end);
                }
            }
            if (request.CreationTime != null && request.CreationTime.Count > 0)
            {
                DateTime? start = request.CreationTime[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.CreationTime >= start);
                if (request.CreationTime.Count > 1 && request.CreationTime[1].HasValue)
                {
                    var end = request.CreationTime[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.CreationTime < end);
                }
            }
            if (request.DateTime1 != null && request.DateTime1.Count > 0)
            {
                DateTime? start = request.DateTime1[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.DateTime1 >= start);
                if (request.DateTime1.Count > 1 && request.DateTime1[1].HasValue)
                {
                    var end = request.DateTime1[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.DateTime1 < end);
                }
            }
            if (request.DateTime2 != null && request.DateTime2.Count > 0)
            {
                DateTime? start = request.DateTime2[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.DateTime2 >= start);
                if (request.DateTime2.Count > 1 && request.DateTime2[1].HasValue)
                {
                    var end = request.DateTime2[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.DateTime2 < end);
                }
            }
            if (request.DateTime3 != null && request.DateTime3.Count > 0)
            {
                DateTime? start = request.DateTime3[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.DateTime3 >= start);
                if (request.DateTime3.Count > 1 && request.DateTime3[1].HasValue)
                {
                    var end = request.DateTime3[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.DateTime3 < end);
                }
            }
            if (request.DateTime4 != null && request.DateTime4.Count > 0)
            {
                DateTime? start = request.DateTime4[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.DateTime4 >= start);
                if (request.DateTime4.Count > 1 && request.DateTime4[1].HasValue)
                {
                    var end = request.DateTime4[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.DateTime4 < end);
                }
            }
            if (request.DateTime5 != null && request.DateTime5.Count > 0)
            {
                DateTime? start = request.DateTime5[0];
                receiptData = receiptData.WhereIF(start.HasValue, u => u.DateTime5 >= start);
                if (request.DateTime5.Count > 1 && request.DateTime5[1].HasValue)
                {
                    var end = request.DateTime5[1].Value.AddDays(1);
                    receiptData = receiptData.Where(u => u.DateTime5 < end);
                }
            }


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

                Type orderType = a.GetType();
                a.Details.ForEach(c =>
                {
                    DataRow row = dt.NewRow();
                    Type orderDetailType = c.GetType();
                    headerTableColumn.ForEach(h =>
                    {
                        if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                        {
                            PropertyInfo property = orderType.GetProperty(h.DbColumnName);
                            //如果该字段有下拉选项，则值取下拉选项中的值
                            if (h.tableColumnsDetails.Count() > 0)
                            {
                                var val = property.GetValue(a);
                                TableColumnsDetail data = new TableColumnsDetail();
                                if (val is int)
                                {
                                    data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                                }
                                else
                                {
                                    data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val)).FirstOrDefault();

                                }
                                //var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
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
                            PropertyInfo property = orderDetailType.GetProperty(d.DbColumnName);
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
                  tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
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