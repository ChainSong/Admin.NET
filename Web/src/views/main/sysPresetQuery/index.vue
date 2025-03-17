<template>
  <div class="sysPresetQuery-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="QueryName">
          <el-input v-model="queryParams.queryName" clearable="" placeholder="请输入QueryName"/>
          
        </el-form-item>
        <el-form-item label="BusinessName">
          <el-input v-model="queryParams.businessName" clearable="" placeholder="请输入BusinessName"/>
          
        </el-form-item>
        <el-form-item label="QueryForm">
          <el-input v-model="queryParams.queryForm" clearable="" placeholder="请输入QueryForm"/>
          
        </el-form-item>
        <el-form-item label="Creator">
          <el-input v-model="queryParams.creator" clearable="" placeholder="请输入Creator"/>
          
        </el-form-item>
        <el-form-item label="CreationTime">
          <el-date-picker placeholder="请选择CreationTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.creationTimeRange" />
          
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'sysPresetQuery:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddSysPresetQuery" v-auth="'sysPresetQuery:add'"> 新增 </el-button>
          
        </el-form-item>
        
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table
				:data="tableData"
				style="width: 100%"
				v-loading="loading"
				tooltip-effect="light"
				row-key="id"
				border="">
        <el-table-column type="index" label="序号" width="55" align="center"/>
         <el-table-column prop="queryName" label="QueryName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="businessName" label="BusinessName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="queryForm" label="QueryForm" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creator" label="Creator" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="CreationTime" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('sysPresetQuery:edit') || auth('sysPresetQuery:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditSysPresetQuery(scope.row)" v-auth="'sysPresetQuery:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delSysPresetQuery(scope.row)" v-auth="'sysPresetQuery:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination
				v-model:currentPage="tableParams.page"
				v-model:page-size="tableParams.pageSize"
				:total="tableParams.total"
				:page-sizes="[10, 20, 50, 100]"
				small=""
				background=""
				@size-change="handleSizeChange"
				@current-change="handleCurrentChange"
				layout="total, sizes, prev, pager, next, jumper"
	/>
      <editDialog
			    ref="editDialogRef"
			    :title="editSysPresetQueryTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="sysPresetQuery">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/sysPresetQuery/component/editDialog.vue'
  import { pageSysPresetQuery, deleteSysPresetQuery } from '/@/api/main/sysPresetQuery';


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
        const editSysPresetQueryTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageSysPresetQuery(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddSysPresetQuery = () => {
        editSysPresetQueryTitle.value = '添加SysPresetQuery';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditSysPresetQuery = (row: any) => {
        editSysPresetQueryTitle.value = '编辑SysPresetQuery';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delSysPresetQuery = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteSysPresetQuery(row);
        handleQuery();
        ElMessage.success("删除成功");
        })
        .catch(() => {});
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


