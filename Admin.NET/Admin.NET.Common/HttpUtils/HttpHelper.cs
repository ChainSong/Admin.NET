using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Utility;
public static class HttpHelper
{
    private static readonly HttpClient _httpClient = new HttpClient();

    static HttpHelper()
    {
        // 设置默认的 BaseAddress 和超时时间
        _httpClient.BaseAddress = new Uri("https://api.example.com/");
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    /// <summary>
    /// 发送 GET 请求
    /// </summary>
    public static async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// 发送 POST 请求
    /// </summary>
    public static async Task<T> PostAsync<T>(string url, object data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// 发送 PUT 请求
    /// </summary>
    public static async Task<T> PutAsync<T>(string url, object data)
    {
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// 发送 DELETE 请求
    /// </summary>
    public static async Task<T> DeleteAsync<T>(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        return await HandleResponse<T>(response);
    }

    /// <summary>
    /// 处理 HTTP 响应
    /// </summary>
    private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"请求失败，状态码：{response.StatusCode}，错误信息：{error}");
        }

        var responseData = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}