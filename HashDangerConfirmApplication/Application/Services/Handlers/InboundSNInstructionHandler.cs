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
    public class InboundSNInstructionHandler : IInstructionHandler
    {
        public InstructionType Type => InstructionType.入库单序列号回传HachDG;
        private readonly ISqlSugarClient _db;
        ExternalFeedbackClient _external = new ExternalFeedbackClient();
        private readonly IThirdPartyConfigProvider _provider;
       

        Logger _log = new Logger();

        public InboundSNInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
        {
            _db = db;
            _provider = provider; // 正确初始化 _provider
        }

        public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
        {
            // —— 入参校验（读库之前）
            if (ins == null)
                return new HandlerResult(false, "Instruction is null");

            List<HachWmsReceivingSNConfirm> hachWmsReceivingConfirm = new List<HachWmsReceivingSNConfirm>();

            var ContentMain = await _db.Queryable<WMSRFReceiptAcquisition>()
                .LeftJoin<HachWmsReceiving>((rf,hwr)=>rf.ExternReceiptNumber==hwr.OrderNo)             
                .LeftJoin<HachWmsReceivingDetail>((rf, hwr,hwrd) => hwr.Id == hwrd.ReceivingId)
                .LeftJoin<WMSProductBom>((rf, hwr, hwrd, wp) => rf.SKU == wp.ChildSKU)
                .LeftJoin<HachWmsProduct>((rf, hwr, hwrd, wp, hwp) => hwrd.OrganizationCode == hwp.OrganizationCode && hwrd.ItemNum == hwp.ItemNumber && hwrd.ItemId == hwp.InventoryItemId)
                .Where((rf, hwr, hwrd, wp, hwp) => rf.Type == "SN")
                .Where((rf, hwr, hwrd, wp, hwp) => rf.ExternReceiptNumber == ins.OrderNumber)
                .Where((rf, hwr, hwrd, wp, hwp) => rf.SN!="" || rf.SN!=null)
                .Select((rf, hwr, hwrd, wp, hwp) => new  
                {
                    docNumber= hwr.DocNumber,
                    docType = "PO",
                    lineNum= hwrd.LineNum,
                    receiptNum = hwr.ReceiptNum,
                    organizationCode= hwrd.OrganizationCode,
                    organizationId = hwrd.OrganizationId,
                    subinventory = hwrd.Subinventory,
                    itemId = hwrd.ItemId,
                    itemNum = hwrd.ItemNum,
                    quantity =(float)rf.Qty,
                    lsnNumber =rf.SN,
                    expirationDate =rf.ExpirationDate,
                    scanDate =rf.CreationTime,
                    transactionId =hwrd.TransactionId,
                    attribute1 =rf.Lot,
                    attribute2 =rf.ExpirationDate,
                    attribute3 =wp.SKU,
                    attribute6 ="IN",
                    CustomerId = rf.CustomerId,
                    WarehouseId=rf.WarehouseId,
                }).ToListAsync();
        
            // //回传主信息
            if (ContentMain == null || ContentMain.Count==0)
            {
                _log.Log("获取订单信息失败");
                return new HandlerResult(false, "获取订单信息失败");
            }
            
            ContentMain.ForEach(item =>
            {
                hachWmsReceivingConfirm.Add(new HachWmsReceivingSNConfirm
                {
                    docNumber = item.docNumber,
                    docType = "PO",
                    lineNum = item.lineNum,
                    receiptNum = item.receiptNum,
                    organizationCode = item.organizationCode,
                    organizationId = item.organizationId,
                    subinventory = item.subinventory,
                    itemId = item.itemId,
                    itemNum = item.itemNum,
                    quantity = item.quantity,
                    lsnNumber = item.lsnNumber,
                    expirationDate =item.expirationDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    scanDate = item.scanDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    transactionId = item.transactionId,
                    attribute1 = item.attribute1,
                    attribute2 =item.attribute2?.ToString("yyyy-MM-dd HH:mm:ss"),
                    attribute3 = item.attribute3,
                    attribute6 = item.attribute6,
                });
            });


            // 找出重复的 TransactionId（排除 null/空串视情况）
            var duplicateTransactionIds = ContentMain
         .GroupBy(x => x.transactionId)
         .Where(g => g.Count() > 1)
         .Select(g => g.Key)
         .ToList();

            if (duplicateTransactionIds.Any())
            {
                var msg = $"存在重复的TransactionId：{string.Join(",", duplicateTransactionIds)}";
                _log.Log(msg);
                //return new HandlerResult(false, msg);
            }
            //获取 url地址

            HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
            {
                Type = "HachWMSConfirmApi",
                InterfaceName = "SERNO",
                CustomerId = ContentMain[0].CustomerId,
                WarehouseId = ContentMain[0].WarehouseId
            };

            var Authentication = await _provider.GetAsync(input, ct);

            if (Authentication == null)
            {
                _log.Log("未能获取到客户或目标仓库身份鉴权信息");
                return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
            }

            var JSON = JsonConvert.SerializeObject(hachWmsReceivingConfirm);
            _log.Log($"ExternReceiptNumber:{hachWmsReceivingConfirm[0].receiptNum}入库序列号回传报文：{JSON}");

            if (hachWmsReceivingConfirm == null || hachWmsReceivingConfirm.Count == 0)
            {
                return new HandlerResult(false, "获取Body信息失败");
            }
            ReceivingResponseData? confirmResp;
            try
            {
                confirmResp = await _external.SendPostAsync<List<HachWmsReceivingSNConfirm>, ReceivingResponseData>(
                    url: Authentication.Url,
                    body: hachWmsReceivingConfirm,
                    bearerToken: Token,
                    ct: ct
                );
            }
            catch (Exception ex)
            {
                _log.Log($"回传入库序列号失败：{ex.Message}");
                return new HandlerResult(false, $"回传入库序列号失败：{ex.Message}");
            }
            // —— 根据返回判定成功与否（具体字段以你的返回模型为准）————————
            var ok = confirmResp?.success == true || string.Equals(confirmResp?.message, "成功", StringComparison.OrdinalIgnoreCase);
            var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Inbound" };
            return new HandlerResult(ok, ok ? null : (confirmResp?.message ?? "失败"), callback);
        }
    }
}
