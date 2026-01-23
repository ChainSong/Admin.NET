<template>
  <div class="wMSPackage-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <div style="border: 2px solid #ff2d2d ">
        <div style="background-color:rgb(146, 187, 205);">
          <el-row>
            <el-row :gutter="[12, 24]" width="50%">
              <el-row style="width: 100%;">
                <el-button style="font-size:20px;" type="primary" @click="reset">清空重扫</el-button>
                <!-- <el-button style="font-size:20px;" type="success" @click="printExpress">打印</el-button>-->
                <el-button style="font-size:20px;" type="info" @click="shortagePackage">短包</el-button>
                <!-- <el-button type="warning">换箱</el-button> -->
                <el-button style="font-size:20px;" type="danger" @click="addPackage">新增箱</el-button>
                <el-button style="font-size:20px;" type="danger"
                  @click="state.dialogVisible = true; state.sndata.pickTaskNumber = ''; state.sndata.snCode = ''; state.sndata.sku = ''">扫描SN</el-button>
              </el-row>
            </el-row>
          </el-row>
        </div>
        <el-row :gutter="[8, 24]">
          <div>
            <table style="height:350px;width: 400px;">
              <tr>
                <th style="padding-left:5px;font-size:20px" rowspan="1">快递公司:</th>
                <td>
                  <el-select v-model="expressValue" filterable style="width: 100%;font-size:20px">
                    <el-option v-for="item in expressOptions" :key="item.value" :label="item.label" :value="item.value">
                    </el-option>
                  </el-select>
                </td>

              </tr>
               <tr>
                <th style="padding-left:5px;font-size:20px" rowspan="1">包装箱型:</th>
                <td>
                    <el-input v-model="state.vm.form.boxType"   
                   style="font-size:20px" placeholder="请输入内容"></el-input>
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px" rowspan="1">扫描框:</th>
                <td>
                  <el-input v-model="state.vm.form.input" v-focus="input" v-select="input" ref="input"
                    v-on:keyup.enter="scanPackage" style="font-size:20px" placeholder="请输入内容"></el-input>
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px">拣货任务号:</th>
                <td>
                  <el-input v-model="state.vm.form.pickTaskNumber" :disabled="true" style="font-size:20px"
                    placeholder="拣货任务号" />
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px">SKU:</th>
                <td>
                  <el-input v-model="state.vm.form.sku" :disabled="true" style="font-size:20px" placeholder="SKU" />
                  <!-- <label style="font-size:20px;">{{ state.vm.form.sku }}</label> -->
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px">重量:</th>
                <td>
                  <el-input style="font-size:20px;" v-model="state.vm.form.weight"
                    onkeyup="value=value.replace(/^\D*(\d*(?:\.\d{0,3})?).*$/g, '$1')" placeholder="请输入重量"></el-input>
                  <!-- <input id="netweight" type="text" onkeyup="value=value.replace(/^\D*(\d*(?:\.\d{0,3})?).*$/g, '$1')"
                  class="form-control" style="width:100%" /> -->
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px">数量:</th>
                <td>
                  <el-input style="font-size:20px;" v-model="state.vm.form.scanQty"
                    onkeyup="value=value.replace(/^\D*(\d*(?:\.\d{0,3})?).*$/g, '$1')" placeholder="请输入数量"></el-input>
                  <!-- <input id="netweight" type="text" onkeyup="value=value.replace(/^\D*(\d*(?:\.\d{0,3})?).*$/g, '$1')"
                  class="form-control" style="width:100%" /> -->
                </td>
              </tr>
              <tr>
                <th style="padding-left:5px;font-size:20px">备注:</th>
                <td>
                  <div>
                    <label style="font-size:20px;">{{ state.vm.form.remark }}</label>
                  </div>
                </td>
              </tr>
            </table>
          </div>

          <div style="padding-left: 100px;padding-top: 30px;">
            <el-row>
              <el-table show-summary :data="state.vm.tableData" height="350" style="width: 100%;font-size:20px;">
                <el-table-column prop="sku" label="SKU" width="200">
                </el-table-column>
                <el-table-column prop="pickQty" label="拣货数量" width="120">
                </el-table-column>
                <el-table-column prop="scanQty" label="扫描数量" width="120">
                </el-table-column>
                <el-table-column prop="remainingQty" label="剩余数量" width="120">
                </el-table-column>
                <el-table-column prop="packageQty" label="包装数量" width="120">
                </el-table-column>

              </el-table>
            </el-row>
          </div>
        </el-row>

      </div>
      <div>
        <el-button-group>
          <el-button type="primary" icon="ele-Printer" @click="printExpressBatchFun('')">
            批量快递打印
          </el-button>
        </el-button-group>
         <!-- <el-button-group>
          <el-button type="primary" icon="ele-Printer" @click="printExpressBatchFun('')">
            批量快递箱号
          </el-button>
        </el-button-group> -->
      </div>

      <el-table :data="state.vm.packageData" ref="multipleTableRef"
        style="width: 100%;height: 300px;top: 30px;;font-size:20px;">
        <el-table-column type="selection" width="55">
        </el-table-column>
        <el-table-column prop="orderNumber" label="出库单号">
        </el-table-column>
        <el-table-column prop="pickTaskNumber" label="拣货任务号">
        </el-table-column>
        <el-table-column prop="packageNumber" label="箱号">
        </el-table-column>
          <el-table-column prop="serialNumber" label="序号">
        </el-table-column>
        <el-table-column prop="detailCount" label="包装数量">
        </el-table-column>
        <el-table-column prop="expressCompany" label="快递公司">
        </el-table-column>
        <el-table-column prop="expressNumber" label="快递单号">
        </el-table-column>
        <el-table-column prop="printNum" label="打印次数">
        </el-table-column>
        <el-table-column fixed="right" label="操作">
          <template #default="scope">
            <el-button icon="ele-Printer" type="primary" @click="printExpress(scope.row)">打印快递单
            </el-button>
              <el-button type="primary" icon="ele-Printer" @click="printPackageListFun(scope.row)"
              v-auth="'wMSPackage:printPackage'">
              打印箱清单
            </el-button>
             <el-button type="primary" icon="ele-Printer" @click="printPackageNumberFun(scope.row)"
              v-auth="'wMSPackage:printPackage'">
              打印箱号
            </el-button> 
          </template>
        </el-table-column>
      </el-table>


      <printDialog ref="printDialogRef" :title="ptintTitle" />
      <el-dialog title="扫描SN" v-model="state.dialogVisible" width="50%">
        <div>
          <label style="padding-left:5px;font-size:20px">BOM件获取JNE码</label>
          <table style="height:350px;width: 450px;">
            <tr>
              <th style="padding-left:5px;font-size:20px" rowspan="1">拣货任务号:</th>
              <td>
                <el-input style="width: 80%;font-size:20px" v-model="state.sndata.pickTaskNumber"
                  v-on:keyup.enter="scanPickNumber" placeholder="请输入拣货任务号"></el-input>
              </td>
            </tr>
            <tr>
              <th style="padding-left:5px;font-size:20px">SKU:</th>
              <td>
                <el-input style="width: 80%; font-size:20px" v-model="state.sndata.sku" v-focus="inputSKU"
                  ref="inputSKU" v-select="inputSKU" placeholder="请输入SKU"></el-input>
              </td>
            </tr>
            <tr>
              <th style="padding-left:5px;font-size:20px">条码:</th>
              <td>
                <el-input style="width: 80%; font-size:20px" v-model="state.sndata.snCode"
                  v-on:keyup.enter="scanSNPickNumber" placeholder="请输入SN"></el-input>
              </td>
            </tr>

          </table>
        </div>
      </el-dialog>
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="suitpackage">
import { ref, onMounted, nextTick } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
// import printDialog from '/@/views/main/wMSPackage/component/printDialog.vue'
import printDialog from '/@/views/tools/printDialog.vue';
import { pageWMSPackage, deleteWMSPackage, scanPackageData,scanPackagSuiteData, printExpressData, allWMSPackage, addPackageData, shortagePackageData, resetPackageData, printBatchExpress, scanSNPackage, printPackageList ,printPackageNumber} from '/@/api/main/wMSPackage';
import { getExpressConfig, allExpress } from '/@/api/main/wMSExpressConfig';
import { getByTableNameList } from "/@/api/main/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import Header from "/@/entities/packageMain";
import Details from "/@/entities/packageDetail";
import PickTaskDetail from "/@/entities/pickTaskDetail";
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import sfExpress from "/@/api/expressInterface/sfExpress";
import { getWMSBoxType } from '/@/api/main/wMSBoxType';
// import SCPPrint from "";

