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

namespace Admin.NET.Express.Strategy.SFExpress.Dto;
public class SFExpressOutput
{



}

public class RootobjectPrint
{
    public string apiErrorMsg { get; set; }
    public string apiResponseID { get; set; }
    public string apiResultCode { get; set; }
    public string apiResultData { get; set; }
    public string accessToken { get; set; }

    
}

public class Rootobject
{
    public string apiErrorMsg { get; set; }
    public string apiResponseID { get; set; }
    public string apiResultCode { get; set; }
    public string apiResultData { get; set; }
}

public class Apiresultdata
{
    public string success { get; set; }
    public string errorCode { get; set; }
    public string errorMsg { get; set; }
    public Msgdata msgData { get; set; }
}

public class Msgdata
{
    public string orderId { get; set; }
    public string originCode { get; set; }
    public string destCode { get; set; }
    public int filterResult { get; set; }
    public string remark { get; set; }
    public string url { get; set; }
    public string paymentLink { get; set; }
    public string isUpstairs { get; set; }
    public string isSpecialWarehouseService { get; set; }
    public string mappingMark { get; set; }
    public string agentMailno { get; set; }
    public string returnExtraInfoList { get; set; }
    public Waybillnoinfolist[] waybillNoInfoList { get; set; }
    public Routelabelinfo[] routeLabelInfo { get; set; }
    public string contactInfoList { get; set; }
}

public class Waybillnoinfolist
{
    public int waybillType { get; set; }
    public string waybillNo { get; set; }
}

public class Routelabelinfo
{
    public string code { get; set; }
    public Routelabeldata routeLabelData { get; set; }
    public string message { get; set; }
}

public class Routelabeldata
{
    public string waybillNo { get; set; }
    public string sourceTransferCode { get; set; }
    public string sourceCityCode { get; set; }
    public string sourceDeptCode { get; set; }
    public string sourceTeamCode { get; set; }
    public string destCityCode { get; set; }
    public string destDeptCode { get; set; }
    public string destDeptCodeMapping { get; set; }
    public string destTeamCode { get; set; }
    public string destTeamCodeMapping { get; set; }
    public string destTransferCode { get; set; }
    public string destRouteLabel { get; set; }
    public string proName { get; set; }
    public string cargoTypeCode { get; set; }
    public string limitTypeCode { get; set; }
    public string expressTypeCode { get; set; }
    public string codingMapping { get; set; }
    public string codingMappingOut { get; set; }
    public string xbFlag { get; set; }
    public string printFlag { get; set; }
    public string twoDimensionCode { get; set; }
    public string proCode { get; set; }
    public string printIcon { get; set; }
    public string abFlag { get; set; }
    public string destPortCode { get; set; }
    public string destCountry { get; set; }
    public string destPostCode { get; set; }
    public string goodsValueTotal { get; set; }
    public string currencySymbol { get; set; }
    public string cusBatch { get; set; }
    public string goodsNumber { get; set; }
    public string errMsg { get; set; }
    public string checkCode { get; set; }
    public string proIcon { get; set; }
    public string fileIcon { get; set; }
    public string fbaIcon { get; set; }
    public string icsmIcon { get; set; }
    public string destGisDeptCode { get; set; }
    public string newIcon { get; set; }
}









public class RootobjectPrint_obj
{
    public Obj obj { get; set; }
    public string requestId { get; set; }
    public bool success { get; set; }
}

public class Obj
{
    public string clientCode { get; set; }
    public string fileType { get; set; }
    public File[] files { get; set; }
    public string templateCode { get; set; }
}

public class File
{
    public int areaNo { get; set; }
    public int documentSize { get; set; }
    public int pageCount { get; set; }
    public int pageNo { get; set; }
    public int seqNo { get; set; }
    public string token { get; set; }
    public string url { get; set; }
    public string waybillNo { get; set; }
}
