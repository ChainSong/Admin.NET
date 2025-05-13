
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos.Enum;
using XAct;
using Admin.NET.Application.Enumerate;
using Nest;
using SpliteToBox.Common;
using static SKIT.FlurlHttpClient.Wechat.TenpayV3.Models.UploadMarketingShoppingReceiptResponse.Types;

namespace Admin.NET.Application.Strategy
{
    public class ReceiptRFIDHachStrategy : IReceiptRFIDInterface
    {
        public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
        public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public ReceiptRFIDHachStrategy()
        {

        }
        //默认方法
        /// <summary>
        /// 创建RFID打印序列
        /// </summary>
        /// <param name="receipts"></param>
        /// <param name="entityASN"></param>
        /// <returns></returns>
        public async Task<Response<List<OrderStatusDto>>> SaveRFID(UpdateWMSReceiptInput input)
        {

            List<WMSReceipt> receipts = new List<WMSReceipt>();
            List<WMSASN> entityASN = new List<WMSASN>();
            receipts = _repReceipt.AsQueryable().Where(a => input.Id == (a.Id)).ToList();
            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //判断是不是已经生成过
            var check = _repRFIDInfo.AsQueryable().Where(a => a.ReceiptId == input.Id).Any();
            if (check)
            {
                response.Code = StatusCode.Error;
                response.Msg = "已经生成过RFID";
                return response;
            }

            //获取需要生成RFID序列的入库单明细信息
            var rfidReceiptDetails = _repReceiptDetail.AsQueryable()
                .Where(a => receipts.Select(b => b.Id).Contains(a.ReceiptId)
                 && SqlFunc.Subqueryable<WMSProduct>().Where(c => c.SKU == a.SKU && c.IsRFID == 1 && c.CustomerId == a.CustomerId).Any()
            ).ToList();

            //获取product
            var products = _repProduct.AsQueryable().Where(a => a.IsRFID == 1 && rfidReceiptDetails.Select(b => b.SKU).Contains(a.SKU)).ToList();
            if (rfidReceiptDetails == null || rfidReceiptDetails.Count == 0)
            {
                response.Code = StatusCode.Error;
                response.Msg = "没有需要生成RFID的明细";
                return response;
            }
            //构建RFID序列容器
            List<WMSRFIDInfo> rfidInfos = new List<WMSRFIDInfo>();
            //根据入库单号生成RFID打印序列号
            //SKU唯一序列号根据SKUID+客户编码+唯一序列+三位随机数+随机数%10
            foreach (var item in rfidReceiptDetails)
            {
                for (var i = 0; i < item.ReceivedQty; i++)
                {
                    var skuId = products.Where(a => a.SKU == item.SKU).First().Id;
                    var customerCode = item.CustomerId;
                    var warehouseCode = item.WarehouseId;
                    long uniqueCode;
                    RedisCacheHelper.IncrementValue("RFID_UNIQUE_CODE", out uniqueCode);
                    if (uniqueCode < 0)
                    {
                        return new Response<List<OrderStatusDto>>() { Code = StatusCode.Error, Msg = "序列号生成失败" };
                    }
                    var randomCode = new Random(Guid.NewGuid().GetHashCode()).Next(100, 999);
                    var rfidCode = skuId.ToString().PadLeft(7, '0') + "" + customerCode.ToString().PadLeft(3, '0') + "" + warehouseCode.ToString().PadLeft(3, '0') + "" + uniqueCode.ToString().PadLeft(7, '0') + "" + randomCode + "" + (randomCode % 10).ToString();
                    var rfidInfo = item.Adapt<WMSRFIDInfo>();
                    rfidInfo.ReceiptDetailId = item.Id;
                    rfidInfo.Id = 0;
                    rfidInfo.Creator = _userManager.Account;
                    rfidInfo.CreationTime = DateTime.Now;
                    rfidInfo.ReceiptPerson = _userManager.Account;
                    rfidInfo.ReceiptTime = DateTime.Now;
                    rfidInfo.Qty = 1;
                    rfidInfo.Status = (int)RFIDStatusEnum.初始化;
                    rfidInfo.Sequence = rfidCode;
                    rfidInfo.SnCode = rfidCode;
                    rfidInfo.RFID = rfidCode;
                    rfidInfo.Link = "";
                    rfidInfo.Remark = "";
                    rfidInfos.Add(rfidInfo);
                }
            }
            //插入入库单
            ////开始插入订单（排除数量为0的明细行）
            if (rfidInfos.Count > 0)
            {
                await _repRFIDInfo.InsertRangeAsync(rfidInfos);
            }
            response.Code = StatusCode.Success;
            response.Msg = "成功";
            return response;
        }


    }

    //public class PropertyValue<T>
    //{
    //    private static ConcurrentDictionary<string, MemberGetDelegate> _memberGetDelegate = new ConcurrentDictionary<string, MemberGetDelegate>();
    //    delegate object MemberGetDelegate(T obj);
    //    public PropertyValue(T obj)
    //    {
    //        Target = obj;
    //    }
    //    public T Target { get; private set; }
    //    public object Get(string name)
    //    {
    //        MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
    //        return memberGet(Target);
    //    }
    //    private MemberGetDelegate BuildDelegate(string name)
    //    {
    //        Type type = typeof(T);
    //        PropertyInfo property = type.GetProperty(name);
    //        return (MemberGetDelegate)Delegate.CreateDelegate(typeof(MemberGetDelegate), property.GetGetMethod());
    //    }
    //}
}