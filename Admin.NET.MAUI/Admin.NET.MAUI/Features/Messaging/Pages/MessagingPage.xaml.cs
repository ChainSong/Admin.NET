using Admin.NET.Entity;
using Admin.NET.MAUI.AppApis;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace Admin.NET.MAUI;

public partial class MessagingPage : INotifyPropertyChanged
{
    //public ObservableCollection<Message> Messages { get; set; }

    public MessagingPageViewModel _vm;

    private ObservableCollection<MessageModel> messages;

    public ObservableCollection<MessageModel> Messages { get => messages; set { messages = value; OnPropertyChanged(nameof(Messages)); } }

    public MessagingPage(MessagingPageViewModel vm)
    {
        InitializeComponent();
        vm.Initialize();
        _vm = vm;
        BindingContext = this;
        Messages = new ObservableCollection<MessageModel>
        {  
            // 初始消息  
            new MessageModel {   Msg = "Hello!", Sender = "Alice", BackgroundColor = Colors.White }
        };
    }
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private List<string> strings = new List<string>();

    private async void SendButton_Clicked(object sender, EventArgs e)
    {

        //string sqlStatements= "select top 10 * from WMS_PreOrder ";
        //var GptResults = await new AsyncNetworkHttpClient(_vm._appSettingsService, _vm._appNavigator).GetAsync<BaseResponse<string>>(GptApi._GptGetData, sqlStatements);


        string messageText = MessageEntry.Text;
        if (!string.IsNullOrEmpty(messageText))
        {
            flag = 0;
            //    // 发送消息逻辑  
            Messages.Add(new MessageModel
            {
                Msg = messageText,
                Sender = "我",
                BackgroundColor = Colors.White,
                IsString = true
            });
            var GetChatGptData = await GetChatGpt(messageText);
            if (GetChatGptData.Code == StatusCode.Error)
            {

            }

        }
        ChatList.ScrollTo(Messages.Last(), ScrollToPosition.End, true);
    }
    int flag = 0;
    private async Task<Response> GetChatGpt(string messageText)
    {
        try
        {
            //string str = "{'select_param': 'DDDDD', 'chat_mode': 'chat_knowledge','model_name': 'qwen-1.8b-chat','user_input': '" + messageText + "','conv_uid': '1229a9a4-8929-11ef-8d1f-0242ac110008'}";
            strings = new List<string>();

            //string str = "{'select_param': 'DDDDD', 'chat_mode': 'chat_knowledge','model_name': 'qwen-1.8b-chat','user_input': '" + messageText + "','conv_uid': '1229a9a4-8929-11ef-8d1f-0242ac110008'}";
            ChatJson chatJson = new ChatJson();
            chatJson.select_param = "AdminWMS";
            chatJson.chat_mode = "chat_knowledge";
            chatJson.model_name = "qwen-1.8b-chat";
            chatJson.user_input = messageText;
            chatJson.conv_uid = "55dc0adc-8967-11ef-985a-0242ac110004";
            //string htmlContent = "<chart-view content=\"{&quot;type&quot;: &quot;Data display method&quot;, &quot;sql&quot;: &quot;SELECT * FROM wms.student&quot;, &quot;data&quot;: [{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;},{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;}]}\" />";
            var result = await new AsyncNetworkHttpClientNoToken(_vm._appSettingsService, _vm._appNavigator).PostKnowledgeBAsync<string>(BaseApi._chatDbUrl, chatJson);

            //List<string> strs = new List<string>();
            //strs=result.Split('data:').ToList();
            //string[] sArray = Regex.Split(result, "js", RegexOptions.IgnoreCase)
            //string sqlPattern = @"```sql\n(.+?)\n```";
            //Match match = Regex.Match(result.ToString(), sqlPattern);

            string sqlPatterns = @"```sql(.+?)```";
            string ssssss = result;
            Match match = Regex.Match(ssssss, sqlPatterns);
            //string sqlStatements = "select top 10 * from WMS_PreOrder ";
            //var GptResults = await new AsyncNetworkHttpClient(_vm._appSettingsService, _vm._appNavigator).GetAsync<BaseResponse<string>>(GptApi._GptGetData, sqlStatements);

            if (match.Success)
            {

                string sqlStatement = match.Groups[1].Value;
                sqlStatement = sqlStatement.Replace("\\n", " ");
                ChatSql chatSql = new ChatSql();
                //sqlStatement = "select top 10 * from WMS_PreOrder ";
                chatSql.type = "Data display method";
                chatSql.sql = sqlStatement;

                var GptResult = await new AsyncNetworkHttpClient(_vm._appSettingsService, _vm._appNavigator).PostAsync<BaseResponse<Response<dynamic>>>(GptApi._GptGetData, chatSql);
                var asdas = GptResult;
                var jsonContent = GptResult.Result.Data.ToString();
                JArray jsonArray = JArray.Parse(jsonContent);

                //Console.WriteLine("<table>");
                //Console.WriteLine("<tr><th>Name</th><th>Age</th><th>City</th></tr>");
                string _html = "";
                string _htmlhead = "";
                string _htmlbody = "";
                foreach (JObject item in jsonArray)
                {
                    _htmlhead = "<tr>";
                    _htmlbody += "<tr>";
                    string patterns = @"{}";
                    Match matchs = Regex.Match(item.ToString(), patterns);
                    // 将JSON字符串转换为Dictionary
                    Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.ToString());

                    // 打印Dictionary的键值对
                    foreach (var kvp in dict)
                    {
                        _htmlhead += "<td>" + kvp.Key + "</td>";
                        _htmlbody += "<td>" + kvp.Value + "</td>";
                    }
                    _htmlhead += "</tr>";
                    _htmlbody += "</tr>";
                }
                _html = "<table>" + _htmlhead + "" + _htmlbody + "</table>";


                Messages.Add(new MessageModel
                {
                    Msg = _html,
                    Sender = "Alice",
                    BackgroundColor = Colors.White,
                    IsDataTable = true
                });
                MessageEntry.Text = "";

            }
            else
            {
                flag += 1;
                if (flag > 3)
                {
                    Messages.Add(new MessageModel
                    {
                        Msg = "失败",
                        Sender = "Alice",
                        BackgroundColor = Colors.White,
                        IsDataTable = true
                    });
                    return new Response() { Code = StatusCode.Error, Msg = "Error" };
                }
                else
                {
                    GetChatGpt(messageText);
                }
            }
        }
        catch (Exception ex)
        {
            flag += 1;
            if (flag > 3)
            {
                Messages.Add(new MessageModel
                {
                    Msg = "失败",
                    Sender = "Alice",
                    BackgroundColor = Colors.White,
                    IsDataTable = true
                });
                return new Response() { Code = StatusCode.Error, Msg = "Error" };
            }
            else
            {
                GetChatGpt(messageText);
            }

        }
        return new Response() { Code = StatusCode.Success, Msg = "Success" };
    }
    //private async void Pairs()
    //{
    //    string messageText = MessageEntry.Text;
    //    if (!string.IsNullOrEmpty(messageText))
    //    {
    //        Messages.Add(new MessageModel { Msg = messageText, IsDataTable = true });
    //        MessageEntry.Text = string.Empty;
    //        // 模拟接收对方回复（实际应用中需与后端交互）
    //        await Task.Delay(1000);
    //        Messages.Add(new MessageModel { Msg = "Received: " + messageText, IsDataTable = false });
    //    }

    //    ChatList.ScrollTo(Messages.Last() , position: ScrollToPosition.End, false);
    //    //Device.BeginInvokeOnMainThread(() => ChatList.ScrollTo(Messages.LastOrDefault(), ScrollToPosition.MakeVisible, true));
    //}

    //private void ChatPage_Appearing(object sender, EventArgs e)
    //{
    //    if (Messages.Count > 0)
    //    {
    //        ChatList.ScrollTo(Messages.LastOrDefault(), ScrollToPosition.End, false);
    //    }
    //}
    //private void ChatListView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    //{
    //    // 判断是否滚动到底部
    //    isAtBottom = e.LastVisibleItemIndex == Messages.Count - 1;
    //}
}
public class ChatJson
{
    public string select_param { get; set; }
    public string chat_mode { get; set; }
    public string model_name { get; set; }
    public string user_input { get; set; }
    public string conv_uid { get; set; }
}


public class ChatSql
{
    public string type { get; set; }
    public string sql { get; set; }

    public List<dynamic> data { get; set; }
    //public decimal data { get; set; } 
}