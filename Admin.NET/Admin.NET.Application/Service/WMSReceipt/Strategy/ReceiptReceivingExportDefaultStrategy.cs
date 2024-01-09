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

namespace Admin.NET.Application.Strategy
{

    public class ReceiptReceivingExportDefaultStrategy : IReceiptReceivingExportInterface
    {
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
        public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }

        public ReceiptReceivingExportDefaultStrategy(
        )
        {

        }

        //默认方法不做任何处理
        public Response<DataTable> Strategy(List<long> request)
        {

            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptOutput>();

            var headerTableColumn = GetColumns("WMS_Receipt");
            var detailTableColumn = GetColumns("WMS_ReceiptReceiving");
            var receiptData = _repReceipt.AsQueryable().Includes(a => a.Details).Includes(b => b.ReceiptReceivings).Where(a => request.Contains(a.Id)).ToListAsync();
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
                if (a.ReceiptReceivings == null || a.ReceiptReceivings.Count == 0)
                {
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
                                        var LocationData = _repTableInventoryUsable.AsQueryable().Where(i => i.CustomerId == a.CustomerId && i.WarehouseId == a.WarehouseId && i.SKU == c.SKU).GroupBy(i => i.Location).OrderBy(i => i.Location).FirstAsync();
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
                                        var LocationData = _repTableInventoryUsable.AsQueryable().Where(i => i.CustomerId == a.CustomerId && i.WarehouseId == a.WarehouseId && i.SKU == c.SKU).GroupBy(i => i.Location).OrderBy(i => i.Location).Select(b => b.Location).First();
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
                }
                else
                {
                    a.ReceiptReceivings.ForEach(c =>
                    {
                        DataRow row = dt.NewRow();
                        Type receiptReceivingType = c.GetType();
                        headerTableColumn.ForEach(h =>
                        {
                            if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                            {
                                PropertyInfo property = receiptType.GetProperty(h.DbColumnName);
                                if (property != null)
                                {
                                    //row[h.DisplayName] = property.GetValue(a);
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
                                    row[h.DisplayName] = "";
                                }
                            }
                        });

                        detailTableColumn.ForEach(d =>
                        {
                            if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                            {
                                PropertyInfo property = receiptReceivingType.GetProperty(d.DbColumnName);
                                if (property != null)
                                {
                                    row[d.DisplayName] = property.GetValue(c);
                                }
                                else
                                {
                                    row[d.DisplayName] = "";
                                }

                            }
                        });
                        dt.Rows.Add(row);
                    });
                }


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