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

namespace Admin.NET.Express.Strategy.STOExpress.Dto.STOResponse;

#region 请求中通下单响应实体
public class STOExpressNumberRequestOutput
{
    /// <summary>
    ///  是否成功
    /// </summary>
    public bool success { get; set; }
    /// <summary>
    ///PARAM_INVALID
    ///QUERY_ROOKIE_EXCEPTION
    ///AddOrderFailed
    ///SendMobileError
    ///SendPhoneError
    ///SERVER_EXCEPTION
    ///TIME_OUT
    ///PrintCodeRepeat
    /// </summary>
    public string error { get; set; }
    /// <summary>
    /// waybillNo invalid:运单号不符合申通单号规则；
    /// ROUTING_INFO_QUERY_NO_REACHABLE: 
    /// 物流服务不支持派送；
    /// 该账号剩余面单库存不足，请联系合作网点充值；
    /// </summary>
    public string errorMsg { get; set; }

    public OrderCreateResponse data { get; set; }

}

#region 响应详细数据
public class OrderCreateResponse
{
    /// <summary>
    /// 订单号
    /// </summary>
    public string orderNo {  get; set; }

    /// <summary>
    /// 运单号
    /// </summary>
    public string waybillNo { get; set; }
    /// <summary>
    /// 大字/三段码
    /// </summary>
    public string bigWord { get; set; }
    /// <summary>
    /// 集包地
    /// </summary>
    public string packagePlace { get; set; }
    /// <summary>
    /// 客户订单号（调度订单时返回客户订单号，非调度订单不返回该值）
    /// </summary>
    public string sourceOrderId { get; set; }
    /// <summary>
    /// 安全号码
    /// </summary>
    public string safeNo { get; set; }
    /// <summary>
    /// 新四段码
    /// </summary>
    public string newBlockCode { get; set; }
}
#endregion

#endregion
