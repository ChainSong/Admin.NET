// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。


using Admin.NET.Core.Do;
using Admin.NET.Core.Entity;
using SqlSugar;
using System.Data;

namespace Admin.NET.Core;

/// <summary>
/// 清理在线用户作业任务
/// </summary>
[JobDetail("AutomatedAllocationJobJob", Description = "自动分配", GroupName = "default", Concurrent = false)]
[Daily(TriggerId = "AutomatedAllocationJobJob", Description = "自动分配")]
//[Daily(TriggerId = "AutomatedAllocationJobJob", Description = "自动分配")]
//[PeriodSeconds(2, TriggerId = "AutomatedAllocationJobJob", Description = "清理在自动分配线用户", MaxNumberOfRuns = 1, RunOnStart = true)]

//[PeriodSeconds(1, TriggerId = "trigger_onlineUser", Description = "清理在线用户", MaxNumberOfRuns = 1, RunOnStart = true)]
public class AutomatedAllocationJob : IJob
{
    private readonly IServiceProvider _serviceProvider;




    public AutomatedAllocationJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
    {


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


        foreach (var tenant in tenantdata)
        {
            //if (tenant.Id == 1300000000001)
            //{
            //    continue;
            //}
            // 默认数据库配置
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
            var data = db.Ado.SqlQuery<WMSInstruction>("  select distinct InstructionTaskNo,CustomerName,MAX(OperationId) OperationId, Creator from WMS_Instruction where BusinessType ='自动分配'  and InstructionStatus = 1  group by InstructionTaskNo,CustomerName,Creator  order by InstructionTaskNo");
            if (data != null && data.Count > 0)
            {
                List<WMSAdjustmentInformationDo> AdjustmentInfo = new List<WMSAdjustmentInformationDo>();

                foreach (var instruction in data)
                {
                    //默认分配规则
                    string procedureName = "Proc_WMS_AutomatedOutbound";
                    //获取订单，判断订单类型
                    var repOrderData = repOrder.AsQueryable().Where(a => a.Id == instruction.OperationId).First();
                    //根据订单类型判断是否存在该流程
                    var workflow = await repWorkFlow.AsQueryable()
                       .Includes(a => a.SysWorkFlowSteps)
                       .Where(a => a.WorkName == instruction.CustomerName + repOrderData.OrderType + "自动分配").FirstAsync();

                    if (workflow != null && workflow.SysWorkFlowSteps.Count() > 0)
                    {
                        procedureName = workflow.SysWorkFlowSteps[1].StepName;
                    }
                   

                    var sugarParameter = new SugarParameter("@InstructionTaskNo", instruction.InstructionTaskNo, typeof(string), ParameterDirection.Input);
                    DataTable infoData = await db.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, sugarParameter);
                    if (infoData != null && infoData.Rows.Count > 0)
                    {
                        AdjustmentInfo.AddRange(infoData.TableToList<WMSAdjustmentInformationDo>());
                    }
                    SysNotice notice = new SysNotice();
                    notice.Title = "自动分配";
                    notice.PublicTime = DateTime.Now;
                    notice.Type = NoticeTypeEnum.NOTICE;
                    if (AdjustmentInfo.Where(a => a.Qty != a.InventoryQty).Count() > 0)
                    {
                        notice.Title = "库存不足";
                        //构建一个html的table
                        notice.Content = "<table border='1'><tr><th>订单号</th><th>SKU</th><th>订单数量</th><th>库存数量</th></tr>";

                        foreach (var item in AdjustmentInfo)
                        {
                            if (item.Qty != item.InventoryQty)
                            {
                                notice.Content += "<tr><td>" + item.OrderNumber + "</td><td>" + item.SKU + "</td><td>" + item.Qty + "</td><td>" + item.InventoryQty + "</td></tr>";
                                //notice.Content += "订单号:"+item.OrderNumber + ":SKU:" + item.SKU + "订单数量" + item.Qty + "，库存数量" + item.InventoryQty  + ";数量不足，请及时补货！\n";
                            }
                        }
                        notice.Content += "</table>";
                        //notice.Content+= "";
                    }
                    else
                    {
                        notice.Title = "分配完成";
                        notice.Content = "分配完成";
                    }
                    notice.Status = NoticeStatusEnum.PUBLIC;
                    notice.PublicOrgId = 0;
                    notice.PublicUserId = 0;
                    notice.PublicOrgName = "";
                    await repSysNotice.InsertAsync(notice);
                    instruction.InstructionStatus = 99;
                    //修改任务状态
                    //await repInstruction.UpdateAsync(data.Result);
                    db.Ado.SqlQuery<WMSInstruction>("  update WMS_Instruction set  InstructionStatus=99 where InstructionTaskNo='" + instruction.InstructionTaskNo + "' ");
                    NoticeInput noticeInput = new NoticeInput();
                    noticeInput.Id = notice.Id;
                    var userIdList = await repSysUser.AsQueryable().Where(a => a.Account == instruction.Creator).Select(u => u.Id).ToListAsync();
                    await noticeServic.PublicByUser(noticeInput, userIdList.First());

                    // 通知到的人（发布分配的人)
                    //var userIdList = await repSysUser.AsQueryable().Where(a => a.Account == instruction.Creator).Select(u => u.Id).ToListAsync();
                    ////var userIdList= data.Result.InstructionStatus.
                    //await repSysNoticeUser.DeleteAsync(u => u.NoticeId == notice.Id);
                    //var noticeUserList = userIdList.Select(u => new SysNoticeUser
                    //{
                    //    NoticeId = notice.Id,
                    //    UserId = u,
                    //}).ToList();
                    //await repSysNoticeUser.InsertRangeAsync(noticeUserList);
                }
            }
        }
        //var dasd = repWarehouse.AsTenant;




