using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Furion.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace Admin.NET.Application;
/// <summary>
/// WMSStockCheck服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSGptService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSASN> _repASN;
    private readonly SqlSugarRepository<WMSPreOrder> _repPreOrder;
    //private readonly SqlSugarRepository<WMSStockCheck> _rep;
    //private readonly SqlSugarRepository<WMSStockCheckDetail> _repStockCheckDetail;
    //private readonly SqlSugarRepository<WMSStockCheckDetailScan> _repStockCheckDetailScan;
    //private readonly SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable;
    //private readonly SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving;
    //private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;
    private readonly UserManager _userManager;
    public WMSGptService(SqlSugarRepository<WMSASN> repASN, SqlSugarRepository<WMSPreOrder> repPreOrder, UserManager userManager)
    {
        _repASN = repASN;
        _repPreOrder = repPreOrder;
        //_repInventoryUsable = repInventoryUsable;
        //_repReceiptReceiving = repReceiptReceiving;
        //_repStockCheckDetail = repStockCheckDetail;
        //_repStockCheckDetailScan = repStockCheckDetailScan;
        _userManager = userManager;

    }

    //[AuthorizePermission(AppPermissions.None)] // 表示这个操作不需要权限
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetData")]
    public async Task<Response<dynamic, int>> GetData(dynamic input)
    {
        string strSql = input.sql;
        //string page = input.page;
        //string pageSize = input.pageSize;
        //string total = input.total;

        Response<dynamic, int> response = new Response<dynamic, int>();
        //string str = input;
        //Dictionary<string, string> pairs = GetModelDictionary(input.formDataModel);
        //StringBuilder strSql = new StringBuilder();
        //var sqlCode = _repASN.AsQueryable().Where(u => u. == name).ToList();
        if (!ProcessSqlStr(strSql.ToString()))
        {
            response.Code = StatusCode.Error;
            response.Msg = "失败";
            return response;
        }
        var data = _repASN.Context.Ado.GetDataTable(strSql.ToString());
        response.Data = data;
        response.Code = StatusCode.Success;
        response.Msg = "成功";
        return response;
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


}

