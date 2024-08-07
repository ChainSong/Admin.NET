﻿
using Admin.NET.Application.Dtos.Enum;
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
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Service;
using FluentEmail.Core;

namespace Admin.NET.Application.Strategy
{
    public class AdjustmentConfirmMoveDefaultStrategy : IAdjustmentConfirmInterface
    {

        //注入数据库实例
        //public ISqlSugarClient _db { get; set; }

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

        public AdjustmentConfirmMoveDefaultStrategy()
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

            var AdjustmentCheck = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));

            if (AdjustmentCheck == null && AdjustmentCheck.ToList().Count > 0)
            {

                AdjustmentCheck.ForEach(a =>
                {
                    if (a.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
                    {
                        response.Data.Add(new OrderStatusDto
                        {
                            ExternOrder = a.ExternNumber,
                            SystemOrder = a.AdjustmentNumber,
                            Type = a.AdjustmentType,
                            StatusCode = StatusCode.Warning,
                            //StatusMsg = StatusCode.warning.ToString(),
                            Msg = "订单状态异常"
                        });
                    }
                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }
            List<WMSAdjustmentInformationDto> AdjustmentInfo = new List<WMSAdjustmentInformationDto>();
            foreach (var a in request)
            {
                var sugarParameter = new SugarParameter("@AdjustmentId", a, typeof(long), ParameterDirection.Input);
                DataTable AdjustmentInfoData = await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmMove", sugarParameter);
                if (AdjustmentInfoData != null && AdjustmentInfoData.Rows.Count > 0)
                {
                    AdjustmentInfo.AddRange(AdjustmentInfoData.TableToList<WMSAdjustmentInformationDto>());
                }
            }
            if (AdjustmentInfo.Count > 0)
            {
                AdjustmentInfo.ForEach(a =>
                {
                    response.Data.Add(new OrderStatusDto
                    {
                        ExternOrder = a.OrderNumber,
                        SystemOrder = a.OrderNumber,
                        Type = a.InformationType,
                        StatusCode = StatusCode.Error,
                        //StatusMsg = StatusCode.warning.ToString(),
                        Msg = "订单移库失败:sku" + a.SKU + ",订单数量：" + a.Qty + ",库存数量：" + a.InventoryQty
                    });
                });

                response.Code = StatusCode.Error;
                response.Msg = "操作失败";
                return response;
            }
            //await request.ForEachAsync(async a => 
            //{
            //    var sugarParameter = new SugarParameter("@AdjustmentId", a, typeof(long), ParameterDirection.Input);
            //    await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmMove", sugarParameter);
            //});

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
                    StatusCode = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? StatusCode.Success : StatusCode.Error,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? "订单移库成功" : "订单移库失败",

                });
                //}
            });


            response.Code = StatusCode.Success;
            response.Msg = "操作成功";
            return response;

        }

    }
}
