using Newtonsoft.Json;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Utilities;
using TaskPlaApplication.Domain.Entities;


namespace TaskPlaApplication.Application.Services.Handlers
{
    public class OutboundSNInstructionHandler : IInstructionHandler
    {
        public InstructionType Type => InstructionType.出库单序列号回传HachDG;
        private readonly ISqlSugarClient _db;
        ExternalFeedbackClient _external = new ExternalFeedbackClient();
        private readonly IThirdPartyConfigProvider _provider;
        Logger _log = new Logger();

        public OutboundSNInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
        {
            _db = db;
            _provider = provider; // 正确初始化 _provider
        }
        public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
        {
            // —— 入参校验（读库之前）
            if (ins == null)
            {
                _log.Log("Instruction is null");
                return new HandlerResult(false, "Instruction is null");
            }

            List<HachWmsOutBoundSNConfirm> hachWmsOutBoundSNConfirm = new List<HachWmsOutBoundSNConfirm>();

            var ContentMain = await _db.Queryable<WMSRFPackageAcquisition>()
                .LeftJoin<WMSOrder>((rfPackage, order) => rfPackage.OrderId == order.Id)
                .LeftJoin<WMSOrderDetail>((rfPackage, order, orderDetail) => rfPackage.OrderId == order.Id
                && rfPackage.OrderId == orderDetail.OrderId && rfPackage.SKU == orderDetail.SKU)
                .LeftJoin<HachWmsOutBound>((rfPackage, order, orderDetail, hachOrderBound) =>
                rfPackage.ExternOrderNumber == hachOrderBound.OrderNumber)
                .LeftJoin<HachWmsOutBoundDetail>((rfPackage, order, orderDetail, hachOrderBound, hachOrderBoundDetail) =>
                hachOrderBound.Id == hachOrderBoundDetail.OutBoundId
                && orderDetail.Str1 == hachOrderBoundDetail.DeliveryDetailId.ToString())
                .Where((rfPackage, order, orderDetail, hachOrderBound, hachOrderBoundDetail) =>
                rfPackage.Type == "SN" && (rfPackage.SN != "" || rfPackage.SN != null))
                 .Where((rfPackage, order, orderDetail, hachOrderBound, hachOrderBoundDetail) => rfPackage.ExternOrderNumber == ins.OrderNumber
                 && order.ExternOrderNumber == ins.OrderNumber && orderDetail.ExternOrderNumber == ins.OrderNumber
                 && hachOrderBound.OrderNumber == ins.OrderNumber)
                 .Where((rfPackage, order, orderDetail, hachOrderBound, hachOrderBoundDetail) => order.Id == ins.OperationId)
                 .Select((rfPackage, order, orderDetail, hachOrderBound, hachOrderBoundDetail) => new
                 {
                     docNumber = hachOrderBound.SoNumber,
                     docType = string.IsNullOrEmpty(hachOrderBound.DocType) ? "SO" : hachOrderBound.DocType,
                     lineNum = hachOrderBoundDetail.LineNumber,
                     delivery = hachOrderBound.DeliveryNumber,
                     organizationCode = hachOrderBoundDetail.OrganizationCode,
                     organizationId = hachOrderBoundDetail.OrganizationId,
                     subinventory = hachOrderBound.Subinventory,
                     itemId = hachOrderBoundDetail.ItemId,
                     itemNum = hachOrderBoundDetail.ItemNumber,
                     quantity = (float)rfPackage.Qty,
                     containerNo = rfPackage.PackageNumber,
                     lsnNumber = rfPackage.SN,
                     expirationDate = rfPackage.ExpirationDate,
                     scanDate = rfPackage.CreationTime,
                     deliveryDetailId = hachOrderBoundDetail.DeliveryDetailId,
                     attribute1 = rfPackage.Lot,
                     attribute2 = rfPackage.ExpirationDate,
                     attribute3 = hachOrderBoundDetail.ParentItemNumber,
                     attribute6 = "OUT",
                     customerId = rfPackage.CustomerId,
                     warehouseId = rfPackage.WarehouseId,
                 }).ToListAsync();

            // //回传主信息
            if (ContentMain == null || ContentMain.Count == 0)
            {
                _log.Log("获取订单信息失败");
                return new HandlerResult(false, "获取订单信息失败");
            }

            ContentMain.ForEach(item =>
            {
                hachWmsOutBoundSNConfirm.Add(new HachWmsOutBoundSNConfirm
                {
                    docNumber = item.docNumber,
                    docType = "SO",
                    lineNum = item.lineNum,
                    delivery = item.delivery,
                    organizationCode = item.organizationCode,
                    organizationId = item.organizationId,
                    subinventory = item.subinventory,
                    itemId = item.itemId,
                    itemNum = item.itemNum,
                    quantity = item.quantity,
                    lsnNumber = item.lsnNumber,
                    containerNo=item.containerNo,
                    expirationDate = item.expirationDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    scanDate = item.scanDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    deliveryDetailId = item.deliveryDetailId,
                    attribute1 = item.attribute1,
                    attribute2 = item.attribute2?.ToString("yyyy-MM-dd HH:mm:ss"),
                    attribute3 = item.attribute3,
                    attribute6 = item.attribute6,
                });
            });


            // 找出重复的 deliveryDetailId（排除 null/空串视情况）
            var duplicateTransactionIds = ContentMain
                                         .GroupBy(x => x.deliveryDetailId)
                                         .Where(g => g.Count() > 1)
                                         .Select(g => g.Key)
                                         .ToList();

            if (duplicateTransactionIds.Any())
            {
                var msg = $"deliveryDetailId：{string.Join(",", duplicateTransactionIds)}";
                _log.Log(msg);
                //return new HandlerResult(false, msg);
            }

            //获取 url地址

            HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
            {
                Type = "HachWMSConfirmApi",
                InterfaceName = "SERNO",
                CustomerId = ContentMain[0].customerId,
                WarehouseId = ContentMain[0].warehouseId
            };

            var Authentication = await _provider.GetAsync(input, ct);

            if (Authentication == null)
            {
                _log.Log("未能获取到客户或目标仓库身份鉴权信息");
                return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
            }
            var jsonstr = JsonConvert.SerializeObject(hachWmsOutBoundSNConfirm);
            _log.Log($"ExternOrderNumber{hachWmsOutBoundSNConfirm[0].delivery}回传出库装箱数据：{jsonstr}");

            if (hachWmsOutBoundSNConfirm == null || hachWmsOutBoundSNConfirm.Count == 0)
            {
                return new HandlerResult(false, "获取Body信息失败");
            }

            OutboundResponseData? confirmResp;
            try
            {
                confirmResp = await _external.SendPostAsync<List<HachWmsOutBoundSNConfirm>, OutboundResponseData>(
                    url: Authentication.Url,
                    body: hachWmsOutBoundSNConfirm,
                    bearerToken: Token,
                    ct: ct
                );
            }
            catch (Exception ex)
            {
                _log.Log($"回传出库序列号失败：{ex.Message}");
                return new HandlerResult(false, $"回传出库序列号失败：{ex.Message}");
            }
            // —— 根据返回判定成功与否（具体字段以你的返回模型为准）————————
            var ok = confirmResp?.success == true || string.Equals(confirmResp?.message, "成功", StringComparison.OrdinalIgnoreCase);
            var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Inbound" };
            return new HandlerResult(ok, ok ? null : (confirmResp?.message ?? "失败"), callback);
        }
    }
}
