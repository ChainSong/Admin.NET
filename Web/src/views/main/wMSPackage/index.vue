<template>
  <div class="wMSPackage-container">
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
            <el-button type="primary" icon="ele-Search" @click="handleQuery" v-auth="'wMSPackage:page'"> 查询
            </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Printer" @click="printPackageNumberFun('')"
              v-auth="'wMSPackage:printPackage'">
              打印箱号
            </el-button>
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Printer" @click="printPackageListFun('')"
              v-auth="'wMSPackage:printPackage'">
              打印箱清单
            </el-button>
           <!--   <el-button type="primary" icon="ele-Printer" @click="printPackageDGListFun('')"
              v-auth="'wMSPackage:printPackage'">
              危险仓打印箱清单
            </el-button> !-->
          </el-button-group>
        </el-form-item>
        <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Download" @click="exportPackageFun('')"
              v-auth="'wMSPackage:printPackage'"> 导出
            </el-button>
          </el-button-group>
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
              width="150" max-height="50">
            </el-table-column>
          </template>
        </template>
        <el-table-column fixed="right" label="操作" width="280">

          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button class="el-icon-printer" v-auth="'wMSPackage:printExpress'" type="text"
              @click="printExpress(scope.row)" size="small">打印
            </el-button>
            <el-button class="el-icon-printer" v-auth="'wMSPackage:printPackage'" type="text"
              @click="printPackageNumberFun(scope.row)" size="small">打印箱号
            </el-button>
            <el-button @click="openEdit(scope.row)" class="el-icon-edit" type="text" size="small">编辑</el-button>
            <!-- <el-button @click="openPrint(scope.row)" class="el-icon-s-comment" type="text" size="small">打印
            </el-button> -->
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
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100, 500, 1000]" backgroundsmall="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
      <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <!-- <editDialog ref="editDialogRef" :title="editTitle" @reloadTable="handleQuery" />
      <addDialog ref="addDialogRef" :title="addTitle" @reloadTable="handleQuery" /> -->
      <queryDialog ref="queryDialogRef" :title="queryTitle" @reloadTable="handleQuery" />
      <printDialog ref="printDialogRef" :title="ptintTitle" @reloadTable="handleQuery" />
    </el-card>

    <!-- <el-dialog v-model="resultPopupShow" title="转入库单结果" :append-to-body="true">
    <el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg" :type="i.statusMsg">
    </el-alert>
  </el-dialog> -->
  </div>
</template>

<script lang="ts" setup="" name="wMSPackage">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import editDialog from '/@/views/main/wMSPackage/component/editDialog.vue'
import addDialog from '/@/views/main/wMSPackage/component/addDialog.vue'
import queryDialog from '/@/views/main/wMSPackage/component/queryDialog.vue'
// import printDialog from '/@/views/main/wMSPackage/component/printDialog.vue'
import printDialog from '/@/views/tools/printDialog.vue';
import { getExpressConfig, allExpress } from '/@/api/main/wMSExpressConfig';
import {
  pageWMSPackage, deleteWMSPackage, printExpressData, exportPackage, printPackageList
  , printDGPackageList, printPackageNumber
} from '/@/api/main/wMSPackage';
import { getByTableNameList } from "/@/api/main/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import Header from "/@/entities/packageMain";
import Details from "/@/entities/packageDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import sfExpress from "/@/api/expressInterface/sfExpress";
import { downloadByData, getFileName } from '/@/utils/download';
// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
const state = ref({
  vm: {
    id: "",
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
});
const editTitle = ref("");
const editDialogRef = ref();
const addDialogRef = ref();
const queryDialogRef = ref();
const printDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
const token = ref("");
const expressConfig = ref({});
//自定义提示
const resultPopupShow = ref(false);
// const tableData = ref<any>
// ([]);
const queryParams = ref<any>
  ({});
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
const queryTitle = ref("");
const ptintTitle = ref("");

// 页面加载时
onMounted(async () => {
  gettableColumn();
});

const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_Package");
  state.value.tableColumnHeaders = res.data.result;
};


//导出预出库单
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
  if (ids.length > 0) {
    let res = await exportPackage({ "ids": ids });
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  } else {
    let res = await exportPackage(state.value.header);
    var fileName = getFileName(res.headers);
    downloadByData(res.data as any, fileName);
  }
}


