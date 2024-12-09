using Admin.NET.Entity;
using Admin.NET.Services.Services.Auth;

namespace Admin.NET.MAUI;

public partial class GamesPage
{

    private readonly IAppSettingsService _appSettingsService;
    public GamesPage(GamesPageViewModel vm, IAppSettingsService appSettingsService)
    {
        InitializeComponent();
        _appSettingsService = appSettingsService;
        BindingContext = vm;
        AddCookies();

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        AddCookies();
        // 每次进入页面时需要执行的代码
        // 例如：更新UI, 请求数据等
        //Console.WriteLine("MainPage is appearing");
    }


    public async void AddCookies()
    {

        var accessToken = _appSettingsService.GetAccessTokenAsync();
        var refreshToken = _appSettingsService.GetRefreshTokenAsync();

        var webView = new WebView
        {
            Source = "https://wms.rbow-logistics.com.cn:8036",
        };

        // 注入JavaScript代码，将token设置到页面中
        webView.Navigated += async (s, e) =>
        {
            var js = "window.localStorage.setItem('access-token', '" + accessToken.Result + "');";
            await webView.EvaluateJavaScriptAsync(js);
            var jss = "window.localStorage.setItem('x-access-token', '" + refreshToken.Result + "');";
            await webView.EvaluateJavaScriptAsync(jss);
        };

        //window.localStorage.getItem('vue-next-admin:access-token')

        // 加载页面
        Content = webView;

        //return webRequest;

        //CookieContainer cookieContainer = new CookieContainer();
        //Uri uri = new Uri("http://180.169.76.197:8071/#/dashboard/home", UriKind.RelativeOrAbsolute);

        //Cookie cookie = new Cookie
        //{
        //    Name = "DotNetMAUICookie",
        //    Expires = DateTime.Now.AddDays(1),
        //    Value = "My cookie",
        //    Domain = uri.Host,
        //    Path = "/",

        //};
        //cookieContainer.Add(uri, cookie);
        //webView.Cookies = cookieContainer;
        //webView.Source = new UrlWebViewSource { Url = uri.ToString() };
    }
}

