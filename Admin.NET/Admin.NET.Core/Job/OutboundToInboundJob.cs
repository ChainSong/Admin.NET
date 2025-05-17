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
using Admin.NET.Core.Dto;
using Admin.NET.Core.Dtos;
using Admin.NET.Core.Dtos.Enum;
using Admin.NET.Core.Entity;
using Common.Utility;
using Furion.Logging.Extensions;
using SqlSugar;
using System.Data;
using System.Security.AccessControl;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.SemanticSemproxySearchResponse.Types;

namespace Admin.NET.Core;

/// <summary>
/// 清理在线用户作业任务
/// </summary>
[JobDetail("OutboundToInboundJob", Description = "出库转入库对接", GroupName = "OutboundToInboundJob", Concurrent = false)]
//[Daily(TriggerId = "OutboundToInboundJob", Description = "出库转入库对接")]
[PeriodSeconds(2, TriggerId = "OutboundToInboundJob", Description = "出库转入库对接", MaxNumberOfRuns = 0, RunOnStart = false)]
public class OutboundToInboundJob : IJob
{
    private readonly IServiceProvider _serviceProvider;

    private const string ALLOCATEKEY = "OutboundToInboundJob";
    public OutboundToInboundJob(IServiceProvider serviceProvider)
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
            lockFlag = _cache.AcquireLock(ALLOCATEKEY, 2000, 15000, false);
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
                var data = db.Ado.SqlQuery<WMSInstruction>("  select distinct CustomerName,Creator,OperationId  from WMS_Instruction where \r\n BusinessType ='HACH出库同步下发'  and InstructionStatus = 1 \r\n  group by CustomerName,Creator,OperationId");

