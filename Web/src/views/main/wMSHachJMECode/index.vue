<template>
  <div class="wMSHachJMECode-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" :inline="true">

        <el-form-item label="产品名称">
          <el-input v-model="queryParams.sKU" clearable="" placeholder="请输入产品名称" />

        </el-form-item>
        <!-- <el-form-item label="GoodsName">
          <el-input v-model="queryParams.goodsName" clearable="" placeholder="请输入GoodsName"/>
          
        </el-form-item> -->
        <el-form-item label="产品校验码">
          <el-input v-model="queryParams.jMECode" clearable="" placeholder="请输入产品校验码" />

        </el-form-item>
        <!-- <el-form-item label="QRCode">
          <el-input v-model="queryParams.qRCode" clearable="" placeholder="请输入QRCode"/>
        </el-form-item>
        <el-form-item label="Url">
          <el-input v-model="queryParams.url" clearable="" placeholder="请输入Url"/>
          
        </el-form-item> -->
        <!-- <el-form-item label="JMEData">
          <el-input v-model="queryParams.jMEData" clearable="" placeholder="请输入JMEData"/>
          
        </el-form-item> -->
        <el-form-item label="打印次数">
          <el-input-number v-model="queryParams.printNum" clearable="" placeholder="请输入打印次数" />

        </el-form-item>
        <!-- <el-form-item label="Creator">
          <el-input v-model="queryParams.creator" clearable="" placeholder="请输入Creator"/>
          
        </el-form-item>
        <el-form-item label="CreationTime">
          <el-date-picker placeholder="请选择CreationTime" value-format="YYYY/MM/DD" type="daterange" v-model="queryParams.creationTimeRange" />
          
        </el-form-item>
        <el-form-item label="Updator">
          <el-input v-model="queryParams.updator" clearable="" placeholder="请输入Updator"/>
          
        </el-form-item> -->
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery"> 查询 </el-button>
            <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAddWMSHachJMECode"> 新增 </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Fold" @click="printJWEFun"> 打印 </el-button>
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <el-table :data="tableData" ref="multipleTableRef" style="width: 100%" v-loading="loading" tooltip-effect="light"
        row-key="id" border="">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column type="index" label="序号" width="55" />
        <el-table-column prop="sku" label="产品编码" />
        <!-- <el-table-column prop="goodsName" label="GoodsName"   /> -->
        <el-table-column prop="jmeCode" label="产品校验码" />
        <el-table-column prop="qrCode" label="二维码数据" />
        <!-- <el-table-column prop="url" label="Url"  /> -->
        <!-- <el-table-column prop="jMEData" label="JMEData"   /> -->
        <el-table-column prop="printNum" label="打印次数" />
        <el-table-column prop="creator" label="创建人" />
        <el-table-column prop="creationTime" label="创建时间" f />
        <!-- <el-table-column prop="updator" label="Updator" fixed="" show-overflow-tooltip="" /> -->
        <el-table-column label="操作" width="140" fixed="right">
          <template #default="scope">
            <!-- <el-button icon="ele-Edit" size="small" text="" type="primary" @click="openEditWMSHachJMECode(scope.row)" v-auth="'wMSHachJMECode:edit'"> 编辑 </el-button> -->
            <el-button icon="ele-Delete" size="small" text="" type="primary" @click="delWMSHachJMECode(scope.row)"> 删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100, 500, 1000]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editWMSHachJMECodeTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSHachJMECode">
import { ref } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';
import printDialog from '/@/views/tools/printDialog.vue';
import editDialog from '/@/views/main/wMSHachJMECode/component/editDialog.vue'
import { pageWMSHachJMECode, deleteWMSHachJMECode, printJME } from '/@/api/main/wMSHachJMECode';
const printDialogRef = ref();
const multipleTableRef = ref();
const editDialogRef = ref();
const loading = ref(false);
const ptintTitle = ref("");
const tableData = ref<any>
  ([]);
const queryParams = ref<any>
  ({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editWMSHachJMECodeTitle = ref("");


// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSHachJMECode(Object.assign(queryParams.value, tableParams.value));
  tableData.value = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAddWMSHachJMECode = () => {
  editWMSHachJMECodeTitle.value = '添加JME打印';
  editDialogRef.value.openDialog({});
};

// 打开编辑页面
const openEditWMSHachJMECode = (row: any) => {
  editWMSHachJMECodeTitle.value = '编辑JME打印';
  editDialogRef.value.openDialog(row);
};

// 删除
const delWMSHachJMECode = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSHachJMECode(row);
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

//打印箱唛
const printJWEFun = async () => {

  ptintTitle.value = '打印';
  var packageNumbers = new Array<any>();

  multipleTableRef.value.getSelectionRows().forEach(a => {
    // console.log("a");
    // console.log(a);
    packageNumbers.push(a.id);
  });

  if (packageNumbers.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }

  ElMessageBox.confirm("是否要打印？", "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      console.log("idsasas");
      let printData = {};
      console.log("printData");
      printData.printTemplate = "JME打印";
      console.log("ids", packageNumbers);
      let result = await printJME(packageNumbers);
      console.log("result", result);
      if (result.data.result != null) {
        printData.data = result.data.result.data.data;
      }
      printData.printTemplate = result.data.result.data.printTemplate;
      // console.log("packageNumbers", packageNumbers);
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    })
    .catch(() => { });
};


handleQuery();
</script>
