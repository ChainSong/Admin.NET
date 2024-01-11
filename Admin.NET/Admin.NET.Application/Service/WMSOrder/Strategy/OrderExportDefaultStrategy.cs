// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

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
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;

namespace Admin.NET.Application.Service.Strategy;
public class OrderExportDefaultStrategy : IOrderExcelInterface
{

    public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

    public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public UserManager _userManager { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }

    public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }

    public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }



    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

    public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }

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

        var headerTableColumn = GetColumns("WMS_Order");
        var detailTableColumn = GetColumns("WMS_OrderDetail");
        var orderData = _repOrder.AsQueryable().Includes(a => a.Details).Where(a => request.Contains(a.Id)).ToList();


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
        orderData.ForEach(a =>
        {
            DataRow row = dt.NewRow();
            Type orderType = a.GetType();
            a.Details.ForEach(c =>
            {
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
            });
            dt.Rows.Add(row);

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
//}