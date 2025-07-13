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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Admin.NET.Common;
public static class PhoneNumberValidator
{
    public static bool ValidatePhoneNumber(string input)
    {
        // 提取所有数字（保留可能的开头加号）
        string digits = new string(input.Where(char.IsDigit).ToArray());
        if (string.IsNullOrEmpty(digits))
            return false;

        // 检查并处理国际前缀（86）
        string[] candidates = { digits };
        if (digits.StartsWith("86") && digits.Length > 2)
        {
            candidates = new string[] { digits, digits.Substring(2) };
        }

        // 验证候选号码
        foreach (string candidate in candidates)
        {
            if (IsMobileNumber(candidate) || IsFixedLineNumber(candidate))
            {
                return true;
            }
        }
        return false;
    }

    // 验证手机号码（11位，1开头，第二位3-9）
    private static bool IsMobileNumber(string number)
    {
        return number.Length == 11 &&
               number[0] == '1' &&
               Regex.IsMatch(number, @"^1[3-9]\d{9}$");
    }

    // 验证固定电话号码（带区号）
    private static bool IsFixedLineNumber(string number)
    {
        // 基本规则：长度10-12位，以0开头
        if (number.Length < 10 || number.Length > 12 || number[0] != '0')
            return false;

        // 区号规则：3位（如010）或4位（如0755）
        int areaCodeLength = number[1] == '1' || number[1] == '2' ? 3 : 4;
        int numberLength = number.Length - areaCodeLength;

        // 本地号码长度需为7-8位
        return numberLength == 7 || numberLength == 8;
    }
}