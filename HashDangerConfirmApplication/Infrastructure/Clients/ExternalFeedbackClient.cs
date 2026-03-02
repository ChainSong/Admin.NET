using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SQLitePCL;
using System.Net.Http.Headers;
using System.Text;
using TaskPlaApplication.Application.Contracts;
using TaskPlaApplication.Utilities;

namespace TaskPlaApplication.Infrastructure.Clients;

public class ExternalFeedbackClient : IExternalFeedbackClient
{
    Logger _log = new Logger();
    public string BuildUrl(string baseUrl, Dictionary<string, string?> parameters)
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
    public async Task<T> SendRequestAsync<T>(string url) where T : class
    {
        try
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                _log.Log("《SendRequestAsync》请求 URL 不能为空");
                throw new ArgumentException("请求 URL 不能为空", nameof(url));
            }
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);
            if (result == null)
            {
                _log.Log("《SendRequestAsync》反序列化响应内容失败");

                throw new InvalidOperationException("《SendRequestAsync》反序列化响应内容失败");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// POST: application/x-www-form-urlencoded
    /// </summary>
    public async Task<TResponse> PostFormUrlEncodedAsync<TResponse>(string urlWithQuery, IDictionary<string, string>? headers = null, string? bearerToken = null, CancellationToken ct = default) where TResponse : class
    {
        if (string.IsNullOrWhiteSpace(urlWithQuery))
        {
            _log.Log("《PostFormUrlEncodedAsync》请求 URL 不能为空");
            throw new ArgumentException("《PostFormUrlEncodedAsync》请求 URL 不能为空", nameof(urlWithQuery));
        }

        // 解析 URL -> baseUrl + form fields
        var (baseUrl, formFields) = ExtractQueryAsForm(urlWithQuery);

        using var client = new HttpClient();

        if (!string.IsNullOrWhiteSpace(bearerToken))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        if (headers != null)
            foreach (var kv in headers)
                client.DefaultRequestHeaders.TryAddWithoutValidation(kv.Key, kv.Value);

        using var content = new FormUrlEncodedContent(formFields);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
        {
            CharSet = "UTF-8"
        };

        using var resp = await client.PostAsync(baseUrl, content, ct);
        var text = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
        {
            _log.Log($"《PostFormUrlEncodedAsync》：POST {baseUrl} 失败，状态 {(int)resp.StatusCode} {resp.ReasonPhrase}，响应：{text}");
            throw new HttpRequestException($"POST {baseUrl} 失败，状态 {(int)resp.StatusCode} {resp.ReasonPhrase}，响应：{text}");

        }

        var result = JsonConvert.DeserializeObject<TResponse>(text);
        if (result is null)
        {
            _log.Log($"《SendRequestAsync》反序列化响应内容失败:{text}");
            throw new InvalidOperationException("反序列化响应内容失败");
        }
        return result;
    }

    /// <summary>
    /// 把 url 中的查询串提取为表单字段，并返回去掉查询串的基础地址
    /// </summary>
    private static (string baseUrl, List<KeyValuePair<string, string>> fields) ExtractQueryAsForm(string urlWithQuery)
    {
        var qIdx = urlWithQuery.IndexOf('?', StringComparison.Ordinal);
        if (qIdx < 0)
            return (urlWithQuery, new List<KeyValuePair<string, string>>());

        var baseUrl = urlWithQuery[..qIdx];
        var query = urlWithQuery[(qIdx + 1)..];

        var fields = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrEmpty(query))
        {
            var parts = query.Split('&', StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var eq = p.IndexOf('=', StringComparison.Ordinal);
                string key, val;
                if (eq >= 0)
                {
                    key = p[..eq];
                    val = p[(eq + 1)..];
                }
                else
                {
                    key = p;
                    val = string.Empty;
                }

                // URL 解码
                key = Uri.UnescapeDataString(key);
                val = Uri.UnescapeDataString(val);

                fields.Add(new KeyValuePair<string, string>(key, val));
            }
        }

        return (baseUrl, fields);
    }
    /// <summary>
    /// 通用的 HTTP POST 请求发送和 JSON 解析
    /// </summary>
    /// <typeparam name="TRequest">请求体类型</typeparam>
    /// <typeparam name="TResponse">响应体类型</typeparam>
    /// <param name="url">请求地址</param>
    /// <param name="body">请求体对象，会序列化为 JSON</param>
    /// <param name="headers">可选：额外请求头（如 X-Trace-Id 等）</param>
    /// <param name="bearerToken">可选：Bearer Token，会写入 Authorization 头</param>
    /// <param name="ct">取消令牌</param>
    public async Task<TResponse> SendPostAsync<TRequest, TResponse>(string url,TRequest body,IDictionary<string, string>? headers = null,string? bearerToken = null,CancellationToken ct = default)where TResponse : class
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            _log.Log("请求 URL 不能为空");
            throw new ArgumentException("请求 URL 不能为空", nameof(url));
        }

        using var client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(5)
        };

        // 可选：设置认证头
        if (!string.IsNullOrWhiteSpace(bearerToken))
        {
            //client.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", bearerToken);
            bearerToken = bearerToken.Trim();
            if (bearerToken.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
                bearerToken = bearerToken.Substring(7).Trim(); // 去掉前缀
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        }

        // 可选：附加自定义请求头
        if (headers != null)
        {
            foreach (var kv in headers)
                client.DefaultRequestHeaders.TryAddWithoutValidation(kv.Key, kv.Value);
        }

        // 组装 POST 请求
        using var req = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(
                JsonConvert.SerializeObject(body),
                Encoding.UTF8,
                "application/json")
        };

        try
        {
            using var resp = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            var content = await resp.Content.ReadAsStringAsync(ct);
            // 抛出包含状态码与响应内容的异常，方便排查
            if (!resp.IsSuccessStatusCode)
            {
                _log.Log($"POST {url} 失败，状态码 {(int)resp.StatusCode} {resp.ReasonPhrase}，响应：{content}");
                throw new HttpRequestException(
                    $"POST {url} 失败，状态码 {(int)resp.StatusCode} {resp.ReasonPhrase}，响应：{content}");
            }
            _log.Log($"反序列化响应内容：{content}");
            var result = JsonConvert.DeserializeObject<TResponse>(content);
            if (result is null)
            {
                _log.Log($"反序列化响应内容失败：{content}");
                throw new InvalidOperationException("反序列化响应内容失败");
            }
            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
