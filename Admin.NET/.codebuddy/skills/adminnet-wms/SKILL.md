---
name: adminnet-wms
description: 此技能提供了在Admin.NET中进行WMS（仓库管理系统）开发的专业知识，包括RF拣货、收货、包装工作流程、策略模式实现和特定领域的实体。
---

# Admin.NET WMS开发技能

## 技能目的

此技能为在Admin.NET平台上开发和维护WMS（仓库管理系统）功能提供全面指导。它涵盖了RF（射频）拣货操作、收货工作流程、包装流程、策略模式实现和领域特定的数据结构。

## 何时使用此技能

在以下情况使用此技能：
- 开发新的WMS功能或服务（拣货、收货、包装、库存管理）
- 实现或修改RF工作流程（RF拣货、RF收货、RF包装）
- 为不同客户使用策略模式实现
- 创建或修改WMS实体和DTO
- 排查WMS相关问题
- 理解WMS业务逻辑和数据流

## 核心WMS概念

### WMS业务流程

```
订单生成 → 拣货任务生成 → RF拣货 → 包装 → 交接 → 发货
     |                    |                   |          |          |          |
     |                    |                   |          |          |          → 创建指令（同步到外部系统）
     |                    |                   |          |          → 更新交接状态
     |                    |                   |          → 创建包装记录和明细
     |                    |                   → 扫描一次=拣货一次，存储在Redis中，包装时提交
     |                    → 从订单明细生成拣货明细
     → 从外部系统接收订单
```

### 关键实体

**WMSPickTask（拣货任务）：**
- 拣货操作的主要实体
- 字段：PickTaskNumber、OrderNumber、ExternOrderNumber、PickStatus、StartTime、EndTime
- 导航：Details（WMSPickTaskDetail列表）

**WMSPickTaskDetail（拣货明细）：**
- 需要拣货的单个商品
- 字段：SKU、UPC、Location、Area、BatchCode、Qty、PickQty、PickStatus
- 关键关系：PickTaskId关联到WMSPickTask

**WMSPackage（包装）：**
- 包装商品的容器
- 字段：PackageNumber、PackageStatus、PackageTime、DetailCount
- 导航：Details（WMSPackageDetail列表）

**WMSReceiptReceiving（收货）：**
- 到货处理
- 字段：ReceiptNumber、CustomerId、WarehouseId、ReceiptType
- 导航：Details（WMSReceiptReceivingDetail列表）

### 拣货任务状态枚举

```csharp
public enum PickTaskStatusEnum
{
    New = 1,           // 新增
    Picking = 10,      // 拣货中
    Suspended = 20,    // 挂起
    Picked = 99,       // 拣货完成
    Packaged = 100     // 包装完成
}
```

## 服务层开发

### 标准服务结构

```csharp
[ApiDescriptionSettings(ApplicationConst.GroupName, Order = 100)]
public class WMSRFOrderPickService : IDynamicApiController, ITransient
{
    // 1. 仓储注入
    private readonly SqlSugarRepository<WMSPickTask> _rep;
    private readonly SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail;
    
    // 2. 服务注入
    private readonly UserManager _userManager;
    private readonly SysCacheService _sysCacheService;
    
    // 3. 构造函数
    public WMSRFOrderPickService(
        SqlSugarRepository<WMSPickTask> rep,
        UserManager userManager,
        SysCacheService sysCacheService)
    {
        _rep = rep;
        _userManager = userManager;
        _sysCacheService = sysCacheService;
    }
    
    // 4. API方法
    [HttpPost]
    [ApiDescriptionSettings(Name = "Page")]
    public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
    {
        // 实现
    }
}
```

### DTO命名规范

- 输入DTO：`{Entity}Input` 或 `{Operation}Input`（例如：`WMSPickTaskInput`、`RFSinglePickInput`）
- 输出DTO：`{Entity}Output` 或 `{Operation}Output`（例如：`WMSPickTaskOutput`、`RFPickResultOutput`）
- 数据传输：`{Entity}Dto`（例如：`WMSPickTaskDto`）

## 策略模式实现

### 模式组件

1. **接口：** 定义契约（例如：`IOrderPickRFInterface`）
2. **策略：** 为不同场景实现业务逻辑
3. **工厂：** 根据配置创建适当的策略

### 创建策略

**1. 定义接口：**

