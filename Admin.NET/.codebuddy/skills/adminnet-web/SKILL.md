---
name: adminnet-web
description: 此技能提供了在Admin.NET平台中使用Vue 3、TypeScript和Element Plus开发Web前端应用的专业知识，包括页面开发、API集成、组件使用和状态管理。
---

# Admin.NET Web前端开发技能

## 技能目的

此技能为在Admin.NET平台中使用Vue 3、TypeScript和Element Plus开发Web前端应用提供全面指导。它涵盖了页面开发、API服务集成、UI组件、状态管理、路由配置、权限控制和常用开发模式。

## 何时使用此技能

在以下情况使用此技能：
- 开发新的Web前端页面（WMS管理、系统管理、业务模块）
- 集成前端页面与后端API
- 使用Element Plus组件库
- 实现权限控制和指令
- 创建或修改API服务
- 使用Pinia进行状态管理
- 配置路由和导航
- 开发表单、表格等常用组件
- 排查前端相关问题

## Web项目结构

```
Web/
├── src/                        # 源代码目录
│   ├── api-services/          # API服务层（自动生成）
│   │   ├── api.ts             # API入口
│   │   ├── base.ts            # 基础配置
│   │   ├── configuration.ts   # API配置
│   │   └── apis/              # 具体API模块
│   │       ├── wms-product-api.ts
│   │       ├── wms-order-api.ts
│   │       ├── sys-user-api.ts
│   │       └── ...
│   │
│   ├── api/                   # 自定义API层
│   │   └── module/           # 按模块分组
│   │
│   ├── views/                 # 页面视图
│   │   ├── login/            # 登录页
│   │   ├── home/             # 首页
│   │   ├── main/             # 主要业务模块
│   │   │   ├── wMSPickTask/  # WMS拣货任务
│   │   │   ├── wMSOrder/     # WMS订单
│   │   │   ├── wMSProduct/   # WMS商品
│   │   │   └── ...
│   │   ├── system/           # 系统管理
│   │   ├── personal/         # 个人中心
│   │   └── error/            # 错误页面
│   │
│   ├── components/           # 公共组件
│   │   ├── auth/             # 权限组件
│   │   ├── basic/            # 基础组件
│   │   ├── table/            # 表格组件
│   │   ├── editor/           # 编辑器组件
│   │   ├── flow/             # 流程组件
│   │   └── workflow/         # 工作流组件
│   │
│   ├── router/               # 路由配置
│   │   ├── index.ts          # 路由入口
│   │   ├── backEnd.ts        # 后端路由
│   │   ├── frontEnd.ts       # 前端路由
│   │   └── route.ts          # 路由工具
│   │
│   ├── stores/               # Pinia状态管理
│   │   ├── userInfo.ts       # 用户信息
│   │   ├── routesList.ts     # 路由列表
│   │   ├── tagsViewRoutes.ts # 标签页路由
│   │   ├── themeConfig.ts   # 主题配置
│   │   └── keepAliveNames.ts # 缓存页面
│   │
│   ├── layout/               # 布局组件
│   │   └── index.vue         # 主布局
│   │
│   ├── entities/             # 实体类型定义
│   │   ├── wms-product.ts
│   │   ├── wms-order.ts
│   │   ├── page-request.ts
│   │   └── ...
│   │
│   ├── directive/            # 自定义指令
│   │   ├── authDirective.ts  # 权限指令
│   │   ├── customDirective.ts # 自定义指令
│   │   └── index.ts
│   │
│   ├── i18n/                 # 国际化
│   │   └── index.ts
│   │
│   ├── theme/                # 主题样式
│   │   ├── element.scss      # Element Plus主题
│   │   ├── app.scss          # 应用样式
│   │   ├── dark.scss         # 暗黑主题
│   │   └── loading.scss      # 加载样式
│   │
│   ├── assets/               # 静态资源
│   │   ├── logo.png
│   │   └── images/
│   │
│   ├── App.vue               # 应用根组件
│   └── main.ts               # 应用入口
│
├── public/                   # 公共静态资源
│   ├── favicon.ico
│   ├── images/
│   ├── audio/
│   └── logo/
│
├── api_build/               # API构建脚本
│   ├── build.bat
│   ├── build.sh
│   └── readme.md
│
├── dist/                    # 构建输出
├── .env                     # 环境变量
├── .env.development
├── .env.production
├── package.json             # 项目配置
├── tsconfig.json            # TypeScript配置
├── vite.config.ts          # Vite配置
└── index.html               # HTML入口
```

