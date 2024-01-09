<template>
  <div class="wMSInventoryReport-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="InboundQty">
          <el-input v-model="queryParams.inboundQty" clearable="" placeholder="请输入InboundQty"/>
          
        </el-form-item>
        <el-form-item label="OutboundQty">
          <el-input v-model="queryParams.outboundQty" clearable="" placeholder="请输入OutboundQty"/>
          
        </el-form-item>
        <el-form-item label="AvailableInventory">
          <el-input v-model="queryParams.availableInventory" clearable="" placeholder="请输入AvailableInventory"/>
          
        </el-form-item>
        <el-form-item label="FreezeInventory">
          <el-input v-model="queryParams.freezeInventory" clearable="" placeholder="请输入FreezeInventory"/>
          
        </el-form-item>
        <el-form-item label="OccupyInventory">
          <el-input v-model="queryParams.occupyInventory" clearable="" placeholder="请输入OccupyInventory"/>
          
        </el-form-item>
        <el-form-item label="AdjustInventory">
          <el-input v-model="queryParams.adjustInventory" clearable="" placeholder="请输入AdjustInventory"/>
          
        </el-form-item>
        <el-form-item label="SKU">
          <el-input v-model="queryParams.sKU" clearable="" placeholder="请输入SKU"/>
          
        </el-form-item>
        <el-form-item label="Qty">
          <el-input v-model="queryParams.qty" clearable="" placeholder="请输入Qty"/>
          
        </el-form-item>
        <el-form-item label="InventoryType">
          <el-input v-model="queryParams.inventoryType" clearable="" placeholder="请输入InventoryType"/>
          
        </el-form-item>
        <el-form-item label="InventoryStatus">
          <el-input-number v-model="queryParams.inventoryStatus"  clearable="" placeholder="请输入InventoryStatus"/>
          
        </el-form-item>
        <el-form-item label="InventoryDate">
          <el-date-picker placeholder="请选择InventoryDate" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.inventoryDateRange" />
          
        </el-form-item>
        <el-form-item label="CustomerId">
          <el-input v-model="queryParams.customerId" clearable="" placeholder="请输入CustomerId"/>
          
        </el-form-item>
        <el-form-item label="CustomerName">
          <el-input v-model="queryParams.customerName" clearable="" placeholder="请输入CustomerName"/>
          
        </el-form-item>
        <el-form-item label="WarehouseId">
          <el-input v-model="queryParams.warehouseId" clearable="" placeholder="请输入WarehouseId"/>
          
        </el-form-item>
        <el-form-item label="WarehouseName">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入WarehouseName"/>
          
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'wMSInventoryReport:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSInventoryReport" v-auth="'wMSInventoryReport:add'"> 新增 </el-button>
          
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
         <el-table-column prop="inboundQty" label="InboundQty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="outboundQty" label="OutboundQty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="availableInventory" label="AvailableInventory" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="freezeInventory" label="FreezeInventory" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="occupyInventory" label="OccupyInventory" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="adjustInventory" label="AdjustInventory" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sKU" label="SKU" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="qty" label="Qty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="inventoryType" label="InventoryType" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="inventoryStatus" label="InventoryStatus" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="inventoryDate" label="InventoryDate" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerName" label="CustomerName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseName" label="WarehouseName" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSInventoryReport:edit') || auth('wMSInventoryReport:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSInventoryReport(scope.row)" v-auth="'wMSInventoryReport:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSInventoryReport(scope.row)" v-auth="'wMSInventoryReport:delete'"> 删除 </el-button>
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
			    :title="editWMSInventoryReportTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSInventoryReport">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/wMSInventoryReport/component/editDialog.vue'
  import { pageWMSInventoryReport, deleteWMSInventoryReport } from '/@/api/main/wMSInventoryReport';


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
        const editWMSInventoryReportTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSInventoryReport(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSInventoryReport = () => {
        editWMSInventoryReportTitle.value = '添加WMSInventoryReport';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSInventoryReport = (row: any) => {
        editWMSInventoryReportTitle.value = '编辑WMSInventoryReport';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSInventoryReport = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSInventoryReport(row);
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


