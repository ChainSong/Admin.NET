using Newtonsoft.Json;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Utilities;
using TaskPlaApplication.Domain.Entities;

namespace TaskPlaApplication.Application.Services.Handlers;

public sealed class OutboundInstructionHandler : IInstructionHandler
{
    public InstructionType Type => InstructionType.出库单回传HachDG;
    private readonly ISqlSugarClient _db;
    ExternalFeedbackClient _external = new ExternalFeedbackClient();
    private readonly IThirdPartyConfigProvider _provider;
    Logger _log = new Logger();
    public OutboundInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
    {
        _db = db;
        _provider = provider; // 正确初始化 _provider
    }
    public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
    {
        // —— 入参校验（读库之前）———————————————————————————————
        if (ins == null)
            return new HandlerResult(false, "Instruction is null");
        //HachWmsOutBoundConfirm hachWmsOutBoundConfirm = new HachWmsOutBoundConfirm();

        var lastHwr = await _db.Queryable<HachWmsOutBound>()
              .Where(x => x.OrderNumber == ins.OrderNumber && x.IsDelete == false)
              .OrderBy(x => x.CreateTime, OrderByType.Desc)
              .FirstAsync();

        if (lastHwr == null)
        {
            _log.Log("获取对接订单信息失败");
            return new HandlerResult(false, "获取对接订单信息失败");
        }

        //回传主信息
        var ContentMain = await _db.Queryable<WMSOrder>()
            .LeftJoin<HachWmsOutBound>((wo, hob) => wo.ExternOrderNumber == hob.OrderNumber)
            .Where((wo, hob) => wo.ExternOrderNumber == ins.OrderNumber && hob.OrderNumber == ins.OrderNumber)
            .Where((wo, hob) => wo.Id == ins.OperationId)
            .Select((wo, hob) => new
            {
                CustomerId = wo.CustomerId,
                CustomerName = wo.CustomerName,
                soNumber = hob.SoNumber,
                deliveryDate = wo.CompleteTime.ToString(),
                deliveryNumber = hob.DeliveryNumber,
                WarehouseId = wo.WarehouseId,
                WarehouseName = wo.WarehouseName
            }).FirstAsync();

        if (ContentMain == null)
        {
            _log.Log("获取订单信息失败");
            return new HandlerResult(false, "获取订单信息失败");
        }

        string Sql = string.Empty;
        if (ins.InstructionPriority == 63)
        {
            Sql = $@"SELECT hod.DeliveryDetailId AS deliveryDetailId,hod.LineNumber AS lineNum,hod.OrganizationId AS organizationId,
                        hod.OrganizationCode AS organizationCode,hod.ItemId  AS itemId,hod.ItemNumber AS itemNum,
                        hod.ItemDescription  AS itemDescription,ISNULL(od.AllocatedQty, 0)AS shippedQuantity,hod.Uom AS uom,
                        ho.Subinventory AS subinventory,FORMAT((SELECT TOP 1 PackageTime FROM WMS_Package WHERE ExternOrderNumber = @OrderNumber ORDER BY PackageTime DESC), 'yyyy-MM-dd HH:mm:ss') AS shipConfirmDate,
                        CONVERT(VARCHAR(50), @InstructionPriority)  AS attribute1,hod.Attribute2,hod.Attribute3,hod.Attribute4,
                        hod.Attribute5,hod.Attribute6,hod.Attribute7,hod.Attribute8,hod.Attribute9,hod.Attribute10
                        FROM hach_wms_outBound ho LEFT JOIN hach_wms_outBound_detail hod  ON ho.Id = hod.OutBoundId
                        LEFT JOIN WMS_Order wo  ON ho.OrderNumber = wo.ExternOrderNumber AND wo.Id =@OperationId
                        LEFT JOIN WMS_OrderDetail od ON od.OrderId = wo.Id AND od.SKU = hod.ItemNumber 
                        AND od.Str1 = CAST(hod.DeliveryDetailId AS VARCHAR(50))
                        WHERE ho.OrderNumber = @OrderNumber;";
        }
        else
        {
            Sql = $@"SELECT hod.DeliveryDetailId AS deliveryDetailId,hod.LineNumber AS lineNum,hod.OrganizationId AS organizationId,
                        hod.OrganizationCode AS organizationCode,hod.ItemId  AS itemId,hod.ItemNumber AS itemNum,
                        hod.ItemDescription  AS itemDescription,ISNULL(od.AllocatedQty, 0)AS shippedQuantity,hod.Uom AS uom,
                        ho.Subinventory AS subinventory,FORMAT(wo.CompleteTime, 'yyyy-MM-dd HH:mm:ss') AS shipConfirmDate,
                        CONVERT(VARCHAR(50), @InstructionPriority)  AS attribute1,hod.Attribute2,hod.Attribute3,hod.Attribute4,
                        hod.Attribute5,hod.Attribute6,hod.Attribute7,hod.Attribute8,hod.Attribute9,hod.Attribute10
                        FROM hach_wms_outBound ho LEFT JOIN hach_wms_outBound_detail hod  ON ho.Id = hod.OutBoundId
                        LEFT JOIN WMS_Order wo  ON ho.OrderNumber = wo.ExternOrderNumber AND wo.Id =@OperationId
                        LEFT JOIN WMS_OrderDetail od ON od.OrderId = wo.Id AND od.SKU = hod.ItemNumber 
                        AND od.Str1 = CAST(hod.DeliveryDetailId AS VARCHAR(50))
                        WHERE ho.OrderNumber = @OrderNumber;";
        }

        // 参数
        var parameters = new
        {
            InstructionPriority = ins.InstructionPriority,  // 使用枚举转换为字符串
            OperationId = ins.OperationId,
            OrderNumber = ins.OrderNumber
        };

        // 直接查成 DTO 列表
        var ContentItems = _db.Ado.SqlQuery<HachWmsOutBoundDetailConfirm>(Sql, parameters);

        if (ContentItems == null || ContentItems.Count == 0)
        {
            _log.Log("获取订单明细信息失败");
            return new HandlerResult(false, "获取订单明细信息失败");
        }
        // 检查每一条记录是否符合条件
        bool allItemsValid = ContentItems.All(item =>
            item.organizationId.HasValue && item.organizationId != 0
            && !string.IsNullOrEmpty(item.itemNum)
            && !string.IsNullOrEmpty(item.itemDescription)
        //&& item.shippedQuantity != 0
        );

        if (!allItemsValid)
        {
            _log.Log("获取订单明细信息失败，某些条目无效");
            return new HandlerResult(false, "获取订单明细信息失败，某些条目无效");
        }

        // 找出重复的 deliveryDetailId（排除 null/空串视情况）
        var duplicateTransactionIds = ContentItems
                                     .GroupBy(x => x.deliveryDetailId)
                                     .Where(g => g.Count() > 1)
                                     .Select(g => g.Key)
                                     .ToList();

        if (duplicateTransactionIds.Any())
        {
            var msg = $"deliveryDetailId：{string.Join(",", duplicateTransactionIds)}";
            _log.Log(msg);
            return new HandlerResult(false, msg);
        }

        /// 检查 shipConfirmDate 是否有 null 或 空值
        if (ContentItems.Where(a => a.shipConfirmDate == null).Any())
        {
            var msg = $"shipConfirmDate：so:{ContentMain.soNumber}  dn:{ContentMain.deliveryNumber}  shipConfirmDate为空";
            _log.Log(msg);
            return new HandlerResult(false, msg);
        }

        var hachWmsOutBoundConfirm = new HachWmsOutBoundConfirm
        {
            soNumber = ContentMain.soNumber,
            deliveryNum = ContentMain.deliveryNumber,
            items = ContentItems
        };

        hachWmsOutBoundConfirm.items = ContentItems;
        var requestBody = new List<HachWmsOutBoundConfirm> { hachWmsOutBoundConfirm };

        //获取 url地址

        HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
        {
            Type = "HachWMSConfirmApi",
            InterfaceName = "SOB",
            CustomerId = ContentMain.CustomerId,
            WarehouseId = ContentMain.WarehouseId
        };

        var Authentication = await _provider.GetAsync(input, ct);

        if (Authentication == null)
        {
            _log.Log("未能获取到客户或目标仓库身份鉴权信息");
            return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
        }

        var json1 = JsonConvert.SerializeObject(requestBody);
        _log.Log($"ExternOrderNumber:{hachWmsOutBoundConfirm.deliveryNum}出库回传报文：{json1}");


        OutboundResponseData? confirmResp = new OutboundResponseData();
       

        if (requestBody == null || requestBody.Count == 0)
        {
            return new HandlerResult(false, "获取Body信息失败");
        }

        try
        {
            confirmResp = await _external.SendPostAsync<List<HachWmsOutBoundConfirm>, OutboundResponseData>(
                url: Authentication.Url,
                body: requestBody,
                bearerToken: Token,
                ct: ct
            );
        }
        catch (Exception ex)
        {
            _log.Log($"回传出库失败：{ex.Message}");
            return new HandlerResult(false, $"回传出库失败：{ex.Message}");
        }
        // —— 根据返回判定成功与否（具体字段以你的返回模型为准）————————
        var ok = confirmResp?.success == true || string.Equals(confirmResp?.message, "成功", StringComparison.OrdinalIgnoreCase);
        var json = JsonConvert.SerializeObject(requestBody);
        var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Outbound" };
        return new HandlerResult(ok, ok ? null : (confirmResp?.message ?? "失败"), callback);
    }
}