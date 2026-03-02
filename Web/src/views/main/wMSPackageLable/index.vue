<template>
  <div class="wMSPackageLable-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">

        <el-form-item label="预入库单号">
          <el-input v-model="queryParams.preOrderNumber" clearable="" placeholder="请输入预入库单号" />

        </el-form-item>
        <el-form-item label="包装单号">
          <el-input v-model="queryParams.packageNumber" clearable="" placeholder="请输入包装单号" />

        </el-form-item>
        <el-form-item label="出库单号">
          <el-input v-model="queryParams.orderNumber" clearable="" placeholder="请输入出库单号" />

        </el-form-item>
        <el-form-item label="外部单号">
          <el-input v-model="queryParams.externOrderNumber" clearable="" placeholder="请输入出库外部单号" />

        </el-form-item>
        <el-form-item label="客户名称">
          <el-input v-model="queryParams.customerName" clearable="" placeholder="请输入名称客户" />

        </el-form-item>
        <el-form-item label="仓库名称">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入仓库" />

        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSPackageLable"> 新增 </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="openPrintBoxNumber" :loading="opLoading.printBoxNumber"
            :disabled="opLoading.printBoxNumber"> 前置打印箱号
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" ref="multipleTableRef" style="width: 100%" v-loading="loading" tooltip-effect="light"
        row-key="id" border="">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column type="index" label="序号" fixed="" width="55" align="center" />
        <!-- <el-table-column prop="orderId" label="OrderId" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="preOrderNumber" label="预入库单号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="packageNumber" label="包装单号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="orderNumber" label="出库单号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="externOrderNumber" label="外部单号" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="customerId" label="货主ID" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="warehouseName" label="仓库名称" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="packageType" label="包装类型" fixed="" show-overflow-tooltip="" /> -->
        <!-- <el-table-column prop="length" label="长" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="width" label="宽" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="height" label="高" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="netWeight" label="净重" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="grossWeight" label="毛重" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expressCompany" label="快递公司" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expressNumber" label="快递单号" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="serialNumber" label="序号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="printNum" label="打印次数" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="printPersonnel" label="打印人" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="printTime" label="打印时间" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="creator" label="创建人" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="creationTime" label="创建时间" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="updator" label="更新人" fixed="" show-overflow-tooltip="" /> -->
        <!-- <el-table-column prop="remark" label="Remark" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="">
          <template #default="scope">
            <!-- <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSPackageLable(scope.row)"
              >打印 </el-button> -->
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSPackageLable(scope.row)"
              > 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editWMSPackageLableTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addWMSPackageLableTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSPackageLable">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSPackageLable/component/editDialog.vue'
import addDialog from '/@/views/main/wMSPackageLable/component/addDialog.vue'
import printDialog from '/@/views/tools/printDialog.vue'
import { pageWMSPackageLable, deleteWMSPackageLable, printBoxNumber } from '/@/api/main/wMSPackageLable';
import Header from '/@/entities/packageMain';

const multipleTableRef = ref();
const editDialogRef = ref();
const addDialogRef = ref();
const printDialogRef = ref();
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
const editWMSPackageLableTitle = ref("");
const addWMSPackageLableTitle = ref("");
const ptintTitle = ref("");
// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
const opLoading = ref({
  printBoxNumber: false
});


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSPackageLable(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddWMSPackageLable = () => {
  addWMSPackageLableTitle.value = '添加';
  addDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditWMSPackageLable = (row: any) => {
  editWMSPackageLableTitle.value = '编辑';
  editDialogRef.value.openDialog(row);
};

// 删除
const delWMSPackageLable = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSPackageLable(row);
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


// ----------------------// 前置打印箱号-----------------------------

// 前置打印箱号
const openPrintBoxNumber = async () => {
  ptintTitle.value = '打印箱号';
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach((a: any) => {
    ids.push(a.id);
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  opLoading.value.printBoxNumber = true;
  let printData: any = [];
  try {
    let result = await printBoxNumber(ids);
    console.log("箱号打印", result);
    console.log("箱号打印",  result.data.result.data.data);
    if (result.data.result != null) {
      printData = result.data.result.data;
      // printData.forEach((a: any) => {
      //   if (a.customerConfig != null) {
      //     a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
      //   }
      // });
    }
    // 判断有没有配置客户自定义打印模板
    if (printData.printTemplate != "") {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    } else if (printData[0]?.customerConfig != null && printData[0].customerConfig.printShippingTemplate != null) {
      printDialogRef.value.openDialog({ "printData": printData, "templateName": printData[0].customerConfig.printShippingTemplate });
    } else {
      printDialogRef.value.openDialog({ "printData": printData, "templateName": "打印箱号" });
    }
  } catch (e: any) {
    ElMessage.error(e?.message ?? "打印失败");
  } finally {
    opLoading.value.printBoxNumber = false;
  }
};

handleQuery();
</script>
