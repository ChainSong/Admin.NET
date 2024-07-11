// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using System.Data;
using AngleSharp.Html.Dom;

namespace Admin.NET.Application;
public class ImportDefaultStrategy : IImportDefaultInterface
{

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
    public UserManager _userManager { get; set; }


    public List<string> _tableNames { get; set; }

    public ImportDefaultStrategy(List<string> tableNames, SqlSugarRepository<TableColumns> repTableColumns, UserManager userManager)
    {
        this._tableNames = tableNames;
        this._repTableColumns = repTableColumns;
        this._userManager = userManager;

    }


    /// <summary>
    /// 导入
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    //默认方法不做任何处理
    public virtual Response<DataTable, List<OrderStatusDto>> Import(dynamic request)
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
                StatusCode = StatusCode.Warning,
                //StatusMsg = (string)StatusCode.warning,
                Msg = "模板更新，请重新下载模板"
            });
            response.Result = statusDtos;
            response.Data = request;
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
    public virtual Response<DataTable> Export(List<long> request)
    {
        Response<DataTable> response = new Response<DataTable>();
        //CreateOrUpdateWMS_ReceiptInput orders = new CreateOrUpdateWMS_ReceiptInput();
        //orders.WMS_Receipts = new List<WMSReceiptEditDto>();
        //var orderDatas = _repASN.AsQueryable()
        //    //.LeftJoin<WMSASNDetail>（）
        //    .LeftJoin<WMSASNDetail>((a,b)=>a.Id==b.ASNId).Where(a => request.Contains(a.Id)).Select((a,b) => new
        //    {
        //        a,
        //        b
        //    }).ToList();

        //var headerTableColumn = GetColumns("WMS_ASN");
        //var detailTableColumn = GetColumns("WMS_ASNDetail");

        List<TableColumns> tableColumnList = new List<TableColumns>();
        foreach (var item in _tableNames)
        {
            tableColumnList.AddRange(GetColumns(item));
        }
        //var orderData = _repASN.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();


        DataTable dt = new DataTable();
        DataColumn dc = new DataColumn();

        //1，构建主表需要的信息
        tableColumnList.ForEach(a =>
        {
            if (a.IsImportColumn == 1)
            {
                dc = dt.Columns.Add(a.DisplayName, typeof(string));
            }
        });
        //2.构建明细需要的信息
        //detailTableColumn.ForEach(a =>
        //{
        //    if (a.IsImportColumn == 1 && !dt.Columns.Contains(a.DisplayName))
        //    {
        //        dc = dt.Columns.Add(a.DisplayName, typeof(string));
        //    }
        //});


        //塞数据
        //request.ForEach(a =>
        //{

        //    Type orderType = a.GetType();
        //    a.Details.ForEach(c =>
        //    {
        //        DataRow row = dt.NewRow();
        //        Type orderDetailType = c.GetType();
        //        headerTableColumn.ForEach(h =>
        //        {
        //            if (h.IsImportColumn == 1 && dt.Columns.Contains(h.DisplayName))
        //            {
        //                PropertyInfo property = orderType.GetProperty(h.DbColumnName);
        //                //如果该字段有下拉选项，则值取下拉选项中的值
        //                if (h.tableColumnsDetails.Count() > 0)
        //                {
        //                    var val = property.GetValue(a);
        //                    TableColumnsDetail data = new TableColumnsDetail();
        //                    if (val is int)
        //                    {
        //                        data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
        //                    }
        //                    else
        //                    {
        //                        data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val)).FirstOrDefault();

        //                    }
        //                    //var data = h.tableColumnsDetails.Where(c => c.CodeStr == Convert.ToString(val) || c.CodeInt == Convert.ToInt32(val)).FirstOrDefault();
        //                    if (data != null)
        //                    {
        //                        row[h.DisplayName] = data.Name;
        //                    }
        //                    else
        //                    {
        //                        row[h.DisplayName] = "";
        //                    }
        //                }
        //                else
        //                {
        //                    row[h.DisplayName] = property.GetValue(a);
        //                }

        //            }
        //        });

        //        detailTableColumn.ForEach(d =>
        //        {
        //            if (d.IsImportColumn == 1 && dt.Columns.Contains(d.DisplayName))
        //            {
        //                PropertyInfo property = orderDetailType.GetProperty(d.DbColumnName);
        //                row[d.DisplayName] = property.GetValue(c);

        //            }
        //        });

        //        dt.Rows.Add(row);
        //    });

        //});
        response.Data = dt;
        response.Code = StatusCode.Success;
        //throw new NotImplementedException();
        return response;
    }

    public List<TableColumns> GetColumns(string TableName)
    {
      try
      {


            var aaa = _repTableColumns.AsQueryable()
            .Where(a => a.TableName == TableName &&
              a.TenantId == _userManager.TenantId &&
              a.IsImportColumn == 1
            );

            var aaass = _repTableColumns.AsQueryable()
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
               //tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).ToList();
        }
      catch (Exception ex)
      {
          throw ex;
      }
    }
}
