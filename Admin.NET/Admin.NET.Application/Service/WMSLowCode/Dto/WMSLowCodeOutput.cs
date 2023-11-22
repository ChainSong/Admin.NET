namespace Admin.NET.Application;

    /// <summary>
    /// WMSLowCode输出参数
    /// </summary>
    public class WMSLowCodeOutput
    {
       /// <summary>
       /// Id
       /// </summary>
       public long Id { get; set; }
    
       /// <summary>
       /// Name
       /// </summary>
       public string? MenuName { get; set; }
    
       /// <summary>
       /// UICode
       /// </summary>
       public string? UICode { get; set; }

       public string? SQLCode { get; set; }

    /// <summary>
    /// UIType
    /// </summary>
    public string? UIType { get; set; }
    
       /// <summary>
       /// DataSource
       /// </summary>
       public string? DataSource { get; set; }
    
       /// <summary>
       /// Creator
       /// </summary>
       public string? Creator { get; set; }
    
       /// <summary>
       /// CreationTime
       /// </summary>
       public DateTime? CreationTime { get; set; }
    
    }
 

