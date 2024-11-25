<template>
  <div class="wMSASNReceiptDetail-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="Id">
          <el-input v-model="queryParams.id" clearable="" placeholder="请输入Id"/>
          
        </el-form-item>
        <el-form-item label="ASNId">
          <el-input v-model="queryParams.aSNId" clearable="" placeholder="请输入ASNId"/>
          
        </el-form-item>
        <el-form-item label="ASNDetailId">
          <el-input v-model="queryParams.aSNDetailId" clearable="" placeholder="请输入ASNDetailId"/>
          
        </el-form-item>
        <el-form-item label="ASNNumber">
          <el-input v-model="queryParams.aSNNumber" clearable="" placeholder="请输入ASNNumber"/>
          
        </el-form-item>
        <el-form-item label="ExternReceiptNumber">
          <el-input v-model="queryParams.externReceiptNumber" clearable="" placeholder="请输入ExternReceiptNumber"/>
          
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
        <el-form-item label="LineNumber">
          <el-input v-model="queryParams.lineNumber" clearable="" placeholder="请输入LineNumber"/>
          
        </el-form-item>
        <el-form-item label="SKU">
          <el-input v-model="queryParams.sKU" clearable="" placeholder="请输入SKU"/>
          
        </el-form-item>
        <el-form-item label="UPC">
          <el-input v-model="queryParams.uPC" clearable="" placeholder="请输入UPC"/>
          
        </el-form-item>
        <el-form-item label="GoodsType">
          <el-input v-model="queryParams.goodsType" clearable="" placeholder="请输入GoodsType"/>
          
        </el-form-item>
        <el-form-item label="GoodsName">
          <el-input v-model="queryParams.goodsName" clearable="" placeholder="请输入GoodsName"/>
          
        </el-form-item>
        <el-form-item label="BoxCode">
          <el-input v-model="queryParams.boxCode" clearable="" placeholder="请输入BoxCode"/>
          
        </el-form-item>
        <el-form-item label="TrayCode">
          <el-input v-model="queryParams.trayCode" clearable="" placeholder="请输入TrayCode"/>
          
        </el-form-item>
        <el-form-item label="BatchCode">
          <el-input v-model="queryParams.batchCode" clearable="" placeholder="请输入BatchCode"/>
          
        </el-form-item>
        <el-form-item label="LotCode">
          <el-input v-model="queryParams.lotCode" clearable="" placeholder="请输入LotCode"/>
          
        </el-form-item>
        <el-form-item label="PoCode">
          <el-input v-model="queryParams.poCode" clearable="" placeholder="请输入PoCode"/>
          
        </el-form-item>
        <el-form-item label="Weight">
          <el-input v-model="queryParams.weight" clearable="" placeholder="请输入Weight"/>
          
        </el-form-item>
        <el-form-item label="Volume">
          <el-input v-model="queryParams.volume" clearable="" placeholder="请输入Volume"/>
          
        </el-form-item>
        <el-form-item label="ExpectedQty">
          <el-input v-model="queryParams.expectedQty" clearable="" placeholder="请输入ExpectedQty"/>
          
        </el-form-item>
        <el-form-item label="ReceivedQty">
          <el-input v-model="queryParams.receivedQty" clearable="" placeholder="请输入ReceivedQty"/>
          
        </el-form-item>
        <el-form-item label="Qty">
          <el-input v-model="queryParams.qty" clearable="" placeholder="请输入Qty"/>
          
        </el-form-item>
        <el-form-item label="UnitCode">
          <el-input v-model="queryParams.unitCode" clearable="" placeholder="请输入UnitCode"/>
          
        </el-form-item>
        <el-form-item label="Onwer">
          <el-input v-model="queryParams.onwer" clearable="" placeholder="请输入Onwer"/>
          
        </el-form-item>
        <el-form-item label="ProductionDate">
          <el-date-picker placeholder="请选择ProductionDate" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.productionDateRange" />
          
        </el-form-item>
        <el-form-item label="ExpirationDate">
          <el-date-picker placeholder="请选择ExpirationDate" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.expirationDateRange" />
          
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
            <el-button type="primary"  icon="ele-Search" @click="handleQuery" v-auth="'wMSASNReceiptDetail:page'"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
            
          </el-button-group>
          
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSASNReceiptDetail" v-auth="'wMSASNReceiptDetail:add'"> 新增 </el-button>
          
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
         <el-table-column prop="aSNId" label="ASNId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="aSNDetailId" label="ASNDetailId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="aSNNumber" label="ASNNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="externReceiptNumber" label="ExternReceiptNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerId" label="CustomerId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="customerName" label="CustomerName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseId" label="WarehouseId" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="warehouseName" label="WarehouseName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="lineNumber" label="LineNumber" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="sKU" label="SKU" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="uPC" label="UPC" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="goodsType" label="GoodsType" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="goodsName" label="GoodsName" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="boxCode" label="BoxCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="trayCode" label="TrayCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="batchCode" label="BatchCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="lotCode" label="LotCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="poCode" label="PoCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="weight" label="Weight" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="volume" label="Volume" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expectedQty" label="ExpectedQty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="receivedQty" label="ReceivedQty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="qty" label="Qty" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="unitCode" label="UnitCode" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="onwer" label="Onwer" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="productionDate" label="ProductionDate" fixed="" show-overflow-tooltip="" />
         <el-table-column prop="expirationDate" label="ExpirationDate" fixed="" show-overflow-tooltip="" />
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
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="" v-if="auth('wMSASNReceiptDetail:edit') || auth('wMSASNReceiptDetail:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSASNReceiptDetail(scope.row)" v-auth="'wMSASNReceiptDetail:edit'"> 编辑 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSASNReceiptDetail(scope.row)" v-auth="'wMSASNReceiptDetail:delete'"> 删除 </el-button>
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
			    :title="editWMSASNReceiptDetailTitle"
			    @reloadTable="handleQuery"
      />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSASNReceiptDetail">
  import { ref } from "vue";
  import { ElMessageBox, ElMessage } from "element-plus";
  import { auth } from '/@/utils/authFunction';
  //import { formatDate } from '/@/utils/formatTime';

  import editDialog from '/@/views/main/wMSASNReceiptDetail/component/editDialog.vue'
  import { pageWMSASNReceiptDetail, deleteWMSASNReceiptDetail } from '/@/api/main/wMSASNReceiptDetail';


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
        const editWMSASNReceiptDetailTitle = ref("");


        // 查询操作
        const handleQuery = async () => {
        loading.value = true;
        var res = await pageWMSASNReceiptDetail(Object.assign(queryParams.value, tableParams.value));
        tableData.value = res.data.result?.items ?? [];
        tableParams.value.total = res.data.result?.total;
        loading.value = false;
        };

        // 打开新增页面
        const openAddWMSASNReceiptDetail = () => {
        editWMSASNReceiptDetailTitle.value = '添加入库点数';
        editDialogRef.value.openDialog({});
        };

        // 打开编辑页面
        const openEditWMSASNReceiptDetail = (row: any) => {
        editWMSASNReceiptDetailTitle.value = '编辑入库点数';
        editDialogRef.value.openDialog(row);
        };

        // 删除
        const delWMSASNReceiptDetail = (row: any) => {
        ElMessageBox.confirm(`确定要删除吗?`, "提示", {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        })
        .then(async () => {
        await deleteWMSASNReceiptDetail(row);
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


