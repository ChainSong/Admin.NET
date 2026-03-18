using Newtonsoft.Json;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Utilities;
using TaskPlaApplication.Domain.Entities;

namespace TaskPlaApplication.Application.Services.Handlers;

public sealed class InboundInstructionHandler : IInstructionHandler
{
    public InstructionType Type => InstructionType.入库单回传HachDG;
    private readonly ISqlSugarClient _db;
    ExternalFeedbackClient _external = new ExternalFeedbackClient();
    private readonly IThirdPartyConfigProvider _provider;

    Logger _log = new Logger();
    public InboundInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
    {
        _db = db;
        _provider = provider; // 正确初始化 _provider
    }

    public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
    {
        // —— 入参校验（读库之前）———————————————————————————————
        if (ins == null)
            return new HandlerResult(false, "Instruction is null");

        var lastHwr = await _db.Queryable<HachWmsReceiving>()
                      .Where(x => x.OrderNo == ins.OrderNumber && x.IsDelete == false)
                      .OrderBy(x => x.CreateTime, OrderByType.Desc)
                      .FirstAsync();
        if (lastHwr == null)
        {
            _log.Log("获取对接订单信息失败");
            return new HandlerResult(false, "获取对接订单信息失败");
        }

        //对接的信息
        var dockingReceipt = await _db.Queryable<HachWmsReceiving>()
            .Where(hr => hr.OrderNo == ins.OrderNumber)
            .Includes(hr=> hr.items)
            .OrderByDescending(hr => hr.CreateTime)
            .FirstAsync();

        // 获取对接明细信息并统计SKU和数量
        var dockingDetails = dockingReceipt?.items ?? new List<HachWmsReceivingDetail>();
        // 统计每个SKU的数量
        var dockingSKUQuantity = dockingDetails
            .GroupBy(d => d.ItemNum)
            .ToDictionary(g => g.Key, g => g.Sum(d => d.Quantity));  

        //回传业务主信息
        var ContentMain = await _db.Queryable<WMSReceipt>()
            .Where(wa => wa.ExternReceiptNumber == ins.OrderNumber && wa.ReceiptStatus == 99)
            .Where(wa => wa.Id == ins.OperationId)
            .Select(wa => new
            {
                CustomerId = wa.CustomerId,
                CustomerName = wa.CustomerName,
                receiptNum = lastHwr.ReceiptNum,
                //deliveryDate = wa.CompleteTime.ToString(),
                deliveryDate = wa.CompleteTime,
                docNumber = lastHwr.DocNumber,
                docType = lastHwr.DocType,
                WarehouseId = wa.WarehouseId,
                WarehouseName = wa.WarehouseName
            }).FirstAsync();

        if (ContentMain == null)
        {
            _log.Log("获取订单信息失败");
            return new HandlerResult(false, "获取订单信息失败");
        }

        //回传的业务明细信息
        var ContentItems = await _db.Queryable<WMSReceipt>()
                           .LeftJoin<WMSReceiptDetail>((r, rd) => r.Id == rd.ReceiptId)
                           .LeftJoin<WMSReceiptReceiving>((r, rd, rr) => r.Id == rd.ReceiptId && rd.Id == rr.ReceiptDetailId)
                           .LeftJoin<HachWmsReceiving>((r, rd, rr, hr) => r.ExternReceiptNumber == hr.OrderNo)
                           .LeftJoin<HachWmsReceivingDetail>((r, rd, rr, hr, hrd) => hr.Id == hrd.ReceivingId && rd.SKU == hrd.ItemNum && rd.LineNumber == hrd.TransactionId.ToString())
                           .Where((r, rd, rr, hr, hrd) => hr.OrderNo == ins.OrderNumber && r.ExternReceiptNumber == ins.OrderNumber && r.Id == ins.OperationId)
                           .Select((r, rd, rr, hr, hrd) => new HachWmsReceivingDetailConfirm
                           {
                               transactionId = hrd.TransactionId,
                               sourceHeaderId = hrd.SourceHeaderId,
                               sourceLineId = hrd.SourceLineId,
                               shipmentHeaderId = hrd.ShipmentHeaderId,
                               shipmentLineId = hrd.ShipmentLineId,
                               lineNum = hrd.LineNum,
                               organizationId = hrd.OrganizationId,
                               organizationCode = hrd.OrganizationCode,
                               subinventory = hrd.Subinventory,
                               itemDescription = hrd.ItemDescription,
                               itemId = hrd.ItemId,
                               itemNum = hrd.ItemNum,
                               uom = hrd.Uom,
                               attribute1 = hrd.Attribute1,
                               attribute2 = hrd.Attribute2,
                               attribute3 = hrd.Attribute3,
                               attribute4 = hrd.Attribute4,
                               attribute5 = hrd.Attribute5,
                               quantity = (float)rd.ReceivedQty,
                           })
                           .ToListAsync();

        if (ContentItems == null)
        {
            _log.Log("获取订单明细信息失败");
            return new HandlerResult(false, "获取订单明细信息失败");
        }


        // 找出重复的 TransactionId（排除 null/空串视情况）
        var duplicateTransactionIds = ContentItems
                                      .GroupBy(x => x.transactionId)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key)
                                      .ToList();

