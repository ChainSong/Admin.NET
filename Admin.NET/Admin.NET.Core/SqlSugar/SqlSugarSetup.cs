// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Microsoft.Extensions.Primitives;
using NewLife.Reflection;
using OfficeOpenXml.FormulaParsing.Utilities;
using SqlSugar;

namespace Admin.NET.Core;

public static class SqlSugarSetup
{
    /// <summary>
    /// SqlSugar 上下文初始化
    /// </summary>
    /// <param name="services"></param>
    public static void AddSqlSugar(this IServiceCollection services)
    {
        // 自定义 SqlSugar 雪花ID算法
        SnowFlakeSingle.WorkId = App.GetOptions<SnowIdOptions>().WorkerId;
        StaticConfig.CustomSnowFlakeFunc = () =>
        {
            return YitIdHelper.NextId();
        };

        var dbOptions = App.GetOptions<DbConnectionOptions>();
        dbOptions.ConnectionConfigs.ForEach(SetDbConfig);

        SqlSugarScope sqlSugar = new(dbOptions.ConnectionConfigs.Adapt<List<ConnectionConfig>>(), db =>
        {
            dbOptions.ConnectionConfigs.ForEach(config =>
            {
                var dbProvider = db.GetConnectionScope(config.ConfigId);
                SetDbAop(dbProvider);
                SetDbDiffLog(dbProvider, config);
            });
        });

        services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
        services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
        services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册

        // 初始化数据库表结构及种子数据
        dbOptions.ConnectionConfigs.ForEach(config =>
        {
            InitDatabase(sqlSugar, config);
        });
    }

    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    public static void SetDbConfig(DbConnectionConfig config)
    {
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => // 处理表
            {
                entity.IsDisabledDelete = true; // 禁止删除非 sqlsugar 创建的列
                // 只处理贴了特性[SugarTable]表
                if (!type.GetCustomAttributes<SugarTable>().Any())
                    return;
                if (config.DbSettings.EnableUnderLine && !entity.DbTableName.Contains('_'))
                    entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            },
            EntityService = (type, column) => // 处理列 SCMSCMSCMSCMSCM
            {
                // 只处理贴了特性[SugarColumn]列
                if (!type.GetCustomAttributes<SugarColumn>().Any())
                    return;
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                    column.IsNullable = true;
                if (config.DbSettings.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                    column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                if (config.DbType == SqlSugar.DbType.Oracle)
                {
                    if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                        column.DataType = "number(18)";
                    if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                        column.DataType = "number(1)";
                }
            },
            DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true,
            IsAutoDeleteQueryFilter = true, // 启用删除查询过滤器
            IsAutoUpdateQueryFilter = true, // 启用更新查询过滤器
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
    }

    /// <summary>
    /// 配置Aop
    /// </summary>
    /// <param name="db"></param>
    public static void SetDbAop(SqlSugarScopeProvider db)
    {
        var config = db.CurrentConnectionConfig;

        // 设置超时时间
        db.Ado.CommandTimeOut = 30;

        // 打印SQL语句
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            var originColor = Console.ForegroundColor;
            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Green;
            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Red;
            if (sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("【" + DateTime.Now + "——执行SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, sql, pars) + "\r\n");
            Console.ForegroundColor = originColor;
            App.PrintToMiniProfiler("SqlSugar", "Info", sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
        };
        db.Aop.OnError = ex =>
        {
            if (ex.Parametres == null) return;
            var originColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            var pars = db.Utilities.SerializeObject(((SugarParameter[])ex.Parametres).ToDictionary(it => it.ParameterName, it => it.Value));
            Console.WriteLine("【" + DateTime.Now + "——错误SQL】\r\n" + UtilMethods.GetSqlString(config.DbType, ex.Sql, (SugarParameter[])ex.Parametres) + "\r\n");
            Console.ForegroundColor = originColor;
            App.PrintToMiniProfiler("SqlSugar", "Error", $"{ex.Message}{Environment.NewLine}{ex.Sql}{pars}{Environment.NewLine}");
        };
        db.Aop.OnLogExecuted = (sql, pars) =>
        {
            // 执行时间超过5秒
            if (db.Ado.SqlExecutionTime.TotalSeconds > 5)
            {
                var fileName = db.Ado.SqlStackTrace.FirstFileName; // 文件名
                var fileLine = db.Ado.SqlStackTrace.FirstLine; // 行号
                var firstMethodName = db.Ado.SqlStackTrace.FirstMethodName; // 方法名
                var log = $"【所在文件名】：{fileName}\r\n【代码行数】：{fileLine}\r\n【方法名】：{firstMethodName}\r\n" + $"【sql语句】：{UtilMethods.GetSqlString(config.DbType, sql, pars)}";
                var originColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(log);
                Console.ForegroundColor = originColor;
            }
            //if (pars.Length > 0)
            //{
            //    foreach (var item in pars)
            //    {
            //        if (item.ParameterName == "@WarehouseId")
            //        {
            //            //多表Queryable查询
            //            db.QueryFilter.Add(new SqlFilterItem()
            //            {
            //                FilterValue = it =>
            //                {
            //                    //Writable logic
            //                    return new SqlFilterResult() { Sql = " WarehouseId =5 " };
            //                },
            //                //IsJoinQuery = false //多表生效
            //            });
            //        }
            //    }

            //}

        };
        // 数据审计
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {
            // 演示环境判断
            if (entityInfo.EntityColumnInfo.IsPrimarykey)
            {
                if (entityInfo.EntityName != nameof(SysJobDetail) && entityInfo.EntityName != nameof(SysJobTrigger) &&
                    entityInfo.EntityName != nameof(SysLogOp) && entityInfo.EntityName != nameof(SysLogVis) &&
                    entityInfo.EntityName != nameof(SysOnlineUser))
                {
                    var isDemoEnv = App.GetService<SysConfigService>().GetConfigValue<bool>(CommonConst.SysDemoEnv).GetAwaiter().GetResult();
                    if (isDemoEnv)
                        throw Oops.Oh(ErrorCodeEnum.D1200);
                }
            }

            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                // 主键(long类型)且没有值的---赋值雪花Id
                if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                {
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (long)id == 0)
                        entityInfo.SetValue(YitIdHelper.NextId());
                }
                if (entityInfo.PropertyName == "CreateTime")
                    entityInfo.SetValue(DateTime.Now);
                if (App.User != null)
                {
                    if (entityInfo.PropertyName == "TenantId")
                    {
                        var tenantId = ((dynamic)entityInfo.EntityValue).TenantId;
                        if (tenantId == null || tenantId == 0)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.TenantId)?.Value);
                    }
                    if (entityInfo.PropertyName == "CreateUserId")
                    {
                        var createUserId = ((dynamic)entityInfo.EntityValue).CreateUserId;
                        if (createUserId == 0 || createUserId == null)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.UserId)?.Value);
                    }

                    if (entityInfo.PropertyName == "CreateOrgId")
                    {
                        var createOrgId = ((dynamic)entityInfo.EntityValue).CreateOrgId;
                        if (createOrgId == 0 || createOrgId == null)
                            entityInfo.SetValue(App.User.FindFirst(ClaimConst.OrgId)?.Value);
                    }
                    //Console.WriteLine(entityInfo.PropertyName);
                    //if (entityInfo.PropertyName == "WarehouseId")
                    //{
                    //    var tenantId = ((dynamic)entityInfo.EntityValue).WarehouseId;
                    //    if (tenantId == null || tenantId == 0)
                    //        entityInfo.SetValue(App.User.FindFirst(ClaimConst.TenantId)?.Value);
                    //}
                }
                //if (entityInfo.PropertyName == "CustomerId")
                //{
                //    db.Ado.SelectAll()
                //    entityInfo.SetValue(DateTime.Now);
                //}

            }
            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                if (entityInfo.PropertyName == "UpdateTime")
                    entityInfo.SetValue(DateTime.Now);
                if (entityInfo.PropertyName == "UpdateUserId")
                    entityInfo.SetValue(App.User?.FindFirst(ClaimConst.UserId)?.Value);
            }
          
        };

        // 配置用户操作客户仓库（数据范围）过滤器
        //SqlSugarFilter.SetCustomerWarehouseEntityFilter(db);

        // 配置租户过滤器
        //var tenantId = App.User?.FindFirst(ClaimConst.UserId)?.Value;
        //if (!string.IsNullOrWhiteSpace(tenantId))
        //    db.QueryFilter.AddTableFilter<ITenantIdFilter>(u => u.WarehouseId == long.Parse(tenantId));


        // 超管时排除各种过滤器
        if (App.User?.FindFirst(ClaimConst.AccountType)?.Value == ((int)AccountTypeEnum.SuperAdmin).ToString())
            return;


        //db.Deleteable<WMSOrder>().Where(it => it.Id == 0).ExecuteCommand();




        // 配置实体假删除过滤器
        db.QueryFilter.AddTableFilter<IDeletedFilter>(u => u.IsDelete == false);
        // 配置租户过滤器
        var tenantId = App.User?.FindFirst(ClaimConst.TenantId)?.Value;
        if (!string.IsNullOrWhiteSpace(tenantId))
            db.QueryFilter.AddTableFilter<ITenantIdFilter>(u => u.TenantId == long.Parse(tenantId));


        // 配置用户机构（数据范围）过滤器
        SqlSugarFilter.SetOrgEntityFilter(db);
        // 配置自定义过滤器
        SqlSugarFilter.SetCustomEntityFilter(db);
    }

    /// <summary>
    /// 开启库表差异化日志
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void SetDbDiffLog(SqlSugarScopeProvider db, DbConnectionConfig config)
    {
        if (!config.DbSettings.EnableDiffLog) return;

        db.Aop.OnDiffLogEvent = async u =>
        {
            var logDiff = new SysLogDiff
            {
                // 操作后记录（字段描述、列名、值、表名、表描述）
                AfterData = JSON.Serialize(u.AfterData),
                // 操作前记录（字段描述、列名、值、表名、表描述）
                BeforeData = JSON.Serialize(u.BeforeData),
                // 传进来的对象
                BusinessData = JSON.Serialize(u.BusinessData),
                // 枚举（insert、update、delete）
                DiffType = u.DiffType.ToString(),
                Sql = UtilMethods.GetSqlString(config.DbType, u.Sql, u.Parameters),
                Parameters = JSON.Serialize(u.Parameters),
                Elapsed = u.Time == null ? 0 : (long)u.Time.Value.TotalMilliseconds
            };
            await db.Insertable(logDiff).ExecuteCommandAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(DateTime.Now + $"\r\n*****差异日志开始*****\r\n{Environment.NewLine}{JSON.Serialize(logDiff)}{Environment.NewLine}*****差异日志结束*****\r\n");
        };
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="db"></param>
    /// <param name="config"></param>
    private static void InitDatabase(SqlSugarScope db, DbConnectionConfig config)
    {
        SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);

        // 初始化/创建数据库
        if (config.DbSettings.EnableInitDb)
        {
            if (config.DbType != SqlSugar.DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();
        }

        // 初始化表结构
        if (config.TableSettings.EnableInitTable)
        {
            var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false))
                .WhereIF(config.TableSettings.EnableIncreTable, u => u.IsDefined(typeof(IncreTableAttribute), false)).ToList();
            if (entityTypes.Any())
            {
                foreach (var entityType in entityTypes)
                {
                    try
                    {
                        var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                        if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
                        if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                        if (entityType.GetCustomAttribute<SplitTableAttribute>() == null)
                            dbProvider.CodeFirst.InitTables(entityType);
                        else
                            dbProvider.CodeFirst.SplitTables().InitTables(entityType);
                    }
                    catch (Exception ex)
                    {

                        //throw;
                    }
                  
                }
            }
        }

        // 初始化种子数据
        if (config.SeedSettings.EnableInitSeed)
        {
            var seedDataTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>))))
                .WhereIF(config.SeedSettings.EnableIncreSeed, u => u.IsDefined(typeof(IncreSeedAttribute), false)).ToList();
            if (seedDataTypes.Any())
            {
                foreach (var seedType in seedDataTypes)
                {
                    var instance = Activator.CreateInstance(seedType);

                    var hasDataMethod = seedType.GetMethod("HasData");
                    var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
                    if (seedData == null) continue;

                    var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                    var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                    if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
                    if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                    var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                    if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                    {
                        // 按主键进行批量增加和更新
                        var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                        storage.AsInsertable.ExecuteCommand();
                        storage.AsUpdateable.ExecuteCommand();
                    }
                    else
                    {
                        // 无主键则只进行插入
                        if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                            dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                    }
                }
            }
        }
    }

    /// <summary>
    /// 初始化租户业务数据库
    /// </summary>
    /// <param name="iTenant"></param>
    /// <param name="config"></param>
    public static void InitTenantDatabase(ITenant iTenant, DbConnectionConfig config)
    {
        SetDbConfig(config);

        iTenant.AddConnection(config);
        SqlSugarScopeProvider db = iTenant.GetConnectionScope(config.ConfigId);
        db.DbMaintenance.CreateDatabase();


        // 获取所有实体表-初始化租户业务表
        var entityTypes = App.EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
            && u.IsDefined(typeof(SugarTable), false) && !u.IsDefined(typeof(SystemTableAttribute), false)).ToList();
        if (!entityTypes.Any()) return;
        foreach (var entityType in entityTypes)
        {
            try
            {
                if (entityType.Name.Contains("Hach"))
                {
                    continue;
                }

                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                Console.WriteLine(entityType.Name);
                if (splitTable == null)
                    db.CodeFirst.InitTables(entityType);
                else
                    db.CodeFirst.SplitTables().InitTables(entityType);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        //创建完成租户数据库,将原有的种子数据,移植过去;
        var tableColumnss = iTenant.QueryableWithAttr<TableColumns>().Where(a => a.TenantId == 1300000000001).ToList();
        tableColumnss.ForEach(a =>
        {
            string ConfigId = config.ConfigId;
            a.TenantId = Convert.ToInt64(ConfigId);
            a.Id = 0;
        });
        //db.Insertable<TableColumns>(tableColumnss);

        var storage = db.StorageableByObject(tableColumnss).ToStorage();
        storage.AsInsertable.ExecuteCommand();

        ////创建完成租户数据库,将原有的种子数据,移植过去;
        var tableColumnsDetails = iTenant.QueryableWithAttr<TableColumnsDetail>().Where(a => a.TenantId == 1300000000001).ToList();
        tableColumnsDetails.ForEach(a =>
        {
            string ConfigId = config.ConfigId;
            a.TenantId = Convert.ToInt64(ConfigId);
            a.Id = 0;

        });
        var storageDetail = db.StorageableByObject(tableColumnsDetails).ToStorage();
        //storageDetail..ExecuteCommand();
        storageDetail.AsInsertable.ExecuteCommand();
        StringBuilder procSql = new StringBuilder();
        //创建分配存储过程
        procSql.Append(@"Create PROC [dbo].[Proc_WMS_AutomatedOutbound]  
(
@InstructionTaskNo varchar(50)
)
AS 
  BEGIN 
      SET nocount ON;
SET xact_abort ON; --出库主表     

DECLARE @Flag INT;
----明细临时表   
DECLARE @tempAddPerOrderId TABLE
  (
     poid        BIGINT,
     ordernumber [NVARCHAR](50)
  );
----操作完成运单信息   
 
DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(100),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );
DECLARE @POID BIGINT;
DECLARE @tempOrderNumber VARCHAR(50);
DECLARE @ExternOrderNumber VARCHAR(50);
DECLARE @PreOrderNumber VARCHAR(50);
DECLARE @CustomerId VARCHAR(50);
DECLARE @WarehouseId VARCHAR(50);
DECLARE @CId BIGINT;
--DECLARE @WarehouseName VARCHAR(50);
DECLARE @OrderType VARCHAR(50);
DECLARE @LineNumber VARCHAR(50);
DECLARE @SKU VARCHAR(50);
DECLARE @UPC VARCHAR(50);
DECLARE @GoodsName VARCHAR(50);
DECLARE @Goodstype VARCHAR(50);
DECLARE @OrderQty DECIMAL(18, 2);
DECLARE @AllocatedQty DECIMAL(18, 2);
DECLARE @ODId BIGINT;
DECLARE @OId BIGINT;
DECLARE @BatchCode VARCHAR(100);
DECLARE @BoxCode VARCHAR(100);
DECLARE @UnitCode VARCHAR(100);
DECLARE @Onwer VARCHAR(100);
DECLARE @TrayCode VARCHAR(100);
DECLARE @AllocationBatch VARCHAR(100);
DECLARE @PoCode VARCHAR(100);


DECLARE @ISKU VARCHAR(50);
DECLARE @IUPC VARCHAR(50);
DECLARE @IGoodsName VARCHAR(50);
DECLARE @IGoodstype VARCHAR(50);
DECLARE @ICustomerId VARCHAR(50);
DECLARE @IWarehouseId VARCHAR(50); 
DECLARE @IQty DECIMAL(18, 2);
DECLARE @IExternOrderNumber VARCHAR(50);
DECLARE @IArea VARCHAR(50);
DECLARE @ILocation VARCHAR(50);
DECLARE @IBatchCode VARCHAR(100);
DECLARE @IBoxCode VARCHAR(100); 
DECLARE @IUnitCode VARCHAR(100);
DECLARE @IOnwer VARCHAR(100);
DECLARE @ITrayCode VARCHAR(100);


DECLARE @Cursorid BIGINT;
DECLARE @IId BIGINT;
DECLARE @NewIId BIGINT;
set @AllocationBatch=NEWID();
------------全局变量 保存分配后剩余的数量---------   
DECLARE @NumberRemaining DECIMAL(18, 2);
------------------声明获取DetailID----------------   
DECLARE @IIdTemp BIGINT;

IF ( 1 = 1 )
  BEGIN
      DECLARE cursor_externordernumber CURSOR FOR
        SELECT id
        FROM   wms_order
        WHERE  orderstatus =5
               AND id IN (SELECT operationid
                          FROM   wms_instruction
                          WHERE  instructiontaskno = @InstructionTaskNo
                                 AND instructionstatus = 1)
        ORDER  BY id

      OPEN cursor_externordernumber;

      FETCH cursor_externordernumber INTO @Cursorid;

      WHILE ( @@FETCH_STATUS = 0 )
        BEGIN
            ---------------------------------------------判断条件满不满足  能不能全部分配--------------------------------------   
            SELECT @Flag = Count(*)
            FROM   (SELECT Max(od.externordernumber) AS ExternOrderNumber,
                           Max (od.customername)     AS CustomerName,
                           ( od.warehousename )      WarehouseName,
                           ( od.sku )                SKU,
                           ( od.upc )                UPC,
                           ( od.goodstype )          GoodsType,
                           Max (od.orderqty)         AS OrderQty,
                           Max (od.allocatedqty)     AS AllocatedQty,
                           ( od.batchcode )          AS BatchCode,
                           ( od.boxcode )            AS BoxCode,
                           ( od.unitcode )           AS UnitCode,
                           ( od.PoCode )           AS PoCode
                    FROM   [wms_orderdetail] od
                    WHERE  od.orderid = @Cursorid
                           
                    GROUP  BY od.sku,
                              od.upc,
                              od.warehousename,
                              od.goodstype,
                              od.batchcode,
                              od.boxcode,
                              od.PoCode,
                              od.unitcode) a
                   OUTER apply (SELECT Sum(qty) Qty
                                FROM   wms_inventory_usable i
                                WHERE  i.sku = a.sku
                                       AND i.goodstype = a.goodstype
                                       AND i.customername = a.customername
                                       AND i.warehousename = a.warehousename
									    AND i.Location   in (select Location from WMS_Location where LocationStatus=1 and  WarehouseId=i.WarehouseId and Area=i.Area and Location=i.Location)
									     AND( i.UPC = a.UPC 
                                             OR  a.UPC='' ) 
                                      AND( i.batchcode = a.batchcode 
                                             OR  a.batchcode='' ) 
											   AND( i.PoCode = a.PoCode 
                                             OR  a.PoCode='' ) 
                                     AND( i.boxcode = a.boxcode 
                                             OR  a.boxcode='' ) 
                                        AND( i.unitcode = a.unitcode 
                                             OR  a.unitcode='' )  
                                       AND i.inventorystatus = 1
                                       AND i.qty > 0
                                GROUP  BY i.sku,
                                          i.goodstype,
                                          i.warehousename) Q
            WHERE  Q.qty >= ( a.orderqty - a.allocatedqty )
                   AND a.orderqty > 0;

            IF ( @Flag > 0
                 AND @Flag >= (SELECT Count(*)
                               FROM   (SELECT Count(*) a
                                       FROM   [wms_orderdetail] od
                                       WHERE  orderid = @Cursorid
                                       GROUP  BY od.sku,
                                                 od.warehousename,
                                                 od.goodstype,
                                                 od.boxcode,
                                                 od.PoCode,
                                                 od.upc,
                                                 od.boxcode,
                                                 od.unitcode) b) )
              BEGIN
                  -----------------------------------------------------------事物开始---------------------------------------------------   
                  BEGIN try
                      BEGIN TRANSACTION

                      ------------------------------------------------------得到Detail----------------------------------------------------   
                      DECLARE cursor_order CURSOR FOR
                        SELECT od.id,
                               od.orderid,
                               od.externordernumber,
                               od.customerid,
                               od.WarehouseId,
                               od.sku,
                               od.upc,
                               od.goodstype,
                               od.orderqty,
                               od.allocatedqty,
                               od.batchcode,
                               od.boxcode,
                               od.unitcode,
                               od.Onwer,
                               od.TrayCode,
                               od.PoCode
                        FROM   [wms_orderdetail] od
                        WHERE  od.orderqty > od.allocatedqty
                               AND od.orderid = @Cursorid
                        ORDER  BY od.id
                      OPEN cursor_order
                      FETCH cursor_order INTO @ODId, @OId, @ExternOrderNumber,
                      @CustomerId,
                      @Warehouseid, @SKU, @UPC, --@GoodsName,   
                      @Goodstype, @OrderQty, @AllocatedQty, @BatchCode, @BoxCode,
                      @UnitCode,@Onwer,@TrayCode,@PoCode;

                      WHILE ( @@FETCH_STATUS = 0 )
                        BEGIN
                            SET @NumberRemaining = @OrderQty - @AllocatedQty;

                            ------------------------------------------------------得到Inventory----------------------------------------------------   
                            DECLARE cursor_inventory CURSOR FOR
                              SELECT i.id,
                                     i.customerid,
                                     i.WarehouseId,
                                     i.area,
                                     i.location,
                                     i.sku,
                                     i.upc,
                                     i.goodstype,
                                     i.qty,
                                     i.batchcode,
                                     i.boxcode,
                                     i.unitcode,
									 i.Onwer,
									 i.TrayCode
                              FROM   [dbo].wms_inventory_usable i
                              WHERE  i.sku = @SKU
                                     AND i.goodstype = @GoodsType
                                     AND i.customerid = @CustomerId
                                     AND i.WarehouseId = @WarehouseId
                                     AND i.inventorystatus = 1
									  AND i.Location   in (select Location from WMS_Location where LocationStatus=1 and  WarehouseId=i.WarehouseId and Area=i.Area and Location=i.Location)
                                     AND i.qty > 0
									    AND( i.UPC = @UPC 
                                             OR  @UPC='' ) 
                                     --AND i.upc = @UPC
									  AND( i.BatchCode = @BatchCode
                                             OR  @BatchCode='' ) 
                                     --AND i.batchcode = @BatchCode
									   AND( i.boxcode = @boxcode
                                             OR  @boxcode='' ) 
                                     --AND i.boxcode = @BoxCode
									   AND( i.unitcode = @unitcode
                                             OR  @unitcode='' ) 
                                     --AND i.unitcode = @UnitCode
									  AND( i.Onwer = @Onwer
                                             OR  @Onwer='' ) 
                                     --AND i.Onwer = @Onwer
									  AND( i.TrayCode = @TrayCode
                                             OR  @TrayCode='' ) 
											   AND( i.PoCode = @PoCode
                                             OR  @PoCode='' ) 
                                     --AND i.TrayCode = @TrayCode
                              ORDER  BY i.inventorytime
                            OPEN cursor_inventory
                            FETCH cursor_inventory INTO @IId, @ICustomerId,
                            @IWarehouseId,
                            @IArea,
                            @ILocation, @ISKU, @IUPC, @IGoodsType, @IQty,
                            @IBatchCode,
                            @IBoxCode,
                            @IUnitCode,@IOnwer,@ITrayCode

                            WHILE ( @@FETCH_STATUS = 0 )
                              BEGIN
                                  IF( @IQty >= @NumberRemaining )
                                    BEGIN
                                        --------------------------------------------------插入[WMS_OrderDetail]--------------------------------------------------   
                                       
									    INSERT INTO wms_inventory_usable
                                                    (
                                                     [receiptdetailid],
                                                     [receiptreceivingid],
                                                     [customerid],
                                                     [customername],
                                                     [warehouseid],
                                                     [warehousename],
                                                     [area],
                                                     [location],
                                                     [sku],
                                                     [upc],
                                                     [goodstype],
                                                     [inventorystatus],
                                                     [superid],
                                                     [relatedid],
                                                     [goodsname],
                                                     [unitcode],
                                                     [onwer],
                                                     [boxcode],
                                                     [traycode],
                                                     [batchcode],
													 [LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                     [qty],
                                                     [productiondate],
                                                     [expirationdate],
                                                     [remark],
                                                     [inventorytime],
                                                     [creator],
                                                     [creationtime],
                                                     [updator],
                                                     [updatetime],
                                                     [str1],
                                                     [str2],
                                                     [str3],
                                                     [str4],
                                                     [str5],
                                                     [datetime1],
                                                     [datetime2],
                                                     [int1],
                                                     [int2],[TenantId])
                                        (SELECT  
                                                [receiptdetailid],
                                                [receiptreceivingid],
                                                [customerid],
                                                [customername],
                                                [warehouseid],
                                                [warehousename],
                                                [area],
                                                [location],
                                                [sku],
                                                [upc],
                                                [goodstype],
                                                10,
                                                [superid],
                                                [relatedid],
                                                [goodsname],
                                                [unitcode],
                                                [onwer],
                                                [boxcode],
                                                [traycode],
                                                [batchcode],
												[LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                @NumberRemaining,
                                                [productiondate],
                                                [expirationdate],
                                                [remark],
                                                [inventorytime],
                                                [creator],
                                                getdate(),
                                                [updator],
                                                [updatetime],
                                                [str1],
                                                [str2],
                                                [str3],
                                                [str4],
                                                [str5],
                                                [datetime1],
                                                [datetime2],
                                                [int1],
                                                [int2],[TenantId]
                                         FROM   wms_inventory_usable
                                         WHERE  id = @IId);
									    SET @NewIId= @@IDENTITY
									   INSERT INTO [dbo].wms_orderallocation
                                                    (
													 AllocationBatch,
													 AllocationStatus,
													 [inventoryid],
                                                     [orderid],
                                                     [orderdetailid],
                                                     [customerid],
                                                     [customername],
                                                     [warehouseid],
                                                     [warehousename],
                                                     [area],
                                                     [location],
                                                     [sku],
                                                     [upc],
                                                     [goodstype],
                                                     [inventorystatus],
                                                     [superid],
                                                     [relatedid],
                                                     [goodsname],
                                                     [unitcode],
                                                     [onwer],
                                                     [boxcode],
                                                     [traycode],
                                                     [batchcode],
													 [LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                     [qty],
                                                     [productiondate],
                                                     [expirationdate],
                                                     [remark],
                                                     [creator],
                                                     [creationtime],
                                                     [str1],
                                                     [str2],
                                                     [str3],
                                                     [str4],
                                                     [str5],
                                                     [datetime1],
                                                     [datetime2],
                                                     [int1],
                                                     [int2],[TenantId])
                                        (SELECT @AllocationBatch,1,i.id,
                                                @OId,
                                                @ODId,
                                                [customerid],
                                                [customername],
                                                [warehouseid],
                                                [warehousename],
                                                [area],
                                                [location],
                                                [sku],
                                                [upc],
                                                [goodstype],
                                                1,
                                                [superid],
                                                [relatedid],
                                                [goodsname],
                                                [unitcode],
                                                [onwer],
                                                [boxcode],
                                                [traycode],
                                                [batchcode],
												[LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                @NumberRemaining,
                                                [productiondate],
                                                [expirationdate],
                                                [remark],
                                                '创建人',
                                                Getdate(),
                                                [str1],
                                                [str2],
                                                [str3],
                                                [str4],
                                                [str5],
                                                [datetime1],
                                                [datetime2],
                                                [int1],
                                                [int2],[TenantId]
                                         FROM   wms_inventory_usable i where Id=@NewIId);

                                       

                                        UPDATE wms_inventory_usable
                                        SET    qty = ( qty - @NumberRemaining ),
                                               inventorystatus = ( CASE
                                               WHEN qty = @NumberRemaining
                                                                   THEN
                                                                     99
                                                                     ELSE 1
                                                                   END )
                                        WHERE  id = @IId

                                       

                                        --SET @IIdTemp= @@IDENTITY

                                        --UPDATE WMS_OrderAllocation   
                                        --SET     = @IIdTemp   
                                        --WHERE  id = @ODId   
                                        UPDATE WMS_OrderDetail   
                                        SET    allocatedqty+=  @NumberRemaining   
                                        WHERE  id = @ODId   
                                        UPDATE wms_order   
                                        SET    OrderStatus = 20   
                                        WHERE  id = @Cursorid;   
                                        --SELECT  @Message += ExternOrderNumber   
                                        --FROM    [dbo].[WMS_PreOrder]   
                                        --WHERE   ID = @Cursorid;   
                                        --SET @Message += '分配完成.';   
                                        BREAK;
                                    END;
                                  ELSE
                                    BEGIN
                                        --SET @NumberRemaining =@NumberRemaining-@IQty;   
                                        INSERT INTO [dbo].wms_orderallocation
                                                    (AllocationBatch,
													 AllocationStatus,
                                                     [inventoryid],
                                                     [orderid],
                                                     [orderdetailid],
                                                     [customerid],
                                                     [customername],
                                                     [warehouseid],
                                                     [warehousename],
                                                     [area],
                                                     [location],
                                                     [sku],
                                                     [upc],
                                                     [goodstype],
                                                     [inventorystatus],
                                                     [superid],
                                                     [relatedid],
                                                     [goodsname],
                                                     [unitcode],
                                                     [onwer],
                                                     [boxcode],

                                                     [traycode],
                                                     [batchcode],
													 [LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                     [qty],
                                                     [productiondate],
                                                     [expirationdate],
                                                     [remark],
                                                     [creator],
                                                     [creationtime],
                                                     [str1],
                                                     [str2],
                                                     [str3],
                                                     [str4],
                                                     [str5],
                                                     [datetime1],
                                                     [datetime2],
                                                     [int1],
                                                     [int2],[TenantId])
                                        (SELECT @AllocationBatch,1,
                                                @IId,
                                                @OId,
                                                @ODId,
                                                [customerid],
                                                [customername],
                                                [warehouseid],
                                                [warehousename],
                                                [area],
                                                [location],
                                                [sku],
                                                [upc],
                                                [goodstype],
                                                1,
                                                [superid],
                                                [relatedid],
                                                [goodsname],
                                                [unitcode],
                                                [onwer],
                                                [boxcode],
                                                [traycode],
                                                [batchcode],
												[LotCode],
                                                     [PoCode] ,
                                                     [Weight] ,
                                                     [Volume] ,
                                                @IQty,
                                                [productiondate],
                                                [expirationdate],
                                                [remark],
                                                '创建人',
                                                Getdate(),
                                                [str1],
                                                [str2],
                                                [str3],
                                                [str4],
                                                [str5],
                                                [datetime1],
                                                [datetime2],
                                                [int1],
                                                [int2],[TenantId]
                                         FROM   wms_inventory_usable i where Id=@IId);

                                        --SET @ODId= @@IDENTITY

                                        UPDATE wms_inventory_usable
                                        SET   InventoryStatus=10 
                                        WHERE  id = @IId

                                        --INSERT INTO wms_inventory_usable
                                        --            ( 
                                        --             [receiptdetailid],
                                        --             [receiptreceivingid],
                                        --             [customerid],
                                        --             [customername],
                                        --             [warehouseid],
                                        --             [warehousename],
                                        --             [area],
                                        --             [location],
                                        --             [sku],
                                        --             [upc],
                                        --             [goodstype],
                                        --             [inventorystatus],
                                        --             [superid],
                                        --             [relatedid],
                                        --             [goodsname],
                                        --             [unitcode],
                                        --             [onwer],
                                        --             [boxcode],
                                        --             [traycode],
                                        --             [batchcode],
                                        --             [qty],
                                        --             [productiondate],
                                        --             [expirationdate],
                                        --             [remark],
                                        --             [inventorytime],
                                        --             [creator],
                                        --             [creationtime],
                                        --             [updator],
                                        --             [updatetime],
                                        --             [str1],
                                        --             [str2],
                                        --             [str3],
                                        --             [str4],
                                        --             [str5],
                                        --             [datetime1],
                                        --             [datetime2],
                                        --             [int1],
                                        --             [int2])
                                        --(SELECT  
                                        --        [receiptdetailid],
                                        --        [receiptreceivingid],
                                        --        [customerid],
                                        --        [customername],
                                        --        [warehouseid],
                                        --        [warehousename],
                                        --        [area],
                                        --        [location],
                                        --        [sku],
                                        --        [upc],
                                        --        [goodstype],
                                        --        10,
                                        --        [superid],
                                        --        [relatedid],
                                        --        [goodsname],
                                        --        [unitcode],
                                        --        [onwer],
                                        --        [boxcode],
                                        --        [traycode],
                                        --        [batchcode],
                                        --        @NumberRemaining,
                                        --        [productiondate],
                                        --        [expirationdate],
                                        --        [remark],
                                        --        [inventorytime],
                                        --        [creator],
                                        --        [creationtime],
                                        --        [updator],
                                        --        [updatetime],
                                        --        [str1],
                                        --        [str2],
                                        --        [str3],
                                        --        [str4],
                                        --        [str5],
                                        --        [datetime1],
                                        --        [datetime2],
                                        --        [int1],
                                        --        [int2]
                                        -- FROM   wms_inventory_usable i);

                                        --SET @IIdTemp= @@IDENTITY
										 UPDATE WMS_OrderDetail   
                                        SET    allocatedqty+=@IQty   
                                        WHERE  id = @ODId   
                                        UPDATE wms_order   
                                        SET    OrderStatus = 20   
                                        WHERE  id = @Cursorid;   
                                        --UPDATE wms_orderallocation
                                        --SET    inventoryid = @IIdTemp
                                        --WHERE  id = @ODId

                                        SET @NumberRemaining =
                                        @NumberRemaining - @IQty
                                        ;
                                    END;

                                  -----------------------------------------------回收Cursor_Inventory-------------------------------------------   
                                  FETCH cursor_inventory INTO @IId, @ICustomerId ,
                                  @IWarehouseid,
                                  @IArea,
                                  @ILocation, @ISKU, @IUPC, @IGoodsType, @IQty,
                                  @IBatchCode,
                                  @IBoxCode,
                                  @IUnitCode,@IOnwer,@ITrayCode;
                              END

                            CLOSE cursor_inventory;

                            DEALLOCATE cursor_inventory;

                            -----------------------------------------------回收cursor_order---------------------------------------------   
                            FETCH cursor_order INTO @ODId, @OId,
                            @ExternOrderNumber,
                            @CustomerId,
                            @Warehouseid, @SKU, @UPC, @Goodstype, @OrderQty,
                            @AllocatedQty,
                            @BatchCode, @BoxCode, @UnitCode,@Onwer,@TrayCode,@PoCode;
                        END;

                      CLOSE cursor_order;

                      DEALLOCATE cursor_order;

                             INSERT INTO @Information
                                  (Id,
                                   Ordernumber,
                                   CustomerName,
                                   Note,
								   InformationType)
                      (SELECT id,
                              externordernumber,
                              customername,
                              orderstatus,
                              '分配成功'
                       FROM   [dbo].[wms_order]
                       WHERE  id = @Cursorid)

                      -----------------------------------------------------事物结束---------------------------------------------------   
					  --------------判断是否分配正确，提交数据还是回滚数据------------------
					  if((select count(1) from WMS_OrderDetail where OrderId=@Cursorid and OrderQty<>AllocatedQty)=0 )
					  begin
					      COMMIT
					  end
					  else begin
					       ROLLBACK TRANSACTION;
					  end 
                      
                  END try

                  BEGIN catch
                      IF Xact_state() <> 0
                        BEGIN
                            ROLLBACK TRANSACTION;

                            IF Cursor_status('local', 'Cursor_Inventory') <> -3
                              BEGIN
                                  IF Cursor_status('local', 'Cursor_Inventory')
                                     <>
                                     -1
                                    BEGIN
                                        CLOSE cursor_inventory

                                        DEALLOCATE cursor_inventory
                                    END
                              END

                            IF Cursor_status('local', 'cursor_order') <> -3
                              BEGIN
                                  IF Cursor_status('local', 'cursor_order') <>
                                     -1
                                    BEGIN
                                        CLOSE cursor_order

                                        DEALLOCATE cursor_order
                                    END
                              END
                        END;
                  --RETURN ERROR_MESSAGE()   
                  END catch
              END;
            ELSE
              BEGIN
                  --SELECT  @Message += ExternOrderNumber   
                  --FROM    [dbo].[WMS_PreOrder]   
                  --WHERE   ID = @Cursorid;    
                  --SET @Message += '库存不足，不能全部分配.';   
                  UPDATE wms_order
                  SET    orderstatus = ( CASE orderstatus
                                           WHEN 15 THEN 15
                                           ELSE 10
                                         END )
                  WHERE  id = @Cursorid
                         AND orderstatus != 9
                         AND orderstatus !=- 1;

                INSERT INTO @Information
                              (Id,
                               OrderNumber,
                               CustomerName,
                               Note,
                               InformationType,
                               SKU,
                               InventoryQty,
                               Qty)
                  (SELECT od.id,
                          od.externordernumber,
                          od.customername,
                          orderstatus,
                          '分配失败',
                          sku,
                          q.qty, od.orderqty  
                   FROM   [dbo].[wms_order] o
                          RIGHT JOIN [wms_orderdetail] od
                                  ON od.OrderId = o.id
                          OUTER apply ((SELECT Isnull(Sum(qty), 0) Qty
                                        FROM   wms_inventory_usable i
                                        WHERE  i.sku = od.sku
                                               AND i.goodstype = od.goodstype
                                               AND i.CustomerId = od.CustomerId
                                               AND i.WarehouseId = od.WarehouseId
                                               AND (i.upc = od.upc or od.upc='')
                                               AND (i.batchcode = od.batchcode or od.batchcode='')
                                               AND (i.Onwer = od.Onwer  or od.Onwer='')
                                               AND (i.BoxCode = od.BoxCode  or od.BoxCode='')
                                               AND (i.TrayCode = od.TrayCode or od.TrayCode='')
                                               AND (i.unitcode = od.unitcode or od.unitcode='')
                                               AND (i.PoCode = od.PoCode or od.PoCode='')
                                               AND i.inventorystatus = 1
                                               AND i.qty > 0)) Q
                   WHERE  o.id = @Cursorid)
              END;

            ----------------------------------------------------外围游标回收 --------------------------------------------------   
            FETCH cursor_externordernumber INTO @Cursorid;
        END;
		 CLOSE cursor_externordernumber;
                  DEALLOCATE cursor_externordernumber;
		SELECT  * FROM   @Information
      IF Cursor_status('local', 'Cursor_ExternOrderNumber') <> -3
        BEGIN
            IF Cursor_status('local', 'Cursor_ExternOrderNumber') <> -1
              BEGIN
                  CLOSE cursor_externordernumber;

                  DEALLOCATE cursor_externordernumber;
              END;
        END;
  END;
 
  END; ");
        db.Ado.GetLong(procSql.ToString(), null);
        //db.Ado.GetCommand(procSql.ToString(), null);
        //StringBuilder procSql = new StringBuilder();
        StringBuilder procSqlAdjustmentConfirmFrozen = new StringBuilder();
        //创建Proc_WMS_AdjustmentConfirmFrozen
        procSqlAdjustmentConfirmFrozen.Append(@"Create PROCEDURE [dbo].[Proc_WMS_AdjustmentConfirmFrozen]( @AdjustmentId FLOAT)
AS
  BEGIN
       
      BEGIN try
          BEGIN TRANSACTION;
DECLARE @IId bigint		   
DECLARE @ICustomerId bigint	   
DECLARE @IWarehouseId bigint	   
DECLARE @IArea VARCHAR(50)		   
DECLARE @ILocation VARCHAR(50)		   
DECLARE @ISKU VARCHAR(100)		   
DECLARE @IUPC VARCHAR(100)		   
DECLARE @IGoodsType VARCHAR(50)		   
DECLARE @IInventoryStatus int		   
DECLARE @IUnitCode VARCHAR(100)		   
DECLARE @IOnwer VARCHAR(100)		   
DECLARE @IBoxCode VARCHAR(100)		   
DECLARE @ITrayCode VARCHAR(100)		   
DECLARE @IBatchCode VARCHAR(100)		   
DECLARE @IQty decimal(18,2)		   
DECLARE @IProductionDate VARCHAR(50)		   
DECLARE @IExpirationDate VARCHAR(50)		   

DECLARE @Id  bigint	   	   
DECLARE @AdjustmentNumber  VARCHAR(50)		   
DECLARE @ExternNumber  VARCHAR(50)		   
DECLARE @CustomerId  bigint	   
DECLARE @WarehouseId bigint	   
DECLARE @SKU  VARCHAR(100)		   
DECLARE @UPC  VARCHAR(100)		   
DECLARE @TrayCode  VARCHAR(100)		   
DECLARE @BatchCode  VARCHAR(100)		   
DECLARE @BoxCode  VARCHAR(100)		 	   
DECLARE @FromWarehouseName  VARCHAR(50)		   
DECLARE @ToWarehouseName  VARCHAR(50)		   
DECLARE @FromArea  VARCHAR(50)		   
DECLARE @ToArea  VARCHAR(50)		   
DECLARE @FromLocation  VARCHAR(50)		   
DECLARE @ToLocation  VARCHAR(50)		   
DECLARE @Qty    decimal(18,2)	 
DECLARE @FromOnwer  VARCHAR(50)		   
DECLARE @ToOnwer  VARCHAR(50)		   
DECLARE @FromGoodsType  VARCHAR(50)		   
DECLARE @ToGoodsType  VARCHAR(50)		   
DECLARE @FromUnitCode  VARCHAR(100)		   
DECLARE @ToUnitCode   VARCHAR(100)		    
----操作信息   

DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(50),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );

DECLARE @NumberRemaining FLOAT

          --------------------------获取调整明细------------------------------------------------------
          DECLARE cursor_adjustmentDetail CURSOR FOR --定义游标
             select Id     	   
,AdjustmentNumber   
,ExternNumber  	
,CustomerId  
,WarehouseId  	   
,SKU   		   
,UPC   		   
,TrayCode   		   
,BatchCode   		
,BoxCode   		 	
,FromWarehouseName  
,ToWarehouseName   
,FromArea   
,ToArea   	   
,FromLocation 	
,ToLocation   	
,Qty   
,FromOnwer
,ToOnwer
,FromGoodsType 	
,ToGoodsType   	
,FromUnitCode   		
,ToUnitCode    		 from  WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId

      OPEN cursor_adjustmentDetail --打开游标

      FETCH next FROM cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据

      WHILE @@FETCH_STATUS = 0
        BEGIN
		  set  @NumberRemaining=@Qty
            -------------------------获取移动的库存数据------------------------------------------------
            DECLARE cursor_inventory CURSOR FOR --定义游标
              SELECT  Id 		   
             ,CustomerId 	   
             ,WarehouseId 	   
             ,Area 	   
             ,Location 	    
             ,SKU 		   
             ,UPC	   
             ,GoodsType    
             ,InventoryStatus  		   
             ,UnitCode    
             ,Onwer     
             ,BoxCode  	   
             ,TrayCode  		   
             ,BatchCode     
             ,Qty  	   
             ,ProductionDate    
             ,ExpirationDate from WMS_Inventory_Usable 
			 where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and Area 	   			=   @FromArea 	   		
            and Location 	    	=   @FromLocation 	    
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType    	
            and InventoryStatus  	=	1   
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode    	
			and Qty>0
  OPEN cursor_inventory --打开游标

  FETCH next FROM cursor_inventory INTO  @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据

  WHILE @@FETCH_STATUS = 0
    BEGIN
	if(@NumberRemaining>0)
	begin
        -----------判断当前库存行数量是否满足要求--------------------------
        IF( @NumberRemaining>=@IQty)
          BEGIN

              INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	 , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,@ToArea
      ,@ToLocation
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,20
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	   , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId] from WMS_Inventory_Usable where Id=@IId)

              UPDATE WMS_Inventory_Usable
              SET    InventoryStatus=99,Qty=0
              WHERE  id = @IId
			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@IQty)
              WHERE  id = @Id
			  	  set @NumberRemaining=ABS(@NumberRemaining - @IQty);
				  if(@NumberRemaining=0)
							begin
							break;
							end
            
          END
        ELSE
          BEGIN
                  INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	 , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,@ToArea
      ,@ToLocation
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,20
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	  , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,@NumberRemaining
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId] from WMS_Inventory_Usable where Id=@IId)
	       
	           UPDATE WMS_Inventory_Usable
              SET    Qty=ABS(@NumberRemaining-@IQty)
              WHERE  id = @IId
			    UPDATE WMS_AdjustmentDetail
              SET    ToQty=ABS( ToQty+@NumberRemaining)
              WHERE  id = @Id
			  	  set @NumberRemaining=0;
				  	break;
          END

		end
        FETCH cursor_inventory INTO @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据
    END

  CLOSE cursor_inventory --关闭游标

  DEALLOCATE cursor_inventory --释放游标
  IF Cursor_status('local', 'cursor_inventory') <> -3
    BEGIN
        IF Cursor_status('local', 'cursor_inventory') <> -1
          BEGIN
              CLOSE cursor_inventory;

              DEALLOCATE cursor_inventory;
          END;
    END;

  FETCH cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据
  END

  CLOSE cursor_adjustmentDetail --关闭游标

  DEALLOCATE cursor_adjustmentDetail --释放游标
  insert @Information(Id,OrderNumber,CustomerName,Note,InformationType,SKU,InventoryQty,Qty)
  select Id,ExternNumber,CustomerName,'库存冻结差异','库存冻结',SKU,ToQty,Qty from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
  and ToQty<>Qty
 	 ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
	 if((select COUNT(1) from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
	 and ToQty<>Qty)>0)
	 begin
	     ---有数据没有处理完成，数据回退
	      ROLLBACK TRANSACTION;
	 end
	  
	 else  begin
	 ------明细全部处理完成，提交
	     update WMS_Adjustment set AdjustmentStatus=99 where id=@AdjustmentId 
		 
	   COMMIT;
	
	 end
	    select * from @Information

  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
  BEGIN
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
    BEGIN
        CLOSE cursor_adjustmentDetail;

        DEALLOCATE cursor_adjustmentDetail;
    END;
  END;
  END try

      BEGIN catch
          IF Xact_state() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;
			 DECLARE @ERRORMessage NVARCHAR(max);

          SELECT @ERRORMessage = Error_message();

          RAISERROR( @ERRORMessage,16,1 );
          CLOSE cursor_inventory;

          DEALLOCATE cursor_inventory;

          CLOSE cursor_adjustmentDetail --关闭游标

          DEALLOCATE cursor_adjustmentDetail --释放游标

         
      END catch;



	 
  END 

");
        db.Ado.GetLong(procSqlAdjustmentConfirmFrozen.ToString(), null);
        StringBuilder procSqlAdjustmentConfirmGoodsType = new StringBuilder();
        //创建Proc_WMS_AdjustmentConfirmGoodsType
        procSqlAdjustmentConfirmGoodsType.Append(@"Create PROCEDURE [dbo].[Proc_WMS_AdjustmentConfirmGoodsType]( @AdjustmentId FLOAT)
AS
  BEGIN
       
      BEGIN try
          BEGIN TRANSACTION;
DECLARE @IId bigint		   
DECLARE @ICustomerId bigint	   
DECLARE @IWarehouseId bigint	   
DECLARE @IArea VARCHAR(50)		   
DECLARE @ILocation VARCHAR(50)		   
DECLARE @ISKU VARCHAR(100)		   
DECLARE @IUPC VARCHAR(100)		   
DECLARE @IGoodsType VARCHAR(50)		   
DECLARE @IInventoryStatus int		   
DECLARE @IUnitCode VARCHAR(100)		   
DECLARE @IOnwer VARCHAR(100)		   
DECLARE @IBoxCode VARCHAR(100)		   
DECLARE @ITrayCode VARCHAR(100)		   
DECLARE @IBatchCode VARCHAR(100)		   
DECLARE @IQty decimal(18,2)		   
DECLARE @IProductionDate VARCHAR(50)		   
DECLARE @IExpirationDate VARCHAR(50)		   

DECLARE @Id  bigint	   	   
DECLARE @AdjustmentNumber  VARCHAR(50)		   
DECLARE @ExternNumber  VARCHAR(50)		   
DECLARE @CustomerId  bigint	   
DECLARE @WarehouseId bigint	   
DECLARE @SKU  VARCHAR(100)		   
DECLARE @UPC  VARCHAR(100)		   
DECLARE @TrayCode  VARCHAR(100)		   
DECLARE @BatchCode  VARCHAR(100)		   
DECLARE @BoxCode  VARCHAR(100)		 	   
DECLARE @FromWarehouseName  VARCHAR(50)		   
DECLARE @ToWarehouseName  VARCHAR(50)		   
DECLARE @FromArea  VARCHAR(50)		   
DECLARE @ToArea  VARCHAR(50)		   
DECLARE @FromLocation  VARCHAR(50)		   
DECLARE @ToLocation  VARCHAR(50)		   
DECLARE @Qty    decimal(18,2)	 
DECLARE @FromOnwer  VARCHAR(50)		   
DECLARE @ToOnwer  VARCHAR(50)		   
DECLARE @FromGoodsType  VARCHAR(50)		   
DECLARE @ToGoodsType  VARCHAR(50)		   
DECLARE @FromUnitCode  VARCHAR(100)		   
DECLARE @ToUnitCode   VARCHAR(100)		    

DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(50),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );

DECLARE @NumberRemaining FLOAT

          --------------------------获取调整明细------------------------------------------------------
          DECLARE cursor_adjustmentDetail CURSOR FOR --定义游标
             select Id     	   
,AdjustmentNumber   
,ExternNumber  	
,CustomerId  
,WarehouseId  	   
,SKU   		   
,UPC   		   
,TrayCode   		   
,BatchCode   		
,BoxCode   		 	
,FromWarehouseName  
,ToWarehouseName   
,FromArea   
,ToArea   	   
,FromLocation 	
,ToLocation   	
,Qty   
,FromOnwer
,ToOnwer
,FromGoodsType 	
,ToGoodsType   	
,FromUnitCode   		
,ToUnitCode    		 from  WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId

      OPEN cursor_adjustmentDetail --打开游标

      FETCH next FROM cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据

      WHILE @@FETCH_STATUS = 0
        BEGIN
		  set  @NumberRemaining=@Qty
            -------------------------获取调整的库存数据------------------------------------------------
            DECLARE cursor_inventory CURSOR FOR --定义游标
              SELECT  Id 		   
             ,CustomerId 	   
             ,WarehouseId 	   
             ,Area 	   
             ,Location 	    
             ,SKU 		   
             ,UPC	   
             ,GoodsType    
             ,InventoryStatus  		   
             ,UnitCode    
             ,Onwer     
             ,BoxCode  	   
             ,TrayCode  		   
             ,BatchCode     
             ,Qty  	   
             ,ProductionDate    
             ,ExpirationDate from WMS_Inventory_Usable 
			 where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and Area 	   			=   @FromArea 	   		
            and Location 	    	=   @FromLocation 	    
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType    	
            and InventoryStatus  	=	1   
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode    	
			and Qty>0
  OPEN cursor_inventory --打开游标

  FETCH next FROM cursor_inventory INTO  @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据

  WHILE @@FETCH_STATUS = 0
    BEGIN
	if(@NumberRemaining>0)
	begin
        -----------判断当前库存行数量是否满足要求--------------------------
        IF( @NumberRemaining>=@IQty)
          BEGIN

            
              UPDATE WMS_Inventory_Usable
              SET     GoodsType=@ToGoodsType
              WHERE  id = @IId 
			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@IQty)
              WHERE  id = @Id
			  	  set @NumberRemaining=ABS(@NumberRemaining - @IQty);
				  if(@NumberRemaining=0)
							begin
							break;
							end
            
          END
        ELSE
          BEGIN
                  INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	   , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,Area
      ,Location
      ,[SKU]
      ,[UPC]
      ,@ToGoodsType
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	   , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,@NumberRemaining
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId] from WMS_Inventory_Usable where Id=@IId)
	       
	           UPDATE WMS_Inventory_Usable
              SET    Qty=abs(@NumberRemaining-@IQty)
              WHERE  id = @IId
			    UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@NumberRemaining)
              WHERE  id = @Id
			  	  set @NumberRemaining=0;
				  break;
          END

		end
        FETCH cursor_inventory INTO @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据
    END

  CLOSE cursor_inventory --关闭游标

  DEALLOCATE cursor_inventory --释放游标
  IF Cursor_status('local', 'cursor_inventory') <> -3
    BEGIN
        IF Cursor_status('local', 'cursor_inventory') <> -1
          BEGIN
              CLOSE cursor_inventory;

              DEALLOCATE cursor_inventory;
          END;
    END;

  FETCH cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据
  END

  CLOSE cursor_adjustmentDetail --关闭游标

  DEALLOCATE cursor_adjustmentDetail --释放游标
   insert @Information(Id,OrderNumber,CustomerName,Note,InformationType,SKU,InventoryQty,Qty)
  select Id,ExternNumber,CustomerName,'库存冻结差异','库存冻结',SKU,ToQty,Qty from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
  and ToQty<>Qty
 	 ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
	 if((select COUNT(1) from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
	 and ToQty<>Qty)>0)
	 begin
	     ---有数据没有处理完成，数据回退
	      ROLLBACK TRANSACTION;
	 end
	  
	 else  begin
	 ------明细全部处理完成，提交
	     update WMS_Adjustment set AdjustmentStatus=99 where id=@AdjustmentId 
		 
	   COMMIT;
	 end
	 
	    select * from @Information
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
  BEGIN
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
    BEGIN
        CLOSE cursor_adjustmentDetail;

        DEALLOCATE cursor_adjustmentDetail;
    END;
  END;
  END try

      BEGIN catch
          IF Xact_state() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

          CLOSE cursor_inventory;

          DEALLOCATE cursor_inventory;

          CLOSE cursor_adjustmentDetail --关闭游标

          DEALLOCATE cursor_adjustmentDetail --释放游标

          DECLARE @ERRORMessage NVARCHAR(max);

          SELECT @ERRORMessage = Error_message();

          RAISERROR( @ERRORMessage,16,1 );
      END catch;



	 
  END 

