﻿<template>
  <div class="sysWorkFlow-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="流程名称">
          <el-input v-model="queryParams.workName" clearable="" placeholder="请输入流程名称" />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'sysWorkFlow:page'"> 查询
            </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddSysWorkFlow" v-auth="'sysWorkFlow:add'"> 新增流程
          </el-button>
          <el-button type="primary" icon="ele-Plus" @click="openAddSysWorkFlowAudit" v-auth="'sysWorkFlow:add'"> 新增审批
          </el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="index" label="序号" />
        <el-table-column prop="workName" label="流程名称" />
        <el-table-column prop="workTable" label="WorkTable" />
        <el-table-column prop="workTableName" label="WorkTableName" />
        <el-table-column prop="nodeConfig" label="NodeConfig" show-overflow-tooltip="" />
        <el-table-column prop="lineConfig" label="LineConfig" show-overflow-tooltip="" />
        <el-table-column prop="remark" label="备注" show-overflow-tooltip="" />
        <el-table-column prop="createDate" label="创建时间" show-overflow-tooltip="" />
        <el-table-column prop="creator" label="创建人" show-overflow-tooltip="" />
        <el-table-column prop="auditingEdit" label="AuditingEdit" show-overflow-tooltip="" />
        <el-table-column label="操作" width="240" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('sysWorkFlow:edit') || auth('sysWorkFlow:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditSysWorkFlow(scope.row)"
              v-auth="'sysWorkFlow:edit'"> 编辑 </el-button>
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openCopySysWorkFlow(scope.row)"
              v-auth="'sysWorkFlow:edit'"> 复制 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delSysWorkFlow(scope.row)"
              v-auth="'sysWorkFlow:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-dialog title="复制流程" v-model="dialogVisible" width="30%" :before-close="handleClose">
        <span>请输入流程名称<el-input v-model="workFlowCopy.workName" placeholder="请输入内容"></el-input></span>
        <template #footer>
          <el-button @click="dialogVisible = false">取 消</el-button>
          <el-button type="primary" @click="copyWorkFlowData">确 定</el-button>
        </template>
      </el-dialog>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editSysWorkFlowTitle" @reloadTable="handleQuery" />
      <workFlowGridHeader ref="workFlowGridHeaderRef" @reloadTable="handleQuery" />
      <workFlowGridHeaderAdd ref="workFlowGridHeaderAddRef" @reloadTable="handleQuery" />
      <workFlowGridAdd ref="workFlowGridAddRef" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="sysWorkFlow">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import workFlowGridHeader from '/@/views/system/flow/WorkFlowGridHeader.vue'
import workFlowGridHeaderAdd from '/@/views/system/flow/WorkFlowGridHeaderAdd.vue'
import workFlowGrid from '/@/views/system/workFlow/WorkFlowGridHeader.vue'
import workFlowGridAdd from '/@/views/system/workFlow/WorkFlowGridHeaderAdd.vue'
// import editDialog from '/@/views/main/sysWorkFlow/component/editDialog.vue'
import { pageSysWorkFlow, deleteSysWorkFlow,copyWorkFlow } from '/@/api/main/sysWorkFlow';


const dialogVisible = ref();
const workFlowGridHeaderRef = ref();
const workFlowGridHeaderAddRef = ref();
const workFlowGridAddRef = ref();
const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any>
  ([]);
const queryParams = ref<any>
  ({
    workName:'',
  });
  const workFlowCopy = ref<any>
  ({
    
  });
  const workFlowData = ref<any>
  ({
    
  });
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editSysWorkFlowTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageSysWorkFlow(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddSysWorkFlow = () => {
  editSysWorkFlowTitle.value = '添加业务流程';
  workFlowGridHeaderAddRef.value.open({});
};

// 打开新增页面
const openAddSysWorkFlowAudit = () => {
  editSysWorkFlowTitle.value = '添加审批流程';
  workFlowGridAddRef.value.open({});
};
// 打开编辑页面
const openEditSysWorkFlow = (row: any) => {
  editSysWorkFlowTitle.value = '编辑业务流程';
  // console.log(row);
  // console.log("row");
  workFlowGridHeaderRef.value.open(row);
  // editSysWorkFlowTitle.value = '编辑SysWorkFlow';
  // editDialogRef.value.openDialog(row);
};

// 打开编辑页面
const openEditSysWorkFlowAudit = (row: any) => {
  editSysWorkFlowTitle.value = '编辑审批流程';
  // console.log(row);
  // console.log("row");
  workFlowGridHeaderRef.value.open(row);
  // editSysWorkFlowTitle.value = '编辑SysWorkFlow';
  // editDialogRef.value.openDialog(row);
};
const handleClose = (row: any) => {
  dialogVisible.value = false;
}
// 复制一个流程
const openCopySysWorkFlow = (row: any) => {
  dialogVisible.value = true;
  // console.log(row);
  workFlowData.value = {...row};
  // console.log(workFlowData.value);

}

// 复制一个流程
const copyWorkFlowData = () => {
  workFlowData.value.workName = workFlowCopy.value.workName;
  // console.log(workFlowData.value);
  copyWorkFlow(workFlowData.value);
  dialogVisible.value = false;
  ElMessage.success("操作成功");
  handleQuery();
}
// 删除
const delSysWorkFlow = (row: any) => {
  console.log(row);
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteSysWorkFlow(row);
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
