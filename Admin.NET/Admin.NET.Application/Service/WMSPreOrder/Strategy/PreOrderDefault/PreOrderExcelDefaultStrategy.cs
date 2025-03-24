
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using System.Reflection;
using XAct;
using Microsoft.AspNetCore.Components.Forms;
using Nest;
using System.Globalization;

namespace Admin.NET.Application.Strategy
{
    public class PreOrderExcelDefaultStrategy : IPreOrderExcelInterface
    {
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        static List<string> _tableNames = new List<string>() { "WMS_PreOrder", "WMS_PreOrderDetail", "WMS_OrderAddress" };


        public PreOrderExcelDefaultStrategy()
        {

        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //默认方法不做任何处理
        public Response<DataTable, List<OrderStatusDto>> Import(dynamic request)
        {
            Response<DataTable, List<OrderStatusDto>> response = new Response<DataTable, List<OrderStatusDto>>();
            List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();
            //var headerTableColumn = GetColumns("WMS_PreOrder");
            //var detailTableColumn = GetColumns("WMS_PreOrderDetail");
            //var orderAddressTableColumn = GetColumns("WMS_OrderAddress");

            List<TableColumns> tableColumnList = new List<TableColumns>();
            foreach (var item in _tableNames)
            {
                //var  tableColumns = GetColumns(item);
                tableColumnList.AddRange(GetImportColumns(item));
            }
            //判断模板是不是最新的
            //判断方法直接比较字段数量，如果数量不一致，则提示需要更新模板
            //主信息需要导入的字段数量
            //int headerCount = headerTableColumn.Count(a => a.IsImportColumn == 1);
            //明细信息需要导入的字段数量
            //int detailCount = detailTableColumn.Count(a => a.IsImportColumn == 1);
            if (tableColumnList.Count != request.Columns.Count)
            {
                statusDtos.Add(new OrderStatusDto()
                {
                    //Id = b.Id,
                    ExternOrder = "第1行",
                    SystemOrder = "第1行",
                    //Type = b.OrderType,
                    StatusCode = StatusCode.Error,
                    //StatusMsg = (string)StatusCode.warning,
                    Msg = "模板更新，请重新下载最新模板"
                });
                response.Result = statusDtos;
                response.Data = request;
                response.Code = StatusCode.Error;
                //response.Code = StatusCode.Success;
                //throw new NotImplementedException();
                return response;
            }

            //循环datatable
            for (int i = 0; i < request.Columns.Count; i++)
            {
                //获取datatable的标头
                var s = request.Columns[i].ColumnName;
                var Column = tableColumnList.Where(a => a.DisplayName == s).FirstOrDefault();
                //if (Column == null)
                //{
                //    Column = detailTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
                //}
                //if (Column == null)
                //{
                //    Column = orderAddressTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
                //}
                if (Column == null)
                {
                    continue;
                }
                //验证数据
                if (Column.Validation == "Required")
                {
                    int flag = 1;
                    foreach (DataRow row in request.Rows)
                    {
                        flag++;

                        if (string.IsNullOrEmpty(row[s].ToString()))
                        {
                            statusDtos.Add(new OrderStatusDto()
                            {
                                //Id = b.Id,
                                ExternOrder = "第" + flag + "行",
                                SystemOrder = "第" + flag + "行",
                                //Type = b.OrderType,
                                StatusCode = StatusCode.Warning,
                                //StatusMsg = (string)StatusCode.warning,
                                Msg = Column.DisplayName + ":数据不能为空"
                            });
                        }
                        else
                        {
                            if (Column.Type == "DropDownListInt" && Column.tableColumnsDetails.Where(a => a.Name == row[s].ToString()).Count() == 0)
                            {
                                //mag = "数据错误,内容不能为空，或者不在系统提供范围内";
                                statusDtos.Add(new OrderStatusDto()
                                {
                                    //Id = b.Id,
                                    ExternOrder = "第" + flag + "行",
                                    SystemOrder = "第" + flag + "行",
                                    //Type = b.OrderType,
                                    StatusCode = StatusCode.Warning,
                                    //StatusMsg = (string)StatusCode.warning,
                                    Msg = Column.DisplayName + ":数据错误,“" + row[s].ToString() + "”不在系统提供范围内"
                                });
                            }
                            else if (Column.Type == "DropDownListStr" && Column.tableColumnsDetails.Where(a => a.Name == row[s].ToString()).Count() == 0)
                            {
                                statusDtos.Add(new OrderStatusDto()
                                {
                                    //Id = b.Id,
                                    ExternOrder = "第" + flag + "行",
                                    SystemOrder = "第" + flag + "行",
                                    //Type = b.OrderType,
                                    StatusCode = StatusCode.Warning,
                                    //StatusMsg = (string)StatusCode.warning,
                                    Msg = Column.DisplayName + ":数据错误,“" + row[s].ToString() + "”不在系统提供范围内"
                                });
                            }
                            else if (Column.Type == "DatePicker" || Column.Type == "DateTimePicker")
                            {
                                if (string.IsNullOrEmpty(row[s].ToString()))
                                {
                                    statusDtos.Add(new OrderStatusDto()
                                    {
                                        //Id = b.Id,
                                        ExternOrder = "第" + flag + "行",
                                        SystemOrder = "第" + flag + "行",
                                        //Type = b.OrderType,
                                        StatusCode = StatusCode.Warning,
                                        //StatusMsg = (string)StatusCode.warning,
                                        Msg = Column.DisplayName + ":数据错误,“" + row[s].ToString() + "”不是有效的日期格式"
                                    });
                                }
                                else
                                {
                                    var isDate = DateTime.TryParse(row[s].ToString(), out DateTime date);
                                    if (!isDate && date.Year < 2000)
                                    {
                                        statusDtos.Add(new OrderStatusDto()
                                        {
                                            //Id = b.Id,
                                            ExternOrder = "第" + flag + "行",
                                            SystemOrder = "第" + flag + "行",
                                            //Type = b.OrderType,
                                            StatusCode = StatusCode.Warning,
                                            //StatusMsg = (string)StatusCode.warning,
                                            Msg = Column.DisplayName + ":数据错误,“" + row[s].ToString() + "”不是有效的日期格式"
                                        });
                                    }
                                }
                            }
                            //else if (Column.Type == "DateTimePicker")
                            //{
                            //    var isDate = DateTime.TryParse(row[s].ToString(), out DateTime date);
                            //    if (isDate && date.Year > 2000)
                            //    {
                            //        statusDtos.Add(new OrderStatusDto()
                            //        {
                            //            //Id = b.Id,
                            //            ExternOrder = "第" + flag + "行",
                            //            SystemOrder = "第" + flag + "行",
                            //            //Type = b.OrderType,
                            //            StatusCode = StatusCode.Warning,
                            //            //StatusMsg = (string)StatusCode.warning,
                            //            Msg = Column.DisplayName + ":数据错误,“" + row[s].ToString() + "”不是有效的日期格式"
                            //        });
                            //    }
                            //}
                        }

                    }
                }
                //判断标头与key是否相等
                if (s.Equals(Column.DisplayName))
                {
                    //相等替换掉原来的表头
                    request.Columns[i].ColumnName = Column.DbColumnName;
                }

            }
            //验证数据


            //var config = new MapperConfiguration(cfg => cfg.CreateMap<DataTable, WMS_ASN>() );
            //var mapper = new Mapper(config);
            //var asnData = mapper.Map<List<WMS_ASN>>(request);
            if (statusDtos.Count > 0)
            {
                response.Code = StatusCode.Error;
            }
            else
            {
                response.Code = StatusCode.Success;
            }
            response.Result = statusDtos;
            response.Data = request;

            //throw new NotImplementedException();
            return response;
        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //默认方法不做任何处理
        public Response<DataTable> Export(WMSPreOrderExcelInput request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_PreOrder");
            var detailTableColumn = GetColumns("WMS_PreOrderDetail");
            var orderAddressTableColumn = GetColumns("WMS_OrderAddress");

            var PreOrderData = _repPreOrder.AsQueryable().Includes(a => a.Details).Includes(a => a.OrderAddress)
                    //.WhereIF(!string.IsNullOrWhiteSpace(request.PreOrderNumber), u => u.PreOrderNumber.Contains(request.PreOrderNumber.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(request.ExternOrderNumber), u => u.ExternOrderNumber.Contains(request.ExternOrderNumber.Trim()))
                    .WhereIF(request.CustomerId.HasValue && request.CustomerId > 0, u => u.CustomerId == request.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.CustomerName), u => u.CustomerName.Contains(request.CustomerName.Trim()))
                    .WhereIF(request.WarehouseId.HasValue && request.WarehouseId > 0, u => u.WarehouseId == request.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.WarehouseName), u => u.WarehouseName.Contains(request.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.OrderType), u => u.OrderType.Contains(request.OrderType.Trim()))
                    .WhereIF(request.PreOrderStatus.HasValue && request.PreOrderStatus != 0, u => u.PreOrderStatus == request.PreOrderStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Po), u => u.Po.Contains(request.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.So), u => u.So.Contains(request.So.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Creator), u => u.Creator.Contains(request.Creator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Updator), u => u.Updator.Contains(request.Updator.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Remark), u => u.Remark.Contains(request.Remark.Trim()))
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
                    .WhereIF(request.Ids != null && request.Ids.Count > 0, u => request.Ids.Contains(u.Id))

                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSPreOrder>()
;