```csharp
public interface IOrderPickRFInterface
{
    // 注入的依赖（用于手动注入的公共属性）
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    
    // 核心方法
    Task<Response<List<WMSRFPickTaskDetailOutput>>> OrderPickTask(WMSRFPickTaskInput request);
    Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanOrderPickTask(WMSRFPickTaskDetailInput request);
}
```

**2. 实现策略：**

```csharp
public class OrderPickWithRedisStrategy : IOrderPickRFInterface
{
    // 实现接口方法
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> OrderPickTask(WMSRFPickTaskInput request)
    {
        // 使用Redis缓存的实现
    }
}
```

**3. 创建工厂：**

```csharp
public class OrderPickRFFactory
{
    public static IOrderPickRFInterface OrderPickTask(string FactoryName)
    {
        switch (FactoryName)
        {
            case "Hach":
                return new OrderPickWithRedisStrategy();
            default:
                return new OrderPickRFStrategy();
        }
    }
}
```

**4. 在服务中使用策略：**

```csharp
// 获取工作流配置
var workflow = await _repWorkFlowService.GetSystemWorkFlow(
    customerName, 
    InboundWorkFlowConst.Workflow_Inbound,
    InboundWorkFlowConst.Workflow__RF_ReceiptReceiving,
    receiptType
);

// 从工厂获取策略
IOrderPickRFInterface strategy = OrderPickRFFactory.OrderPickTask(workflow);

// 手动注入依赖
strategy._repPickTask = _rep;
strategy._userManager = _userManager;
strategy._sysCacheService = _sysCacheService;

// 执行策略
return await strategy.OrderPickTask(input);
```

## 使用Redis策略的RF拣货

### 核心概念

每次扫描=拣货一次。在Redis中存储单独的拣货记录，然后在包装时一次性提交。

### 关键流程

**1. 初始化拣货任务：**

```csharp
// 从数据库获取拣货明细，按SKU/批次/库位分组
var orderPickTask = await _repPickTaskDetail.AsQueryable()
    .Where(a => a.PickTaskId == request.Id)
    .GroupBy(a => new { a.SKU, a.BatchCode, a.Location })
    .Select(a => new WMSRFPickTaskDetailOutput()
    {
        SKU = a.Key.SKU,
        Qty = SqlFunc.AggregateSum(a.Qty),
        PickQty = SqlFunc.AggregateSum(a.PickQty),
        BatchCode = a.Key.BatchCode,
        Location = a.Key.Location,
        Order = (SqlFunc.AggregateSum(a.Qty) == SqlFunc.AggregateSum(a.PickQty) ? 99 : 1)
    }).ToListAsync();

// 从Redis获取已拣货记录
string cacheKey = $"RFSinglePick:{customerId}:{warehouseId}:{pickTaskNumber}";
var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

// 从Redis计算实际拣货数量
var pickQtyDict = pickedRecords
    .Where(r => !r.IsPackaged)
    .GroupBy(r => new { r.SKU, r.BatchCode, r.Location })
    .ToDictionary(g => g.Key, g => g.Sum(r => r.PickQty));

// 更新拣货数量
foreach (var item in orderPickTask)
{
    var key = new { item.SKU, item.BatchCode, item.Location };
    if (pickQtyDict.ContainsKey(key))
    {
        item.PickQty = pickQtyDict[key];
        item.Order = (item.Qty == item.PickQty) ? 99 : 1;
    }
}
```

**2. 扫描拣货：**

