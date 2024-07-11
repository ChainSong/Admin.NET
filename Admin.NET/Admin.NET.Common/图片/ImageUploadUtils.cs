// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Furion;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Common;

/// <summary>
/// 将导入的文件 写入到磁盘
/// </summary>
public static class ImageUploadUtils
{
    private const string FileDir = "/File/";
    /// <summary>
    /// 写入文件
    /// </summary>
    /// <param name="avatar"></param>
    /// <param name="reDir"></param>
    /// <returns></returns>
    public async static Task<ImageInfo> WriteFile(IFormFile avatar, string reDir = "Default")
    {
        ImageInfo imageInfo = new ImageInfo();
        string reName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
        string url = FileDir + reDir;
        string dir = GetDirPath(url);
        string path = $"{dir}\\{reName}";
        Stream stream = avatar.OpenReadStream();
        using (FileStream fileStream = new FileStream(path, FileMode.Create))
        {
            await avatar.CopyToAsync(fileStream);
        }

        imageInfo.Name = avatar.FileName;
        imageInfo.Path = dir;
        imageInfo.FileName = reName;
        imageInfo.Url = url;
        return imageInfo;//$"{reDir}/{reName}";
    }
    public static string GetDirPath(string reDir)
    {
        string dir = $"{App.WebHostEnvironment.WebRootPath}/{reDir}";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        return Path.GetFullPath(dir);
    }
}

