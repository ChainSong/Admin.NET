// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.CommonCore.SqlSugerCommon;
internal class SqlSugarWhereHelper<T> where T : class, new()
{
    public static Expressionable<T> SqlWhere(T input) {

        //var queryNew = _rep.AsQueryable();
        Type type = typeof(T);
        //List<WMSASNInput> list = new List<T>();
        PropertyInfo[] properties = type.GetProperties();
        FieldInfo[] fiedInfos = type.GetFields();
        //PropertyInfo[] properties = type.GetProperties();
        //foreach (var item in properties)
        //{

        //    var name = input.GetType().GetProperty(item.Name).GetValue(input, null);
        //    Console.WriteLine(name);
        //    ISugarQueryable<WMSASN> sugarQueryable = queryNew.WhereIF(!string.IsNullOrWhiteSpace(name), it => $"it=>it." + item.Name + "=='" + name + "'");
        //    //var newWhere = DynamicExpressionParser.ParseLambda<U_ChargingPile, bool>(
        //    //           ParsingConfig.Default, false, $@"{search.Condition}=(@0)", search.Keyword);
        //    //where = where.And(newWhere);
        //}

        //var names = new string[] { "a", "b" };
        //db.QueryableByObject(typeof(UnitPerson011)).Where("it", $"it=>it.Address.Id=={1}").ToList();

        ////var names = new string[] { "a", "b" };
        Expressionable<T> exp = Expressionable.Create<T>();
        foreach (var p in properties)
        {
            var value = input.GetType().GetProperty(p.Name).GetValue(input, null);
            if (value is not null)
            {
                if (value.ToString() != "0" && value.ToString() != "")
                {
                    try
                    {


                        //Console.WriteLine(name);
                        var newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                               ParsingConfig.Default, false, $@"1==1");
                        if (p.ToString().Contains("DateTime"))
                        {
                            if (p.ToString().Contains("List"))
                            {
                                //转换成List
                                var valuedate = value as List<DateTime?>;
                                if (valuedate != null)
                                {
                                    newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                                ParsingConfig.Default, false, $@"{p.Name}>=(@0)", valuedate[0]);
                                    if (valuedate.Count > 1)
                                    {
                                        //exp.And(newWhere);
                                        newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                                 ParsingConfig.Default, false, $@"{p.Name}<=(@0)", valuedate[1]);
                                    }
                                }

                            }
                            else
                            {
                                newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                           ParsingConfig.Default, false, $@"{p.Name}=(@0)", value);
                            }
                            //newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                            //  ParsingConfig.Default, false, $@"{p.Name}>=(@0) and " + $@"{p.Name}<=(@0)", value[0], value[1]);
                        }
                        else if (p.ToString().Contains("Double"))
                        {
                            newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                      ParsingConfig.Default, false, $@"{p.Name}=(@0)", value);
                        }
                        else if (p.ToString().Contains("Int"))
                        {
                            newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                     ParsingConfig.Default, false, $@"{p.Name}=(@0)", value);
                        }
                        else if (p.ToString().Contains("Decimal"))
                        {
                            newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                     ParsingConfig.Default, false, $@"{p.Name}=(@0)", value);
                        }
                        else if (p.ToString().Contains("String"))
                        {
                            newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                                     ParsingConfig.Default, false, $@"{p.Name}=(@0)", value);
                        }
                        else
                        {
                            //newWhere = DynamicExpressionParser.ParseLambda<WMSASN, bool>(
                            //   ParsingConfig.Default, false, $@"{p.Name}=(@0)", name);
                        }

                        //where = where.And(newWhere);

                        //exp.And(newWhere);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

          
        }
        return exp;
        //var list = queryNew.Where(exp.ToExpression()).Select<WMSASNOutput>();
        //var list = db.Queryable<Order>().Where(exp.ToExpression()).ToList();


        //foreach (DataRow row in input.Rows)
        //{
        //    PropertyInfo[] properties = type.GetProperties();
        //    T model = new T();
    }
}
