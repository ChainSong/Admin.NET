using Microsoft.Extensions.Configuration;

namespace TaskPlaApplication.Utilities
{
    public class Logger
    {
        private readonly string _logBasePath;
        private readonly string _logFileNameFormat;
        private readonly string _encoding;

        public Logger()
        {
            // 获取当前项目根路径
            var baseDirectory = AppContext.BaseDirectory;
            // 设置日志路径为当前项目根目录下的 Logs 文件夹
            _logBasePath = Path.Combine(baseDirectory, "Logs");

            // 设置日志文件名格式
            _logFileNameFormat = "tasklog_{0:yyyyMMdd}.txt"; // 固定格式

            // 设置日志文件编码格式
            _encoding = "UTF-8"; // 固定编码

            // 确保日志目录存在
            EnsureDirectoryExists(_logBasePath);
        }

        public void Log(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}";
            Console.WriteLine(logMessage);
            WriteToFile(logMessage, "INFO");
        }

        public void LogError(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}";
            Console.WriteLine(logMessage);
            WriteToFile(logMessage, "ERROR");
        }

        public void LogWarning(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] WARN: {message}";
            Console.WriteLine(logMessage);
            WriteToFile(logMessage, "WARN");
        }

        public void LogSuccess(string message)
        {
            var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] SUCCESS: {message}";
            Console.WriteLine(logMessage);
            WriteToFile(logMessage, "SUCCESS");
        }

        private void WriteToFile(string message, string level)
        {
            try
            {
                var logFilePath = GetLogFilePath();
                var fullMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";

                File.AppendAllText(logFilePath, fullMessage, GetEncoding());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法写入日志文件: {ex.Message}");
            }
        }

        private string GetLogFilePath()
        {
            var fileName = string.Format(_logFileNameFormat, DateTime.Now);
            return Path.Combine(_logBasePath, fileName);
        }

        private System.Text.Encoding GetEncoding()
        {
            return _encoding.ToUpper() switch
            {
                "UTF-8" => System.Text.Encoding.UTF8,
                "UTF-8-BOM" => new System.Text.UTF8Encoding(true),
                "ASCII" => System.Text.Encoding.ASCII,
                "UNICODE" => System.Text.Encoding.Unicode,
                _ => System.Text.Encoding.UTF8
            };
        }

        private void EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine($"创建目录: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"创建目录失败: {path}, 错误: {ex.Message}");
            }
        }

        // 清理旧日志文件
        public void CleanOldLogs(int retentionDays)
        {
            try
            {
                if (!Directory.Exists(_logBasePath))
                    return;

                var cutoffDate = DateTime.Now.AddDays(-retentionDays);
                var logFiles = Directory.GetFiles(_logBasePath, "*.txt");

                foreach (var file in logFiles)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTime < cutoffDate)
                    {
                        fileInfo.Delete();
                        Console.WriteLine($"删除旧日志文件: {file}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"清理旧日志失败: {ex.Message}");
            }
        }
    }
}