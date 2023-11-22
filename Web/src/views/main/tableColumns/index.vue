<template>
  <div class="tableColumns-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="表名">
          <el-input v-model="queryParams.tableName" clearable="" placeholder="请输入表名" />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'tableColumns:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
          </el-button-group>
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddTableColumns" v-auth="'tableColumns:add'"> 新增
          </el-button>
        </el-form-item> -->

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData"   @row-dblclick="openAddTableColumns" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column prop="tableName" label="表名">
        </el-table-column>
        <!-- <el-table-column prop="dbColumnName" label="字段名称" width="180">
            </el-table-column> -->
        <el-table-column prop="tableNameCH" label="中文名称"></el-table-column>
        <el-table-column label="操作">
          <template #default="scope">
            <el-button @click="cleanCache(scope.row)" type="text" size="small">清理缓存</el-button>
          </template>
        </el-table-column>
        <!-- <el-table-column type="index" label="序号" width="55" align="center"/> -->
        <!-- <el-table-column prop="projectId" label="ProjectId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="roleName" label="RoleName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="tableName" label="TableName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="tableNameCH" label="TableNameCH" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="displayName" label="DisplayName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dbColumnName" label="DbColumnName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isKey" label="IsKey" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isSearchCondition" label="IsSearchCondition" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isHide" label="IsHide" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isReadOnly" label="IsReadOnly" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isShowInList" label="IsShowInList" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isImportColumn" label="IsImportColumn" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isDropDownList" label="IsDropDownList" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isCreate" label="IsCreate" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="isUpdate" label="IsUpdate" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="searchConditionOrder" label="SearchConditionOrder" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="validation" label="验证" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="group" label="Group" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="type" label="Type" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="characteristic" label="Characteristic" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="order" label="Order" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="associated" label="Associated" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="precision" label="精确" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="step" label="步骤  台阶" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="max" label="最大值" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="min" label="最小值" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="default" label="默认值" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="link" label="Link" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="relationDBColumn" label="关联" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="forView" label="ForView" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="CreationTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" /> -->
        <!-- <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('tableColumns:edit') || auth('tableColumns:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditTableColumns(scope.row)"
              v-auth="'tableColumns:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delTableColumns(scope.row)"
              v-auth="'tableColumns:delete'"> 删除 </el-button>
          </template>
        </el-table-column> -->
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editTableColumnsTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="tableColumns">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/tableColumns/component/editDialog.vue'
import { pageTableColumns, pageAllTableColumns, deleteTableColumns,cleanTableColumnsCache } from '/@/api/main/tableColumns';


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
const editTableColumnsTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  // console.log("asdasd")
  var res = await pageAllTableColumns(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// // 打开新增页面
// const openAddTableColumns = () => {
//   editTableColumnsTitle.value = '添加表管理';
//   editDialogRef.value.openDialog({});
// };

// 打开编辑页面
const openAddTableColumns = (row: any) => {
  editTableColumnsTitle.value = '编辑表管理';
  editDialogRef.value.openDialog(row);
};

// 删除
const delTableColumns = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteTableColumns(row);
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
//清除缓存
const cleanCache = (row: any) =>
{
  // console.log(row)
  var res =  cleanTableColumnsCache(row);
};

handleQuery();
</script>