import { addWMSPickTask } from "/@/api/main/wMSPickTask";
import { forEach } from "lodash-es";

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
const state = ref({
  vm: {
    id: "",
    form: {
      input: "",
      sku: "",
      pickTaskNumber: "",
      weight: 0,
      expirationDate: "",
      expressCompany: "",
      lot: "",
      sn: "",
    },
    tableData: [],
    packageData: [],
  },
  sndata: {
    snCode: "",
    pickTaskNumber: "",
    sku: ""
  },
  dialogVisible: false,
  visible: false,
  loading: false,
  header: new Header(),
  headers: new Array<Header>(),
  details: new Array<Details>(),
  // pickTaskDetail: new Array<Details>(),
  // header: new Array<Details>(),

  tableColumnHeader: new TableColumns(),
  tableColumnHeaders: new Array<TableColumns>(),
  tableColumnDetail: new TableColumns(),
  tableColumnDetails: new Array<TableColumns>(),
  //自定义提示
  orderStatus: new Array<orderStatus>(),

  // expressList:



});
// 生明失败的音频文件
const audio_error = new Audio('/audio/error.mp3'); // 替换为实际的音频文件路径
// 生明成功的音频文件
const audio_success = new Audio('/audio/success.mp3'); // 替换为实际的音频文件路径

