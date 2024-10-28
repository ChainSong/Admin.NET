using Admin.NET.Entity;
using Admin.NET.Services.Services.Auth;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace Admin.NET.MAUI2C;

public partial class SignInPageViewModel(
        IAppSettingsService appSettingsService,
        IAppNavigator appNavigator) : BaseViewModel(appNavigator)
{
    private readonly IAppSettingsService appSettingsService = appSettingsService;

    public SignInFormModel Form { get; init; } = new();


    //private readonly HttpClient _httpClient = new HttpClient();


    [RelayCommand]
    async Task SignInAsync()
    {
        var isValid = Form.IsValid();
        if (!isValid)
        {
            return;

        }
        //Login
        AuthModel authModel = new AuthModel();
        authModel.Account = Form.UserName;
        authModel.Password = Form.Password;
        //AuthServices authServices = new AuthServices();
        var result = await new AsyncNetworkHttpClient(appSettingsService, appNavigator).PostAsync<BaseResponse<TokenModel>>(AppAuthApi._login, authModel);
        //AddCookies(result.Result);
        //var body = new StringContent($"{{\"account\": \"{Form.UserName}\", \"password\": \"{Form.Password}\" , \"codeId\": 0 , \"code\": ''  }}", Encoding.UTF8, "application/json");
        await appSettingsService.SetAccessTokenAsync(
                   //Convert.ToBase64String(
                   //    System.Text.Encoding.UTF8.GetBytes(
                   result.Result.AccessToken
        //)
        //)
        );

        await appSettingsService.SetRefreshTokenAsync(
                  //Convert.ToBase64String(
                  //    System.Text.Encoding.UTF8.GetBytes(
                  result.Result.RefreshToken
      //    )
      //)
      );
        //var adasd = appSettingsService.GetAccessTokenAsync();
        //var adasdaa = appSettingsService.GetRefreshTokenAsync();
        await GoHomeAsync();

        // 创建HttpContent对象  
        //var content = new StringContent(body, Encoding.UTF8, "application/json");

        //try
        //{
        //    // 发送POST请求  
        //    var response = _httpClient.PostAsync("http://180.169.76.197:8070/api/sysAuth/login", body);

        //    // 确保HTTP成功状态值  
        //    //response.EnsureSuccessStatusCode();

        //    // 读取响应内容  
        //    string responseBody = response.Result.Content.ReadAsStringAsync().Result;

        //    // 输出响应内容  
        //    //Console.WriteLine(responseBody);
        //}
        //catch (HttpRequestException e)
        //{
        //    Console.WriteLine("\nException Caught!");
        //    Console.WriteLine("Message :{0} ", e.Message);
        //}
        ////var response =   _httpClient.PostAsync("http://localhost:5005/api/login", body);
        ////response.EnsureSuccessStatusCode();

        ////var content = await response.Content.ReadAsStringAsync();
        //// 假设返回的内容是JSON，且包含访问令牌
        //// 你需要根据Furion的返回格式来解析具体的访问令牌
        ////return content; // 返回包含访问令牌的字符串
        //return Task.CompletedTask;

        //appSettingsService.SetAccessTokenAsync(
        //    Convert.ToBase64String(
        //        System.Text.Encoding.UTF8.GetBytes(
        //            $"{Form.UserName}:{Form.Password}"
        //        )
        //    )
        //);

        //return GoHomeAsync();
    }














    [RelayCommand]
    Task SignUpAsync() => AppNavigator.NavigateAsync(AppRoutes.SignUp);

    [RelayCommand]
    Task ForgotPasswordAsync() => AppNavigator.NavigateAsync(AppRoutes.ForgotPassword);

    [RelayCommand]
    Task SignInWithSocialAccountAsync(SocialAccountType socialAccountType)
    {
        return GoHomeAsync();
    }

    Task GoHomeAsync() => AppNavigator.NavigateAsync(AppRoutes.Home);
}

