using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.MAUI2C
{
    public class HttpHeadersFilterAttribute : Attribute
    {
        public HttpHeadersFilterAttribute(HttpClient filter)
        {
            var Filter = filter;
        }
    }
}
