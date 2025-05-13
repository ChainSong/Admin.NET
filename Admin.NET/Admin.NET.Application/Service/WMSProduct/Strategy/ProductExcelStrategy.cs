// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Service;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;

namespace Admin.NET.Application.Service;
public class ProductExcelStrategy : IProductExcelInterface
{

    //用户仓储
    public UserManager _userManager { get; set; }
    ////asn仓储
    //SqlSugarRepository<WMSASN> _repASN { get; set; }
    ////客户用户关系仓储
    //SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    ////仓库用户关系仓储
    //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

    //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    //表字段仓储
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public Response<DataTable> Strategy(dynamic request)
    {
        Response<DataTable> response = new Response<DataTable>();

        var headerTableColumn = GetColumns("WMS_Product");
        //var detailTableColumn = GetColumns("WMS_ASNDetail");
        List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();

        //循环datatable
        for (int i = 0; i < request.Columns.Count; i++)
        {
            //获取datatable的标头
            var s = request.Columns[i].ColumnName;
            var Column = headerTableColumn.Where(a => a.DisplayName == s).FirstOrDefault();
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

            }


            //替换下拉数据
            foreach (DataRow row in request.Rows)
            {
                if (Column.Type == "DropDownListInt" && Column.tableColumnsDetails.Where(a => a.Name == row[s].ToString()).Count() > 0)
                {
                    row[s]= Column.tableColumnsDetails.Where(a => a.Name == row[s].ToString()).FirstOrDefault().CodeInt;
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
        response.Data = request;
        response.Code = StatusCode.Success;
        //throw new NotImplementedException();
        return response;

    }
    public List<TableColumns> GetColumns(string TableName)
    {
        return _repTableColumns.AsQueryable()
           .Where(a => a.TableName == TableName &&
             a.TenantId == _userManager.TenantId &&
             (a.IsImportColumn == 1 || a.IsKey == 1)
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
