// Application/Services/FeedbackAppService.cs
using Microsoft.Extensions.Options;
using SqlSugar;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Domain;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Infrastructure.Persistence;
using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Shared.Options;
using TaskPlaApplication.Utilities;
using TaskPlaApplication.Domain;

namespace TaskPlaApplication.Application.Services;
public sealed class FeedbackAppService(
    ITaskRepository repo,
    IThirdPartyConfigProvider _provider,
    IEnumerable<IInstructionHandler> handlers,
    IOptions<FeedbackOptions> opt) : IFeedbackAppService
{
    private readonly Dictionary<InstructionType, IInstructionHandler> _handlerMap =
        handlers.ToDictionary(h => h.Type, h => h);

    ExternalFeedbackClient _external = new ExternalFeedbackClient();
    Logger _log = new Logger();
    public async Task ProcessAsync(CancellationToken ct = default)
    {
        string Token = string.Empty;
        var cfg = opt.Value;
        var batch = await repo.GetPendingAsync(cfg.BatchSize, ct);
        if (batch.Count == 0)
        { 
            _log.Log("队列中没有待回传的订单");
            return;
        }
        foreach ( var ins in batch)
         {
            InstructionStatus statucCode = InstructionStatus.Pending;
            try
             {
                // 从枚举属性取类型
                  var typeEnum = ins.InstructionTypeEnum;

                if (!_handlerMap.TryGetValue(typeEnum, out var handler))
                {
                     statucCode = InstructionStatus.Failed;
                    ins.Message = $"未找到当前类型: {typeEnum}";
                    ins.UpdationTime = DateTime.Now;
                    await repo.UpdateAsync(ins, statucCode,ct);
                    continue;
                }
                HachConfirmWmsAuthorizationConfig input = new HachConfirmWmsAuthorizationConfig
                {
                    Type = "HachWMSConfirmApi",
                    InterfaceName = "login"
,
                };
                var Authentication = await _provider.GetAsync(input, ct);

                if (Authentication == null)
                {
                    _log.Log($"身份验证失败: {typeEnum}，目标单号：{ins.OrderNumber}");
                    ins.Message = $"身份验证失败: {typeEnum}，目标单号：{ins.OrderNumber}";
                    ins.UpdationTime = DateTime.Now;
                    statucCode= InstructionStatus.Failed;
                    await repo.UpdateAsync(ins, statucCode,ct);
                    continue;
                }
                
                Token = Authentication.Token;

                if (string.IsNullOrEmpty(Token))
                {
                    var requestUrl = _external
                        .BuildUrl(Authentication.Url, new Dictionary<string, string>
                        {
                            { "username", Authentication.AppId },
                            { "pwd", Authentication.AppSecret },
                        });
                    var authInfo = await _external.PostFormUrlEncodedAsync<GetTokenResponse>(requestUrl);
                    Token = authInfo.data.token;
                }
                var result = await handler.HandleAsync(ins,Token ,ct);
                if (result.Success)
                {
                    statucCode = InstructionStatus.Succeeded;
                }
                else
                {
                    statucCode = InstructionStatus.Failed;
                }
                
                ins.Message = result.Result;

                // 先落本地成功状态，再回传第三方（若担心幂等可用Outbox）
                await repo.UpdateAsync(ins, statucCode, ct);

                _log.Log($"指令 {ins.Id},({typeEnum}) 处理{result.Success}:{result.Result}。");
            }
            catch (Exception ex)
            {
                statucCode= InstructionStatus.Failed;
                await repo.UpdateAsync(ins, statucCode,ct);

                _log.Log($"指令 {ins.Id}处理异常");
            }
        }
    }
}
