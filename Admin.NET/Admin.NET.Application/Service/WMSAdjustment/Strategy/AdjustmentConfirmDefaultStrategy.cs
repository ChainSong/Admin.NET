
using Admin.NET.Application.CommonCore.EnumCommon;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AngleSharp.Dom;
using Furion.DistributedIDGenerator;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Strategy
{
    public class AdjustmentConfirmDefaultStrategy : IAdjustmentConfirmInterface
    {

        //注入数据库实例
        public ISqlSugarClient _db { get; set; }

        //注入权限仓储
        public UserManager _userManager { get; set; }
        //注入Adjustment仓储

        public SqlSugarRepository<WMSAdjustment> _repAdjustment { get; set; }
        //注入AdjustmentDetail仓储
        public SqlSugarRepository<WMSAdjustmentDetail> _repAdjustmentDetail { get; set; }
        //注入仓库关系仓储

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //注入客户关系仓储
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public AdjustmentConfirmDefaultStrategy()
        {
        }

        //public Task<Response<List<OrderStatusDto>>> Strategy(List<AddWMSAdjustmentInput> request)

        /// <summary>
        /// 默认方法不做任何处理
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<Response<List<OrderStatusDto>>> Strategy(List<long> request)
        {

            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };

            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单

            //var AdjustmentCheck = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));


            var AdjustmentData = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));


            AdjustmentData.ForEach(a =>
            {
                //if (a.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
                //{
                response.Data.Add(new OrderStatusDto
                {
                    ExternOrder = a.ExternNumber,
                    SystemOrder = a.AdjustmentNumber,
                    Type = a.AdjustmentType,
                    StatusCode = StatusCode.Error,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单类型不存在",

                });
                //}
            });


            response.Code = StatusCode.Error;
            response.Msg = "操作失败";
            return response;

        }

    }
}
