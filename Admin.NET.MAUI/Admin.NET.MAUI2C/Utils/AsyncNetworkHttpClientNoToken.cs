using Admin.NET.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Admin.NET.MAUI2C
{
    public class AsyncNetworkHttpClientNoToken
    {
        //private readonly IAppSettingsService _appSettingsService;
        //protected IAppNavigator _appNavigator { get; }
        //新建一个http实例
        private static HttpClient _httpClients = new HttpClient();

        public AsyncNetworkHttpClientNoToken(IAppSettingsService appSettingsService, IAppNavigator appNavigator)
        {

            //_appSettingsService = appSettingsService;
            //_appNavigator = appNavigator;
            if (_httpClients.Timeout.Seconds < 10)
            {
                _httpClients.Timeout = new TimeSpan(0, 0, 30);
            }
            //AddHeaders(_httpClients.DefaultRequestHeaders).WaitAsync(new TimeSpan(0, 0, 0, 0, 0));
            //AddHeaders(_httpClients.DefaultRequestHeaders).Wait();
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                //await AddHeaders(_httpClients.DefaultRequestHeaders);
                //var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                //发送get请求
                var response = await _httpClients.GetAsync(BaseApi._baseUrl + url);
                // 读取响应内容  
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
                {
                    // 忽略JSON属性名称的大小写
                    PropertyNameCaseInsensitive = true
                });

                //发送get请求
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }


        public async Task<T> GetAsync<T>(string url, string data)
        {
            try
            {
                //await AddHeaders(_httpClients.DefaultRequestHeaders);
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                //发送get请求
                var response = await _httpClients.GetAsync(BaseApi._baseUrl + url + "/" + data);
                // 读取响应内容  
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
                {
                    // 忽略JSON属性名称的大小写
                    PropertyNameCaseInsensitive = true
                });

                //发送get请求
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        public async Task<BaseResponse<T>> GetBaseAsync<T>(string url)
        {
            try
            {
                //await AddHeaders(_httpClients.DefaultRequestHeaders);
                //var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                //发送get请求
                var response = await _httpClients.GetAsync(BaseApi._baseUrl + url);
                // 读取响应内容  
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<BaseResponse<T>>(responseBody, new JsonSerializerOptions
                {
                    // 忽略JSON属性名称的大小写
                    PropertyNameCaseInsensitive = true
                });

                //发送get请求
                return result;
            }
            catch (Exception ex)
            {
                return default(BaseResponse<T>);
            }
        }

        public async Task<T> GetTokenAsync<T>(string url)
        {
            try
            {

                //var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                //发送get请求
                var response = await _httpClients.GetAsync(BaseApi._baseUrl + url);
                // 读取响应内容  
                string responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
                {
                    // 忽略JSON属性名称的大小写
                    PropertyNameCaseInsensitive = true
                });

                //发送get请求
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        //发送POST请求
        public async Task<T> PostAsync<T>(string url, Object data)
        {
            //await AddHeaders(_httpClients.DefaultRequestHeaders);
            //AddHeaders(_httpClients.DefaultRequestHeaders);
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            // 发送POST请求  
            var response = _httpClients.PostAsync(BaseApi._baseUrl + url, content);
            // 读取响应内容  
            string responseBody = await response.Result.Content.ReadAsStringAsync();
            // 解析JSON响应  
            var result = JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions
            {
                // 忽略JSON属性名称的大小写
                PropertyNameCaseInsensitive = true
            });
            // 输出响应内容  
            //Console.WriteLine(responseBody);
            return result;
            //return await _httpClients.PostAsync(url, content);
        }



        public async Task<BaseResponse<T>> PostUrlAsync<T>(string url, Object data)
        {
            //AddHeaders(_httpClients.DefaultRequestHeaders);
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            // 发送POST请求  
            var response = _httpClients.PostAsync(url, content);
            // 读取响应内容  
            string responseBody = await response.Result.Content.ReadAsStringAsync();
            // 解析JSON响应  
            var result = JsonSerializer.Deserialize<BaseResponse<T>>(responseBody, new JsonSerializerOptions
            {
                // 忽略JSON属性名称的大小写
                PropertyNameCaseInsensitive = true
            });
            // 输出响应内容  
            //Console.WriteLine(responseBody);
            return result;
            //return await _httpClients.PostAsync(url, content);
        }



        public async Task<string> PostKnowledgeBAsync<T>(string url, Object data)
        {
            //AddHeaders(_httpClients.DefaultRequestHeaders);
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            // 发送POST请求  
            var response = _httpClients.PostAsync(url, content);
            // 读取响应内容  
            string result = await response.Result.Content.ReadAsStringAsync();
            // 解析JSON响应  
            //var result = JsonSerializer.Deserialize<BaseResponse<T>>(responseBody, new JsonSerializerOptions
            //{
            //    // 忽略JSON属性名称的大小写
            //    PropertyNameCaseInsensitive = true
            //});
            // 输出响应内容  
            //Console.WriteLine(responseBody);
            return result;
            //return await _httpClients.PostAsync(url, content);
        }

        //添加请求头信息   
        //private async Task AddHeaders(HttpRequestHeaders headers)
        //{
        //    var accessToken = await _appSettingsService.GetAccessTokenAsync();
        //    var refreshToken = await _appSettingsService.GetRefreshTokenAsync();

        //    if (accessToken != null)
        //    {
        //        //解析accessToken
        //        //解析JWT
        //        //var jwt = JwtHelper.Decode(accessToken.Result);

        //        string token = accessToken;
        //        var handler = new JwtSecurityTokenHandler();
        //        var jwtToken = handler.ReadJwtToken(token);
        //        var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
        //        var now = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        //        if (DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp)).DateTime < DateTime.Parse(now))
        //        {
        //            // 添加或修改请求头信息
        //            if (headers.Contains("Authorization"))
        //            {
        //                headers.Remove("Authorization");
        //            }
        //            headers.Add("Authorization", "Bearer " + token);
        //            //刷新accessToken
        //            var refreshResult = await GetTokenAsync<BaseResponse<string>>(AppAuthApi._refreshToken + "?accessToken=" + refreshToken + "");
        //            if (refreshResult != null && refreshResult.Code == StatusCode.HTTP_Successful)
        //            {
        //                //更新accessToken
        //                await _appSettingsService.SetAccessTokenAsync(refreshResult.Result);
        //                token = refreshResult.Result;
        //            }
        //            else
        //            {
        //                await _appNavigator.NavigateAsync(AppRoutes.SignIn, false, true);
        //                _appSettingsService.Clear();
        //                return;
        //            }

        //        }

        //        // 添加或修改请求头信息
        //        if (headers.Contains("Authorization"))
        //        {
        //            headers.Remove("Authorization");
        //        }
        //        headers.Add("Authorization", "Bearer " + token);

        //        //if (headers.Where(h => h.Key == "Authorization").Count() == 0)
        //        //{
        //        //headers.Add("Authorization", "Bearer " + token);
        //        //}
        //        //else {
        //        //    headers.Where(h => h.Key == "Authorization").re = "Bearer " + token;
        //        //    headers["Authorization"].First().Value = "Bearer " + token;
        //        //}
        //    }
        //    return;
        //}
    }
}