");
        db.Ado.GetLong(procSqlAdjustmentConfirmGoodsType.ToString(), null);
        StringBuilder procSqlAdjustmentConfirmMove = new StringBuilder();
        //创建Proc_WMS_AdjustmentConfirmMove
        procSqlAdjustmentConfirmMove.Append(@"Create PROCEDURE [dbo].[Proc_WMS_AdjustmentConfirmMove]( @AdjustmentId FLOAT)
AS
  BEGIN
       
      BEGIN try
       BEGIN TRANSACTION;

    DECLARE @IId BIGINT
    DECLARE @ICustomerId BIGINT
    DECLARE @IWarehouseId BIGINT
    DECLARE @IArea VARCHAR(50)
    DECLARE @ILocation VARCHAR(50)
    DECLARE @ISKU VARCHAR(100)
    DECLARE @IUPC VARCHAR(100)
    DECLARE @IGoodsType VARCHAR(50)
    DECLARE @IInventoryStatus INT
    DECLARE @IUnitCode VARCHAR(100)
    DECLARE @IOnwer VARCHAR(100)
    DECLARE @IBoxCode VARCHAR(100)
    DECLARE @ITrayCode VARCHAR(100)
    DECLARE @IBatchCode VARCHAR(100)
    DECLARE @IQty DECIMAL(18, 2)
    DECLARE @IProductionDate VARCHAR(50)
    DECLARE @IExpirationDate VARCHAR(50)
    DECLARE @Id BIGINT
    DECLARE @AdjustmentNumber VARCHAR(50)
    DECLARE @ExternNumber VARCHAR(50)
    DECLARE @CustomerId BIGINT
    DECLARE @WarehouseId BIGINT
    DECLARE @SKU VARCHAR(100)
    DECLARE @UPC VARCHAR(100)
    DECLARE @TrayCode VARCHAR(100)
    DECLARE @BatchCode VARCHAR(100)
    DECLARE @BoxCode VARCHAR(100)
    DECLARE @FromWarehouseName VARCHAR(50)
    DECLARE @ToWarehouseName VARCHAR(50)
    DECLARE @FromArea VARCHAR(50)
    DECLARE @ToArea VARCHAR(50)
    DECLARE @FromLocation VARCHAR(50)
    DECLARE @ToLocation VARCHAR(50)
    DECLARE @Qty DECIMAL(18, 2)
    DECLARE @FromOnwer VARCHAR(50)
    DECLARE @ToOnwer VARCHAR(50)
    DECLARE @FromGoodsType VARCHAR(50)
    DECLARE @ToGoodsType VARCHAR(50)
    DECLARE @FromUnitCode VARCHAR(100)
    DECLARE @ToUnitCode VARCHAR(100)
    DECLARE @NumberRemaining FLOAT

	
DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(50),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );


    --------------------------获取调整明细------------------------------------------------------
    DECLARE cursor_adjustmentdetail CURSOR FOR --定义游标
      SELECT id,
             adjustmentnumber,
             externnumber,
             customerid,
             warehouseid,
             sku,
             upc,
             traycode,
             batchcode,
             boxcode,
             fromwarehousename,
             towarehousename,
             fromarea,
             toarea,
             fromlocation,
             tolocation,
             qty,
             fromonwer,
             toonwer,
             fromgoodstype,
             togoodstype,
             fromunitcode,
             tounitcode
      FROM   wms_adjustmentdetail
      WHERE  adjustmentid = @AdjustmentId

    OPEN cursor_adjustmentdetail --打开游标
    FETCH next FROM cursor_adjustmentdetail INTO @Id, @AdjustmentNumber,
    @ExternNumber, @CustomerId, @WarehouseId, @SKU, @UPC, @TrayCode, @BatchCode,
    @BoxCode, @FromWarehouseName, @ToWarehouseName, @FromArea, @ToArea,
    @FromLocation, @ToLocation, @Qty, @FromOnwer, @ToOnwer, @FromGoodsType,
    @ToGoodsType, @FromUnitCode, @ToUnitCode --抓取下一行游标数据
    WHILE @@FETCH_STATUS = 0
      BEGIN
          SET @NumberRemaining=@Qty 
          -------------------------获取移动的库存数据------------------------------------------------
          DECLARE cursor_inventory CURSOR FOR --定义游标
            SELECT id,
                   customerid,
                   warehouseid,
                   area,
                   location,
                   sku,
                   upc,
                   goodstype,
                   inventorystatus,
                   unitcode,
                   onwer,
                   boxcode,
                   traycode,
                   batchcode,
                   qty,
                   productiondate,
                   expirationdate
            FROM   wms_inventory_usable
            WHERE  customerid = @CustomerId
                   AND warehouseid = @WarehouseId
                   AND area = @FromArea
                   AND location = @FromLocation
                   AND sku = @SKU
                   AND upc = @UPC
                   AND goodstype = @FromGoodsType
                   AND inventorystatus = 1
                   AND unitcode = @FromUnitCode
                   AND onwer = @FromOnwer
                   AND boxcode = @BoxCode
                   AND traycode = @TrayCode
                   AND batchcode = @BatchCode
                   AND qty > 0

          OPEN cursor_inventory --打开游标
          FETCH next FROM cursor_inventory INTO @IId, @ICustomerId,
          @IWarehouseId,
          @IArea, @ILocation, @ISKU, @IUPC, @IGoodsType, @IInventoryStatus,
          @IUnitCode, @IOnwer, @IBoxCode, @ITrayCode, @IBatchCode, @IQty, @IProductionDate,
          @IExpirationDate

          --抓取下一行游标数据
          WHILE @@FETCH_STATUS = 0
            BEGIN

                IF( @NumberRemaining > 0 )
                  BEGIN
                      -----------判断当前库存行数量是否满足要求--------------------------
                      IF( @NumberRemaining >= @IQty )
                        BEGIN
                            INSERT INTO wms_inventory_usable
                                        ([receiptdetailid],
                                         [receiptreceivingid],
                                         [customerid],
                                         [customername],
                                         [warehouseid],
                                         [warehousename],
                                         [area],
                                         [location],
                                         [sku],
                                         [upc],
                                         [goodstype],
                                         [inventorystatus],
                                         [superid],
                                         [relatedid],
                                         [goodsname],
                                         [unitcode],
                                         [onwer],
                                         [boxcode],
                                         [traycode],
                                         [batchcode],
                                         [qty],
                                         [productiondate],
                                         [expirationdate],
                                         [remark],
                                         [inventorytime],
                                         [creator],
                                         [creationtime],
                                         [updator],
                                         [updatetime],
                                         [str1],
                                         [str2],
                                         [str3],
                                         [str4],
                                         [str5],
                                         [datetime1],
                                         [datetime2],
                                         [int1],
                                         [int2],[TenantId])
                            (SELECT [receiptdetailid],
                                    [receiptreceivingid],
                                    [customerid],
                                    [customername],
                                    [warehouseid],
                                    [warehousename],
                                    @ToArea,
                                    @ToLocation,
                                    [sku],
                                    [upc],
                                    [goodstype],
                                    [inventorystatus],
                                    [superid],
                                    [relatedid],
                                    [goodsname],
                                    [unitcode],
                                    [onwer],
                                    [boxcode],
                                    [traycode],
                                    [batchcode],
                                    [qty],
                                    [productiondate],
                                    [expirationdate],
                                    [remark],
                                    [inventorytime],
                                    [creator],
                                    [creationtime],
                                    [updator],
                                    [updatetime],
                                    [str1],
                                    [str2],
                                    [str3],
                                    [str4],
                                    [str5],
                                    [datetime1],
                                    [datetime2],
                                    [int1],
                                    [int2],[TenantId]
                             FROM   wms_inventory_usable
                             WHERE  id = @IId)

                            UPDATE wms_inventory_usable
                            SET    inventorystatus = 99,
                                   qty = 0
                            WHERE  id = @IId

                            UPDATE wms_adjustmentdetail
                            SET    toqty = ( toqty + @IQty )
                            WHERE  id = @Id

                            SET @NumberRemaining=abs(@NumberRemaining - @IQty);
							if(@NumberRemaining=0)
							begin
							break;
							end
                        END
                      ELSE
                        BEGIN
                            INSERT INTO wms_inventory_usable
                                        ([receiptdetailid],
                                         [receiptreceivingid],
                                         [customerid],
                                         [customername],
                                         [warehouseid],
                                         [warehousename],
                                         [area],
                                         [location],
                                         [sku],
                                         [upc],
                                         [goodstype],
                                         [inventorystatus],
                                         [superid],
                                         [relatedid],
                                         [goodsname],
                                         [unitcode],
                                         [onwer],
                                         [boxcode],
                                         [traycode],
                                         [batchcode],
                                         [qty],
                                         [productiondate],
                                         [expirationdate],
                                         [remark],
                                         [inventorytime],
                                         [creator],
                                         [creationtime],
                                         [updator],
                                         [updatetime],
                                         [str1],
                                         [str2],
                                         [str3],
                                         [str4],
                                         [str5],
                                         [datetime1],
                                         [datetime2],
                                         [int1],
                                         [int2],[TenantId])
                            (SELECT [receiptdetailid],
                                    [receiptreceivingid],
                                    [customerid],
                                    [customername],
                                    [warehouseid],
                                    [warehousename],
                                    @ToArea,
                                    @ToLocation,
                                    [sku],
                                    [upc],
                                    [goodstype],
                                    [inventorystatus],
                                    [superid],
                                    [relatedid],
                                    [goodsname],
                                    [unitcode],
                                    [onwer],
                                    [boxcode],
                                    [traycode],
                                    [batchcode],
                                    @NumberRemaining,
                                    [productiondate],
                                    [expirationdate],
                                    [remark],
                                    [inventorytime],
                                    [creator],
                                    [creationtime],
                                    [updator],
                                    [updatetime],
                                    [str1],
                                    [str2],
                                    [str3],
                                    [str4],
                                    [str5],
                                    [datetime1],
                                    [datetime2],
                                    [int1],
                                    [int2],[TenantId]
                             FROM   wms_inventory_usable
                             WHERE  id = @IId)

                            UPDATE wms_inventory_usable
                            SET    qty = ABS( @NumberRemaining - @IQty )
                            WHERE  id = @IId

                            UPDATE wms_adjustmentdetail
                            SET    toqty = ( toqty + @NumberRemaining )
                            WHERE  id = @Id

                            SET @NumberRemaining=0;
							break;
                        END
                  END

                FETCH cursor_inventory INTO @IId, @ICustomerId,
          @IWarehouseId,
          @IArea, @ILocation, @ISKU, @IUPC, @IGoodsType, @IInventoryStatus,
          @IUnitCode, @IOnwer, @IBoxCode, @ITrayCode, @IBatchCode, @IQty, @IProductionDate,
          @IExpirationDate
            END
			 
          CLOSE cursor_inventory --关闭游标
          DEALLOCATE cursor_inventory --释放游标

          IF Cursor_status('local', 'cursor_inventory') <> -3
            BEGIN
                IF Cursor_status('local', 'cursor_inventory') <> -1
                  BEGIN
                      CLOSE cursor_inventory;

                      DEALLOCATE cursor_inventory;
                  END;
            END;

          FETCH cursor_adjustmentdetail INTO @Id, @AdjustmentNumber,
    @ExternNumber, @CustomerId, @WarehouseId, @SKU, @UPC, @TrayCode, @BatchCode,
    @BoxCode, @FromWarehouseName, @ToWarehouseName, @FromArea, @ToArea,
    @FromLocation, @ToLocation, @Qty, @FromOnwer, @ToOnwer, @FromGoodsType,
    @ToGoodsType, @FromUnitCode, @ToUnitCode --抓取下一行游标数据
      END 
    CLOSE cursor_adjustmentdetail --关闭游标
    DEALLOCATE cursor_adjustmentdetail --释放游标

	  insert @Information(Id,OrderNumber,CustomerName,Note,InformationType,SKU,InventoryQty,Qty)
  select Id,ExternNumber,CustomerName,'库存冻结差异','库存冻结',SKU,ToQty,Qty from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
  and ToQty<>Qty

    ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
    IF( (SELECT Count(1)
         FROM   wms_adjustmentdetail
         WHERE  adjustmentid = @AdjustmentId
                AND toqty <> qty) > 0 )
      BEGIN
          ---有数据没有处理完成，数据回退
          ROLLBACK TRANSACTION;
      END
    ELSE
      BEGIN
          ------明细全部处理完成，提交
          UPDATE wms_adjustment
          SET    adjustmentstatus = 99
          WHERE  id = @AdjustmentId

          COMMIT;
      END
	     select * from @Information
    IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
      BEGIN
          IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
            BEGIN
                CLOSE cursor_adjustmentdetail;

                DEALLOCATE cursor_adjustmentdetail;
            END;
      END;
