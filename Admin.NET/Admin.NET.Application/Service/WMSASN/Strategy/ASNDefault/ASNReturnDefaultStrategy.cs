

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Admin.NET.Application.Strategy
//{
//    public class ASNReturnDefaultStrategy : IASNReturnInterface
//    {


//        /// <summary>
//        ///【WMS_ASN】仓储层
//        /// </summary>
//        private readonly IRepository<WMS_ASN, long> _wms_asnRepository;
//        /// <summary>
//        ///【WMS_ASNDetail】仓储层
//        /// </summary>
//        private readonly IRepository<WMS_ASNDetail, long> _wms_asndetailRepository;



//        /// <summary>
//        ///【WarehouseUserMapping】领域服务
//        /// </summary>
//        private readonly IWarehouseUserMappingManager _warehouseusermappingManager;


//        /// <summary>
//        ///【CustomerUserMapping】领域服务
//        /// </summary>
//        private readonly ICustomerUserMappingManager _customerusermappingManager;
//        public ASNReturnDefaultStrategy(
//            IRepository<WMS_ASN, long> wms_asnRepository,
//            IRepository<WMS_ASNDetail, long> wms_asndetailRepository,
//            IWarehouseUserMappingManager warehouseusermappingManager,
//            ICustomerUserMappingManager customerusermappingManager
//        )
//        {
//            _wms_asnRepository = wms_asnRepository;
//            _wms_asndetailRepository = wms_asndetailRepository;
//            _warehouseusermappingManager = warehouseusermappingManager;
//            _customerusermappingManager = customerusermappingManager;
//        }



//        /// <summary>
//        /// 默认方法不做任何处理
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        public Response<List<OrderStatusDto>> Strategy(List<long> request, IAbpSessionExtension abpSession)
//        {
//            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

//            //开始校验数据
//            //List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
//            //1判断PreOrder 是否已经存在已有的订单

//            var asnCheck = _wms_asnRepository.GetAll().Where(a => request.Contains(a.Id));
//            if (asnCheck != null && asnCheck.ToList().Count > 0)
//            {
//                asnCheck.ToList().ForEach(b =>
//                {
//                    if (b.ASNStatus != (int)ASNStatusEnum.新增)
//                        response.Data.Add(new OrderStatusDto()
//                        {
//                            ExternOrder = b.ExternReceiptNumber,
//                            SystemOrder = b.ASNNumber,
//                            Type = b.ReceiptType,
//                            StatusCode = StatusCode.warning,
//                            //StatusMsg = StatusCode.warning.ToString(),
//                            Msg = "订单状态异常"
//                        });

//                });
//                response.Code = StatusCode.error;
//                response.Msg = "订单异常";
//                return response;
//            }
//            _wms_asnRepository.GetAll().Where(a => request.Contains(a.Id)).BatchUpdate(new WMS_ASN { ASNStatus = (int)ASNStatusEnum.新增 });
//            response.Code = StatusCode.success;
//            return response;

//        }
//    }
//}
