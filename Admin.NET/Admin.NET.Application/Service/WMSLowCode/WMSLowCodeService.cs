using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Common.MD5Common;
using Admin.NET.Common.TextCommon;
using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using Furion.VirtualFileServer;
using Nest;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using System.Data;
using NewLife;

namespace Admin.NET.Application;
/// <summary>
/// WMSLowCode服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSLowCodeService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSLowCode> _rep;
    private readonly UserManager _userManager;
    private readonly SqlSugarRepository<SysMenu> _repMenu;
    private readonly SqlSugarRepository<SysRoleMenu> _repRoleMenu;
    private readonly SqlSugarRepository<SysUserRole> _repUserRole;
    public WMSLowCodeService(SqlSugarRepository<WMSLowCode> rep, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<SysMenu> repMenu, SqlSugarRepository<SysRoleMenu> repRoleMenu, SqlSugarRepository<SysUserRole> repUserRole)
    {
        _rep = rep;
        _userManager = userManager;
        _repMenu = repMenu;
        _repRoleMenu = repRoleMenu;
        _repUserRole = repUserRole;
    }

    /// <summary>
    /// 分页查询WMSLowCode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSLowCodeOutput>> Page(WMSLowCodeInput input)
    {
        var query = _rep.AsQueryable()
                    .WhereIF(!string.IsNullOrWhiteSpace(input.MenuName), u => u.MenuName.Contains(input.MenuName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UICode), u => u.UICode.Contains(input.UICode.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.UIType), u => u.UIType.Contains(input.UIType.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.DataSource), u => u.DataSource.Contains(input.DataSource.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    .Select<WMSLowCodeOutput>()
;
        if (input.CreationTimeRange != null && input.CreationTimeRange.Count > 0)
        {
            DateTime? start = input.CreationTimeRange[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTimeRange.Count > 1 && input.CreationTimeRange[1].HasValue)
            {
                var end = input.CreationTimeRange[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加WMSLowCode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Add")]
    [UnitOfWork]
    public async Task<Response> Add(AddWMSLowCodeInput input)
    {
        Response response = new Response();
        //判断SQL 语句有没有注入的风险
        if (!ProcessSqlStr(input.SQLCode))
        {
            response.Code = StatusCode.Warning;
            response.Msg = "有风险的SQL语句";
            return response;
        }
        //判断是否存在
        var menuCheck = _repMenu.AsQueryable().Where(a => a.Title == input.MenuName);
        if (menuCheck.Count() > 0)
        {
            response.Code = StatusCode.Warning;
            response.Msg = "名称已经存在";
            return response;
        }
        var entity = input.Adapt<WMSLowCode>();
        //将UI另存为文件
        var path = TextHelper.Writtxt(input.UICode, input.MenuName);
        entity.UICode = path;
        entity.Creator = _userManager.Account;
        entity.CreationTime = DateTime.Now;
        await _rep.InsertAsync(entity);

        var menu = _repMenu.AsQueryable().Where(a => a.Title == "自定义").ToList().First();
        //构建新的菜单
        SysMenu menuData = new SysMenu();
        menuData.Id = 0;
        menuData.Title = input.MenuName;
        menuData.Name = input.MenuName;
        //将路径转换成MD5 存文字路由会有问题
        menuData.Path = "/custom/" + MD5Helper.CalcMD5(input.MenuName);
        menuData.Pid = menu.Id;
        menuData.Type = MenuTypeEnum.Menu;
        menuData.Component = "/system/formDes/lowCodePage";
        menuData.Icon = "ele-Basketball";
        menuData.IsIframe = false;
        menuData.IsHide = false;
        menuData.IsKeepAlive = true;
        menuData.IsAffix = false;
        menuData.OrderNo = 9990;
        menuData.Status = StatusEnum.Enable;
        menuData.CreateTime = DateTime.Now;
        await _repMenu.InsertAsync(menuData);

        //构建新的权限
        var menuId = _repMenu.AsQueryable().Where(a => a.Title == input.MenuName).ToList().First();

        var getRoleId = _repUserRole.AsQueryable().Where(a => a.UserId == _userManager.UserId).First();
        //1300000000101
        SysRoleMenu roleMenuData = new SysRoleMenu();
        roleMenuData.MenuId = menuId.Id;
        //针对超级管理员特殊操作
        roleMenuData.RoleId = _userManager.RealName == "超级管理员" ? 1300000000101 : getRoleId.RoleId;
        roleMenuData.Id = 0;
        await _repRoleMenu.InsertAsync(roleMenuData);
        response.Code = StatusCode.Success;
        response.Msg = "添加成功";
        return response;
    }




    /// 
    /// 判断字符串中是否有SQL攻击代码
    /// 
    /// 传入用户提交数据
    /// true-安全；false-有注入攻击现有；
    public bool ProcessSqlStr(string inputString)
    {
        string SqlStr = @"exec|execute|insert|delete|update|alter|create|drop|\*|chr|char|asc|mid|substring|master|truncate|declare|xp_cmdshell|restore|backup|net +user|net +localgroup +administrators";
        try
        {
            if ((inputString != null) && (inputString != String.Empty))
            {
                string str_Regex = @"\b(" + SqlStr + @")\b";
                Regex Regex = new Regex(str_Regex, RegexOptions.IgnoreCase);
                if (true == Regex.IsMatch(inputString))
                    return false;
            }
        }
        catch
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 获取WMSLowCode 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Initialize")]
    [UnitOfWork]
    public async Task Initialize(string name)
    {
        var entity = _rep.AsQueryable().Where(u => u.MenuName == name).ToList();
        if (entity.Count > 0)
        {
            //删除低代码配置表
            await _rep.DeleteAsync(a => a.MenuName == name);
            //删除菜单关系
            var getMenu = _repMenu.AsQueryable().Where(a => a.Title == name).ToList();
            if (getMenu.Count > 0)
            {
                await _repRoleMenu.DeleteAsync(a => a.MenuId == getMenu.First().Id);
                await _repMenu.DeleteAsync(a => a.Id == getMenu.First().Id);
            }

        }
        //return entity;
    }


    /// <summary>
    /// 删除WMSLowCode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Delete")]
    public async Task Delete(DeleteWMSLowCodeInput input)
    {
        var entity = _rep.AsQueryable().Where(u => u.MenuName == input.MenuName).ToList();
        if (entity.Count > 0)
        {
            //删除低代码配置表
            await _rep.DeleteAsync(a => a.MenuName == input.MenuName);
            //删除菜单关系
            var getMenu = _repMenu.AsQueryable().Where(a => a.Title == input.MenuName).ToList();
            if (getMenu.Count > 0)
            {
                await _repRoleMenu.DeleteAsync(a => a.MenuId == getMenu.First().Id);
                await _repMenu.DeleteAsync(a => a.Id == getMenu.First().Id);
            }

        }
    }

    /// <summary>
    /// 更新WMSLowCode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Update")]
    public async Task Update(UpdateWMSLowCodeInput input)
    {
        var entity = input.Adapt<WMSLowCode>();
        await _rep.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
    }




    /// <summary>
    /// 获取WMSLowCode 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "Query")]
    public async Task<WMSLowCode> Get(string name)
    {
        var entity = await _rep.AsQueryable().Where(u => u.MenuName == name).FirstAsync();
        string UICode = TextHelper.Readtxt(entity.UICode);
        entity.UICode = UICode;
        return entity;
    }



    /// <summary>
    /// 获取WMSLowCode列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiDescriptionSettings(Name = "List")]
    public async Task<List<WMSLowCodeOutput>> List([FromQuery] WMSLowCodeInput input)
    {
        return await _rep.AsQueryable().Select<WMSLowCodeOutput>().ToListAsync();
    }



    [HttpPost]
    [ApiDescriptionSettings(Name = "GetData")]
    public async Task<Response<dynamic, int>> GetData(dynamic input)
    {
        string name = input.name;
        string page = input.page;
        string pageSize = input.pageSize;
        string total = input.total;

        Response<dynamic, int> response = new Response<dynamic, int>();
        //string str = input;
        Dictionary<string, string> pairs = GetModelDictionary(input.formDataModel);
        StringBuilder strSql = new StringBuilder();
        var sqlCode = _rep.AsQueryable().Where(u => u.MenuName == name).ToList();
        if (sqlCode.Count > 0)
        {
            strSql.Append(sqlCode[0].SQLCode);
            foreach (var item in pairs)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    strSql.Append(" and " + item.Key + "='" + item.Value + "'");
                }
            }
            //获取总数和列名称

            //DataTable dt = _rep.Context.Ado.GetDataTable(strSql.ToString().Replace("select ", "SELECT TOP 1 COUNT(*) Total, "));
            //DataTable dt = _rep.Context.Ado.GetDataTable(strSql.ToString());
            //if (dt.Rows.Count > 0)
            //{
            //    response.Result = Convert.ToInt32(dt.Rows[0]["Total"]);
            //    strSql.Append(" ORDER BY ");

            //    //strSql.Append("ORDER BY ");

            //    strSql.Append(string.Join(",", dt.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "Total").Select(c => "'" + c.ColumnName + "'").ToArray()));
            //    //foreach (DataColumn dc in dt.Columns)
            //    //{
            //    //    strSql.Append("" + dc.ColumnName);
            //    //}
            //}
            var data = _rep.Context.Ado.GetDataTable(strSql.ToString());
            response.Data = data;
        }
        return response;
    }


    [HttpPost]
    [ApiDescriptionSettings(Name = "GetExcel")]
    public ActionResult GetExcel(dynamic input)
    {
        string name = input.name;

        Response<dynamic> response = new Response<dynamic>();
        //string str = input;
        Dictionary<string, string> pairs = GetModelDictionary(input.formDataModel);
        StringBuilder strSql = new StringBuilder();
        var sqlCode = _rep.AsQueryable().Where(u => u.MenuName == name).ToList();
        if (sqlCode.Count > 0)
        {
            strSql.Append(sqlCode[0].SQLCode);
            foreach (var item in pairs)
            {
                if (!string.IsNullOrEmpty(item.Value))
                {
                    strSql.Append(" and " + item.Key + "='" + item.Value + "'");
                }
            }
            var data = _rep.Context.Ado.GetDataTable(strSql.ToString());
            response.Data = data;
        }
        IExporter exporter = new ExcelExporter();
        var result = exporter.ExportAsByteArray<DataTable>(response.Data);
        var fs = new MemoryStream(result.Result);
        return new FileStreamResult(fs, "application/octet-stream")
        {
            FileDownloadName = "" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx" // 配置文件下载显示名
        };
    }

    /// <summary>
    /// 动态类 获取字典集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="model"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetModelDictionary(dynamic model)
    {

        var dic = new Dictionary<string, string>();
        try
        {

            foreach (var info in model)
            {
                try
                {
                    dic.Add(info.Name.ToString(), info.Value.ToString());
                }
                catch (Exception)
                {
                }
            }

            return dic;
        }
        catch (Exception ex)
        {
            //LogHelper.Error("动态类 获取字典集合 异常：" + ex.Message);
            //return dic;
        }

        return null;
    }



}