        //serviceScope.ServiceProvider.GetService<>();


        ////获取最早的任务编号，通过任务编号
        //var data = repInstruction.AsQueryable().Where(a => a.BusinessType == "自动分配" && a.InstructionStatus == 1).OrderBy(a => a.Id).FirstAsync();
        //if (data.Result != null)
        //{
        //    var sugarParameter = new SugarParameter("@InstructionTaskNo", data.Result.InstructionTaskNo, typeof(string), ParameterDirection.Input);

        //     await repSysUser.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AutomatedOutbound", sugarParameter);
        //    //var ysxx = await _db.Ado.UseStoredProcedure().GetDataTableAsync("exec Proc_WMS_AutomatedOutbound ", sugarParameter);

        //    SysNotice notice = new SysNotice();
        //    notice.Title = "自动分配";
        //    notice.PublicTime = DateTime.Now;
        //    notice.Type = NoticeTypeEnum.NOTICE;
        //    notice.Content = "分配完成";
        //    notice.Status = NoticeStatusEnum.PUBLIC;
        //    notice.PublicOrgId = 0;
        //    notice.PublicUserId = 0;
        //    notice.PublicOrgName = "";
        //    await repSysNotice.InsertAsync(notice);
        //    data.Result.InstructionStatus = 99;
        //    //修改任务状态
        //    await repInstruction.UpdateAsync(data.Result);

        //    //// 通知到的人（发布分配的人)
        //    var userIdList = await repSysUser.AsQueryable().Where(a => a.Account == data.Result.Creator).Select(u => u.Id).ToListAsync();
        //    //var userIdList= data.Result.InstructionStatus.
        //    await repSysNoticeUser.DeleteAsync(u => u.NoticeId == notice.Id);
        //    var noticeUserList = userIdList.Select(u => new SysNoticeUser
        //    {
        //        NoticeId = notice.Id,
        //        UserId = u,
        //    }).ToList();
        //    await repSysNoticeUser.InsertRangeAsync(noticeUserList);
        //}


        // 更新发布状态和时间
        //await _sysNoticeRep.UpdateAsync(u => new SysNotice() { Status = NoticeStatusEnum.PUBLIC, PublicTime = DateTime.Now }, u => u.Id == input.Id);


        //var rep = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysOnlineUser>>();
        //await rep.AsDeleteable().ExecuteCommandAsync();

        //var originColor = Console.ForegroundColor;
        //Console.ForegroundColor = ConsoleColor.Red;
        //Console.WriteLine("【" + DateTime.Now + "】服务重启清空在线用户");
        //Console.ForegroundColor = originColor;

        // 缓存多租户
        //await serviceScope.ServiceProvider.GetService<SysTenantService>().UpdateTenantCache();
    }
}