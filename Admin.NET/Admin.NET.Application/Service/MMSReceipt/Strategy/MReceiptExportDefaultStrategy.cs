
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
using Admin.NET.Application.ReceiptReceivingCore.Interface;

namespace Admin.NET.Application.Strategy
{
    public class MReceiptExportDefaultStrategy : IMReceiptExcelInterface
    {


        public SqlSugarRepository<MMSReceipt> _repMReceipt { get; set; }
        public SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail { get; set; }
        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public SqlSugarRepository<MMSSupplier> _repSupplier { get; set; }
        public SqlSugarRepository<SupplierUserMapping> _repSupplierUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail { get; set; }
        public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        public SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable { get; set; }

        public MReceiptExportDefaultStrategy(
        )
        {
        }

        //默认方法不做任何处理
        /// <summary>
        /// 导出入库信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<DataTable> ExportReceipt(List<long> request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_Receipt");
            var detailTableColumn = GetColumns("WMS_ReceiptDetail");
            var receiptData = _repMReceipt.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();


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

        /// <summary>
        /// 导出上架模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<DataTable> ExportReceiptReceiving(List<long> request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptOutput>();

            var headerTableColumn = GetColumns("MMS_Receipt");
            var detailTableColumn = GetColumns("MMS_ReceiptReceivingDetail");
            var receiptData = _repMReceipt.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToListAsync();
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

            //塞数据
            receiptData.Result.ForEach(a =>
            {

                //判断是采用上架表数据，还是使用入库明细数据(上架表有数据，就采用上架表数据，否则采用入库明细数据)
                //采用入库明细数据需要推荐上架库位
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


                            if (property != null)
                            {

                                // 判断是下拉列表 就取下拉列表的数据
                                if (h.Type == "DropDownListInt")
                                {
                                    try
                                    {


                                        row[h.DisplayName] = _repTableColumnsDetail.AsQueryable().Where(q => q.Associated == h.Associated).First().Name;
                                    }
                                    catch (Exception e)
                                    {
                                        row[h.DisplayName] = property.GetValue(a);
                                    }

                                }
                                else
                                {
                                    row[h.DisplayName] = property.GetValue(a);
                                }
                            }
                            else
                            {
                                //判断是库位 ，获取推荐库位
                                if (h.DbColumnName == "Location")
                                {
                                    var LocationData = _repInventoryUsable.AsQueryable().Where(i => i.SupplierId == a.SupplierId && i.WarehouseId == a.WarehouseId && i.SKU == c.SKU).GroupBy(i => i.Location).OrderBy(i => i.Location).FirstAsync();
                                    if (LocationData == null)
                                    {
                                        row[h.DisplayName] = "";
                                    }
                                    else
                                    {
                                        row[h.DisplayName] = LocationData.Result.Location;
                                    }

                                }
                                else
                                {
                                    row[h.DisplayName] = "";
                                }
                            }
                        }
                    });

                    detailTableColumn.ForEach(d =>
                    {
                        if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                        {
                            PropertyInfo property = receiptDetailType.GetProperty(d.DbColumnName);
                            if (property != null)
                            {
                                row[d.DisplayName] = property.GetValue(c);
                            }
                            else
                            {
                                //判断是库位 ，获取推荐库位
                                if (d.DbColumnName == "Location")
                                {
                                    var LocationData = _repInventoryUsable.AsQueryable().Where(i => i.SupplierId == a.SupplierId && i.WarehouseId == a.WarehouseId && i.SKU == c.SKU).GroupBy(i => i.Location).OrderBy(i => i.Location).Select(b => b.Location).First();
                                    if (LocationData == null)
                                    {
                                        row[d.DisplayName] = "";
                                    }
                                    else
                                    {
                                        row[d.DisplayName] = LocationData;
                                    }

                                }
                                else
                                {
                                    row[d.DisplayName] = "";
                                }
                                //row[d.DisplayName] = "";
                            }

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
                 (a.IsImportColumn == 1 || a.IsKey == 1)
               )
              .Select(a => new TableColumns
              {
                  DisplayName = a.DisplayName,
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