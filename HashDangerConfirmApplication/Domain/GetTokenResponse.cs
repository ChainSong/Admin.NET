using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlaApplication.Domain
{
    public class GetTokenResponse
    {
        public bool Success { get; set; }
        public string Result { get; set; }
        public GetTokenResponseDetail data { get; set; }
    }
    public class GetTokenResponseDetail
    {
        public string token { get; set; }
        public string refresh_token { get; set; }
        public string sessionKey { get; set; }
        public object? expires { get; set; }
        public object? unionId { get; set; }
        public object? openId { get; set; }
        public string accessToken { get; set; }
    }
}