END try

BEGIN catch
    IF Xact_state() <> 0
      BEGIN
          ROLLBACK TRANSACTION;
      END; 
    
    DECLARE @ERRORMessage NVARCHAR(max);

    SELECT @ERRORMessage = Error_message(); 

    RAISERROR( @ERRORMessage,16,1 );
	CLOSE cursor_inventory;
    DEALLOCATE cursor_inventory;
    CLOSE cursor_adjustmentdetail --关闭游标
    DEALLOCATE cursor_adjustmentdetail --释放游标
END catch; 

	 
  END 

");
        db.Ado.GetLong(procSqlAdjustmentConfirmMove.ToString(), null);
        StringBuilder procSqlAdjustmentConfirmQuantity = new StringBuilder();
        //创建PProc_WMS_AdjustmentConfirmQuantity
        procSqlAdjustmentConfirmQuantity.Append(@"Create PROCEDURE [dbo].[Proc_WMS_AdjustmentConfirmQuantity]( @AdjustmentId FLOAT)
AS
  BEGIN
       
      BEGIN try
          BEGIN TRANSACTION;
DECLARE @IId bigint		   
DECLARE @ICustomerId bigint	   
DECLARE @IWarehouseId bigint	   
DECLARE @IArea VARCHAR(50)		   
DECLARE @ILocation VARCHAR(50)		   
DECLARE @ISKU VARCHAR(100)		   
DECLARE @IUPC VARCHAR(100)		   
DECLARE @IGoodsType VARCHAR(50)		   
DECLARE @IInventoryStatus int		   
DECLARE @IUnitCode VARCHAR(100)		   
DECLARE @IOnwer VARCHAR(100)		   
DECLARE @IBoxCode VARCHAR(100)		   
DECLARE @ITrayCode VARCHAR(100)		   
DECLARE @IBatchCode VARCHAR(100)		   
DECLARE @IQty decimal(18,2)		   
DECLARE @IProductionDate VARCHAR(50)		   
DECLARE @IExpirationDate VARCHAR(50)		   

DECLARE @Id  bigint	   	   
DECLARE @AdjustmentNumber  VARCHAR(50)		   
DECLARE @ExternNumber  VARCHAR(50)		   
DECLARE @CustomerId  bigint	   
DECLARE @WarehouseId bigint	   
DECLARE @SKU  VARCHAR(100)		   
DECLARE @UPC  VARCHAR(100)		   
DECLARE @TrayCode  VARCHAR(100)		   
DECLARE @BatchCode  VARCHAR(100)		   
DECLARE @BoxCode  VARCHAR(100)		 	   
DECLARE @FromWarehouseName  VARCHAR(50)		   
DECLARE @ToWarehouseName  VARCHAR(50)		   
DECLARE @FromArea  VARCHAR(50)		   
DECLARE @ToArea  VARCHAR(50)		   
DECLARE @FromLocation  VARCHAR(50)		   
DECLARE @ToLocation  VARCHAR(50)		   
DECLARE @Qty    decimal(18,2)	 
DECLARE @FromOnwer  VARCHAR(50)		   
DECLARE @ToOnwer  VARCHAR(50)		   
DECLARE @FromGoodsType  VARCHAR(50)		   
DECLARE @ToGoodsType  VARCHAR(50)		   
DECLARE @FromUnitCode  VARCHAR(100)		   
DECLARE @ToUnitCode   VARCHAR(100)		    
DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(50),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );


