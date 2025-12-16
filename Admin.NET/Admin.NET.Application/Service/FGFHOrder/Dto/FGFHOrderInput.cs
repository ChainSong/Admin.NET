using Admin.NET.Core;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Admin.NET.Application;

    /// <summary>
    /// 福光出库回传基础输入参数
    /// </summary>
    public class FGFHOrderBaseInput
    {
        /// <summary>
        /// OutStockMID
        /// </summary>
        public virtual long OutStockMID { get; set; }
        
        /// <summary>
        /// OutStockNo
        /// </summary>
        public virtual string OutStockNo { get; set; }
        
        /// <summary>
        /// OutStockNumber
        /// </summary>
        public virtual string? OutStockNumber { get; set; }
        
        /// <summary>
        /// OutStockDate
        /// </summary>
        public virtual DateTime OutStockDate { get; set; }
        
        /// <summary>
        /// OutStockType
        /// </summary>
        public virtual string OutStockType { get; set; }
        
        /// <summary>
        /// DownloadURL
        /// </summary>
        public virtual string? DownloadURL { get; set; }
        
        /// <summary>
        /// ContractNo
        /// </summary>
        public virtual string? ContractNo { get; set; }
        
        /// <summary>
        /// Addressee
        /// </summary>
        public virtual string? Addressee { get; set; }
        
        /// <summary>
        /// Phone
        /// </summary>
        public virtual string? Phone { get; set; }
        
        /// <summary>
        /// Address
        /// </summary>
        public virtual string? Address { get; set; }
        
        /// <summary>
        /// IsReturn
        /// </summary>
        public virtual int? IsReturn { get; set; }
        
        /// <summary>
        /// ReturnDate
        /// </summary>
        public virtual DateTime? ReturnDate { get; set; }
        
        /// <summary>
        /// Reason
        /// </summary>
        public virtual string? Reason { get; set; }
        
        /// <summary>
        /// Str1
        /// </summary>
        public virtual string? Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public virtual string? Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public virtual string? Str3 { get; set; }
        
        /// <summary>
        /// Int1
        /// </summary>
        public virtual int? Int1 { get; set; }
        
        /// <summary>
        /// Int2
        /// </summary>
        public virtual int? Int2 { get; set; }
        
        /// <summary>
        /// Int3
        /// </summary>
        public virtual int? Int3 { get; set; }
        
        /// <summary>
        /// Datetime1
        /// </summary>
        public virtual DateTime? Datetime1 { get; set; }
        
        /// <summary>
        /// Datetime2
        /// </summary>
        public virtual DateTime? Datetime2 { get; set; }
        
        /// <summary>
        /// Datetime3
        /// </summary>
        public virtual DateTime? Datetime3 { get; set; }
        
    }

    /// <summary>
    /// 福光出库回传分页查询输入参数
    /// </summary>
    public class FGFHOrderInput : BasePageInput
    {
        /// <summary>
        /// OutStockMID
        /// </summary>
        public long OutStockMID { get; set; }
        
        /// <summary>
        /// OutStockNo
        /// </summary>
        public string OutStockNo { get; set; }
        
        /// <summary>
        /// OutStockNumber
        /// </summary>
        public string? OutStockNumber { get; set; }
        
        /// <summary>
        /// OutStockDate
        /// </summary>
        public DateTime OutStockDate { get; set; }
        
        /// <summary>
         /// OutStockDate范围
         /// </summary>
         public List<DateTime?> OutStockDateRange { get; set; } 
        /// <summary>
        /// OutStockType
        /// </summary>
        public string OutStockType { get; set; }
        
        /// <summary>
        /// DownloadURL
        /// </summary>
        public string? DownloadURL { get; set; }
        
        /// <summary>
        /// ContractNo
        /// </summary>
        public string? ContractNo { get; set; }
        
        /// <summary>
        /// Addressee
        /// </summary>
        public string? Addressee { get; set; }
        
        /// <summary>
        /// Phone
        /// </summary>
        public string? Phone { get; set; }
        
        /// <summary>
        /// Address
        /// </summary>
        public string? Address { get; set; }
        
        /// <summary>
        /// IsReturn
        /// </summary>
        public int? IsReturn { get; set; }
        
        /// <summary>
        /// ReturnDate
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        
        /// <summary>
         /// ReturnDate范围
         /// </summary>
         public List<DateTime?> ReturnDateRange { get; set; } 
        /// <summary>
        /// Reason
        /// </summary>
        public string? Reason { get; set; }
        
        /// <summary>
        /// Str1
        /// </summary>
        public string? Str1 { get; set; }
        
        /// <summary>
        /// Str2
        /// </summary>
        public string? Str2 { get; set; }
        
        /// <summary>
        /// Str3
        /// </summary>
        public string? Str3 { get; set; }
        
        /// <summary>
        /// Int1
        /// </summary>
        public int? Int1 { get; set; }
        
        /// <summary>
        /// Int2
        /// </summary>
        public int? Int2 { get; set; }
        
        /// <summary>
        /// Int3
        /// </summary>
        public int? Int3 { get; set; }
        
        /// <summary>
        /// Datetime1
        /// </summary>
        public DateTime? Datetime1 { get; set; }
        
        /// <summary>
         /// Datetime1范围
         /// </summary>
         public List<DateTime?> Datetime1Range { get; set; } 
        /// <summary>
        /// Datetime2
        /// </summary>
        public DateTime? Datetime2 { get; set; }
        
        /// <summary>
         /// Datetime2范围
         /// </summary>
         public List<DateTime?> Datetime2Range { get; set; } 
        /// <summary>
        /// Datetime3
        /// </summary>
        public DateTime? Datetime3 { get; set; }
        
        /// <summary>
         /// Datetime3范围
         /// </summary>
         public List<DateTime?> Datetime3Range { get; set; } 
    }

    /// <summary>
    /// 福光出库回传增加输入参数
    /// </summary>
    public class AddFGFHOrderInput : FGFHOrderBaseInput
    {
    }

    /// <summary>
    /// 福光出库回传删除输入参数
    /// </summary>
    public class DeleteFGFHOrderInput : BaseIdInput
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required(ErrorMessage = "ID不能为空")]
        public long ID { get; set; }
        
    }

    /// <summary>
    /// 福光出库回传更新输入参数
    /// </summary>
    public class UpdateFGFHOrderInput : FGFHOrderBaseInput
    {
        /// <summary>
        /// ID
        /// </summary>
        [Required(ErrorMessage = "ID不能为空")]
        public long ID { get; set; }
        
    }

    /// <summary>
    /// 福光出库回传主键查询输入参数
    /// </summary>
    public class QueryByIdFGFHOrderInput : DeleteFGFHOrderInput
    {

    }
