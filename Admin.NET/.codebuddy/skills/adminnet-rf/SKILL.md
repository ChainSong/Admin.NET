---
name: adminnet-rf
description: 此技能提供了在Admin.NET平台中使用uni-app开发RF（射频）移动应用的专业知识，包括页面开发、API集成和UI组件使用。
---

# Admin.NET RF移动开发技能

## 技能目的

此技能为在Admin.NET平台中使用uni-app框架开发RF（射频）移动应用提供全面指导。它涵盖了页面开发、API服务集成、UI组件（ColorUI）、条码扫描、音频反馈和移动端特定工作流程。

## 何时使用此技能

在以下情况使用此技能：
- 开发新的RF移动页面（拣货、收货、盘点、包装）
- 将RF页面与后端API集成
- 使用uni-app框架和ColorUI组件
- 实现条码/扫描功能
- 创建或修改RF API服务
- 排查RF移动应用问题

## RF项目结构

```
Admin.NET.RF/
├── pages/                      # 页面目录
│   ├── wMSRFOrderPick/        # RF拣货页面
│   │   ├── index.vue         # 拣货任务列表
│   │   └── component/
│   │       └── editDialog.vue # 拣货明细编辑
│   ├── wMSRFReceiptReceiving/ # RF收货页面
│   ├── wMSRFStockCheck/      # RF盘点
│   ├── login/                # 登录页
│   └── menu/                 # 菜单页
│
├── services/                   # API服务层
│   ├── wMSRFOrderPick/
│   │   └── wMSRFOrderPick.js  # 拣货API
│   ├── common/
│   │   ├── playaudio.js      # 音频播放
│   │   └── menu.js
│   └── login/
│       └── index.js
│
├── components/                 # 组件目录
│   ├── you-scroll.vue        # 自定义滚动组件
│   └── t-table/              # 表格组件
│
├── static/                     # 静态资源
│   ├── success.mp3           # 成功音效
│   ├── error.mp3             # 错误音效
│   └── logo.png
│
├── utils/                      # 工具函数
│   ├── request.js            # 请求封装
│   └── thermal-print.js      # 热敏打印
│
├── colorui/                   # ColorUI框架
├── config/                     # 配置文件
│   └── index.js              # 环境配置
├── store/                     # Vuex状态管理
├── App.vue                   # 应用入口
├── main.js                   # 主入口
├── pages.json                # 页面路由配置
└── manifest.json             # 应用配置
```

## Page Development

### Standard Page Structure

```vue
<template>
    <view>
        <!-- Custom navigation bar -->
        <cu-custom bgColor="bg-gradual-blue" :isBack="true">
            <block slot="content">Page Title</block>
        </cu-custom>
        
        <!-- Page content -->
        <view class="content">
            <!-- UI components -->
        </view>
    </view>
</template>

<script>
import { apiFunction } from "@/services/module/service";

export default {
    data() {
        return {
            form: {},
            list: []
        };
    },
    onLoad(options) {
        // Page load initialization
        this.form.id = options.id;
    },
    methods: {
        // Methods
    }
}
</script>

<style>
/* Styles */
</style>
```

### List Page Template

