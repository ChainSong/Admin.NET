<template>
  <div class="wMSExpressConfig-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="客户名称">
          <el-input v-model="queryParams.customerName" clearable="" placeholder="请输入客户名称"/>
          
        </el-form-item>
        <el-form-item label="仓库名称">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入仓库名称"/>
          
        </el-form-item>
        <el-form-item label="快递代码">
          <el-input v-model="queryParams.expressCode" clearable="" placeholder="请输入快递代码"/>
          
        </el-form-item>
        <el-form-item label="快递公司">
          <el-input v-model="queryParams.expressCompany" clearable="" placeholder="请输入快递公司"/>
          
        </el-form-item>
     
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'wMSExpressConfig:page'"> 查询 </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
            
          </el-button-group>
          
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSExpressConfig" v-auth="'wMSExpressConfig:add'"> 新增 </el-button>
          
        </el-form-item>
         -->
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
         <!-- <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="warehouseName" label="仓库名称" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expressCode" label="快递代码" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expressCompany" label="快递公司" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="url" label="Url" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="appKey" label="AppKey" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="companyCode" label="CompanyCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sign" label="Sign" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerCode" label="CustomerCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="partnerId" label="PartnerId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="checkword" label="Checkword" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="clientId" label="ClientId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="version" label="Version" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="password" label="Password" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="monthAccount" label="MonthAccount" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="status" label="Status" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creator" label="Creator" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="CreationTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str1" label="Str1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str2" label="Str2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str3" label="Str3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str4" label="Str4" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str5" label="Str5" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str6" label="Str6" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str7" label="Str7" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str8" label="Str8" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str9" label="Str9" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str10" label="Str10" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str11" label="Str11" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str12" label="Str12" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str13" label="Str13" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str14" label="Str14" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str15" label="Str15" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str16" label="Str16" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str17" label="Str17" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str18" label="Str18" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str19" label="Str19" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="str20" label="Str20" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dateTime1" label="DateTime1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dateTime2" label="DateTime2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dateTime3" label="DateTime3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dateTime4" label="DateTime4" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="dateTime5" label="DateTime5" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSExpressConfig:edit') || auth('wMSExpressConfig:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSExpressConfig(scope.row)" v-auth="'wMSExpressConfig:edit'"> 编辑 </el-button>
            <!-- <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSExpressConfig(scope.row)" v-auth="'wMSExpressConfig:delete'"> 删除 </el-button> -->
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
			    :title="editWMSExpressConfigTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSExpressConfig">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/wMSExpressConfig/component/editDialog.vue'
  import { pageWMSExpressConfig, deleteWMSExpressConfig } from '/@/api/main/wMSExpressConfig';


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
        const editWMSExpressConfigTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSExpressConfig(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSExpressConfig = () => {
        editWMSExpressConfigTitle.value = '添加WMSExpressConfig';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSExpressConfig = (row: any) => {
        editWMSExpressConfigTitle.value = '编辑WMSExpressConfig';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSExpressConfig = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSExpressConfig(row);
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


