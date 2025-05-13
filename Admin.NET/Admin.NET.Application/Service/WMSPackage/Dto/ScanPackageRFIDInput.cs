using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

/// <summary>
/// WMSPackage基础输入参数
/// </summary>
public class ScanPackageRFIDInput
{
    public string Input { get; set; }
    public string SKU { get; set; }
    public string SN { get; set; }
    public string Lot { get; set; }
    public string AcquisitionData { get; set; }

    public string GoodsName { get; set; }
    public string GoodsType { get; set; }

    /// <summary>
    /// 箱类型
    /// </summary>
    public string BoxType { get; set; }
    /// <summary>
    /// 拣货单号
    /// </summary>
    public string PickTaskNumber { get; set; }

    /// <summary>
    /// 包装单号
    /// </summary>
    public string PackageNumber { get; set; }
    /// <summary>
    /// 公司信息
    /// </summary>
    public long CustomerId { get; set; }
    public string CustomerName { get; set; }

    /// <summary>
    /// 仓库信息
    /// </summary>
    public long WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    /// <summary>
    /// 快递公司
    /// </summary>
    public string ExpressCompany { get; set; }

    /// <summary>
    /// 快递单号
    /// </summary>
    public string ExpressNumber { get; set; }


    //public string sku { get; set; }
    /// <summary>
    /// 重量
    /// </summary>
    public double Weight { get; set; }

    public List<string> RFIDs { get; set; }
    public List<WMSRFIDInfo> RFIDInfo { get; set; }
    public string RFIDStr { get; set; }


}