```vue
<template>
    <view>
        <cu-custom bgColor="bg-gradual-blue" :isBack="true">
            <block slot="content">RF拣货</block>
        </cu-custom>
        
        <!-- Query area -->
        <view class="cu-form-group bg-white">
            <input placeholder="请输入拣货任务号" v-model="form.pickTaskNumber"/>
            <button class="cu-btn bg-blue" @tap="getOrderList()">查询</button>
        </view>
        
        <!-- Task list -->
        <view v-if="this.list.length>0">
            <view v-for="(item, index) in this.list" :key="index" class="cu-item">
                <!-- Status badge -->
                <view class="cu-avatar round" :class="getStatusClass(item.pickStatus)">
                    {{item.pickStatus}}
                </view>
                
                <!-- Task info -->
                <view class="content">
                    <view class="text-lg text-bold">{{item.pickTaskNumber}}</view>
                    <view>订单号: {{item.orderNumber}}</view>
                    <view>外部订单: {{item.externOrderNumber}}</view>
                    <view>仓库: {{item.warehouseName}}</view>
                    <view>客户: {{item.customerName}}</view>
                    <view>状态: {{getStatusText(item.pickStatus)}}</view>
                </view>
                
                <!-- Action button -->
                <view class="action">
                    <button class="cu-btn bg-blue" @tap="goCollect(item)">拣货</button>
                </view>
            </view>
        </view>
    </view>
</template>

<script>
import { pageWMSRFOrderPickApi } from "@/services/wMSRFOrderPick/wMSRFOrderPick";

export default {
    data() {
        return {
            form: {
                pickTaskNumber: '',
            },
            list: [],
        };
    },
    methods: {
        // Get status text
        getStatusText(status) {
            const statusMap = {
                1: '待拣货',
                2: '拣货中',
                3: '拣货完成',
                4: '包装完成'
            };
            return statusMap[status] || `状态${status}`;
        },
        
        // Get status class
        getStatusClass(status) {
            const classMap = {
                1: 'bg-grey',
                2: 'bg-orange',
                3: 'bg-green',
                4: 'bg-blue'
            };
            return classMap[status] || 'bg-grey';
        },
        
        // Navigate to detail
        goCollect(row) {
            uni.navigateTo({
                url: '/pages/wMSRFOrderPick/component/editDialog?' +
                     'pickTaskNumber=' + row.pickTaskNumber +
                     '&id=' + row.id
            });
        },
        
        // Get task list
        async getOrderList() {
            await pageWMSRFOrderPickApi(this.form).then((res) => {
                this.list = res.data.result.items ?? [];
            });
        }
    }
}
</script>
```

### Detail/Edit Page Template

```vue
<template>
    <view>
        <!-- Task info card -->
        <view class="cu-card">
            <view class="title">任务信息</view>
            <view class="content">
                <text>拣货任务号: {{form.pickTaskNumber}}</text>
            </view>
        </view>
        
        <!-- Detail list -->
        <view v-if="this.list.length>0">
            <view class="cu-bar">拣货明细</view>
            <view v-for="(item, index) in this.list" :key="index"
                  class="cu-item" :class="{'completed-item': item.order === 99}">
                <!-- Quantity badge -->
                <view class="cu-avatar" :class="getPickStatusClass(item)">
                    {{item.pickQty}}/{{item.qty}}
                </view>
                
                <!-- Product info -->
                <view class="content">
                    <view>SKU: {{item.sku}} ({{item.goodsName}})</view>
                    <view>库位: {{item.location}} | 区域: {{item.area}}</view>
                    <view v-if="item.batchCode">批次: {{item.batchCode}}</view>
                    
                    <!-- Quantity info -->
                    <view class="flex">
                        <text>应拣: {{item.qty}}</text>
                        <text>已拣: {{item.pickQty}}</text>
                        <text v-if="item.order === 99">待拣: 0</text>
                        <text v-else>待拣: {{item.qty - item.pickQty}}</text>
                    </view>
                    
                    <!-- Completion marker -->
                    <view v-if="item.order === 99" class="text-center">
                        <text class="cuIcon-roundcheck text-green"></text>
                        <text class="text-green">已完成</text>
                    </view>
                </view>
            </view>
        </view>
        
        <!-- Scan area -->
        <view class="cu-form-group">
            <view class="title">库位</view>
            <input disabled v-model="form.location"/>
        </view>
        <view class="cu-form-group">
            <view class="title">扫描</view>
            <input v-model="form.scanInput" 
                   :focus="focusflag"
                   @confirm="scanAcquisition()" 
                   placeholder="请扫描条码/SKU"/>
        </view>
    </view>
</template>

<script>
import { scanOrderPickTaskApi } from "@/services/wMSRFOrderPick/wMSRFOrderPick";
import { playErrorSound, playSuccessSound } from "@/services/common/playaudio.js";

export default {
    data() {
        return {
            focusflag: true,
            form: {
                pickTaskNumber: "",
                id: "",
                scanInput: "",
                location: "",
            },
            list: [],
        };
    },
    
    onLoad(options) {
        this.form.pickTaskNumber = options.pickTaskNumber;
        this.form.id = options.id;
        this.getPickDetails();
    },
    
    methods: {
        // Get pick status class
        getPickStatusClass(item) {
            if (item.order === 99) return 'bg-green';     // Completed
            else if (item.pickQty > 0) return 'bg-orange'; // Partially picked
            else return 'bg-blue';                        // Not picked
        },
        
        // Scan to pick
        async scanAcquisition() {
            let res = await scanOrderPickTaskApi(this.form);
            
            if (res.data.result.code == "1") {
                if (res.data.result.msg == "Location") {
                    this.form.location = this.form.scanInput;
                } else if (res.data.result.msg == "SKU") {
                    this.form.location = res.data.result.data[0].location;
                }
                
                await this.getPickDetails();
                playSuccessSound();
            } else {
                uni.showToast({ title: "操作失败", icon: 'none' });
                playErrorSound();
            }
            
            // Reset input focus
            this.focusflag = false;
            this.$nextTick(() => { this.focusflag = true; });
        },
        
        // Get pick details
        async getPickDetails() {
            let res = await getPickTaskDetailsApi({
                pickTaskNumber: this.form.pickTaskNumber
            });
            this.list = res.data.result.data || [];
            
            // Get first pending item's location
            const firstPendingItem = this.list.find(item => item.order !== 99);
            if (firstPendingItem) {
                this.form.location = firstPendingItem.location;
            }
        }
    }
}
</script>
```

