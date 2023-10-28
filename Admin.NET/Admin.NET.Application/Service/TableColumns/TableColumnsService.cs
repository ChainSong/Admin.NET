using Admin.NET.Application.Const;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using FluentEmail.Core;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.Caching.Distributed;
using NewLife.Net;
using NewLife.Reflection;
using NPOI.HPSF;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.ComponentTCBCreateCloudBaseRunServerVersionRequest.Types.Code.Types;

namespace Admin.NET.Application;
/// <summary>
/// 表管理服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class TableColumnsService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<TableColumns> _rep;
    private readonly SqlSugarRepository<TableColumnsDetail> _repDetail;
    private readonly UserManager _userManager;
    private readonly ISqlSugarClient _db;
    //private readonly UserService _userService;

    private readonly SysCacheService _sysCacheService;
    public TableColumnsService(SqlSugarRepository<TableColumns> rep, SqlSugarRepository<TableColumnsDetail> repDetail, SysCacheService sysCacheService, UserManager userManager, ISqlSugarClient db)
    {
        _rep = rep;
        _repDetail = repDetail;
        _sysCacheService = sysCacheService;
        _userManager = userManager;
        _db = db;
    }

    /// <summary>
    /// 分页查询表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<TableColumnsOutput>> Page(TableColumnsInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(input.ProjectId > 0, u => u.ProjectId == input.ProjectId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RoleName), u => u.RoleName.Contains(input.RoleName.Trim()))
                    .WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TableName), u => u.TableName.Contains(input.TableName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.TableNameCH), u => u.TableNameCH.Contains(input.TableNameCH.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DisplayName), u => u.DisplayName.Contains(input.DisplayName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DbColumnName), u => u.DbColumnName.Contains(input.DbColumnName.Trim()))
                    .WhereIF(input.IsKey > 0, u => u.IsKey == input.IsKey)
                    .WhereIF(input.IsSearchCondition > 0, u => u.IsSearchCondition == input.IsSearchCondition)
                    .WhereIF(input.IsHide > 0, u => u.IsHide == input.IsHide)
                    .WhereIF(input.IsReadOnly > 0, u => u.IsReadOnly == input.IsReadOnly)
                    .WhereIF(input.IsShowInList > 0, u => u.IsShowInList == input.IsShowInList)
                    .WhereIF(input.IsImportColumn > 0, u => u.IsImportColumn == input.IsImportColumn)
                    .WhereIF(input.IsDropDownList > 0, u => u.IsDropDownList == input.IsDropDownList)
                    .WhereIF(input.IsCreate > 0, u => u.IsCreate == input.IsCreate)
                    .WhereIF(input.IsUpdate > 0, u => u.IsUpdate == input.IsUpdate)
                    .WhereIF(input.SearchConditionOrder > 0, u => u.SearchConditionOrder == input.SearchConditionOrder)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Validation), u => u.Validation.Contains(input.Validation.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Group), u => u.Group.Contains(input.Group.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Type), u => u.Type.Contains(input.Type.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Characteristic), u => u.Characteristic.Contains(input.Characteristic.Trim()))
                    .WhereIF(input.Order > 0, u => u.Order == input.Order)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Associated), u => u.Associated.Contains(input.Associated.Trim()))
                    .WhereIF(input.Precision > 0, u => u.Precision == input.Precision)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Default), u => u.Default.Contains(input.Default.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Link), u => u.Link.Contains(input.Link.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.RelationDBColumn), u => u.RelationDBColumn.Contains(input.RelationDBColumn.Trim()))
                    .WhereIF(input.ForView > 0, u => u.ForView == input.ForView)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))

                    .Select<TableColumnsOutput>()