## 技术栈

### 核心框架
- **Vue 3.3+** - 渐进式JavaScript框架
- **TypeScript 5.2+** - JavaScript超集，提供类型安全
- **Vite 4.4+** - 新一代前端构建工具
- **Vue Router 4.2+** - Vue官方路由
- **Pinia 2.1+** - Vue官方状态管理

### UI框架
- **Element Plus 2.3+** - Vue 3组件库
- **@element-plus/icons-vue 2.1+** - Element Plus图标库

### 常用依赖
- **Axios 1.5+** - HTTP客户端
- **js-cookie 3.0+** - Cookie操作
- **lodash-es 4.17+** - JavaScript工具库
- **echarts 5.4+** - 图表库
- **@wangeditor/editor 5.1+** - 富文本编辑器
- **cropperjs 1.6+** - 图片裁剪
- **qrcodejs2-fixes 0.0.2** - 二维码生成
- **print-js 1.6+** - 打印功能
- **sortablejs 1.15+** - 拖拽排序
- **nprogress 0.2+** - 进度条
- **@microsoft/signalr 7.0+** - 实时通信

## 页面开发模板

### 标准页面结构

```vue
<template>
  <div class="page-container">
    <!-- 查询表单 -->
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.queryParams" ref="queryFormRef" :inline="true">
        <el-row :gutter="[16, 15]">
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
            <el-form-item class="mb-0" label="名称">
              <el-input
                v-model="state.queryParams.name"
                placeholder="请输入名称"
                clearable
              />
            </el-form-item>
          </el-col>
          <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
            <el-form-item class="mb-0" label="状态">
              <el-select
                v-model="state.queryParams.status"
                placeholder="请选择状态"
                clearable
              >
                <el-option label="启用" :value="1" />
                <el-option label="禁用" :value="0" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-form-item>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Search"
              @click="handleQuery"
              v-auth="'module:page'"
            >
              查询
            </el-button>
            <el-button icon="ele-Refresh" @click="resetQuery">
              重置
            </el-button>
          </el-button-group>
          <el-button-group>
            <el-button
              type="primary"
              icon="ele-Plus"
              @click="openAdd"
              v-auth="'module:add'"
            >
              新增
            </el-button>
          </el-button-group>
        </el-form-item>
      </el-form>
    </el-card>

    <!-- 数据表格 -->
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table
        :data="state.tableData"
        v-loading="state.loading"
        border
        stripe
        height="100%"
      >
        <el-table-column type="index" label="序号" width="60" />
        <el-table-column prop="name" label="名称" min-width="120" show-overflow-tooltip />
        <el-table-column prop="code" label="编码" min-width="100" />
        <el-table-column prop="status" label="状态" width="80">
          <template #default="{ row }">
            <el-tag v-if="row.status === 1" type="success">启用</el-tag>
            <el-tag v-else type="danger">禁用</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="createTime" label="创建时间" width="160" />
        <el-table-column label="操作" width="200" fixed="right">
          <template #default="{ row }">
            <el-button
              icon="ele-Edit"
              size="small"
              text
              type="primary"
              @click="openEdit(row)"
              v-auth="'module:edit'"
            >
              编辑
            </el-button>
            <el-button
              icon="ele-Delete"
              size="small"
              text
              type="danger"
              @click="handleDelete(row)"
              v-auth="'module:delete'"
            >
              删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>

      <!-- 分页 -->
      <el-pagination
        v-model:current-page="state.tableParams.page"
        v-model:page-size="state.tableParams.pageSize"
        :total="state.tableParams.total"
        :page-sizes="[10, 20, 50, 100]"
        small
        background
        layout="total, sizes, prev, pager, next, jumper"
        @size-change="handleQuery"
        @current-change="handleQuery"
      />
    </el-card>

    <!-- 新增/编辑弹窗 -->
    <el-dialog
      v-model="state.dialogVisible"
      :title="state.dialogTitle"
      width="800px"
      destroy-on-close
    >
      <el-form
        :model="state.form"
        :rules="state.rules"
        ref="formRef"
        label-width="100px"
      >
        <el-form-item label="名称" prop="name">
          <el-input v-model="state.form.name" placeholder="请输入名称" />
        </el-form-item>
        <el-form-item label="编码" prop="code">
          <el-input v-model="state.form.code" placeholder="请输入编码" />
        </el-form-item>
        <el-form-item label="状态" prop="status">
          <el-radio-group v-model="state.form.status">
            <el-radio :label="1">启用</el-radio>
            <el-radio :label="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="state.dialogVisible = false">取消</el-button>
          <el-button type="primary" @click="handleSubmit">确定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script setup lang="ts" name="moduleName">
import { reactive, ref, onMounted } from 'vue';
import { ElMessage, ElMessageBox } from 'element-plus';
import { ModuleApi } from '/@/api-services/apis/module-api';

// 定义响应式状态
const state = reactive({
  loading: false,
  tableData: [] as any[],
  queryParams: {
    name: '',
    status: undefined,
  },
  tableParams: {
    page: 1,
    pageSize: 20,
    total: 0,
  },
  dialogVisible: false,
  dialogTitle: '',
  form: {
    id: 0,
    name: '',
    code: '',
    status: 1,
  },
  rules: {
    name: [{ required: true, message: '请输入名称', trigger: 'blur' }],
    code: [{ required: true, message: '请输入编码', trigger: 'blur' }],
  },
});

// 引用
const queryFormRef = ref();
const formRef = ref();

// 查询
const handleQuery = async () => {
  state.loading = true;
  try {
    const res = await ModuleApi.apiModulePage({
      ...state.queryParams,
      page: state.tableParams.page,
      pageSize: state.tableParams.pageSize,
    });
    state.tableData = res.data.result?.items ?? [];
    state.tableParams.total = res.data.result?.total ?? 0;
  } finally {
    state.loading = false;
  }
};

// 重置
const resetQuery = () => {
  state.queryParams = {
    name: '',
    status: undefined,
  };
  handleQuery();
};

// 新增
const openAdd = () => {
  state.dialogTitle = '新增';
  state.form = {
    id: 0,
    name: '',
    code: '',
    status: 1,
  };
  state.dialogVisible = true;
};

// 编辑
const openEdit = (row: any) => {
  state.dialogTitle = '编辑';
  state.form = { ...row };
  state.dialogVisible = true;
};

// 提交
const handleSubmit = async () => {
  await formRef.value.validate();
  if (state.form.id === 0) {
    await ModuleApi.apiModuleAdd(state.form);
  } else {
    await ModuleApi.apiModuleUpdate(state.form);
  }
  ElMessage.success('操作成功');
  state.dialogVisible = false;
  handleQuery();
};

// 删除
const handleDelete = (row: any) => {
  ElMessageBox.confirm('确定要删除该数据吗?', '提示', {
    confirmButtonText: '确定',
    cancelButtonText: '取消',
    type: 'warning',
  }).then(async () => {
    await ModuleApi.apiModuleDelete({ id: row.id });
    ElMessage.success('删除成功');
    handleQuery();
  });
};

// 初始化
onMounted(() => {
  handleQuery();
});
</script>

<style scoped lang="scss">
.page-container {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.full-table {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.full-table :deep(.el-card__body) {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.full-table :deep(.el-table) {
  flex: 1;
}

.el-pagination {
  margin-top: 10px;
}
</style>
```