            if (request.PreOrderNumber != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (request.PreOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = request.PreOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.PreOrderNumber.IndexOf(',') > 0)
                {
                    numbers = request.PreOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (request.PreOrderNumber.IndexOf(' ') > 0)
                {
                    numbers = request.PreOrderNumber.Split(' ').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    PreOrderData.WhereIF(!string.IsNullOrWhiteSpace(request.PreOrderNumber), u => numbers.Contains(u.PreOrderNumber.Trim()));

                }
                else
                {
                    PreOrderData.WhereIF(!string.IsNullOrWhiteSpace(request.PreOrderNumber), u => u.PreOrderNumber.Contains(request.PreOrderNumber.Trim()));
                }
            }

            if (request.ExternOrderNumber != null)
            {
                IEnumerable<string> numbers = Enumerable.Empty<string>();
                if (request.ExternOrderNumber.IndexOf("\n") > 0)
                {
                    numbers = request.ExternOrderNumber.Split('\n').Select(s => { return s.Trim(); });
                }
                if (request.ExternOrderNumber.IndexOf(',') > 0)
                {
                    numbers = request.ExternOrderNumber.Split(',').Select(s => { return s.Trim(); });
                }
                if (request.ExternOrderNumber.IndexOf(' ') > 0)
                {
                    numbers = request.ExternOrderNumber.Split(' ').Select(s => { return s.Trim(); });
                }
                if (numbers != null && numbers.Any())
                {
                    numbers = numbers.Where(c => !string.IsNullOrEmpty(c));
                }
                if (numbers != null && numbers.Any())
                {
                    PreOrderData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternOrderNumber), u => numbers.Contains(u.ExternOrderNumber.Trim()));

                }
                else
                {
                    PreOrderData.WhereIF(!string.IsNullOrWhiteSpace(request.ExternOrderNumber), u => u.ExternOrderNumber.Contains(request.ExternOrderNumber.Trim()));
                }
            }
            if (request.OrderTime != null && request.OrderTime.Count > 0)
            {
                DateTime? start = request.OrderTime[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.OrderTime >= start);
                if (request.OrderTime.Count > 1 && request.OrderTime[1].HasValue)
                {
                    var end = request.OrderTime[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.OrderTime < end);
                }
            }
            if (request.CompleteTime != null && request.CompleteTime.Count > 0)
            {
                DateTime? start = request.CompleteTime[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.CompleteTime >= start);
                if (request.CompleteTime.Count > 1 && request.CompleteTime[1].HasValue)
                {
                    var end = request.CompleteTime[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.CompleteTime < end);
                }
            }
            if (request.CreationTime != null && request.CreationTime.Count > 0)
            {
                DateTime? start = request.CreationTime[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.CreationTime >= start);
                if (request.CreationTime.Count > 1 && request.CreationTime[1].HasValue)
                {
                    var end = request.CreationTime[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.CreationTime < end);
                }
            }
            if (request.DateTime1 != null && request.DateTime1.Count > 0)
            {
                DateTime? start = request.DateTime1[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.DateTime1 >= start);
                if (request.DateTime1.Count > 1 && request.DateTime1[1].HasValue)
                {
                    var end = request.DateTime1[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.DateTime1 < end);
                }
            }
            if (request.DateTime2 != null && request.DateTime2.Count > 0)
            {
                DateTime? start = request.DateTime2[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.DateTime2 >= start);
                if (request.DateTime2.Count > 1 && request.DateTime2[1].HasValue)
                {
                    var end = request.DateTime2[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.DateTime2 < end);
                }
            }
            if (request.DateTime3 != null && request.DateTime3.Count > 0)
            {
                DateTime? start = request.DateTime3[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.DateTime3 >= start);
                if (request.DateTime3.Count > 1 && request.DateTime3[1].HasValue)
                {
                    var end = request.DateTime3[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.DateTime3 < end);
                }
            }
            if (request.DateTime4 != null && request.DateTime4.Count > 0)
            {
                DateTime? start = request.DateTime4[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.DateTime4 >= start);
                if (request.DateTime4.Count > 1 && request.DateTime4[1].HasValue)
                {
                    var end = request.DateTime4[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.DateTime4 < end);
                }
            }
            if (request.DateTime5 != null && request.DateTime5.Count > 0)
            {
                DateTime? start = request.DateTime5[0];
                PreOrderData = PreOrderData.WhereIF(start.HasValue, u => u.DateTime5 >= start);
                if (request.DateTime5.Count > 1 && request.DateTime5[1].HasValue)
                {
                    var end = request.DateTime5[1].Value.AddDays(1);
                    PreOrderData = PreOrderData.Where(u => u.DateTime5 < end);
                }
            }

            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn();

