using Admin.NET.Entity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Admin.NET.MAUI
{
    public static class RequestUrl
    {
        //private readonly IAppSettingsService _appSettingsService;
        //protected IAppNavigator _appNavigator { get; }
        //新建一个http实例
        private static HttpClient _httpClient = new HttpClient();

        //public RequestUrl(IAppSettingsService appSettingsService, IAppNavigator appNavigator)
        //{
        //    _appSettingsService = appSettingsService;
        //    _appNavigator = appNavigator;
        //    AddHeaders(_httpClient.DefaultRequestHeaders);
        //}

        public static async Task<BaseResponse<T>> GetAsync<T>(string url)
        {
            try
            {

                //var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                //发送get请求
                var response = await _httpClient.GetAsync(BaseApi._baseUrl + url);
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
                return new BaseResponse<T>() { Code = StatusCode.Error, Message = ex.Message };
            }
        }

        //发送POST请求
        public static async Task<BaseResponse<T>> PostAsync<T>(string url, Object data)
        {
            //AddHeaders(_httpClient.DefaultRequestHeaders);
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            //请求时间20秒之后超市
            //_httpClient.Timeout = new TimeSpan(0, 0, 20);
            // 发送POST请求  
            var response = _httpClient.PostAsync(url, content);
            // 读取响应内容  
            string responseBody = await response.Result.Content.ReadAsStringAsync();
            return new BaseResponse<T>() { Code = StatusCode.HTTP_Successful, Message = responseBody };
            // 解析JSON响应  
            //var result = JsonSerializer.Deserialize<BaseResponse<T>>(responseBody, new JsonSerializerOptions
            //{
            //    // 忽略JSON属性名称的大小写
            //    PropertyNameCaseInsensitive = true
            //});
            // 输出响应内容  
            //Console.WriteLine(responseBody);
            //return responseBody;
            //return await _httpClient.PostAsync(url, content);
        }



        //public async Task<BaseResponse<T>> PostUrlAsync<T>(string url, Object data)
        //{
        //    //AddHeaders(_httpClient.DefaultRequestHeaders);
        //    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        //    // 发送POST请求  
        //    var response = _httpClient.PostAsync(url, content);
        //    // 读取响应内容  
        //    string responseBody = await response.Result.Content.ReadAsStringAsync();
        //    // 解析JSON响应  
        //    var result = JsonSerializer.Deserialize<BaseResponse<T>>(responseBody, new JsonSerializerOptions
        //    {
        //        // 忽略JSON属性名称的大小写
        //        PropertyNameCaseInsensitive = true
        //    });
        //    // 输出响应内容  
        //    //Console.WriteLine(responseBody);
        //    return result;
        //    //return await _httpClient.PostAsync(url, content);
        //}

        ////添加请求头信息   
        //private async void AddHeaders(HttpRequestHeaders headers)
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
        //            headers.Add("Authorization", "Bearer " + token);
        //            //刷新accessToken
        //            var refreshResult = await GetAsync<string>(AppAuthApi._refreshToken + "?accessToken=" + refreshToken + "");
        //            if (refreshResult.Code == StatusCode.Success)
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
        //        headers.Add("Authorization", "Bearer " + token);
        //    }
        //}
    }
}
