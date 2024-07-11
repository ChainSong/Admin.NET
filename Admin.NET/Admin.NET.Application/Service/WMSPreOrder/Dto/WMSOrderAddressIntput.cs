namespace Admin.NET.Application;

/// <summary>
/// WMS_PreOrder输出参数
/// </summary>
public class WMSOrderAddressIntput
{

    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public long PreOrderId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? PreOrderNumber { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExternOrderNumber { get; set; }

    /// <summary>
    /// 联系人
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? CompanyName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? AddressTag { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮编
    /// </summary>
    public string? ZipCode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? City { get; set; }

    /// <summary>
    /// 
    /// </summary> 
    public string? Country { get; set; }

    /// <summary>
    /// 
    /// </summary> 
    public string? County { get; set; }

    /// <summary>
    /// 
    /// </summary> 
    public string? Address { get; set; }

    /// <summary>
    /// 是否返回签回单 （签单返还）的运单号， 支持以下值： 1：要求 0：不要求
    /// </summary>
    public int? IsSignBack { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExpressCompany { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? ExpressNumber { get; set; }

    /// <summary>
    /// 付款方式，支持以下值： 1:寄方付 2:收方付 3:第三方付
    /// </summary>
    public string? payMethod { get; set; }

    /// <summary>
    /// 快件自取，支持以下值： 1：客户同意快件自取 0：客户不同意快件自取
    /// </summary>
    public string? isOneselfPickup { get; set; }

    /// <summary>
    /// 快件产品类别表 https://open.sf-express.com/developSupport/734349?activeIndex=324604
    /// </summary>
    public string? expressTypeId { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public string Creator { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// 
    /// </summary>

    public string Updator { get; set; }


    /// <summary>
    /// 
    /// </summary>

    public DateTime? UpdateTime { get; set; }
     
}


