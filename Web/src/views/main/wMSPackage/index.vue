<template>
  <div class="wMSPackage-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-row :gutter="[16, 15]">
          <template v-for="i in  state.tableColumnHeaders">
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
            <!-- <el-button icon="ele-Refresh" @click="() => queryParams = {}"> 重置 </el-button> -->
          </el-button-group>

        </el-form-item>

        <!-- <el-form-item>
          <el-button-group>
            <el-button type="primary" icon="ele-Printer" @click="openPrint" v-auth="'wMSPackage:page'"> 打印
            </el-button>
          </el-button-group>
        </el-form-item> -->
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="openAdd" v-auth="'wMSPackage:add'"> 新增
          </el-button>
        </el-form-item> -->
        <!-- <el-form-item>
          <el-button type="primary" icon="ele-Plus" @click="PackageForOrderFun" v-auth="'wMSPackage:PackageForOrder'"> 转出库单
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
        <el-table-column fixed="right" label="操作" width="200">

          <template #default="scope">
            <el-button @click="openQuery(scope.row)" class="el-icon-s-comment" type="text" size="small">查看
            </el-button>
            <el-button class="el-icon-printer" type="text" @click="printExpress(scope.row)" size="small">打印
            </el-button>
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
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background="" @size-change="handleSizeChange"
        @current-change="handleCurrentChange" layout="total, sizes, prev, pager, next, jumper" />
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
import printDialog from '/@/views/main/wMSPackage/component/printDialog.vue'
import { getExpressConfig, allExpress } from '/@/api/main/wMSExpressConfig';
import { pageWMSPackage, deleteWMSPackage, printExpressData } from '/@/api/main/wMSPackage';
import { getByTableNameList } from "/@/api/main/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import Header from "/@/entities/packageMain";
import Details from "/@/entities/packageDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import sfExpress from "/@/api/expressInterface/sfExpress";

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
const token = ref("");
const expressConfig = ref({});

// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
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
// const editTitle = ref("");
// const addTitle = ref("");
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

