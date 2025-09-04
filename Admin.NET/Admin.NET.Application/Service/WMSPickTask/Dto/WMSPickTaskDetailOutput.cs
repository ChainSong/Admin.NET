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

namespace Admin.NET.Application.Service;
public class WMSPickTaskDetailOutput
{
    /// <summary>
    /// 租户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "租户Id", IsOnlyIgnoreUpdate = true)]
    public virtual long? TenantId { get; set; }


    /// <summary>
    /// Id
    /// </summary>
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public virtual long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
   
    public long PickTaskId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long InventoryId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long OrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long OrderDetailId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long OrderDetailBomId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public long WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string ExternOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string? PreOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string OrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public int PickStatus { get; set; }

    /// <summary>
    /// 
    /// </summary>
    //[SugarColumn(ColumnDescription = "")]
    //public DateTime? PickTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? PickBoxNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? PickPersonnel { get; set; }



    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? PickTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
   
    public double PickQty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string Area { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string Location { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string SKU { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string UPC { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string GoodsType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string GoodsName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string UnitCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string Onwer { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string BoxCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string TrayCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    
    public string BatchCode { get; set; }


    
    public string LotCode { get; set; }

    
    public string PoCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? SoCode { get; set; }


    
    public string Weight { get; set; }


    
    public string Volume { get; set; }


    /// <summary>
    /// 
    /// </summary>
    
   
    public double Qty { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// 
    /// </summary>
 
    public string? Remark { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Updator { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? UpdateTime { get; set; }

    public string IsSN { get; set; }
    public string CN805 { get; set; }


    /// <summary>
    /// 
    /// </summary>

    public string? Str1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str3 { get; set; }

    /// <summary>
    /// 
    /// </summary>
    
    public string? Str4 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public string? Str5 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? DateTime1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public DateTime? DateTime2 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public int? Int1 { get; set; }

    /// <summary>
    /// 
    /// </summary>
   
    public int? Int2 { get; set; }
}
