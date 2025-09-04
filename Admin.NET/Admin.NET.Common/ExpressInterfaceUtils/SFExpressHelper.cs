// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//using XSystem.Security.Cryptography;

namespace Admin.NET.Common.ExpressInterfaceUtils;
public static class SFExpressHelper
{
    //public static string MD5ToBase64String(string str)
    //{
    //    MD5 md5 = new MD5CryptoServiceProvider();
    //    byte[] MD5 = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));//MD5(注意UTF8编码)
    //    string result = Convert.ToBase64String(MD5);//Base64
    //    return result;
    //}
    public static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }
    public static string callSfExpressServiceByCSIM(string reqURL, string partnerID, string requestID, string serviceCode, string timestamp, string msgDigest, string msgData)
    {
        String result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqURL);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

        Dictionary<string, string> content = new Dictionary<string, string>();
        content["partnerID"] = partnerID;
        content["requestID"] = requestID;
        content["serviceCode"] = serviceCode;
        content["timestamp"] = timestamp;
        content["msgDigest"] = msgDigest;
        content["msgData"] = msgData;

        if (!(content == null || content.Count == 0))
        {
            StringBuilder buffer = new StringBuilder();
            int i = 0;
            foreach (string key in content.Keys)
            {
                if (i > 0)
                {
                    buffer.AppendFormat("&{0}={1}", key, content[key]);
                }
                else
                {
                    buffer.AppendFormat("{0}={1}", key, content[key]);
                }
                i++;
            }

            byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

        }

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }
    public static string UrlEncode(string str)
    {

        StringBuilder builder = new StringBuilder();
        foreach (char c in str)
        {
            if (System.Web.HttpUtility.UrlEncode(c.ToString()).Length > 1)
            {
                builder.Append(System.Web.HttpUtility.UrlEncode(c.ToString()).ToUpper());
            }
            else
            {
                builder.Append(c);
            }
        }
        return builder.ToString();
    }
}
