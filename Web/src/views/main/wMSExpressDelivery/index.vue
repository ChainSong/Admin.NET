<template>
  <div class="wMSExpressDelivery-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="包装号">
          <el-input v-model="queryParams.packageNumber" clearable="" placeholder="包装号"/>
        </el-form-item>
        <el-form-item label="快递单号">
          <el-input v-model="queryParams.expressNumber" clearable="" placeholder="快递单号"/>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'wMSExpressConfig:page'"> 查询 </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
            
          </el-button-group>
          
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
        <!-- <el-table-column type="customerName" label="序号" width="55" align="center"/> -->
        <el-table-column prop="customerName" label="公司名称"  />
        <el-table-column prop="warehouseName" label="仓库名称"  />
        <el-table-column prop="warehouseName" label="仓库名称"  />
        <el-table-column prop="packageNumber" label="包装号"  />
        <el-table-column prop="expressNumber" label="快递单号"  />
        <el-table-column prop="expressCompany" label="快递公司"  />
        <el-table-column prop="senderProvince" label="发货省份"  />
        <el-table-column prop="senderCity" label="发货城市"  />
        <el-table-column prop="recipientsProvince" label="收货省份"  />
        <el-table-column prop="recipientsCity" label="收货城市"  />
        <el-table-column prop="estimatedPrice" label="预计价格"  />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSExpressDelivery:edit') || auth('wMSExpressDelivery:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSExpressDelivery(scope.row)" v-auth="'wMSExpressDelivery:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSExpressDelivery(scope.row)" v-auth="'wMSExpressDelivery:delete'"> 删除 </el-button>
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
			    :title="editWMSExpressDeliveryTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSExpressDelivery">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/wMSExpressDelivery/component/editDialog.vue'
  import { pageWMSExpressDelivery, deleteWMSExpressDelivery } from '/@/api/main/wMSExpressDelivery';


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
        const editWMSExpressDeliveryTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSExpressDelivery(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSExpressDelivery = () => {
        editWMSExpressDeliveryTitle.value = '添加WMSExpressDelivery';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSExpressDelivery = (row: any) => {
        editWMSExpressDeliveryTitle.value = '编辑WMSExpressDelivery';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSExpressDelivery = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSExpressDelivery(row);
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


