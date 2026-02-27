<template>
  <div class="wMSOrder-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="[16, 15]">
          <template v-for="i in state.tableColumnHeaders">
            <el-col :xs="24" :sm="12" :md="8" :lg="6" :xl="6" v-if="i.isSearchCondition" :key="i">

              <template v-if="i.type == 'TextBox'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-input v-model="state.header[i.dbColumnName]" :placeholder="i.displayName" style="width: 100%" size="small" clearable />
                </el-form-item>
              </template>
              <template v-if="i.type == 'DropDownListInt'">
                <el-form-item class="mb-0" :label="i.displayName">
                  <el-select v-model="state.header[i.columnName]" clearable filterable v-if="i.isSearchCondition"
                    size="small" placeholder="请选择" style="width: 100%">
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
                    size="small" placeholder="请选择" style="width: 100%">
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
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSOrder:page'"> 查询 </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>

        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Download" @click="exportOrderFun" v-auth="'wMSOrder:export'" :loading="opLoading.exportOrder" :disabled="opLoading.exportOrder"> 导出出库单
            </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>

        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Download" @click="exportPackageFun" v-auth="'wMSOrder:export'" :loading="opLoading.exportPackage" :disabled="opLoading.exportPackage"> 导出包装
            </el-button>
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>
        </el-form-item>
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSOrder:add'"> 新增
          </el-button>
        </el-form-item> -->

        <el-form-item>
          <el-button type="primary" icon="ele-Share" @click="automatedAllocationFun" v-auth="'wMSOrder:allocation'" :loading="opLoading.automatedAllocation" :disabled="opLoading.automatedAllocation">
            分配库存
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="createPickTaskFun" v-auth="'wMSOrder:pickTask'" :loading="opLoading.createPickTask" :disabled="opLoading.createPickTask"> 拣货任务
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="completeOrderFun" v-auth="'wMSOrder:completeOrder'" :loading="opLoading.completeOrder" :disabled="opLoading.completeOrder"> 完成
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="openPrint" v-auth="'wMSOrder:page'" :loading="opLoading.print" :disabled="opLoading.print"> 打印
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="openPrintJob" v-auth="'wMSOrder:page'" :loading="opLoading.printJob" :disabled="opLoading.printJob"> 打印JOB汇总清单
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="openPrintBoxNumber" v-auth="'wMSOrder:page'" :loading="opLoading.printBoxNumber" :disabled="opLoading.printBoxNumber"> 前置打印箱号
          </el-button>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" icon="ele-Help" @click="exportWMSOrderByRFIDFun"
            v-auth="'wMSOrder:page'" :loading="opLoading.exportRFID" :disabled="opLoading.exportRFID">
            导出RFID
          </el-button>
        </el-form-item>
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
              width="200" max-height="50">
            </el-table-column>
          </template>
        </template>
        <el-table-column fixed="right" label="操作" width="200">

          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button @click="del(scope.row)" class="el-icon-delete" type="text" size="small">删除
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
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100, 300]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <!-- <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" /> -->
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="handleQuery" />
    </el-card>


    <el-dialog v-model="resultPopupShow" title="消息" :append-to-body="true">
      <el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + ':' + i.msg"
        :type="i.statusMsg">
      </el-alert>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup="" name="wMSOrder">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

// import editDialog from '/@/views/main/wMSOrder/component/editDialog.vue'
// import addDialog from '/@/views/main/wMSOrder/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSOrder/component/queryDialog.vue'
import {
  pageWMSOrder, deleteWMSOrder, automatedAllocation, printShippingList,
  createPickTask, completeOrder, exportOrder, exportPackage, exportWMSOrderByRFID
  , printJobList, printBoxNumber
} from '/@/api/main/wMSOrder';
import { getByTableNameList } from "/@/api/main/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import printDialog from '/@/views/tools/printDialog.vue';
import Header from "/@/entities/order";
import Details from "/@/entities/orderDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import { useUserInfo } from '/@/stores/userInfo';
import { storeToRefs } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import { downloadByData, getFileName } from '/@/utils/download';
import { classNameToArray } from "element-plus/es/utils";
// import { useRoute } from 'vue-router'

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
// 定义变量内容
const stores = useUserInfo();
const { userInfos } = storeToRefs(stores);
const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
const printDialogRef = ref();
// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
//自定义提示
const resultPopupShow = ref(false);
const opLoading = ref({
  exportOrder: false,
  exportPackage: false,
  exportRFID: false,
  automatedAllocation: false,
  createPickTask: false,
  completeOrder: false,
  print: false,
  printJob: false,
  printBoxNumber: false
});
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
const ptintTitle = ref("");
// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {

  let res = await getByTableNameList("WMS_Order");
  state.value.tableColumnHeaders = res.data.result;

};
// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSOrder(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};

