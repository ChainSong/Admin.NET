

using Admin.NET.Application.Dtos.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Dtos
{


    public class Response
    {
        public StatusCode Code { get; set; }
        public string Msg { get; set; }

        public string ResponseType { get; set; }


    }

    public class Response<T>
    {
        public StatusCode Code { get; set; }

        public string Msg { get; set; }
        public string ResponseType { get; set; }

        public T Data { get; set; }
    }

    public class Response<T, R>
    {
        public StatusCode Code { get; set; }

        public string Msg { get; set; }
        public string ResponseType { get; set; }

        public T Data { get; set; }
        public R Result { get; set; }
    }
}
