using Newtonsoft.Json;
using SqlSugar;
using System.Text;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Domain.Entities;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Utilities;
using TaskPlaApplication.Domain;

namespace TaskPlaApplication.Application.Services.Handlers
{
    public class OutboundAFCInstructionHandler: IInstructionHandler
    {
        public InstructionType Type => InstructionType.出库单防伪码回传HachDG;
        private readonly ISqlSugarClient _db;
        private readonly ExternalFeedbackClient _external = new ExternalFeedbackClient();
        private readonly IThirdPartyConfigProvider _provider;
        
        Logger _log = new Logger();

        public OutboundAFCInstructionHandler(ISqlSugarClient db, IThirdPartyConfigProvider provider)
        {
            _db = db;
            _provider = provider; // 正确初始化 _provider
        }

        public async Task<HandlerResult> HandleAsync(WMSInstruction ins, string Token, CancellationToken ct)
        {
            // —— 入参校验（读库之前）
            if (ins == null)
                return new HandlerResult(false, "Instruction is null");

            List<HachWmsOutBoundAFCConfirm> hachWmsOutBoundAFCConfirm = new List<HachWmsOutBoundAFCConfirm>();
            // 查询包装数据
            var packageData = await GetPackageDataAsync(ins, ct);

            if (packageData == null || packageData.Count == 0)
            {
                _log.Log("获取防伪码和RFID信息失败");
                return new HandlerResult(false, "获取防伪码和RFID信息失败");
            }

            // 获取订单信息
            var ObInfo = await GetOrderInfoAsync(ins.OrderNumber);
            if (ObInfo == null)
            {
                _log.Log("获取订单信息失败");
                return new HandlerResult(false, "获取订单信息失败");
            }


            // 构建确认数据
            hachWmsOutBoundAFCConfirm = BuildConfirmData(packageData, ObInfo);

            // 找出重复的 deliveryDetailId（排除 null/空串视情况）
            var duplicateTransactionIds = hachWmsOutBoundAFCConfirm
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

            // 获取API配置
            var authConfig = await GetAuthConfigAsync(packageData.First(), ct);
            if (authConfig == null)
            {
                _log.Log("未能获取到客户或目标仓库身份鉴权信息");
                return new HandlerResult(false, "未能获取到客户或目标仓库身份鉴权信息");
            }

            // 序列化用于日志记录
            var jsonstr = JsonConvert.SerializeObject(hachWmsOutBoundAFCConfirm);
            _log.Log($"ExternOrderNumber:{hachWmsOutBoundAFCConfirm[0].delivery}出库防伪码回传数据：{jsonstr}");

            if (hachWmsOutBoundAFCConfirm == null || hachWmsOutBoundAFCConfirm.Count == 0)
            {
                return new HandlerResult(false, "获取Body信息失败");
            }

            ConfirmResponse? confirmResp = new ConfirmResponse();
            try
            {
                confirmResp = await _external.SendPostAsync<List<HachWmsOutBoundAFCConfirm>, ConfirmResponse>(
                    url: authConfig.Url,
                    body: hachWmsOutBoundAFCConfirm,
                    bearerToken: Token,
                    ct: ct
                );
            }
            catch (Exception ex)
            {
                _log.Log($"回传出库防伪码失败：{ex.Message}");
                return new HandlerResult(false, $"回传出库防伪码失败：{ex.Message}");
            }
            // —— 根据返回判定成功与否（具体字段以你的返回模型为准）————————
            var ok = confirmResp?.Success == true || string.Equals(confirmResp?.Result, "成功", StringComparison.OrdinalIgnoreCase);
            var callback = new { instructionId = ins.Id, result = ok ? "ok" : "fail", kind = "Inbound" };
            return new HandlerResult(ok, ok ? null : (confirmResp?.Result ?? "失败"), callback);
        }

