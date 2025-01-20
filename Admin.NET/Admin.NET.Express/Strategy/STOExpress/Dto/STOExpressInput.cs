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

namespace Admin.NET.Express.Strategy.STExpress.Dto;

public class STOExpressInput<T>
{
    public string PartnerId { get; set; }//此处替换为您在丰桥平台获取的顾客编码       
    public string Checkword { get; set; }//此处替换为您在丰桥平台获取的校验码   
    public string Url { get; set; }//请求地址   
    public string UrlToken { get; set; }//Token 地址   
    public string Env { get; set; }//环境   
    public string ServiceCode { get; set; }//请求地址   
    //public int IsSignBack { get; set; }//是否返回签回单路由标签： 默认0， 1：返回路由标签， 0：不返回   
    public T Data { get; set; }//请求数据   

}


