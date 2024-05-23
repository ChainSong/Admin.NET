// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Common.ExpressInterfaceUtils;
using Admin.NET.Core;
using Admin.NET.Express.Interface;
using Admin.NET.Express.Strategy.SFExpress.Dto;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using XSystem.Security.Cryptography;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.BuildMarketingPartnershipRequest.Types;

namespace Admin.NET.Express.Strategy.SFExpress;

/// <summary>
/// 顺丰快递
/// </summary>
public class SFExpressServiceStrategy: IExpressServiceInterface
{
    public dynamic GetExpressData(SFExpressInput<SFRootobject> request)
    {
        string partnerId = request.PartnerId;//此处替换为您在丰桥平台获取的顾客编码       
        string checkword = request.Checkword;//此处替换为您在丰桥平台获取的校验码  
        string reqURL = request.Url;//生产环境
        string serviceCode = request.ServiceCode;//  "EXP_RECE_CREATE_ORDER";//下订单
        //request.Data.contactInfoList[0].tel = "13647294728";
        //request.Data.contactInfoList[1].tel = "13647294728";
        //request.Data.orderId = request.Data.orderId+"2";
        //request.Env = "sbox";
        string timestamp = GetTimeStamp(); //获取时间戳       

        string requestID = Guid.NewGuid().ToString(); //获取uuid
        //string msgData = request.Data.ToJson();
        //业务报文  去报文中的msgData（业务数据报文）
        string msgData = request.Data.ToJson();
            //"{\r\n\t\"cargoDetails\": [{\r\n\t\t\"amount\": 308.0,\r\n\t\t\"count\": 1.0,\r\n\t\t\"name\": \"君宝牌地毯\",\r\n\t\t\"unit\": \"个\",\r\n\t\t\"volume\": 0.0,\r\n\t\t\"weight\": 0.1\r\n\t}],\r\n\t\"contactInfoList\": [{\r\n\t\t\"address\": \"十堰市丹江口市公园路155号\",\r\n\t\t\"city\": \"十堰市\",\r\n\t\t\"company\": \"清雅轩保健品专营店\",\r\n\t\t\"contact\": \"张三丰\",\r\n\t\t\"contactType\": 1,\r\n\t\t\"county\": \"武当山风景区\",\r\n\t\t\"mobile\": \"17006805888\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}, {\r\n\t\t\"address\": \"湖北省襄阳市襄城区环城东路122号\",\r\n\t\t\"city\": \"襄阳市\",\r\n\t\t\"contact\": \"郭襄阳\",\r\n\t\t\"county\": \"襄城区\",\r\n\t\t\"contactType\": 2,\r\n\t\t\"mobile\": \"18963828829\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}],\r\n\t\"customsInfo\": {},\r\n\t\"expressTypeId\": 1,\r\n\t\"extraInfoList\": [],\r\n\t\"isOneselfPickup\": 0,\r\n\t\"language\": \"zh-CN\",\r\n\t\"monthlyCard\": \"7551234567\",\r\n\t\"orderId\": \"QIAO-20210618-003\",\r\n\t\"parcelQty\": 1,\r\n\t\"payMethod\": 1,\r\n\t\"totalWeight\": 6\r\n}";

        //将业务报文+时间戳+校验码组合成需加密的字符串(注意顺序)
        string toVerifyText = msgData + timestamp + checkword;

        string msgDigest = MD5ToBase64String(UrlEncode(msgData + timestamp + checkword));

        //因业务报文中可能包含加号、空格等特殊字符，需要urlEnCode处理
        //toVerifyText = UrlEncode(toVerifyText);
        string respJson = callSfExpressServiceByCSIM(reqURL, partnerId, checkword, serviceCode, timestamp, msgDigest, msgData);
        return respJson;

    }

