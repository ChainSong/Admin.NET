using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI2C
{


    public class SysMessage
    {
        public int code { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public List<Notifications> result { get; set; }
        public string extras { get; set; }
        public string time { get; set; }
    }

    public class Notifications
    {
        public string title { get; set; }
        public string content { get; set; }
        public int type { get; set; }
        public int publicUserId { get; set; }
        public string publicUserName { get; set; }
        public int publicOrgId { get; set; }
        public string publicOrgName { get; set; }
        public string publicTime { get; set; }
        public object cancelTime { get; set; }
        public int status { get; set; }
        public string createTime { get; set; }
        public string updateTime { get; set; }
        public string createUserId { get; set; }
        public string updateUserId { get; set; }
        public bool isDelete { get; set; }
        public long id { get; set; }
    }



}