;
        if (input.CreationTimeRange != null && input.CreationTimeRange.Length > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTimeRange.Length > 1)
            {
                var end = input.CreationTimeRange[1].AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }



    /// <summary>
    /// 查询不分页表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "PageAll")]
    public async Task<SqlSugarPagedList<TableColumnsOutput>> PageAll(TableColumnsInput input)
    {

        var query = _rep.AsQueryable()
        .WhereIF(!input.RoleName.IsNullOrEmpty(), a =>
                      //模糊搜索RoleName
                      a.RoleName.Contains(input.RoleName))
        .WhereIF(!input.TableName.IsNullOrEmpty(), a =>

                      //模糊搜索TableName
                      a.TableName.Contains(input.TableName))
        .WhereIF(_userManager.TenantId != 0, a =>
                        //模糊搜索TenantId
                        a.TenantId == (_userManager.TenantId))
         .WhereIF(input.CustomerId != 0, a =>
                        //模糊搜索CustomerId
                        a.CustomerId == (input.CustomerId))
        .WhereIF(!input.RoleName.IsNullOrEmpty(), a =>
                      //模糊搜索RoleName
                      a.RoleName.Contains(input.RoleName)
        ).Select(a => new TableColumnsOutput { CustomerId = a.CustomerId, RoleName = a.RoleName, TableName = a.TableName, TableNameCH = a.TableNameCH }).Distinct().OrderBy(a => a.CustomerId);
        // TODO:根据传入的参数添加过滤条件

        //if (input.CreationTimeRange != null && input.CreationTimeRange.Length > 0)
        //{
        //    DateTime? start = input.CreationTimeRange[0];
        //    query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
        //    if (input.CreationTimeRange.Length > 1)
        //    {
        //        var end = input.CreationTimeRange[1].AddDays(1);
        //        query = query.Where(u => u.CreationTime < end);
        //    }
        //}
        query = query.OrderBuilder(input, "", "TableName");
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }



    /// <summary>
    /// 通过指定表名获取Table_ColumnsListDto信息
    /// </summary>
    [HttpGet]
    [ApiDescriptionSettings(Name = "GetByTableNameList")]
    public List<TableColumnsOutput> GetByTableNameList(string TableName)
    {
        TableColumnsInput tableColumns = new TableColumnsInput();
        tableColumns.TableName = TableName;
        tableColumns.TenantId = _userManager.TenantId;
        int count = 0;
        //List<TableColumnsOutput> tableColumnsList=new List<TableColumnsOutput>();
        //input.Order = "Order";
        //input.TenantId = AbpSession.TenantId ?? 0;
        //var table_ColumnsList = _cacheManage.GetCache("TableColumn").Get("TableColumn_" + input.TableName + "_" + AbpSession.RoleName + "", (val) =>
        //  {
        //      return val;
        //  }) as List<Table_Columns>;
        var tableColumnsList = _sysCacheService.Get<List<TableColumnsOutput>>("TableColumn_" + tableColumns.TableName + "_" + tableColumns.TenantId + "_" + tableColumns.CustomerId);

        if (tableColumnsList != null && tableColumnsList.Count() > 0)
        {
            //count = table_ColumnsList.Count();
        }
        else
        {
            tableColumnsList = _sysCacheService.Get<List<TableColumnsOutput>>("TableColumn_" + tableColumns.TableName + "_" + tableColumns.TenantId + "_" + tableColumns.CustomerId);
            if (tableColumnsList != null && tableColumnsList.Count() > 0)
            {
            }
            else
            {
                tableColumnsList = GetAll(tableColumns).Result;
            }
        }
        count = tableColumnsList.Count();


        //var customerInfoListDtos = ObjectMapper.Map<List<Table_ColumnsListDto>>(table_ColumnsList);
        //var aaaa = new PagedResultDto<Table_ColumnsListDto>(count, customerInfoListDtos);
        return tableColumnsList;

    }

    /// <summary>
    /// 获取的不分页列表信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetAll")]
    //[AbpAuthorize(Table_ColumnsPermissions.Table_Columns_Query)]
    public async Task<List<TableColumnsOutput>> GetAll(TableColumnsInput input)
    {

        var query = _rep.AsQueryable()
        .WhereIF(!input.RoleName.IsNullOrEmpty(), a =>
                      //模糊搜索RoleName
                      a.RoleName == (input.RoleName))
        .WhereIF(!input.TableName.IsNullOrEmpty(), a =>

                      //模糊搜索TableName
                      a.TableName == (input.TableName))
        .WhereIF(input.TenantId != 0, a =>
                        //模糊搜索TenantId
                        a.TenantId == (input.TenantId))
         .WhereIF(input.CustomerId != 0, a =>
                        //模糊搜索CustomerId
                        a.CustomerId == (input.CustomerId))
        .WhereIF(!input.RoleName.IsNullOrEmpty(), a =>
                      //模糊搜索RoleName
                      a.RoleName == (input.RoleName)
        ).Select(a => new TableColumnsOutput
        {
            Id = a.Id
          //,TenantId = a.TenantId
          ,
            ProjectId = a.ProjectId
          ,
            RoleName = a.RoleName
          ,
            CustomerId = a.CustomerId
          ,
            TableName = a.TableName
          ,
            TableNameCH = a.TableNameCH
          ,
            DisplayName = a.DisplayName
          ,
            //由于框架约定大于配置， 数据库的字段首字母小写
            //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
            DbColumnName = a.DbColumnName
          ,
            IsKey = a.IsKey
          ,
            IsSearchCondition = a.IsSearchCondition
          ,
            IsHide = a.IsHide
          ,
            IsReadOnly = a.IsReadOnly
          ,
            IsShowInList = a.IsShowInList
          ,
            IsImportColumn = a.IsImportColumn
          ,
            IsDropDownList = a.IsDropDownList
          ,
            Associated = a.Associated

            ,
            TableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1).OrderBy(b => b.Order).ToList()
            ,
            IsCreate = a.IsCreate
          ,
            IsUpdate = a.IsUpdate
          ,
            SearchConditionOrder = a.SearchConditionOrder
          ,
            Validation = a.Validation
          ,
            Group = a.Group
          ,
            Default = a.Default,
            RelationDBColumn = a.RelationDBColumn,
            Type = a.Type,
            Order = a.Order,
            Precision = a.Precision,
            Step = a.Step,
            Max = a.Max,
            Min = a.Min,
            Link = a.Link,
            ForView = a.ForView
        }).OrderBy(a => a.Order);
        //.Mapper(c => {
        //    c.TableColumnsDetail = _repDetail.AsQueryable().Where(b => b.Associated == c.Associated).ToList();
        //});
        // .Includes(v => v.TableColumnsDetail == _repDetail.AsQueryable().Where(b => b.Associated == v.Associated).Select(b =>
        //       new TableColumnsDetail
        //       {
        //           Id = b.Id,
        //           CodeInt = b.CodeInt,
        //           CodeStr = b.CodeStr,
        //           Name = b.Name,
        //           Type = b.Type,
        //           Color = b.Color,
        //           Associated = b.Associated,
        //           Status = b.Status,
        //           Creator = b.Creator,
        //           CreationTime = b.CreationTime
        //       }
        //     ).ToList());
        // var asdasd = _repDetail.AsQueryable().Where(a => a.Status == 1);
        //var asdasdsss= _repDetail.AsQueryable();
        //query.The
        //var aaaaa= SqlFunc.Subqueryable<TableColumnsDetail>();
        // TODO:根据传入的参数添加过滤条件

        //var count = await query.CountAsync();

        //var table_ColumnsList = await query
        //.OrderByDescending(t => t.Id).AsNoTracking()
        //.OrderBy(a => a.Order).AsNoTracking()
        ////.PageBy(input)
        //.ToListAsync();

        //var table_ColumnsListDtos = ObjectMapper.Map<List<Table_ColumnsListDto>>(table_ColumnsList);
        //query = query.OrderBuilder(input, "", "TableName");
        return await query.ToListAsync();
        //return new PagedResultDto<Table_ColumnsListDto>(count, table_ColumnsListDtos);



    }
    /// <summary>
    /// 增加表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    public async Task Add(AddTableColumnsInput input)
    {
        var entity = input.Adapt<TableColumns>();
        await _rep.InsertAsync(entity);
    }

    /// <summary>
    /// 删除表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteTableColumnsInput input)
    {
        var entity = await _rep.GetFirstAsync(u => u.Id == input.Id) ?? throw Oops.Oh(ErrorCodeEnum.D1002);
        await _rep.DeleteAsync(entity);   //假删除
    }

    /// <summary>
    /// 更新表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(List<UpdateTableColumnsInput> input)
    {
        var entity = input.Adapt<List<TableColumns>>();

        int index = 1;
        entity.OrderBy(a => a.Order).ForEach(x =>
        {
            x.Order = index++;
            x.TenantId = _userManager.TenantId;
            x.DbColumnName=x.DbColumnName.Trim();
            int indexDetail = 1;
            foreach (var item in x.tableColumnsDetails.OrderBy(v => v.Order))
            {
                item.Order = indexDetail++;
                item.Status = 1;
                item.Creator = _userManager.Account;
                item.CreationTime = DateTime.Now;
                
            }
        });
        //await _db.UpdateNav(entity).Include(a => a.tableColumnsDetails).ExecuteCommandAsync();
        //await _rep.UpdateRangeAsync(entity);
        //await _rep.InsertRangeAsync(input);
        //var entity = input.Adapt<TableColumns>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 更新表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetTableColumnsDetail")]
    public async Task<List<TableColumnsDetail>> GetTableColumnsDetail(TableColumnsBaseInput input)
    {
        //var entity = input.Adapt<TableColumns>();
        return await _repDetail.AsQueryable().Where(a => a.Associated == input.Associated).ToListAsync();
    }

    /// <summary>
    /// 更新表管理明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "UpdateDetail")]
    public async Task UpdateDetail(List<TableColumnsDetail> input)
    {
        //var currentUser = _userManager.

        await _repDetail.DeleteAsync(a => (input.Select(b => b.Associated).ToList().Contains(a.Associated)));
        int index = 1;
        input.OrderBy(o => o.Order).ForEach(a =>
        {
            a.Order = index++;
            a.Status = 1;
            a.CreationTime = DateTime.Now;
            a.Creator = _userManager.Account; 
        });
        await _repDetail.InsertRangeAsync(input);

        //var entity = input.Adapt<TableColumns>();
        //await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }


    /// <summary>
    /// 获取表管理
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Detail")]
    public async Task<TableColumns> Get([FromQuery] QueryByIdTableColumnsInput input)
    {
        return await _rep.GetFirstAsync(u => u.Id == input.Id);
    }

    /// <summary>
    /// 获取表管理列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<TableColumnsOutput>> List(TableColumnsInput input)
    {
        return await _rep.AsQueryable().Select<TableColumnsOutput>().ToListAsync();
    }




    /// <summary>
    /// 删除自定义表缓存的方法
    /// </summary>
    [HttpPost]
    [ApiDescriptionSettings(Name = "CleanTableColumnsCache")]
    public void CleanTableColumnsCache(TableColumnsInput input)
    {
        // TODO:批量删除前的逻辑判断，是否允许删除
        _sysCacheService.SetValue("TableColumn_" + input.TableName + "_" + input.TenantId + "_" + input.CustomerId, new List<TableColumns>());
        _sysCacheService.SetValue("TableColumn_" + input.TableName + "_" + input.TenantId + "_" + input.CustomerId, new List<TableColumns>());
    }



    /// <summary>
    /// 获取导出表格的方法
    /// </summary>
    //[AbpAuthorize(WMS_ASNPermissions.Node)]
    [HttpPost]
    public ActionResult ImportExcelTemplate(TableColumnsInput input)
    {
        // TODO:批量删除前的逻辑判断，是否允许删除
        var query = _rep.AsQueryable()
       //模糊搜索TableName
       .WhereIF(!input.TableName.IsNullOrEmpty(), a => a.TableName == input.TableName)
       //.WhereIF(input.CustomerId > 0, a => a.CustomerId == input.CustomerId)
       .Where(a => a.TenantId == _userManager.TenantId && a.IsImportColumn == 1)
       //.WhereIF(a => a.IsImportColumn == 1)
       //.WhereIf(1 == 1, a => a.TenantId == input.TenantId)
       .Select(a => new TableColumns
       {
           DisplayName = a.DisplayName,
           //由于框架约定大于配置， 数据库的字段首字母小写
           //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
           DbColumnName = a.DbColumnName,
           IsImportColumn = a.IsImportColumn
       });

        //使用简单工厂定制化
        IImportExcelTemplate factory = ImportExcelFactory.ImportExcelTemplate(input.CustomerId, input.TableName);
        factory._repTableColumns = _rep;
        factory._userManager = _userManager;
        var response = factory.Strategy(input.CustomerId, _userManager.TenantId);

        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        //return new XlsxFileResult(stream: fs, fileDownloadName: "下载文件");
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "下载文件.xlsx" // 配置文件下载显示名
        };


        //var result = await _excelExporter.ExportAsByteArray(dto);
        //var fs = new MemoryStream(result);

        //return new XlsxFileResult(stream: fs, "活动报名信息表.xlsx");


    }
}