        /// <summary>
        /// 查询包装数据（优先RFID，其次防伪码）
        /// </summary>
        private async Task<List<dynamic>> GetPackageDataAsync(WMSInstruction ins, CancellationToken ct)
        {
            var commonWhere = new
            {
                ExternOrderNumber = ins.OrderNumber,
                CustomerId = ins.CustomerId,
                WarehouseId = ins.WarehouseId
            };

            // 防伪码查询
            var afcQuery = _db.Queryable<WMSRFPackageAcquisition>()
                .Where(rf => rf.Type == "AFC" && !string.IsNullOrEmpty(rf.Type))
                .Where(rf => rf.ExternOrderNumber == commonWhere.ExternOrderNumber)
                .Where(rf => rf.OrderId == ins.OperationId)
                .Where(rf => rf.CustomerId == commonWhere.CustomerId)
                .Where(rf => rf.WarehouseId == commonWhere.WarehouseId)
                .Where(rf=>rf.SN!=null && rf.SN!="")
                .Distinct()
                .GroupBy(rf => new
                {
                    rf.SKU,
                    rf.SN,
                    rf.Lot,
                    rf.ExpirationDate,
                    rf.Qty,
                    rf.CustomerId,
                    rf.WarehouseId,
                    rf.PackageNumber,
                })
                .Select(rf => new
                {
                    docType = "SO",
                    itemNum = rf.SKU,
                    quantity = (float)rf.Qty,
                    lsnNumber = rf.SN,
                    expirationDate = rf.ExpirationDate.ToString(),
                    scanDate = SqlFunc.AggregateMax(rf.CreationTime).ToString("yyyy-MM-dd hh:mm:ss"),
                    attribute1 = rf.Lot,
                    attribute2 = rf.ExpirationDate,
                    attribute6 = "OUT",
                    CustomerId = rf.CustomerId,
                    WarehouseId = rf.WarehouseId,
                    DataSource = "AFC",
                    containerNo = rf.PackageNumber,
                });

            // RFID查询
            var rfidQuery = _db.Queryable<WMSRFIDInfo>()
                .Where(rf => rf.ExternOrderNumber == commonWhere.ExternOrderNumber)
                .Where(rf => rf.CustomerId == commonWhere.CustomerId)
                .Where(rf => rf.WarehouseId == commonWhere.WarehouseId)
                .Distinct()
                .GroupBy(rf => new
                {
                    rf.SKU,
                    rf.RFID,
                    rf.Qty,
                    rf.CustomerId,
                    rf.WarehouseId,
                    rf.PackageNumber
                })
                .Select(rf => new
                {
                    docType = "SO",
                    itemNum = rf.SKU,
                    quantity = (float)rf.Qty,
                    lsnNumber = rf.RFID,
                    expirationDate = "",
                    scanDate = SqlFunc.AggregateMax(rf.CreationTime).ToString("yyyy-MM-dd hh:mm:ss"),
                    attribute1 = "",
                    attribute2 = "",
                    attribute6 = "OUT",
                    CustomerId = rf.CustomerId,
                    WarehouseId = rf.WarehouseId,
                    DataSource = "RFID",
                    containerNo=rf.PackageNumber
                });

            // 分别执行查询
            var rfidData = await rfidQuery.ToListAsync();

            // 如果RFID有数据，直接返回
            if (rfidData != null && rfidData.Count > 0)
            {
                _log.Log($"使用RFID数据，共{rfidData.Count}条记录");
                return ConvertToDynamicList(rfidData);
            }

            // RFID没有数据，查询防伪码数据
            var afcData = await afcQuery.ToListAsync();
            if (afcData != null && afcData.Count > 0)
            {
                _log.Log($"使用防伪码数据，共{afcData.Count}条记录");
                return ConvertToDynamicList(afcData);
            }

            // 两者都没有数据
            _log.Log("防伪码和RFID均无数据");
            return null;
        }
        // 辅助方法：将匿名类型列表转换为 dynamic 列表
        private List<dynamic> ConvertToDynamicList<T>(List<T> sourceList)
        {
            var result = new List<dynamic>();
            foreach (var item in sourceList)
            {
                dynamic dynamicItem = new System.Dynamic.ExpandoObject();
                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    ((IDictionary<string, object>)dynamicItem)[property.Name] = property.GetValue(item);
                }

                result.Add(dynamicItem);
            }
            return result;
        }
        /// <summary>
        /// 根据优先级选择数据源：RFID优先，防伪码次之
        /// </summary>
        private List<dynamic> GetPriorityData(List<dynamic> afcData, List<dynamic> rfidData)
        {
            // 如果RFID有数据，优先使用RFID数据
            if (rfidData != null && rfidData.Count > 0)
            {
                _log.Log($"使用RFID数据，共{rfidData.Count}条记录");
                return rfidData;
            }

            // 如果RFID没有数据，但防伪码有数据，使用防伪码数据
            if (afcData != null && afcData.Count > 0)
            {
                _log.Log($"使用防伪码数据，共{afcData.Count}条记录");
                return afcData;
            }

            // 两者都没有数据
            _log.Log("防伪码和RFID均无数据");
            return null;
        }
        /// <summary>
        /// 获取订单信息
        /// </summary>
        private async Task<HachWmsOutBound> GetOrderInfoAsync(string orderNumber)
        {
            return await _db.Queryable<HachWmsOutBound>()
                .Where(a => a.OrderNumber == orderNumber)
                .Includes(a => a.items)
                .FirstAsync();
        }
        /// <summary>
        /// 构建确认数据
        /// </summary>
        private List<HachWmsOutBoundAFCConfirm> BuildConfirmData(List<dynamic> sourceData, HachWmsOutBound orderInfo)
        {
            var confirmData = new List<HachWmsOutBoundAFCConfirm>();

            foreach (var item in sourceData)
            {
                var itemInfo = orderInfo.items
                    .FirstOrDefault(a => a.ItemNumber == item.itemNum);

                if (itemInfo == null)
                {
                    _log.Log($"未找到物料编号 {item.itemNum} 的订单行信息");
                    continue;
                }

                confirmData.Add(new HachWmsOutBoundAFCConfirm
                {
                    docNumber = orderInfo.SoNumber,
                    docType = "SO",
                    lineNum = itemInfo.LineNumber,
                    delivery = orderInfo.DeliveryNumber,
                    organizationCode = itemInfo.OrganizationCode,
                    organizationId = itemInfo.OrganizationId,
                    subinventory = orderInfo.Subinventory,
                    itemId = itemInfo.ItemId,
                    itemNum = item.itemNum,
                    containerNo=item.containerNo,
                    quantity = (float)item.quantity,
                    //lsnNumber = GenerateSecurityCode(item.lsnNumber),
                    lsnNumber = item.lsnNumber,
                    expirationDate = item.expirationDate,
                    scanDate = item.scanDate,
                    attribute1 = item.attribute1,
                    attribute2 = item.attribute2?.ToString() ?? "",
                    attribute3 = itemInfo.ParentItemNumber,
                    attribute6 = item.attribute6,
                    attribute7 = orderInfo.ContractNo,
                });
            }

            return confirmData;
        }
        /// <summary>
        /// 获取认证配置
        /// </summary>
        private async Task<HachConfirmWmsAuthorizationConfig> GetAuthConfigAsync(dynamic sampleData, CancellationToken ct = default)
        {
            var input = new HachConfirmWmsAuthorizationConfig
            {
                Type = "HachWMSConfirmApi",
                InterfaceName = "AFC",
                CustomerId = sampleData.CustomerId,
                WarehouseId = sampleData.WarehouseId
            };
            return await _provider.GetAsync(input, ct);
        }
        private String validate(String a)
        {
            switch (a)
            {
                case "0":
                    return "A";
                case "1":
                    return "E";
                case "2":
                    return "J";
                case "3":
                    return "3";
                case "4":
                    return "M";
                case "5":
                    return "8";
                case "6":
                    return "U";
                case "7":
                    return "W";
                case "8":
                    return "G";
                case "9":
                    return "T";
                default:
                    return a;
            }
        }
        private string GenerateSecurityCode(string originalText)
        {
            if (string.IsNullOrEmpty(originalText) || originalText.Length != 16)
            {
                throw new ArgumentException("原始明文必须是16位数字");
            }

            StringBuilder result = new StringBuilder();

            foreach (char c in originalText)
            {
                string charStr = c.ToString();
                result.Append(validate(charStr));
            }

            return result.ToString();
        }
    }
}