## API Service Development

### Service File Structure

```javascript
import request from '@/utils/request'

// API endpoint definitions
const ApiEndpoint1 = '/api/Module/Action';
const ApiEndpoint2 = '/api/Module/AnotherAction';

// API functions
export const apiFunction1 = (params) =>
    request({
        url: ApiEndpoint1,
        method: 'post',
        data: params,
    });

export const apiFunction2 = (params) =>
    request({
        url: ApiEndpoint2,
        method: 'post',
        data: params,
    });
```

### Example: RF Picking API Service

```javascript
import request from '@/utils/request'

// API endpoints
const PageWMSRFOrderPick = '/api/WMSRFOrderPick/page';
const ScanOrderPickTask = '/api/WMSRFOrderPick/ScanRFOrderPick';
const ScanBoxNumberCompletePackage = '/api/WMSRFOrderPick/ScanBoxNumberCompletePackage';
const GetPickTaskDetailsByLocation = '/api/WMSRFOrderPick/GetPickTaskDetailsByLocation';

// Paginated query of pick tasks
export const pageWMSRFOrderPickApi = (params) =>
    request({
        url: PageWMSRFOrderPick,
        method: 'post',
        data: params,
    });

// Scan to pick
export const scanOrderPickTaskApi = (params) =>
    request({
        url: ScanOrderPickTask,
        method: 'post',
        data: params,
    });

// Scan box number to complete packaging
export const scanBoxNumberCompletePackageApi = (params) =>
    request({
        url: ScanBoxNumberCompletePackage,
        method: 'post',
        data: params,
    });

// Get pick details sorted by location
export const getPickTaskDetailsByLocationApi = (params) =>
    request({
        url: GetPickTaskDetailsByLocation,
        method: 'post',
        data: params,
    });
```

## Request Wrapper (utils/request.js)

The request wrapper handles:
- Token authentication
- Request interceptors
- Response interceptors
- Error handling
- Duplicate submission prevention

### Key Features

```javascript
// Token management
export const accessTokenKey = 'access-token';
export const getToken = () => uni.getStorageSync(accessTokenKey);

// Request interceptor - adds token
service.interceptors.request.use((config) => {
    const accessToken = uni.getStorageSync(accessTokenKey);
    if (accessToken) {
        config.header['Authorization'] = `Bearer ${accessToken}`;
    }
    return config;
});

// Response interceptor - handles errors
service.interceptors.response.use((res) => {
    // Handle 401
    if (res.status === 401 || (res.data && res.data.code === 401)) {
        clearAccessTokens();
    }
    // Handle business errors
    if (res.data && res.data.code !== 200) {
        uni.showToast({
            title: res.data.message || '操作失败',
            icon: 'none'
        });
        return Promise.reject(new Error(res.data.message));
    }
    return res;
});
```