## API服务开发

### API服务结构

API服务位于 `src/api-services/apis/` 目录，由Swagger自动生成。

### 使用API服务

```typescript
import { ModuleApi } from '/@/api-services/apis/module-api';
import { ProductApi } from '/@/api-services/apis/wms-product-api';
import { UserApi } from '/@/api-services/apis/sys-user-api';

// 分页查询
const pageData = await ModuleApi.apiModulePage({
  page: 1,
  pageSize: 20,
  name: '测试',
});

// 新增
await ModuleApi.apiModuleAdd({
  name: '测试',
  code: 'TEST',
  status: 1,
});

// 更新
await ModuleApi.apiModuleUpdate({
  id: 1,
  name: '测试更新',
});

// 删除
await ModuleApi.apiModuleDelete({ id: 1 });

// 详情
const detail = await ModuleApi.apiModuleDetail({ id: 1 });

// 批量操作
await ModuleApi.apiModuleBatchDelete({ ids: [1, 2, 3] });
```

### 自定义API服务

如需添加自定义API，在 `src/api/module/` 下创建：

```typescript
import request from '/@/api-services/base';

// 定义API端点
const MODULE_API = '/api/Module';

// 分页查询
export const pageModule = (params: any) => {
  return request({
    url: `${MODULE_API}/page`,
    method: 'post',
    data: params,
  });
};

// 新增
export const addModule = (params: any) => {
  return request({
    url: `${MODULE_API}/add`,
    method: 'post',
    data: params,
  });
};

// 更新
export const updateModule = (params: any) => {
  return request({
    url: `${MODULE_API}/update`,
    method: 'post',
    data: params,
  });
};

// 删除
export const deleteModule = (id: number) => {
  return request({
    url: `${MODULE_API}/delete`,
    method: 'post',
    data: { id },
  });
};
```

