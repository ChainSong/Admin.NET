// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service.WMSReport.Dto;
public class ReturnResult
{
    /// <summary>
    /// 返回条数
    /// </summary>
    public int count { get; set; }
    /// <summary>
    /// 是否成功
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    /// 返回消息提示
    /// </summary>
    public string msg { get; set; }
    /// <summary>
    /// 额外信息携带
    /// </summary>
    public string? OtherInformation { get; set; }
    /// <summary>
    /// 数据实体
    /// </summary>
    public object data { get; set; }
    //数据实体备份
    public object dataBackup { get; set; }

    //备注：4个对象，每个对象对应两个统计图，共8个
    /// <summary>
    /// 客户：出入库，出入库金额数据
    /// </summary>
    public object CustOrderReceiptAmount { get; set; }

    /// <summary>
    /// Top5货号，出库数量，出库金额
    /// </summary>
    public object TopFiveOrderCountAmount { get; set; }

    /// <summary>
    /// Top3客户，出库数量，出库金额
    /// </summary>
    public object TopThreeOrderCountAmount { get; set; }


    /// <summary>
    /// Top5客户，出库数量，出库金额
    /// </summary>
    public object TopFiveCustOrderCountGdp { get; set; }

    /// <summary>
    /// 入库总金额、数量
    /// </summary>
    public object AllMoneyQtyIn { get; set; }

    /// <summary>
    /// 出库总金额、数量
    /// </summary>
    public object AllMoneyQtyOut { get; set; }


    //入库总数量
    public int InTotalCount { get; set; }
    //入库总金额
    public int InTotalAmount { get; set; }
    //出库总数量
    public int OutTotalCount { get; set; }
    //出库总金额
    public int OutTotalAmount { get; set; }

}
