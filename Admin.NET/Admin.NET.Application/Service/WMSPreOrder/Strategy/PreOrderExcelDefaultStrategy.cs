
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

namespace Admin.NET.Application.Strategy
{
    public class PreOrderExcelDefaultStrategy : IPreOrderExcelInterface
    {
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public  UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public  SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
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
                tableColumnList.AddRange(GetColumns(item));
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
                                //var date = new DateTime();
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
        public Response<DataTable> Export(List<long> request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //receipts.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_PreOrder");
            var detailTableColumn = GetColumns("WMS_PreOrderDetail");
            var orderAddressTableColumn = GetColumns("WMS_OrderAddress");

            var PreOrderData = _repPreOrder.AsQueryable().Includes(a => a.Details).Includes(a => a.OrderAddress).Where(a => request.Contains(a.Id)).ToList();


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

            //3.构建地址需要的信息
            orderAddressTableColumn.ForEach(a =>
            {
                if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
                {
                    dc = dt.Columns.Add(a.DisplayName, typeof(string));
                }
            });
            //塞数据
            PreOrderData.ForEach(a =>
            {


                a.Details.ForEach(c =>
                {
                    DataRow row = dt.NewRow();
                    Type preOrderType = a.GetType();
                    Type preOrderTypeAddress = a.OrderAddress.GetType();

                    Type receiptDetailType = c.GetType();
                    headerTableColumn.ForEach(h =>
                    {
                        if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
                        {
                            PropertyInfo property = preOrderType.GetProperty(h.DbColumnName);
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
                            PropertyInfo property = receiptDetailType.GetProperty(d.DbColumnName);
                            row[d.DisplayName] = property.GetValue(c);

                        }
                    });

                    orderAddressTableColumn.ForEach(d =>
                    {
                        if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
                        {
                            PropertyInfo property = preOrderTypeAddress.GetProperty(d.DbColumnName);
                            row[d.DisplayName] = property.GetValue(a.OrderAddress);
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
                //由于框架约定大于配置， 数据库的字段首字母小写
                //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
                DbColumnName = a.DbColumnName,
                Type = a.Type,
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