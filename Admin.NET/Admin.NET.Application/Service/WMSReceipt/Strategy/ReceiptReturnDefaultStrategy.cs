
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

namespace Admin.NET.Application.Strategy
{
    public class ReceiptReturnDefaultStrategy : IReceiptReturnInterface
    {

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public ReceiptReturnDefaultStrategy()
        {

        }
        //默认方法
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<DeleteWMSReceiptInput> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
            //先判断状态是否正常 是否允许回退
            var receipt = _repReceipt.AsQueryable().Where(a => request.Select(b => b.Id).Contains(a.Id));
            await receipt.ForEachAsync(a =>
            {
                if (a.ReceiptStatus != (int)ReceiptStatusEnum.新增)
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = a.ExternReceiptNumber,
                        SystemOrder = a.ASNNumber,
                        Type = a.ReceiptType,
                        StatusCode = StatusCode.Warning,
                        Msg = "状态异常"
                    });
                }

            });
            if (response.Data != null && response.Data.Count > 0)
            {
                response.Code = StatusCode.Warning;
                response.Msg = "状态异常";
                return response;
            }
            receipt.ForEach(r =>
            {
                var repASNDetailData = _repASNDetail.AsQueryable().Where(a => a.ASNId == r.ASNId).ToList();
                repASNDetailData.ForEach(a =>
                {
                    a.ReceivedQty = a.ReceivedQty - (_repReceiptDetail.AsQueryable().Where(b => a.Id == b.ASNDetailId && b.ReceiptId == r.Id).Sum(c => c.ReceivedQty));
                    a.Updator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                });
                _repASNDetail.UpdateRange(repASNDetailData);


                var asnDetail = _repASNDetail.AsQueryable().Where(b => b.ReceivedQty > 0 && b.ASNId == r.ASNId).ToList();
                if (asnDetail.Count > 0)
                {
                    //await _repASN.UpdateAsync(a => new WMSASN { ASNStatus = (int)ASNStatusEnum.部分转入库单 }, (a => ASNIds.Contains(a.Id)));
                    _repASN.Context.Updateable<WMSASN>()
                      .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.部分转入库单)
                      .Where(p => (p.Id) == r.ASNId)
                      .ExecuteCommand();
                }
                else
                {
                    //await _repASN.UpdateAsync(a => new WMSASN { ASNStatus = (int)ASNStatusEnum.新增 }, (a => ASNIds.Contains(a.Id)));
                    _repASN.Context.Updateable<WMSASN>()
                    .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.新增)
                    .Where(p => (p.Id) == r.ASNId)
                    .ExecuteCommand();
                }


            });

            //var ASNIds = await receipt.Select(b => b.ASNId).Distinct().ToListAsync();
            //先更新主表，在更新明细表
            //await _repASN.Context.Updateable<WMSASN>()
            //.SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.新增, p => p.CompleteTime == DateTime.Now)
            //.Where(p => receipt.ASNId == p.Id)
            //.ExecuteCommandAsync();


            //await _repASNDetail.UpdateAsync(a => new WMSASNDetail
            //{
            //    ReceivedQty = a.ReceivedQty - (SqlFunc.Subqueryable<WMSReceiptDetail>().Where(b => a.Id == b.ASNDetailId).Sum(c => c.ReceivedQty)),
            //    //_repReceiptDetail.AsQueryable().Where(b => a.Id == b.ASNDetailId).Sum(c => c.ReceivedQty),
            //    Updator = _userManager.Account,
            //    UpdateTime = DateTime.Now
            //}, a => ASNIds.Contains(a.ASNId));

            //判断ASN明细是否全部回退
            //ASNIds.ForEach(id =>
            //{

            //    var repASNDetailData = _repASNDetail.AsQueryable().Where(a => ASNIds.Contains(a.ASNId)).ToList();
            //    repASNDetailData.ForEach(a =>
            //    {
            //        a.ReceivedQty = a.ReceivedQty - (_repReceiptDetail.AsQueryable().Where(b => a.Id == b.ASNDetailId).Sum(c => c.ReceivedQty));
            //        a.Updator = _userManager.Account;
            //        a.UpdateTime = DateTime.Now;
            //    });
            //    _repASNDetail.UpdateRangeAsync(repASNDetailData);


            //    var asnDetail = _repASNDetail.AsQueryable().Where(b => b.ReceivedQty > 0 && b.ASNId == id).ToList();
            //    if (asnDetail.Count > 0)
            //    {
            //        //await _repASN.UpdateAsync(a => new WMSASN { ASNStatus = (int)ASNStatusEnum.部分转入库单 }, (a => ASNIds.Contains(a.Id)));
            //        _repASN.Context.Updateable<WMSASN>()
            //          .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.部分转入库单)
            //          .Where(p => (p.Id) == id)
            //          .ExecuteCommandAsync();
            //    }
            //    else
            //    {
            //        //await _repASN.UpdateAsync(a => new WMSASN { ASNStatus = (int)ASNStatusEnum.新增 }, (a => ASNIds.Contains(a.Id)));
            //        _repASN.Context.Updateable<WMSASN>()
            //        .SetColumns(p => p.ASNStatus == (int)ASNStatusEnum.新增)
            //        .Where(p => (p.Id) == id)
            //        .ExecuteCommandAsync();
            //    }
            //});

            //先删除明细表，再删除主表
            _repReceiptDetail.Delete(a => request.Select(a => a.Id).Contains(a.ReceiptId));
            _repReceipt.Delete(a => request.Select(a => a.Id).Contains(a.Id));

            //_wms_asndetailRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(c =>
            //{
            //    c.ReceiptQty = 0;
            //    _wms_asndetailRepository.Update(c);
            //}); 
            //先处理上架=>入库单

            await receipt.ForEachAsync(a =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = a.ExternReceiptNumber,
                    SystemOrder = a.ASNNumber,
                    Type = a.ReceiptType,
                    StatusCode = StatusCode.Success,
                    Msg = "操作成功"
                });
            });
            response.Code = StatusCode.Success;
            response.Msg = "操作成功";
            //throw new NotImplementedException();
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