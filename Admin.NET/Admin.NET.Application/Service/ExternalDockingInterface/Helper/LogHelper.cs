// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Furion.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.ExternalDockingInterface.Helper
{
    /// <summary>
    /// 日志帮助类：用于记录 HachWMS 各接口请求与处理日志
    /// </summary>
    public class LogHelper : ITransient
    {
        public enum LogLevel
        {
            Info,
            Debug,
            Success,
            Warn,
            Error
        }

        public enum LogMainType
        {
            身份鉴权,
            主档数据下发,
            主档BOM数据下发,
            入库订单下发,
            出库订单下发,
            其他
        }

        /// <summary>
        /// 写日志（支持主类型 + 日志等级 + 原始报文）
        /// </summary>
        public async Task LogAsync(
            LogMainType mainType,
            string batchId,
            string orderNo,
            LogLevel level,
            string message,
            bool success = true,
            string payload = null)
        {
            try
            {
                string logRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
                if (!Directory.Exists(logRoot))
                    Directory.CreateDirectory(logRoot);

                string subDir = Path.Combine(logRoot, mainType.ToString());
                if (!Directory.Exists(subDir))
                    Directory.CreateDirectory(subDir);

                string filePath = Path.Combine(subDir, $"{mainType}_{DateTime.Now:yyyy-MM-dd}.log");

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string icon = success ? "✅" : "❌";

                // 主体日志
                string line = $"{time} [{level}] [{mainType}] Batch:{batchId} | Order:{orderNo} | Result:{icon} | Msg:{message}";

                // 若附带报文，分隔打印
                if (!string.IsNullOrEmpty(payload))
                {
                    line += Environment.NewLine + "---- Payload ----" + Environment.NewLine + payload + Environment.NewLine + "-----------------" + Environment.NewLine;
                }

                await File.AppendAllTextAsync(filePath, line + Environment.NewLine);
            }
            catch
            {
                // 忽略日志写入错误
            }
        }
    }
}
