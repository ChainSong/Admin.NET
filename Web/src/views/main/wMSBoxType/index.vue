<template>
  <div class="wMSBoxType-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <!-- <el-form-item label="CustomerId">
          <el-input v-model="queryParams.customerId" clearable="" placeholder="请输入CustomerId"/>
        </el-form-item> -->
        <el-form-item label="客户名称">
          <el-input v-model="queryParams.customerName" clearable="" placeholder="请输入客户名称" />
        </el-form-item>
        <!-- <el-form-item label="WarehouseId">
          <el-input v-model="queryParams.warehouseId" clearable="" placeholder="请输入WarehouseId"/>
        </el-form-item> -->
        <el-form-item label="仓库名称">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入仓库名称" />

        </el-form-item>
        <el-form-item label="箱型号">
          <el-input v-model="queryParams.boxCode" clearable="" placeholder="请输入箱型号" />

        </el-form-item>
        <el-form-item label="箱类型">
          <el-input v-model="queryParams.boxType" clearable="" placeholder="请输入箱类型" />

        </el-form-item>
        <!-- <el-form-item label="Length">
          <el-input v-model="queryParams.length" clearable="" placeholder="请输入Length"/>
          
        </el-form-item>
        <el-form-item label="Width">
          <el-input v-model="queryParams.width" clearable="" placeholder="请输入Width"/>
          
        </el-form-item>
        <el-form-item label="Height">
          <el-input v-model="queryParams.height" clearable="" placeholder="请输入Height"/>
          
        </el-form-item>
        <el-form-item label="Volume">
          <el-input v-model="queryParams.volume" clearable="" placeholder="请输入Volume"/>
          
        </el-form-item>
        <el-form-item label="NetWeight">
          <el-input v-model="queryParams.netWeight" clearable="" placeholder="请输入NetWeight"/>
          
        </el-form-item>
        <el-form-item label="GrossWeight">
          <el-input v-model="queryParams.grossWeight" clearable="" placeholder="请输入GrossWeight"/>
          
        </el-form-item>
        <el-form-item label="BoxStatus">
          <el-input-number v-model="queryParams.boxStatus"  clearable="" placeholder="请输入BoxStatus"/>
          
        </el-form-item>
        <el-form-item label="Remark">
          <el-input v-model="queryParams.remark" clearable="" placeholder="请输入Remark"/>
          
        </el-form-item>
        <el-form-item label="Str1">
          <el-input v-model="queryParams.str1" clearable="" placeholder="请输入Str1"/>
          
        </el-form-item>
        <el-form-item label="Str2">
          <el-input v-model="queryParams.str2" clearable="" placeholder="请输入Str2"/>
        </el-form-item>
        <el-form-item label="Str3">
          <el-input v-model="queryParams.str3" clearable="" placeholder="请输入Str3"/>
        </el-form-item>
        <el-form-item label="Str4">
          <el-input v-model="queryParams.str4" clearable="" placeholder="请输入Str4"/>
        </el-form-item>
        <el-form-item label="Str5">
          <el-input v-model="queryParams.str5" clearable="" placeholder="请输入Str5"/>
        </el-form-item>
        <el-form-item label="Creator">
          <el-input v-model="queryParams.creator" clearable="" placeholder="请输入Creator"/>
        </el-form-item>
        <el-form-item label="CreationTime">
          <el-date-picker placeholder="请选择CreationTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.creationTimeRange" />
        </el-form-item>
        <el-form-item label="Updator">
          <el-input v-model="queryParams.updator" clearable="" placeholder="请输入Updator"/>
        </el-form-item> -->
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSBoxType:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>

          </el-button-group>

        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSBoxType" v-auth="'wMSBoxType:add'"> 新增
          </el-button>

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="index" label="序号" fixed="" width="55" align="center" />
        <!-- <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column prop="warehouseName" label="仓库名称" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="boxCode" label="箱号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="boxType" label="箱类型" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="length" label="长度" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="width" label="宽度" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="height" label="高度" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="volume" label="体积" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="netWeight" label="净重" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="grossWeight" label="毛重" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="boxStatus" label="箱状态" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="remark" label="备注" fixed="" show-overflow-tooltip="" />
        <!-- <el-table-column prop="str1" label="Str1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str2" label="Str2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str3" label="Str3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str4" label="Str4" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str5" label="Str5" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creator" label="Creator" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="CreationTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('wMSBoxType:edit') || auth('wMSBoxType:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSBoxType(scope.row)"
              v-auth="'wMSBoxType:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSBoxType(scope.row)"
              v-auth="'wMSBoxType:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editWMSBoxTypeTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSBoxType">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSBoxType/component/editDialog.vue'
import { pageWMSBoxType, deleteWMSBoxType } from '/@/api/main/wMSBoxType';
import selectRemote from '/@/views/tools/select-remote.vue';

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
const editWMSBoxTypeTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSBoxType(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddWMSBoxType = () => {
  editWMSBoxTypeTitle.value = '添加箱类型';
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditWMSBoxType = (row: any) => {
  editWMSBoxTypeTitle.value = '编辑WMSBoxType';
  editDialogRef.value.openDialog(row);
};

// 删除
const delWMSBoxType = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSBoxType(row);
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
