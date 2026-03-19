---
name: adminnet-rfpick
description: 此技能提供了在Admin.NET中开发RF拣货服务的专业知识，包括WMSRFOrderPick服务的API、策略模式、Redis缓存、DTO定义和前后端交互。
---

# Admin.NET RF拣货服务开发技能

## 技能目的

此技能为在Admin.NET平台上开发和维护WMSRFOrderPick（RF拣货服务）提供全面指导。涵盖API设计、策略模式实现、Redis缓存使用、前后端数据交互等核心开发场景。

## 何时使用此技能

在以下情况使用此技能：
- 开发或修改RF拣货相关API
- 理解或修改拣货策略模式实现
- 调试Redis缓存的拣货数据
- 创建或修改拣货相关的DTO
- 前端RF拣货页面与后端API对接
- 排查RF拣货相关问题

## 文件结构

```
WMSRFOrderPick/
├── WMSRFOrderPickService.cs      # 主服务类（API入口）
├── Dto/                          # 数据传输对象
│   ├── WMSRFPickTaskInput.cs          # 拣货任务查询输入
│   ├── WMSRFPickTaskOutput.cs         # 拣货任务输出
│   ├── WMSRFPickTaskDetailInput.cs    # 拣货明细输入（扫描参数）
│   ├── WMSRFPickTaskDetailOutput.cs   # 拣货明细输出
│   ├── RFSinglePickRecord.cs          # Redis单次拣货记录
│   ├── RFBoxNumberPackageInput.cs     # 包装箱号输入
│   ├── RFPackageCompleteInput.cs      # 包装完成输入
│   ├── RFPickCacheData.cs             # 拣货缓存数据
│   └── GetPickTaskDetailsByLocationInput.cs  # 按库位查询输入
├── Interface/                    # 策略接口
│   ├── IOrderPickRFInterface.cs       # 拣货策略接口
│   ├── IOrderPickAndPackageInterface.cs # 拣货包装策略接口
│   └── IRFOrderPickInterface.cs       # RF拣货接口
├── Factory/                      # 策略工厂
│   ├── OrderPickRFFactory.cs          # 拣货策略工厂
│   └── OrderPickAndPackageFactory.cs  # 拣货包装策略工厂
└── Strategy/                     # 策略实现
    ├── OrderPickRFStrategy.cs         # 基础拣货策略
    ├── OrderPickWithRedisStrategy.cs  # Redis拣货策略（主要使用）
    ├── OrderPickAndPackageStrategy.cs # 拣货包装策略
    ├── RFPackageStrategy.cs           # RF包装策略
    └── RFPickStrategy.cs              # RF拣货策略
```

## 核心API

### 1. 分页查询拣货任务

```csharp
[HttpPost]
[ApiDescriptionSettings(Name = "Page")]
public async Task<SqlSugarPagedList<WMSPickTaskOutput>> Page(WMSPickTaskInput input)
```

**用途：** 获取待拣货任务列表

**前端调用：**
```javascript
// services/wMSRFOrderPick/wMSRFOrderPick.js
export const pageWMSRFOrderPickApi = (params) =>
    request({
        url: '/api/WMSRFOrderPick/page',
        method: 'post',
        data: params,
    });
```

### 2. 扫描拣货

```csharp
[HttpPost]
[ApiDescriptionSettings(Name = "ScanRFOrderPick")]
public async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanRFOrderPick(WMSRFPickTaskDetailInput input)
```

**用途：** RF扫描拣货（库位扫描/SKU扫描），通过工厂模式选择策略

**前端调用：**
```javascript
export const scanOrderPickTaskApi = (params) =>
    request({
        url: '/api/WMSRFOrderPick/ScanRFOrderPick',
        method: 'post',
        data: params,
    });
```

### 3. 按库位获取拣货明细

```csharp
[HttpPost]
[ApiDescriptionSettings(Name = "GetPickTaskDetailsByLocation")]
public async Task<Response<List<WMSRFPickTaskDetailOutput>>> GetPickTaskDetailsByLocation(GetPickTaskDetailsByLocationInput input)
```

**用途：** 进入拣货页面时，获取按库位排序的拣货明细

**前端调用：**
```javascript
export const getPickTaskDetailsByLocationApi = (params) =>
    request({
        url: '/api/WMSRFOrderPick/GetPickTaskDetailsByLocation',
        method: 'post',
        data: params,
    });
```

### 4. 扫描箱号完成包装

```csharp
[HttpPost]
[ApiDescriptionSettings(Name = "ScanBoxNumberCompletePackage")]
public async Task<Response<string>> ScanBoxNumberCompletePackage(RFBoxNumberPackageInput input)
```

**用途：** 完成包装，将Redis中的拣货记录提交到数据库

**前端调用：**
```javascript
export const scanBoxNumberCompletePackageApi = (params) =>
    request({
        url: '/api/WMSRFOrderPick/ScanBoxNumberCompletePackage',
        method: 'post',
        data: params,
    });
```

