<template>
    <div class="wMSInventoryReport-container">
        <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
            <el-form :model="queryParams" ref="queryForm" :inline="true">

                <el-form-item label="SKU">
                    <el-input v-model="queryParams.sKU" clearable="" placeholder="请输入SKU" />
                </el-form-item>

                <el-form-item>
                    <el-button-group>
                        <el-button type="primary" icon="ele-Search" @click="handleQuery"
                            v-auth="'wMSInventoryReport:page'"> 查询 </el-button>
                        <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>

                          <el-button type="primary" icon="ele-Search" @click="handleExportData"
                           > 导出 </el-button>
                    </el-button-group>
                </el-form-item>

            </el-form>
        </el-card>
        <el-card class="full-table" shadow="hover" style="margin-top: 8px">
            <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id"
                border="">
                <el-table-column type="index" label="序号" width="55" fixed="" show-overflow-tooltip=""  />
                <el-table-column prop="sku" label="SKU" fixed="" show-overflow-tooltip="" />
                <el-table-column prop="goodsName" label="产品名称" fixed="" show-overflow-tooltip="" />
                <el-table-column prop="qty" label="数量" fixed="" show-overflow-tooltip="" />
                <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
                <el-table-column prop="warehouseName" label="仓库名称" fixed="" show-overflow-tooltip="" />
            </el-table>
            <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
                :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
                @size-change="handleSizeChange" @current-change="handleCurrentChange"
                layout="total, sizes, prev, pager, next, jumper" />
            <editDialog ref="editDialogRef" :title="editWMSInventoryReportTitle" @reloadTable="handleQuery" />
        </el-card>
    </div>
</template>

<script lang="ts" setup="" name="wMSInventoryReport">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';
import { downloadByData, getFileName } from '/@/utils/download';

import { pageWMSInventoryReport, availableInventorySummaryReportExport,availableInventorySummaryReport } from '/@/api/main/wMSInventoryReport';


const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>
    ([]);
const queryParams = ref<any>
    ({});
const tableParams = ref({
    page: 1,
    pageSize: 10,
    total: 0,
});
const editWMSInventoryReportTitle = ref("");


// 查询操作
const handleQuery = async () => {
    loading.value = true;
    console.log("res");
    queryParams.value.inventoryStatus="1";
    var res = await availableInventorySummaryReport(Object.assign(queryParams.value, tableParams.value));
    console.log(res);
    tableData.value = res.data.result?.items ?? [];
    tableParams.value.total = res.data.result?.total;
    loading.value = false;
};


// 改变页面容量
const handleSizeChange = (val: number) => {
    tableParams.value.pageSize = val;
    handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
    tableParams.value.page = val;
    handleQuery();
};
//导出
const handleExportData=async()=>{
   queryParams.value.inventoryStatus="1";
    var res = await availableInventorySummaryReportExport(Object.assign(queryParams.value, tableParams.value));
  var fileName = getFileName(res.headers);
  downloadByData(res.data as any, fileName);
}

handleQuery();
</script>
