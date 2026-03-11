<template>
  <div class="wMSRFReceiptAcquisition-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
         
        <el-form-item label="入库单号">
          <el-input v-model="queryParams.receiptNumber" clearable="" placeholder="请输入入库单号"/>
          
        </el-form-item>
        <el-form-item label="外部单号">
          <el-input v-model="queryParams.externReceiptNumber" clearable="" placeholder="请输入外部单号"/>
          
        </el-form-item>
          
        <el-form-item label="类型">
          <el-input v-model="queryParams.type" clearable="" placeholder="请输入类型"/>
          
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" > 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSRFReceiptAcquisition" v-auth="'wMSRFReceiptAcquisition:add'"> 新增 </el-button>
          
        </el-form-item> -->
        <el-form-item>
          <el-button type="primary" icon="ele-Download" @click="handleExport"  > 导出 </el-button>
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
         <!-- <el-table-column prop="aSNId" label="ASNId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="aSNNumber" label="预入库单号" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="receiptDetailId" label="ReceiptDetailId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="receiptNumber" label="入库单号" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="externReceiptNumber" label="外部单号" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="customerName" label="客户名称" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="warehouseName" label="仓库名称" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sKU" label="SKU" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="lot" label="Lot" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sN" label="SN" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="qty" label="Qty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="productionDate" label="生产日期" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expirationDate" label="过期日期" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="receiptAcquisitionStatus" label="ReceiptAcquisitionStatus" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="creator" label="创建人" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="creationTime" label="创建时间" fixed="" show-overflow-tooltip="" />
         <!-- <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" /> -->
         <el-table-column prop="type" label="类型" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSRFReceiptAcquisition:edit') || auth('wMSRFReceiptAcquisition:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSRFReceiptAcquisition(scope.row)" v-auth="'wMSRFReceiptAcquisition:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSRFReceiptAcquisition(scope.row)" v-auth="'wMSRFReceiptAcquisition:delete'"> 删除 </el-button>
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
			    :title="editWMSRFReceiptAcquisitionTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSRFReceiptAcquisition">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  import { downloadByData, getFileName } from '/@/utils/download';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/wMSRFReceiptAcquisition/component/editDialog.vue'
  import { pageWMSRFReceiptAcquisition, deleteWMSRFReceiptAcquisition, exportWMSRFReceiptAcquisition } from '/@/api/main/wMSRFReceiptAcquisition';


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
        const editWMSRFReceiptAcquisitionTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSRFReceiptAcquisition(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSRFReceiptAcquisition = () => {
        editWMSRFReceiptAcquisitionTitle.value = '添加WMS_RFReceiptAcquisition';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSRFReceiptAcquisition = (row: any) => {
        editWMSRFReceiptAcquisitionTitle.value = '编辑WMS_RFReceiptAcquisition';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSRFReceiptAcquisition = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSRFReceiptAcquisition(row);
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

        // 导出
        const handleExport = async () => {
        try {
            loading.value = true;
            const res = await exportWMSRFReceiptAcquisition(queryParams.value);
            var fileName = getFileName(res.headers);
            downloadByData(res.data as any, fileName);
            ElMessage.success("导出成功");
        } catch (error) {
            ElMessage.error("导出失败");
        } finally {
            loading.value = false;
        }
        };


handleQuery();
</script>


