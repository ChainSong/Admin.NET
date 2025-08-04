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
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utf8Json.Internal;

namespace Admin.NET.Common.AMap.Response;
/// <summary>
/// 地理编码信息
/// </summary>
public class GeoCodes
{
    /// <summary>
    /// 国内地址默认返回中国 国家
    /// </summary>
    public string country {get; set;}
    /// <summary>
    /// 地址所在的省份名
    /// 中国的四大直辖市也算作省级单位。
    /// </summary>
    public string province { get; set; }
    /// <summary>
    /// 地址所在的城市名
    /// </summary>
    public string city { get; set; }
    /// <summary>
    /// 城市编码
    /// </summary>
    public string citycode { get; set; }
    /// <summary>
    /// 地址所在的区
    /// </summary>
    public string district { get; set; }
    /// <summary>
    /// 街道
    /// </summary>
    [JsonConverter(typeof(NumberConverter))]
    public object street { get; set; }

    /// <summary>
    /// 门牌
    /// </summary>
    [JsonConverter(typeof(NumberConverter))]
    public object number { get; set; }
    /// <summary>
    /// 区域编码
    /// </summary>
    public string adcode { get; set; }
    /// <summary>
    /// 坐标点
    /// </summary>
    public string location { get; set; }
    /// <summary>
    /// 匹配级别
    /// </summary>
    public string level { get; set; }
}


public class Pois
{
    public long id { get; set; }

    public string name {  get; set; }
}