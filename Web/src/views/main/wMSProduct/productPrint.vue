<template>
    <div class="wMSProduct-container">
        <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
            <el-form :model="queryParams" ref="queryForm" :inline="true">
                <el-row :gutter="[16, 15]">
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="SKU">
                            <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="数量">
                            <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" />
                        </el-form-item>
                    </el-col>
                </el-row>

                <el-form-item>
                    <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSProduct:add'"> 打印
                    </el-button>
                </el-form-item>
            </el-form>
        </el-card>

    </div>
</template>

<script lang="ts" setup="" name="wMSProduct">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
import editDialog from '/@/views/main/wMSProduct/component/editDialog.vue'
import addDialog from '/@/views/main/wMSProduct/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSProduct/component/queryDialog.vue'
import selectRemote from '/@/views/tools/select-remote.vue'

// import { pageWMSProduct, deleteWMSProduct } from '/@/api/main/wMSProduct';
import { pageWMSProduct, deleteWMSProduct } from '/@/api/main/wMSProduct';
import { getByTableNameList } from "/@/api/main/tableColumns";

import Header from "/@/entities/Product";
// import Details from "/@/entities/ProductDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";


const state = ref({
    vm: {
        id: "",

        // form: {
        //     ProductDetails: []
        // } as any,
    },
    visible: false,
    loading: false,
    header: new Header(),
    headers: new Array<Header>(),
    // details: new Array<Details>(),
    // header: new Array<Details>(),

    tableColumnHeader: new TableColumns(),
    tableColumnHeaders: new Array<TableColumns>(),
    tableColumnDetail: new TableColumns(),
    tableColumnDetails: new Array<TableColumns>()

    // tableColumn: new TableColumns(),
    // tableColumns: new Array<TableColumns>(),
    // tableColumnsDetails: new Array<TableColumnsDetails>(),
    //   tableColumnsDetail = ref();
});

const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
// const tableData = ref<any>
// ([]);
const queryParams = ref<any>
    ({});
const tableParams = ref({
    page: 1,
    pageSize: 10,
    total: 0,
});
const editTitle = ref("");
const addTitle = ref("");
const queryTitle = ref("");

// 页面加载时
onMounted(async () => {
    gettableColumn();
});

const gettableColumn = async () => {
    let res = await getByTableNameList("WMS_Product");
    state.value.tableColumnHeaders = res.data.result;
};

// 查询操作
const handleQuery = async () => {
    loading.value = true;
    var res = await pageWMSProduct(Object.assign(state.value.header, tableParams.value));
    state.value.headers = res.data.result?.items ?? [];
    tableParams.value.total = res.data.result?.total;
    loading.value = false;
};

// 打开新增页面
const openAdd = () => {
    addTitle.value = '添加';
    addDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEdit = (row: any) => {
    editTitle.value = '编辑';
    editDialogRef.value.openDialog(row);
};
// 打开查询页面
const openQuery = (row: any) => {
    queryTitle.value = '查看';
    queryDialogRef.value.openDialog(row);
};

// 删除
const del = (row: any) => {
    ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
    })
        .then(async () => {
            await deleteWMSProduct(row);
            handleQuery();
            ElMessage.success("删除成功");
        })
        .catch(() => { });
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

// =============================自定义功能============================================


handleQuery();
</script>