// 查询操作
const handleQuery = async () => {
  loading.value = true;
  var res = await pageWMSPackage(Object.assign(state.value.header, tableParams.value));
  state.value.headers = res.data.result?.items ?? [];
  tableParams.value.total = res.data.result?.total;
  loading.value = false;
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

// 打开打印页面
const openPrint = (row: any) => {
  ptintTitle.value = '打印';
  printDialogRef.value.openDialog(row);
};

// 删除
const del = (row: any) => {
  ElMessageBox.confirm(`确定要删除吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      await deleteWMSPackage(row);
      handleQuery();
      ElMessage.success("删除成功");
    })
    .catch(() => { });
};


// 打印快递单
const printExpress = async (row: any) => {

ElMessageBox.confirm(`确定要打印吗?`, "提示", {
  confirmButtonText: "确定",
  cancelButtonText: "取消",
  type: "warning",
})
  .then(async () => {
    // console.log(row);
    if (row.expressCompany == "顺丰快递") {
        // let resToken = await getExpressConfig(row);
        // // console.log("resToken")
        // if (resToken.data.result.code == 1) {
        //   // console.log(resToken)
        //   expressConfig.value = resToken.data.result.data;
        // }

        // console.log(expressConfig.value);
        let res = await printExpressData(row);
        // alert(res.data.result.code);
        if(res.data.result.code==-1)
        {
          ElMessage.error(res.data.result.msg);
          return ;
        }
       
        // alert(res.data.result.data.expressNumber);
        // // console.log(expressConfig.value);
        // sdkParams.env = expressConfig.value.env;// 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
        // sdkParams.partnerID = expressConfig.value.partnerId;
        // sdkParams.callback = sdkCallback;
        // sdkParams.notips = true;
        // printSdk = new SCPPrint(sdkParams);
        // };
        console.log(expressConfig.value);
        //print(res.data.result.data.expressNumber, expressConfig.value.token);
        sfExpress.print(res.data.result.data)
        // allPackage(state.value.vm.form);
    }
  })
  .catch(() => { });
// allPackage(state.value.vm.form);
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


// const sdkCallback = result => {};
//     const sdkParams = {
//       env: "pro", // 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
//       partnerID: "HJSRJOEY88G9",
//       callback: sdkCallback,
//       notips: false
//     };
//     const printSdk = new SCPPrint(sdkParams);

// // 鑾峰彇鎵撳嵃鏈哄垪琛�
// const getPrintersCallback = result => {
//       if (result.code === 1) {
//         const printers = result.printers;

//         const selectElement = document.getElementById("printers");

//         // 娓叉煋鎵撳嵃鏈洪€夋嫨妗嗕笅鎷夊€�
//         for (let i = 0; i < printers.length; i++) {
//           const item = printers[i];
//           var option = document.createElement("option");
//           option.innerHTML = item.name;
//           option.value = item.index;
//           selectElement.appendChild(option);
//         }

//         // 璁剧疆榛樿鎵撳嵃鏈�
//         var printer = 0;
//         selectElement.value = printer;
//         printSdk.setPrinter(printer);
//       }
//     };
//     printSdk.getPrinters(getPrintersCallback);


//   // 鎵撳嵃
//   function print(masterWaybillNo: string,token:string) {
//       const data = {
//         requestID: "HJSRJOEY88G9",
//         accessToken: token,
//         templateCode: "fm_150_standard_HJSRJOEY88G9",
//         templateVersion: "",
//         documents: [
//           {
//             masterWaybillNo: masterWaybillNo
//           }
//         ],
//         extJson: {},
//         customTemplateCode: ""
//       };
//       console.log("data");
//       console.log(data);
//       const callback = function(result) {};
//       const options = {
//         lodopFn: "PRINT" // 榛樿鎵撳嵃锛岄瑙堜紶PREVIEW
//       };
//       printSdk.print(data, callback, options);
//     }



// // --------------------顺丰快递打印-----------------------
// // 寮曞叆SDK鍚庡垵濮嬪寲瀹炰緥锛屼粎鎵ц涓€娆�
// const sdkCallback = result => { };
// let sdkParams = {
//   env: expressConfig.value.env, // 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
//   partnerID: expressConfig.value.partnerId,
//   callback: sdkCallback,
//   notips: true
// };
// let printSdk = new SCPPrint(sdkParams);


// // 鑾峰彇鎵撳嵃鏈哄垪琛�
// const getPrintersCallback = result => {
//   if (result.code === 1) {
//     const printers = result.printers;

//     const selectElement = document.getElementById("printers");

//     // 娓叉煋鎵撳嵃鏈洪€夋嫨妗嗕笅鎷夊€�
//     for (let i = 0; i < printers.length; i++) {
//       const item = printers[i];
//       var option = document.createElement("option");
//       option.innerHTML = item.name;
//       option.value = item.index;
//       selectElement.appendChild(option);
//     }

//     // 璁剧疆榛樿鎵撳嵃鏈�
//     var printer = 0;
//     selectElement.value = printer;
//     printSdk.setPrinter(printer);
//   }
// };
// printSdk.getPrinters(getPrintersCallback);

// // 閫夋嫨鎵撳嵃鏈�
// const selectPrinter = (e) => {
//   // 璁剧疆鎵撳嵃鏈�
//   printSdk.setPrinter(e.target.value);
// }

// // 鎵撳嵃
// const print = (masterWaybillNo: string) => {
//   console.log(expressConfig.value);
//   const data = {
//     requestID: expressConfig.value.partnerId,
//     accessToken: expressConfig.value.token,
//     templateCode: expressConfig.value.templateCode,
//     templateVersion: "",
//     documents: [
//       {
//         masterWaybillNo: masterWaybillNo
//       }
//     ],
//     extJson: {},
//     customTemplateCode: ""
//   };
//   const callback = function (result) { };
//   const options = {
//     lodopFn: "PRINT" // 榛樿鎵撳嵃锛岄瑙堜紶PREVIEW
//   };
//   console.log(printSdk);
//   console.log(printSdk.print(data, callback, options));
// };



</script>


