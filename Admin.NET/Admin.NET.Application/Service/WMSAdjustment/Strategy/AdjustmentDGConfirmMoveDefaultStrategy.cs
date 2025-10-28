// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Interface;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Service;

namespace Admin.NET.Application.Strategy;
public class AdjustmentDGConfirmMoveDefaultStrategy : IAdjustmentConfirmInterface
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

    public AdjustmentDGConfirmMoveDefaultStrategy()
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
            DataTable AdjustmentInfoData = await _repAdjustment.Context.Ado.UseStoredProcedure().GetDataTableAsync("Proc_WMS_AdjustmentConfirmMove_DG", sugarParameter);
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

        AdjustmentData.ForEach(a =>
        {
            response.Data.Add(new OrderStatusDto
            {
                ExternOrder = a.ExternNumber,
                SystemOrder = a.AdjustmentNumber,
                Type = a.AdjustmentType,
                StatusCode = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? StatusCode.Success : StatusCode.Error,
                Msg = a.AdjustmentStatus == (int)AdjustmentStatusEnum.完成 ? "订单移库成功" : "订单移库失败",

            });
            //}
        });


        response.Code = StatusCode.Success;
        response.Msg = "操作成功";
        return response;

    }

}
