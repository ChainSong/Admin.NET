using Admin.NET.Application.Const;
using Admin.NET.Application.Dtos;
using Admin.NET.Application.Dtos.Enum;
using Admin.NET.Application.Enumerate;
using Admin.NET.Application.Factory;
using Admin.NET.Application.Interface;
using Admin.NET.Application.Service;
using Admin.NET.Application.Strategy;
using Admin.NET.Common;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using AngleSharp.Dom;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.FriendlyException;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.NET.Application;
/// <summary>
/// WMSReceipt服务
/// </summary>
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFOrderPickService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<WMSPickTask> _rep;


    private readonly SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser;
    private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;

    private readonly SqlSugarRepository<WMSPackage> _repPackage;
    private readonly SqlSugarRepository<WMSPackageDetail> _repPackageDetail;
    private readonly SqlSugarRepository<WMSProduct> _repProduct;

    //private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;
    //private readonly SqlSugarRepository<CustomerUserMapping> _repCustomerUser;


    private readonly UserManager _userManager;
    //private readonly ISqlSugarClient _db;
    private readonly SysCacheService _sysCacheService;
    private readonly SqlSugarRepository<WMSOrder> _repOrder;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;
    private readonly SqlSugarRepository<WMSLocation> _repLocation;
    private readonly SysWorkFlowService _repWorkFlowService;

    public WMSRFOrderPickService(SqlSugarRepository<WMSPickTask> rep, UserManager userManager, ISqlSugarClient db, SqlSugarRepository<WarehouseUserMapping> repWarehouseUser, SqlSugarRepository<CustomerUserMapping> repCustomerUser, SqlSugarRepository<WMSOrder> repOrder, SqlSugarRepository<WMSPickTaskDetail> repPickTaskDetail, SqlSugarRepository<WMSPackage> repPackage, SqlSugarRepository<WMSPackageDetail> repPackageDetail, SysCacheService sysCacheService, SqlSugarRepository<WMSLocation> repLocation, SqlSugarRepository<WMSProduct> repProduct, SysWorkFlowService repWorkFlowService)
    {
        _rep = rep;
        _userManager = userManager;
        //_db = db;
        _repWarehouseUser = repWarehouseUser;
        _repCustomerUser = repCustomerUser;
        _repOrder = repOrder;
        _repPickTaskDetail = repPickTaskDetail;
        _repPackage = repPackage;
        _repPackageDetail = repPackageDetail;
        _sysCacheService = sysCacheService;
        _repLocation = repLocation;
        _repProduct = repProduct;
        _repWorkFlowService = repWorkFlowService;
    }

    /// <summary>
    /// 分页查询WMSPickTask
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
    {
        var query = _rep.AsQueryable()
                    //.WhereIF(input.CustomerId > 0, u => u.CustomerId == input.CustomerId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.CustomerName), u => u.CustomerName.Contains(input.CustomerName.Trim()))
                    //.WhereIF(input.WarehouseId > 0, u => u.WarehouseId == input.WarehouseId)
                    .WhereIF(!string.IsNullOrWhiteSpace(input.WarehouseName), u => u.WarehouseName.Contains(input.WarehouseName.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.ExternOrderNumber), u => u.ExternOrderNumber.Contains(input.ExternOrderNumber.Trim()))
                    .WhereIF(!string.IsNullOrWhiteSpace(input.OrderNumber), u => u.OrderNumber.Contains(input.OrderNumber.Trim()))
                    //.WhereIF(input.PickStatus != 0, u => u.PickStatus == input.PickStatus)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.PickType), u => u.PickType.Contains(input.PickType.Trim()))
                    //.WhereIF(input.PrintNum > 0, u => u.PrintNum == input.PrintNum)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.PrintPersonnel), u => u.PrintPersonnel.Contains(input.PrintPersonnel.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.PickPlanPersonnel), u => u.PickPlanPersonnel.Contains(input.PickPlanPersonnel.Trim()))
                    //.WhereIF(input.DetailQty > 0, u => u.DetailQty == input.DetailQty)
                    //.WhereIF(input.DetailKindsQty > 0, u => u.DetailKindsQty == input.DetailKindsQty)
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Creator), u => u.Creator.Contains(input.Creator.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Updator), u => u.Updator.Contains(input.Updator.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str1), u => u.Str1.Contains(input.Str1.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str2), u => u.Str2.Contains(input.Str2.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str3), u => u.Str3.Contains(input.Str3.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str4), u => u.Str4.Contains(input.Str4.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str5), u => u.Str5.Contains(input.Str5.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str6), u => u.Str6.Contains(input.Str6.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str7), u => u.Str7.Contains(input.Str7.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str8), u => u.Str8.Contains(input.Str8.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str9), u => u.Str9.Contains(input.Str9.Trim()))
                    //.WhereIF(!string.IsNullOrWhiteSpace(input.Str10), u => u.Str10.Contains(input.Str10.Trim()))
                    //.WhereIF(input.Int1 > 0, u => u.Int1 == input.Int1)
                    //.WhereIF(input.Int2 > 0, u => u.Int2 == input.Int2)
                    //.WhereIF(input.Int3 > 0, u => u.Int3 == input.Int3)
                    //.WhereIF(input.Int4 > 0, u => u.Int4 == input.Int4)
                    //.WhereIF(input.Int5 > 0, u => u.Int5 == input.Int5)
                    .Where(a => a.PickStatus < (int)PickTaskStatusEnum.拣货完成)
                    //.Where(a => _repCustomerUser.AsQueryable().Where(b => b.CustomerId == a.CustomerId).Count() > 0)
                    //.Where(a => _repWarehouseUser.AsQueryable().Where(b => b.WarehouseId == a.WarehouseId).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<CustomerUserMapping>().Where(b => b.CustomerId == a.CustomerId && b.UserId == _userManager.UserId).Count() > 0)
                    //.Where(a => SqlFunc.Subqueryable<WarehouseUserMapping>().Where(b => b.WarehouseId == a.WarehouseId && b.UserId == _userManager.UserId).Count() > 0)

                    .Select<WMSPickTaskOutput>()
