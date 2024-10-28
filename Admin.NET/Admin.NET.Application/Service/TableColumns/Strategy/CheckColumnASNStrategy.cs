// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public class CheckColumnASNStrategy : CheckColumnDefaultStrategy
{
    static List<string> _tableNames = new List<string>() { "WMS_ASN", "WMS_ASNDetail" };

    List<TableColumns> tableColumnList;
    public CheckColumnASNStrategy(
        //ITable_ColumnsManager table_ColumnsManager,
        //      ITable_ColumnsDetailManager table_ColumnsDetailManager
        )
    {
        //_table_ColumnsManager = table_ColumnsManager;
        //_table_ColumnsDetailManager = table_ColumnsDetailManager;
    }
    public async Task<Response<List<OrderStatusDto>>> CheckColumns<T>(IEnumerable<T> collection, string tableName)
    {


        Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
        List<OrderStatusDto> statusDtos = new List<OrderStatusDto>();
        foreach (var item in _tableNames)
        {
            tableColumnList.AddRange(GetColumns(tableName));
        }
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
                    rule.Expression = "Convert.ToDateTime(" + item.DbColumnName + ").Year<2000";
                }
                else
                {
                    rule.ErrorMessage = item.DisplayName + "不能为空";
                    rule.Expression = "!string.IsNullOrEmpty(" + item.DbColumnName + ")";
                }
                rules.Add(rule);
            }
            else
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
                    rule.Expression = "Convert.ToDateTime(" + item.DbColumnName + ").Year<2000";
                }
                else
                {
                    rule.ErrorMessage = item.DisplayName + "不能为空";
                    rule.Expression = "!string.IsNullOrEmpty(" + item.DbColumnName + ")";
                }
                rules.Add(rule);
            }
        }
        workflowRules[0] = (new WorkflowRules() { Rules = rules, WorkflowName = "WMS_ASN_Workflow" });
        //反序列化Json格式规则字符串
        //var workflowRuless = JsonConvert.DeserializeObject<List<WorkflowRules>>(workflowRules);

        //初始化规则引擎
        var rulesEngine = new RulesEngine.RulesEngine(workflowRules);
        List<RuleResultTree> resultList = new List<RuleResultTree>();
        //使用规则进行判断，并返回结果
        foreach (var item in collection)
        {
            resultList = await rulesEngine.ExecuteAllRulesAsync("WMS_ASN_Workflow", item);
        }
        foreach (var item in collection)
        {
            //resultList.Add(new OrderStatusDto()
            //{
            //    ExternOrder = item.ExternOrder,
            //     Status=Sta = item.ExternOrder,
            //    ExternOrder = item.ExternOrder,
            //    ExternOrder = item.ExternOrder,
            //});
        }

        return null;

    }

}
