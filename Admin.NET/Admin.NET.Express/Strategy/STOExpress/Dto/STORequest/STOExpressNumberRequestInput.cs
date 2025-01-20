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

namespace Admin.NET.Express.Strategy.STExpress.Dto.STRequest;

#region 公共请求参数
public class STOExpressNumberRequestInput
{
    public string api_name { get; set; }//接口名称
    public string content { get; set; }//具体报文
    public string from_appkey { get; set; }//订阅方/请求发起方的应用key
    public string from_code { get; set; }//订阅方/请求发起方的应用资源code
    public string to_appkey { get; set; }
    public string to_code { get; set; }
    public string data_digest { get; set; }//报文签名。详情请参考：对接订阅API  https://open.sto.cn/#/help/gtknvy
}
#endregion

#region 在线请求电子面单实体内容

//接口名称: OMS_EXPRESS_ORDER_CREATE
//接口中文名称: 订单创建接口
//功能描述: 提供申通外部客户在线下单的API接口；该接口适用标准订单业务，散单业务，国际订单业务；附：该接口可提供'轨迹订阅'的增值服务
//申通平台介入文档：https://open.sto.cn/#/help/tz5gl0
//申通订单创建接口：https://open.sto.cn/#/apiDocument/OMS_EXPRESS_ORDER_CREATE

public class STOExpressRequestContent
{
    /// <summary>
    /// 订单号（客户系统自己生成，唯一）
    /// </summary>
    public string orderNo { get; set; }

    /// <summary>
    /// 订单来源（订阅服务时填写的来源编码）
    /// </summary>
    public string orderSource { get; set; }

    /// <summary>
    ///获取面单的类型
    ///（00-普通、03-国际、01-代收、02-到付、04-生鲜），
    ///默认普通业务，如果有其他业务先与业务方沟通清楚
    /// </summary>
    public string billType { get; set; }

    /// <summary>
    /// 订单类型（01-普通订单、02-调度订单）默认01-普通订单，
    /// 如果有散单业务需先业务方沟通清楚
    /// </summary>
    public string orderType { get; set; }
    /// <summary>
    /// 寄件人信息
    /// </summary>
    public SenderDO sender { get; set; }
    /// <summary>
    /// 收件人信息
    /// </summary>
    public Receiver receiver { get; set; }
    /// <summary>
    /// 包裹信息
    /// </summary>
    public CargoDO cargo { get; set; }
    /// <summary>
    /// 客户信息
    /// </summary>
    public CustomerDO customer { get; set; }
    /// <summary>
    /// 国际订单附属信息
    /// </summary>
    public InternationalAnnexDO internationalAnnex { get; set; }
    /// <summary>
    /// 运单号（下单前已获取运单号时必传，否则不传或传NULL）
    /// </summary>
    public string? waybillNo { get; set; }
    /// <summary>
    /// 代收货款金额，单位：元（代收货款业务时必填）
    /// </summary>
    public string? codValue { get; set; }
    /// <summary>
    /// 到付运费金额，单位：元（到付业务时必填）
    /// </summary>
    public string? freightCollectValue { get; set; }
    /// <summary>
    /// 时效类型（01-普通）
    /// </summary>
    public string? timelessType { get; set; }
    /// <summary>
    /// 产品类型 （01-普通、02-冷链、03-生鲜）
    /// </summary>
    public string? productType { get; set; }
    /// <summary>
    /// 增值服务
    /// </summary>
    public List<ServiceTypeList> serviceTypeList { get; set; }
    /// <summary>
    ///  拓展字段
    /// </summary>
    public List<KeyValuePair<string, string>> ExtendFieldMap { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? remark { get; set; }
    /// <summary>
    /// 快递流向（01-正向订单)默认01
    /// </summary>
    public string? expressDirection { get; set; }
    /// <summary>
    /// 创建原因（01-客户创建）默认01
    /// </summary>
    public string? createChannel { get; set; }

    /// <summary>
    /// 区域类型（01-国内）默认01
    /// </summary>
    public string? regionType { get; set; }


    /// <summary>
    ///保价模型（保价服务必填）
    /// </summary>
    public InsuredAnnexDo insuredAnnex { get; set; }
    /// <summary>
    /// 预估费用（散单业务使用）
    /// </summary>
    public string expectValue {  get; set; }
    /// <summary>
    ///支付方式（1-现付；2-到付；3-月结）
    /// </summary>
    public string payModel { get; set; }

}

#region 寄件人信息
public class SenderDO
{
    /// <summary>
    /// 寄件人名称--必填
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// 寄件人固定电话
    /// </summary>
    public string? tel { get; set; }

    /// <summary>
    /// 寄件人手机号码
    /// </summary>
    public string? mobile { get; set; }

