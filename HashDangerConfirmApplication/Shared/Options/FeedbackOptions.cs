using TaskPlaApplication.Models.Entities;

namespace TaskPlaApplication.Shared.Options;

public sealed class FeedbackOptions
{
    public int BatchSize { get; set; } = 100;
    public string? ExternalEndpoint { get; set; } // 如需回传第三方
    public int HttpTimeoutSeconds { get; set; } = 15;
    // 回传相关配置
    public HachConfirmWmsAuthorizationConfig Auth { get; set; } = new HachConfirmWmsAuthorizationConfig();
    public List<HachConfirmWmsAuthorizationConfig> Content { get; set; } = new List<HachConfirmWmsAuthorizationConfig>();
}
 