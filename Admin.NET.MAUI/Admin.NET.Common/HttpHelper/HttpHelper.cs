using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Admin.NET.Common
{
    public static class HttpHelper
    {

        /// <summary>
        /// http/https请求响应
        /// </summary>
        /// <param name="getOrPost"></param>
        /// <param name="url">地址（要带上http或https）</param>
        /// <param name="headers">请求头</param>
        /// <param name="parameters">提交数据</param>
        /// <param name="dataEncoding">编码类型 utf-8</param>
        /// <param name="contentType">application/x-www-form-urlencoded</param>
        /// <returns></returns>
        public static HttpWebResponse HttpRequest(
            string getOrPost,
            string url,
            Dictionary<string, string> headers,
            Dictionary<string, string> parameters,
            Encoding dataEncoding,
            string contentType
            )
        {
            var request = CreateRequest(getOrPost, url, headers, parameters, dataEncoding, contentType);

            //如果需要POST数据  
            if (getOrPost == "POST" && !(parameters == null || parameters.Count == 0))
            {
                var data = FormatPostParameters(parameters, dataEncoding, contentType);

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }

            WebResponse Res = null;
            try
            {
                Res = request.GetResponse();
            }
            catch (WebException ex)
            {
                Res = (HttpWebResponse)ex.Response;
            }
            catch (Exception e)
            {
                throw e;
            }

            if (null == Res)
            {
                return request.GetResponse() as HttpWebResponse;
            }

            return (HttpWebResponse)Res;
        }


        /// <summary>
        /// 创建HTTP请求对象
        /// </summary>
        /// <param name="getOrPost"></param>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="parameters"></param>
        /// <param name="paraEncoding"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        private static HttpWebRequest CreateRequest(
            string getOrPost
            , string url
            , Dictionary<string, string> headers
            , Dictionary<string, string> parameters
            , Encoding paraEncoding
            , string contentType
            )
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (parameters != null && parameters.Count > 0 && paraEncoding == null)
            {
                throw new ArgumentNullException("requestEncoding");
            }

            HttpWebRequest request = null;
            //判断是否是https  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (getOrPost == "GET")
            {
                request.Method = "GET";

                if (parameters != null && parameters.Count > 0)
                {
                    url = FormatGetParametersToUrl(url, parameters, paraEncoding);
                }
            }
            else
            {
                request.Method = "POST";
            }

            if (contentType == null)
            {
                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            }
            else
            {
                request.ContentType = contentType;
            }

            //POST的数据大于1024字节的时候，如果不设置会分两步
            request.ServicePoint.Expect100Continue = false;
            request.ServicePoint.ConnectionLimit = int.MaxValue;

            if (headers != null)
            {
                FormatRequestHeaders(headers, request);
            }

            return request;
        }


        /// <summary>
        /// 格式化请求头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="request"></param>
        private static void FormatRequestHeaders(Dictionary<string, string> headers, HttpWebRequest request)
        {
            foreach (var hd in headers)
            {
                //因为HttpWebRequest中很多标准标头都被封装成只能通过属性设置，添加的话会抛出异常
                switch (hd.Key.ToLower())
                {
                    case "connection":
                        request.KeepAlive = false;
                        break;
                    case "content-type":
                        request.ContentType = hd.Value;
                        break;
                    case "transfer-enconding":
                        request.TransferEncoding = hd.Value;
                        break;
                    default:
                        request.Headers.Add(hd.Key, hd.Value);
                        break;
                }
            }
        }


        /// <summary>
        /// 格式化Get请求参数
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="parameters">参数</param>
        /// <param name="paraEncoding">编码格式</param>
        /// <returns></returns>

        private static string FormatGetParametersToUrl(string url, Dictionary<string, string> parameters, Encoding paraEncoding)
        {
            if (url.IndexOf("?") < 0)
                url += "?";
            int i = 0;
            string sendContext = "";
            foreach (var parameter in parameters)
            {
                if (i > 0)
                {
                    sendContext += "&";
                }

                sendContext += HttpUtility.UrlEncode(parameter.Key, paraEncoding)
                       + "=" + HttpUtility.UrlEncode(parameter.Value, paraEncoding);
                ++i;
            }

            url += sendContext;
            return url;
        }


        /// <summary>
        /// 格式化Post请求参数
        /// </summary>
        /// <param name="parameters">编码格式</param>
        /// <param name="dataEncoding">编码格式</param>
        /// <param name="contentType">类型</param>
        /// <returns></returns>
        private static byte[] FormatPostParameters(Dictionary<string, string> parameters, Encoding dataEncoding, string contentType)
        {
            string sendContext = "";
            int i = 0;
            if (!string.IsNullOrEmpty(contentType) && contentType.ToLower().Trim() == "application/json")
            {
                sendContext = "{";
            }

            foreach (var para in parameters)
            {
                if (!string.IsNullOrEmpty(contentType) && contentType.ToLower().Trim() == "application/json")
                {
                    if (i > 0)
                    {
                        if (para.Value.StartsWith("{"))
                        {
                            sendContext += string.Format(@",""{0}"":{1}", para.Key, para.Value);
                        }
                        else
                        {
                            sendContext += string.Format(@",""{0}"":""{1}""", para.Key, para.Value);
                        }

                    }
                    else
                    {
                        if (para.Value.StartsWith("{"))
                        {
                            sendContext += string.Format(@"""{0}"":{1}", para.Key, para.Value);
                        }
                        else
                        {
                            sendContext += string.Format(@"""{0}"":""{1}""", para.Key, para.Value);
                        }

                    }
                }
                else
                {
                    if (i > 0)
                    {
                        sendContext += string.Format("&{0}={1}", para.Key, HttpUtility.UrlEncode(para.Value, dataEncoding));
                    }
                    else
                    {
                        sendContext = string.Format("{0}={1}", para.Key, HttpUtility.UrlEncode(para.Value, dataEncoding));
                    }
                }

                i++;
            }

            if (!string.IsNullOrEmpty(contentType) && contentType.ToLower().Trim() == "application/json")
            {
                sendContext += "}";
            }

            byte[] data = dataEncoding.GetBytes(sendContext);
            return data;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }

}