DECLARE @NumberRemaining FLOAT

          --------------------------获取调整明细------------------------------------------------------
          DECLARE cursor_adjustmentDetail CURSOR FOR --定义游标
             select Id     	   
,AdjustmentNumber   
,ExternNumber  	
,CustomerId  
,WarehouseId  	   
,SKU   		   
,UPC   		   
,TrayCode   		   
,BatchCode   		
,BoxCode   		 	
,FromWarehouseName  
,ToWarehouseName   
,FromArea   
,ToArea   	   
,FromLocation 	
,ToLocation   	
,Qty   
,FromOnwer
,ToOnwer
,FromGoodsType 	
,ToGoodsType   	
,FromUnitCode   		
,ToUnitCode    		 from  WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
and ToQty<Qty
      OPEN cursor_adjustmentDetail --打开游标

      FETCH next FROM cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据

      WHILE @@FETCH_STATUS = 0
        BEGIN
		set  @NumberRemaining=@Qty

		---判断是调增，还是调减 
	   --调增，比较容易，找到一条合适的条件的数据，按照需求增加就好（必须是已经入库过的数据）
	   if(@NumberRemaining>0)
	   begin
	           INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
              ,[ReceiptReceivingId]
              ,[CustomerId]
              ,[CustomerName]
              ,[WarehouseId]
              ,[WarehouseName]
              ,[Area]
              ,[Location]
              ,[SKU]
              ,[UPC]
              ,[GoodsType]
              ,[InventoryStatus]
              ,[SuperId]
              ,[RelatedId]
              ,[GoodsName]
              ,[UnitCode]
              ,[Onwer]
              ,[BoxCode]
              ,[TrayCode]
              ,[BatchCode]
              ,[Qty]
              ,[ProductionDate]
              ,[ExpirationDate]
              ,[Remark]
              ,[InventoryTime]
              ,[Creator]
              ,[CreationTime]
              ,[Updator]
              ,[UpdateTime]
              ,[Str1]
              ,[Str2]
              ,[Str3]
              ,[Str4]
              ,[Str5]
              ,[DateTime1]
              ,[DateTime2]
              ,[Int1]
              ,[Int2],[TenantId])
                     (select top 1 [ReceiptDetailId]
              ,[ReceiptReceivingId]
              ,[CustomerId]
              ,[CustomerName]
              ,[WarehouseId]
              ,[WarehouseName]
              ,@ToArea
              ,@ToLocation
              ,[SKU]
              ,[UPC]
              ,@ToGoodsType
              ,1
              ,[SuperId]
              ,[RelatedId]
              ,[GoodsName]
              ,[UnitCode]
              , @ToOnwer
              ,[BoxCode]
              ,@TrayCode
              ,@BatchCode
              ,@Qty
              ,[ProductionDate]
              ,[ExpirationDate]
              ,[Remark]
              ,getdate()
              ,[Creator]
              ,[CreationTime]
              ,[Updator]
              ,[UpdateTime]
              ,[Str1]
              ,[Str2]
              ,[Str3]
              ,[Str4]
              ,[Str5]
              ,[DateTime1]
              ,[DateTime2]
              ,[Int1]
              ,[Int2],[TenantId] from WMS_Inventory_Usable    where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType      
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode )

			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=@Qty
              WHERE  id = @Id
			  set  @NumberRemaining=0;
	   end
	   else begin
	    
		  
            -------------------------获取移动的库存数据------------------------------------------------
            DECLARE cursor_inventory CURSOR FOR --定义游标
              SELECT  Id 		   
             ,CustomerId 	   
             ,WarehouseId 	   
             ,Area 	   
             ,Location 	    
             ,SKU 		   
             ,UPC	   
             ,GoodsType    
             ,InventoryStatus  		   
             ,UnitCode    
             ,Onwer     
             ,BoxCode  	   
             ,TrayCode  		   
             ,BatchCode     
             ,Qty  	   
             ,ProductionDate    
             ,ExpirationDate from WMS_Inventory_Usable 
			 where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and Area 	   			=   @FromArea 	   		
            and Location 	    	=   @FromLocation 	    
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType    	
            and InventoryStatus  	=	1   
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode    	
			and Qty>0
  OPEN cursor_inventory --打开游标

  FETCH next FROM cursor_inventory INTO  @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据

  WHILE @@FETCH_STATUS = 0
    BEGIN
	if(ABS(@NumberRemaining)>0)
	begin 
        -----------判断当前库存行数量是否满足要求--------------------------
        IF( ABS(@NumberRemaining)>=@IQty)
          BEGIN


              UPDATE WMS_Inventory_Usable
              SET    InventoryStatus=99,Qty=0
              WHERE  id = @IId
			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+@IQty)
              WHERE  id = @Id
			  	  set @NumberRemaining=@NumberRemaining - @IQty;
            
          END
        ELSE
          BEGIN
                  
	           UPDATE WMS_Inventory_Usable
              SET    Qty=(ABS(@NumberRemaining)-@IQty)
              WHERE  id = @IId
			    UPDATE WMS_AdjustmentDetail
              SET    ToQty=( ToQty+ABS(@NumberRemaining))
              WHERE  id = @Id
			  	  set @NumberRemaining=0;
          END

		end
        FETCH cursor_inventory INTO @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
    END

  CLOSE cursor_inventory --关闭游标

  DEALLOCATE cursor_inventory --释放游标
  IF Cursor_status('local', 'cursor_inventory') <> -3
    BEGIN
        IF Cursor_status('local', 'cursor_inventory') <> -1
          BEGIN
              CLOSE cursor_inventory;

              DEALLOCATE cursor_inventory;
          END;
    END;
	 end
  FETCH cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据
  END

  CLOSE cursor_adjustmentDetail --关闭游标

  DEALLOCATE cursor_adjustmentDetail --释放游标
    insert @Information(Id,OrderNumber,CustomerName,Note,InformationType,SKU,InventoryQty,Qty)
  select Id,ExternNumber,CustomerName,'库存冻结差异','库存冻结',SKU,ToQty,Qty from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
  and ToQty<>Qty

 	 ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
	 if((select COUNT(1) from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
	 and ToQty<>Qty)>0)
	 begin
	     ---有数据没有处理完成，数据回退
	      ROLLBACK TRANSACTION;
	 end
	  
	 else  begin
	 ------明细全部处理完成，提交
	     update WMS_Adjustment set AdjustmentStatus=99 where id=@AdjustmentId 
		 
	   COMMIT;
	 end
	   select * from @Information

  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
  BEGIN
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
    BEGIN
        CLOSE cursor_adjustmentDetail;

        DEALLOCATE cursor_adjustmentDetail;
    END;
  END;
  END try

      BEGIN catch
          IF Xact_state() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

          CLOSE cursor_inventory;

          DEALLOCATE cursor_inventory;

          CLOSE cursor_adjustmentDetail --关闭游标

          DEALLOCATE cursor_adjustmentDetail --释放游标

          DECLARE @ERRORMessage NVARCHAR(max);

          SELECT @ERRORMessage = Error_message();

          RAISERROR( @ERRORMessage,16,1 );
      END catch;



	 
  END 