        if (duplicateTransactionIds.Any())
        {
            var msg = $"存在重复的TransactionId：{string.Join(",", duplicateTransactionIds)}";
            _log.Log(msg);
            return new HandlerResult(false, msg);
        }

        // 统计回传的SKU和数量
        // 统计回传的每个SKU的数量
        var returnedSKUQuantity = ContentItems
            .GroupBy(ci => ci.itemNum)
            .ToDictionary(g => g.Key, g => g.Sum(ci => ci.quantity));  

        // 校验回传的SKU数量是否多于对接的数量
        foreach (var dockingItem in dockingSKUQuantity)
        {
            // 如果回传的SKU数量多于对接信息中的数量，则报错
            if (returnedSKUQuantity.ContainsKey(dockingItem.Key) && returnedSKUQuantity[dockingItem.Key] > dockingItem.Value)
            {
                _log.Log($"SKU: {dockingItem.Key}的回传数量超过了对接数量，回传数量: {returnedSKUQuantity[dockingItem.Key]}, 对接数量: {dockingItem.Value}");
                return new HandlerResult(false, $"SKU: {dockingItem.Key}的回传数量超过了对接数量");
            }
        }

        // 构建主对象
        var hachWmsReceivingConfirm = new HachWmsReceivingConfirm
        {
            receiptNum = ContentMain.receiptNum,
            deliveryDate = ContentMain.deliveryDate?.ToString("yyyy-MM-dd HH:mm:ss"),
            docNumber = ContentMain.docNumber,
            docType = ContentMain.docType,
            items = ContentItems
        };

        // 将单个对象包装成列表
        var requestBody = new List<HachWmsReceivingConfirm> { hachWmsReceivingConfirm };

        //获取 url地址

        HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
        {
            Type = "HachWMSConfirmApi",
            InterfaceName = "ASNB",
            CustomerId = ContentMain.CustomerId,
            WarehouseId = ContentMain.WarehouseId
        };

        var Authentication = await _provider.GetAsync(input, ct);

        if (Authentication == null)
        {
            _log.Log("未能获取到客户或目标仓库身份鉴权信息");
            return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
        }

        var JSON = JsonConvert.SerializeObject(requestBody);
        _log.Log($"ExternReceiptNumber:{requestBody[0].receiptNum}入库回传报文：{JSON}");

        if (requestBody == null || requestBody.Count == 0)
        {
            return new HandlerResult(false, "获取Body信息失败");
        }

        ReceivingResponseData? confirmResp = new ReceivingResponseData();
        try
        {
            confirmResp = await _external.SendPostAsync<List<HachWmsReceivingConfirm>, ReceivingResponseData>(
                url: Authentication.Url,
                body: requestBody,
                bearerToken: Token,
                ct: ct
            );
        }
        catch (Exception ex)
        {
            _log.Log($"回传入库失败：{ex.Message}");
            return new HandlerResult(false, $"回传入库失败：{ex.Message}");
        }
        // —— 根据返回判定成功与否（具体字段以你的返回模型为准）————————
        var ok = confirmResp?.success == true || string.Equals(confirmResp?.message, "成功", StringComparison.OrdinalIgnoreCase);
        var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Inbound" };
        return new HandlerResult(ok, ok ? null : (confirmResp?.message ?? "失败"), callback);
    }
}