## 权限控制

### v-auth 指令

```vue
<template>
  <!-- 按钮级别权限 -->
  <el-button
    type="primary"
    icon="ele-Plus"
    @click="handleAdd"
    v-auth="'module:add'"
  >
    新增
  </el-button>

  <!-- 多个权限，满足一个即可 -->
  <el-button
    v-auth="'module:edit,module:delete'"
  >
    操作
  </el-button>
</template>
```

### 自定义权限指令

```typescript
// src/directive/authDirective.ts
import type { Directive, DirectiveBinding } from 'vue';
import { useUserInfo } from '/@/stores/userInfo';

/**
 * 权限指令
 * 使用方式：v-auth="'user:add'"
 */
export const auth: Directive = {
  mounted(el: HTMLElement, binding: DirectiveBinding) {
    const { userInfo } = useUserInfo();
    const permissions = userInfo.permissions || [];

    if (binding.value) {
      const hasPermission = permissions.some((p: string) =>
        binding.value.includes(p)
      );

      if (!hasPermission) {
        el.parentNode?.removeChild(el);
      }
    }
  },
};
```

## 状态管理

### Pinia Store

```typescript
// src/stores/userInfo.ts
import { defineStore } from 'pinia';

interface UserInfoState {
  userId: number;
  userName: string;
  token: string;
  permissions: string[];
}

export const useUserInfo = defineStore('userInfo', {
  state: (): UserInfoState => ({
    userId: 0,
    userName: '',
    token: '',
    permissions: [],
  }),

  getters: {
    // 是否已登录
    isLogin: (state) => !!state.token,

    // 是否有指定权限
    hasPermission: (state) => (permission: string) => {
      return state.permissions.includes(permission);
    },
  },

  actions: {
    // 设置用户信息
    setUserInfo(data: Partial<UserInfoState>) {
      Object.assign(this, data);
    },

    // 清除用户信息
    clearUserInfo() {
      this.userId = 0;
      this.userName = '';
      this.token = '';
      this.permissions = [];
    },
  },
});
```

### 使用Store

