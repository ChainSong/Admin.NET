<template>
    <div class="common-layout">
        <el-card shadow="always" :body-style="{ paddingBottom: '0' }">
            <el-form :inline="true" :model="formData" class="demo-form-inline">
                <el-row :gutter="10" :span="24">
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="客户">
                            <el-select v-model="formData.customerId" placeholder="请选择客户名称" clearable filterable
                                style="width: 200px" @visible-change="onCustomerSelectVisibleChange">
                                <el-option v-for="item in customerSelectList" :key="item.id" :label="item.text"
                                    :value="item.value" />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="仓库">
                            <el-select v-model="formData.warehouseId" placeholder="请选择仓库名称" clearable filterable
                                style="width: 200px" @visible-change="onWarehouseSelectVisibleChange">
                                <el-option v-for="item in warehouseSelectList" :key="item.id" :label="item.text"
                                    :value="item.value" />
                            </el-select>
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="出库单号">
                            <el-input v-model="formData.orderNumber" placeholder="请输入出库单号" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="外部单号">
                            <el-input v-model="formData.externOrderNumber" placeholder="请输入外部单号" clearable />
                        </el-form-item>
                    </el-col>
                </el-row>

                <el-row :gutter="10" :span="24">
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="预出库单号">
                            <el-input v-model="formData.preOrderNumber" placeholder="请输入预出库单号" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="类型">
                            <el-input v-model="formData.type" placeholder="请输入类型" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="SKU">
                            <el-input v-model="formData.sku" placeholder="请输入SKU" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="Lot">
                            <el-input v-model="formData.lot" placeholder="请输入Lot" clearable />
                        </el-form-item>
                    </el-col>
                </el-row>


                <el-row :gutter="10" :span="24">
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">
                        <el-form-item label="SN">
                            <el-input v-model="formData.sn" placeholder="请输入SN" clearable />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="12" :sm="6" :md="6" :lg="6" :xl="6">

                    </el-col>
                    <el-col :xs="12" :sm="12" :md="12" :lg="12" :xl="12">
                        <el-button type="primary" @click="handleQuery">查询</el-button>
                        <el-button type="primary" @click="handleExport">导出</el-button>
                        <el-button @click="handleReset">清空</el-button>
                    </el-col>
                </el-row>
            </el-form>
        </el-card>
        <el-card class="full-table" shadow="hover" style="margin-top: 8px">
            <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light">
                <el-table-column prop="preOrderNumber" label="预出库单号" width="180"  align="center"/>
                <el-table-column prop="orderNumber" label="出库单号" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="externOrderNumber" label="出库单号" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="customerName" label="客户名称" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="warehouseName" label="仓库名称" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="type" label="类型" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="sku" label="SKU" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="lot" label="Lot号" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="sn" label="SN码" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="qty" label="数量" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="creator" label="创建用户" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="creationTime" label="创建时间" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="updator" label="修改用户" show-overflow-tooltip="" width="180"  align="center"/>
                <el-table-column prop="updateTime" label="修改时间" show-overflow-tooltip="" width="180"  align="center"/>
            </el-table>
            <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
                :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
                @size-change="handleSizeChange" @current-change="handleCurrentChange"
                layout="total, sizes, prev, pager, next, jumper" />
        </el-card>
    </div>
</template>
<script lang="ts" setup>
import { ref, onMounted, onBeforeUnmount, watch } from 'vue';
import { ElMessage } from 'element-plus';

import { selectCustomer } from '/@/api/main/wMSCustomer';
import { selectWarehouse } from '/@/api/main/wMSWarehouse';
import { getPackageSNPageList, exportPackageSN} from '/@/api/main/wMSReport';
import { downloadByData, getFileName } from '/@/utils/download';

