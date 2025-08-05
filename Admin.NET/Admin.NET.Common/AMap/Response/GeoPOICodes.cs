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

namespace Admin.NET.Common.AMap.Response;
public class Suggestion
{
    [JsonPropertyName("keywords")]
    public List<string> Keywords { get; set; }

    [JsonPropertyName("cities")]
    public List<City> Cities { get; set; }
}
public class City
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("citycode")]
    public string Citycode { get; set; }

    [JsonPropertyName("adcode")]
    public string Adcode { get; set; }
}

public class Poi
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("typecode")]
    public string TypeCode { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("location")]
    public string Location { get; set; }

    [JsonPropertyName("tel")]
    public object Tel { get; set; }   // string or []

    [JsonPropertyName("distance")]
    public object Distance { get; set; } // string or []

    [JsonPropertyName("pname")]
    public string PName { get; set; }

    [JsonPropertyName("cityname")]
    public string CityName { get; set; }

    [JsonPropertyName("adname")]
    public string AdName { get; set; }

    [JsonPropertyName("importance")]
    public object Importance { get; set; }

    [JsonPropertyName("shopinfo")]
    public string ShopInfo { get; set; }

    [JsonPropertyName("parent")]
    public object Parent { get; set; }

    [JsonPropertyName("childtype")]
    public object ChildType { get; set; }

    [JsonPropertyName("shopid")]
    public object ShopId { get; set; }

    [JsonPropertyName("poiweight")]
    public object PoiWeight { get; set; }

    [JsonPropertyName("biz_type")]
    public object BizType { get; set; }

    [JsonPropertyName("photos")]
    public List<Photo> Photos { get; set; }

    [JsonPropertyName("biz_ext")]
    public BizExt BizExt { get; set; }
}

public class Photo
{
    [JsonPropertyName("title")]
    public object Title { get; set; } // sometimes string, sometimes []

    [JsonPropertyName("url")]
    public string Url { get; set; }
}

public class BizExt
{
    [JsonPropertyName("cost")]
    public object Cost { get; set; } // sometimes string, sometimes []

    [JsonPropertyName("rating")]
    public object Rating { get; set; } // sometimes string, sometimes []
}

