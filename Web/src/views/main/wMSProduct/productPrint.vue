<template>
    <div class="wMSProduct-container">
        <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
            <el-form :model="queryParams" ref="queryForm" :inline="true">
                <el-row :gutter="[16, 15]">
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="SKU">
                            <el-input v-model="state.header.sku" />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="数量">
                            <el-input v-model="state.header.qty" />
                        </el-form-item>
                    </el-col>
                      <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                           <el-form-item>
                    <el-button type="primary" icon="ele-Help" @click="openPrintJob" v-auth="'wMSProduct:add'"> 打印
                    </el-button>
                </el-form-item>
                    </el-col>
                </el-row>

              
            </el-form>
        </el-card>
<printDialog ref="printDialogRef" :title="ptintTitle" />
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
import printDialog from '/@/views/tools/printDialog.vue';
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
const printDialogRef = ref();
const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
const ptintTitle = ref("");
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

});



// 查询操作
// const handleQuery = async () => {
//     var res = await pageWMSProduct(Object.assign(state.value.header, tableParams.value));
// };


// =============================自定义功能============================================

// 打开打印询页面
const openPrintJob = async () => {
    ptintTitle.value = '打印SKU';
    // let ids = ref(Array<Number>);
    // let ids = new Array<Number>();
    let printData = {};
    printData.printTemplate = "打印SKU模板";
    if(state.value.header.sku=="" || state.value.header.sku==undefined){
        ElMessage.warning("请输入SKU");
        return;
    }
    console.log("打印SKU");
    console.log(state.value.header);
    var res = await pageWMSProduct(Object.assign(state.value.header, tableParams.value));
    if (res.data.result != null) {
        printData.data = [];
        for (let i = 0; i < state.value.header.qty; i++) {
            printData.data.push( res.data.result?.items[0]);
        }
    }
    // 判断有没有配置客户自定义打印模板
    printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });

};




</script>