```vue
<script setup lang="ts">
import { useUserInfo } from '/@/stores/userInfo';
import { useThemeConfig } from '/@/stores/themeConfig';

const userInfoStore = useUserInfo();
const themeConfig = useThemeConfig();

// 获取状态
console.log(userInfoStore.userName);
console.log(themeConfig.isDark);

// 调用方法
userInfoStore.setUserInfo({
  userName: 'admin',
  token: 'xxx',
});

// 使用getter
if (userInfoStore.isLogin) {
  console.log('已登录');
}
</script>
```

## 路由配置

### 动态路由

```typescript
// src/router/backEnd.ts
import { RouteRecordRaw } from 'vue-router';

/**
 * 后端路由（动态加载）
 */
export const backEndRoutes: RouteRecordRaw[] = [
  {
    path: '/main',
    name: 'main',
    component: () => import('/@/layout/index.vue'),
    redirect: '/home',
    meta: {
      title: '首页',
      isAffix: true,
    },
    children: [
      {
        path: '/home',
        name: 'home',
        component: () => import('/@/views/home/index.vue'),
        meta: {
          title: '首页',
          isAffix: true,
        },
      },
    ],
  },
];
```

### 静态路由

```typescript
// src/router/frontEnd.ts
import { RouteRecordRaw } from 'vue-router';

/**
 * 前端路由（静态）
 */
export const frontEndRoutes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'login',
    component: () => import('/@/views/login/index.vue'),
    meta: {
      title: '登录',
    },
  },
  {
    path: '/404',
    name: '404',
    component: () => import('/@/views/error/404.vue'),
    meta: {
      title: '404',
    },
  },
];
```

## Element Plus常用组件

### 表单组件

```vue
<template>
  <!-- 输入框 -->
  <el-input v-model="form.name" placeholder="请输入名称" clearable />

  <!-- 下拉选择 -->
  <el-select v-model="form.status" placeholder="请选择" clearable>
    <el-option label="启用" :value="1" />
    <el-option label="禁用" :value="0" />
  </el-select>

  <!-- 远程搜索 -->
  <el-select
    v-model="form.customerId"
    filterable
    remote
    reserve-keyword
    placeholder="请搜索客户"
    :remote-method="remoteSearchCustomer"
    :loading="loading"
  >
    <el-option
      v-for="item in customerList"
      :key="item.id"
      :label="item.name"
      :value="item.id"
    />
  </el-select>

  <!-- 日期选择 -->
  <el-date-picker
    v-model="form.date"
    type="date"
    placeholder="选择日期"
  />

  <!-- 日期范围 -->
  <el-date-picker
    v-model="form.dateRange"
    type="daterange"
    range-separator="~"
    start-placeholder="开始日期"
    end-placeholder="结束日期"
  />

  <!-- 单选 -->
  <el-radio-group v-model="form.status">
    <el-radio :label="1">启用</el-radio>
    <el-radio :label="0">禁用</el-radio>
  </el-radio-group>

  <!-- 多选 -->
  <el-checkbox-group v-model="form.permissions">
    <el-checkbox :label="1">查看</el-checkbox>
    <el-checkbox :label="2">编辑</el-checkbox>
    <el-checkbox :label="3">删除</el-checkbox>
  </el-checkbox-group>
</template>
```

### 表格组件

```vue
<template>
  <el-table
    :data="tableData"
    v-loading="loading"
    border
    stripe
    height="100%"
    @selection-change="handleSelectionChange"
  >
    <!-- 多选 -->
    <el-table-column type="selection" width="55" />

    <!-- 序号 -->
    <el-table-column type="index" label="序号" width="60" />

    <!-- 普通列 -->
    <el-table-column
      prop="name"
      label="名称"
      min-width="120"
      show-overflow-tooltip
    />

    <!-- 自定义列 -->
    <el-table-column prop="status" label="状态" width="80">
      <template #default="{ row }">
        <el-tag v-if="row.status === 1" type="success">启用</el-tag>
        <el-tag v-else type="danger">禁用</el-tag>
      </template>
    </el-table-column>

    <!-- 操作列 -->
    <el-table-column label="操作" width="200" fixed="right">
      <template #default="{ row }">
        <el-button
          icon="ele-Edit"
          size="small"
          text
          type="primary"
          @click="handleEdit(row)"
        >
          编辑
        </el-button>
        <el-button
          icon="ele-Delete"
          size="small"
          text
          type="danger"
          @click="handleDelete(row)"
        >
          删除
        </el-button>
      </template>
    </el-table-column>
  </el-table>
</template>

<script setup lang="ts">
const handleSelectionChange = (selection: any[]) => {
  console.log('选中的行', selection);
};
</script>
```

