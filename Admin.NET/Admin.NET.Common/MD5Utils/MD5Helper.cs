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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Common.MD5Common
{
    public static class MD5Helper
    {
        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CalcMD5(byte[] buffer)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(buffer);
                return BytesToString(md5Bytes);
            }
        }

        /// <summary>
        /// 将得到的 MD5 字节数组转成 字符串
        /// </summary>
        /// <param name="md5Bytes"></param>
        /// <returns></returns>
        private static string BytesToString(byte[] md5Bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算字符串的 MD5 值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CalcMD5(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return CalcMD5(buffer);
        }

        /// <summary>
        /// 计算流的 MD5 值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(stream);
                return BytesToString(buffer);
            }
        }
    }
}