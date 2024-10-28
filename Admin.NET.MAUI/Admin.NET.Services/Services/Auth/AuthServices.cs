using Admin.NET.Entity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Admin.NET.Services.Services.Auth
{
    public class AuthServices
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public Task<TokenModel> Login(string username, string password)
        {

            var body = new StringContent($"{{\"account\": \"{username}\", \"password\": \"{password}\" , \"codeId\": 0 , \"code\": ''  }}", Encoding.UTF8, "application/json");


            // 创建HttpContent对象  
            //var content = new StringContent(body, Encoding.UTF8, "application/json");

            try
            {
                // 发送POST请求  
                var response = _httpClient.PostAsync("http://localhost:5005/api/sysAuth/login", body);

                // 确保HTTP成功状态值  
                //response.EnsureSuccessStatusCode();

                // 读取响应内容  
                string responseBody = response.Result.Content.ReadAsStringAsync().Result;
                //var options = new JsonSerializerOptions
                //{
                //    PropertyNameCaseInsensitive = true,
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //};

                // 解析JSON响应  
                var result = JsonSerializer.Deserialize<BaseResponse<TokenModel>>(responseBody, new JsonSerializerOptions
                {
                    // 忽略JSON属性名称的大小写
                    PropertyNameCaseInsensitive = true
                });
                //var result = JsonConvert.DeserializeObject<BaseResponse<TokenModel>>(responseBody, new JsonSerializerSettings
                //{
                //    // 忽略JSON属性名称的大小写
                //    PropertyNameCaseInsensitive = true
                //});

                // 输出响应内容  
                //Console.WriteLine(responseBody);
                return Task.FromResult(result.Result);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return null;
        }
    }
}