            //1，构建主表需要的信息
            headerTableColumn.ForEach(a =>
            {
                if (a.IsImportColumn == 1 || a.IsKey == 1)
                {
                    dc = dt.Columns.Add(a.DisplayName, typeof(string));
                }
            });
            //2.构建明细需要的信息
            detailTableColumn.ForEach(a =>
            {
                if ((a.IsImportColumn == 1 || a.IsKey == 1) && !dt.Columns.Contains(a.DisplayName))
                {
                    dc = dt.Columns.Add(a.DisplayName, typeof(string));
                }
            });

            //3.构建地址需要的信息
            orderAddressTableColumn.ForEach(a =>
            {
                if ((a.IsImportColumn == 1 || a.IsKey == 1) && !dt.Columns.Contains(a.DisplayName))
                {
                    dc = dt.Columns.Add(a.DisplayName, typeof(string));
                }
            });
            //塞数据
            PreOrderData.ForEach(a =>
            {

                Type orderType = a.GetType();
                a.Details.ForEach(c =>
                {
                    DataRow row = dt.NewRow();
                    Type orderDetailType = c.GetType();
                    Type preOrderTypeAddress = a.OrderAddress.GetType();
                    headerTableColumn.ForEach(h =>
                    {
                        if ((h.IsImportColumn == 1 || h.IsKey == 1) && dt.Columns.Contains(h.DisplayName))
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
                        if ((d.IsImportColumn == 1 || d.IsKey == 1) && dt.Columns.Contains(d.DisplayName))
                        {
                            PropertyInfo property = orderDetailType.GetProperty(d.DbColumnName);
                            row[d.DisplayName] = property.GetValue(c);

                        }
                    });
                    orderAddressTableColumn.ForEach(d =>
                    {
                        if ((d.IsImportColumn == 1 || d.IsKey == 1) && dt.Columns.Contains(d.DisplayName))
                        {
                            PropertyInfo property = preOrderTypeAddress.GetProperty(d.DbColumnName);
                            row[d.DisplayName] = property.GetValue(a.OrderAddress);
                        }
                    });
                    dt.Rows.Add(row);
                });

                //a.Details.ForEach(c =>
                //{
                //    DataRow row = dt.NewRow();
                //    Type preOrderType = a.GetType();
                //    Type preOrderTypeAddress = a.OrderAddress.GetType();

                //    Type receiptDetailType = c.GetType();
                //    headerTableColumn.ForEach(h =>
                //    {
                //        if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                //        {
                //            PropertyInfo property = preOrderType.GetProperty(h.DbColumnName);
                //            //如果该字段有下拉选项，则值取下拉选项中的值
                //            if (h.tableColumnsDetails.Count() > 0)
                //            {
                //                var val = property.GetValue(a);
                //                TableColumnsDetail data = new TableColumnsDetail();
                //                if (val is int)
                //                {
                //                    data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                //                }
                //                else
                //                {
                //                    data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val)).FirstOrDefault();

                //                }
                //                //var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
                //                if (data != null)
                //                {
                //                    row[h.DisplayName] = data.Name;
                //                }
                //                else
                //                {
                //                    row[h.DisplayName] = "";
                //                }
                //            }
                //            else
                //            {
                //                row[h.DisplayName] = property.GetValue(a);
                //            }

                //        }
                //    });

                //    detailTableColumn.ForEach(d =>
                //    {
                //        if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                //        {
                //            PropertyInfo property = receiptDetailType.GetProperty(d.DbColumnName);
                //            row[d.DisplayName] = property.GetValue(c);

                //        }
                //    });

                //    orderAddressTableColumn.ForEach(d =>
                //    {
                //        if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                //        {
                //            PropertyInfo property = preOrderTypeAddress.GetProperty(d.DbColumnName);
                //            row[d.DisplayName] = property.GetValue(a.OrderAddress);
                //        }
                //    });
                //    dt.Rows.Add(row);
                //});


            });
            response.Data = dt;
            response.Code = StatusCode.Success;
            //throw new NotImplementedException();
            return response;
        }


        private List<TableColumns> GetImportColumns(string TableName)
        {

            return _repTableColumns.AsQueryable()
             .Where(a => a.TableName == TableName &&
               a.TenantId == _userManager.TenantId &&
               (a.IsImportColumn == 1)
             )
            .Select(a => new TableColumns
            {
                DisplayName = a.DisplayName,
                //由于框架约定大于配置， 数据库的字段首字母小写
                //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
                DbColumnName = a.DbColumnName,
                Type = a.Type,
                IsKey = a.IsKey,
                IsImportColumn = a.IsImportColumn,
                Validation = a.Validation,
                tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
                //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
                //.Select()
            }).ToList();
            //return _repTableColumns.AsQueryable()
            //   .Where(a => a.TableName == TableName &&
            //     a.TenantId == _userManager.TenantId &&
            //     a.IsImportColumn == 1
            //   )
            //  .Select(a => new TableColumns
            //  {
            //      DisplayName = a.DisplayName,
            //      //由于框架约定大于配置， 数据库的字段首字母小写
            //      //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
            //      DbColumnName = a.DbColumnName,
            //      IsImportColumn = a.IsImportColumn
            //  }).ToList();
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
                Type = a.Type,
                IsKey = a.IsKey,
                IsImportColumn = a.IsImportColumn,
                Validation = a.Validation,
                tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
                //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
                //.Select()
            }).ToList();
            //return _repTableColumns.AsQueryable()
            //   .Where(a => a.TableName == TableName &&
            //     a.TenantId == _userManager.TenantId &&
            //     a.IsImportColumn == 1
            //   )
            //  .Select(a => new TableColumns
            //  {
            //      DisplayName = a.DisplayName,
            //      //由于框架约定大于配置， 数据库的字段首字母小写
            //      //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
            //      DbColumnName = a.DbColumnName,
            //      IsImportColumn = a.IsImportColumn
            //  }).ToList();
        }
    }
}