const expressOptions = ref([]);
const expressValue = ref("");
const printDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
const input = ref();
const inputSKU = ref();
// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
//自定义提示
const resultPopupShow = ref(false);

const queryTitle = ref("");
const ptintTitle = ref("");
const token = ref("");
const expressConfig = ref({});


// 页面加载时
onMounted(async () => {
  gettableColumn();
  getExpress();
});

const getExpress = async () => {
  let res = await allExpress();
  res.data.result.forEach(a => {
    console.log(expressValue.value);
    if (expressValue.value == "") {
      expressValue.value = a.expressCompany;
    }
    expressOptions.value.push({ value: a.expressCompany, label: a.expressCompany })
  });

};
const shortagePackage = async () => {
  let res = await shortagePackageData(state.value.vm.form);
  if (res.data.result.code == 1) {
    audio_success.play(); // 播放音频

    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;
  } else if (res.data.result.code == 99) {
    audio_success.play(); // 播放音频

    state.value.vm.form.input = "";
    state.value.vm.form.sku = "";
    // state.value.vm.form.pickTaskNumber = "";
    state.value.vm.form.weight = 0,
      state.value.vm.tableData = res.data.result.data.packageDatas;

    ElMessage.success(res.data.result.msg);
  } else {
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;
    ElMessage.error(res.data.result.msg);
  }
  input.value = true;
  input.value = false;
  nextTick(() => {
    input.value.focus();
    input.value.select();
  });
  allPackage(state.value.vm.form);
};

const gettableColumn = async () => {
  let res = await getByTableNameList("WMS_Package");
  state.value.tableColumnHeaders = res.data.result;
};

const allPackage = async (data: any) => {
  let res = await allWMSPackage(data);
  state.value.vm.packageData = res.data.result;
};