```csharp
// 解析扫描输入（支持多种格式：条码、HTTP二维码等）
private void ParseScanInput(WMSRFPickTaskDetailInput request)
{
    // 条码格式：SKU|LOT|EXP
    if (request.ScanInput.Split(' ').Length > 1 || request.ScanInput.Split('|').Length > 1)
    {
        string skuRegex = @"(?<=\|ITM)[^|]+|^[^\s:]+=[0-9]{3,4}[CN]{0,2}(?=[0-9]{5}\b)";
        MatchCollection matchesSKU = Regex.Matches(request.ScanInput, skuRegex);
        request.SKU = matchesSKU[0].Value;
    }
    // HTTP二维码
    else if (request.ScanInput.Contains("http"))
    {
        Uri uri = new Uri(request.ScanInput);
        var p = HttpUtility.ParseQueryString(uri.Query)["p"];
        request.SKU = p.Split(':')[1];
    }
    // 直接商品条码扫描
    else
    {
        var checkProduct = _repProduct.AsQueryable()
            .Where(m => m.SKU == request.ScanInput)
            .First();
        request.SKU = checkProduct.SKU;
    }
}

// 扫描SKU拣货
private async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanSKU(WMSRFPickTaskDetailInput request)
{
    // 查找匹配的拣货明细
    var pickTaskDetail = await _repPickTaskDetail.AsQueryable()
        .Where(a => a.PickTaskId == request.Id
            && a.SKU == request.SKU
            && a.Location == request.Location
            && a.PickQty < a.Qty)
        .FirstAsync();
    
    // 检查是否已完全拣货
    var currentPickedQty = pickedRecords
        .Where(r => r.SKU == request.SKU && r.Location == request.Location && !r.IsPackaged)
        .Sum(r => r.PickQty);
    
    if (currentPickedQty >= pickTaskDetail.Qty)
    {
        return new Response<List<WMSRFPickTaskDetailOutput>>
        {
            Code = StatusCode.Error,
            Msg = "该产品已经全部拣货完成"
        };
    }
    
    // 创建单独拣货记录（每次扫描=拣货1件）
    var pickRecord = new RFSinglePickRecord
    {
        RecordId = Guid.NewGuid().ToString(),
        Id = pickTaskDetail.Id,
        PickTaskId = pickTaskDetail.PickTaskId,
        SKU = pickTaskDetail.SKU,
        Qty = pickTaskDetail.Qty,
        PickQty = 1,  // 每次扫描只拣货1件
        Location = pickTaskDetail.Location,
        PickTime = DateTime.Now,
        PickPersonnel = _userManager.Account,
        IsPackaged = false
    };
    
    // 添加到Redis缓存
    pickedRecords.Add(pickRecord);
    _sysCacheService.Set(cacheKey, pickedRecords);
    
    // 更新拣货任务状态
    await _repPickTask.Context.Updateable<WMSPickTask>()
        .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.Picking)
        .Where(p => p.Id == request.Id)
        .ExecuteCommandAsync();
}
```

**3. 包装完成：**

```csharp
// 从Redis获取拣货记录
string cacheKey = $"RFSinglePick:{customerId}:{warehouseId}:{pickTaskNumber}";
var cachedPickTaskDetails = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey);

// 筛选已拣货但未包装的记录
var pickTaskDetails = cachedPickTaskDetails.Where(a => a.PickQty > 0).ToList();

// 使用导航属性创建包装记录
var package = new WMSPackage
{
    PickTaskId = pickTask.Id,
    PickTaskNumber = pickTask.PickTaskNumber,
    PackageNumber = input.BoxNumber,
    PackageType = "标准箱",
    PackageStatus = 1,
    PackageTime = DateTime.Now,
    Creator = _userManager.Account,
    DetailCount = pickTaskDetails.Count
};

// 创建包装明细记录
package.Details = pickTaskDetails.GroupBy(a => new { a.SKU, a.BatchCode })
    .Select(a => new WMSPackageDetail()
    {
        // 字段映射
        Qty = a.Sum(b => b.Qty)  // 使用Redis中的拣货数量
    }).ToList();

// 使用导航属性插入主表和明细
await _repPackage.Context.InsertNav(package).Include(a => a.Details).ExecuteCommandAsync();

// 更新拣货明细数量
foreach (var detail in cachedPickTaskDetails)
{
    var data = entity.Where(a => a.Id == detail.Id).First();
    data.PickQty += detail.PickQty;
    if (data.PickQty == data.Qty)
    {
        data.PickStatus = (int)PickTaskStatusEnum.Picked;
    }
}
await _repPickTaskDetail.UpdateRangeAsync(entity);

// 清除Redis缓存
_sysCacheService.Remove(cacheKey);
```

## 常用查询模式

### 动态条件查询

```csharp
var query = _rep.AsQueryable()
    .WhereIF(!string.IsNullOrWhiteSpace(input.PickTaskNumber), 
        u => u.PickTaskNumber.Contains(input.PickTaskNumber.Trim()))
    .WhereIF(input.CustomerId > 0, 
        u => u.CustomerId == input.CustomerId)
    .WhereIF(input.PickStatus != 0, 
        u => u.PickStatus == input.PickStatus)
    .WhereIF(input.StartTime != null, 
        u => u.StartTime >= input.StartTime)
    .WhereIF(input.EndTime != null, 
        u => u.EndTime <= input.EndTime)
    .Select<WMSPickTaskOutput>();
```

