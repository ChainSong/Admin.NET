<template>
  <div class="wMSPickTask-container">
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
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSPickTask:page'"> 查询
            </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Printer" @click="openPrint" v-auth="'wMSPickTask:page'"> 打印
            </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>

        <!-- <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Printer" @click="openPrint" v-auth="'wMSPickTask:page'"> 打印
            </el-button>
          </el-button-group>
        </el-form-item> -->
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSPickTask:add'"> 新增
          </el-button>
        </el-form-item> -->
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="PickTaskForOrderFun" v-auth="'wMSPickTask:PickTaskForOrder'"> 转出库单
          </el-button>
        </el-form-item> -->

      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">

      <el-table :data="state.headers" ref="multipleTableRef" show-overflow-tooltip tooltip-effect="light" row-key="id"
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
        <el-table-column fixed="right" label="操作" width="250">

          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <!-- <el-button @click="openPrint(scope.row)" class="el-icon-s-comment" type="text" size="small">打印
            </el-button> -->
            <el-button @click="complete(scope.row)" class="el-icon-success" type="text" size="small">完成
            </el-button>
            <el-button @click="pickTaskReturn(scope.row)" class="el-icon-delete" type="text" size="small">回退
            </el-button>
            <!-- <el-button @click="openEdit(scope.row)" class="el-icon-edit" type="text" size="small">编辑</el-button> -->
            <!--   <el-popconfirm confirm-button-text="确定"  cancel-button-text="取消"
              icon="el-icon-info" icon-color="red" @confirm="handleDelete(scope.row)" title="确定删除吗？">
              <el-button   type="text" class="el-icon-delete" style="color:#F56C6C;margin-left: 10px;"
                size="small">删除</el-button>
            </el-popconfirm> -->

          </template>
        </el-table-column>
      </el-table>

      <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <!-- <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" /> -->
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="printOrder" />

    </el-card>

    <!-- <el-dialog v-model="resultPopupShow" title="转入库单结果" :append-to-body="true">
    <el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg" :type="i.statusMsg">
    </el-alert>
  </el-dialog> -->
  </div>
</template>

<script lang="ts" setup="" name="wMSPickTask">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSPickTask/component/editDialog.vue'
import addDialog from '/@/views/main/wMSPickTask/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSPickTask/component/queryDialog.vue'
// import printDialog from '/@/views/main/wMSPickTask/component/printDialog.vue'
import { pageWMSPickTask, deleteWMSPickTask, wmsPickComplete, getPickTasks, addWMSPickTaskPrintLog } from '/@/api/main/wMSPickTask';
import { getByTableNameList } from "/@/api/main/tableColumns";
import printDialog from '/@/views/tools/printDialog.vue';
import selectRemote from '/@/views/tools/select-remote.vue';
import Header from "/@/entities/pickTask";
import Details from "/@/entities/pickTaskDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import pickTask from "/@/entities/pickTask";


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
  tableColumnDetails: new Array<TableColumns>(),
  //自定义提示
  orderStatus: new Array<orderStatus>(),


  // tableColumn: new TableColumns(),
  // tableColumns: new Array<TableColumns>(),
  // tableColumnsDetails: new Array<TableColumnsDetails>(),
  //   tableColumnsDetail = ref();
});

const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const printDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
//自定义提示
const resultPopupShow = ref(false);
// const tableData = ref<any>
// ([]);
const queryParams = ref<any>({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
// const editTitle = ref("");
let ids = ref(Array<Number>);
const ptintTitle = ref("");
const queryTitle = ref("");


// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_PickTask");
  state.value.tableColumnHeaders = res.data.result;
};

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSPickTask(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};


const pickTaskReturn = async (row: any) => {
  ElMessageBox.confirm(`确定要回退拣货吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSPickTask(row);
      handleQuery();
      ElMessage.success("回退完成");
    })
    .catch(() => { });
  // loading.value = true;

  // state.value.headers = res.data.result?.items ?? [];
  // tableParams.value.total = res.data.result?.total;
  // loading.value = false;
};


const complete = async (row: any) => {
  ElMessageBox.confirm(`确定要完成拣货吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await wmsPickComplete(row);
      handleQuery();
      ElMessage.success("拣货完成");
    })
    .catch(() => { });
  // loading.value = true;

  // state.value.headers = res.data.result?.items ?? [];
  // tableParams.value.total = res.data.result?.total;
  // loading.value = false;
};



// // 打开新增页面
// const openAdd = () => {
// addTitle.value = '添加';
// addDialogRef.value.openDialog({});
// };

// // 打开编辑页面
// const openEdit = (row: any) => {

// if (row.PickStatus != 1) {
//   ElMessage.warning("订单状态不允许编辑");
//   return;
// }
// editTitle.value = '编辑';
// editDialogRef.value.openDialog(row);
// };
// 打开查询页面
const openQuery = (row: any) => {
  queryTitle.value = '查看';
  queryDialogRef.value.openDialog(row);
};

// 打开打印询页面
const openPrint = async () => {
  ptintTitle.value = '拣货单打印';
  ids.value = new Array<Number>();

  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.value.push(a.id);
  });
  if (ids.value.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }

  let printData = new Array<Header>();
  var flag = 0;
  //判断列表中有没有打印次数大于0的数据
  multipleTableRef.value.getSelectionRows().forEach(a => {
    if (a.printNum > 0) {
      flag = 1;
    }
  })
  if (flag == 0) {
    let result = await getPickTasks(ids.value);
    if (result.data.result != null) {
      printData = result.data.result;
    }
    printDialogRef.value.openDialog({ "printData": printData, "templateName": "拣货单打印模板" });
  } else {
    ElMessageBox.confirm(`存在已经打印过的拣货单，是否继续打印?`, "提示", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning",
    })
      .then(async () => {
        let result = await getPickTasks(ids.value);
        if (result.data.result != null) {
          printData = result.data.result;
        }
        printDialogRef.value.openDialog({ "printData": printData, "templateName": "拣货单打印模板" });
      })
      .catch(() => { });
  }


};



//打印
const printOrder = async () => {
  // let ids = new Array<Number>();
  // multipleTableRef.value.getSelectionRows().forEach(a => {
  //     ids.push(a.id);
  // });
  // if (ids.length == 0) {
  //   ElMessage.error("请勾选需要打印的订单");
  //   return;
  // }
  //修改打印的时间和打印次数
  await addWMSPickTaskPrintLog(ids.value);
  handleQuery();
};
// 删除
const del = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSPickTask(row);
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

<style scoped>
.el-table .el-checkbox {
  transform: scale(3.5);
  /* 调整复选框的缩放比例 */
  margin-right: 2px;
  /* 调整复选框与文字的间距 */
}

::v-deep(.el-table .el-checkbox__input) {
  height: 24px;
  /* 调整复选框的高度 */
  width: 24px;
  /* 调整复选框的宽度 */
}
</style>
