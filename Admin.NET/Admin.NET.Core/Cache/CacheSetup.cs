﻿// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using NewLife.Caching;

namespace Admin.NET.Core;

public static class CacheSetup
{
    /// <summary>
    /// 缓存注册（新生命Redis组件）
    /// </summary>
    /// <param name="services"></param>
    public static void AddCache(this IServiceCollection services)
    {
        ICache cache = Cache.Default;

        var cacheOptions = App.GetConfig<CacheOptions>("Cache", true);
        if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
        {
            cache = new FullRedis(new RedisOptions
            {
                Configuration = cacheOptions.Redis.Configuration,
                Prefix = cacheOptions.Redis.Prefix
            });
            //if (cacheOptions.Redis.MaxMessageSize > 0)
            ((FullRedis)cache).MaxMessageSize = 10485760;
        }

        services.AddSingleton(cache);
    }
    //public static void AddCache(this IServiceCollection services)
    //{
    //    ICache cache = Cache.Default;

    //    var cacheOptions = App.GetOptions<CacheOptions>();
    //    if (cacheOptions.CacheType == CacheTypeEnum.Redis.ToString())
    //    {
    //        cache = new FullRedis(new RedisOptions
    //        {
    //            Configuration = cacheOptions.Redis.Configuration,
    //            Prefix = cacheOptions.Redis.Prefix
    //        });
    //    }

    //    services.AddSingleton(cache);
    //}
}