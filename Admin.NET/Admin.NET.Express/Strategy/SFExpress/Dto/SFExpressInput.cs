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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Express.Strategy.SFExpress.Dto;
public class SFExpressInput<T>
{
    public string PartnerId { get; set; }//此处替换为您在丰桥平台获取的顾客编码       
    public string Checkword { get; set; }//此处替换为您在丰桥平台获取的校验码   
    public string Url { get; set; }//请求地址   
    public string UrlToken { get; set; }//Token 地址   
    public string Env { get; set; }//环境   
    public string ServiceCode { get; set; }//请求地址   

    public T Data { get; set; }//请求数据   

}



public class SFRootobjectPrint
{
    public string templateCode { get; set; }
    public string version { get; set; }
    public string fileType { get; set; }
    public string sync { get; set; }
    public List<Document> documents { get; set; }
}

public class Document
{
    public string masterWaybillNo { get; set; }
}

public class SFRootobject
{
    public List<SFCargodetail> cargoDetails { get; set; }
    public List<SFContactinfolist> contactInfoList { get; set; }
    public SFCustomsinfo customsInfo { get; set; }
    public int expressTypeId { get; set; }
    //public [] extraInfoList { get; set; }
    public int isOneselfPickup { get; set; }
    public string language { get; set; }
    public string monthlyCard { get; set; }
    public string orderId { get; set; }
    public int parcelQty { get; set; }
    public int payMethod { get; set; }
    public int totalWeight { get; set; }
}

public class SFCustomsinfo
{
}

public class SFCargodetail
{
    public float amount { get; set; }
    public float count { get; set; }
    public string name { get; set; }
    public string unit { get; set; }
    public float volume { get; set; }
    public float weight { get; set; }
}

public class SFContactinfolist
{
    public string address { get; set; }
    public string city { get; set; }
    public string company { get; set; }
    public string contact { get; set; }
    public int contactType { get; set; }
    public string county { get; set; }
    public string mobile { get; set; }
    public string province { get; set; }
    public string tel { get; set; }
}
