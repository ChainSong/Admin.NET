
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Interface;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Admin.NET.Application.Strategy
{
    public class ASNExcelDefaultStrategy : IASNExcelInterface
    {
        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        ////注入ASN仓储

        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        ////注入仓库关系仓储

        //public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        static List<string> _tableNames = new List<string>() { "WMS_ASN", "WMS_ASNDetail" };


        public ASNExcelDefaultStrategy()
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

            //var headerTableColumn = GetColumns("WMS_ASN");
            //var detailTableColumn = GetColumns("WMS_ASNDetail");

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
                    Msg = "模板更新，请重新下载模板"
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
                        // 如果该值是下拉的，那么必须使用下拉列表中的数据
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
                        }

                    }

                }
                //判断标头与key是否相等
                if (s.Equals(Column.DisplayName))
                {
                    //相等替换掉原来的表头
                    request.Columns[i].ColumnName = Column.DbColumnName;
                }


                //var config = new MapperConfiguration(cfg => cfg.CreateMap<DataTable, WMS_ASN>() );
                //var mapper = new Mapper(config);
                //var asnData = mapper.Map<List<WMS_ASN>>(request);


            }

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
            //response.Code = StatusCode.Success;
            //throw new NotImplementedException();
            return response;

        }


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Response<DataTable> Export(WMSASNExcelInput request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_Receiptrequest orders = new CreateOrUpdateWMS_Receiptrequest();
            //orders.WMS_Receipts = new List<WMSReceiptEditDto>();
            //var orderDatas = _repASN.AsQueryable()
            //    //.LeftJoin<WMSASNDetail>（）
            //    .LeftJoin<WMSASNDetail>((a,b)=>a.Id==b.ASNId).Where(a => request.Contains(a.Id)).Select((a,b) => new
            //    {
            //        a,
            //        b
            //    }).ToList();

            var headerTableColumn = GetColumns("WMS_ASN");
            var detailTableColumn = GetColumns("WMS_ASNDetail");


            var query = _repASN.AsQueryable().Includes(a=>a.Details)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.ASNNumber), u => u.ASNNumber.Contains(request.ASNNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.ExternReceiptNumber), u => u.ExternReceiptNumber.Contains(request.ExternReceiptNumber.Trim()))
                    .WhereIF(request.CustomerId > 0, u => u.CustomerId == request.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.CustomerName), u => u.CustomerName.Contains(request.CustomerName.Trim()))
                    .WhereIF(request.WarehouseId > 0, u => u.WarehouseId == request.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.WarehouseName), u => u.WarehouseName.Contains(request.WarehouseName.Trim()))
                    .WhereIF(request.ASNStatus != 0, u => u.ASNStatus == request.ASNStatus)
                    .WhereIF(!string.IsNullOrWhiteSpace(request.ReceiptType), u => u.ReceiptType.Contains(request.ReceiptType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.Po), u => u.Po.Contains(request.Po.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(request.So), u => u.So.Contains(request.So.Trim()))
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
                    //.WhereIF(request.Int1 > 0, u => u.Int1 == request.Int1)
                    //.WhereIF(request.Int2 > 0, u => u.Int2 == request.Int2)
                    //.WhereIF(request.Int3 > 0, u => u.Int3 == request.Int3)
                    //.WhereIF(request.Int4 > 0, u => u.Int4 == request.Int4)
                    //.WhereIF(request.Int5 > 0, u => u.Int5 == request.Int5)
                    .WhereIF(request.Ids!= null && request.Ids.Count > 0, u => request.Ids.Contains(u.Id))
                    //.Where(a=>_repCustomerUser.AsQueryable().Where(b=>b.CustomerId==a.CustomerId).Count()>0)
                    //.Where(a=>_repWarehouseUser.AsQueryable().Where(b=>b.WarehouseId==a.WarehouseId).Count()>0)
                    .Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    .Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)
                    .Select<WMSASN>()
;
            if (request.ExpectDate != null && request.ExpectDate.Count > 0)
            {
                DateTime? start = request.ExpectDate[0];
                query = query.WhereIF(start.HasValue, u => u.ExpectDate > start);
                if (request.ExpectDate.Count > 1 && request.ExpectDate[1].HasValue)
                {
                    var end = request.ExpectDate[1].Value.AddDays(1);
                    query = query.Where(u => u.ExpectDate < end);
                }
            }
            if (request.CompleteTime != null && request.CompleteTime.Count > 0)
            {
                DateTime? start = request.CompleteTime[0];
                query = query.WhereIF(start.HasValue, u => u.CompleteTime > start);
                if (request.CompleteTime.Count > 1 && request.CompleteTime[1].HasValue)
                {
                    var end = request.CompleteTime[1].Value.AddDays(1);
                    query = query.Where(u => u.CompleteTime < end);
                }
            }
            if (request.CreationTime != null && request.CreationTime.Count > 0)
            {
                DateTime? start = request.CreationTime[0];
                query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
                if (request.CreationTime.Count > 1 && request.CreationTime[1].HasValue)
                {
                    var end = request.CreationTime[1].Value.AddDays(1);
                    query = query.Where(u => u.CreationTime < end);
                }
            }
            if (request.DateTime1 != null && request.DateTime1.Count > 0)
            {
                DateTime? start = request.DateTime1[0];
                query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
                if (request.DateTime1.Count > 1 && request.DateTime1[1].HasValue)
                {
                    var end = request.DateTime1[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime1 < end);
                }
            }
            if (request.DateTime2 != null && request.DateTime2.Count > 0)
            {
                DateTime? start = request.DateTime2[0];
                query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
                if (request.DateTime2.Count > 1 && request.DateTime2[1].HasValue)
                {
                    var end = request.DateTime2[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime2 < end);
                }
            }
            if (request.DateTime3 != null && request.DateTime3.Count > 0)
            {
                DateTime? start = request.DateTime3[0];
                query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
                if (request.DateTime3.Count > 1 && request.DateTime3[1].HasValue)
                {
                    var end = request.DateTime3[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime3 < end);
                }
            }
            if (request.DateTime4 != null && request.DateTime4.Count > 0)
            {
                DateTime? start = request.DateTime4[0];
                query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
                if (request.DateTime4.Count > 1 && request.DateTime4[1].HasValue)
                {
                    var end = request.DateTime4[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime4 < end);
                }
            }
            if (request.DateTime5 != null && request.DateTime5.Count > 0)
            {
                DateTime? start = request.DateTime5[0];
                query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
                if (request.DateTime5.Count > 1 && request.DateTime5[1].HasValue)
                {
                    var end = request.DateTime5[1].Value.AddDays(1);
                    query = query.Where(u => u.DateTime5 < end);
                }
            }
            var orderData = query.Includes(a => a.Details).ToList();


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
            orderData.ForEach(a =>
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


        public List<TableColumns> GetColumns(string TableName)
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
                  Validation = a.Validation,
                  IsImportColumn = a.IsImportColumn,
                  tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
                  //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
                  //.Select()
              }).ToList();
        }
    }
}
