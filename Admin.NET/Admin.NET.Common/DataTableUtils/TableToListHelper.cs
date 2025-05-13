
using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Data
{
    public static class TableToListHelper
    {
        //public static List<T> TableToList<T>(this DataTable dt) where T : class, new()
        //{

        //    try
        //    {

        //        Type type = typeof(T);
        //        List<T> list = new List<T>();
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            PropertyInfo[] properties = type.GetProperties();
        //            T model = new T();
        //            foreach (PropertyInfo p in properties)
        //            {

        //                //判断model中的字段在datatable中存不存在
        //                if (row.Table.Columns.Contains(p.Name))
        //                {
        //                    try
        //                    {
        //                        object value = row[p.Name];
        //                        if (value == DBNull.Value && string.IsNullOrEmpty(value.ToString()))
        //                        {
        //                            if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name == "DateTime").Count() > 0)
        //                            {
        //                                p.SetValue(model, null, null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name == ("Double")).Count() > 0 || p.PropertyType.Name == "Double"))
        //                            {
        //                                p.SetValue(model, 0.0, null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int")).Count() > 0 || p.PropertyType.Name.Contains("Int")))
        //                            {
        //                                p.SetValue(model, 0, null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("decimal")).Count() > 0)
        //                            {
        //                                p.SetValue(model, 0, null);
        //                            }
        //                            else
        //                            {
        //                                p.SetValue(model, "", null);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (value is decimal)
        //                            {
        //                                p.SetValue(model, Convert.ToDecimal(value), null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name == "DateTime").Count() > 0)
        //                            {
        //                                if (value == null || value.ToString() == "")
        //                                {
        //                                    p.SetValue(model, null, null);

        //                                }
        //                                else
        //                                {
        //                                    //p.SetValue(model, Convert.ToDateTime(value), null);
        //                                    DateTime parsedDate;
        //                                    string[] dateFormats = { "dd-MMM-yyyy", "yyyy-MM-dd", "yyyy-M-dd", "yyyy-M-d", "yyyy-MM-d", "MM/dd/yyyy" };
        //                                    if (DateTime.TryParseExact(value.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out parsedDate))
        //                                    {
        //                                        p.SetValue(model, parsedDate, null);
        //                                    }
        //                                    else if (Convert.ToDateTime(value.ToString()) is DateTime)
        //                                    {
        //                                        p.SetValue(model, Convert.ToDateTime(value), null);
        //                                    }
        //                                    else
        //                                    {
        //                                        // 如果解析失败，可以抛出异常或处理为默认值
        //                                        p.SetValue(model, null, null);
        //                                    }
        //                                }
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name == ("Double")).Count() > 0 || p.PropertyType.Name == "Double"))
        //                            {
        //                                if (value == null || value.ToString() == "")
        //                                {
        //                                    p.SetValue(model, Convert.ToDouble(0), null);
        //                                }
        //                                else
        //                                {
        //                                    p.SetValue(model, Convert.ToDouble(value), null);
        //                                }
        //                            }

        //                            else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int")).Count() > 0 || p.PropertyType.Name.Contains("Int"))
        //                            {
        //                                if (value == null || value.ToString() == "")
        //                                {
        //                                    p.SetValue(model, Convert.ToInt32(0), null);
        //                                }
        //                                else
        //                                {
        //                                    if (p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int64")).Count() > 0)
        //                                    {
        //                                        p.SetValue(model, Convert.ToInt64(value), null);
        //                                    }
        //                                    else
        //                                    {
        //                                        p.SetValue(model, Convert.ToInt32(value), null);
        //                                    }
        //                                }
        //                            }
        //                            //else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int64")).Count() > 0 || p.PropertyType.Name.Contains("Int"))
        //                            //{
        //                            //    p.SetValue(model, Convert.ToInt64(value), null);
        //                            //}
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.ToUpper() == ("Decimal").ToUpper()).Count() > 0 || p.PropertyType.Name.ToUpper() == "Decimal".ToUpper()))
        //                            {
        //                                p.SetValue(model, Convert.ToDecimal(value), null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.ToUpper() == ("float").ToUpper()).Count() > 0 || p.PropertyType.Name.ToUpper() == "float".ToUpper()))
        //                            {
        //                                p.SetValue(model, Convert.ToDecimal(value), null);
        //                            }
        //                            else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.ToUpper() == ("Single").ToUpper()).Count() > 0 || p.PropertyType.Name.ToUpper() == "Single".ToUpper()))
        //                            {
        //                                p.SetValue(model, Convert.ToSingle(value), null);
        //                            }
        //                            else
        //                            {
        //                                if (value == null)
        //                                {
        //                                    p.SetValue(model, value, null);
        //                                }
        //                                else
        //                                {
        //                                    p.SetValue(model, Convert.ToString(value).Trim(), null);
        //                                }

        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        throw Oops.Oh(ex, $"TableToListHelper.TableToList<{type.Name}>");
        //                    }
        //                }


        //            }
        //            list.Add(model);
        //        }
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw Oops.Oh(ex, $"TableToListHelper.TableToList<{typeof(T).Name}>");
        //    }

        //}


        public static List<T> TableToList<T>(this DataTable dt) where T : class, new()
        {
            try
            {
                Type type = typeof(T);
                List<T> list = new List<T>();

                foreach (DataRow row in dt.Rows)
                {
                    PropertyInfo[] properties = type.GetProperties();
                    T model = new T();

                    foreach (PropertyInfo p in properties)
                    {
                        if (row.Table.Columns.Contains(p.Name))
                        {
                            try
                            {
                                object value = row[p.Name];
                                if (value == DBNull.Value || string.IsNullOrEmpty(value?.ToString()))
                                {
                                    SetDefaultValue(p, model);
                                }
                                else
                                {
                                    SetPropertyValue(p, model, value);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw Oops.Oh(ex, $"TableToListHelper.TableToList<{type.Name}>");
                            }
                        }
                    }
                    list.Add(model);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex, $"TableToListHelper.TableToList<{typeof(T).Name}>");
            }
        }

        private static void SetDefaultValue<T>(PropertyInfo property, T model) where T : class
        {
            if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
            {
                property.SetValue(model, null);
            }
            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                property.SetValue(model, 0.0);
            }
            else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
            {
                property.SetValue(model, 0f);
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                property.SetValue(model, 0);
            }
            else if (property.PropertyType == typeof(Int64) || property.PropertyType == typeof(Int64?))
            {
                property.SetValue(model, 0);
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                property.SetValue(model, 0m);
            }
            else
            {
                property.SetValue(model, "");
            }
        }

        private static void SetPropertyValue<T>(PropertyInfo property, T model, object value) where T : class
        {
            if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime))
            {
                if (DateTime.TryParse(value.ToString(), out DateTime parsedDate))
                {
                    property.SetValue(model, parsedDate);
                }
                else
                {
                    property.SetValue(model, "");
                }
            }
            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                property.SetValue(model, Convert.ToDouble(value));
            }
            else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(float?))
            {
                property.SetValue(model, Convert.ToSingle(value));
            }
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(int?))
            {
                property.SetValue(model, Convert.ToInt32(value));
            }
            else if (property.PropertyType == typeof(long) || property.PropertyType == typeof(long?))
            {
                property.SetValue(model, Convert.ToInt64(value));
            }
            else if (property.PropertyType == typeof(double) || property.PropertyType == typeof(double?))
            {
                property.SetValue(model, Convert.ToInt64(value));
            }
            else if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
            {
                property.SetValue(model, Convert.ToDecimal(value));
            }
            else
            {
                property.SetValue(model, Convert.ToString(value)?.Trim());
            }
        }
    }
}