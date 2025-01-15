using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI
{
    public static class BaseApi
    {

        //测试
        //public const string _baseUrl = "https://wms.rbow-logistics.com.cn:8000";
        ////public const string _baseUrl = "http://localhost:5005";
        //public const string _chatDbUrl = "http://localhost:6006/api/v1/chat/completions";

        //正式
        public const string _baseUrl = "https://wms.rbow-logistics.com.cn:8074";
        //public const string _baseUrl = "http://localhost:5005";
        public const string _chatDbUrl = "http://localhost:6006/api/v1/chat/completions";
    }
}