    public dynamic PrintExpressData(SFExpressInput<SFRootobjectPrint> request)
    {
        string partnerId = request.PartnerId;//此处替换为您在丰桥平台获取的顾客编码       
        string checkword = request.Checkword;//此处替换为您在丰桥平台获取的校验码  
        string reqURL = request.Url;//生产环境
        string serviceCode = request.ServiceCode;//  "EXP_RECE_CREATE_ORDER";//下订单


        string timestamp = GetTimeStamp(); //获取时间戳       

        string requestID = Guid.NewGuid().ToString(); //获取uuid
        string msgData = request.Data.ToJson();
        //业务报文  去报文中的msgData（业务数据报文）
        //string msgData = "{\r\n\t\"cargoDetails\": [{\r\n\t\t\"amount\": 308.0,\r\n\t\t\"count\": 1.0,\r\n\t\t\"name\": \"君宝牌地毯\",\r\n\t\t\"unit\": \"个\",\r\n\t\t\"volume\": 0.0,\r\n\t\t\"weight\": 0.1\r\n\t}],\r\n\t\"contactInfoList\": [{\r\n\t\t\"address\": \"十堰市丹江口市公园路155号\",\r\n\t\t\"city\": \"十堰市\",\r\n\t\t\"company\": \"清雅轩保健品专营店\",\r\n\t\t\"contact\": \"张三丰\",\r\n\t\t\"contactType\": 1,\r\n\t\t\"county\": \"武当山风景区\",\r\n\t\t\"mobile\": \"17006805888\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}, {\r\n\t\t\"address\": \"湖北省襄阳市襄城区环城东路122号\",\r\n\t\t\"city\": \"襄阳市\",\r\n\t\t\"contact\": \"郭襄阳\",\r\n\t\t\"county\": \"襄城区\",\r\n\t\t\"contactType\": 2,\r\n\t\t\"mobile\": \"18963828829\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}],\r\n\t\"customsInfo\": {},\r\n\t\"expressTypeId\": 1,\r\n\t\"extraInfoList\": [],\r\n\t\"isOneselfPickup\": 0,\r\n\t\"language\": \"zh-CN\",\r\n\t\"monthlyCard\": \"7551234567\",\r\n\t\"orderId\": \"QIAO-20210618-001\",\r\n\t\"parcelQty\": 1,\r\n\t\"payMethod\": 1,\r\n\t\"totalWeight\": 6\r\n}";

        //将业务报文+时间戳+校验码组合成需加密的字符串(注意顺序)
        string toVerifyText = msgData + timestamp + checkword;

        string msgDigest = MD5ToBase64String(UrlEncode(msgData + timestamp + checkword));

        //因业务报文中可能包含加号、空格等特殊字符，需要urlEnCode处理
        //toVerifyText = UrlEncode(toVerifyText);
        string respJson = callSfExpressServiceByCSIM(reqURL, partnerId, checkword, serviceCode, timestamp, msgDigest, msgData);
        return respJson;
    }




    public dynamic TokenExpressData(SFExpressInput<SFRootobjectPrint> request)
    {
        string partnerId = request.PartnerId;//此处替换为您在丰桥平台获取的顾客编码       
        string checkword = request.Checkword;//此处替换为您在丰桥平台获取的校验码  
        //request.e
        string reqURL = request.UrlToken;
            //"https://sfapi-sbox.sf-express.com/oauth2/accessToken";//生产环境
        string serviceCode = request.ServiceCode;//  "EXP_RECE_CREATE_ORDER";//下订单


        string timestamp = GetTimeStamp(); //获取时间戳       

        string requestID = Guid.NewGuid().ToString(); //获取uuid
        string msgData = request.Data.ToJson();
        //业务报文  去报文中的msgData（业务数据报文）
        //string msgData = "{\r\n\t\"cargoDetails\": [{\r\n\t\t\"amount\": 308.0,\r\n\t\t\"count\": 1.0,\r\n\t\t\"name\": \"君宝牌地毯\",\r\n\t\t\"unit\": \"个\",\r\n\t\t\"volume\": 0.0,\r\n\t\t\"weight\": 0.1\r\n\t}],\r\n\t\"contactInfoList\": [{\r\n\t\t\"address\": \"十堰市丹江口市公园路155号\",\r\n\t\t\"city\": \"十堰市\",\r\n\t\t\"company\": \"清雅轩保健品专营店\",\r\n\t\t\"contact\": \"张三丰\",\r\n\t\t\"contactType\": 1,\r\n\t\t\"county\": \"武当山风景区\",\r\n\t\t\"mobile\": \"17006805888\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}, {\r\n\t\t\"address\": \"湖北省襄阳市襄城区环城东路122号\",\r\n\t\t\"city\": \"襄阳市\",\r\n\t\t\"contact\": \"郭襄阳\",\r\n\t\t\"county\": \"襄城区\",\r\n\t\t\"contactType\": 2,\r\n\t\t\"mobile\": \"18963828829\",\r\n\t\t\"province\": \"湖北省\"\r\n\t}],\r\n\t\"customsInfo\": {},\r\n\t\"expressTypeId\": 1,\r\n\t\"extraInfoList\": [],\r\n\t\"isOneselfPickup\": 0,\r\n\t\"language\": \"zh-CN\",\r\n\t\"monthlyCard\": \"7551234567\",\r\n\t\"orderId\": \"QIAO-20210618-001\",\r\n\t\"parcelQty\": 1,\r\n\t\"payMethod\": 1,\r\n\t\"totalWeight\": 6\r\n}";

        //将业务报文+时间戳+校验码组合成需加密的字符串(注意顺序)
        string toVerifyText = msgData + timestamp + checkword;

        string msgDigest = MD5ToBase64String(UrlEncode(msgData + timestamp + checkword));

        //因业务报文中可能包含加号、空格等特殊字符，需要urlEnCode处理
        //toVerifyText = UrlEncode(toVerifyText);
        string respJson = callSfExpressServiceByCSIM(reqURL+ "?partnerID="+ partnerId+ "&grantType=password" + "&secret="+ checkword);
        return respJson;
    }


