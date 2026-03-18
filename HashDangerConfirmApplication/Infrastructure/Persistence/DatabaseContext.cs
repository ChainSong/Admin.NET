using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace TaskPlaApplication.Infrastructure.Persistence
{
    /// <summary>
    /// SqlSugar 统一入口：单例 SqlSugarScope + AOP（SQL/错误/审计）+ 简单事务
    /// </summary>
    public sealed class DatabaseContext
    {
        private readonly ILogger<DatabaseContext> _logger;
        private static bool _aopConfigured = false;

        /// <summary>线程安全的 SqlSugarScope；实现 ISqlSugarClient</summary>
        public SqlSugarScope Db { get; }

        public DatabaseContext(IConfiguration configuration, ILogger<DatabaseContext> logger)
        {
            _logger = logger;

            // 兼容两种常见命名：DefaultConnection / Default
            var connectionString =
      configuration.GetConnectionString("DefaultConnection")
      ?? configuration.GetConnectionString("Default")
      ?? configuration["ConnectionStrings:DefaultConnection"]
      ?? configuration["ConnectionStrings:Default"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // 打印可见的 ConnectionStrings 子键，方便排查
                var keys = configuration.GetSection("ConnectionStrings")
                                        .GetChildren()
                                        .Select(c => c.Key)
                                        .ToArray();
                var joined = keys.Length == 0 ? "(无子键)" : string.Join(", ", keys);
                throw new InvalidOperationException(
                    $"缺少连接串：ConnectionStrings:DefaultConnection / Default。已找到的 ConnectionStrings 子键: {joined}");
            }

            var dbTypeName = configuration["DatabaseSettings:DbType"]
                   ?? configuration["Database:DbType"]
                   ?? "SqlServer";

            Db = new SqlSugarScope(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = GetDbType(dbTypeName),
                InitKeyType = InitKeyType.Attribute,   // 用实体特性映射
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true
                }
            });

            if (!_aopConfigured)
            {
                ConfigureAop(Db);
                _aopConfigured = true;
            }
        }

        private void ConfigureAop(ISqlSugarClient db)
        {
            // SQL 日志
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                _logger.LogInformation("SQL: {Sql}\nPARAMS: {Params}",
                    sql, string.Join(", ", pars.Select(p => $"{p.ParameterName}={p.Value}")));
            };

            // SQL 错误
            db.Aop.OnError = ex =>
            {
                _logger.LogError(ex, "SqlSugar 执行异常：{Msg}", ex.Message);
            };

            // 自动审计：插入/更新时给 CreationTime / UpdationTime 赋值（区分大小写不敏感）
            db.Aop.DataExecuting = (oldValue, info) =>
            {
                if (info.OperationType == DataFilterType.InsertByObject)
                {
                    if (info.PropertyName.Equals("CreationTime", StringComparison.OrdinalIgnoreCase))
                        info.SetValue(DateTime.Now);

                    if (info.PropertyName.Equals("UpdationTime", StringComparison.OrdinalIgnoreCase))
                        info.SetValue(DateTime.Now);
                }
                else if (info.OperationType == DataFilterType.UpdateByObject)
                {
                    if (info.PropertyName.Equals("UpdationTime", StringComparison.OrdinalIgnoreCase))
                        info.SetValue(DateTime.Now);
                }
            };

            // ✅ 可选：全局过滤（多租户/软删等）
            // db.QueryFilter.AddTableFilter<WMSInstruction>(x => x.TenantId == CurrentTenantId);
        }

        private static DbType GetDbType(string dbTypeName) => dbTypeName.ToLower() switch
        {
            "mysql" => DbType.MySql,
            "postgresql" => DbType.PostgreSQL,
            "oracle" => DbType.Oracle,
            "sqlite" => DbType.Sqlite,
            _ => DbType.SqlServer
        };

        // —— 简单事务（如果没有单独的 UnitOfWork，可以直接调用这三个）
        public System.Threading.Tasks.Task BeginTranAsync() => Db.AsTenant().BeginTranAsync();
        public System.Threading.Tasks.Task CommitTranAsync() => Db.AsTenant().CommitTranAsync();
        public System.Threading.Tasks.Task RollbackTranAsync() => Db.AsTenant().RollbackTranAsync();
    }
}
