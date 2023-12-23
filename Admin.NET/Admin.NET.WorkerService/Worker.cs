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
using Furion.TimeCrontab;
using SqlSugar;
using System.Data;

namespace Admin.NET.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private const int delay = 1000;
    private readonly Crontab _crontab;
    //private readonly IServiceProvider _serviceProvider;
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _crontab = Crontab.Parse("* * * * * *", CronStringFormat.WithSeconds);
        //_serviceProvider = serviceProvider;
    }

    // 启动
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {


            //using var serviceScope = _serviceProvider.CreateScope();

            //// 获取指令仓储 
            //var repWarehouse = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSWarehouse>>();
            //var repInstruction = serviceScope.ServiceProvider.GetService<SqlSugarRepository<WMSInstruction>>();
            //var repSysUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysUser>>();
            //var repSysNotice = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNotice>>();
            //var repSysNoticeUser = serviceScope.ServiceProvider.GetService<SqlSugarRepository<SysNoticeUser>>();


            ////获取最早的任务编号，通过任务编号
            //var data = repInstruction.AsQueryable().Where(a => a.BusinessType == "自动分配" && a.InstructionStatus == 1).OrderBy(a => a.Id).FirstAsync();
            //if (data.Result != null)
            //{
            //    var sugarParameter = new SugarParameter("@InstructionTaskNo", data.Result.InstructionTaskNo, typeof(string), ParameterDirection.Input);

            //    await repSysUser.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AutomatedOutbound", sugarParameter);
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
            //执行时间间隔方式
            //await Task.Delay(delay, stoppingToken);
            //var taskFactory = new TaskFactory(System.Threading.Tasks.TaskScheduler.Current);
            //await taskFactory.StartNew(async () =>
            //{
            //    // 你的业务代码写到这里面
            //    _logger.LogInformation("Worker running at: {time}", DateTime.Now);
            //    await Task.CompletedTask;
            //}, stoppingToken);

            //执行表达式方式
            //await Task.Delay(_crontab.GetSleepTimeSpan(DateTime.Now), stoppingToken);

            //var taskFactory = new TaskFactory(System.Threading.Tasks.TaskScheduler.Current);
            //await taskFactory.StartNew(async () =>
            //{
            //    // 你的业务代码写到这里面

            //    _logger.LogInformation("Worker running at: {time}", DateTime.Now);

            //    await Task.CompletedTask;
            //}, stoppingToken);

            //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //await Task.Delay(1000, stoppingToken);
        }
    }


    // 停止
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

}
