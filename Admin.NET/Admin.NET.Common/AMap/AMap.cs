// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Common.AMap.Response;
using Furion;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Common.AMap;
/// <summary>
/// 请求高德地图服务API
/// </summary>
public class AMap
{
    public static readonly string GeoUrl = App.GetConfig<string>("AMap:GeoUrl");
    public static readonly string GeoKey = App.GetConfig<string>("AMap:GeoKey");

    public static readonly string GeoPOIUrl = App.GetConfig<string>("AMap:GeoPOIUrl");
    public static readonly string GeoPOIKey = App.GetConfig<string>("AMap:GeoPOIKey");

    /// <summary>
    /// 请求高德地图 地理编码
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    //public async Task<GeoCodeResponse> RequestGeoCode(string address, string? city=null)
    //{
    //    if (string.IsNullOrWhiteSpace(address)) return null;

    //    GeoCodeResponse response = new GeoCodeResponse();
    //    string RequestUrl = string.Empty;
    //    //将字符串中的特殊字符转换为符合 URL 规范的格式
    //    var encodedAddress = Uri.EscapeDataString(address);
    //    //获取请求Url
    //    RequestUrl = BuildRequestUrl(encodedAddress, city);
    //    // 检查请求 URL 是否为空
    //    if (string.IsNullOrEmpty(RequestUrl))
    //    {
    //        throw new InvalidOperationException("请求 URL 不能为空");
    //    }
    //    // 创建 HttpClient
    //    using (var client = new HttpClient())
    //    {
    //        // 发送 GET 请求
    //       var  GeoRresponse = await client.GetAsync(RequestUrl);

    //        // 读取响应内容
    //        var content = await GeoRresponse.Content.ReadAsStringAsync();

    //        // 反序列化响应内容
    //        var geoResponse = JsonConvert.DeserializeObject<GeoCodeResponse>(content);

    //        // 检查反序列化结果
    //        if (geoResponse == null)
    //        {
    //            throw new InvalidOperationException("反序列化响应内容失败");
    //        }

    //        return geoResponse;
    //    }
    //}

    public async Task<GeoCodeResponse> RequestGeoCode(string address, string? city = null)
    {
        if (string.IsNullOrWhiteSpace(address)) return null;
        var encodedAddress = EncodeUrlParam(address);
        var requestUrl = BuildUrl(GeoUrl, new Dictionary<string, string>
        {
            { "address", encodedAddress },
            { "key", GeoKey },
            { "city", city }
        });
        return await SendRequestAsync<GeoCodeResponse>(requestUrl);
    }

    /// <summary>
    /// 请求高德地图 地理编码
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    public async Task<GeoCodePOIResponse> RequestGeoCodePOI(string KeyWords)
    {
        if (string.IsNullOrWhiteSpace(KeyWords)) return null;
        var encodedKeywords = EncodeUrlParam(KeyWords);
        var requestUrl = BuildUrl(GeoPOIUrl, new Dictionary<string, string>
        {
            { "keywords", encodedKeywords },
            { "types", "公司" },
            { "key", GeoPOIKey }
        });
        return await SendRequestAsync<GeoCodePOIResponse>(requestUrl);
    }

    /// <summary>
    /// 构建请求GEO URL
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    public string BuildRequestUrl(string address, string? city)
    {
        string RequestUrl = string.Empty;
        //如果地址为空 那么就返回报错
        if (string.IsNullOrEmpty(address))
        {
            return null;
        }
        RequestUrl = GeoUrl + $"?address={address}&key={GeoKey}";
        if (!string.IsNullOrEmpty(city))
        {
            RequestUrl += $"&city={city}";
        }
        return RequestUrl;
    }

    #region 公共方法封装

    /// <summary>
    /// URL参数编码
    /// </summary>
    private string EncodeUrlParam(string value)
    {
        return Uri.EscapeDataString(value);
    }

    /// <summary>
    /// 构建 URL 参数字符串
    /// </summary>
    private string BuildUrl(string baseUrl, Dictionary<string, string?> parameters)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) throw new ArgumentNullException(nameof(baseUrl));

        var query = string.Join("&", parameters
            .Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .Select(p => $"{p.Key}={p.Value}"));

        return $"{baseUrl}?{query}";
    }
  
    /// <summary>
    /// 通用的 HTTP GET 请求发送和 JSON 解析
    /// </summary>
    private async Task<T> SendRequestAsync<T>(string url) where T : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url)) throw new InvalidOperationException("请求 URL 不能为空");
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);
            if (result == null)
            {
                throw new InvalidOperationException("反序列化响应内容失败");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion
}