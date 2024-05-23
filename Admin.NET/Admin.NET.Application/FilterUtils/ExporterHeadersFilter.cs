// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core;
using Magicodes.ExporterAndImporter.Core.Filters;
using Magicodes.ExporterAndImporter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.FilterUtils;

/// <summary>
/// 通用导出筛选器
/// </summary>
public class ExporterHeadersFilter : IExporterHeadersFilter
{
    public IList<ExporterHeaderInfo> Filter(IList<ExporterHeaderInfo> exporterHeaderInfos)
    {
        foreach (var item in exporterHeaderInfos)
        {

            ////装卸台映射
            //if (item.PropertyName == "DcqyCode")
            //{
            //    var res = App.GetService<ISqlSugarClient>().Queryable<Dim_Dcqy_Info>().Where(x => x.Status == StatusEnum.Enable).ToDictionary(x => x.DcqyName, x => x.DcqyCode);

            //    item.MappingValues = res ?? new Dictionary<string, object>();
            //}
            //else if (item.PropertyName == "DcqyType")   //装卸类型
            //{

            //    var res = App.GetService<ISqlSugarClient>().Queryable<SysDictData>().Where(x => x.DictType.Code.Equals("dcqy_type") && x.Status == StatusEnum.Enable).ToDictionary(x => x.Value, x => x.Code);

            //    item.MappingValues = res ?? new Dictionary<string, object>();
            //}
        }

        return exporterHeaderInfos;
    }


}