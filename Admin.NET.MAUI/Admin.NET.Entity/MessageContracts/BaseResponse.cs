using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Entity
{
    public class BaseResponse
    {
        public StatusCode Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        //public T Result { get; set; }
        public string Extras { get; set; }
        public string Time { get; set; }
    }


    public class BaseResponse<T>
    {
        public StatusCode Code { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
        public string Extras { get; set; }
        public string Time { get; set; }
    }


    public class Response
    {
        public StatusCode Code { get; set; }
        public string Msg { get; set; }

    }

    public class Response<T>
    {
        public StatusCode Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
    }

    public class Response<T, R>
    {
        public StatusCode Code { get; set; }

        public string Msg { get; set; }

        public T Data { get; set; }
        public R Result { get; set; }
    }
}