### 分组和聚合

```csharp
var result = await _repPickTaskDetail.AsQueryable()
    .Where(a => a.PickTaskId == request.Id)
    .GroupBy(a => new { a.SKU, a.BatchCode, a.Location })
    .Select(a => new
    {
        a.Key.SKU,
        a.Key.Location,
        TotalQty = SqlFunc.AggregateSum(a.Qty),
        PickQty = SqlFunc.AggregateSum(a.PickQty),
        Order = (SqlFunc.AggregateSum(a.Qty) == SqlFunc.AggregateSum(a.PickQty) ? 99 : 1)
    }).ToListAsync();
```

### 分页

```csharp
public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
{
    var query = _rep.AsQueryable()
        .WhereIF(condition, expression)
        .Select<WMSPickTaskOutput>();
    
    query = query.OrderBuilder(input);  // 动态排序
    return await query.ToPagedListAsync(input.Page, input.PageSize);
}
```

## Redis缓存操作

### 缓存键格式

```
{模块}:{Id}:{键}
示例：RFSinglePick:123:TASK001
```

### 常用操作

```csharp
// 设置缓存
string cacheKey = $"RFSinglePick:{customerId}:{warehouseId}:{pickTaskNumber}";
_sysCacheService.Set(cacheKey, data);

// 获取缓存
var data = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey);

// 检查缓存是否存在
if (!_sysCacheService.ExistKey(cacheKey)) { }

// 删除缓存
_sysCacheService.Remove(cacheKey);
```

## 导航属性操作

### 使用导航属性插入

```csharp
// 一次操作插入主表和明细
await _repPackage.Context.InsertNav(package)
    .Include(a => a.Details)
    .ExecuteCommandAsync();
```

### 使用导航属性查询

```csharp
// 使用导航属性查询
var pickTask = await _rep.AsQueryable()
    .Includes(a => a.Details)
    .FirstAsync(a => a.Id == taskId);
```

## 响应模式

```csharp
Response<List<WMSRFPickTaskDetailOutput>> response = new Response<List<WMSRFPickTaskDetailOutput>>
{
    Code = StatusCode.Success,
    Msg = "操作成功",
    Data = data
};
```

## 批量操作

### 批量插入

```csharp
// 简单批量插入
await _repPickTaskDetail.InsertRangeAsync(entityList);

// 导航属性批量插入（主表+明细）
await _repPackage.Context.InsertNav(package)
    .Include(a => a.Details)
    .ExecuteCommandAsync();
```

### 批量更新

```csharp
// 简单批量更新
await _repPickTaskDetail.UpdateRangeAsync(entityList);

// 条件批量更新
await _repPickTask.Context.Updateable<WMSPickTask>()
    .SetColumns(p => p.PickStatus == (int)PickTaskStatusEnum.Picking)
    .Where(p => p.Id == request.Id)
    .ExecuteCommandAsync();
```

## WMS服务位置

WMS服务位于 `Admin.NET.Application/Service/`：

```
WMSRFOrderPick/              # RF拣货服务
WMSRFReceiptReceiving/       # RF收货服务
WMSPackageLable/             # 包装标签服务
WMSASN/                      # 预先发货通知
WMSReceipt/                  # 收货单
WMSOrder/                    # 订单管理
WMSPickTask/                 # 拣货任务
WMSPackage/                  # 包装
WMSProduct/                  # 商品
WMSInventoryUsable/          # 可用库存
WMSStockCheck/               # 盘点
```

每个服务通常包含：
- 主服务类（例如：`WMSRFOrderPickService.cs`）
- DTO文件夹，用于输入/输出对象
- Strategy文件夹，用于策略实现
- Factory文件夹，用于策略创建
- Interface文件夹，用于策略契约

## 关键要点

1. **策略模式：** 广泛用于支持不同客户的工作流程
2. **Redis缓存：** RF拣货使用Redis存储单独的拣货记录，包装时提交
3. **导航属性：** 使用 `InsertNav()` 和 `Include()` 进行主从表操作
4. **动态查询：** 使用 `WhereIF()` 进行条件查询构建
5. **响应包装：** 始终使用 `Response<T>` 包装响应
6. **命名规范：** Input/Output/Dto后缀以清晰区分
7. **服务属性：** 使用 `[ApiDescriptionSettings]` 修饰，实现 `IDynamicApiController` 和 `ITransient`
