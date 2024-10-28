using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI.AppApis
{
    public static class GptApi
    {
        /// <summary>
        /// 聊天接口
        /// </summary>
        public const string _GptGetData = "/api/wMSGpt/getData";


        //public const string _chatDbUrl = "http://localhost:6006/api/v1/chat/completions";

        /// <summary>
        /// Chat Data
        /// </summary>
        public const string _ChatData = "<chart-view content=\"{&quot;type&quot;: &quot;Data display method&quot;, &quot;sql&quot;: &quot;SELECT * FROM wms.student&quot;, &quot;data&quot;: [{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;},{&quot;Id&quot;: 1, &quot;name&quot;: &quot;张三&quot;, &quot;age&quot;: 2, &quot;fenShu&quot;: 50, &quot;class&quot;: &quot;数学&quot;}]}\" />";

        /// <summary>
        /// Knowledge Base
        /// </summary>
        public const string _KnowledgeBase = "{\"select_param\": \"DDDDD\", \"chat_mode\": \"chat_knowledge\",\"model_name\": \"qwen-1.8b-chat\",\"user_input\": \"查询预出库订单\",\"conv_uid\": \"1229a9a4-8929-11ef-8d1f-0242ac110008\"}";

    }
}