    /// <summary>
    /// 邮编
    /// </summary>
    public string? postCode { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    public string? country { get; set; }

    /// <summary>
    /// 省
    /// </summary>
    public string province { get; set; }

    /// <summary>
    /// 市
    /// </summary>
    public string city { get; set; }

    /// <summary>
    /// 区
    /// </summary>
    public string area { get; set; }
    /// <summary>
    /// 镇
    /// </summary>
    public string? town { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    public string address { get; set; }
}
#endregion

#region 收件人信息
public class Receiver
{
    /// <summary>
    /// 收件人名称
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// 收件人固定电话
    /// </summary>
    public string? tel { get; set; }
    /// <summary>
    /// 收件人手机号码
    /// </summary>
    public string? mobile { get; set; }
    /// <summary>
    /// 邮编
    /// </summary>
    public string? postCode { get; set; }
    /// <summary>
    /// 国家
    /// </summary>
    public string? country { get; set; }
    /// <summary>
    /// 省
    /// </summary>
    public string province { get; set; }
    /// <summary>
    /// 区
    /// </summary>
    public string area { get; set; }
    /// <summary>
    /// 镇
    /// </summary>
    public string? towns { get; set; }
    /// <summary>
    /// 详细地址
    /// </summary>
    public string address { get; set; }
    /// <summary>
    /// 安全号码
    /// </summary>
    public string? safeNo { get; set; }
}
#endregion

#region 包裹信息
public class CargoDO
{
    /// <summary>
    /// 带电标识 （10/未知 20/带电 30/不带电）
    /// </summary>
    public string battery { get; set; }
    /// <summary>
    /// 物品类型（大件、小件、扁平件\文件）
    /// </summary>
    public string goodsType { get; set; }
    /// <summary>
    /// 物品名称
    /// </summary>
    public string goodsName { get; set; }
    /// <summary>
    /// 物品数量
    /// </summary>
    public int goodsCount { get; set; }
    /// <summary>
    /// 长
    /// </summary>
    public double spaceX { get; set; }
    /// <summary>
    /// 宽
    /// </summary>
    public double spaceY { get; set; }
    /// <summary>
    /// 高
    /// </summary>
    public double spaceZ { get; set; }
    /// <summary>
    /// 重量
    /// </summary>
    public double weight { get; set; }
    /// <summary>
    /// 商品金额
    /// </summary>
    public string? goodsAmount { get; set; }
    /// <summary>
    /// 小包信息（国际业务专用，其他业务不需要填写）
    /// </summary>
    public List<CargoItemDO> cargoItemList { get; set; }
}
#endregion

#region 小包信息
public class CargoItemDO
{
    /// <summary>
    /// 小包号
    /// </summary>
    public string? serialNumber { get; set; }
    /// <summary>
    /// 关联单号
    /// </summary>
    public string? referenceNumber { get; set; }
    /// <summary>
    /// 商品ID
    /// </summary>
    public string? productId { get; set; }
    /// <summary>
    /// 名称
    /// </summary>
    public string? name { get; set; }
    /// <summary>
    /// 单价
    /// </summary>
    public double? unitPrice { get; set; }
    /// <summary>
    /// 总价
    /// </summary>
    public double? amount { get; set; }
    /// <summary>
    /// 币种
    /// </summary>
    public string? currency { get; set; }
    /// <summary>
    /// 重量(kg)
    /// </summary>
    public double? weight { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? remark { get; set; }
}
#endregion

#region 客户信息
public class CustomerDO
{
    /// <summary>
    /// 网点编码必传，测试传值"siteCode":"666666"
    /// 示例值：666666
    /// </summary>
    public string SiteCode { get; set; }

    /// <summary>
    /// 客户编码，测试传值"customerName":"666666000001"
    /// 示例值：666666000001
    /// </summary>
    public string CustomerName { get; set; }

    /// <summary>
    /// 电子面单密码，测试传值"sitePwd":"abc123"
    /// 示例值：abc123
    /// </summary>
    public string SitePwd { get; set; }

    /// <summary>
    /// 月结客户编码（不传单号需调度才传月结编号），如果填写一般和客户编号值相同
    /// 示例值：9000000
    /// </summary>
    public string MonthCustomerCode { get; set; }
}
#endregion

#region 国际订单附属信息
public class InternationalAnnexDO
{
    /// <summary>
    /// 国际业务类型（01-国际进口，02-国际保税，03-国际直邮）
    /// 示例值：01
    /// 必填：是
    /// </summary>
    public string InternationalProductType { get; set; }

    /// <summary>
    /// 是否报关，默认为否
    /// 示例值：false
    /// 必填：是
    /// </summary>
    public bool CustomsDeclaration { get; set; }

    /// <summary>
    /// 发件人所在国家，国际件为必填字段
    /// 示例值：中国
    /// 必填：是
    /// </summary>
    public string SenderCountry { get; set; }

    /// <summary>
    /// 收件人所在国家，国际件为必填字段
    /// 示例值：俄罗斯
    /// 必填：是
    /// </summary>
    public string ReceiverCountry { get; set; }
}
#endregion

#region 增值服务
public class ServiceTypeList
{
    public string listItem {  get; set; }
}
#endregion

#region 拓展字段
public class extendFieldMap
{
    /// <summary>
    ///  注意事项:属性值有逗号等于号需过滤掉
    /// </summary>
    public string mapValue { get; set; }
}
#endregion

#region 指定网点揽收
public class AssignAnnex
{
    /// <summary>
    /// 指定取件的网点编号
    /// </summary>
    public string takeCompanyCode { get; set; }
    /// <summary>
    /// 指定取件的业务员编号
    /// （指定业务员时takeCompanyCode可传可不传，若传必须传正确，)
    /// 举例：寄件地址是上海，只能是指定上海业务员取件
    /// </summary>
    public string takeUserCode { get; set; }

}
#endregion

#region 保价模型（保价服务必填）
public class InsuredAnnexDo
{
    /// <summary>
    /// 保价金额，单位：元（保价服务时必填，精确到小数点后两位）
    /// </summary>
    public string insuredValue {  get; set; }
    /// <summary>
    ///物品价值，单位：元（保价服务时必填，精确到小数点后两位）
    /// </summary>
    public string goodsValue { get; set; }

}
#endregion

#endregion

#region 获取电子面单
#region 申通请求参数
#endregion

#region 申通响应参数
#endregion
#endregion

#region 取消订单
#region 申通请求参数
#endregion

#region 申通响应参数
#endregion
#endregion