## Audio Feedback

### Import Audio Functions

```javascript
import { playSuccessSound, playErrorSound } from "@/services/common/playaudio.js";
```

### Use Audio Feedback

```javascript
// Success
if (result.code === "1") {
    playSuccessSound();
    uni.showToast({ title: "操作成功", icon: 'success' });
} else {
    // Error
    playErrorSound();
    uni.showToast({ title: "操作失败", icon: 'none' });
}
```

### Audio Files Location

- Success sound: `static/success.mp3`
- Error sound: `static/error.mp3`

## Barcode Scanning

### Auto-Focus Input Pattern

```vue
<template>
    <view class="cu-form-group">
        <input v-model="scanInput" 
               :focus="focusflag"
               @confirm="handleScan"
               placeholder="请扫描条码"/>
    </view>
</template>

<script>
export default {
    data() {
        return {
            focusflag: true,
            scanInput: ''
        };
    },
    methods: {
        async handleScan() {
            // Process scan
            let res = await scanApi({ scanInput: this.scanInput });
            
            // Clear and refocus
            this.scanInput = '';
            this.focusflag = false;
            this.$nextTick(() => { this.focusflag = true; });
        }
    }
}
</script>
```

### Key Points

1. **Auto-focus:** Use `:focus="focusflag"` and toggle it with `$nextTick()`
2. **Clear input:** Clear `scanInput` after processing
3. **Audio feedback:** Always provide audio feedback for scan success/failure
4. **Error handling:** Show toast message for errors

## ColorUI Components

### Common ColorUI Classes

**Background Colors:**
- `bg-blue` - Blue background
- `bg-green` - Green background
- `bg-orange` - Orange background
- `bg-grey` - Grey background
- `bg-red` - Red background
- `bg-gradual-blue` - Blue gradient

**Text Colors:**
- `text-blue` - Blue text
- `text-green` - Green text
- `text-orange` - Orange text
- `text-grey` - Grey text

**Text Sizes:**
- `text-xs` - Extra small
- `text-sm` - Small
- `text-df` - Default
- `text-lg` - Large
- `text-xl` - Extra large
- `text-sl` - Super large

**Font Weights:**
- `text-bold` - Bold
- `text-center` - Center aligned

**Components:**
- `cu-btn` - Button
- `cu-card` - Card
- `cu-item` - List item
- `cu-form-group` - Form group
- `cu-avatar` - Avatar/badge
- `cu-bar` - Bar/header

### Custom Navigation Bar

```vue
<cu-custom bgColor="bg-gradual-blue" :isBack="true">
    <block slot="content">Page Title</block>
    <block slot="right">
        <button class="cu-btn bg-blue">Button</button>
    </block>
</cu-custom>
```

## Page Navigation

### uni.navigateTo - Open new page

```javascript
uni.navigateTo({
    url: '/pages/wMSRFOrderPick/component/editDialog?pickTaskNumber=' + taskNumber + '&id=' + id
});
```

### uni.navigateBack - Return to previous page

```javascript
// Return to previous page
uni.navigateBack();

// Return with parameters
uni.navigateBack({
    delta: 1
});

// Return after delay
setTimeout(() => {
    uni.navigateBack();
}, 1500);
```

### uni.redirectTo - Replace current page

```javascript
uni.redirectTo({
    url: '/pages/login/index'
});
```

## Toast Messages

```javascript
// Success
uni.showToast({
    title: "操作成功",
    icon: 'success'
});

// Error
uni.showToast({
    title: "操作失败",
    icon: 'none'
});

// Custom icon
uni.showToast({
    title: "处理中...",
    icon: 'loading'
});

// Hide toast
uni.hideToast();
```

## Pull-to-Refresh and Infinite Scroll

### Using you-scroll Component

```vue
<template>
    <you-scroll :height="'100vh'" 
                 @onPullDown="refreshData"
                 @onReachBottom="loadMore">
        <view v-for="item in list" :key="item.id">
            {{item.name}}
        </view>
    </you-scroll>
</template>

<script>
export default {
    methods: {
        refreshData(done) {
            this.getList();
            done();  // Call done() to end refresh
        },
        loadMore() {
            this.page++;
            this.getList();
        }
    }
}
</script>
```