const scanSNPickNumber = async () => {
  let res = await scanSNPackage(state.value.sndata);
  if (res.data.result.code == 1) {
    state.value.sndata.snCode = "";
    state.value.sndata.sku = "";
    inputSKU.value = true;
    inputSKU.value = false;
    nextTick(() => {
      inputSKU.value.focus();
      inputSKU.value.select();
    });
    ElMessage.success(res.data.result.msg);
  } else {
    state.value.sndata.snCode = "";
    state.value.sndata.sku = "";
    inputSKU.value = true;
    inputSKU.value = false;
    nextTick(() => {
      inputSKU.value.focus();
      inputSKU.value.select();
    });
    ElMessage.error(res.data.result.msg);
  }
};
const addPackage = async (data: any) => {
  state.value.vm.form.expressCompany = expressValue.value;
  // allPackage(state.value.vm.form);
  let res = await addPackageData(state.value.vm.form);
  // let res = await scanPackageData(state.value.vm.form);
  if (res.data.result.code == 1) {
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;
  } else if (res.data.result.code == 99) {
    state.value.vm.form.input = "";
    state.value.vm.form.sku = "";
    // state.value.vm.form.pickTaskNumber = "";
    state.value.vm.form.weight = 0,
      state.value.vm.tableData = res.data.result.data.packageDatas;
    ElMessage.success(res.data.result.msg);
  } else {
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;
    ElMessage.error(res.data.result.msg);
  }
  input.value = true;
  input.value = false;
  nextTick(() => {
    input.value.focus();
    input.value.select();
  });
  allPackage(state.value.vm.form);
};


const scanPackage = async () => {
  if (state.value.vm.form.input == "2035600-CN") {

    if ('speechSynthesis' in window) {
      // this.speech = window.speechSynthesis;
      var utterance = new SpeechSynthesisUtterance("货号要扫描防伪码");
      window.speechSynthesis.speak(utterance);
      // if (window.speechSynthesis) {
      //   window.speechSynthesis.cancel(); // 停止当前正在播放的语音
      // }
    }
    ElMessage.warning("货号要扫描防伪码");
    ElMessage.warning("货号要扫描防伪码");
    ElMessage.warning("货号要扫描防伪码");
  }
  state.value.vm.form.expressCompany = expressValue.value;
  let res = await scanPackagSuiteData(state.value.vm.form);
  if (res.data.result.code == 1) {
    if (res.data.result.data.sku == "2035600-CN") {

      if ('speechSynthesis' in window) {
        // this.speech = window.speechSynthesis;
        var utterance = new SpeechSynthesisUtterance("货号要扫描防伪码");
        window.speechSynthesis.speak(utterance);
        // if (window.speechSynthesis) {
        //   window.speechSynthesis.cancel(); // 停止当前正在播放的语音
        // }
      }
      ElMessage.warning("货号要扫描防伪码");
      ElMessage.warning("货号要扫描防伪码");
      ElMessage.warning("货号要扫描防伪码");
    }
    audio_success.play(); // 播放音频

    allPackage(state.value.vm.form);
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;

  } else if (res.data.result.code == 99) {
    audio_success.play(); // 播放音频
    allPackage(state.value.vm.form);
    state.value.vm.form.input = "";
    state.value.vm.form.sku = "";
    // state.value.vm.form.pickTaskNumber = "";
    state.value.vm.form.weight = 0,
      state.value.vm.tableData = res.data.result.data.packageDatas;

    ElMessage.success(res.data.result.msg);
  } else {
    audio_error.play(); // 播放音频
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;
    ElMessage.error(res.data.result.msg);
  }
  input.value = true;
  input.value = false;
  allPackage(state.value.vm.form);
  nextTick(() => {
    input.value.focus();
    input.value.select();
  });
  // input.focus();
  // input.select();
  // $("#input").focus();
  // $("#ScanText").select();
  // console.log(state);
  // state.value.vm.tableData = res.data.result.data.packageDatas;

}




