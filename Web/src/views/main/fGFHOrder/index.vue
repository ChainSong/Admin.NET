<template>
  <div class="fGFHOrder-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="OutStockMID">
          <el-input v-model="queryParams.outStockMID" clearable="" placeholder="请输入OutStockMID" />

        </el-form-item>
        <el-form-item label="OutStockNo">
          <el-input v-model="queryParams.outStockNo" clearable="" placeholder="请输入OutStockNo" />

        </el-form-item>
        <el-form-item label="OutStockNumber">
          <el-input v-model="queryParams.outStockNumber" clearable="" placeholder="请输入OutStockNumber" />

        </el-form-item>
        <el-form-item label="OutStockDate">
          <el-date-picker placeholder="请选择OutStockDate" value-format="YYYY/MM/DD" type="daterange"
            v-model="queryParams.outStockDateRange" />

        </el-form-item>
        <el-form-item label="OutStockType">
          <el-input v-model="queryParams.outStockType" clearable="" placeholder="请输入OutStockType" />

        </el-form-item>
        <!-- <el-form-item label="DownloadURL">
          <el-input v-model="queryParams.downloadURL" clearable="" placeholder="请输入DownloadURL"/>
          
        </el-form-item>
        <el-form-item label="ContractNo">
          <el-input v-model="queryParams.contractNo" clearable="" placeholder="请输入ContractNo"/>
          
        </el-form-item>
        <el-form-item label="Addressee">
          <el-input v-model="queryParams.addressee" clearable="" placeholder="请输入Addressee"/>
          
        </el-form-item>
        <el-form-item label="Phone">
          <el-input v-model="queryParams.phone" clearable="" placeholder="请输入Phone"/>
          
        </el-form-item>
        <el-form-item label="Address">
          <el-input v-model="queryParams.address" clearable="" placeholder="请输入Address"/>
          
        </el-form-item> -->
        <el-form-item label="IsReturn">
          <el-input-number v-model="queryParams.isReturn" clearable="" placeholder="请输入IsReturn" />

        </el-form-item>
        <el-form-item label="ReturnDate">
          <el-date-picker placeholder="请选择ReturnDate" value-format="YYYY/MM/DD" type="daterange"
            v-model="queryParams.returnDateRange" />

        </el-form-item>
        <el-form-item label="Reason">
          <el-input v-model="queryParams.reason" clearable="" placeholder="请输入Reason" />

        </el-form-item>
        <!-- <el-form-item label="Str1">
          <el-input v-model="queryParams.str1" clearable="" placeholder="请输入Str1"/>
          
        </el-form-item>
        <el-form-item label="Str2">
          <el-input v-model="queryParams.str2" clearable="" placeholder="请输入Str2"/>
          
        </el-form-item>
        <el-form-item label="Str3">
          <el-input v-model="queryParams.str3" clearable="" placeholder="请输入Str3"/>
          
        </el-form-item>
        <el-form-item label="Int1">
          <el-input-number v-model="queryParams.int1"  clearable="" placeholder="请输入Int1"/>
          
        </el-form-item>
        <el-form-item label="Int2">
          <el-input-number v-model="queryParams.int2"  clearable="" placeholder="请输入Int2"/>
          
        </el-form-item>
        <el-form-item label="Int3">
          <el-input-number v-model="queryParams.int3"  clearable="" placeholder="请输入Int3"/>
          
        </el-form-item>
        <el-form-item label="Datetime1">
          <el-date-picker placeholder="请选择Datetime1" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.datetime1Range" />
          
        </el-form-item>
        <el-form-item label="Datetime2">
          <el-date-picker placeholder="请选择Datetime2" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.datetime2Range" />
          
        </el-form-item>
        <el-form-item label="Datetime3">
          <el-date-picker placeholder="请选择Datetime3" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.datetime3Range" />
          
        </el-form-item> -->
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'fGFHOrder:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>

          </el-button-group>

        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddFGFHOrder" v-auth="'fGFHOrder:add'"> 新增 </el-button>

        </el-form-item> -->

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="index" label="序号" width="55" align="center" />
        <el-table-column prop="outStockMID" label="OutStockMID" show-overflow-tooltip="" />
        <el-table-column prop="outStockNo" label="OutStockNo" show-overflow-tooltip="" />
        <el-table-column prop="outStockNumber" label="OutStockNumber" show-overflow-tooltip="" />
        <el-table-column prop="outStockDate" label="OutStockDate" show-overflow-tooltip="" />
        <el-table-column prop="outStockType" label="OutStockType" show-overflow-tooltip="" />
        <!-- <el-table-column prop="downloadURL" label="DownloadURL"   show-overflow-tooltip="" /> -->
        <el-table-column prop="contractNo" label="ContractNo" show-overflow-tooltip="" />
        <!-- <el-table-column prop="addressee" label="Addressee"   show-overflow-tooltip="" />
         <el-table-column prop="phone" label="Phone"   show-overflow-tooltip="" />
         <el-table-column prop="address" label="Address"   show-overflow-tooltip="" /> -->
        <el-table-column prop="isReturn" label="IsReturn" show-overflow-tooltip="" />
        <el-table-column prop="returnDate" label="ReturnDate" show-overflow-tooltip="" />
        <el-table-column prop="reason" label="Reason" show-overflow-tooltip="" />
        <!-- <el-table-column prop="str1" label="Str1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str2" label="Str2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str3" label="Str3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int1" label="Int1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int2" label="Int2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int3" label="Int3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="datetime1" label="Datetime1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="datetime2" label="Datetime2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="datetime3" label="Datetime3" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditFGFHOrder(scope.row)"> 重置回传
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editFGFHOrderTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="fGFHOrder">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/fGFHOrder/component/editDialog.vue'
import { pageFGFHOrder, deleteFGFHOrder } from '/@/api/main/fGFHOrder';


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
const editFGFHOrderTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageFGFHOrder(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddFGFHOrder = () => {
  editFGFHOrderTitle.value = '添加福光出库回传';
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditFGFHOrder = (row: any) => {
 ElMessageBox.confirm(`确定要重置吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteFGFHOrder(row);
      handleQuery();
      ElMessage.success("重置成功");
    })
    .catch(() => { });
};

// 删除
const delFGFHOrder = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteFGFHOrder(row);
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


handleQuery();
</script>
