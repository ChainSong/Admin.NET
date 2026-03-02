using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Application.Services;
using TaskPlaApplication.Application.Services.Handlers;
using TaskPlaApplication.Infrastructure.Clients;
using TaskPlaApplication.Infrastructure.Persistence;
using TaskPlaApplication.Shared.Options;

var builder = Host.CreateApplicationBuilder(args);

// 显式加载（如果文件不存在会直接报错，便于定位）
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
// 日志
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Options
builder.Services.Configure<FeedbackOptions>(builder.Configuration.GetSection("Feedback"));

// ========= SqlSugar =========
// 只注册 DatabaseContext 单例，由它内部创建 SqlSugarScope
builder.Services.AddSingleton<DatabaseContext>();
// 映射 ISqlSugarClient 到 DatabaseContext.Db（避免重复实例化 SqlSugar）
builder.Services.AddSingleton<ISqlSugarClient>(sp => sp.GetRequiredService<DatabaseContext>().Db);
builder.Services.AddScoped<IThirdPartyConfigProvider, DbThirdPartyConfigProvider>();

// ========= DI =========
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IFeedbackAppService, FeedbackAppService>();

// 指令处理器（按需继续加）
// 指令处理器（与枚举保持一一对应）
builder.Services.AddScoped<IInstructionHandler, InboundInstructionHandler>();
builder.Services.AddScoped<IInstructionHandler, InboundSNInstructionHandler>();
builder.Services.AddScoped<IInstructionHandler, OutboundInstructionHandler>();
builder.Services.AddScoped<IInstructionHandler, OutboundSNInstructionHandler>();
builder.Services.AddScoped<IInstructionHandler, OutboundAFCInstructionHandler>();
builder.Services.AddScoped<IInstructionHandler, OutboundPickingInstructionHandler>();

// 回传客户端（建议用 HttpClient 工厂）
builder.Services.AddHttpClient<IExternalFeedbackClient, ExternalFeedbackClient>().AddStandardResilienceHandler();

var app = builder.Build();

try
{
    using var scope = app.Services.CreateScope();
    var svc = scope.ServiceProvider.GetRequiredService<IFeedbackAppService>();

    // Windows 计划任务每次启动就执行一次
    await svc.ProcessAsync();

    Environment.ExitCode = 0;
}
catch (Exception ex)
{
    app.Services.GetRequiredService<ILoggerFactory>()
        .CreateLogger("Program")
        .LogError(ex, "任务执行失败");

    Environment.ExitCode = 1;
}
