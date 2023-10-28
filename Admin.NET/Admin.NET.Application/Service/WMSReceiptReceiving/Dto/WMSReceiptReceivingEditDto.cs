
using System;

using System.ComponentModel.DataAnnotations;
using MyProject;
namespace MyProject.ReceiptReceivingCore.Dto
{	
	/// <summary>
	/// WMS_ReceiptReceiving的列表DTO
	/// <see cref="WMS_ReceiptReceiving"/>
	/// </summary>
    public class WMSReceiptReceivingEditDto
    {
		/// <summary>
        /// Id 
        /// </summary>
        public long Id { get; set; }

		/// <summary>
		/// 字段ASNDetailId
		/// </summary>
		public long ASNDetailId { get; set; }
		/// <summary>
		/// 字段ReceiptId
		/// </summary>
		public long ReceiptId { get; set; }
		/// <summary>
		/// 字段ReceiptDetailId
		/// </summary>
		public long ReceiptDetailId { get; set; }
		/// <summary>
		/// 字段ReceiptNumber
		/// </summary>
		public string ReceiptNumber { get; set; }
		/// <summary>
		/// 字段ExternReceiptNumber
		/// </summary>
		public string ExternReceiptNumber { get; set; }
		/// <summary>
		/// 字段ASNNumber
		/// </summary>
		public string ASNNumber { get; set; }
		/// <summary>
		/// 字段CustomerId
		/// </summary>
		public long CustomerId { get; set; }
		/// <summary>
		/// 字段CustomerName
		/// </summary>
		public string CustomerName { get; set; }
		/// <summary>
		/// 字段WarehouseId
		/// </summary>
		public long WarehouseId { get; set; }
		/// <summary>
		/// 字段WarehouseName
		/// </summary>
		public string WarehouseName { get; set; }
		/// <summary>
		/// 字段ReceiptReceivingStatus
		/// </summary>
		public int ReceiptReceivingStatus { get; set; }

		/// <summary>
		/// 用来区分标记（多货，缺货，串货）
		/// </summary>
		public int GoodsStatus { get; set; }
		/// <summary>
		/// 字段LineNumber
		/// </summary>
		public string LineNumber { get; set; }
		/// <summary>
		/// 字段SKU
		/// </summary>
		public string SKU { get; set; }
		/// <summary>
		/// 字段UPC
		/// </summary>
		public string UPC { get; set; }
		/// <summary>
		/// 字段GoodsType
		/// </summary>
		public string GoodsType { get; set; }
		/// <summary>
		/// 字段GoodsName
		/// </summary>
		public string GoodsName { get; set; }
		/// <summary>
		/// 字段BoxCode
		/// </summary>
		public string BoxCode { get; set; }
		/// <summary>
		/// 字段TrayCode
		/// </summary>
		public string TrayCode { get; set; }
		/// <summary>
		/// 字段BatchCode
		/// </summary>
		public string BatchCode { get; set; }
		/// <summary>
		/// 字段ReceivedQty
		/// </summary>
		public double ReceivedQty { get; set; }
		/// <summary>
		/// 字段UnitCode
		/// </summary>
		public string UnitCode { get; set; }
		/// <summary>
		/// 字段Onwer
		/// </summary>
		public string Onwer { get; set; }
		/// <summary>
		/// 字段Area
		/// </summary>
		public string Area { get; set; }
		/// <summary>
		/// 字段Location
		/// </summary>
		public string Location { get; set; }
		/// <summary>
		/// 字段Remark
		/// </summary>
		public string Remark { get; set; }
		/// <summary>
		/// 字段Creator
		/// </summary>
		public string Creator { get; set; }
		/// <summary>
		/// 字段CreationTime
		/// </summary>
		public DateTime CreationTime { get; set; }
		/// <summary>
		/// 字段Updator
		/// </summary>
		public string Updator { get; set; }
		/// <summary>
		/// 字段Str1
		/// </summary>
		public string Str1 { get; set; }
		/// <summary>
		/// 字段Str2
		/// </summary>
		public string Str2 { get; set; }
		/// <summary>
		/// 字段Str3
		/// </summary>
		public string Str3 { get; set; }
		/// <summary>
		/// 字段Str4
		/// </summary>
		public string Str4 { get; set; }
		/// <summary>
		/// 字段Str5
		/// </summary>
		public string Str5 { get; set; }
		/// <summary>
		/// 字段Str6
		/// </summary>
		public string Str6 { get; set; }
		/// <summary>
		/// 字段Str7
		/// </summary>
		public string Str7 { get; set; }
		/// <summary>
		/// 字段Str8
		/// </summary>
		public string Str8 { get; set; }
		/// <summary>
		/// 字段Str9
		/// </summary>
		public string Str9 { get; set; }
		/// <summary>
		/// 字段Str10
		/// </summary>
		public string Str10 { get; set; }
		/// <summary>
		/// 字段Str11
		/// </summary>
		public string Str11 { get; set; }
		/// <summary>
		/// 字段Str12
		/// </summary>
		public string Str12 { get; set; }
		/// <summary>
		/// 字段Str13
		/// </summary>
		public string Str13 { get; set; }
		/// <summary>
		/// 字段Str14
		/// </summary>
		public string Str14 { get; set; }
		/// <summary>
		/// 字段Str15
		/// </summary>
		public string Str15 { get; set; }
		/// <summary>
		/// 字段Str16
		/// </summary>
		public string Str16 { get; set; }
		/// <summary>
		/// 字段Str17
		/// </summary>
		public string Str17 { get; set; }
		/// <summary>
		/// 字段Str18
		/// </summary>
		public string Str18 { get; set; }
		/// <summary>
		/// 字段Str19
		/// </summary>
		public string Str19 { get; set; }
		/// <summary>
		/// 字段Str20
		/// </summary>
		public string Str20 { get; set; }
		
		/* 这里创建自己的代码 */
	}
}