## RF-Backend Interaction Flow

```
[RF Mobile]                    [Backend API]
    |                              |
    |-- 1. Query tasks --------->   |
    |   POST /api/WMSRFOrderPick/page |
    |<-- Return task list ---------  |
    |                              |
    |-- 2. Click pick button --->   |
    |   Navigate to detail page    |
    |                              |
    |-- 3. Get pick details --->   |
    |   POST /GetPickTaskDetails   |
    |<-- Return sorted details ----  |
    |                              |
    |-- 4. Scan SKU/Location -->    |
    |   POST /ScanRFOrderPick      |
    |   {                         |
    |     pickTaskNumber: "...",  |
    |     scanInput: "..."        |
    |   }                         |
    |<-- Return updated details ---  |
    |   - Play success.mp3         |
    |   - Play error.mp3           |
    |   - Auto refresh list        |
    |                              |
    |-- 5. Scan box number ----->   |
    |   POST /ScanBoxNumber...     |
    |   {                         |
    |     pickTaskNumber: "...",  |
    |     boxNumber: "..."         |
    |   }                         |
    |<-- Return packaging result --  |
    |   - Navigate back if success |
```

## Key Development Patterns

### 1. State Management Pattern

```javascript
data() {
    return {
        form: {
            id: '',
            name: '',
            scanInput: ''
        },
        list: [],
        focusflag: true
    };
}
```

### 2. API Call Pattern

```javascript
async methodName() {
    await apiFunction(this.form).then((res) => {
        if (res.data.result) {
            this.list = res.data.result.data || [];
            playSuccessSound();
        } else {
            playErrorSound();
            uni.showToast({ title: "操作失败", icon: 'none' });
        }
    }).catch((err) => {
        playErrorSound();
        uni.showToast({ title: "网络错误", icon: 'none' });
    });
}
```

### 3. Scan Handler Pattern

```javascript
async handleScan() {
    let res = await scanApi(this.form);
    
    if (res.data.result.code == "1") {
        playSuccessSound();
        await this.refreshData();
    } else {
        playErrorSound();
        uni.showToast({ title: res.data.result.msg, icon: 'none' });
    }
    
    // Reset input
    this.scanInput = '';
    this.focusflag = false;
    this.$nextTick(() => { this.focusflag = true; });
}
```

### 4. Page Parameter Pattern

```javascript
onLoad(options) {
    this.form.id = options.id || '';
    this.form.name = options.name || '';
}
```

## Best Practices

1. **Always provide audio feedback** for scan operations
2. **Use auto-focus** for scan inputs with focus toggle
3. **Clear input** after processing scans
4. **Show toast messages** for user feedback
5. **Handle errors** with try-catch or .catch()
6. **Use v-if** for conditional rendering of empty states
7. **Use ColorUI classes** for consistent styling
8. **Navigate properly** using uni APIs
9. **Manage state** in data() with clear structure
10. **Call async methods** with async/await

## Common Issues and Solutions

### Input not auto-focusing

```javascript
// Solution: Toggle focus flag with nextTick
this.focusflag = false;
this.$nextTick(() => { this.focusflag = true; });
```

### List not updating after API call

```javascript
// Solution: Use reactive assignment
this.list = res.data.result.data || [];  // Use =, not push()

// Or use Vue.set for reactivity
this.$set(this.list, index, newValue);
```

### Toast not showing

```javascript
// Solution: Check icon parameter
uni.showToast({ 
    title: "message",
    icon: 'none'  // 'success', 'loading', 'none'
});
```

## Key Takeaways

1. **uni-app framework** for cross-platform mobile development
2. **ColorUI** provides beautiful, consistent UI components
3. **Audio feedback** (success.mp3, error.mp3) for operations
4. **Auto-focus pattern** for barcode scanning
5. **API service layer** in `services/` directory
6. **Request wrapper** handles authentication and errors
7. **Component reusability** with `you-scroll`, `t-table`
8. **Consistent patterns** for scanning, API calls, and navigation
