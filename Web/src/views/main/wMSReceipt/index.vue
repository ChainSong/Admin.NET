<template>
  <div class="wMSReceipt-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="[16, 15]">
          <template v-for="i in state.tableColumnHeaders">
            <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6" v-if="i.isSearchCondition" :key="i">
              <template v-if="i.type == 'TextBox'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" />
                </el-form-item>
              </template>
              <template v-if="i.type == 'DropDownListInt'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" clearable filterable v-if="i.isSearchCondition"
                    size="small" placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt" style="width: 100%"
                      :label="item.name" :value="item.codeInt">
                    </el-option>
                  </el-select>

                </el-form-item>

              </template>
              <template v-if="i.type == 'DropDownListStrRemote'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <select-Remote :whereData="state.header" :isDisabled="i.isSearchCondition" :columnData="i"
                    :defaultvValue="state.header[i.columnName]"
                    @select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>
                </el-form-item>
              </template>


              <template v-if="i.type == 'DropDownListStr'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" clearable filterable v-if="i.isSearchCondition"
                    size="small" placeholder="请选择">
                    <el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr" style="width: 100%"
                      :label="item.name" :value="item.codeStr">
                    </el-option>
                  </el-select>
                </el-form-item>
              </template>
              <template v-if="i.type == 'DatePicker'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-date-picker v-model="state.header[i.columnName]" type="daterange" size="small"
                    v-if="i.isSearchCondition" range-separator="~" start-placeholder="开始日期" end-placeholder="结束日期"
                    style="width: 100%">
                  </el-date-picker>
                </el-form-item>
              </template>
              <template v-if="i.type == 'DateTimePicker'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-date-picker v-model="state.header[i.columnName]" v-if="i.isSearchCondition" size="small"
                    type="datetimerange" range-separator="~" start-placeholder="开始日期" end-placeholder="结束日期"
                    style="width: 100%">
                  </el-date-picker>
                </el-form-item>
              </template>
            </el-col>
          </template>
        </el-row>

        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSReceipt:page'"> 查询 </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Upload" @click="exportReceipts" v-auth="'wMSReceipt:receipt'"> 导出入库信息
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Upload" @click="exportReceiptReceivingfun"
            v-auth="'wMSReceipt:receiptReceiving'">
            导出上架信息
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Printer" @click="openPrint" v-auth="'wMSReceipt:Print'"> 打印
          </el-button>
          <el-button type="primary" icon="ele-Fold" @click="printRFID" v-auth="'wMSReceipt:printRFID'">
            打印RFID</el-button>

        </el-form-item>

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">

      <el-table :data="state.headers" show-overflow-tooltip ref="multipleTableRef" tooltip-effect="light" row-key="id"
        style="width: 100%">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <template v-for="v in state.tableColumnHeaders">
          <template v-if="v.isShowInList">
            <el-table-column v-if="v.type == 'DropDownListInt'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
              <template #default="scope">
                <template v-for="item in v.tableColumnsDetails">
                  <el-tag v-if="item.codeInt == state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr"
                    show-icon :type="item.color">
                    {{ item.name }}
                  </el-tag>
                </template>
              </template>
            </el-table-column>
            <el-table-column v-else-if="v.type == 'DropDownListStr'" v-bind:key="v.columnName" :fixed="false"
              :prop="v.columnName" :label="v.displayName" width="150" max-height="50">
              <template #default="scope">
                <template v-for="item in v.tableColumnsDetails">
                  <el-tag v-if="item.codeStr == state.headers[scope.$index][v.columnName]" v-bind:key="item.codeStr"
                    show-icon :type="item.color">
                    {{ item.name }}
                  </el-tag>
                </template>
              </template>
            </el-table-column>
            <el-table-column v-else v-bind:key="v.id" :fixed="false" :prop="v.columnName" :label="v.displayName"
              width="150" max-height="50">
            </el-table-column>
          </template>
        </template>
        <el-table-column fixed="right" label="操作" width="200">
          <template #header>
            <el-select placeholder="请选择">
              <template v-for="item in state.tableColumnHeaders">
                <el-option v-if="item.isShowInList == 1" :key="item.value">
                  <el-checkbox @change="checked => showColumnOption(checked, item)" :true-label="1" :false-label="0"
                    :label="item.displayName" :key="item.columnName" v-model="item.isShowInList">{{
                      item.displayName }}</el-checkbox>
                </el-option>
              </template>
            </el-select>
          </template>
          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button @click="del(scope.row)" class="el-icon-delete" type="text" size="small">删除
            </el-button>

          </template>
        </el-table-column>
      </el-table>

      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" />
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="handleQuery" />
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="wMSReceipt">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSReceipt/component/editDialog.vue'
import addDialog from '/@/views/main/wMSReceipt/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSReceipt/component/queryDialog.vue'
import { pageWMSReceipt, deleteWMSReceipt, exportReceipt, exportReceiptReceiving, getReceipts } from '/@/api/main/wMSReceipt';
import { getPrinrRFIDInfoByReceiptId } from '/@/api/main/wMSRFIDInfo';
import { getByTableNameList } from "/@/api/main/tableColumns";
import printDialog from '/@/views/tools/printDialog.vue';
import selectRemote from '/@/views/tools/select-remote.vue'
import Header from "/@/entities/receipt";
import Details from "/@/entities/receiptDetail";
import TableColumns from "/@/entities/tableColumns";
// import { number } from "echarts";
import { downloadByData, getFileName } from '/@/utils/download';
import { stringify } from "querystring";
import { signalR } from '/@/utils/signalRCustom';
const state = ref({
  vm: {
    id: "",

    // form: {
    //     customerDetails: []
    // } as any,
  },
  visible: false,
  loading: false,
  header: new Header(),
  headers: new Array<Header>(),
  details: new Array<Details>(),
  // header: new Array<Details>(),

  tableColumnHeader: new TableColumns(),
  tableColumnHeaders: new Array<TableColumns>(),
  tableColumnDetail: new TableColumns(),
  tableColumnDetails: new Array<TableColumns>()

  // tableColumn: new TableColumns(),
  // tableColumns: new Array<TableColumns>(),
  // tableColumnsDetails: new Array<TableColumnsDetails>(),
  //   tableColumnsDetail = ref();
});