// ================== 表单数据 ==================
const formData = ref<any>({
    customerId: undefined,
    warehouseId: undefined,
    orderNumber: '',
    externOrderNumber: '',
    preOrderNumber: '',
    type: '',
    sku: '',
    lot: '',
    sn: ''
});
// 清空查询条件
const handleReset = () => {
    formData.value = {
        customerId: undefined,
        warehouseId: undefined,
        orderNumber: '',
        externOrderNumber: '',
        preOrderNumber: '',
        type: '',
        sku: '',
        lot: '',
        sn: ''
    };
    // 回到第一页
    tableParams.value.page = 1;

    handleQuery();
};
// ================== 下拉数据 ==================
const customerSelectList = ref<any[]>([]);
const warehouseSelectList = ref<any[]>([]);

// ================== 表格数据 ==================
const loading = ref(false);
const tableData = ref<any[]>([]);
const tableParams = ref({
    page: 1,
    pageSize: 10,
    total: 0,
});

// ================== 防止并发请求（可选） ==================
let requestId = 0;

// ================== 获取客户下拉 ==================
const getCustomerSelectList = async () => {
    try {
        const res = await selectCustomer({ "inputData": "" });
        customerSelectList.value = res.data.result ?? [];
    } catch (err) {
        ElMessage.error('加载客户列表失败');
    }
};

// ================== 获取仓库下拉 ==================
const getWarehouseSelectList = async () => {
    try {
        const res = await selectWarehouse({ "inputData": "" });
        warehouseSelectList.value = res.data.result ?? [];
    } catch (err) {
        ElMessage.error('加载仓库列表失败');
    }
};

// ================== 客户下拉展开触发 ==================
const onCustomerSelectVisibleChange = (visible: boolean) => {
    if (visible && customerSelectList.value.length === 0) {
        getCustomerSelectList();
    }
};

// ================== 仓库下拉展开触发==================
const onWarehouseSelectVisibleChange = (visible: boolean) => {
    if (visible && warehouseSelectList.value.length === 0) {
        getWarehouseSelectList();
    }
};

// ================== 查询（核心） ==================
const handleQuery = async () => {
    const current = ++requestId;
    loading.value = true;

    try {
        const reqParams = Object.assign({}, formData.value, tableParams.value);
        const res = await getPackageSNPageList(reqParams);

        // 防止旧请求覆盖新请求
        if (current !== requestId) return;

        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total ?? 0;
    } catch (err) {
        ElMessage.error('查询失败，请稍后重试');
    } finally {
        if (current === requestId) loading.value = false;
    }
};

// ================== 改变每页条数 ==================
const handleSizeChange = (val: number) => {
    tableParams.value.pageSize = val;
    tableParams.value.page = 1; //  改页容量时回到第一页
    handleQuery();
};

// ================== 改变页码 ==================
const handleCurrentChange = (val: number) => {
    tableParams.value.page = val;
    handleQuery();
};

// ================== 导出 ==================
const handleExport = async () => {
    try {
        loading.value = true;
        const reqParams = Object.assign({}, formData.value, tableParams.value);
        const res = await exportPackageSN(reqParams);

        const fileName = getFileName(res.headers);
        downloadByData(res.data as any, fileName);
    } catch (err) {
        ElMessage.error('导出失败');
    } finally {
        loading.value = false;
    }
};

// ================== 生命周期：页面初始化 ==================
onMounted(async () => {
    // 页面加载时先拉下拉数据（可选：不提前加载，等展开触发）
    await Promise.all([
        getCustomerSelectList(),
        getWarehouseSelectList(),
    ]);

    // 页面默认查询一次
    handleQuery();
});

// ================== watch：查询条件变化回到第一页 ==================
watch(
    () => ({
        ...formData.value
    }),
    () => {
        tableParams.value.page = 1;
    },
    { deep: true }
);

// ================== 生命周期：离开页面 ==================
onBeforeUnmount(() => {
    // 清理状态，避免残留（可选）
    requestId++;
    loading.value = false;
    tableData.value = [];
});
</script>


<style scoped></style>