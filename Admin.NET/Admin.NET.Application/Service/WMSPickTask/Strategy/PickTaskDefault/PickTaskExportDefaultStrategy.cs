// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
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

/// <summary>
/// 默认拣货任务导出策略实现类
/// </summary>
public class PickTaskExportDefaultStrategy : IPickTaskExportInterface
{
    #region 依赖注入

    /// <summary>
    /// 拣货任务仓储
    /// </summary>
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }

    /// <summary>
    /// 仓库用户映射仓储
    /// </summary>
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

    /// <summary>
    /// 客户用户映射仓储
    /// </summary>
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

    /// <summary>
    /// 用户管理服务
    /// </summary>
    public UserManager _userManager { get; set; }

    /// <summary>
    /// 系统缓存服务
    /// </summary>
    public SysCacheService _sysCacheService { get; set; }

    /// <summary>
    /// 订单仓储
    /// </summary>
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }

    /// <summary>
    /// 拣货任务明细仓储
    /// </summary>
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

    /// <summary>
    /// RFID信息仓储
    /// </summary>
    public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

    /// <summary>
    /// 表字段配置仓储
    /// </summary>
    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

    /// <summary>
    /// 包装信息仓储
    /// </summary>
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }

    /// <summary>
    /// 包装明细仓储
    /// </summary>
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

    #endregion

    #region IPickTaskExportInterface 实现

    /// <summary>
    /// 导出拣货任务数据
    /// </summary>
    /// <param name="request">拣货任务ID列表</param>
    /// <returns>包含导出数据的数据表格响应</returns>
    public async Task<Response<DataTable>> PickTaskExport(List<long> request)
    {
        // 初始化响应对象
        Response<DataTable> response = new Response<DataTable>();

        // 获取WMS_PickTaskDetail表的导出列配置
        var tableColumn = GetExportColumns("WMS_PickTaskDetail");

        // 构建动态SQL查询语句
        StringBuilder sql = new StringBuilder();
        sql.Append("SELECT ");

        // 动态拼接SELECT字段，使用配置的显示名称作为列别名
        foreach (var item in tableColumn)
        {
            sql.Append(item.TableName + "." + item.DbColumnName + " as '" + item.DisplayName + "',");
        }

        // 移除最后一个逗号
        sql.Remove(sql.Length - 1, 1);

        // 添加FROM子句
        sql.Append(@"from WMS_PickTaskDetail");

        // 添加WHERE条件，根据拣货任务ID筛选
        sql.Append(" where WMS_PickTaskDetail.PickTaskId in (" + string.Join(",", request) + ")");

        // 执行查询获取数据
        var data = await _repOrder.Context.Ado.GetDataTableAsync(sql.ToString());

        // 设置响应数据
        response.Data = data;
        response.Code = StatusCode.Success;

        return response;
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 获取导出列配置
    /// </summary>
    /// <param name="_tableNames">表名数组</param>
    /// <returns>表列配置列表</returns>
    public virtual List<TableColumns> GetExportColumns(params string[] _tableNames)
    {
        var tenantId = _userManager.TenantId;

        return _repTableColumns.AsQueryable()
            .Where(a => _tableNames.Contains(a.TableName) &&
                       a.TenantId == tenantId &&
                       (a.IsCreate == 1 || a.IsKey == 1)
            )
            // 按数据库列名、关联字段、导入列标识等字段分组
            .GroupBy(a => new { a.DbColumnName, a.Associated, a.IsImportColumn, a.DisplayName, a.IsKey, a.Type, a.IsCreate, a.Validation, a.TenantId })
            .Select(a => new TableColumns
            {
                DisplayName = a.DisplayName,
                Type = a.Type,
                // 使用聚合函数获取表名（处理多表关联情况）
                TableName = SqlFunc.AggregateMax(a.TableName),
                DbColumnName = a.DbColumnName,
                Validation = a.Validation,
                IsImportColumn = a.IsImportColumn,
                IsCreate = a.IsCreate,
                IsKey = a.IsKey,
                // 子查询获取关联的列详情信息
                tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>()
                    .Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId)
                    .ToList()
            })
            .Distinct()
            .ToList();
    }

    /// <summary>
    /// 获取导入列配置
    /// </summary>
    /// <param name="_tableNames">表名数组</param>
    /// <returns>表列配置列表</returns>
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
                DbColumnName = a.DbColumnName,
                Validation = a.Validation,
                IsImportColumn = a.IsImportColumn,
                // 子查询获取关联的列详情信息
                tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>()
                    .Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId)
                    .ToList()
            })
            .Distinct()
            .ToList();
    }

    #endregion
}