### 5. 清理拣货缓存

```csharp
[HttpPost]
[ApiDescriptionSettings(Name = "ClearPickCache")]
public async Task<Response<string>> ClearPickCache(GetPickTaskDetailsByLocationInput input)
```

**用途：** 清除当前拣货任务的Redis缓存（重新拣货）

**前端调用：**
```javascript
export const clearPickCacheApi = (params) =>
    request({
        url: '/api/WMSRFOrderPick/ClearPickCache',
        method: 'post',
        data: params,
    });
```

## 策略模式实现

### 策略接口定义

```csharp
public interface IOrderPickRFInterface
{
    // 注入的依赖
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public UserManager _userManager { get; set; }
    public SysCacheService _sysCacheService { get; set; }
    public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
    public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
    public SqlSugarRepository<WMSLocation> _repLocation { get; set; }
    public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SqlSugarRepository<WMSProductBom> _repProductBom { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    
    // 核心方法
    Task<Response<List<WMSRFPickTaskDetailOutput>>> OrderPickTask(WMSRFPickTaskInput request);
    Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanOrderPickTask(WMSRFPickTaskDetailInput request);
}
```

### 策略工厂

```csharp
public class OrderPickRFFactory
{
    public static IOrderPickRFInterface OrderPickTask(string FactoryName)
    {
        switch (FactoryName)
        {
            case "Hach":
                return new OrderPickWithRedisStrategy();  // 使用Redis的策略
            default:
                return new OrderPickRFStrategy();         // 基础策略
        }
    }
}
```

### 在服务中使用策略

```csharp
// 获取工作流配置
var workflow = await _repWorkFlowService.GetSystemWorkFlow(
    pickTask.CustomerName, 
    OutboundWorkFlowConst.Workflow_Outbound, 
    OutboundWorkFlowConst.Workflow_Outbound_RF_PICK, 
    pickTask.PickType
);

// 通过工厂获取策略
IOrderPickRFInterface strategy = OrderPickRFFactory.OrderPickTask("Hach");

// 手动注入依赖
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
strategy._repProductBom = _repProductBom;

// 执行策略
return await strategy.ScanOrderPickTask(input);
```

## Redis缓存使用

### 缓存键格式

```
RFSinglePick:{CustomerId}:{WarehouseId}:{PickTaskNumber}
```

### 核心数据结构 - RFSinglePickRecord

每次扫描拣货生成一条记录，存储在Redis中：

```csharp
public class RFSinglePickRecord
{
    public string RecordId { get; set; } = Guid.NewGuid().ToString();
    public long Id { get; set; }
    public long PickTaskId { get; set; }
    public string PickTaskNumber { get; set; }
    public long OrderId { get; set; }
    public string OrderNumber { get; set; }
    public string SKU { get; set; }
    public string UPC { get; set; }
    public string GoodsName { get; set; }
    public string BatchCode { get; set; }
    public string Location { get; set; }
    public string Area { get; set; }
    public double Qty { get; set; }
    public double PickQty { get; set; } = 1;  // 每次扫描拣货1件
    public DateTime PickTime { get; set; }
    public string PickPersonnel { get; set; }
    public bool IsPackaged { get; set; } = false;
    public long CustomerId { get; set; }
    public long WarehouseId { get; set; }
    // ... 其他字段
}
```

### Redis操作示例

```csharp
// 构建缓存键
string cacheKey = $"RFSinglePick:{pickTask.CustomerId}:{pickTask.WarehouseId}:{input.PickTaskNumber}";

// 获取缓存
var pickedRecords = _sysCacheService.Get<List<RFSinglePickRecord>>(cacheKey) ?? new List<RFSinglePickRecord>();

// 添加拣货记录
var pickRecord = new RFSinglePickRecord
{
    RecordId = Guid.NewGuid().ToString(),
    Id = pickTaskDetail.Id,
    PickTaskId = pickTaskDetail.PickTaskId,
    SKU = pickTaskDetail.SKU,
    Location = pickTaskDetail.Location,
    PickQty = 1,  // 每次扫描拣货1件
    PickTime = DateTime.Now,
    PickPersonnel = _userManager.Account,
    IsPackaged = false
};
pickedRecords.Add(pickRecord);

// 更新缓存
_sysCacheService.Set(cacheKey, pickedRecords);

// 清除缓存（重新拣货或包装完成后）
_sysCacheService.Remove(cacheKey);
```

## 拣货流程详解

### 完整拣货流程

