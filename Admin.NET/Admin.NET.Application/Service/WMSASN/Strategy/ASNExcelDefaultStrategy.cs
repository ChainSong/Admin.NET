
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
        public ASNExcelDefaultStrategy(
        )
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

            var headerTableColumn = GetColumns("WMS_ASN");
            var detailTableColumn = GetColumns("WMS_ASNDetail");



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
                //验证数据
                if (Column.Validation == "Required")
                {
                    int flag = 0;


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
                                Msg = Column.DisplayName + "不能为空"
                            });
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
        public Response<DataTable> Export(List<long> request)
        {
            Response<DataTable> response = new Response<DataTable>();
            //CreateOrUpdateWMS_ReceiptInput orders = new CreateOrUpdateWMS_ReceiptInput();
            //orders.WMS_Receipts = new List<WMSReceiptEditDto>();

            var headerTableColumn = GetColumns("WMS_ASN");
            var detailTableColumn = GetColumns("WMS_ASNDetail");
            var orderData = _repASN.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();


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
