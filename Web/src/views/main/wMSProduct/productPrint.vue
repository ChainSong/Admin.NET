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
                        <el-form-item class="mb-0" label="批次号">
                            <el-input v-model="state.header.batchNo" />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="过期日期">
                            <el-date-picker 
                                v-model="state.header.expiryDate" 
                                type="date" 
                                placeholder="选择日期"
                                value-format="YYYY-MM-DD"
                                style="width: 100%;" 
                            />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="数量">
                            <el-input v-model="state.header.qty" />
                        </el-form-item>
                    </el-col>
                    <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6">
                        <el-form-item class="mb-0" label="打印格式" style="width: 70% !important;">
                            <el-select v-model="state.header.printFormat" placeholder="选择格式" width="150">
                                <el-option label="普通" value="normal" style="width: 100%"></el-option>
                                <el-option label="Hach" value="hach" style="width: 100%"></el-option>
                                <el-option label="Json" value="json" style="width: 100%"></el-option>
                            </el-select>
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

// 格式化日期为指定格式 (例如: 01AP2026)
const formatDateForPrint = (dateString: string) => {
    if (!dateString) return "";
    const date = new Date(dateString);
    const day = date.getDate().toString().padStart(2, '0');
    const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    const month = months[date.getMonth()];
    const year = date.getFullYear().toString();
    return `${day}${month}${year}`;
};

// 打开打印页面
const openPrintJob = async () => {
    ptintTitle.value = '打印SKU';
    let printData = {};
    printData.printTemplate = "打印SKU模板";
    if(state.value.header.sku == "" || state.value.header.sku == undefined){
        ElMessage.warning("请输入SKU");
        return;
    }
    console.log("打印SKU");
    console.log(state.value.header);
    var res = await pageWMSProduct(Object.assign(state.value.header, tableParams.value));
    if (res.data.result != null) {
        printData.data = [];
        for (let i = 0; i < state.value.header.qty; i++) {
            // 获取原始产品数据
            const item = res.data.result?.items[0] || {};
            
            // 获取打印格式，默认为hach
            const printFormat = state.value.header.printFormat || 'hach';
            
            // 根据选择的格式处理打印数据
            if (printFormat === 'json') {
                // JSON格式：将sku,批次,过期日期转换成json格式
                item.printData = JSON.stringify({
                    sku: state.value.header.sku || "",
                    batchNo: state.value.header.batchNo || "",
                    expiryDate: state.value.header.expiryDate || ""
                });
            } else if (printFormat === 'normal') {
                // 普通格式：仅仅打印sku
                item.printData = state.value.header.sku || "";
            } else {
                // Hach格式：使用现在的功能
                const sku = state.value.header.sku || "";
                const batchNo = state.value.header.batchNo || "";
                const expiryDate = state.value.header.expiryDate ? formatDateForPrint(state.value.header.expiryDate) : "";
                
                // 组合成指定格式: |ITM2651600|LOT4085|EXP01AP2026|
                item.printData = `|ITM${sku}|LOT${batchNo}|EXP${expiryDate}|`;
            }
            
            // 保留原始数据
            item.batchNo = state.value.header.batchNo;
            item.expiryDate = state.value.header.expiryDate;
            item.printFormat = printFormat;
            
            printData.data.push(item);
        }
    }
    // 判断有没有配置客户自定义打印模板
    printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
};

</script>
