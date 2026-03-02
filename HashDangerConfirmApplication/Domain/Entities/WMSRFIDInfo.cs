using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlaApplication.Models;

namespace TaskPlaApplication.Domain.Entities;
    /// <summary>
    /// RFID 信息表
    /// </summary>
    [SugarTable("WMS_RFIDInfo", "RFID 信息表")]
    public class WMSRFIDInfo : ITenantIdFilter
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
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? ReceiptNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? ExternReceiptNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? ASNNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? ASNDetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? ReceiptDetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? ReceiptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? CustomerId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? CustomerName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? WarehouseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? WarehouseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? SKU { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? GoodsType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? ReceiptPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public DateTime? ReceiptTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? OrderNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? ExternOrderNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? PickTaskNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? PackageNumber { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? PreOrderId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? PreOrderDetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public long? OrderDetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? OrderPerson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public DateTime? OrderTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public int? Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public double? Qty { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 80)]
        public string Sequence { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 80)]
        public string RFID { get; set; }
        [SugarColumn(ColumnDescription = "", Length = 80)]
        public string Link { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string BatchCode { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string PoCode { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string SoCode { get; set; }

        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string SnCode { get; set; }


        [SugarColumn(ColumnDescription = "", Length = 80)]
        public string Remark { get; set; }

        [SugarColumn(ColumnDescription = "")]
        public int PrintNum { get; set; }

        [SugarColumn(ColumnDescription = "")]
        public DateTime? PrintTime { get; set; }

        [SugarColumn(ColumnDescription = "")]
        public string? PrintPerson { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string Creator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [SugarColumn(ColumnDescription = "")]
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Updator { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Str1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Str2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Str3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Str4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "", Length = 50)]
        public string? Str5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public DateTime? DateTime1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public DateTime? DateTime2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public int? Int1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnDescription = "")]
        public int? Int2 { get; set; }

    }

