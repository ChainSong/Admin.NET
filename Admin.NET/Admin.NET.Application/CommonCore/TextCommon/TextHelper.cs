// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Admin.NET.Application.CommonCore.TextCommon;
public static class TextHelper
{
    private static string FileDir = "/File/Text";
    /// <summary>
    /// 保存成功之后返回地址
    /// </summary>
    /// <param name="html"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string Writtxt(string html, string file)
    {

        FileStream fileStream = new FileStream(Path.GetFullPath(Environment.CurrentDirectory + FileDir + "/" + file), FileMode.Append);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);
        streamWriter.Write(html + "\r\n");
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
        return Path.GetFullPath(Environment.CurrentDirectory + FileDir + "/" + file);

    }


    /// <summary>
    /// 读文件
    /// </summary>
    /// <param name="html"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public static string Readtxt(string file)
    {

        FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
        int n = (int)fs.Length;
        byte[] buffer = new byte[n];
        fs.Read(buffer, 0, n);
        fs.Close();
       return Encoding.UTF8.GetString(buffer, 0, n);
    }
}
