//using Admin.NET.Application.Dtos.Enum;
//using Admin.NET.Common.ExcelCommon;
//using Admin.NET.Application.Const;
//using Admin.NET.Application.Dtos;
//using Admin.NET.Application.Factory;
//using Admin.NET.Application.ReceiptCore.Factory;
//using Admin.NET.Application.ReceiptCore.Interface;
//using Admin.NET.Application.ReceiptReceivingCore.Factory;
//using Admin.NET.Application.ReceiptReceivingCore.Interface;
//using Admin.NET.Applicationt.ReceiptReceivingCore.Factory;
//using Admin.NET.Core;
//using Admin.NET.Core.Entity;
//using Furion.DatabaseAccessor;
//using Furion.DependencyInjection;
//using Furion.FriendlyException;
//using Microsoft.AspNetCore.Http;
////using MyProject.ReceiptReceivingCore.Dto;
//using NewLife.Net;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;

//namespace Admin.NET.Application;
///// <summary>
///// WMSReceipt服务
///// </summary>
//[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
//public class WMSReportService : IDynamicApiController, ITransient
//{

//    //private readonly ISqlSugarClient _db;
//    private readonly SqlSugarRepository<WMSCustomer> _repCustomer;
//    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
//    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
//    private readonly SqlSugarRepository<TableColumns> _repTableColumns;
//    private readonly UserManager _userManager;

//    private readonly SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed;
//    public WMSReportService(SqlSugarRepository<WMSCustomer> repCustomer, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, UserManager userManager, SqlSugarRepository<TableColumns> repTableColumns, SqlSugarRepository<WMSInventoryUsed> repTableInventoryUsed)
//    {

//        //_db = db;
//        _repCustomer = repCustomer;
//        _repCustomerUser = repCustomerUser;
//        _repWarehouseUser = repWarehouseUser;
//        _userManager = userManager;
//        _repTableColumns = repTableColumns;
//        _repTableInventoryUsed = repTableInventoryUsed;
//    }

//    /// <summary>
//    /// 可用库存汇总报表
//    /// </summary>
//    /// <param name="input"></param>
//    /// <returns></returns>
//    [HttpPost("/wms/report/inventory")]
//    public async Task<dynamic> InventoryReport(InventoryReportInput input)
//    {
//        var user = await _userManager.GetUserAsync(User);
//        var customerId = _repCustomerUser.GetFirst(u => u.UserId == user.Id).CustomerId;
//        var warehouseId = _repWarehouseUser.GetFirst(u => u.UserId == user.Id).WarehouseId;
//        var inventoryUsed = _repTableInventoryUsed.GetList(u => u.CustomerId == customerId && u.WarehouseId == warehouseId);
//        var inventoryUsedList = inventoryUsed.Select(u => new InventoryReportOutput
//        {
//            Id = u.Id,
//            ProductName = u.ProductName,
//            ProductCode = u.ProductCode,
//            Barcode = u.Barcode,
//            Batch = u.Batch,
//            InventoryNum = u.InventoryNum,
//            InventoryCost = u.InventoryCost,
//            InventoryPrice = u.InventoryPrice,
//            InventoryAmount = u.InventoryAmount,
//            CreateTime = u.CreateTime,
//            UpdateTime = u.UpdateTime
//        }).ToList();
//        return inventoryUsedList;

//    }
//}

