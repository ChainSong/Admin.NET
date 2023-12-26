
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
        public static List<T> TableToList<T>(this DataTable dt) where T : class, new()
        {


            Type type = typeof(T);
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                PropertyInfo[] properties = type.GetProperties();
                T model = new T();
                foreach (PropertyInfo p in properties)
                {
                    //try
                    //{


                        //判断model中的字段在datatable中存不存在
                        if (row.Table.Columns.Contains(p.Name))
                        {
                            object value = row[p.Name];
                            if (value == DBNull.Value)
                            {
                                if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name == "DateTime").Count() > 0)
                                {
                                    p.SetValue(model, null, null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name == ("Double")).Count() > 0 || p.PropertyType.Name == "Double"))
                                {
                                    p.SetValue(model, 0.0, null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int")).Count() > 0 || p.PropertyType.Name.Contains("Int")))
                                {
                                    p.SetValue(model, 0, null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("decimal")).Count() > 0)
                                {
                                    p.SetValue(model, 0, null);
                                }
                                else
                                {
                                    p.SetValue(model, "", null);
                                }
                            }
                            else
                            {
                                if (value is decimal)
                                {
                                    p.SetValue(model, Convert.ToInt32(value), null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name == "DateTime").Count() > 0)
                                {
                                    p.SetValue(model, Convert.ToDateTime(value), null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name == ("Double")).Count() > 0 || p.PropertyType.Name == "Double"))
                                {
                                    p.SetValue(model, Convert.ToInt64(value), null);
                                }

                                else if (p.PropertyType.GenericTypeArguments != null && p.PropertyType.GenericTypeArguments.Where(a => a.Name.Contains("Int")).Count() > 0 || p.PropertyType.Name.Contains("Int"))
                                {
                                    p.SetValue(model, Convert.ToInt32(value), null);
                                }
                                else if (p.PropertyType.GenericTypeArguments != null && (p.PropertyType.GenericTypeArguments.Where(a => a.Name.ToUpper() == ("Decimal").ToUpper()).Count() > 0 || p.PropertyType.Name.ToUpper() == "Decimal".ToUpper()))
                                {
                                    p.SetValue(model, Convert.ToDecimal(value), null);
                                }
                                else
                                {
                                    p.SetValue(model, value, null);
                                }
                            }
                        }

                    //}
                    //catch (Exception asdas)
                    //{

                    //    throw;
                    //}
                }
                list.Add(model);
            }
            return list;


        }
    }
}