// 打开编辑页面
const openEdit = (row: any) => {
  editTitle.value = '编辑';
  editDialogRef.value.openDialog(row);
};
// 查询操作
const handleQuery = async () => {
  loading.value = true;

  console.log("row");
  console.log(state.value.header);

  var res = await pageWMSPackage(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
};
// 打开查询页面
const openQuery = (row: any) => {
  queryTitle.value = '查看';
  queryDialogRef.value.openDialog(row);
};

// 打开打印页面
// const openPrint = (row: any) => {
//   ptintTitle.value = '打印';
//   printDialogRef.value.openDialog(row);
// };

// 删除
// const del = (row: any) => {
//   ElMessageBox.confirm(`确定要删除吗?`, "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       await deleteWMSPackage(row);
//       handleQuery();
//       ElMessage.success("删除成功");
//     })
//     .catch(() => { });
// };



//打印箱唛
const printPackageListFun = async (row: any) => {
  ptintTitle.value = '打印';
  var ids = new Array<any>();
  if (row == null || row == undefined || row == "") {
    multipleTableRef.value.getSelectionRows().forEach(a => {
      ids.push(a.id);
    });
  } else {
    ids.push(row.id);
  }
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }

  var flag = 0;
  var printMessage = "是否要打印？";
  //判断列表中有没有打印次数大于0的数据
  multipleTableRef.value.getSelectionRows().forEach(a => {
    if (a.printNum > 0) {
      flag = 1;
    }
  })
  if (flag == 1) {
    printMessage = "勾选的订单存在已经打印过的数据，是否继续打印?";
  }
  ElMessageBox.confirm(printMessage, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {

      let printData = new Array<any>();
      printData.printTemplate = "";
      let result = await printPackageList(ids);
         console.log("result", result);
      if (result.data.result != null) {
        printData = result.data.result.data;
        console.log("printData", printData);
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
        printDialogRef.value.openDialog({ "printData": ids, "templateName": "装箱清单" });
      }

    })
    .catch(() => { });
};

//打印箱唛
const printPackageNumberFun = async (row: any) => {
  console.log("row");
  console.log(row);
  ptintTitle.value = '打印';
  var packageNumbers = new Array<any>();
  if (row == null || row == undefined || row == "") {
    multipleTableRef.value.getSelectionRows().forEach(a => {
      // console.log("a");
      // console.log(a);
      packageNumbers.push(a.id);
    });
  } else {
    packageNumbers.push(row.id);
  }
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
      let printData = new Array<Header>();
        console.log("printData");
      printData.printTemplate = "";
         console.log("ids", packageNumbers);
      let result = await printPackageNumber(packageNumbers);
      console.log("result", result);
      if (result.data.result != null) {
        printData = result.data.result.data;
      }
      printData.printTemplate = "打印出库箱号";
      // console.log("packageNumbers", packageNumbers);
      printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
    })
    .catch(() => { });
};

// 打印快递单
const printExpress = async (row: any) => {

  // var flag = 0;
  var printMessage = "是否要打印？";
  //判断列表中有没有打印次数大于0的数据

  if (row.printNum > 0) {
    printMessage = "勾选的订单存在已经打印过的数据，是否继续打印?";
  }

  ElMessageBox.confirm(printMessage, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      console.log("row");
      console.log(row);
      if (row.expressCompany == "顺丰快递") {
        let res = await printExpressData(row);
        if (res.data.result.code == -1) {
          ElMessage.error(res.data.result.msg);
          return;
        }
        sfExpress.print(res.data.result.data)
      }
    })
    .catch(() => { });
};

//打印箱唛
// const printPackageDGListFun = async (row: any) => {
//   ptintTitle.value = '打印';
//   var ids = new Array<any>();
//   if (row == null || row == undefined || row == "") {
//     multipleTableRef.value.getSelectionRows().forEach((a: any) => {
//       ids.push(a.id);
//     });
//   } else {
//     ids.push(row.id);
//   }
//   if (ids.length == 0) {
//     ElMessage.error("请勾选需要打印的订单");
//     return;
//   }
//   var flag = 0;
//   var printMessage = "是否要打印？";
//   //判断列表中有没有打印次数大于0的数据
//   multipleTableRef.value.getSelectionRows().forEach(a => {
//     if (a.printNum > 0) {
//       flag = 1;
//     }
//   })
//   if (flag == 1) {
//     printMessage = "勾选的订单存在已经打印过的数据，是否继续打印?";
//   }
//   ElMessageBox.confirm(printMessage, "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       let printData = new Array<any>();
//       printData.printTemplate = "";
//       let result = await printDGPackageList(ids);
//       console.log("result", result)
//       if (result.data.result != null) {
//         printData = result.data.result.data;
//         console.log("printData", printData);
//         printData.data.forEach((a: any) => {
//           if (a.customerConfig != null) {
//             a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
//           }
//         });
//       }
//       // 判断有没有配置客户自定义打印模板
//       if (printData.printTemplate != "") {
//         printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.printTemplate });
//       } else if (printData.data[0].customerConfig != null && printData.data[0].customerConfig.printShippingTemplate != null) {
//         printDialogRef.value.openDialog({ "printData": printData.data, "templateName": printData.data[0].customerConfig.printShippingTemplate });
//       } else {
//         printDialogRef.value.openDialog({ "printData": ids, "templateName": "装箱清单" });
//       }
//     })
//     .catch(() => { });
// };


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
