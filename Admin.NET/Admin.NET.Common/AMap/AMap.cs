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
//using XAct.Messages;

namespace Admin.NET.Common.AMap;
/// <summary>
/// 请求高德地图服务API
/// </summary>
public class AMap
{
    public static readonly string GeoUrl = App.GetConfig<string>("AMap:GeoUrl");
    public static readonly string GeoPOIUrl = App.GetConfig<string>("AMap:GeoPOIUrl");
    public static readonly string GeoDistrictUrl = App.GetConfig<string>("AMap:GeoDistrictUrl");
    public static readonly string GeoWebServicesAPI = App.GetConfig<string>("AMap:GeoWebServicesAPI");

    public static readonly string GeonamesUrl = App.GetConfig<string>("AMap:GeonamesUrl");
    public static readonly string GeonamesUserName = App.GetConfig<string>("AMap:GeonamesUserName");
    public static string logFilePath = @"C:\HachLogs\OrderAddress_Format_Logs\OrderAddressRequestGeocodeJob.log";

    /// <summary>
    /// 请求高德地图地理编码
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    public async Task<GeoCodeResponse> RequestGeoCode(string address, string? city = null)
    {
        LogMessage("请求高德 开始：" + DateTime.Now + "," + address + "", logFilePath);
        try
        {
            if (string.IsNullOrWhiteSpace(address)) return null;
            var encodedAddress = EncodeUrlParam(address);
            var requestUrl = BuildUrl(GeoUrl, new Dictionary<string, string>
        {
            { "address", encodedAddress },
            { "key", GeoWebServicesAPI },
            { "city", city }
        });
            LogMessage("请求高德Url 开始：" + DateTime.Now + "," + requestUrl + "", logFilePath);

            return await SendRequestAsync<GeoCodeResponse>(requestUrl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 请求高德地图POI 地理编码
    /// </summary>
    /// <param name="address"></param>
    /// <param name="city"></param>
    /// <returns></returns>
    public async Task<GeoCodePOIResponse> RequestGeoCodePOI(string KeyWords)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(KeyWords)) return null;
            var encodedKeywords = EncodeUrlParam(KeyWords);
            var requestUrl = BuildUrl(GeoPOIUrl, new Dictionary<string, string>
        {
            { "keywords", encodedKeywords },
            { "types", "公司" },
            { "key", GeoWebServicesAPI }
        });
            return await SendRequestAsync<GeoCodePOIResponse>(requestUrl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// 行政区域查询 API 服务地址
    /// </summary>
    /// <param name="KeyWords"></param>
    /// <returns></returns>
    public async Task<GeoDistrictCodeResponse> RequestGeoCodeDistrict(string KeyWords, string? Subdistrict = "1", string? Extensions = "base")
    {
        try
        {
            if (string.IsNullOrWhiteSpace(KeyWords)) return null;
            var encodedKeywords = EncodeUrlParam(KeyWords);
            var requestUrl = BuildUrl(GeoDistrictUrl, new Dictionary<string, string>
        {
            { "keywords", encodedKeywords },
            { "subdistrict",Subdistrict},
            { "extensions",Extensions },
            { "key", GeoWebServicesAPI }
        });
            return await SendRequestAsync<GeoDistrictCodeResponse>(requestUrl);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 请求 Geonames查询国家行政区   免费  用不了
    /// </summary>
    /// <param name="KeyWords"></param>
    /// <returns></returns>
    public async Task<GeoCodePOIResponse> RequestGeonames(string KeyWords)
    {
        if (string.IsNullOrWhiteSpace(KeyWords)) return null;
        var encodedKeywords = EncodeUrlParam(KeyWords);
        var requestUrl = BuildUrl(GeoPOIUrl, new Dictionary<string, string>
        {
            { "GeonamesUrl", KeyWords },
            { "username",GeonamesUserName },
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
        try
        {
            string RequestUrl = string.Empty;
            //如果地址为空 那么就返回报错
            if (string.IsNullOrEmpty(address))
            {
                return null;
            }
            RequestUrl = GeoUrl + $"?address={address}&key={GeoWebServicesAPI}";
            if (!string.IsNullOrEmpty(city))
            {
                RequestUrl += $"&city={city}";
            }
            return RequestUrl;
        }
        catch (Exception ex)
        {

            throw;
        }
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
            LogMessage("发送请求开始：" + DateTime.Now + "，" + url + "", logFilePath);
            var response = await client.GetAsync(url);
            LogMessage("请求结果：" + DateTime.Now + "，" + response + "", logFilePath);
            var content = await response.Content.ReadAsStringAsync();
            LogMessage("序列化报文的结果：" + DateTime.Now + "，" + content + "", logFilePath);
            var result = JsonConvert.DeserializeObject<T>(content);
            if (result == null)
            {
            LogMessage("反序列化响应内容失败：" + DateTime.Now + "，" + content + "", logFilePath);
                throw new InvalidOperationException("反序列化响应内容失败");
            }
            return result;
        }
        catch (Exception ex)
        {
            LogMessage("错误：" + DateTime.Now + "，" + ex.ToString() + "", logFilePath);
            throw ex;
        }
    }

    private void LogMessage(string message, string logFilePath)
    {
        // 确保日志目录存在
        string directory = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // 将日志写入文件
        using (var writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }
    }
    #endregion
}