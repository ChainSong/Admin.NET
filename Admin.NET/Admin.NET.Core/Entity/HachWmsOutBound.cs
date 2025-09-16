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

namespace Admin.NET.Core.Entity;

[SugarTable("hach_wms_outBound", "出库对接数据表")]
public class HachWmsOutBound : EntityTenant
{
    /// <summary>
    /// 雪花id
    /// </summary>
    [SugarColumn(ColumnDescription = "雪花id", IsOnlyIgnoreUpdate = true, IsIdentity = true, IsPrimaryKey = true)]
    public override long Id { get; set; }
    //为销售单号与发运编号拼接
    [SugarColumn(ColumnDescription = "挑库单号",Length =120)]
    public string OrderNumber { get; set; }
    /// <summary>
    /// yyyy-MM-dd HH:mm:ss 
    /// </summary>
    [SugarColumn(ColumnDescription ="预计入库时间",Length =50)]
    public string ScheduleShippingDate { get; set; }
    //销售单号
    [SugarColumn(ColumnDescription = "销售单号", Length = 120)]
    public string SoNumber { get; set; }
    //oms合同号
    [SugarColumn(ColumnDescription = "oms合同号", Length = 120)]
    public string ContractNo { get; set; }
    [SugarColumn(ColumnDescription = "发运编号", Length = 120)]
    public string DocType { get; set; } = "SH Direct Sales Orders";
    //job号
    [SugarColumn(ColumnDescription = "发运编号", Length = 120)]
    public string DeliveryNumber { get; set; }
    [SugarColumn(ColumnDescription = "联系人", Length = 120)]
    public string ContactName { get; set; }
    [SugarColumn(ColumnDescription = "收货地址", Length = 120)]
    public string Address { get; set; }
    //such as :重庆四联技术进出口有限公司   可以作为货主概念
    [SugarColumn(ColumnDescription =("客户名称"))]
    public string CustomerName { get; set; }
    //such as :重庆四联技术进出口有限公司   可以作为货主概念
    [SugarColumn(ColumnDescription = ("联系电话"))]
    public string Telephone { get; set; }
    [SugarColumn(ColumnDescription = ("子仓库"))]
    public string Subinventory { get; set; }
    [SugarColumn(ColumnDescription = "最终用户", IsNullable = false)]
    public string? EndUserName { get; set; }
    [SugarColumn(ColumnDescription = "是否回传")]
    public bool? IsReturn { get; set; }= false;
    [SugarColumn(ColumnDescription = "回传时间")]
    public DateTime? ReturnDate { get; set; }

    [Navigate(NavigateType.OneToMany, nameof(HachWmsOutBoundDetail.OutBoundId))]
    public virtual List<HachWmsOutBoundDetail> items { get; set; }
}
