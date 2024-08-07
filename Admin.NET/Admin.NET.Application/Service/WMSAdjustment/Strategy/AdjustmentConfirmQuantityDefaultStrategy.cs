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

namespace Admin.NET.Application.Strategy
{
    public class AdjustmentConfirmQuantityDefaultStrategy : IAdjustmentConfirmInterface
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

        public AdjustmentConfirmQuantityDefaultStrategy()
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
            //await request.ForEachAsync(async a =>
            //{
            //    var sugarParameter = new SugarParameter("@AdjustmentId", a, typeof(long), ParameterDirection.Input);
            //    await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmQuantity", sugarParameter);
            //});


            List<WMSAdjustmentInformationDto> AdjustmentInfo = new List<WMSAdjustmentInformationDto>();


            foreach (var a in request)
            {
                var sugarParameter = new SugarParameter("@AdjustmentId", a, typeof(long), ParameterDirection.Input);
                DataTable AdjustmentInfoData = await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmQuantity", sugarParameter);
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

            var AdjustmentData = _repAdjustment.AsQueryable().Where(a => request.Contains(a.Id));
            //var ysxx = await _db.Ado.UseStoredProcedure().GetDataTableAsync("exec Proc_WMS_AutomatedOutbound ", sugarParameter);



            //if (AdjustmentCheck.AdjustmentStatus != (int)AdjustmentStatusEnum.新增)
            //{
            //    response.Data.Add(new OrderStatusDto
            //    {
            //        ExternOrder = AdjustmentCheck.ExternNumber,
            //        SystemOrder = AdjustmentCheck.AdjustmentNumber,
            //        Type = AdjustmentCheck.AdjustmentType,
            //        StatusCode = StatusCode.Warning,
            //        //StatusMsg = StatusCode.warning.ToString(),
            //        Msg = "订单状态异常"
            //    });
            //    response.Code = StatusCode.Error;
            //    response.Msg = "订单状态异常";
            //    return response;
            //}

            //var AdjustmentData = request.Adapt<List<WMSAdjustment>>();



            ////var AdjustmentData = mapper.Map<List<WMS_Adjustment>>(request);
            //int LineNumber = 1;
            //AdjustmentData.ForEach(item =>
            //    {
            //        var CustomerId = _repCustomerUser.AsQueryable().Where(b => b.CustomerName == item.CustomerName).First().CustomerId;
            //        var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

            //        var AdjustmentNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
            //        //ShortIDGen.NextID(new GenerationOptions
            //        //{
            //        //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
            //        //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
            //        item.AdjustmentNumber = AdjustmentNumber;
            //        item.ExternNumber = item.ExternNumber;
            //        item.CustomerId = CustomerId;
            //        item.WarehouseId = WarehouseId;
            //        item.Creator = _userManager.Account;
            //        item.CreationTime = DateTime.Now;
            //        item.AdjustmentStatus = (int)AdjustmentStatusEnum.新增;
            //        item.Details.ForEach(a =>
            //            {
            //            a.AdjustmentNumber = AdjustmentNumber;
            //            item.ExternNumber = item.ExternNumber;
            //            a.CustomerId = CustomerId;
            //            a.CustomerName = item.CustomerName;
            //            a.WarehouseId = WarehouseId;
            //            a.WarehouseName = item.WarehouseName;
            //            a.AdjustmentNumber = item.AdjustmentNumber;
            //            a.Creator = _userManager.Account;
            //            a.CreationTime = DateTime.Now;
            //        });
            //        LineNumber++;
            //    });

            //////开始插入订单
            //await _db.InsertNav(AdjustmentData).Include(a => a.Details).ExecuteCommandAsync();


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
                        Msg = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? "订单调整成功" : "订单调整失败",

                    });
                //}
            });


            response.Code = StatusCode.Success;
            response.Msg = "操作成功";
            return response;

        }

    }
}
