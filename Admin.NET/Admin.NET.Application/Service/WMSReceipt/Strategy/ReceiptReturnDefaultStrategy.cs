
//using Admin.NET.Application.Interface;
//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Admin.NET.Application.Strategy
//{
//    public class ReceiptReturnDefaultStrategy : IReceiptReturnInterface
//    {
 


//        public ReceiptReturnDefaultStrategy(
//        IRepository<WMS_ASN, long> wms_asnRepository,
//        IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
//        IRepository<WMS_Receipt, long> wms_receiptRepository,
//        IRepository<WMS_ReceiptDetail, long> wms_receiptdetailRepository
//        //IRepository<WMS_ReceiptReceiving, long> wms_receiptreceivingRepository
//    )
//        {
//            _wms_asnRepository = wms_asnRepository;
//            _wms_asndetailRepository = wms_asndetailRepository;
//            _wms_receiptRepository = wms_receiptRepository;
//            _wms_receiptdetailRepository = wms_receiptdetailRepository;
//            //_wms_receiptreceivingRepository = wms_receiptreceivingRepository;


//        }
//        //默认方法
//        public async Task<Response<List<OrderStatusDto>>> Strategy(IAbpSessionExtension abpSession, params long[] request)
//        {

//            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
//            //CreateOrUpdateWMS_ReceiptInput receipts = new CreateOrUpdateWMS_ReceiptInput();
//            //先判断状态是否正常 是否允许回退
//            var receipt = _wms_receiptRepository.GetAll().Where(a => request.Contains(a.Id));
//            await receipt.ForEachAsync(a =>
//            {
//                if (a.ReceiptStatus != (int)ReceiptStatusEnum.新增)
//                {
//                    response.Data.Add(new OrderStatusDto()
//                    {
//                        ExternOrder = a.ExternReceiptNumber,
//                        SystemOrder = a.ASNNumber,
//                        Type = a.ReceiptType,
//                        StatusCode = StatusCode.warning,
//                        Msg = "状态异常"
//                    });
//                }
//            });
//            if (response.Data != null && response.Data.Count > 0)
//            {
//                return response;
//            }

//            _wms_receiptRepository.Delete(a => request.Contains(a.Id));
//            _wms_receiptdetailRepository.Delete(a => request.Contains(a.ReceiptId));
//            _wms_asnRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.Id)).BatchUpdate(new WMS_ASN { ASNStatus = (int)ASNStatusEnum.新增 });
//            _wms_asndetailRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.ASNId)).BatchUpdate(new WMS_ASNDetail
//            {
//                ReceivedQty = 0,
//                Updator = abpSession.UserName,
//                UpdateTime = DateTime.Now
//            });

//            //_wms_asndetailRepository.GetAll().Where(a => receipt.Select(b => b.ASNId).Contains(a.ASNId)).ToList().ForEach(c =>
//            //{
//            //    c.ReceiptQty = 0;
//            //    _wms_asndetailRepository.Update(c);
//            //}); 
//            //先处理上架=>入库单

//            await receipt.ForEachAsync(a =>
//            {
//                response.Data.Add(new OrderStatusDto()
//                {
//                    ExternOrder = a.ExternReceiptNumber,
//                    SystemOrder = a.ASNNumber,
//                    Type = a.ReceiptType,
//                    StatusCode = StatusCode.success,
//                    Msg = "操作成功"
//                });
//            });
//            response.Code = StatusCode.success;
//            //throw new NotImplementedException();
//            return response;

//        }
//    }

//    //public class PropertyValue<T>
//    //{
//    //    private static ConcurrentDictionary<string, MemberGetDelegate> _memberGetDelegate = new ConcurrentDictionary<string, MemberGetDelegate>();
//    //    delegate object MemberGetDelegate(T obj);
//    //    public PropertyValue(T obj)
//    //    {
//    //        Target = obj;
//    //    }
//    //    public T Target { get; private set; }
//    //    public object Get(string name)
//    //    {
//    //        MemberGetDelegate memberGet = _memberGetDelegate.GetOrAdd(name, BuildDelegate);
//    //        return memberGet(Target);
//    //    }
//    //    private MemberGetDelegate BuildDelegate(string name)
//    //    {
//    //        Type type = typeof(T);
//    //        PropertyInfo property = type.GetProperty(name);
//    //        return (MemberGetDelegate)Delegate.CreateDelegate(typeof(MemberGetDelegate), property.GetGetMethod());
//    //    }
//    //}
//}