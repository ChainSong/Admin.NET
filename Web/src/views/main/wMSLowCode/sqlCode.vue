<template>
  <div class="wMSLowCode-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-input v-model="input" placeholder="请输入内容"></el-input>
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
        <!-- <el-table-column type="index" label="序号" width="55" align="center"/> -->
         <el-table-column prop="menuName" label="名称" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="uiCode" label="UI设计" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="uiType" label="UI类型" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dataSource" label="数据源" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sqlCode" label="sql" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creator" label="创建人" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="创建时间" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSLowCode:edit') || auth('wMSLowCode:delete')">
          <template #default="scope">
            <!-- <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSLowCode(scope.row)" v-auth="'wMSLowCode:edit'"> 编辑 </el-button> -->
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSLowCode(scope.row)" v-auth="'wMSLowCode:delete'"> 删除 </el-button>
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
			    :title="editWMSLowCodeTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSLowCode">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';
  
  import editDialog from '/@/views/main/wMSLowCode/component/editDialog.vue'
  import { pageWMSLowCode, deleteWMSLowCode } from '/@/api/main/wMSLowCode';


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
        const editWMSLowCodeTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSLowCode(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSLowCode = () => {
        editWMSLowCodeTitle.value = '添加WMSLowCode';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSLowCode = (row: any) => {
        editWMSLowCodeTitle.value = '编辑WMSLowCode';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSLowCode = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSLowCode(row);
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