// 打开新增页面
// const openAdd = () => {
//   addTitle.value = '添加';
//   addDialogRef.value.openDialog({});
// };

// 打开编辑页面
// const openEdit = (row: any) => {
//   editTitle.value = '编辑';
//   editDialogRef.value.openDialog(row);
// };
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
      let res = await deleteWMSOrder(row);
      resultPopupShow.value = true;
      state.value.orderStatus = res.data.result.data;
      // if (res.data.result.code == "1") {
      //   ElMessage.success(res.data.result.msg);

      // } else {
      //   ElMessage.error(res.data.result.msg);
      // }
      handleQuery();
      // ElMessage.success("删除成功");
    })
    .catch(() => { });
};
// const del = (row: any) => {
//   ElMessageBox.confirm(`确定要删除吗?`, "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       await deleteWMSOrder(row);
//       handleQuery();
//       ElMessage.success("删除成功");
//     })
//     .catch(() => { });
// };





const exportWMSOrderByRFIDFun = async () => {
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
  opLoading.value.exportRFID = true;
  try {
    let res = await exportWMSOrderByRFID(ids);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  } catch (e) {
    ElMessage.error(e?.message ?? "导出失败");
  } finally {
    opLoading.value.exportRFID = false;
  }
} 




// 分配库存
const automatedAllocationFun = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选订单");
    return;
  }
  ElMessageBox.confirm(`确定要分配库存吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      opLoading.value.automatedAllocation = true;
      try {
        let result = await automatedAllocation(ids);
        handleQuery();
        if (result.data.result.data.length > 0) {
          state.value.orderStatus = result.data.result.data;
          // console.log(state.value.orderStatus);
          //导入弹框提醒
          resultPopupShow.value = true;
        } else {
          ElMessage.info(result.data.result.msg);
        }
      } catch (e) {
        ElMessage.error(e?.message ?? "分配失败");
      } finally {
        opLoading.value.automatedAllocation = false;
      }
      // console.log(data.data.result);
      // if (data.data.result[0].statusCode == 1) {
      //   handleQuery();
      //   ElMessage.success("转入库单成功");
      // } else {
      //   resultPopupShow.value=true;
      //   state.value.orderStatus = data.data.result;
      // }
    })
    .catch(() => { });
};  




//导出包装
const exportPackageFun = async () => {
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
  opLoading.value.exportPackage = true;
  try {
    let res = await exportPackage(ids);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  } catch (e) {
    ElMessage.error(e?.message ?? "导出失败");
  } finally {
    opLoading.value.exportPackage = false;
  }
} 

//导出出库单
const exportOrderFun = async () => {
  //1 获取选中的订单ID
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  opLoading.value.exportOrder = true;
  try {
    if (ids.length > 0) {
      let res = await exportOrder({ "ids": ids });
      var fileName = getFileName(res.headers);
      downloadByData(res.data as any, fileName);
    } else {
      let res = await exportOrder(state.value.header);
      var fileName = getFileName(res.headers);
      downloadByData(res.data as any, fileName);
    }
  } catch (e) {
    ElMessage.error(e?.message ?? "导出失败");
  } finally {
    opLoading.value.exportOrder = false;
  }
} 


//生成拣货任务
const createPickTaskFun = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    if (a.orderStatus == 20) {
      ids.push(a.id);
    }
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选已分配订单");
    return;
  }
  ElMessageBox.confirm(`确定要生成拣货任务吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      opLoading.value.createPickTask = true;
      try {
        let result = await createPickTask(ids);
        handleQuery();
        if (result.data.result.data.length > 0) {
          state.value.orderStatus = result.data.result.data;
          // console.log(state.value.orderStatus);
          //导入弹框提醒
          resultPopupShow.value = true;
        } else {
          ElMessage.info(result.data.result.msg);
        }
      } catch (e) {
        ElMessage.error(e?.message ?? "生成拣货任务失败");
      } finally {
        opLoading.value.createPickTask = false;
      }
    })
    .catch(() => { });
};



