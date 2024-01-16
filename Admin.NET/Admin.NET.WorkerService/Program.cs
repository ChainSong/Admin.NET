// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Service;
using Admin.NET.Core;
using Admin.NET.WorkerService;
using AspNetCoreRateLimit;
using Furion.VirtualFileServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using OnceMi.AspNetCore.OSS;
using System.Net.Mail;
using System.Net;
using Yitter.IdGenerator;
using Furion;
using System;
using Admin.NET.WorkerService.Handlers;

 
public class Program 
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .Inject()
            .ConfigureServices((hostContext, services) =>
            {
                //// 配置选项
                //services.AddProjectOptions();

                //// 缓存注册
                //services.AddCache();
                //// SqlSugar
                //services.AddSqlSugar();

                //// 注册远程请求
                //services.AddRemoteRequest();
            }); 
}

//IHost host = Host.CreateDefaultBuilder(args)
//     .Inject()
//     .ConfigureServices(services =>
//     {
//         // 以下代码可不用编写，Furion 已实现自动注册 Worker;
//         //services.AddHostedService<Worker>();
//         // 注册数据库服务
//         //services.AddDatabaseAccessor(options =>
//         //{
//         //    options.AddDb<DefaultDbContext>();
//         //});

//     })
//    //.ConfigureServices(services =>
//    //{
//    //    //services.AddHostedService<Worker>();
//    //})
//    .Build();

//await host.RunAsync();
