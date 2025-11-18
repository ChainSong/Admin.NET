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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Common.枚举值帮助类;
public class EnumHelper
{
    /// <summary>
    /// 根据枚举值获取枚举名称
    /// </summary>
    public static string GetEnumName<TEnum>(int value) where TEnum : struct, Enum
    {
        if (Enum.IsDefined(typeof(TEnum), value))
        {
            return ((TEnum)(object)value).ToString();
        }
        return value.ToString(); // 如果不存在，则返回原数字
    }

    /// <summary>
    /// 根据枚举值获取枚举的 Description 描述
    /// </summary>
    public static string GetEnumDescriotion<TEnum>(int Value) where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), Value))
        {
            return Value.ToString();
        }

        var enumValue=(Enum)(Object)Value;
        var memberInfo = typeof(TEnum).GetMember(enumValue.ToString());

        if (memberInfo.Length==0)
        {
            return enumValue.ToString();
        }

        var attrs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

        if (attrs.Length > 0)
            return ((DescriptionAttribute)attrs[0]).Description;

        // 没有 DescriptionAttribute 时返回枚举名称
        return enumValue.ToString();
    }
}