const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
// const tableData = ref<any>
// ([]);
const queryParams = ref<any>
  ({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const editTitle = ref("");
const addTitle = ref("");
const queryTitle = ref("");

let ids = ref(Array<Number>);
const ptintTitle = ref("");
const printDialogRef = ref();

// 页面加载时
onMounted(async () => {
  gettableColumn();
  // signalR.on('Echo', (data: any) => {
  //   console.log("WebSocket data");
  //   console.log(data);
  // });
});
const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_Receipt");
  state.value.tableColumnHeaders = res.data.result;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSReceipt(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
const openAdd = () => {
  addTitle.value = '添加';
  addDialogRef.value.openDialog({});
};

const showColumnOption = async (value: any, item: any) => {
  if (value == 1) {
    item.isShowInList = 1;
  } else {
    item.isShowInList = 0;
  }
};

// 打开编辑页面
const openEdit = (row: any) => {
  editTitle.value = '编辑';
  editDialogRef.value.openDialog(row);
};
// 打开查询页面
const openQuery = (row: any) => {
  queryTitle.value = '查看';
  queryDialogRef.value.openDialog(row);
};

// 删除
const del = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let res = await deleteWMSReceipt(row);
      if (res.data.result.code == "1") {
        ElMessage.success(res.data.result.msg);
      } else {
        ElMessage.error(res.data.result.msg);
      }
      handleQuery();
      // ElMessage.success("删除成功");
    })
    .catch(() => { });
};

// 打开转入库单页面
const printRFID = (row: any) => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // 判断是否勾选订单
  if (ids.length < 1) {
    ElMessage.error("请勾选订单");
    return;
  }
  ElMessageBox.confirm(`确定要打印RFID吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let result = await getPrinrRFIDInfoByReceiptId(ids);
      console.log("result");
      console.log(result);
      if (result.data.result.code == 1) {
        await signalR.send("Echo", result.data.result.data);
        ElMessage.success("打印成功");
      } else {
        ElMessage.success("打印失败");
      }
    })
    .catch(() => { });
};

// 改变页面容量
const handleSizeChange = (val: number) => {
  tableParams.value.pageSize = val;
  handleQuery();
};


const exportReceipts = async () => {

  //1 获取选中的订单ID
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // 2,验证数据有没有勾选
  // if (ids.length < 1) {
  //   ElMessage.error("请勾选订单");
  //   return;
  // }
  if (ids.length > 0) {
    let res = await exportReceipt({ "ids": ids });
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  }else{
    let res = await exportReceipt(state.value.header);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  }
};

//导出上架单
const exportReceiptReceivingfun = async () => {
  //1 获取选中的订单ID
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  // 2,验证数据有没有勾选
  if (ids.length < 1) {
    ElMessage.error("请勾选订单");
    return;
  }
  let res = await exportReceiptReceiving(ids);
  var fileName = getFileName(res.headers);
  downloadByData(res.data as any, fileName);
}


//-----------------打印上架--------------------------------------------------------- 

// 打开打印询页面
const openPrint = async () => {
  ptintTitle.value = '上架单打印';
  ids.value = new Array<Number>();

  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.value.push(a.id);
  });
  if (ids.value.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  let printData = new Array<Header>();
  let result = await getReceipts(ids.value);
  // console.log("result");
  // console.log(result);
  if (result.data.result != null) {
    printData = result.data.result;
    // state.value.details = result.data.result.details;
  }
  printDialogRef.value.openDialog({ "printData": printData, "templateName": "上架单打印模板" });
};


//打印
// const printOrder = async () => {

//   //修改打印的时间和打印次数
//   await addWMSPickTaskPrintLog(ids.value);
//   handleQuery();
// };



// 改变页码序号
const handleCurrentChange = (val: number) => {
  tableParams.value.page = val;
  handleQuery();
};


handleQuery();
</script>
