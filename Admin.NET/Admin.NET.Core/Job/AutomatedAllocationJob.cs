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
[JobDetail("AutomatedAllocationJobJob", Description = "自动分配", GroupName = "default", Concurrent = false)]
//[Daily(TriggerId = "AutomatedAllocationJobJob", Description = "自动分配")]
[PeriodSeconds(2, TriggerId = "AutomatedAllocationJobJob", Description = "自动分配", MaxNumberOfRuns = 1, RunOnStart = false)]

public class AutomatedAllocationJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    private const string ALLOCATEKEY = "AutomatedAllocationJob";


    public AutomatedAllocationJob(IServiceProvider serviceProvider)
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
                try
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
                               .Where(a => a.WorkName == instruction.CustomerName + OutboundWorkFlowConst.Workflow_Outbound).FirstAsync();

                            if (workflow != null && !string.IsNullOrEmpty(workflow.WorkName))
                            {
                                //判断有没有子流程
                                if (workflow.SysWorkFlowSteps.Count > 0)
                                {
                                    //从子流程中获取分配规则
                                    var customWorkFlow = workflow.SysWorkFlowSteps.Where(a => a.StepName == OutboundWorkFlowConst.Workflow_Automated_Outbound).FirstOrDefault();
                                    if (customWorkFlow != null && !string.IsNullOrEmpty(customWorkFlow.Filters))
                                    {
                                        List<SysWorkFlowFieldDto> sysWorkFlowFieldDtos = JsonConvert.DeserializeObject<List<SysWorkFlowFieldDto>>(customWorkFlow.Filters);

                                    }
                                    else
                                    {
                                        if (customWorkFlow != null)
                                        {
                                            procedureName = customWorkFlow.Remark;
                                        }
                                    }
                                }
                            }
                            var sugarParameter = new SugarParameter("@InstructionTaskNo", instruction.InstructionTaskNo, typeof(string), ParameterDirection.Input);
                            db.Ado.CommandTimeOut = 1800;
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
                                notice.Content = "<table border='1'><tr><th>订单号</th><th>SKU</th><th>订单数量</th><th>库存数量</th><th>备注</th></tr>";

                                foreach (var item in AdjustmentInfo)
                                {
                                    if (item.Qty != item.InventoryQty)
                                    {
                                        notice.Content += "<tr><td>" + item.OrderNumber + "</td><td>" + item.SKU + "</td><td>" + item.Qty + "</td><td>" + item.InventoryQty + "</td><td>请核对其他订单要求：如品级，批次，单位</td></tr>";
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
                        }
                    }
                }
                catch (Exception)
                {
                     
                }
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