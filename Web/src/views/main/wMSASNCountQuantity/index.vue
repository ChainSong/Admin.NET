<template>
  <div class="wMSASNCountQuantity-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">

        <el-form-item label="ASN单号">
          <el-input v-model="queryParams.aSNNumber" clearable="" placeholder="请输入ASN单号" />
        </el-form-item>
        <el-form-item label="点数单号">
          <el-input v-model="queryParams.aSNCountQuantityNumber" clearable="" placeholder="请输入点数单号" />
        </el-form-item>
        <el-form-item label="外部单号">
          <el-input v-model="queryParams.externReceiptNumber" clearable="" placeholder="请输入外部单号" />
        </el-form-item>
        <el-form-item label="客户名称">
          <el-input v-model="queryParams.customerName" clearable="" placeholder="请输入客户名称" />
        </el-form-item>

        <el-form-item label="仓库名称">
          <el-input v-model="queryParams.warehouseName" clearable="" placeholder="请输入仓库名称" />
        </el-form-item>



        <el-form-item label="点数状态">
          <!-- <el-input v-model="queryParams.ASNCountQuantityStatus" clearable="" placeholder="请输入仓库名称"/> -->
          <el-select v-model="queryParams.ASNCountQuantityStatus" placeholder="请选择">
            <el-option  v-for="item in state.asnCountQuantityoptions" :key="item.value" :label="item.label"
              :value="item.value">
            </el-option>
          </el-select>

        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSASNCountQuantity:page'"> 查询
            </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>

          </el-button-group>

        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSASNCountQuantity"
            v-auth="'wMSASNCountQuantity:add'"> 新增 </el-button>

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <el-table-column type="index" label="序号" width="55" align="center" />
        <el-table-column prop="asnNumber" label="ASN单号" />
        <el-table-column prop="asnCountQuantityNumber" label="点数单号" show-overflow-tooltip="" />
        <el-table-column prop="externReceiptNumber" label="外部单号" show-overflow-tooltip="" />
        <el-table-column prop="customerName" label="客户名称" show-overflow-tooltip="" />
        <el-table-column prop="warehouseName" label="仓库名称" show-overflow-tooltip="" />
        <el-table-column prop="expectDate" label="入库时间" show-overflow-tooltip="" />
        <!-- <el-table-column prop="aSNCountQuantityStatus" label="点数状态"  show-overflow-tooltip="" /> -->
        <el-table-column prop="receiptType" label="入库类型" show-overflow-tooltip="" />
        <el-table-column prop="creator" label="点数人" show-overflow-tooltip="" />
        <el-table-column prop="creationTime" label="创建时间" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip=""
          v-if="auth('wMSASNCountQuantity:edit') || auth('wMSASNCountQuantity:delete')">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary"
              @click="openQueryWMSASNCountQuantity(scope.row)" v-auth="'wMSASNCountQuantity:edit'"> 查看 </el-button>
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSASNCountQuantity(scope.row)"
              v-auth="'wMSASNCountQuantity:delete'"> 删除 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editWMSASNCountQuantityTitle" @reloadTable="handleQuery" />
      <queryDialog ref="queryDialogRef" :title="queryWMSASNCountQuantityTitle" @reloadTable="handleQuery" />

    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSASNCountQuantity">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSASNCountQuantity/component/editDialog.vue'
import queryDialog from '/@/views/main/wMSASNCountQuantity/component/queryDialog.vue'
import { pageWMSASNCountQuantity, deleteWMSASNCountQuantity } from '/@/api/main/wMSASNCountQuantity';


const state = ref({
  vm: {
    id: "",

    // form: {
    //     customerDetails: []
    // } as any,
  },
  asnCountQuantityoptions: [{"label": "新增", "value": "1"}, {"label": "完成", "value": "99"}],
});
const editDialogRef = ref();
const queryDialogRef = ref();
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
const editWMSASNCountQuantityTitle = ref("");
const queryWMSASNCountQuantityTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSASNCountQuantity(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddWMSASNCountQuantity = () => {
  editWMSASNCountQuantityTitle.value = '添加入库点数';
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openQueryWMSASNCountQuantity  = (row: any) => {
  queryWMSASNCountQuantityTitle.value = '查看';
  queryDialogRef.value.openDialog(row);
};
// 打开编辑页面
const openEditWMSASNCountQuantity = (row: any) => {
  editWMSASNCountQuantityTitle.value = '编辑';
  editDialogRef.value.openDialog(row);
};

// 删除
const delWMSASNCountQuantity = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSASNCountQuantity(row);
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