### 弹窗组件

```vue
<template>
  <el-dialog
    v-model="dialogVisible"
    :title="dialogTitle"
    width="800px"
    destroy-on-close
    :close-on-click-modal="false"
  >
    <el-form
      :model="form"
      :rules="rules"
      ref="formRef"
      label-width="100px"
    >
      <!-- 表单项 -->
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <el-button @click="dialogVisible = false">取消</el-button>
        <el-button type="primary" @click="handleSubmit">确定</el-button>
      </span>
    </template>
  </el-dialog>
</template>
```

## 常用工具函数

### 消息提示

```typescript
import { ElMessage, ElMessageBox, ElNotification } from 'element-plus';

// 成功提示
ElMessage.success('操作成功');

// 错误提示
ElMessage.error('操作失败');

// 警告提示
ElMessage.warning('请先选择数据');

// 普通提示
ElMessage.info('提示信息');

// 确认框
ElMessageBox.confirm('确定要删除吗?', '提示', {
  confirmButtonText: '确定',
  cancelButtonText: '取消',
  type: 'warning',
}).then(() => {
  // 确认操作
}).catch(() => {
  // 取消操作
});

// 通知
ElNotification({
  title: '提示',
  message: '这是一条通知',
  type: 'success',
});
```

### 加载状态

```typescript
import { ElLoading } from 'element-plus';

// 显示加载
const loading = ElLoading.service({
  lock: true,
  text: '加载中...',
  background: 'rgba(0, 0, 0, 0.7)',
});

// 关闭加载
loading.close();
```

## 常用开发模式

### 1. 页面生命周期

```typescript
import { onMounted, onActivated, onDeactivated } from 'vue';

// 组件挂载
onMounted(() => {
  console.log('组件已挂载');
  // 初始化数据
});

// 组件激活（keep-alive）
onActivated(() => {
  console.log('组件已激活');
  // 刷新数据
});

// 组件停用（keep-alive）
onDeactivated(() => {
  console.log('组件已停用');
});
```

### 2. 响应式数据

```typescript
import { reactive, ref, computed } from 'vue';

// ref - 用于基本类型
const count = ref(0);
const loading = ref(false);

// reactive - 用于对象
const state = reactive({
  queryParams: {
    name: '',
    status: 0,
  },
  tableData: [],
});

// computed - 计算属性
const totalCount = computed(() => {
  return state.tableData.length;
});

// 修改值
count.value++;
loading.value = false;
state.queryParams.name = 'test';
```

### 3. 组件通信

```typescript
// 父传子 - props
// 子组件
defineProps<{
  title: string;
  data: any[];
}>();

// 子传父 - emit
const emit = defineEmits<{
  (e: 'update', value: any): void;
  (e: 'delete', id: number): void;
}>();

// 发送事件
emit('update', newValue);
emit('delete', id);

// 父组件使用
<child-component
  title="标题"
  :data="tableData"
  @update="handleUpdate"
  @delete="handleDelete"
/>
```

### 4. 表格排序和筛选

```vue
<template>
  <el-table
    :data="tableData"
    @sort-change="handleSortChange"
    @filter-change="handleFilterChange"
  >
    <el-table-column
      prop="name"
      label="名称"
      sortable="custom"
    />
    <el-table-column
      prop="status"
      label="状态"
      :filters="[
        { text: '启用', value: 1 },
        { text: '禁用', value: 0 }
      ]"
    />
  </el-table>
</template>

<script setup lang="ts">
const handleSortChange = ({ prop, order }: any) => {
  console.log('排序字段:', prop, '排序方式:', order);
  // 执行查询
};

const handleFilterChange = (filters: any) => {
  console.log('筛选条件:', filters);
  // 执行查询
};
</script>
```

