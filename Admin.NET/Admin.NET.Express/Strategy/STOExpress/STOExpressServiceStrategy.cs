// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core;
using Admin.NET.Express.Interface;
using Admin.NET.Express.Strategy.SFExpress;
using Admin.NET.Express.Strategy.STExpress.Dto;
using Admin.NET.Express.Strategy.STExpress.Dto.STRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace Admin.NET.Express.Strategy.STExpress;

/// <summary>
/// 申通
/// </summary>
public class STOExpressServiceStrategy : ISTOExpressServiceInterface
{
    public dynamic GetSTOExpressData(STOExpressInput<STOExpressNumberRequestInput> input)
    {
        string partnerId = input.PartnerId;//此处替换为您在丰桥平台获取的顾客编码       
        string checkword = input.Checkword;//此处替换为您在丰桥平台获取的校验码  
        string reqURL = input.Url;//生产环境
        string serviceCode = input.ServiceCode;//  "EXP_RECE_CREATE_ORDER";//下订单
        string timestamp = GetTimeStamp(); //获取时间戳      
        string requestID = Guid.NewGuid().ToString(); //获取uuid
        string msgData = input.Data.ToJson();
        string msgDigest = CalculateDigest(msgData,checkword);
        string respJson = HttpPost_STO(reqURL, msgData);
        return respJson;
    }

    #region 获得时间戳
    private static string GetTimeStamp()
    {
        TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }
    #endregion

    #region STO签名
    public static string CalculateDigest(string content, string secretKey)
    {
        string toSignContent = content + secretKey;
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.GetEncoding("utf-8").GetBytes(toSignContent);
        byte[] hash = md5.ComputeHash(inputBytes);
        return Convert.ToBase64String(hash);
    }
    #endregion

    #region 请求STO下单接口
    public static string HttpPost_STO(string Url,string body)
    {
        string resResult= string.Empty;
        Encoding encoding = System.Text.Encoding.GetEncoding("utf-8");
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.Accept = "text/html,application/xhtml+json,*/*";
            request.ContentType = "application/json";
            byte[] buffer= encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            Stream stream = request.GetRequestStream();
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader= new StreamReader(response.GetResponseStream(),Encoding.GetEncoding("utf-8")))
            {
                resResult = reader.ReadToEnd();
            }
            return resResult;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    #endregion

}
