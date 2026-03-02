using Newtonsoft.Json;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Domain.Entities;
using TaskPlaApplication.Utilities;

namespace TaskPlaApplication.Application.Services.Handlers
{
    internal class OutboundPickingInstructionHandler : IInstructionHandler
    {
        public InstructionType Type => InstructionType.出库装箱回传HachDG;
        private readonly ISqlSugarClient _db;
        ExternalFeedbackClient _external = new ExternalFeedbackClient();
        private readonly IThirdPartyConfigProvider _provider;
        
        Logger _log = new Logger();

        public OutboundPickingInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
        {
            _db = db;
            _provider = provider; // 正确初始化 _provider
        }
        public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
        {
            // —— 入参校验（读库之前）———————————————————————————————
            if (ins == null)
                return new HandlerResult(false, "Instruction is null");

            List<HachWmsOutBoundPickingConfirm> pickingConfirmList = new List<HachWmsOutBoundPickingConfirm>();

            //先校验dn是否都操作出库了  先PreOrder再Order
            var checkPreOrderStatus = await _db.Queryable<WMSPreOrder>()
                .Where(a => a.Dn == ins.InstructionTaskNo)
                .ToListAsync();

            if (checkPreOrderStatus == null || checkPreOrderStatus.Count == 0)
            {
                _log.Log($"{ins.OrderNumber} 未找到PreOrder，无法校验转单");
                return new HandlerResult(false, $"{ins.OrderNumber} 未找到PreOrder，无法校验转单");
            }

            //先校验dn是否都操作出库了
            var checkOrderStatus = await _db.Queryable<WMSOrder>()
                .Where(a => a.Dn == ins.InstructionTaskNo)
                .ToListAsync();

            if (checkOrderStatus == null || !checkOrderStatus.Any())
            {
                _log.Log($"{ins.OrderNumber}校验Dn交接状态失败");
                return new HandlerResult(false, $"{ins.OrderNumber}校验Dn出库状态失败");
            }

            // 对比 externOrderNumber 集合是否完全对应 ——
            var preExterns = checkPreOrderStatus
                .Select(x => x.ExternOrderNumber)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var orderExterns = checkOrderStatus
                .Select(x => x.ExternOrderNumber)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Order 是否覆盖 PreOrder：只要缺一个就 rerun
            var missingExterns = preExterns.Except(orderExterns).ToList();
            if (missingExterns.Count > 0)
            {
                _log.Log($"{ins.OrderNumber} DN下仍有PreOrder未转Order，缺少ExternOrderNumber数量={missingExterns.Count}，示例={missingExterns[0]}，需要rerun");
                return new HandlerResult(false, $"{ins.OrderNumber} DN下仍有PreOrder未转Order（缺{missingExterns.Count}条），请稍后重试");
            }

            if (checkOrderStatus.Any(x => x.OrderStatus < 80))
            {
                _log.Log($"{ins.OrderNumber}存在状态<80的订单，需要return");
                return new HandlerResult(false, $"{ins.OrderNumber}存在未交接的订单(状态<80)，请稍后重试");
            }

            //获取对接信息
             var HachWmsOutBoundList = await _db.Queryable<HachWmsOutBound>()
            .Where(p => p.DeliveryNumber == ins.InstructionTaskNo)
             .Includes(p => p.items).
             ToListAsync();
            if (HachWmsOutBoundList == null || !HachWmsOutBoundList.Any())
            {
                _log.Log("获取对接订单信息失败");
                return new HandlerResult(false, "获取对接订单信息失败");
            }

            //获取托信息
            List<WMSHandover> wMSHandoverList = new List<WMSHandover>();
            //获取包装信息
            List<WMSPackage> wMSPackageList = new List<WMSPackage>();

            foreach (var item in HachWmsOutBoundList)
            {
                var PalletList =await _db.Queryable<WMSHandover>()
                    .Where(p=>p.ExternOrderNumber==item.OrderNumber)
                    .ToListAsync();

                if (PalletList == null || PalletList.Count==0)
                {
                    _log.Log($"{item.OrderNumber}获取托信息信息失败");
                    return new HandlerResult(false, $"{item.OrderNumber}获取托信息信息失败");
                }

                var PackageList = await _db.Queryable<WMSPackage>()
                   .Where(p => p.ExternOrderNumber == item.OrderNumber)
                   .Includes(a=>a.Details)
                   .ToListAsync();

                if (PackageList == null || PackageList.Count==0)
                {
                    _log.Log($"{item.OrderNumber}获取包装信息信息失败");
                    return new HandlerResult(false, $"{item.OrderNumber}获取托信息信息失败");
                }

                wMSHandoverList.AddRange(PalletList);
                wMSPackageList.AddRange(PackageList);
            }
            //去重托号
            var distinctPallets = wMSHandoverList.Select(p => p.PalletNumber).Distinct().ToList();
            // 根据 palletNumber 去重后再计算总重量
            var totalWeight = wMSHandoverList
                 .DistinctBy(a => a.PalletNumber)
                 .Where(a => a.GrossWeight.HasValue)
                 .Select(a => a.GrossWeight.Value)
                 .Sum();

            // 根据 palletNumber 去重后再计算总体积
            var totalVol = wMSHandoverList
                .DistinctBy(a => a.PalletNumber)
                .Where(a => a.Length.HasValue && a.Width.HasValue && a.Height.HasValue)
                .Select(a => a.Length.Value * a.Width.Value * a.Height.Value)
                .Sum();

            HachWmsOutBoundPickingConfirm pickingConfirm = new HachWmsOutBoundPickingConfirm();

            pickingConfirm = new HachWmsOutBoundPickingConfirm
            {
                deliveryNum = ins.InstructionTaskNo,
                palletQty = distinctPallets.Count,//托号count
                cartonQty = wMSPackageList.Count,
                totalWeight = totalWeight,
                totalVol = totalVol,
                cartonPallets = new List<cartonPallet>()
            };

            foreach (var item in wMSHandoverList)
            {
                var PackageInfo = wMSPackageList.Where(p => p.PackageNumber == item.PackageNumber).FirstOrDefault();

                var palletInfo = new cartonPallet
                {
                    //cartonNum = ins.InstructionTaskNo+item.SerialNumber,
                    cartonNum = (ins.InstructionTaskNo ?? "").Trim()+"_"+ (item.SerialNumber ?? "").Trim(),
                    cartonNumQuantity = PackageInfo.SerialNumber,
                    palletNum = item.PalletNumber,
                    items = new List<items>()
                };

                foreach (var detail in PackageInfo.Details)
                {
                    var obd = HachWmsOutBoundList.Where(a=>a.OrderNumber==PackageInfo.ExternOrderNumber).FirstOrDefault()
                        .items.Where(a => a.ItemNumber == detail.SKU).FirstOrDefault();

                    var details = new items()
                    {
                        itemNum = detail.SKU,
                        quantity = detail.Qty,
                        deliveryDetailId = obd != null ? obd.DeliveryDetailId : 0,//沟通的是先传空值 20251010
                    };
                    palletInfo.items.Add(details);
                }
                pickingConfirm.cartonPallets.Add(palletInfo);
            }
            pickingConfirmList.Add(pickingConfirm);

            //获取 url地址
            HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
            {
                Type = "HachWMSConfirmApi",
                InterfaceName = "SOPICKING",
            };
            var Authentication = await _provider.GetAsync(input, ct);

            if (Authentication == null)
            {
                _log.Log("未能获取到客户或目标仓库身份鉴权信息");
                return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
            }

            ConfirmResponse? confirmResp;

            var jsonstr = JsonConvert.SerializeObject(pickingConfirmList);
            _log.Log($"ExternOrderNumber{pickingConfirmList[0].deliveryNum}回传出库装箱数据：{jsonstr}");

            if (pickingConfirmList == null || pickingConfirmList.Count == 0)
            {
                return new HandlerResult(false, "获取Body信息失败");
            }

            try
            {
                confirmResp = await _external.SendPostAsync<List<HachWmsOutBoundPickingConfirm>, ConfirmResponse>(
                    url: Authentication.Url,
                    body: pickingConfirmList,
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
            var ok = confirmResp?.Success == true || string.Equals(confirmResp?.Result, "成功", StringComparison.OrdinalIgnoreCase);
            var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Inbound" };
            return new HandlerResult(ok, ok ? null : (confirmResp?.Result ?? "失败"), callback);
        }
    }
}