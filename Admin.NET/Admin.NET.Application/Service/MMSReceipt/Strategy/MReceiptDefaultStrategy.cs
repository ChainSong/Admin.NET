
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service.Enumerate; 
using Admin.NET.Common.SnowflakeCommon;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Admin.NET.Application.Strategy
{

    public class MReceiptDefaultStrategy : IMReceiptInterface
    {



        public SqlSugarRepository<MMSReceipt> _repMReceipt { get; set; }
        public SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail { get; set; }
        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public SqlSugarRepository<MMSSupplier> _repSupplier { get; set; }
        public SqlSugarRepository<SupplierUserMapping> _repSupplierUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail { get; set; }
        public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable { get; set; }

        public MReceiptDefaultStrategy()
        {
        }

        //默认方法不做任何处理
        public async Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateMMSReceiptInput> request)
        {
            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();

            //判断是否有权限操作
            //先判断是否能操作客户
            var customerCheck = _repSupplierUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.SupplierName).ToList().Contains(a.SupplierName)).ToList();
            if (customerCheck.GroupBy(a => a.SupplierName).Count() != request.GroupBy(a => a.SupplierName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少客户操作权限";
                return response;
            }

            //先判断是否能操作仓库
            var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
            if (warehouseCheck.GroupBy(a => a.WarehouseName).Count() != request.GroupBy(a => a.WarehouseName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少仓库操作权限";
                return response;
            }

            //1判断PreOrder 是否已经存在已有的订单

            var receiptCheck = _repMReceipt.AsQueryable().Where(a => request.Select(r => r.ExternReceiptNumber + r.SupplierName.ToString()).ToList().Contains(a.ExternReceiptNumber + a.SupplierName.ToString()));
            if (receiptCheck != null && receiptCheck.ToList().Count > 0)
            {
                receiptCheck.ToList().ForEach(b =>
                {
                    response.Data.Add(new OrderStatusDto()
                    {
                        ExternOrder = b.ExternReceiptNumber,
                        SystemOrder = b.ReceiptNumber,
                        Type = b.ReceiptType,
                        StatusCode = StatusCode.Warning,
                        //StatusMsg = StatusCode.warning.ToString(),
                        Msg = "订单已存在"
                    });

                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }

            var mReceiptData = request.Adapt<List<MMSReceipt>>();

            //var receiptData = mapper.Map<List<WMS_receipt>>(request);
            int LineNumber = 1;
            mReceiptData.ForEach(item =>
            {
                var SupplierId = _repSupplierUser.AsQueryable().Where(b => b.SupplierName == item.SupplierName).First().SupplierId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;

                var ReceiptNumber = SnowFlakeHelper.GetSnowInstance().NextId().ToString();
                //ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                item.ReceiptNumber = ReceiptNumber;
                item.SupplierId = SupplierId;
                item.WarehouseId = WarehouseId;
                item.Creator = _userManager.Account;
                item.CreationTime = DateTime.Now;
                item.ReceiptStatus = (int)MReceiptStatusEnum.新增;
                item.Details.ForEach(a =>
                {
                    a.ReceiptNumber = ReceiptNumber;
                    a.SupplierId = SupplierId;
                    a.SupplierName = item.SupplierName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternReceiptNumber = item.ExternReceiptNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Creator = _userManager.Account;
                    a.CreationTime = DateTime.Now;
                });
                LineNumber++;
            });


            ////开始插入订单
            await _repMReceipt.Context.InsertNav(mReceiptData).Include(a => a.Details).ExecuteCommandAsync();

            mReceiptData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ReceiptNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单新增成功"
                });

            });
            response.Code = StatusCode.Success;
            response.Msg = "订单新增成功";
            return response;
        }
        public async Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateMMSReceiptInput> request)
        {
            Response<List<OrderStatusDto>> response = new Response<List<OrderStatusDto>>() { Data = new List<OrderStatusDto>() };
            //开始校验数据
            List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();

            //判断是否有权限操作
            //先判断是否能操作客户
            var customerCheck = _repSupplierUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.SupplierName).ToList().Contains(a.SupplierName)).ToList();
            if (customerCheck.GroupBy(a => a.SupplierName).Count() != request.GroupBy(a => a.SupplierName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少客户操作权限";
                return response;
            }

            //先判断是否能操作仓库
            var warehouseCheck = _repWarehouseUser.AsQueryable().Where(a => a.UserId == _userManager.UserId && request.Select(r => r.WarehouseName).ToList().Contains(a.WarehouseName)).ToList();
            if (warehouseCheck.GroupBy(a => a.WarehouseName).Count() != request.GroupBy(a => a.WarehouseName).Count())
            {
                response.Code = StatusCode.Error;
                response.Msg = "用户缺少仓库操作权限";
                return response;
            }

            //1判断PreOrder 是否已经存在已有的订单
            //开始校验数据
            //List<OrderStatusDto> orderStatus = new List<OrderStatusDto>();
            //1判断PreOrder 是否已经存在已有的订单 判断订单状态能不能修改

            var receiptCheck = _repMReceipt.AsQueryable().Where(a => request.Select(r => r.ExternReceiptNumber).ToList().Contains(a.ExternReceiptNumber));
            if (receiptCheck != null && receiptCheck.ToList().Count > 0)
            {
                receiptCheck.ToList().ForEach(b =>
                {
                    //判断订单状态
                    if (b.ReceiptStatus > (int)MReceiptStatusEnum.新增)
                        response.Data.Add(new OrderStatusDto()
                        {
                            ExternOrder = b.ExternReceiptNumber,
                            SystemOrder = b.ReceiptNumber,
                            Type = b.ReceiptType,
                            StatusCode = StatusCode.Warning,
                            //StatusMsg = StatusCode.warning.ToString(),
                            Msg = "订单状态异常"
                        });

                });
                if (response.Data.Count > 0)
                {
                    response.Code = StatusCode.Error;
                    response.Msg = "订单异常";
                    return response;
                }
            }

            var receiptData = request.Adapt<List<MMSReceipt>>();



            //var receiptData = mapper.Map<List<WMS_receipt>>(request);
            int LineNumber = 1;
            receiptData.ForEach(item =>
            {
                var SupplierId = _repMReceipt.AsQueryable().Where(b => b.SupplierName == item.SupplierName).First().SupplierId;
                var WarehouseId = _repWarehouseUser.AsQueryable().Where(b => b.WarehouseName == item.WarehouseName).First().WarehouseId;
                //var receiptNumber = ShortIDGen.NextID(new GenerationOptions
                //{
                //    Length = 10// 设置长度，注意：不设置次长度是随机长度！！！！！！！
                //});// 生成一个包含数字，字母，不包含特殊符号的 8 位短id
                //item.receiptNumber = receiptNumber;
                item.SupplierId = SupplierId;
                item.WarehouseId = WarehouseId;
                item.Updator = _userManager.Account;
                item.UpdateTime = DateTime.Now;
                item.ReceiptStatus = (int)MReceiptStatusEnum.新增;
                item.Details.ForEach(a =>
                {
                    a.ReceiptNumber = item.ReceiptNumber;
                    a.SupplierId = SupplierId;
                    a.SupplierName = item.SupplierName;
                    a.WarehouseId = WarehouseId;
                    a.WarehouseName = item.WarehouseName;
                    a.ExternReceiptNumber = item.ExternReceiptNumber;
                    a.LineNumber = LineNumber.ToString().PadLeft(5, '0');
                    a.Updator = _userManager.Account;
                    a.UpdateTime = DateTime.Now;
                });
                LineNumber++;
            });


            ////开始插入订单
            await _repMReceipt.Context.UpdateNav(receiptData).Include(a => a.Details).ExecuteCommandAsync();

            receiptData.ToList().ForEach(b =>
            {
                response.Data.Add(new OrderStatusDto()
                {
                    ExternOrder = b.ExternReceiptNumber,
                    SystemOrder = b.ReceiptNumber,
                    Type = b.ReceiptType,
                    StatusCode = StatusCode.Success,
                    //StatusMsg = StatusCode.warning.ToString(),
                    Msg = "订单修改成功"
                });

            });
            response.Code = StatusCode.Success;
            response.Msg = "订单修改成功";
            return response;
        }
    }



}

 