");
        db.Ado.GetLong(procSqlAdjustmentConfirmQuantity.ToString(), null);
        StringBuilder procSqlAdjustmentConfirmUnfreeze = new StringBuilder();
        //创建Proc_WMS_AdjustmentConfirmUnfreeze
        procSqlAdjustmentConfirmUnfreeze.Append(@"Create PROCEDURE [dbo].[Proc_WMS_AdjustmentConfirmUnfreeze]( @AdjustmentId FLOAT)
AS
  BEGIN
       
      BEGIN try
          BEGIN TRANSACTION;
DECLARE @IId bigint		   
DECLARE @ICustomerId bigint	   
DECLARE @IWarehouseId bigint	   
DECLARE @IArea VARCHAR(50)		   
DECLARE @ILocation VARCHAR(50)		   
DECLARE @ISKU VARCHAR(100)		   
DECLARE @IUPC VARCHAR(100)		   
DECLARE @IGoodsType VARCHAR(50)		   
DECLARE @IInventoryStatus int		   
DECLARE @IUnitCode VARCHAR(100)		   
DECLARE @IOnwer VARCHAR(100)		   
DECLARE @IBoxCode VARCHAR(100)		   
DECLARE @ITrayCode VARCHAR(100)		   
DECLARE @IBatchCode VARCHAR(100)		   
DECLARE @IQty decimal(18,2)		   
DECLARE @IProductionDate VARCHAR(50)		   
DECLARE @IExpirationDate VARCHAR(50)		   

DECLARE @Id  bigint	   	   
DECLARE @AdjustmentNumber  VARCHAR(50)		   
DECLARE @ExternNumber  VARCHAR(50)		   
DECLARE @CustomerId  bigint	   
DECLARE @WarehouseId bigint	   
DECLARE @SKU  VARCHAR(100)		   
DECLARE @UPC  VARCHAR(100)		   
DECLARE @TrayCode  VARCHAR(100)		   
DECLARE @BatchCode  VARCHAR(100)		   
DECLARE @BoxCode  VARCHAR(100)		 	   
DECLARE @FromWarehouseName  VARCHAR(50)		   
DECLARE @ToWarehouseName  VARCHAR(50)		   
DECLARE @FromArea  VARCHAR(50)		   
DECLARE @ToArea  VARCHAR(50)		   
DECLARE @FromLocation  VARCHAR(50)		   
DECLARE @ToLocation  VARCHAR(50)		   
DECLARE @Qty    decimal(18,2)	 
DECLARE @FromOnwer  VARCHAR(50)		   
DECLARE @ToOnwer  VARCHAR(50)		   
DECLARE @FromGoodsType  VARCHAR(50)		   
DECLARE @ToGoodsType  VARCHAR(50)		   
DECLARE @FromUnitCode  VARCHAR(100)		   
DECLARE @ToUnitCode   VARCHAR(100)		    
DECLARE @Information TABLE
  (
     Id              BIGINT,
     OrderNumber VARCHAR(50),
     CustomerName          VARCHAR(50),
     Note              VARCHAR(50),
     InformationType            VARCHAR(50),
     SKU               VARCHAR(50),
     InventoryQty              FLOAT,
     Qty              FLOAT
  );


DECLARE @NumberRemaining FLOAT

          --------------------------获取调整明细------------------------------------------------------
          DECLARE cursor_adjustmentDetail CURSOR FOR --定义游标
             select Id     	   
,AdjustmentNumber   
,ExternNumber  	
,CustomerId  
,WarehouseId  	   
,SKU   		   
,UPC   		   
,TrayCode   		   
,BatchCode   		
,BoxCode   		 	
,FromWarehouseName  
,ToWarehouseName   
,FromArea   
,ToArea   	   
,FromLocation 	
,ToLocation   	
,Qty   
,FromOnwer
,ToOnwer
,FromGoodsType 	
,ToGoodsType   	
,FromUnitCode   		
,ToUnitCode    		 from  WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId

      OPEN cursor_adjustmentDetail --打开游标

      FETCH next FROM cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据

      WHILE @@FETCH_STATUS = 0
        BEGIN
		  set  @NumberRemaining=@Qty
            -------------------------获取调整的库存数据------------------------------------------------
            DECLARE cursor_inventory CURSOR FOR --定义游标
              SELECT  Id 		   
             ,CustomerId 	   
             ,WarehouseId 	   
             ,Area 	   
             ,Location 	    
             ,SKU 		   
             ,UPC	   
             ,GoodsType    
             ,InventoryStatus  		   
             ,UnitCode    
             ,Onwer     
             ,BoxCode  	   
             ,TrayCode  		   
             ,BatchCode     
             ,Qty  	   
             ,ProductionDate    
             ,ExpirationDate from WMS_Inventory_Usable 
			 where  
             CustomerId 	    =   @CustomerId 	 
            and WarehouseId 	   	=   @WarehouseId 	
            and Area 	   			=   @FromArea 	   		
            and Location 	    	=   @FromLocation 	    
            and SKU 		   		=   @SKU 		   	
            and UPC	   			    =   @UPC	   		
            and GoodsType    		=   @FromGoodsType    	
            and InventoryStatus  	=	20 --解冻单需要操作状态为20的库存 
            and UnitCode    		=   @FromUnitCode    	
            and Onwer     			=   @FromOnwer     		
            and BoxCode  	   		=   @BoxCode  	   	
            and TrayCode  		   	=   @TrayCode  		
            and BatchCode    		=   @BatchCode    	
			and Qty>0
  OPEN cursor_inventory --打开游标

  FETCH next FROM cursor_inventory INTO  @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
  --抓取下一行游标数据

  WHILE @@FETCH_STATUS = 0
    BEGIN
	if(@NumberRemaining>0)
	begin
        -----------判断当前库存行数量是否满足要求--------------------------
        IF( @NumberRemaining>=@IQty)
          BEGIN

            
              UPDATE WMS_Inventory_Usable
              SET    InventoryStatus=1
              WHERE  id = @IId 
			      UPDATE WMS_AdjustmentDetail
              SET    ToQty=abs( ToQty+@IQty)
              WHERE  id = @Id
			  	  set @NumberRemaining=abs(@NumberRemaining - @IQty);
				   if(@NumberRemaining=0)
							begin
							break;
							end
            
          END
        ELSE
          BEGIN
                  INSERT INTO WMS_Inventory_Usable
                          ([ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,[Area]
      ,[Location]
      ,[SKU]
      ,[UPC]
      ,[GoodsType]
      ,[InventoryStatus]
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	   , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,[Qty]
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2],[TenantId])
             (select [ReceiptDetailId]
      ,[ReceiptReceivingId]
      ,[CustomerId]
      ,[CustomerName]
      ,[WarehouseId]
      ,[WarehouseName]
      ,Area
      ,Location
      ,[SKU]
      ,[UPC]
      ,GoodsType
      ,1
      ,[SuperId]
      ,[RelatedId]
      ,[GoodsName]
      ,[UnitCode]
      ,[Onwer]
      ,[BoxCode]
      ,[TrayCode]
      ,[BatchCode]
	   , [LotCode]
     , [PoCode] 
     , [Weight] 
     , [Volume] 
      ,@NumberRemaining
      ,[ProductionDate]
      ,[ExpirationDate]
      ,[Remark]
      ,[InventoryTime]
      ,[Creator]
      ,[CreationTime]
      ,[Updator]
      ,[UpdateTime]
      ,[Str1]
      ,[Str2]
      ,[Str3]
      ,[Str4]
      ,[Str5]
      ,[DateTime1]
      ,[DateTime2]
      ,[Int1]
      ,[Int2] ,[TenantId] from WMS_Inventory_Usable where Id=@IId)
	       
	           UPDATE WMS_Inventory_Usable
              SET    Qty=(ABS(@NumberRemaining)-@IQty)
              WHERE  id = @IId
			    UPDATE WMS_AdjustmentDetail
              SET    ToQty=ABS( ToQty+(@NumberRemaining))
              WHERE  id = @Id
			  	  set @NumberRemaining=0;
				  break;
          END

		end
        FETCH cursor_inventory INTO @IId  		   
,@ICustomerId  	   
,@IWarehouseId  	   
,@IArea     
,@ILocation     
,@ISKU     
,@IUPC     
,@IGoodsType     
,@IInventoryStatus  		   
,@IUnitCode 	   
,@IOnwer 		   
,@IBoxCode  	   
,@ITrayCode   
,@IBatchCode 	   
,@IQty 	   
,@IProductionDate  	   
,@IExpirationDate  
    END

  CLOSE cursor_inventory --关闭游标

  DEALLOCATE cursor_inventory --释放游标
  IF Cursor_status('local', 'cursor_inventory') <> -3
    BEGIN
        IF Cursor_status('local', 'cursor_inventory') <> -1
          BEGIN
              CLOSE cursor_inventory;

              DEALLOCATE cursor_inventory;
          END;
    END;

  FETCH cursor_adjustmentDetail INTO @Id     	   
,@AdjustmentNumber   
,@ExternNumber  	
,@CustomerId  
,@WarehouseId  	   
,@SKU   		   
,@UPC   		   
,@TrayCode   		   
,@BatchCode   		
,@BoxCode   		 	
,@FromWarehouseName  
,@ToWarehouseName   
,@FromArea   
,@ToArea   	   
,@FromLocation 	
,@ToLocation   	
,@Qty    		
,@FromOnwer
,@ToOnwer
,@FromGoodsType 	
,@ToGoodsType   	
,@FromUnitCode   		
,@ToUnitCode    		--抓取下一行游标数据
  END

  CLOSE cursor_adjustmentDetail --关闭游标

  DEALLOCATE cursor_adjustmentDetail --释放游标
    insert @Information(Id,OrderNumber,CustomerName,Note,InformationType,SKU,InventoryQty,Qty)
  select Id,ExternNumber,CustomerName,'库存冻结差异','库存冻结',SKU,ToQty,Qty from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
  and ToQty<>Qty
 	 ---当ToQty = Qty 的时候意味着全部处理完成，改变订单状态
	 if((select COUNT(1) from WMS_AdjustmentDetail where AdjustmentId=@AdjustmentId
	 and ToQty<>Qty)>0)
	 begin
	     ---有数据没有处理完成，数据回退
	      ROLLBACK TRANSACTION;
	 end
	  
	 else  begin
	 ------明细全部处理完成，提交
	     update WMS_Adjustment set AdjustmentStatus=99 where id=@AdjustmentId 
		 
	   COMMIT;
	 end
	   select * from @Information

  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -3
  BEGIN
  IF Cursor_status('local', 'cursor_adjustmentDetail') <> -1
    BEGIN
        CLOSE cursor_adjustmentDetail;

        DEALLOCATE cursor_adjustmentDetail;
    END;
  END;
  END try

      BEGIN catch
          IF Xact_state() <> 0
            BEGIN
                ROLLBACK TRANSACTION;
            END;

          CLOSE cursor_inventory;

          DEALLOCATE cursor_inventory;

          CLOSE cursor_adjustmentDetail --关闭游标

          DEALLOCATE cursor_adjustmentDetail --释放游标

          DECLARE @ERRORMessage NVARCHAR(max);

          SELECT @ERRORMessage = Error_message();

          RAISERROR( @ERRORMessage,16,1 );
      END catch;



	 
  END 

");
        db.Ado.GetLong(procSqlAdjustmentConfirmUnfreeze.ToString(), null);
        //db.Ado.GetLong(procSql.ToString(),null);
        //创建数据库存储过程
        //db.Insertable<TableColumnsDetail>(tableColumnsDetails);


    }
}