    private static string JsonCompress(string msgData)
    {
        StringBuilder sb = new StringBuilder();
        using (StringReader reader = new StringReader(msgData))
        {
            int ch = -1;
            int lastch = -1;
            bool isQuoteStart = false;
            while ((ch = reader.Read()) > -1)
            {
                if ((char)lastch != '\\' && (char)ch == '\"')
                {
                    if (!isQuoteStart)
                    {
                        isQuoteStart = true;
                    }
                    else
                    {
                        isQuoteStart = false;
                    }
                }
                if (!char.IsWhiteSpace((char)ch) || isQuoteStart)
                {
                    sb.Append((char)ch);
                }
                lastch = ch;
            }
        }
        return sb.ToString();
    }

    private static string callSfExpressServiceByCSIM(string reqURL)
    {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(reqURL);
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";

        //Dictionary<string, string> content = new Dictionary<string, string>();
        //content["partnerID"] = partnerID;
        //content["requestID"] = requestID;
        //content["serviceCode"] = serviceCode;
        //content["timestamp"] = timestamp;
        //content["msgDigest"] = msgDigest;
        //content["msgData"] = msgData;

        //if (!(content == null || content.Count == 0))
        //{
        //    StringBuilder buffer = new StringBuilder();
        //    int i = 0;
        //    foreach (string key in content.Keys)
        //    {
        //        if (i > 0)
        //        {
        //            buffer.AppendFormat("&{0}={1}", key, content[key]);
        //        }
        //        else
        //        {
        //            buffer.AppendFormat("{0}={1}", key, content[key]);
        //        }
        //        i++;
        //    }

        //    byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
        //    req.ContentLength = data.Length;
        //    using (Stream reqStream = req.GetRequestStream())
        //    {
        //        reqStream.Write(data, 0, data.Length);
        //        reqStream.Close();
        //    }

        //}

        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        //获取响应内容
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        return result;
    }

    private static string callSfExpressServiceByCSIM(string reqURL, string partnerID, string requestID, string serviceCode, string timestamp, string msgDigest, string msgData)
    {
        string result = "";
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

    private static string UrlEncode(string str)
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

    private static string MD5ToBase64String(string str)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] MD5 = md5.ComputeHash(Encoding.UTF8.GetBytes(str));//MD5(注意UTF8编码)
        string result = Convert.ToBase64String(MD5);//Base64
        return result;
    }

    private static string Read(string path)
    {
        StreamReader sr = new StreamReader(path, Encoding.UTF8);

        StringBuilder builder = new StringBuilder();
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            builder.Append(line);
        }
        return builder.ToString();
    }

    private static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    
}
