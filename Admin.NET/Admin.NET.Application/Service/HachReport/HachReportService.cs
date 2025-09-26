// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Const;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service.HachReport.Dto;
using Admin.NET.Application.Service.HachDashBoardConfig.Dto;
using Nest;
using System.Data;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinTagsMembersGetBlackListResponse.Types;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.IO;

namespace Admin.NET.Application.Service.HachReport;
/// <summary>
/// HachReport  哈希报表
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class HachReportService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSHachTagretKRMB> _rep;
    private readonly HachDashBoardService _hachDashBoardService;
    public HachReportService(SqlSugarRepository<WMSHachTagretKRMB> rep, HachDashBoardService hachDashBoardService)
    {
        _rep = rep;
        _hachDashBoardService = hachDashBoardService;
    }
    /// <summary>
    /// 客户下拉
    /// </summary>
    /// <returns></returns>
    public async Task<List<SelectItem>> CustomerSelectList()
    {
        List<SelectItem> selectItems = new List<SelectItem>();
        try
        {
            var sql = $@"SELECT CustomerId AS Id,CustomerName as Label,CustomerId as Value FROM WMS_Hach_Customer_Mapping WHERE TYPE='HachReport'";
            selectItems = _rep.Context.Ado.GetDataTable(sql).TableToList<SelectItem>();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return selectItems;
    }

    #region  经销商配件库存健康度
    /// <summary>
    /// 查询经销商配件库存健康度
    /// </summary>
    [ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1000)]
    /// <summary>
    /// Operational Tracker   经销商配件库存健康度2.5X Month
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<WmsHachTagretKRMBOutput> OperationalTrackerList(WmsHachTagretKRMBInput input)
    {
        WmsHachTagretKRMBOutput data = new WmsHachTagretKRMBOutput();
        try
        {
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and  customerId = " + input.CustomerId + "";
            }
            var reverseMonthMap = _hachDashBoardService.MonthNameMap.ToDictionary(x => x.Value, x => x.Key);
            var sql = $@" DECLARE @YearFilter INT = {DateTime.Now.Year}; 
                DECLARE @sqlPlan NVARCHAR(MAX) = '';
                DECLARE @sqlActual NVARCHAR(MAX) = '';
                DECLARE @month NVARCHAR(10);
                DECLARE @monthDisplay NVARCHAR(10);

                -- 创建临时表存储月份
                CREATE TABLE #Months (Month NVARCHAR(10), MonthDisplay NVARCHAR(10));

                -- 使用递归查询生成12个月份
                WITH MonthSeq AS (
                SELECT 1 AS MonthNum UNION ALL SELECT MonthNum + 1 FROM MonthSeq WHERE MonthNum < 12)
                INSERT INTO #Months (Month, MonthDisplay)
                SELECT  CONCAT(@YearFilter, '-', RIGHT('00' + CAST(MonthNum AS VARCHAR(2)), 2)) AS Month,
                RIGHT('00' + CAST(MonthNum AS VARCHAR(2)), 2) AS MonthDisplay FROM MonthSeq
                OPTION (MAXRECURSION 12);  -- 限制递归深度为12，避免无限递归

                -- 初始化游标
                DECLARE month_cursor CURSOR FOR SELECT Month, MonthDisplay FROM #Months;

                -- 第一次遍历：处理 PlanKRMB
                OPEN month_cursor; 
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                SET @sqlPlan = @sqlPlan + 'MAX(CASE WHEN t.Month = ''' + @month + ''' THEN COALESCE(t.PlanKRMB, ''未维护'') END) AS ''' + @monthDisplay + ''', ';
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;
                END
                CLOSE month_cursor;

                -- 去掉 PlanKRMB 部分的最后一个逗号
                IF LEN(@sqlPlan) > 0
                SET @sqlPlan = LEFT(@sqlPlan, LEN(@sqlPlan) - 1);

                -- 第二次遍历：处理 ActualKRMB
                OPEN month_cursor;
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                SET @sqlActual = @sqlActual + 'MAX(CASE WHEN t.Month = ''' + @month + ''' THEN COALESCE(t.ActualKRMB, ''未维护'') END) AS ''' + @monthDisplay + ''', ';
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;
                END

                CLOSE month_cursor;
                DEALLOCATE month_cursor;

                -- 去掉 ActualKRMB 部分的最后一个逗号
                IF LEN(@sqlActual) > 0
                SET @sqlActual = LEFT(@sqlActual, LEN(@sqlActual) - 1);

                -- 构建最终 SQL
                DECLARE @finalSql NVARCHAR(MAX) = N'
                SELECT ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20))) + ''库存金额（KRMB）'' AS CustomerName, ''Plan'' AS type, 
                MAX(t.YTDPlanKRMB) as YTD, ' + @sqlPlan + 'FROM (SELECT CustomerId, Month, YTDPlanKRMB, PlanKRMB, ActualKRMB FROM WMS_HachTagretKRMB 
                WHERE LEFT(Month, 4) = ''' + CAST(@YearFilter AS VARCHAR(4)) + ''' 
                AND CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''HachReport'' {sqlWhereSql})) t
                RIGHT JOIN #Months m  ON t.Month = m.Month
                LEFT JOIN WMS_Hach_Customer_Mapping c ON t.CustomerId = c.CustomerId AND c.type = ''HachReport''
                 where customerName is not null

                GROUP BY ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20)))
                UNION ALL
                SELECT ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20))) + ''库存金额（KRMB）'' AS CustomerName, 
                ''Actual'' AS type, MAX(t.YTDActualKRMB) as YTD, ' + @sqlActual + '
                FROM (SELECT CustomerId, Month, YTDActualKRMB, PlanKRMB, ActualKRMB
                FROM WMS_HachTagretKRMB  
                WHERE
                LEFT(Month, 4) = ''' + CAST(@YearFilter AS VARCHAR(4)) + '''
                AND CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''HachReport'' {sqlWhereSql})) t
                RIGHT JOIN #Months m 
                ON t.Month = m.Month LEFT JOIN WMS_Hach_Customer_Mapping c 
                ON t.CustomerId = c.CustomerId AND c.type = ''HachReport''
                 where customerName is not null

                GROUP BY ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20)))
                ORDER BY CustomerName, type, YTD;';

                EXEC sp_executesql @finalSql;

                -- 删除临时表
                DROP TABLE #Months;
                ";

            data.data = _rep.Context.Ado.GetDataTable(sql);
            if (data.data != null && data.data.Rows.Count > 0)
            {
                // 创建反向映射字典（数字到英文缩写）
                // 遍历所有列名
                foreach (DataColumn column in data.data.Columns)
                {
                    // 检查是否是月份列（两位数字）
                    if (column.ColumnName.Length == 2 && column.ColumnName.All(char.IsDigit))
                    {
                        if (reverseMonthMap.TryGetValue(column.ColumnName, out var monthAbbr))
                        {
                            // 修改列名为英文缩写
                            column.ColumnName = monthAbbr;
                        }
                    }
                }
            }
            // 计算总条数
            data.Total = data.data.Rows.Count;

            // 分页处理
            var skip = (input.Page - 1) * input.PageSize;
            var pagedData = data.data.AsEnumerable().Skip(skip).Take(input.PageSize).CopyToDataTable();
            data.data = pagedData;

            // 计算分页的总页数
            data.Page = input.Page;
            data.PageSize = input.PageSize;
            data.TotalPages = (int)Math.Ceiling((double)data.Total / input.PageSize);

        }
        catch (Exception ex)
        {
            throw;
        }
        return data;
    }
    /// <summary>
    /// 导出经销商配件库存健康度
    /// </summary>
    [ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1000)]
    /// <summary>
    /// Operational Tracker   经销商配件库存健康度2.5X Month
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<ActionResult> ExportOperationalTrackerList(WmsHachTagretKRMBInput input)
    {
        WmsHachTagretKRMBOutput data = new WmsHachTagretKRMBOutput();
        IExporter exporter = new ExcelExporter();
        try
        {
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and  customerId = " + input.CustomerId + "";
            }
            var reverseMonthMap = _hachDashBoardService.MonthNameMap.ToDictionary(x => x.Value, x => x.Key);
            var sql = $@" DECLARE @YearFilter INT = {DateTime.Now.Year}; 
                DECLARE @sqlPlan NVARCHAR(MAX) = '';
                DECLARE @sqlActual NVARCHAR(MAX) = '';
                DECLARE @month NVARCHAR(10);
                DECLARE @monthDisplay NVARCHAR(10);

                -- 创建临时表存储月份
                CREATE TABLE #Months (Month NVARCHAR(10), MonthDisplay NVARCHAR(10));

                -- 使用递归查询生成12个月份
                WITH MonthSeq AS (
                SELECT 1 AS MonthNum UNION ALL SELECT MonthNum + 1 FROM MonthSeq WHERE MonthNum < 12)
                INSERT INTO #Months (Month, MonthDisplay)
                SELECT  CONCAT(@YearFilter, '-', RIGHT('00' + CAST(MonthNum AS VARCHAR(2)), 2)) AS Month,
                RIGHT('00' + CAST(MonthNum AS VARCHAR(2)), 2) AS MonthDisplay FROM MonthSeq
                OPTION (MAXRECURSION 12);  -- 限制递归深度为12，避免无限递归

                -- 初始化游标
                DECLARE month_cursor CURSOR FOR SELECT Month, MonthDisplay FROM #Months;

                -- 第一次遍历：处理 PlanKRMB
                OPEN month_cursor; 
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                SET @sqlPlan = @sqlPlan + 'MAX(CASE WHEN t.Month = ''' + @month + ''' THEN COALESCE(t.PlanKRMB, ''未维护'') END) AS ''' + @monthDisplay + ''', ';
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;
                END
                CLOSE month_cursor;

                -- 去掉 PlanKRMB 部分的最后一个逗号
                IF LEN(@sqlPlan) > 0
                SET @sqlPlan = LEFT(@sqlPlan, LEN(@sqlPlan) - 1);

                -- 第二次遍历：处理 ActualKRMB
                OPEN month_cursor;
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                SET @sqlActual = @sqlActual + 'MAX(CASE WHEN t.Month = ''' + @month + ''' THEN COALESCE(t.ActualKRMB, ''未维护'') END) AS ''' + @monthDisplay + ''', ';
                FETCH NEXT FROM month_cursor INTO @month, @monthDisplay;
                END

                CLOSE month_cursor;
                DEALLOCATE month_cursor;

                -- 去掉 ActualKRMB 部分的最后一个逗号
                IF LEN(@sqlActual) > 0
                SET @sqlActual = LEFT(@sqlActual, LEN(@sqlActual) - 1);

                -- 构建最终 SQL
                DECLARE @finalSql NVARCHAR(MAX) = N'
                SELECT ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20))) + ''库存金额（KRMB）'' AS CustomerName, ''Plan'' AS type, 
                MAX(t.YTDPlanKRMB) as YTD, ' + @sqlPlan + 'FROM (SELECT CustomerId, Month, YTDPlanKRMB, PlanKRMB, ActualKRMB FROM WMS_HachTagretKRMB 
                WHERE LEFT(Month, 4) = ''' + CAST(@YearFilter AS VARCHAR(4)) + ''' 
                AND CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''HachReport'' {sqlWhereSql})) t
                RIGHT JOIN #Months m  ON t.Month = m.Month
                LEFT JOIN WMS_Hach_Customer_Mapping c ON t.CustomerId = c.CustomerId AND c.type = ''HachReport''
                 where customerName is not null

                GROUP BY ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20)))
                UNION ALL
                SELECT ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20))) + ''库存金额（KRMB）'' AS CustomerName, 
                ''Actual'' AS type, MAX(t.YTDActualKRMB) as YTD, ' + @sqlActual + '
                FROM (SELECT CustomerId, Month, YTDActualKRMB, PlanKRMB, ActualKRMB
                FROM WMS_HachTagretKRMB  
                WHERE
                LEFT(Month, 4) = ''' + CAST(@YearFilter AS VARCHAR(4)) + '''
                AND CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''HachReport'' {sqlWhereSql})) t
                RIGHT JOIN #Months m 
                ON t.Month = m.Month LEFT JOIN WMS_Hach_Customer_Mapping c 
                ON t.CustomerId = c.CustomerId AND c.type = ''HachReport''
                 where customerName is not null

                GROUP BY ISNULL(c.CustomerName, CAST(t.CustomerId AS NVARCHAR(20)))
                ORDER BY CustomerName, type, YTD;';

                EXEC sp_executesql @finalSql;

                -- 删除临时表
                DROP TABLE #Months;
                ";
            data.data = _rep.Context.Ado.GetDataTable(sql);
            if (data.data != null && data.data.Rows.Count > 0)
            {
                // 创建反向映射字典（数字到英文缩写）
                // 遍历所有列名
                foreach (DataColumn column in data.data.Columns)
                {
                    // 检查是否是月份列（两位数字）
                    if (column.ColumnName.Length == 2 && column.ColumnName.All(char.IsDigit))
                    {
                        if (reverseMonthMap.TryGetValue(column.ColumnName, out var monthAbbr))
                        {
                            // 修改列名为英文缩写
                            column.ColumnName = monthAbbr;
                        }
                    }
                }
            }
            var result = exporter.ExportAsByteArray<DataTable>(data.data);
            var fs = new MemoryStream(result.Result);
            return new FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = "Parts Traceability Tracking.xlsx" // 配置文件下载显示名
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion

    #region  经销商配件
    /// <summary>
    /// 查询经销商配件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1000)]
    public async Task<WmsHachTagretKRMBOutput> OperationalTrackerSellThruList(WmsHachTagretKRMBInput input)
    {
        WmsHachTagretKRMBOutput data = new WmsHachTagretKRMBOutput();
        try
        {
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and  customerId = " + input.CustomerId + "";
            }

            var customerList = await CustomerSelectList();
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                customerList = customerList.Where(a => a.Id == input.CustomerId).ToList();
            }
            DataTable mergedData = new DataTable();
            foreach (var item in customerList)
            {
                var sql = $@"
                            DECLARE @Year INT = YEAR(GETDATE()); -- 动态获取当前年份
                            DECLARE @SQL NVARCHAR(MAX);
                            DECLARE @PivotColumns NVARCHAR(MAX) = '';
                            DECLARE @PivotSelectColumns NVARCHAR(MAX) = '';
                            
                            -- 动态生成Week1到Week53的列名
                            SELECT @PivotColumns = @PivotColumns + '[Week' + CAST(Number AS VARCHAR) + '],',
                            @PivotSelectColumns = @PivotSelectColumns + 'ISNULL(Outbound.[Week' + CAST(Number AS VARCHAR) + '], 0) / NULLIF(ISNULL(Inbound.[Week' + CAST(Number AS VARCHAR) + '], 0), 0) AS [Week' + CAST(Number AS VARCHAR) + '],'
                            FROM master.dbo.spt_values
                            WHERE type = 'P' AND number BETWEEN 1 AND 53
                            ORDER BY Number;
                            
                            -- 移除最后一个逗号
                            SET @PivotColumns = LEFT(@PivotColumns, LEN(@PivotColumns) - 1);
                            SET @PivotSelectColumns = LEFT(@PivotSelectColumns, LEN(@PivotSelectColumns) - 1);
                            
                            -- 构建动态SQL
                            SET @SQL = N'
                            -- 入库部分
                            WITH Inbound_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year,
                            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Inbound_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Receipt WHERE CustomerId = {item.Id}),
                            Inbound_AggregatedData AS (SELECT r.CustomerName, ''Actual'' AS Type,SUM(ReceivedQty * p.Price) AS TotalAmount,
                            DATEPART(YEAR, r.CreationTime) AS Year,DATEPART(WEEK, r.CreationTime) AS Week FROM WMS_Receipt r
                            LEFT JOIN  WMS_ReceiptDetail rd ON r.id = rd.ReceiptId
                            LEFT JOIN  WMS_Product p ON p.CustomerId = r.CustomerId AND rd.SKU = p.SKU
                            WHERE r.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'') AND r.ReceiptStatus NOT IN (-1, 1)
                            GROUP BY r.CustomerId, r.CustomerName,DATEPART(YEAR, r.CreationTime),DATEPART(WEEK, r.CreationTime)),
                            Inbound_Result AS (SELECT CustomerName + ''入库金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + 'FROM (
                            SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Inbound_WeekData wd CROSS JOIN  Inbound_CustomerData cd
                            LEFT JOIN Inbound_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 出库部分
                            Outbound_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week 
                            FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Outbound_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Order WHERE CustomerId = {item.Id}),
                            Outbound_AggregatedData AS (
                            SELECT o.CustomerName,''Actual'' AS Type,SUM(AllocatedQty * p.Price) AS TotalAmount,DATEPART(YEAR, o.CreationTime) AS Year,
                            DATEPART(WEEK, o.CreationTime) AS Week FROM 
                            WMS_Order o LEFT JOIN  WMS_OrderDetail od ON o.id = od.OrderId
                            LEFT JOIN   WMS_Product p ON p.CustomerId = o.CustomerId AND od.SKU = p.SKU
                            WHERE o.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'')
                            AND o.orderstatus NOT IN (-1, 1) GROUP BY o.CustomerId, 
                            o.CustomerName,DATEPART(YEAR, o.CreationTime),DATEPART(WEEK, o.CreationTime)),
                            Outbound_Result AS (SELECT CustomerName + ''出库金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + '
                            FROM (SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Outbound_WeekData wd CROSS JOIN  Outbound_CustomerData cd
                            LEFT JOIN Outbound_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 库存部分
                            Inventory_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week  
                            FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Inventory_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Inventory_Usable WHERE CustomerId = {item.Id}),
                            Inventory_AggregatedData AS (
                            SELECT o.CustomerName,''Actual'' AS Type,SUM(Qty * p.Price) AS TotalAmount,DATEPART(YEAR, o.CreationTime) AS Year,DATEPART(WEEK, o.CreationTime) AS Week
                            FROM WMS_Inventory_Usable o LEFT JOIN  WMS_Product p ON p.CustomerId = o.CustomerId AND o.SKU = p.SKU
                            WHERE o.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'')
                            AND o.inventorystatus NOT IN (99)
                            GROUP BY o.CustomerId, o.CustomerName,DATEPART(YEAR, o.CreationTime),DATEPART(WEEK, o.CreationTime)),
                            Inventory_Result AS (SELECT CustomerName + ''库存金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + '
                            FROM (SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Inventory_WeekData wd CROSS JOIN  Inventory_CustomerData cd LEFT JOIN 
                            Inventory_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 出库/入库比率部分
                            Ratio_Result AS (SELECT Outbound.CustomerName + '' Sell Thru '' as CustomerName,
                            ''Actual'' as Type,Outbound.Year,' + @PivotSelectColumns + '
                            FROM Outbound_Result Outbound CROSS JOIN Inbound_Result Inbound
                            WHERE REPLACE(Outbound.CustomerName, ''出库金额（KRMB）'', '''') = REPLACE(Inbound.CustomerName, ''入库金额（KRMB）'', ''''))
                            -- 合并四个结果集
                            SELECT * FROM Ratio_Result UNION ALL
                            SELECT * FROM Inbound_Result UNION ALL
                            SELECT * FROM Outbound_Result UNION ALL
                            SELECT * FROM Inventory_Result
                            '; EXEC sp_executesql @SQL;";
                var result = _rep.Context.Ado.GetDataTable(sql);
                // 第一次循环时，将mergedData的结构设置为与result相同
                if (mergedData.Columns.Count == 0)
                {
                    mergedData = result.Clone();
                }

                // 将result中的数据行添加到mergedData中
                foreach (DataRow row in result.Rows)
                {
                    mergedData.ImportRow(row);
                }
            }
            data.data = mergedData;
            // 计算总条数
            data.Total = data.data.Rows.Count;
            // 分页处理
            var skip = (input.Page - 1) * input.PageSize;
            var pagedData = data.data.AsEnumerable().Skip(skip).Take(input.PageSize).CopyToDataTable();
            data.data = pagedData;
            // 计算分页的总页数
            data.Page = input.Page;
            data.PageSize = input.PageSize;
            data.TotalPages = (int)Math.Ceiling((double)data.Total / input.PageSize);
        }
        catch (Exception ex)
        {
            throw;
        }
        return data;
    }
    /// <summary>
    /// 导出经销商配件
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(ApplicationConst.GroupName, Order = 1000)]
    public async Task<ActionResult>  ExportOperationalTrackerSellThruList(WmsHachTagretKRMBInput input)
    {
        WmsHachTagretKRMBOutput data = new WmsHachTagretKRMBOutput();
        IExporter exporter = new ExcelExporter();
        try
        {
            var sqlWhereSql = string.Empty;
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                sqlWhereSql = "and  customerId = " + input.CustomerId + "";
            }

            var customerList = await CustomerSelectList();
            if (input.CustomerId.HasValue && input.CustomerId > 0)
            {
                customerList = customerList.Where(a => a.Id == input.CustomerId).ToList();
            }
            DataTable mergedData = new DataTable();
            foreach (var item in customerList)
            {
                var sql = $@"
                            DECLARE @Year INT = YEAR(GETDATE()); -- 动态获取当前年份
                            DECLARE @SQL NVARCHAR(MAX);
                            DECLARE @PivotColumns NVARCHAR(MAX) = '';
                            DECLARE @PivotSelectColumns NVARCHAR(MAX) = '';
                            
                            -- 动态生成Week1到Week53的列名
                            SELECT @PivotColumns = @PivotColumns + '[Week' + CAST(Number AS VARCHAR) + '],',
                            @PivotSelectColumns = @PivotSelectColumns + 'ISNULL(Outbound.[Week' + CAST(Number AS VARCHAR) + '], 0) / NULLIF(ISNULL(Inbound.[Week' + CAST(Number AS VARCHAR) + '], 0), 0) AS [Week' + CAST(Number AS VARCHAR) + '],'
                            FROM master.dbo.spt_values
                            WHERE type = 'P' AND number BETWEEN 1 AND 53
                            ORDER BY Number;
                            
                            -- 移除最后一个逗号
                            SET @PivotColumns = LEFT(@PivotColumns, LEN(@PivotColumns) - 1);
                            SET @PivotSelectColumns = LEFT(@PivotSelectColumns, LEN(@PivotSelectColumns) - 1);
                            
                            -- 构建动态SQL
                            SET @SQL = N'
                            -- 入库部分
                            WITH Inbound_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year,
                            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Inbound_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Receipt WHERE CustomerId = {item.Id}),
                            Inbound_AggregatedData AS (SELECT r.CustomerName, ''Actual'' AS Type,SUM(ReceivedQty * p.Price) AS TotalAmount,
                            DATEPART(YEAR, r.CreationTime) AS Year,DATEPART(WEEK, r.CreationTime) AS Week FROM WMS_Receipt r
                            LEFT JOIN  WMS_ReceiptDetail rd ON r.id = rd.ReceiptId
                            LEFT JOIN  WMS_Product p ON p.CustomerId = r.CustomerId AND rd.SKU = p.SKU
                            WHERE r.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'') AND r.ReceiptStatus NOT IN (-1, 1)
                            GROUP BY r.CustomerId, r.CustomerName,DATEPART(YEAR, r.CreationTime),DATEPART(WEEK, r.CreationTime)),
                            Inbound_Result AS (SELECT CustomerName + ''入库金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + 'FROM (
                            SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Inbound_WeekData wd CROSS JOIN  Inbound_CustomerData cd
                            LEFT JOIN Inbound_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 出库部分
                            Outbound_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week 
                            FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Outbound_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Order WHERE CustomerId = {item.Id}),
                            Outbound_AggregatedData AS (
                            SELECT o.CustomerName,''Actual'' AS Type,SUM(AllocatedQty * p.Price) AS TotalAmount,DATEPART(YEAR, o.CreationTime) AS Year,
                            DATEPART(WEEK, o.CreationTime) AS Week FROM 
                            WMS_Order o LEFT JOIN  WMS_OrderDetail od ON o.id = od.OrderId
                            LEFT JOIN   WMS_Product p ON p.CustomerId = o.CustomerId AND od.SKU = p.SKU
                            WHERE o.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'')
                            AND o.orderstatus NOT IN (-1, 1) GROUP BY o.CustomerId, 
                            o.CustomerName,DATEPART(YEAR, o.CreationTime),DATEPART(WEEK, o.CreationTime)),
                            Outbound_Result AS (SELECT CustomerName + ''出库金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + '
                            FROM (SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Outbound_WeekData wd CROSS JOIN  Outbound_CustomerData cd
                            LEFT JOIN Outbound_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 库存部分
                            Inventory_WeekData AS (SELECT ' + CAST(@Year AS VARCHAR) + ' AS Year, ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS Week  
                            FROM master.dbo.spt_values WHERE type = ''P'' AND number <= 53),
                            Inventory_CustomerData AS (SELECT DISTINCT CustomerName FROM WMS_Inventory_Usable WHERE CustomerId = {item.Id}),
                            Inventory_AggregatedData AS (
                            SELECT o.CustomerName,''Actual'' AS Type,SUM(Qty * p.Price) AS TotalAmount,DATEPART(YEAR, o.CreationTime) AS Year,DATEPART(WEEK, o.CreationTime) AS Week
                            FROM WMS_Inventory_Usable o LEFT JOIN  WMS_Product p ON p.CustomerId = o.CustomerId AND o.SKU = p.SKU
                            WHERE o.CustomerId IN (SELECT CustomerId FROM WMS_Hach_Customer_Mapping WHERE type = ''hachReport'')
                            AND o.inventorystatus NOT IN (99)
                            GROUP BY o.CustomerId, o.CustomerName,DATEPART(YEAR, o.CreationTime),DATEPART(WEEK, o.CreationTime)),
                            Inventory_Result AS (SELECT CustomerName + ''库存金额（KRMB）'' as CustomerName,Type,Year,' + @PivotColumns + '
                            FROM (SELECT cd.CustomerName,''Actual'' as Type,wd.Year,''Week'' + CAST(wd.Week AS VARCHAR) AS WeekColumn,ISNULL(ad.TotalAmount, 0) AS TotalAmount
                            FROM  Inventory_WeekData wd CROSS JOIN  Inventory_CustomerData cd LEFT JOIN 
                            Inventory_AggregatedData ad ON ad.Week = wd.Week AND ad.CustomerName = cd.CustomerName AND ad.Year = wd.Year) AS SourceData
                            PIVOT (MAX(TotalAmount)FOR WeekColumn IN (' + @PivotColumns + ')) AS PivotTable),
                            -- 出库/入库比率部分
                            Ratio_Result AS (SELECT Outbound.CustomerName + ''出库/入库比率'' as CustomerName,
                            ''Actual'' as Type,Outbound.Year,' + @PivotSelectColumns + '
                            FROM Outbound_Result Outbound CROSS JOIN Inbound_Result Inbound
                            WHERE REPLACE(Outbound.CustomerName, ''出库金额（KRMB）'', '''') = REPLACE(Inbound.CustomerName, ''入库金额（KRMB）'', ''''))
                            -- 合并四个结果集
                            SELECT * FROM Ratio_Result UNION ALL
                            SELECT * FROM Inbound_Result UNION ALL
                            SELECT * FROM Outbound_Result UNION ALL
                            SELECT * FROM Inventory_Result
                            '; EXEC sp_executesql @SQL;";
                var dataTable = _rep.Context.Ado.GetDataTable(sql);
                // 第一次循环时，将mergedData的结构设置为与result相同
                if (mergedData.Columns.Count == 0)
                {
                    mergedData = dataTable.Clone();
                }

                // 将result中的数据行添加到mergedData中
                foreach (DataRow row in dataTable.Rows)
                {
                    mergedData.ImportRow(row);
                }
            }
            data.data = mergedData;
            var result = exporter.ExportAsByteArray<DataTable>(data.data);
            var fs = new MemoryStream(result.Result);
            return new FileStreamResult(fs, "application/octet-stream")
            {
                FileDownloadName = "Parts Traceability Tracking Sell Thru.xlsx" // 配置文件下载显示名
            };
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion
}
