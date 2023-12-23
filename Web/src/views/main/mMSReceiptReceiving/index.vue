<template>
  <div class="mMSReceiptReceiving-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="Id">
          <el-input v-model="queryParams.id" clearable="" placeholder="请输入Id"/>
          
        </el-form-item>
        <el-form-item label="ReceiptId">
          <el-input v-model="queryParams.receiptId" clearable="" placeholder="请输入ReceiptId"/>
          
        </el-form-item>
        <el-form-item label="ReceiptNumber">
          <el-input v-model="queryParams.receiptNumber" clearable="" placeholder="请输入ReceiptNumber"/>
          
        </el-form-item>
        <el-form-item label="ReceiptReceivingNumber">
          <el-input v-model="queryParams.receiptReceivingNumber" clearable="" placeholder="请输入ReceiptReceivingNumber"/>
          
        </el-form-item>
        <el-form-item label="PurchaseOrderNumber">
          <el-input v-model="queryParams.purchaseOrderNumber" clearable="" placeholder="请输入PurchaseOrderNumber"/>
          
        </el-form-item>
        <el-form-item label="ExternReceiptNumber">
          <el-input v-model="queryParams.externReceiptNumber" clearable="" placeholder="请输入ExternReceiptNumber"/>
          
        </el-form-item>
        <el-form-item label="SupplierId">
          <el-input v-model="queryParams.supplierId" clearable="" placeholder="请输入SupplierId"/>
          
        </el-form-item>
        <el-form-item label="SupplierName">
          <el-input v-model="queryParams.supplierName" clearable="" placeholder="请输入SupplierName"/>
          
        </el-form-item>
        <el-form-item label="WarehouseId">
          <el-input v-model="queryParams.warehouseId" clearable="" placeholder="请输入WarehouseId"/>
          
        </el-form-item>
        <el-form-item label="WarehouseName">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入WarehouseName"/>
          
        </el-form-item>
        <el-form-item label="ReceiptReceivingStartTime">
          <el-date-picker placeholder="请选择ReceiptReceivingStartTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.receiptReceivingStartTimeRange" />
          
        </el-form-item>
        <el-form-item label="ReceiptReceivingEndTime">
          <el-date-picker placeholder="请选择ReceiptReceivingEndTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.receiptReceivingEndTimeRange" />
          
        </el-form-item>
        <el-form-item label="ReceiptReceivingStatus">
          <el-input-number v-model="queryParams.receiptReceivingStatus"  clearable="" placeholder="请输入ReceiptReceivingStatus"/>
          
        </el-form-item>
        <el-form-item label="ReceiptReceivingType">
          <el-input v-model="queryParams.receiptReceivingType" clearable="" placeholder="请输入ReceiptReceivingType"/>
          
        </el-form-item>
        <el-form-item label="CompleteTime">
          <el-date-picker placeholder="请选择CompleteTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.completeTimeRange" />
          
        </el-form-item>
        <el-form-item label="Remark">
          <el-input v-model="queryParams.remark" clearable="" placeholder="请输入Remark"/>
          
        </el-form-item>
        <el-form-item label="Creator">
          <el-input v-model="queryParams.creator" clearable="" placeholder="请输入Creator"/>
          
        </el-form-item>
        <el-form-item label="CreationTime">
          <el-date-picker placeholder="请选择CreationTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.creationTimeRange" />
          
        </el-form-item>
        <el-form-item label="Updator">
          <el-input v-model="queryParams.updator" clearable="" placeholder="请输入Updator"/>
          
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
        <el-form-item label="Str6">
          <el-input v-model="queryParams.str6" clearable="" placeholder="请输入Str6"/>
          
        </el-form-item>
        <el-form-item label="Str7">
          <el-input v-model="queryParams.str7" clearable="" placeholder="请输入Str7"/>
          
        </el-form-item>
        <el-form-item label="Str8">
          <el-input v-model="queryParams.str8" clearable="" placeholder="请输入Str8"/>
          
        </el-form-item>
        <el-form-item label="Str9">
          <el-input v-model="queryParams.str9" clearable="" placeholder="请输入Str9"/>
          
        </el-form-item>
        <el-form-item label="Str10">
          <el-input v-model="queryParams.str10" clearable="" placeholder="请输入Str10"/>
          
        </el-form-item>
        <el-form-item label="Str11">
          <el-input v-model="queryParams.str11" clearable="" placeholder="请输入Str11"/>
          
        </el-form-item>
        <el-form-item label="Str12">
          <el-input v-model="queryParams.str12" clearable="" placeholder="请输入Str12"/>
          
        </el-form-item>
        <el-form-item label="Str13">
          <el-input v-model="queryParams.str13" clearable="" placeholder="请输入Str13"/>
          
        </el-form-item>
        <el-form-item label="Str14">
          <el-input v-model="queryParams.str14" clearable="" placeholder="请输入Str14"/>
          
        </el-form-item>
        <el-form-item label="Str15">
          <el-input v-model="queryParams.str15" clearable="" placeholder="请输入Str15"/>
          
        </el-form-item>
        <el-form-item label="Str16">
          <el-input v-model="queryParams.str16" clearable="" placeholder="请输入Str16"/>
          
        </el-form-item>
        <el-form-item label="Str17">
          <el-input v-model="queryParams.str17" clearable="" placeholder="请输入Str17"/>
          
        </el-form-item>
        <el-form-item label="Str18">
          <el-input v-model="queryParams.str18" clearable="" placeholder="请输入Str18"/>
          
        </el-form-item>
        <el-form-item label="Str19">
          <el-input v-model="queryParams.str19" clearable="" placeholder="请输入Str19"/>
          
        </el-form-item>
        <el-form-item label="Str20">
          <el-input v-model="queryParams.str20" clearable="" placeholder="请输入Str20"/>
          
        </el-form-item>
        <el-form-item label="DateTime1">
          <el-date-picker placeholder="请选择DateTime1" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.dateTime1Range" />
          
        </el-form-item>
        <el-form-item label="DateTime2">
          <el-date-picker placeholder="请选择DateTime2" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.dateTime2Range" />
          
        </el-form-item>
        <el-form-item label="DateTime3">
          <el-date-picker placeholder="请选择DateTime3" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.dateTime3Range" />
          
        </el-form-item>
        <el-form-item label="DateTime4">
          <el-date-picker placeholder="请选择DateTime4" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.dateTime4Range" />
          
        </el-form-item>
        <el-form-item label="DateTime5">
          <el-date-picker placeholder="请选择DateTime5" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.dateTime5Range" />
          
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
        <el-form-item label="Int4">
          <el-input-number v-model="queryParams.int4"  clearable="" placeholder="请输入Int4"/>
          
        </el-form-item>
        <el-form-item label="Int5">
          <el-input-number v-model="queryParams.int5"  clearable="" placeholder="请输入Int5"/>
          
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'mMSReceiptReceiving:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddMMSReceiptReceiving" v-auth="'mMSReceiptReceiving:add'"> 新增 </el-button>
          
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
         <el-table-column prop="id" label="Id" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptId" label="ReceiptId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptNumber" label="ReceiptNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptReceivingNumber" label="ReceiptReceivingNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="purchaseOrderNumber" label="PurchaseOrderNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="externReceiptNumber" label="ExternReceiptNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="supplierId" label="SupplierId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="supplierName" label="SupplierName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseName" label="WarehouseName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptReceivingStartTime" label="ReceiptReceivingStartTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptReceivingEndTime" label="ReceiptReceivingEndTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptReceivingStatus" label="ReceiptReceivingStatus" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receiptReceivingType" label="ReceiptReceivingType" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="completeTime" label="CompleteTime" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="remark" label="Remark" fixed="" show-overflow-tooltip="" />
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
         <el-table-column prop="dateTime5" label="DateTime5" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int1" label="Int1" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int2" label="Int2" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int3" label="Int3" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int4" label="Int4" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="int5" label="Int5" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('mMSReceiptReceiving:edit') || auth('mMSReceiptReceiving:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditMMSReceiptReceiving(scope.row)" v-auth="'mMSReceiptReceiving:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delMMSReceiptReceiving(scope.row)" v-auth="'mMSReceiptReceiving:delete'"> 删除 </el-button>
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
			    :title="editMMSReceiptReceivingTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="mMSReceiptReceiving">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/mMSReceiptReceiving/component/editDialog.vue'
  import { pageMMSReceiptReceiving, deleteMMSReceiptReceiving } from '/@/api/main/mMSReceiptReceiving';


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
        const editMMSReceiptReceivingTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageMMSReceiptReceiving(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddMMSReceiptReceiving = () => {
        editMMSReceiptReceivingTitle.value = '添加MMSReceiptReceiving';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditMMSReceiptReceiving = (row: any) => {
        editMMSReceiptReceivingTitle.value = '编辑MMSReceiptReceiving';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delMMSReceiptReceiving = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteMMSReceiptReceiving(row);
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