## 环境配置

### .env.development

```bash
# 开发环境
VITE_APP_TITLE=Admin.NET
VITE_API_URL=http://localhost:5000/api
```

### .env.production

```bash
# 生产环境
VITE_APP_TITLE=Admin.NET
VITE_API_URL=https://api.example.com/api
```

### 使用环境变量

```typescript
const apiUrl = import.meta.env.VITE_API_URL;
const appTitle = import.meta.env.VITE_APP_TITLE;
```

## API自动生成

### 生成步骤

1. 启动后端项目
2. 访问Swagger文档：`http://localhost:5000/swagger`
3. 执行构建脚本：

```bash
# Windows
cd api_build
build.bat

# Linux/Mac
cd api_build
chmod +x build.sh
./build.sh
```

### 生成的API位置

生成的API文件位于 `src/api-services/apis/` 目录，文件名格式为 `{module}-api.ts`。

## 最佳实践

1. **命名规范**
   - 组件文件使用PascalCase：`UserList.vue`
   - 页面文件夹使用kebab-case：`user-list/`
   - 变量使用camelCase：`userName`
   - 常量使用UPPER_SNAKE_CASE：`API_URL`

2. **代码组织**
   - 单一职责：每个组件只做一件事
   - 组件复用：提取公共组件到components目录
   - 类型定义：使用TypeScript定义接口

3. **性能优化**
   - 合理使用v-if和v-show
   - 使用computed缓存计算结果
   - 长列表使用虚拟滚动
   - 懒加载路由组件

4. **代码风格**
   - 使用ESLint和Prettier
   - 遵循Vue 3 Composition API规范
   - 使用setup语法糖
   - 添加适当的注释

5. **错误处理**
   - API调用添加错误处理
   - 表单验证
   - 加载状态提示
   - 异常边界处理

## 常见问题和解决方案

### 1. 路由跳转问题

```typescript
import { useRouter } from 'vue-router';

const router = useRouter();

// 跳转页面
router.push('/home');

// 带参数跳转
router.push({
  path: '/user/detail',
  query: { id: 1 },
});

// 替换当前路由
router.replace('/home');

// 返回上一页
router.back();
```

### 2. 表格分页问题

```typescript
// 确保分页参数同步
const handleQuery = async () => {
  const res = await ModuleApi.apiModulePage({
    ...state.queryParams,
    page: state.tableParams.page,
    pageSize: state.tableParams.pageSize,
  });
  state.tableData = res.data.result?.items ?? [];
  state.tableParams.total = res.data.result?.total ?? 0;
};

// 页码或每页条数变化时重新查询
watch(
  () => [state.tableParams.page, state.tableParams.pageSize],
  () => {
    handleQuery();
  }
);
```

### 3. 弹窗关闭后清空表单

```vue
<el-dialog
  v-model="dialogVisible"
  destroy-on-close
  @closed="handleDialogClosed"
>
  <el-form :model="form" ref="formRef">
    <!-- 表单项 -->
  </el-form>
</el-dialog>

<script setup lang="ts">
const handleDialogClosed = () => {
  formRef.value?.resetFields();
  state.form = {};
};
</script>
```

## 关键要点

1. **Vue 3 Composition API**：使用setup语法糖，代码更简洁
2. **TypeScript**：提供类型安全，减少运行时错误
3. **Element Plus**：丰富的组件库，快速构建UI
4. **Pinia**：轻量级状态管理，替代Vuex
5. **Vue Router**：路由管理，支持动态路由
6. **Vite**：快速的开发构建工具
7. **API自动生成**：基于Swagger自动生成API服务
8. **权限控制**：v-auth指令实现细粒度权限控制
9. **响应式设计**：支持多端适配
10. **国际化**：支持多语言切换