                if (data != null && data.Count > 0)
                {
                    //List<WMSAdjustmentInformationDo> AdjustmentInfo = new List<WMSAdjustmentInformationDo>();
                    //根据出库数据构建asn信息
                    //获取出库信息
                    //根据出库信息构建AddOrUpdateWMSASNInput信息
                    foreach (var item in data)
                    {
                        try
                        {

                            //获取出库订单信息
                            var order = await db.Queryable<WMSOrder>().Includes(a => a.Details).Includes(a => a.OrderAddress).Where(a => a.CustomerName == item.CustomerName && a.Id == item.OperationId).FirstAsync();
                            //获取出库的RFID信息
                            var orderRFID = await db.Queryable<WMSRFIDInfo>().Where(a => a.CustomerName == item.CustomerName && a.OrderNumber == order.OrderNumber && a.Status == 99).ToListAsync();

                            //根据联系公司名称补充公司名称
                            //var customer = await db.Queryable<WMSCustomer>().Where(a => a.CustomerName == order.OrderAddress.CompanyName).FirstAsync();
                            //根据发货地址获取仓库信息
                            //var warehouse = await db.Queryable<WMSWarehouse>().Where(a => a.Address == order.OrderAddress.Address).FirstAsync();
                            if ( string.IsNullOrEmpty(order.OrderAddress.Address))
                            {
                                await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 20, Message = "获取仓库错误，请核对发货地址" }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();
                                continue;
                            }

                            if (string.IsNullOrEmpty(order.OrderAddress.CompanyName))
                            {
                                await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 20, Message = "获取客户错误，请核对联系人公司" }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();

                                continue;
                            }

                            var interfaceAddress = db.Ado.SqlQuery<WMSInterfaceAddressConfiguration>("  select top 1 * from  WMS_InterfaceAddressConfiguration where WarehouseName='" + order.WarehouseName + "' ");

                            if (order != null)
                            {
                                //构建asn信息
                                AddOrUpdateWMSASNInput wMSASN = new AddOrUpdateWMSASNInput();
                                wMSASN.ExternReceiptNumber = order.ExternOrderNumber;
                                //wMSASN.CustomerId = customer.Id;
                                wMSASN.CustomerName = order.OrderAddress.CompanyName;
                                //wMSASN.WarehouseId = warehouse.Id;
                                wMSASN.WarehouseName = order.OrderAddress.Address;
                                wMSASN.ExpectDate = DateTime.Now;
                                wMSASN.ReceiptType = "采购入库";
                                wMSASN.ASNStatus = 1;
                                wMSASN.Details = new List<WMSASNDetail>();
                                wMSASN.RFIDDetails = new List<WMSRFIDInfo>();
                                //构建明细信息
                                int lineNumber = 1;
                                foreach (var detail in order.Details)
                                {
                                    WMSASNDetail asnDetail = new WMSASNDetail();
                                    asnDetail.ExternReceiptNumber = order.ExternOrderNumber;
                                    asnDetail.SKU = detail.SKU;
                                    asnDetail.GoodsName = detail.GoodsName;
                                    asnDetail.GoodsType = detail.GoodsType;
                                    asnDetail.UnitCode = detail.UnitCode;
                                    asnDetail.PoCode = detail.PoCode;
                                    asnDetail.SoCode = detail.SoCode;
                                    asnDetail.ExpectedQty = detail.AllocatedQty;
                                    asnDetail.Onwer = detail.Onwer;
                                    asnDetail.LineNumber = lineNumber.ToString().PadLeft(5, '0'); ;
                                    wMSASN.Details.Add(asnDetail);
                                    lineNumber++;
                                }
                                foreach (var detail in orderRFID)
                                {
                                    WMSRFIDInfo rfidInfo = new WMSRFIDInfo();
                                    rfidInfo.SKU = detail.SKU;
                                    //rfidInfo.CustomerId = wMSASN.CustomerId;
                                    rfidInfo.CustomerName = wMSASN.CustomerName;
                                    rfidInfo.WarehouseName = order.OrderAddress.Address;
                                    //rfidInfo.WarehouseId = warehouse.Id;
                                    //rfidInfo.GoodsName = detail.GoodsName;
                                    rfidInfo.GoodsType = detail.GoodsType;
                                    //rfidInfo.UnitCode = detail.UnitCode;
                                    rfidInfo.PoCode = detail.PoCode;
                                    rfidInfo.SoCode = detail.SoCode;
                                    rfidInfo.SnCode = detail.SnCode;
                                    rfidInfo.Qty = 1;
                                    rfidInfo.Sequence = detail.Sequence;
                                    rfidInfo.RFID = detail.RFID;
                                    rfidInfo.Qty = 1;
                                    rfidInfo.BatchCode = detail.BatchCode;
                                    rfidInfo.PrintNum = 0;
                                    rfidInfo.BatchCode = detail.BatchCode;
                                    rfidInfo.Creator = item.Creator;
                                    wMSASN.RFIDDetails.Add(rfidInfo);
                                }

                                var response = await HttpHelper.PostAsync<HttpResponse<Response<List<OrderStatusDto>>>>(interfaceAddress.First().Url, wMSASN);

                                if (response.Code == 200)
                                {
                                    if (response.Result.Code == StatusCode.Success)
                                    {
                                        await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 99, Message = "" }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();
                                    }
                                    else
                                    {
                                        var message = "";
                                        if (response.Result.Data != null && response.Result.Data.Count > 0)
                                        {
                                            message = response.Result.Data.First().Msg;
                                        }
                                        await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 20, Message = response.Result.Msg + message }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();

                                    }
                                }
                                else { 
                                        await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 20, Message = "500" }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();

                                }
                            }
                            //获取配置信息
                        }
                        catch (Exception ex)
                        {
                            await db.Updateable<WMSInstruction>().SetColumns(it => new WMSInstruction() { InstructionStatus = 20, Message = ex.Message }).Where(it => it.OperationId == item.OperationId && it.BusinessType == "HACH出库同步下发").ExecuteCommandAsync();

                        }
                    }
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