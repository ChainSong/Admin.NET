<template>
  <div class="wMSRFIDInfo-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        
        <el-form-item label="入库单号">
          <el-input v-model="queryParams.receiptNumber" clearable="" placeholder="请输入入库单号" />
        </el-form-item>
        <el-form-item label="入库外部单号">
          <el-input v-model="queryParams.externReceiptNumber" clearable="" placeholder="请输入入库单号" />
        </el-form-item>
        <el-form-item label="出库单号">
          <el-input v-model="queryParams.orderNumber" clearable="" placeholder="请输入出库单号" />
        </el-form-item>
        <el-form-item label="入库外部单号">
          <el-input v-model="queryParams.externOrderNumber" clearable="" placeholder="请输入出库外部单号" />
        </el-form-item>

        <el-form-item label="Sequence">
          <el-input v-model="queryParams.sequence" clearable="" placeholder="请输入Sequence" />
        </el-form-item>

        <el-form-item label="RFID">
          <el-input v-model="queryParams.rfid" clearable="" placeholder="请输入RFID" />

        </el-form-item>
        <el-form-item label="SKU">
          <el-input v-model="queryParams.sku" clearable="" placeholder="请输入SKU" />

        </el-form-item>
        <el-form-item label="验证码">
          <el-input v-model="queryParams.snCode" clearable="" placeholder="请输入验证码" />
        </el-form-item>

        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSRFIDInfo:page'"> 查询
            </el-button>
            <el-button type="primary" icon="ele-Upload" @click="addRFIDSnCode" v-auth="'wMSRFIDInfo:page'"> 导入验证码
            </el-button>
            <el-button type="primary" icon="ele-Fold" @click="printRFID" v-auth="'wMSReceipt:printRFID'">
              打印RFID</el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
          </el-button-group>
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSRFIDInfo" v-auth="'wMSRFIDInfo:add'"> 新增 </el-button>
          
        </el-form-item> -->

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" ref="multipleTableRef"  tooltip-effect="light" row-key="id" border="">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column type="index" label="序号" width="55" align="center" />
        <el-table-column prop="receiptNumber" label="入库单号" width="150" show-overflow-tooltip="" />
        <el-table-column prop="externReceiptNumber" label="入库外部单号" width="150" show-overflow-tooltip="" />
        <el-table-column prop="asnNumber" label="ASN单号" width="150" show-overflow-tooltip="" />
        <el-table-column prop="customerName" label="客户" show-overflow-tooltip="" />
        <el-table-column prop="warehouseName" label="仓库" show-overflow-tooltip="" width="150"/>
        <el-table-column prop="sku" label="SKU" show-overflow-tooltip="" width="200" />
        <el-table-column prop="goodsType" label="产品品级" show-overflow-tooltip=""  width="150"/>
        <el-table-column prop="batchCode" label="批次" show-overflow-tooltip="" />
        <el-table-column prop="receiptPerson" label="入库人" width="150" show-overflow-tooltip="" />
        <el-table-column prop="receiptTime" label="入库时间" width="150" show-overflow-tooltip="" />
        <el-table-column prop="orderNumber" label="出库单号" width="150" show-overflow-tooltip="" />
        <el-table-column prop="externOrderNumber" label="出库外部单号" width="150" show-overflow-tooltip="" />
        <el-table-column prop="orderPerson" label="出库人" width="150" show-overflow-tooltip="" />
        <el-table-column prop="orderTime" label="出库时间" width="150" show-overflow-tooltip="" />
        <el-table-column prop="status" label="状态" show-overflow-tooltip="" >
          <template #default="scope">
            <el-tag v-if="scope.row.status === 1" type="success">未出库</el-tag>
            <el-tag v-else-if="scope.row.status === 0" type="warning">待入库</el-tag>
            <el-tag v-else  type="danger">已出库</el-tag>
          </template>
        </el-table-column>
        <el-table-column prop="printTime" label="打印时间" width="150" show-overflow-tooltip="" />
        <el-table-column prop="printPerson" label="打印人" width="150" show-overflow-tooltip="" />
        <el-table-column prop="sequence" label="sequence"  width="250" show-overflow-tooltip="" />
        <el-table-column prop="rfid" label="RFID" width="250" show-overflow-tooltip="" />
        <el-table-column prop="snCode" label="验证码"  width="250" show-overflow-tooltip="" />
        <el-table-column prop="printNum" label="打印次数" show-overflow-tooltip="" />
        <el-table-column prop="printTime" label="打印时间" width="150" show-overflow-tooltip="" />
        <el-table-column prop="printPerson" label="打印人" width="150" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('wMSRFIDInfo:edit') || auth('wMSRFIDInfo:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openQueryWMSRFIDInfo(scope.row)"
              v-auth="'wMSRFIDInfo:edit'">查看 </el-button>
            <!-- <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSRFIDInfo(scope.row)" v-auth="'wMSRFIDInfo:edit'">查看 </el-button> -->
            <!-- <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSRFIDInfo(scope.row)" v-auth="'wMSRFIDInfo:delete'"> 删除 </el-button> -->
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editWMSRFIDInfoTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addWMSRFIDInfoTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSRFIDInfo">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSRFIDInfo/component/editDialog.vue'
import addDialog from '/@/views/main/wMSRFIDInfo/component/addDialog.vue'
import { pageWMSRFIDInfo, deleteWMSRFIDInfo } from '/@/api/main/wMSRFIDInfo';
import { getPrinrRFIDInfoByReceiptId,getPrinrRFIDInfoById } from '/@/api/main/wMSRFIDInfo';
import { signalR } from '/@/utils/signalRCustom';
import { stringify } from "querystring";

const editDialogRef = ref();
const addDialogRef = ref();
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
const editWMSRFIDInfoTitle = ref("");
const addWMSRFIDInfoTitle = ref("");
const multipleTableRef = ref();

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSRFIDInfo(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

//添加RFID 验证码
const addRFIDSnCode = (val: number) => {
  addWMSRFIDInfoTitle.value = '添加RFID验证码';
  addDialogRef.value.openDialog({});
};
// 打开新增页面
const openAddWMSRFIDInfo = () => {
  editWMSRFIDInfoTitle.value = '添加WMSRFIDInfo';
  editDialogRef.value.openDialog({});
};


// 打开编辑页面
const openQueryWMSRFIDInfo = (row: any) => {
  editWMSRFIDInfoTitle.value = '查看';
  editDialogRef.value.openDialog(row);
};

// 删除
const delWMSRFIDInfo = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSRFIDInfo(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
};

// 打开转入库单页面
const printRFID = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // 判断是否勾选订单
  if (ids.length < 1) {
    ElMessage.error("请勾选订单");
    return;
  }
  ElMessageBox.confirm(`确定要打印RFID吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let result = await getPrinrRFIDInfoById(ids);
      // console.log("result");
      // console.log(JSON.stringify(result.data.result.data));
      if (result.data.result.code == 1) {
        await signalR.send("Echo", result.data.result.data);
        ElMessage.success("打印成功");
      } else {
        ElMessage.success("打印失败");
      }
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


handleQuery();
</script>