;
        if (input.StartTime != null && input.StartTime.Count > 0)
        {
            DateTime? start = input.StartTime[0];
            query = query.WhereIF(start.HasValue, u => u.StartTime > start);
            if (input.StartTime.Count > 1 && input.StartTime[1].HasValue)
            {
                var end = input.StartTime[1].Value.AddDays(1);
                query = query.Where(u => u.StartTime < end);
            }
        }
        if (input.EndTime != null && input.EndTime.Count > 0)
        {
            DateTime? start = input.EndTime[0];
            query = query.WhereIF(start.HasValue, u => u.EndTime > start);
            if (input.EndTime.Count > 1 && input.EndTime[1].HasValue)
            {
                var end = input.EndTime[1].Value.AddDays(1);
                query = query.Where(u => u.EndTime < end);
            }
        }
        if (input.PrintTime != null && input.PrintTime.Count > 0)
        {
            DateTime? start = input.PrintTime[0];
            query = query.WhereIF(start.HasValue, u => u.PrintTime > start);
            if (input.PrintTime.Count > 1 && input.PrintTime[1].HasValue)
            {
                var end = input.PrintTime[1].Value.AddDays(1);
                query = query.Where(u => u.PrintTime < end);
            }
        }
        if (input.CreationTime != null && input.CreationTime.Count > 0)
        {
            DateTime? start = input.CreationTime[0];
            query = query.WhereIF(start.HasValue, u => u.CreationTime > start);
            if (input.CreationTime.Count > 1 && input.CreationTime[1].HasValue)
            {
                var end = input.CreationTime[1].Value.AddDays(1);
                query = query.Where(u => u.CreationTime < end);
            }
        }
        if (input.DateTime1 != null && input.DateTime1.Count > 0)
        {
            DateTime? start = input.DateTime1[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime1 > start);
            if (input.DateTime1.Count > 1 && input.DateTime1[1].HasValue)
            {
                var end = input.DateTime1[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime1 < end);
            }
        }
        if (input.DateTime2 != null && input.DateTime2.Count > 0)
        {
            DateTime? start = input.DateTime2[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime2 > start);
            if (input.DateTime2.Count > 1 && input.DateTime2[1].HasValue)
            {
                var end = input.DateTime2[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime2 < end);
            }
        }
        if (input.DateTime3 != null && input.DateTime3.Count > 0)
        {
            DateTime? start = input.DateTime3[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime3 > start);
            if (input.DateTime3.Count > 1 && input.DateTime3[1].HasValue)
            {
                var end = input.DateTime3[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime3 < end);
            }
        }
        if (input.DateTime4 != null && input.DateTime4.Count > 0)
        {
            DateTime? start = input.DateTime4[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime4 > start);
            if (input.DateTime4.Count > 1 && input.DateTime4[1].HasValue)
            {
                var end = input.DateTime4[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime4 < end);
            }
        }
        if (input.DateTime5 != null && input.DateTime5.Count > 0)
        {
            DateTime? start = input.DateTime5[0];
            query = query.WhereIF(start.HasValue, u => u.DateTime5 > start);
            if (input.DateTime5.Count > 1 && input.DateTime5[1].HasValue)
            {
                var end = input.DateTime5[1].Value.AddDays(1);
                query = query.Where(u => u.DateTime5 < end);
            }
        }
        query = query.OrderBuilder(input);
        return await query.ToPagedListAsync(input.Page, input.PageSize);
    }
    /// <summary>
    /// RF 统一扫描拣货包装方法（通过抽象工厂判断调用拣货还是包装）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ScanRFOrderPick")]
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanRFOrderPick(WMSRFPickTaskDetailInput input)
    {
        // 验证拣货任务号
        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            return new Response<List<WMSRFPickTaskDetailOutput>>
            {
                Code = StatusCode.Error,
                Msg = "拣货任务号不能为空",
                Data = null
            };
        }

        // 获取拣货任务
        var pickTask = await _rep.AsQueryable()
            .Where(a => a.PickTaskNumber == input.PickTaskNumber)
            .FirstAsync();

        if (pickTask == null)
        {
            return new Response<List<WMSRFPickTaskDetailOutput>>
            {
                Code = StatusCode.Error,
                Msg = "拣货任务不存在",
                Data = null
            };
        }

        // 设置输入参数
        input.Id = pickTask.Id;
        input.CustomerId = pickTask.CustomerId;
        input.CustomerName = pickTask.CustomerName;
        input.WarehouseId = pickTask.WarehouseId;
        input.WarehouseName = pickTask.WarehouseName;
        input.OrderNumber = pickTask.OrderNumber;
        input.ExternOrderNumber = pickTask.ExternOrderNumber;

        // 根据操作类型选择策略（默认为拣货）
        //RFOperationType operationType;
        //if (string.IsNullOrEmpty(input.OperationType))
        //{
        //    // 如果没有指定操作类型，根据输入内容判断
        //    operationType = string.IsNullOrEmpty(input.BoxNumber) ? RFOperationType.Pick : RFOperationType.Package;
        //}
        //else if (Enum.TryParse<RFOperationType>(input.OperationType, true, out operationType))
        //{
        //    // 使用指定的操作类型
        //}
        //else
        //{
        //    return new Response<List<WMSRFPickTaskDetailOutput>>
        //    {
        //        Code = StatusCode.Error,
        //        Msg = "不支持的操作类型",
        //        Data = null
        //    };
        //}

        var workflow = await _repWorkFlowService.GetSystemWorkFlow(pickTask.CustomerName, OutboundWorkFlowConst.Workflow_Outbound, OutboundWorkFlowConst.Workflow_Outbound_RF_PICK, pickTask.PickType);
        // 通过抽象工厂获取对应的策略实例
        IOrderPickRFInterface strategy = OrderPickRFFactory.OrderPickTask("Hach");
        // 注入依赖
        strategy._userManager = _userManager;
        strategy._repPickTask = _rep;
        strategy._repWarehouseUser = _repWarehouseUser;
        strategy._repCustomerUser = _repCustomerUser;
        strategy._sysCacheService = _sysCacheService;
        strategy._repOrder = _repOrder;
        strategy._repPickTaskDetail = _repPickTaskDetail;
        strategy._repPackage = _repPackage;
        strategy._repPackageDetail = _repPackageDetail;
        strategy._repLocation = _repLocation;
        strategy._repProduct = _repProduct;

        // 执行策略
        return await strategy.ScanOrderPickTask(input);
    }

    /// <summary>
    /// RF 扫描箱号并完成包装
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ScanBoxNumberCompletePackage")]
    public async Task<Response<string>> ScanBoxNumberCompletePackage(RFBoxNumberPackageInput input)
    {
        Response<string> response = new Response<string>();

        // 验证输入
        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务号不能为空";
            return response;
        }

        if (string.IsNullOrEmpty(input.BoxNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "箱号不能为空";
            return response;
        }

        // 获取拣货任务
        var pickTask = await _rep.AsQueryable()
            .Where(a => a.PickTaskNumber == input.PickTaskNumber)
            .FirstAsync();

        if (pickTask == null)
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务不存在";
            return response;
        }

        // 检查是否已经包装完成
        if (pickTask.PickStatus == (int)PickTaskStatusEnum.包装完成)
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务已经完成包装";
            return response;
        }

        // 从缓存获取拣货任务明细数据（包含用户扫描的内容）
        string cacheKey = "RFSinglePick:" + pickTask.CustomerId + ":" + pickTask.WarehouseId + ":" + input.PickTaskNumber;
        var cachedPickTaskDetails = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey);

        if (cachedPickTaskDetails == null || cachedPickTaskDetails.Count == 0)
        {
            // 如果缓存不存在，从数据库获取
            cachedPickTaskDetails = await _repPickTaskDetail.AsQueryable()
                .Where(a => a.PickTaskNumber == input.PickTaskNumber)
                .ToListAsync()
                .ContinueWith(t => t.Result.Adapt<List<RFSinglePickRecord>>());
        }

        // 过滤出已拣货完成的明细（PickQty > 0 表示已扫描拣货）
        var pickTaskDetails = cachedPickTaskDetails
            .Where(a => a.PickQty > 0)
            .ToList();

        if (pickTaskDetails == null || pickTaskDetails.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "没有已拣货完成的明细";
            return response;
        }

        try
        {
            // 获取拣货任务明细的订单信息
            var orderIds = pickTaskDetails.Select(a => a.OrderId).Distinct().ToList();

            // 创建包装记录（一个拣货任务可能对应多个订单，每个订单创建一个包装记录）
            var firstOrderNumber = pickTask.OrderNumber?.Split(',')[0] ?? "";
            var firstExternOrderNumber = pickTask.ExternOrderNumber?.Split(',')[0] ?? "";
            var firstOrderId = orderIds.FirstOrDefault();

            var package = new WMSPackage
            {
                PickTaskId = pickTask.Id,
                PickTaskNumber = pickTask.PickTaskNumber,
                OrderId = firstOrderId,
                OrderNumber = firstOrderNumber,
                PreOrderNumber = cachedPickTaskDetails.First().PreOrderNumber,
                ExternOrderNumber = firstExternOrderNumber,
                PackageNumber = input.BoxNumber, // 使用扫描的箱号
                CustomerId = pickTask.CustomerId,
                CustomerName = pickTask.CustomerName,
                WarehouseId = pickTask.WarehouseId,
                WarehouseName = pickTask.WarehouseName,
                PackageType = "标准箱",
                PackageStatus = 1, // 待交接
                PackageTime = DateTime.Now,
                Creator = _userManager.Account,
                CreationTime = DateTime.Now,
                DetailCount = pickTaskDetails.Count
            };

            // 插入包装记录
            var packageId = await _repPackage.InsertReturnIdentityAsync(package);

            // 创建包装明细记录 - 使用缓存中的扫描数据
            List<WMSPackageDetail> packageDetails = new List<WMSPackageDetail>();
            foreach (var detail in pickTaskDetails)
            {
                packageDetails.Add(new WMSPackageDetail
                {
                    PickTaskId = detail.PickTaskId,
                    PackageId = packageId,
                    OrderId = detail.OrderId,
                    OrderNumber = detail.OrderNumber,
                    ExternOrderNumber = detail.ExternOrderNumber,
                    PreOrderNumber = detail.PreOrderNumber,
                    PickTaskNumber = detail.PickTaskNumber,
                    PackageNumber = input.BoxNumber,
                    CustomerId = detail.CustomerId,
                    CustomerName = detail.CustomerName,
                    WarehouseId = detail.WarehouseId,
                    WarehouseName = detail.WarehouseName,
                    SKU = detail.SKU,
                    UPC = detail.UPC,
                    GoodsName = detail.GoodsName,
                    GoodsType = detail.GoodsType,
                    UnitCode = detail.UnitCode,
                    Onwer = detail.Onwer,
                    BoxCode = detail.BoxCode,
                    TrayCode = detail.TrayCode,
                    BatchCode = detail.BatchCode,
                    LotCode = detail.LotCode,
                    PoCode = detail.PoCode,
                    Qty = detail.PickQty, // 使用缓存中用户扫描的拣货数量
                    ProductionDate = detail.ProductionDate,
                    ExpirationDate = detail.ExpirationDate,
                    Creator = _userManager.Account,
                    CreationTime = DateTime.Now
                });
            }

            // 批量插入包装明细
            await _repPackageDetail.InsertRangeAsync(packageDetails);
            var entity = await _repPickTaskDetail.AsQueryable().Where(a => a.PickTaskNumber == pickTask.PickTaskNumber).ToListAsync();
            //修改拣货数量，以及判断拣货状态
            foreach (var detail in cachedPickTaskDetails)
            {

                var data = entity.Where(a => a.Id == detail.Id).First();
                data.PickQty += detail.PickQty;
                if (data.PickQty == data.Qty)
                {
                    data.PickStatus = (int)PickTaskStatusEnum.拣货完成;
                }
                //await _repPickTaskDetail.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
            }
            await _repPickTaskDetail.UpdateRangeAsync(entity);
            // 更新拣货任务状态为包装完成
            //判断是不是都包装完成了
            if (entity.Where(a => a.PickStatus == (int)PickTaskStatusEnum.新增).Count() == 0)
            {
                //判断包装信息是不是等于拣货信息
                //查询包装数量
                var packageCount = await _repPackageDetail.AsQueryable().Where(a => a.PickTaskNumber == pickTask.PickTaskNumber).SumAsync(a => a.Qty);
                //获取拣货数量
                var pickCount = entity.Sum(b => b.Qty);
                if (packageCount == pickCount)
                {
                    pickTask.PickStatus = (int)PickTaskStatusEnum.包装完成;
                    pickTask.EndTime = DateTime.Now;
                    await _rep.UpdateAsync(pickTask);
                    //await _repPickTaskDetail.AsUpdateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandAsync();
                    await _repPickTaskDetail.Context.Updateable<WMSPickTaskDetail>()
                   .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.包装完成)
                   .Where(p => p.PickTaskNumber == pickTask.PickTaskNumber)
                   .ExecuteCommandAsync();
                }
            }

            // 清除缓存
            _sysCacheService.Remove(cacheKey);

            response.Code = StatusCode.Success;
            response.Msg = $"箱号 {input.BoxNumber} 包装完成，共包装 {pickTaskDetails.Count} 条明细";
            response.Data = packageId.ToString();
        }
        catch (Exception ex)
        {
            response.Code = StatusCode.Error;
            response.Msg = $"包装失败：{ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// RF 包装完成（从Redis读取拣货信息并提交到包装表）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "CompletePackage")]
    public async Task<Response<string>> CompletePackage(RFPackageCompleteInput input)
    {
        Response<string> response = new Response<string>();

        // 验证输入
        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务号不能为空";
            return response;
        }

        if (string.IsNullOrEmpty(input.BoxNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "箱号不能为空";
            return response;
        }

        // 获取拣货任务
        var pickTask = await _rep.AsQueryable()
            .Where(a => a.PickTaskNumber == input.PickTaskNumber)
            .FirstAsync();

        if (pickTask == null)
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务不存在";
            return response;
        }

        // 从Redis获取拣货记录
        string cacheKey = $"RFSinglePick:{pickTask.CustomerId}:{pickTask.WarehouseId}:{input.PickTaskNumber}";
        var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey);

        if (pickedRecords == null || pickedRecords.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "没有可包装的拣货数据";
            return response;
        }

        // 过滤出已拣货但未包装的记录
        var recordsToPackage = pickedRecords
            .Where(a => !a.IsPackaged)
            .ToList();

        if (recordsToPackage.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "没有已拣货但未包装的商品";
            return response;
        }

        try
        {
            // 按订单分组
            var groupedRecords = recordsToPackage.GroupBy(r => r.OrderId);

            foreach (var group in groupedRecords)
            {
                var orderRecords = group.ToList();
                var orderId = group.Key;
                var orderNumber = orderRecords.First().OrderNumber;
                var externOrderNumber = orderRecords.First().ExternOrderNumber;

                // 创建包装记录
                var package = new WMSPackage
                {
                    PickTaskId = pickTask.Id,
                    PickTaskNumber = pickTask.PickTaskNumber,
                    OrderId = orderId,
                    OrderNumber = orderNumber,
                    ExternOrderNumber = externOrderNumber,
                    PackageNumber = input.BoxNumber,
                    CustomerId = pickTask.CustomerId,
                    CustomerName = pickTask.CustomerName,
                    WarehouseId = pickTask.WarehouseId,
                    WarehouseName = pickTask.WarehouseName,
                    PackageType = "标准箱",
                    PackageStatus = 1,
                    PackageTime = DateTime.Now,
                    Creator = _userManager.Account,
                    CreationTime = DateTime.Now,
                    DetailCount = orderRecords.Count
                };

                var packageId = await _repPackage.InsertReturnIdentityAsync(package);

                // 创建包装明细记录
                List<WMSPackageDetail> packageDetails = new List<WMSPackageDetail>();
                foreach (var record in orderRecords)
                {
                    packageDetails.Add(new WMSPackageDetail
                    {
                        PickTaskId = record.PickTaskId,
                        PackageId = packageId,
                        OrderId = record.OrderId,
                        OrderNumber = record.OrderNumber,
                        ExternOrderNumber = record.ExternOrderNumber,
                        PickTaskNumber = record.PickTaskNumber,
                        PackageNumber = input.BoxNumber,
                        CustomerId = record.CustomerId,
                        CustomerName = record.CustomerName,
                        WarehouseId = record.WarehouseId,
                        WarehouseName = record.WarehouseName,
                        SKU = record.SKU,
                        UPC = record.UPC,
                        GoodsName = record.GoodsName,
                        GoodsType = record.GoodsType,
                        UnitCode = record.UnitCode,
                        Onwer = record.Onwer,
                        BoxCode = record.BoxCode,
                        TrayCode = record.TrayCode,
                        BatchCode = record.BatchCode,
                        LotCode = record.LotCode,
                        PoCode = record.PoCode,
                        Qty = record.PickQty, // 使用Redis中的拣货数量
                        ProductionDate = record.ProductionDate,
                        ExpirationDate = record.ExpirationDate,
                        Creator = _userManager.Account,
                        CreationTime = DateTime.Now
                    });

                    // 标记为已包装
                    record.IsPackaged = true;
                }

                await _repPackageDetail.InsertRangeAsync(packageDetails);
            }

            // 更新缓存
            _sysCacheService.Set(cacheKey, pickedRecords);

            // 检查是否所有商品都已包装
            var allPackaged = pickedRecords.All(a => a.IsPackaged);
            if (allPackaged)
            {
                // 更新拣货任务状态为包装完成
                pickTask.PickStatus = (int)PickTaskStatusEnum.包装完成;
                pickTask.EndTime = DateTime.Now;
                await _rep.UpdateAsync(pickTask);

                // 清除缓存
                _sysCacheService.Remove(cacheKey);
            }

            response.Code = StatusCode.Success;
            response.Msg = $"箱号 {input.BoxNumber} 包装完成，共包装 {recordsToPackage.Count} 条拣货记录";
            response.Data = allPackaged ? "AllPackaged" : "Continue";
        }
        catch (Exception ex)
        {
            response.Code = StatusCode.Error;
            response.Msg = $"包装失败：{ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// 获取当前用户的拣货任务（每次进入拣货页面时调用）
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetCurrentPickTask")]
    public async Task<Response<WMSPickTask>> GetCurrentPickTask()
    {
        Response<WMSPickTask> response = new Response<WMSPickTask>();

        // 查询当前用户正在进行拣货的任务
        var currentPickTask = await _rep.AsQueryable()
            .Where(a => a.PickPlanPersonnel == _userManager.Account
                && (a.PickStatus == (int)PickTaskStatusEnum.拣货中
                    || a.PickStatus == (int)PickTaskStatusEnum.新增
                    || a.PickStatus == (int)PickTaskStatusEnum.挂起))
            .FirstAsync();

        response.Code = StatusCode.Success;
        response.Msg = currentPickTask != null ? "获取拣货任务成功" : "当前无拣货任务";
        response.Data = currentPickTask;

        return response;
    }

    /// <summary>
    /// 申请拣货任务（点击申请按钮调用）
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "ApplyPickTask")]
    public async Task<Response<WMSPickTask>> ApplyPickTask()
    {
        Response<WMSPickTask> response = new Response<WMSPickTask>();

        // 先检查当前用户是否已有拣货任务
        var existingTask = await _rep.AsQueryable()
            .Where(a => a.PickPlanPersonnel == _userManager.Account
                && (a.PickStatus == (int)PickTaskStatusEnum.拣货中
                    || a.PickStatus == (int)PickTaskStatusEnum.新增
                    || a.PickStatus == (int)PickTaskStatusEnum.挂起))
            .FirstAsync();

        if (existingTask != null)
        {
            response.Code = StatusCode.Success;
            response.Msg = $"您已有一个拣货任务：{existingTask.PickTaskNumber}";
            response.Data = existingTask;
            return response;
        }

        // 查询未被拣货的任务（状态为新增，且拣货人为空的）
        var availableTasks = await _rep.AsQueryable()
            .Where(a => a.PickStatus == (int)PickTaskStatusEnum.新增
                && (string.IsNullOrEmpty(a.PickPlanPersonnel) || a.PickPlanPersonnel == ""))
            .OrderBy(a => SqlFunc.GetRandom()) // 随机排序
            .ToListAsync();

        if (availableTasks == null || availableTasks.Count == 0)
        {
            response.Code = StatusCode.Error;
            response.Msg = "暂无可分配的拣货任务";
            return response;
        }

        // 随机选择一个任务
        var random = new Random();
        var pickTask = availableTasks[random.Next(availableTasks.Count)];

        // 更新拣货人和拣货开始时间
        pickTask.PickPlanPersonnel = _userManager.Account;
        pickTask.StartTime = DateTime.Now;
        pickTask.PickStatus = (int)PickTaskStatusEnum.拣货中;
        pickTask.Updator = _userManager.Account;
        pickTask.UpdateTime = DateTime.Now;

        await _rep.UpdateAsync(pickTask);

        response.Code = StatusCode.Success;
        response.Msg = $"成功分配拣货任务：{pickTask.PickTaskNumber}";
        response.Data = pickTask;

        return response;
    }

    /// <summary>
    /// 获取按库位排序的拣货明细（推荐拣货顺序）
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiDescriptionSettings(Name = "GetPickTaskDetailsByLocation")]
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> GetPickTaskDetailsByLocation(GetPickTaskDetailsByLocationInput input)
    {
        Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>();

        if (string.IsNullOrEmpty(input.PickTaskNumber))
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务号不能为空";
            return response;
        }

        // 获取拣货任务
        var pickTask = await _rep.AsQueryable()
            .Where(a => a.PickTaskNumber == input.PickTaskNumber)
            .FirstAsync();

        if (pickTask == null)
        {
            response.Code = StatusCode.Error;
            response.Msg = "拣货任务不存在";
            return response;
        }

        // 从Redis获取已拣货记录
        string cacheKey = $"RFSinglePick:{pickTask.CustomerId}:{pickTask.WarehouseId}:{input.PickTaskNumber}";
        var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

        // 从数据库获取拣货明细
        var pickTaskDetails = await _repPickTaskDetail.AsQueryable()
            .Where(a => a.PickTaskNumber == input.PickTaskNumber)
            .ToListAsync();

        // 按SKU+批次+库位分组
        var groupedDetails = pickTaskDetails
            .GroupBy(a => new { a.SKU, a.BatchCode, a.Location, a.Area })
            .Select(a => new
            {
                a.Key.SKU,
                a.Key.BatchCode,
                a.Key.Location,
                a.Key.Area,
                TotalQty = a.Sum(x => x.Qty),
                PickQty = a.Sum(x => x.PickQty),  
                FirstDetail = a.First()
            })
            .ToList();

        // 计算已拣货数量（从Redis统计）
        var pickQtyDict = pickedRecords
            .Where(r => !r.IsPackaged)
            .GroupBy(r => new { r.SKU, r.BatchCode, r.Location })
            .ToDictionary(g => g.Key, g => g.Sum(r => r.PickQty));

        // 转换为输出格式
        var outputList = groupedDetails.Select(g =>
        {
            var key = new { g.SKU, g.BatchCode, g.Location };
            var pickQty = pickQtyDict.ContainsKey(key) ? pickQtyDict[key] : 0;
            var isCompleted = pickQty >= g.TotalQty;

            return new WMSRFPickTaskDetailOutput
            {
                PickTaskId = pickTask.Id,
                SKU = g.SKU,
                UPC = g.FirstDetail.UPC,
                GoodsName = g.FirstDetail.GoodsName,
                GoodsType = g.FirstDetail.GoodsType,
                UnitCode = g.FirstDetail.UnitCode,
                BatchCode = g.BatchCode,
                LotCode = g.FirstDetail.LotCode,
                Qty = g.TotalQty,
                PickQty = g.PickQty + pickQty,
                Location = g.Location,
                Area = g.Area,
                Order = isCompleted ? 99 : 1, // 99表示已完成，1表示待拣货
                PickStatus = isCompleted ? (int)PickTaskStatusEnum.拣货完成 : (int)PickTaskStatusEnum.新增,
                CustomerId = pickTask.CustomerId,
                CustomerName = pickTask.CustomerName,
                WarehouseId = pickTask.WarehouseId,
                WarehouseName = pickTask.WarehouseName,
                PickTaskNumber = input.PickTaskNumber
            };
        }).Where(g => g.PickStatus == (int)PickTaskStatusEnum.新增).ToList();

        // 按库位排序（先按区域，再按库位）
        response.Data = outputList
            .OrderBy(a => a.Area)
            .ThenBy(a => a.Location)
            .ThenBy(a => a.Order).ThenBy(a => a.Id).Take(1)
            .ToList();

        response.Code = StatusCode.Success;
        response.Msg = $"获取拣货明细成功，共 {outputList.Count} 个SKU";

        return response;
    }


}


