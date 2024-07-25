using UraniumUI;

namespace MauiAdmin.Pages;

public partial class JiangHu : ContentPage
{
	public JiangHu()
	{
		InitializeComponent();
        //BindingContext = this;

        //WebBrowser.Navigating += WebBrowserNavigating;
        App.Current.RequestedThemeChanged += (_, _) => Reset();
    }

    private void Reset()
    {
        if (BindingContext != null)
        {
            BindingContext = UraniumServiceProvider.Current.GetRequiredService(BindingContext.GetType());
        }
    }
    private void WebBrowserNavigating(object sender, WebNavigatingEventArgs e)
    {


        if (e.Url.Contains("/api/"))
        {
            //    var dataId = e.Url.Substring(e.Url.IndexOf("/api/") + 5);
            //    e.Cancel = true;
            //    Task.Run(() => {
            //        Dispatcher.Dispatch(async () => {
            //            //var data = await WebBrowser.EvaluateJavaScriptAsync($"getData({dataId})");
            //            //var cdData = JsonSerializer.Deserialize<CsData>(data.Replace("\\", ""));

            //            //Now Call the corresponding C# Funktion with data
            //            // - this is only a simple example
            //            // - you could also use a ICommand Interface, create an instance of a class implementing ICommand dynamically from csData.command and call it's execute Method, or ...
            //            //if (cdData != null)
            //            //{
            //            //    switch (cdData.command)
            //            //    {
            //            //        case "Debug.WriteLine":
            //            //            Debug.WriteLine(cdData.data);

            //            //            //Just for the Show...
            //            //            await WebBrowser.EvaluateJavaScriptAsync($"log('{cdData.command} with dataId {cdData.dataId} was executed...')");
            //            //            break;
            //            //    }
            //            //}
            //        });
            //    });
            //}
        }
    }
}