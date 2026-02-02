// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class PickTaskExportDefaultStrategy : IPickTaskExportInterface
{
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

    public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    //public async Task<Response<DataTable>> PickTaskExport(List<DeleteWMSPickTaskInput> request)
    //{


    //    Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
    //    //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
    //    var pickTaskDetailData = await _repPickTaskDetail.AsQueryable().Where(a => request.Select(a => a.Id).Contains(a.PickTaskId)).ToListAsync();
    //    //先判断状态是否正常 是否允许回退
    //    var repOrder = await _repOrder.AsQueryable().Where(a => pickTaskDetailData.Select(a => a.OrderId).Contains(a.Id)).ToListAsync();
    //    repOrder.ForEach(a =>
    //    {
    //        if (a.OrderStatus >= (int)OrderStatusEnum.交接)
    //        {
    //            response.Data.Add(new OrderStatusDto()
    //            {
    //                ExternOrder = a.ExternOrderNumber,
    //                SystemOrder = a.OrderNumber,
    //                Type = a.OrderType,
    //                StatusCode = StatusCode.Warning,
    //                Msg = "已交接订单不允许回退"
    //            });
    //        }

    //    });
    //    if (response.Data != null && response.Data.Count > 0)
    //    {
    //        return response;
    //    }
    //    //先删除包装信息，再删除拣货信息
    //    var orderIds = pickTaskDetailData.Select(a => a.OrderId).Distinct().ToList();
    //    await _repPackageDetail.DeleteAsync(a => orderIds.Contains(a.OrderId));
    //    await _repPackage.DeleteAsync(a => orderIds.Contains(a.OrderId));
    //    //删除RFID 信息
    //    //await _repRFIDInfo;
    //    _repRFIDInfo.Context.Updateable<WMSRFIDInfo>()
    //               .SetColumns(p => p.Status == (int)RFIDStatusEnum.新增)
    //               .SetColumns(p => p.PickTaskNumber == "")
    //               .SetColumns(p => p.OrderNumber == "")
    //               .SetColumns(p => p.ExternOrderNumber == "")
    //               .SetColumns(p => p.PackageNumber == "")
    //               .Where(p => repOrder.Select(e => e.OrderNumber).Contains(p.OrderNumber))
    //               .ExecuteCommand();
    //    await _repPickTaskDetail.DeleteAsync(a => orderIds.Contains(a.OrderId));
    //    await _repPickTask.DeleteAsync(a => request.Select(b => b.Id).Contains(a.Id));

    //    repOrder.ForEach(a =>
    //    {
    //        a.OrderStatus = (int)OrderStatusEnum.已分配;
    //    });
    //    await _repOrder.UpdateRangeAsync(repOrder);


    //    //先删除明细表，再删除主表
    //    //_repOrderDetail.Delete(a => request.Select(a => a.Id).Contains(a.OrderId));
    //    //_repOrder.Delete(a => request.Select(a => a.Id).Contains(a.Id));
    //    //var PreOrderIds = await receipt.Select(b => b.PreOrderId).ToListAsync();
    //    ////先更新主表，在更新明细表
    //    //await _repPreOrder.UpdateAsync(a => new WMSPreOrder { PreOrderStatus = (int)PreOrderStatusEnum.新增 }, (a => PreOrderIds.Contains(a.Id)));
    //    //await _reppreOrderDetail.UpdateAsync(a => new WMSPreOrderDetail
    //    //{
    //    //    //OrderQty = 0,
    //    //    Updator = _userManager.Account,
    //    //    UpdateTime = DateTime.Now
    //    //}, a => PreOrderIds.Contains(a.PreOrderId));

    //    //_wms_asndetailRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(c =>
    //    //{
    //    //    c.ReceiptQty = 0;
    //    //    _wms_asndetailRepository.Update(c);
    //    //}); 
    //    //先处理上架=>入库单

    //    repOrder.ForEach(a =>
    //   {
    //       response.Data.Add(new OrderStatusDto()
    //       {
    //           ExternOrder = a.ExternOrderNumber,
    //           SystemOrder = a.OrderNumber,
    //           Type = a.OrderType,
    //           StatusCode = StatusCode.Success,
    //           Msg = "操作成功"
    //       });
    //   });
    //    response.Code = StatusCode.Success;
    //    response.Msg = "操作成功";
    //    //throw new NotImplementedException();
    //    return response;
    //}
    //默认方法不做任何处理
    public async Task<Response<DataTable>> PickTaskExport(List<long> request)
    {
        Response<DataTable> response = new Response<DataTable>();
        var tableColumn = GetExportColumns("WMS_PickTaskDetail");

        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT ");
        foreach (var item in tableColumn)
        {
            sql.Append(item.TableName + "." + item.DbColumnName + " as '" + item.DisplayName + "',");
        }
        sql.Remove(sql.Length - 1, 1);
        sql.Append(@"from   WMS_PickTaskDetail  
           ");
        sql.Append("where WMS_PickTaskDetail.PickTaskId in (" + string.Join(",", request) + ")");
        var data =await _repOrder.Context.Ado.GetDataTableAsync(sql.ToString());
        response.Data = data;
        response.Code = StatusCode.Success;
        return response;

    }




    public virtual List<TableColumns> GetExportColumns(params string[] _tableNames)
    {
        var tenantId = _userManager.TenantId;
        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == tenantId &&
              (a.IsCreate == 1 || a.IsKey == 1)
            )
            .GroupBy(a => new { a.DbColumnName, a.Associated, a.IsImportColumn, a.DisplayName, a.IsKey, a.Type, a.IsCreate, a.Validation, a.TenantId })
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               TableName = SqlFunc.AggregateMax(a.TableName),
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               IsCreate = a.IsCreate,
               IsKey = a.IsKey,
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).Distinct().ToList();



    }

    public virtual List<TableColumns> GetImportColumns(params string[] _tableNames)
    {

        var tenantId = _userManager.TenantId;
        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
              a.TenantId == tenantId &&
              (a.IsImportColumn == 1 || a.IsKey == 1)
            )
           .Select(a => new TableColumns
           {
               DisplayName = a.DisplayName,
               Type = a.Type,
               IsKey = a.IsKey,
               TableName = a.TableName,
               //由于框架约定大于配置， 数据库的字段首字母小写
               //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
               DbColumnName = a.DbColumnName,
               Validation = a.Validation,
               IsImportColumn = a.IsImportColumn,
               tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
               //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
               //.Select()
           }).Distinct().ToList();

    }
}