//打印箱唛
const printPackageListFun = async (row: any) => {
  console.log("row");
  console.log(row);
  ptintTitle.value = '打印';
  var ids = new Array<any>();
  if (row == null || row == undefined || row == "") {
    multipleTableRef.value.getSelectionRows().forEach(a => {
      // console.log("a");
      // console.log(a);
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
      console.log("result");
      console.log(result);
      if (result.data.result != null) {
        printData = result.data.result.data;

        console.log("printData");
        console.log(printData);
        printData.data.forEach(a => {
          if (a.customerConfig != null) {
            a.customerConfig.customerLogo = baseURL + a.customerConfig.customerLogo;
          }
        });
      }
      console.log("result");
      console.log(printData);
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

// //打印箱唛
// const printPackageNumber = async (row: any) => {
//   console.log("row");
//   console.log(row);
//   ptintTitle.value = '打印';
//   var packageNumbers = new Array<any>();
//   if (row == null || row == undefined || row == "") {
//     multipleTableRef.value.getSelectionRows().forEach(a => {
//       // console.log("a");
//       // console.log(a);
//       packageNumbers.push(a);
//     });
//   } else {
//     packageNumbers.push(row);
//   }
//   if (packageNumbers.length == 0) {
//     ElMessage.error("请勾选需要打印的订单");
//     return;
//   }

//   ElMessageBox.confirm("是否要打印？", "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       console.log("row");
//       console.log(row);
//       console.log(packageNumbers);
//       printDialogRef.value.openDialog({ "printData": packageNumbers, "templateName": "打印出库箱号" });
//     })
//     .catch(() => { });
// };



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

//重新包装
const reset = async (row: any) => {
  ElMessageBox.confirm(`确认是否重新扫描?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {
      let res = await resetPackageData(state.value.vm.form);
      console.log(row);
    })
    .catch(() => { });
}
// 打印快递单
const printExpress = async (row: any) => {
  ElMessageBox.confirm(`确定要打印吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {

      console.log(row);
      if (row.expressCompany == "顺丰快递") {
        let res = await printExpressData(row);
        if (res.data.result.code == "1") {
          ElMessage.success("打印成功");
          sfExpress.print(res.data.result.data)
          allPackage(state.value.vm.form);
        } else {
          ElMessage.error("打印失败:" + res.data.result.msg);
        }

      }
    })
    .catch(() => { });

};

const scanPickNumber = async () => {
  let res = await scanSNPackage(state.value.sndata);
  if (res.data.result.code == 1) {
    ElMessage.success(res.data.result.msg);
  } else {
    ElMessage.error(res.data.result.msg);
  }
};

const printExpressBatchFun = async (row: any) => {

  var ids = new Array<any>();
  if (row == null || row == undefined || row == "") {
    multipleTableRef.value.getSelectionRows().forEach(a => {
      console.log("a");
      console.log(a);
      ids.push(a.id);
    });
  } else {
    ids.push(row.id);
  }
  if (ids.length == 0) {
    ElMessage.error("请勾选需要打印的订单");
    return;
  }
  ElMessageBox.confirm(`确定要打印吗?`, "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning",
  })
    .then(async () => {

      console.log(row);
      // if (row.expressCompany == "顺丰快递") {
      let res = await printBatchExpress(ids);
      console.log("res");
      console.log(res);
      if (res.data.result.code == "1") {
        ElMessage.success("打印成功");
        // forEach(res.data.result.data, (item: any, index: number) => {
        //   setTimeout(() => {
        //     sfExpress.print(item);
        //   }, index * 800); // 每项延迟800毫秒，根据索引增加延迟时间
        // });
        sfExpress.printBatch(res.data.result.data);
        allPackage(state.value.vm.form);
      } else {
        ElMessage.error("打印失败:" + res.data.result.msg);
      }

      // }
    })
    .catch(() => { });

};
</script>

<style scoped>
body>div {
  font-size: 20px;
}

.el-select .el-input,
.el-select-dropdown .el-select-dropdown__item {
  font-size: 16px;
}
</style>
