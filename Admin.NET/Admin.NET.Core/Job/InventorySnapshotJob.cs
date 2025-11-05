// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。


using Admin.NET.Common;
using Admin.NET.Core.Do;
using Admin.NET.Core.Entity;
using Furion.Logging.Extensions;
using SqlSugar;
using System.Data;
using System.Security.AccessControl;

namespace Admin.NET.Core;

/// <summary>
/// 清理在线用户作业任务
/// </summary>
[JobDetail("InventorySnapshotJob", Description = "自动分配", GroupName = "default", Concurrent = false)]
//[Daily(TriggerId = "AutomatedAllocationJobJob", Description = "自动分配")]
[Daily(TriggerId = "trigger_InventorySnapshotJob_log", Description = "备份库存",StartTime = "11:48:00",MaxNumberOfRuns =1,RunOnStart = false)]
//[PeriodSeconds(2, TriggerId = "AutomatedAllocationJobJob", Description = "自动分配", MaxNumberOfRuns = 1, RunOnStart = false)]


public class InventorySnapshotJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    private const string ALLOCATEKEY = "AutomatedAllocationJob";


    public InventorySnapshotJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {
        return;

        using var serviceScope = _serviceProvider.CreateScope();
        // 获取指令仓储 
        var repWarehouse = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSWarehouse>>();
        var repInstruction = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSInstruction>>();
        var repSysUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysUser>>();
        var repSysNotice = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNotice>>();
        var repSysNoticeUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNoticeUser>>();
        var repSysTenant = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysTenant>>();
        var noticeServic = serviceScope.ServiceProvider.GetService<SysNoticeService>();
        var repOrder = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSOrder>>();
        var repWorkFlow = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysWorkFlow>>();
        var tenantdata = repSysTenant.AsQueryable().Select(a => a).ToList();
        var _cache = serviceScope.ServiceProvider.GetService<ICache>();
        string msg = "";
        IDisposable? lockFlag = null;
        try
        {
            //redis分布式锁 避免并发执行
            lockFlag = _cache.AcquireLock(ALLOCATEKEY, 2000, 150000, false);
            if (lockFlag == null)
            {
                return;
            }
            foreach (var tenant in tenantdata)
            {
                var defautConfig = App.GetOptions<DbConnectionOptions>().ConnectionConfigs.FirstOrDefault();

                var config = new DbConnectionConfig
                {
                    ConfigId = tenant.Id.ToString(),
                    DbType = tenant.DbType,
                    ConnectionString = tenant.Connection,
                    DbSettings = new DbSettings()
                    {
                        EnableInitDb = true,
                        EnableDiffLog = false,
                        EnableUnderLine = defautConfig.DbSettings.EnableUnderLine,
                    }
                };


                ITenant iTenant = App.GetRequiredService<ISqlSugarClient>().AsTenant();
                SqlSugarSetup.SetDbConfig(config);
                iTenant.AddConnection(config);
                using SqlSugarScopeProvider db = iTenant.GetConnectionScope(config.ConfigId);
                db.DbMaintenance.CreateDatabase();
                //db.DbMaintenance.c
                //var dasd = db.Queryable<WMSInstruction>();
                var data = db.Ado.SqlQuery<WMSInstruction>(@"   
          INSERT INTO [dbo].[WMS_Inventory_Usable_Snapshot]
           ([InventoryId]
           ,[ReceiptDetailId]
           ,[ReceiptReceivingId]
           ,[CustomerId]
           ,[CustomerName]
           ,[WarehouseId]
           ,[WarehouseName]
           ,[Area]
           ,[Location]
           ,[SKU]
           ,[UPC]
           ,[GoodsType]
           ,[InventoryStatus]
           ,[SuperId]
           ,[RelatedId]
           ,[GoodsName]
           ,[UnitCode]
           ,[Onwer]
           ,[BoxCode]
           ,[TrayCode]
           ,[BatchCode]
           ,[LotCode]
           ,[PoCode]
           ,[Weight]
           ,[Volume]
           ,[Qty]
           ,[ProductionDate]
           ,[ExpirationDate]
           ,[Remark]
           ,[InventoryTime]
           ,[InventorySnapshotTime]
           ,[Creator]
           ,[CreationTime]
           ,[Updator]
           ,[UpdateTime]
           ,[Str1]
           ,[Str2]
           ,[Str3]
           ,[Str4]
           ,[Str5]
           ,[DateTime1]
           ,[DateTime2]
           ,[Int1]
           ,[Int2]
           ,[TenantId])
     (select [Id]
      ,[ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
      ,[LotCode]
      ,[PoCode]
      ,[Weight]
      ,[Volume]
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
	  ,getdate()
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2]
      ,[TenantId] from WMS_Inventory_Usable where InventoryStatus in (1,10,20)) ");
            }
        }
        catch (Exception ex)
        {
            lockFlag?.Dispose();
            ("AutomatedAllocationJob:AutomatedAllocationJob：" + ex.Message).LogError();//用于 错误信息收集
            msg = ex.Message;
            throw Oops.Oh(ex.Message);
        }
        finally
        {
            lockFlag?.Dispose();
        }
    }
}