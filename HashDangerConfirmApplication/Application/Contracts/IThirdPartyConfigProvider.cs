using TaskPlaApplication.Models.Entities;
using TaskPlaApplication.Shared.Options;

namespace TaskPlaApplication.Application.Contracts;

/// <summary>【读库配置】对第三方 API 的所有配置都从数据库读</summary>
public interface IThirdPartyConfigProvider
{
    Task<HachConfirmWmsAuthorizationConfig> GetAsync(HachConfirmWmsAuthorizationConfig InterfaceName,CancellationToken ct = default);
}
