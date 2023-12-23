<template>
  <div class="wMSRFReceiptAcquisition-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="state.vm.form" ref="queryForm" :inline="true">
        <el-form-item label="外部单号">
          <el-input v-model="state.vm.form.externReceiptNumber" clearable="" placeholder="请输入外部单号" />
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" > 查询
            </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSRFReceiptAcquisition"
            v-auth="'wMSRFReceiptAcquisition:add'"> 新增 </el-button>

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="state.vm.data" style="width: 100%" v-loading="loading" tooltip-effect="light" row-key="id" border="">
        <!-- <el-table-column type="index" label="序号" width="55"/> -->
        <el-table-column prop="asnNumber" label="预入库单号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="receiptNumber" label="入库单号" fixed="" show-overflow-tooltip="" />
        <el-table-column prop="externReceiptNumber" label="外部单号" fixed="" show-overflow-tooltip="" />
        <el-table-column label="操作" width="140" align="center" fixed="right" show-overflow-tooltip="">
          <template #default="scope">
            <el-button icon="ele-Edit" size="small" text="" type="primary"
              @click="openEditWMSRFReceiptAcquisition(scope.row)"> 采集 </el-button>
          </template>
        </el-table-column>
      </el-table>
      <!-- <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" /> -->
      <editDialog ref="editDialogRef" :title="editWMSRFReceiptAcquisitionTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSRFReceiptAcquisition">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSRFReceiptAcquisition/component/editDialog.vue'
import { pageWMSRFReceiptAcquisition, deleteWMSRFReceiptAcquisition,allWMSRFReceiptAcquisition } from '/@/api/main/wMSRFReceiptAcquisition';


const editDialogRef = ref();
const loading = ref(false);
const tableData = ref<any> ([]);

const editWMSRFReceiptAcquisitionTitle = ref("");


const state = ref({
  vm: {
    id: "",
    form: {} as any,
    data: [] as any,
  },
  visible: false,
  loading: false,
});


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await allWMSRFReceiptAcquisition(state.value.vm.form);
  console.log(res)
  state.value.vm.data = res.data.result??[];
  // tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
// const openAddWMSRFReceiptAcquisition = () => {
//   editWMSRFReceiptAcquisitionTitle.value = '添加';
//   editDialogRef.value.openDialog({});
// };

// 打开编辑页面
const openEditWMSRFReceiptAcquisition = (row: any) => {
  editWMSRFReceiptAcquisitionTitle.value = '采集';
  editDialogRef.value.openDialog(row);
};

// 删除
// const delWMSRFReceiptAcquisition = (row: any) => {
//   ElMessageBox.confirm(`确定要删除吗?`, "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       await deleteWMSRFReceiptAcquisition(row);
//       handleQuery();
//       ElMessage.success("删除成功");
//     })
//     .catch(() => { });
// };

// 改变页面容量
const handleSizeChange = (val: number) => {
  // tableParams.value.pageSize = val;
  handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
  // tableParams.value.page = val;
  handleQuery();
};


handleQuery();
</script>


