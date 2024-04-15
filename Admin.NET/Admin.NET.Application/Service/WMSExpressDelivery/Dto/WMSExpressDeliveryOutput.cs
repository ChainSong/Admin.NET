namespace Admin.NET.Application;

    /// <summary>
    /// WMSExpressDelivery输出参数
    /// </summary>
    public class WMSExpressDeliveryOutput
    {

    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long? TenantId { get; set; }



    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? CustomerId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? WarehouseId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? WarehouseName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? OrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? OrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExternOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public long? PackageId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PackageNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExpressNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExpressCompany { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double? Weight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double? Volume { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderCompany { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderProvince { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderCity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderContact { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderTel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderCountry { get; set; }

    public string? SenderCounty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderAddress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? SenderPostCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsProvince { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsCity { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsCountry { get; set; }

    public string? RecipientsCounty { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsCompany { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsContact { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsTel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsAddress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int? Status { get; set; }

    /// <summary>
    /// 成功失败标识 true-成功;false-失败（YD）
    /// </summary>
    public string? Success { get; set; }

    /// <summary>
    /// 成功或失败编码200请求成功，S01订单报文不合法，S02数字签名不匹配，S03没有剩余单号，S04接口请求参数为空：logistics_interface, data_digest或clientId，S05唯品会专用，S06请求太快，S07url解码失败，S08订单号重复：订单号+客户编码+orderType全部重复则为重复，S09数据入库异常（YD）
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 三段码
    /// </summary>
    public string? ShortAddress { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime? PrintTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PrintPersonnel { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? RecipientsPostCode { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public string? Creator { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public double? EstimatedPrice { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double? ActualPrice { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public double? Cost { get; set; }




    /// <summary>
    /// 
    /// </summary>
    public DateTime? UpdateTime { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Updator { get; set; }
}
 