//完成订单
const completeOrderFun = () => {
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    if (a.orderStatus >= 20) {
      ids.push(a.id);
    }
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选已分配订单");
    return;
  }
  ElMessageBox.confirm(`确定要完成订单?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      opLoading.value.completeOrder = true;
      try {
        let result = await completeOrder(ids);
        if (result.data.result.data.length > 0) {
          state.value.orderStatus = result.data.result.data;
          //导入弹框提醒
          resultPopupShow.value = true;
          handleQuery();
        } else {
          ElMessage.info(result.data.result.msg);
        }
      } catch (e) {
        ElMessage.error(e?.message ?? "完成订单失败");
      } finally {
        opLoading.value.completeOrder = false;
      }
    })
    .catch(() => { });
};
// ======================================发运单打印=======================================
// 打开打印询页面
const openPrint = async () => {
  ptintTitle.value = '发运单打印';
  // let ids = ref(Array<Number>);
  let ids = new Array<Number>();
  // console.log("ids");
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  opLoading.value.print = true;
  let printData = new Array<Header>();
  printData.printTemplate = "";
  try {
    let result = await printShippingList(ids);
    if (result.data.result != null) {
      printData = result.data.result.data;
      printData.data.forEach(a => {
        if (a.customerConfig != null) {
          a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
        }
      });
    }

    // 判断有没有配置客户自定义打印模板
    if (printData.printTemplate != "") {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    } else if (printData.data[0].customerConfig != null && printData.data[0].customerConfig.printShippingTemplate != null) {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.data[0].customerConfig.printShippingTemplate });
    } else {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": "发运单模板" });
    }
  } catch (e) {
    ElMessage.error(e?.message ?? "打印失败");
  } finally {
    opLoading.value.print = false;
  }
};

// 打开打印询页面
const openPrintJob = async () => {
  ptintTitle.value = 'JOB汇总清单打印';
  // let ids = ref(Array<Number>);
  let ids = new Array<Number>();
  // console.log("ids");
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  opLoading.value.printJob = true;
  let printData = new Array<Header>();
  printData.printTemplate = "";
  try {
    let result = await printJobList(ids);
    console.log("JOB",result)
    if (result.data.result != null) {
      printData = result.data.result.data;
      printData.data.forEach((a: any) => {
        if (a.customerConfig != null) {
          a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
        }
      });
    }
    console.log("printData",printData)
    console.log("result");
    // 判断有没有配置客户自定义打印模板
    if (printData.printTemplate != "") {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    } else if (printData.data[0].customerConfig != null && printData.data[0].customerConfig.printShippingTemplate != null) {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.data[0].customerConfig.printShippingTemplate });
    } else {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": "发运单模板" });
    }
  } catch (e) {
    ElMessage.error(e?.message ?? "打印失败");
  } finally {
    opLoading.value.printJob = false;
  }
};

// 前置打印箱号
const openPrintBoxNumber = async () => {
  ptintTitle.value = '打印箱号';
  let ids = new Array<Number>();
  multipleTableRef.value.getSelectionRows().forEach(a => {
    ids.push(a.id);
  });
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  opLoading.value.printBoxNumber = true;
  let printData = new Array<Header>();
  printData.printTemplate = "";
  try {
    let result = await printBoxNumber(ids);
    console.log("箱号打印", result);
    if (result.data.result != null) {
      printData = result.data.result.data;
      printData.data.forEach((a: any) => {
        if (a.customerConfig != null) {
          a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
        }
      });
    }
    // 判断有没有配置客户自定义打印模板
    if (printData.printTemplate != "") {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    } else if (printData.data[0].customerConfig != null && printData.data[0].customerConfig.printShippingTemplate != null) {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.data[0].customerConfig.printShippingTemplate });
    } else {
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": "打印箱号" });
    }
  } catch (e) {
    ElMessage.error(e?.message ?? "打印失败");
  } finally {
    opLoading.value.printBoxNumber = false;
  }
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
.wMSOrder-container .el-form-item .el-select,
.wMSOrder-container .el-form-item .el-date-picker,
.wMSOrder-container .el-form-item .el-input,
.wMSOrder-container select-remote {
  width: 100%;
}
</style> 
