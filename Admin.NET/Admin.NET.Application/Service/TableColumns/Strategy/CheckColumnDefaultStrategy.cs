// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using FluentEmail.Core;
using Microsoft.CodeAnalysis;
using Nest;
using Newtonsoft.Json;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using Rule = RulesEngine.Models.Rule;

namespace Admin.NET.Application.Service;
public class CheckColumnDefaultStrategy : ICheckColumnsDefaultInterface
{

    public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
    public UserManager _userManager { get; set; }

    static List<TableColumns> tableColumnList = new List<TableColumns>();

    public List<string> _tableNames { get; set; }

    public CheckColumnDefaultStrategy()
    {
        //_tableNames = tableNames;
    }

    public virtual async Task<Response<List<OrderStatusDto>>> CheckColumns<T>(IEnumerable<T> collection, string tableName)
    {

        //var aa=RunBowLibaray.RulesEngine.RuleManage

        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
        List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();

        try
        {
            tableColumnList = GetColumns(tableName);
            Workflow[] workflowRules = new Workflow[1];
            List<Rule> rules = new List<Rule>();
            foreach (var item in tableColumnList)
            {
                if (item.Validation == "Required")
                {
                    Rule rule = new Rule();
                    rule.RuleName = item.DbColumnName;
                    if (item.Type == "DropDownListInt" || item.Type == "DropDownListStr")
                    {
                        //rule.Id = item.DbColumnName;

                        rule.ErrorMessage = item.DisplayName + "不能为空";
                        rule.Expression = "!string.IsNullOrEmpty(" + item.DbColumnName + ")";

                    }
                    else if (item.Type == "DateTimePicker" || item.Type == "DatePicker")
                    {
                        rule.ErrorMessage = "时间格式不正确";
                        rule.Expression = "Convert.ToDateTime(" + item.DbColumnName + ").Year>2000";
                    }
                    else
                    {
                        rule.ErrorMessage = item.DisplayName + "不能为空";
                        rule.Expression = "!string.IsNullOrWhiteSpace(" + item.DbColumnName + ")";

                        //判断是否包含空格
                        //rule.Expression = "string.IsNullOrWhiteSpace(" + item.DbColumnName + ")";

                    }
                    rules.Add(rule);
                }
            }
            workflowRules[0] = (new WorkflowRules() { Rules = rules, WorkflowName = "UserInputWorkflow" });
            //反序列化Json格式规则字符串
            //var workflowRuless = JsonConvert.DeserializeObject<List<WorkflowRules>>(workflowRules);

            //初始化规则引擎
            var rulesEngine = new RulesEngine.RulesEngine(workflowRules);
            //List<RuleResultTree> resultList = new List<RuleResultTree>();
            int flag = 1;
            foreach (var item in collection)
            {
                //使用规则进行判断，并返回结果
                var result = await rulesEngine.ExecuteAllRulesAsync("UserInputWorkflow", item);

                foreach (var rule in result.Where(a => a.IsSuccess == false))
                {
                    statusDtos.Add(new OrderStatusDto()
                    {
                        StatusCode = StatusCode.Warning,
                        Msg = rule.Rule.ErrorMessage,
                        ExternOrder = "第" + flag + "行",
                        SystemOrder = "第" + flag + "行",
                    });
                }
                flag++;
            } 
            //验证数据
            if (statusDtos.Count > 0)
            {
                response.Code = StatusCode.Error;
            }
            else
            {
                response.Code = StatusCode.Success;
            }
            //response.Result = statusDtos;
            response.Data = statusDtos;

            //throw new NotImplementedException();
            return response;

        }
        catch (Exception ex)
        {
            return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = ex.Message };
        }
    }


    //public static List<OrderStatusDto> ValidateCollection<T>(IEnumerable<T> collection, string tableName)
    //{
    //    List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();
    //    foreach (var item in collection)
    //    {
    //        statusDtos.AddRange(ValidateItem(item, tableName));
    //    }
    //    return statusDtos;
    //}

    //private static List<OrderStatusDto> ValidateItem<T>(T item, string tableName)
    //{
    //    List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();
    //    foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
    //    {
    //        var tableColumn = tableColumnList.Where(a => a.DbColumnName == property.Name).FirstOrDefault();
    //        if (tableColumn == null || tableColumn.Validation != "Required")
    //        {
    //            continue;
    //        }
    //        var value = property.GetValue(item);
    //        if (string.IsNullOrEmpty(value.ToString()))
    //        {
    //            statusDtos.Add(new OrderStatusDto()
    //            {
    //                StatusCode = StatusCode.Warning,
    //                Msg = property.Name + ":数据不能为空"
    //            });
    //        }
    //        else
    //        {
    //            if (tableColumn.Type == "DropDownListInt" && tableColumn.tableColumnsDetails.Where(a => a.Name == value.ToString()).Count() == 0)
    //            {
    //                statusDtos.Add(new OrderStatusDto()
    //                {
    //                    StatusCode = StatusCode.Warning,
    //                    Msg = property.Name + ":数据错误,“" + value + "”不在系统提供范围内"
    //                });
    //            }
    //            else if (tableColumn.Type == "DropDownListStr" && tableColumn.tableColumnsDetails.Where(a => a.Name == value.ToString()).Count() == 0)
    //            {
    //                statusDtos.Add(new OrderStatusDto()
    //                {
    //                    StatusCode = StatusCode.Warning,
    //                    Msg = property.Name + ":数据错误,“" + value + "”不在系统提供范围内"
    //                });
    //            }
    //            else if (tableColumn.Type == "DatePicker" || tableColumn.Type == "DateTimePicker")
    //            {
    //                var isDate = DateTime.TryParse(value.ToString(), out DateTime date);
    //                if (!isDate && date.Year < 2000)
    //                {
    //                    statusDtos.Add(new OrderStatusDto()
    //                    {
    //                        StatusCode = StatusCode.Warning,
    //                        Msg = property.Name + ":数据错误,“" + value + "”不是有效格式"
    //                    });
    //                }
    //            }
    //        }
    //    }
    //    return statusDtos;
    //}


    //private static List<OrderStatusDto> ValidateItemDetail<T>(T item, string tableName)
    //{
    //    List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();
    //    foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
    //    {
    //        var tableColumn = tableColumnList.Where(a => a.DbColumnName == property.Name).FirstOrDefault();
    //        if (tableColumn == null || tableColumn.Validation != "Required")
    //        {
    //            continue;
    //        }
    //        var value = property.GetValue(item);
    //        if (string.IsNullOrEmpty(value.ToString()))
    //        {
    //            statusDtos.Add(new OrderStatusDto()
    //            {
    //                StatusCode = StatusCode.Warning,
    //                Msg = property.Name + ":数据不能为空"
    //            });
    //        }
    //        else
    //        {
    //            if (tableColumn.Type == "DropDownListInt" && tableColumn.tableColumnsDetails.Where(a => a.Name == value.ToString()).Count() == 0)
    //            {
    //                statusDtos.Add(new OrderStatusDto()
    //                {
    //                    StatusCode = StatusCode.Warning,
    //                    Msg = property.Name + ":数据错误,“" + value + "”不在系统提供范围内"
    //                });
    //            }
    //            else if (tableColumn.Type == "DropDownListStr" && tableColumn.tableColumnsDetails.Where(a => a.Name == value.ToString()).Count() == 0)
    //            {
    //                statusDtos.Add(new OrderStatusDto()
    //                {
    //                    StatusCode = StatusCode.Warning,
    //                    Msg = property.Name + ":数据错误,“" + value + "”不在系统提供范围内"
    //                });
    //            }
    //            else if (tableColumn.Type == "DatePicker" || tableColumn.Type == "DateTimePicker")
    //            {
    //                var isDate = DateTime.TryParse(value.ToString(), out DateTime date);
    //                if (!isDate && date.Year < 2000)
    //                {
    //                    statusDtos.Add(new OrderStatusDto()
    //                    {
    //                        StatusCode = StatusCode.Warning,
    //                        Msg = property.Name + ":数据错误,“" + value + "”不是有效格式"
    //                    });
    //                }
    //            }
    //        }
    //    }
    //    return statusDtos;
    //}


    public List<TableColumns> GetColumns(string TableName)
    {
        return _repTableColumns.AsQueryable()
           .Where(a => a.TableName == TableName &&
             a.TenantId == _userManager.TenantId &&
             a.IsImportColumn == 1
           )
          .Select(a => new TableColumns
          {
              DisplayName = a.DisplayName,
              Type = a.Type,
              //由于框架约定大于配置， 数据库的字段首字母小写
              //DbColumnName = a.DbColumnName.Substring(0, 1).ToLower() + a.DbColumnName.Substring(1)
              TableName = a.TableName,
              DbColumnName = a.DbColumnName,
              Validation = a.Validation,
              IsImportColumn = a.IsImportColumn,
              tableColumnsDetails = SqlFunc.Subqueryable<TableColumnsDetail>().Where(b => b.Associated == a.Associated && b.Status == 1 && b.TenantId == a.TenantId).ToList()
              //Details = _repTableColumnsDetail.AsQueryable().Where(b => b.Associated == a.Associated)
              //.Select()
          }).ToList();
    }
}