```
1. 进入拣货页面
   └─> 调用 GetPickTaskDetailsByLocation 获取按库位排序的拣货明细
   
2. 扫描库位
   └─> 调用 ScanRFOrderPick，参数 scanInput = 库位条码
   └─> 返回 Location 匹配的拣货明细
   
3. 扫描SKU拣货
   └─> 调用 ScanRFOrderPick，参数 scanInput = SKU条码
   └─> 创建 RFSinglePickRecord 存入Redis
   └─> 每次扫描拣货1件
   
4. 完成包装
   └─> 调用 ScanBoxNumberCompletePackage
   └─> 从Redis读取拣货记录
   └─> 创建 WMSPackage 和 WMSPackageDetail
   └─> 更新 WMSPickTaskDetail.PickQty
   └─> 清除Redis缓存
   
5. 重新拣货（可选）
   └─> 调用 ClearPickCache
   └─> 清除Redis缓存，可重新扫描
```

### 前端拣货页面交互

```vue
<!-- 扫描输入框 -->
<input v-model="form.scanInput" @confirm="scanAcquisition()" placeholder="请扫描条码/SKU" />

<script>
methods: {
    // 扫描拣货
    async scanAcquisition() {
        let res = await scanOrderPickTaskApi(this.form);
        if (res.data.result.code == "1") {
            if (res.data.result.msg == "Location") {
                // 扫描的是库位
                this.form.location = res.data.result.data[0].location;
            } else if (res.data.result.msg == "SKU") {
                // 扫描的是SKU，拣货成功
                await this.getPickDetailsByLocation();
            }
            playSuccessSound();
        } else {
            playErrorSound();
        }
    },
    
    // 完成包装
    async handleScanBoxNumber() {
        let res = await scanBoxNumberCompletePackageApi({
            pickTaskNumber: this.packageForm.pickTaskNumber,
            boxNumber: this.packageForm.boxNumber
        });
        if (res.data.result) {
            uni.navigateBack();
        }
    },
    
    // 重新拣货
    async handleClearPickCache() {
        let res = await clearPickCacheApi({
            pickTaskNumber: this.form.pickTaskNumber
        });
        if (res.data.result.code == "1") {
            await this.getPickDetailsByLocation();
        }
    }
}
</script>
```

## DTO字段说明

### WMSRFPickTaskDetailInput（扫描输入）

| 字段 | 类型 | 说明 |
|------|------|------|
| PickTaskNumber | string | 拣货任务号 |
| ScanInput | string | 扫描输入（库位条码/SKU条码） |
| Location | string | 当前库位 |
| Id | long | 拣货任务ID |

### WMSRFPickTaskDetailOutput（拣货明细输出）

| 字段 | 类型 | 说明 |
|------|------|------|
| SKU | string | 商品编码 |
| UPC | string | 商品条码 |
| GoodsName | string | 商品名称 |
| Location | string | 库位 |
| Area | string | 区域 |
| BatchCode | string | 批次编码 |
| Qty | double | 应拣数量 |
| PickQty | double | 已拣数量 |
| Order | int | 排序（99=已完成） |
| PickStatus | int | 拣货状态 |

## 拣货任务状态枚举

```csharp
public enum PickTaskStatusEnum
{
    新增 = 1,
    拣货中 = 2,
    拣货完成 = 3,
    包装完成 = 4
}
```

## 常见开发场景

### 1. 添加新的拣货策略

```csharp
// 1. 实现 IOrderPickRFInterface
public class NewPickStrategy : IOrderPickRFInterface
{
    public async Task<Response<List<WMSRFPickTaskDetailOutput>>> ScanOrderPickTask(WMSRFPickTaskDetailInput request)
    {
        // 自定义拣货逻辑
    }
}

// 2. 在工厂中注册
public static IOrderPickRFInterface OrderPickTask(string FactoryName)
{
    switch (FactoryName)
    {
        case "NewClient":
            return new NewPickStrategy();
        // ...
    }
}
```

### 2. 添加新的API

```csharp
// WMSRFOrderPickService.cs
[HttpPost]
[ApiDescriptionSettings(Name = "NewApi")]
public async Task<Response<YourOutput>> NewApi(YourInput input)
{
    // 实现
}
```

### 3. 前端添加新的API调用

```javascript
// services/wMSRFOrderPick/wMSRFOrderPick.js
let NewApi = '/api/WMSRFOrderPick/NewApi';

export const newApiCall = (params) =>
    request({
        url: NewApi,
        method: 'post',
        data: params,
    });
```

### 4. 修改Redis缓存过期时间

```csharp
// 设置带过期时间的缓存
_sysCacheService.Set(cacheKey, pickedRecords, TimeSpan.FromHours(8));
```

## 关键要点

1. **策略模式**：不同客户可以使用不同的拣货策略
2. **Redis缓存**：每次扫描生成一条记录，包装时批量提交
3. **扫描解析**：支持多种条码格式（SKU|LOT|EXP、HTTP二维码等）
4. **库位排序**：按库位展示拣货明细，优化拣货路径
5. **响应格式**：统一使用 `Response<T>` 包装响应
6. **前后端分离**：前端uniapp，后端API通过动态控制器暴露
