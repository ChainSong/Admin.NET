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
public class CustOrderReceiptAmountDto
{
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    /// <summary>
    /// 入库数量
    /// </summary>
    public int WoSumQty { get; set; }
    /// <summary>
    /// 入库金额
    /// </summary>
    public int WoSumPrice { get; set; }
    /// <summary>
    /// 出库数量
    /// </summary>
    public int ReSumQty { get; set; }
    /// <summary>
    /// 出库金额
    /// </summary>
    public int ReSumPrice { get; set; }
}



//二版本大屏实体

//左侧1 table
public class CustOrderReceiptAmountDtoTwo
{
    /// <summary>
    /// 客户名称
    /// </summary>
    public string CustomerName { get; set; }
    /// <summary>
    /// 入库数量
    /// </summary>
    public int WoSumQty { get; set; }
    /// <summary>
    /// 入库金额
    /// </summary>
    public int WoSumPrice { get; set; }
    /// <summary>
    /// 出库数量
    /// </summary>
    public int ReSumQty { get; set; }
    /// <summary>
    /// 出库金额
    /// </summary>
    public int ReSumPrice { get; set; }

    /// <summary>
    /// 库存数量
    /// </summary>
    public int AllSumQty { get; set; }
    /// <summary>
    /// 库存金额
    /// </summary>
    public int AllSumPrice { get; set; }
}

//左侧2 table
public class TopFiveOrderCountAmountDtoTwo
{
    //SKU OrderQty TotalMoney
    public string CustomerName { get; set; }
    public int countslast { get; set; }
    public int countsnow { get; set; }
}

//左侧3 table
public class TopThreeOrderCountAmountDtoTwo
{
    public string BigName { get; set; }
    public int counts { get; set; }
    public string Percentage { get; set; }
}


//顶部两个百分比
public class TopNumTwo
{
    public string PricePercentage { get; set; }
    public string QtyPercentage { get; set; }
}