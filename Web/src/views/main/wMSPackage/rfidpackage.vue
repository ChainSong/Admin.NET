<template>
  <div class="wMSPackagerfid-container">
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
        <div style="display: flex; gap: 10px; margin-top: 10px;">

          <div>
            <table style="height:350px;width: 450px;">
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
            </table>
          </div>

          <div style="padding-left: 100px;padding-top: 30px;">
            <el-row>
              <el-table show-summary :data="state.vm.tableData" height="350" style="width: 100%;font-size:20px;">
                <el-table-column prop="sku" label="SKU" width="200">
                </el-table-column>
                <el-table-column prop="pickQty" label="拣货数量" width="150">
                </el-table-column>
                <el-table-column prop="scanQty" label="扫描数量" width="150">
                </el-table-column>
                <el-table-column prop="remainingQty" label="剩余数量" width="150">
                </el-table-column>
                <el-table-column prop="packageQty" label="包装数量" width="150">
                </el-table-column>
              </el-table>
            </el-row>
          </div>
        </div>

      </div>

      <el-row style="top: 30px;">
        <el-table :data="state.vm.packageData" style="width: 100%;height: 500px;;font-size:20px;">
          <el-table-column prop="orderNumber" label="出库单号">
          </el-table-column>
          <el-table-column prop="pickTaskNumber" label="拣货任务号">
          </el-table-column>
          <el-table-column prop="packageNumber" label="箱号">
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
              <!-- <el-button class="el-icon-s-comment" type="text" @click="printExpress(scope.row)" size="small">打印
              </el-button> -->
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
      </el-row>
      <printDialog ref="printDialogRef" :title="ptintTitle" />

      <el-dialog title="扫描SN" v-model="state.dialogVisible" width="50%">
        <div>
          <label style="padding-left:5px;font-size:20px">有二维码的时候请不要扫描SKU</label>
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
                <el-input style="width: 80%; font-size:20px" v-model="state.sndata.sku"
                  v-on:keyup.enter="scanSNPickNumber" placeholder="请输入SKU"></el-input>
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

<script lang="ts" setup="" name="wMSPackagerfid">
import { ref, onMounted, onBeforeUnmount, nextTick } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
// import printDialog from '/@/views/main/wMSPackage/component/printDialog.vue'
import printDialog from '/@/views/tools/printDialog.vue';
import { pageWMSPackage, deleteWMSPackage, scanPackageData, printExpressData, addRFIDPackageData, allWMSPackage, addPackageData, shortagePackageData, resetPackageData, getRFIDInfo, scanPackageData_RFID, scanSNPackage, printPackageList,printPackageNumber } from '/@/api/main/wMSPackage';
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
// import SCPPrint from "";
import { signalR } from '/@/utils/signalRCustom';
import { addWMSPickTask } from "/@/api/main/wMSPickTask";
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
      rfidStr: "",
      rfidInfo: [],
    },
    tableData: [],
    rfidData: [],
    packageData: [],
  },
  sndata: {
    snCode: "",
    pickTaskNumber: ""
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


const expressOptions = ref([]);
const expressValue = ref("");
const printDialogRef = ref();
const loading = ref(false);
const multipleTableRef = ref();
const input = ref();
// const select_order_number = ref('') //表格select选中的条数
// const multipleSelection = ref([])
//自定义提示
const resultPopupShow = ref(false);

const queryTitle = ref("");
const ptintTitle = ref("");
const token = ref("");
const expressConfig = ref({});
// 生明失败的音频文件
const audio_error = new Audio('/audio/error.mp3'); // 替换为实际的音频文件路径
// 生明成功的音频文件
const audio_success = new Audio('/audio/success.mp3'); // 替换为实际的音频文件路径


// --------------------顺丰快递打印-----------------------
// 寮曞叆SDK鍚庡垵濮嬪寲瀹炰緥锛屼粎鎵ц涓€娆�
// const sdkCallback = result => { };
// let sdkParams = {
//   env: expressConfig.value.env, // 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
//   partnerID: expressConfig.value.partnerId,
//   callback: sdkCallback,
//   notips: true
// };
// let printSdk = new SCPPrint(sdkParams);

// 页面加载时
onMounted(async () => {
  gettableColumn();
  getExpress();
  try {
    signalR.on('echo', (data: any) => {
      console.log("WebSocket data");
      console.log(data);
      state.value.vm.form.rfidStr = data;
      // state.value.vm.form.rfidInfo = data.rfidinfo;
      // console.log("state.value.vm.form");
      // console.log(state.value.vm.form);
      if (state.value.vm.form.rfidStr != "") {

        getRFIDInfoData();
      }

    });
  } catch (error) {
    console.error('捕获到错误:', error);
    // 可以显示一个错误消息给用户
    //  ElMessage.error("发生了一个错误，请联系管理员。");
    // alert('发生了一个错误，请联系管理员。');
  }

});


// 获取RFID信息
const getRFIDInfoData = async () => {

  try {
    state.value.vm.form.expressCompany = expressValue.value;
    // ElMessage.warning("getRFIDInfoData");
    let res = await getRFIDInfo(state.value.vm.form);

    if (res.data.result.code == 1) {
      audio_success.play(); // 播放音频

      allPackage(state.value.vm.form);
      if (res.data.result.data != null) {
        state.value.vm.form = res.data.result.data;

        state.value.vm.tableData = res.data.result.data.packageDatas;
      }

    } else if (res.data.result.code == 99) {
      audio_success.play(); // 播放音频

      allPackage(state.value.vm.form);
      signalR.send("echo", 5);
      signalR.send("echo", 9);
      state.value.vm.form.input = "";
      state.value.vm.form.sku = "";
      state.value.vm.form.pickTaskNumber = "";
      state.value.vm.form.weight = 0;
      // state.value.vm.tableData = res.data.result.data.packageDatas;
      state.value.vm.tableData = [];

      input.value = true;
      input.value = false;
      // allPackage(state.value.vm.form);
      nextTick(() => {
        input.value.focus();
        input.value.select();
      });
      // console.log("resRFID");
      // console.log(res);

      ElMessage.success(res.data.result.msg);
    } else if (res.data.result.code == -1) {

      audio_error.play(); // 播放音频
      // signalR.send("echo", 5)
      if (res.data.result.data != null) {
        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
      ElMessage.error(res.data.result.msg);
    } else if (res.data.result.code == 5) {

      audio_error.play(); // 播放音频
      // signalR.send("echo", 5)
      if (res.data.result.data != null) {
        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
      ElMessage.error(res.data.result.msg);
    } else if (res.data.result.code == -1) {
      if (res.data.result.data != null) {

        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
      ElMessage.warning(res.data.result.msg);
    } else {
      if (res.data.result.data != null) {

        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
      ElMessage.warning(res.data.result.msg);
    }
  } catch (error) {
    console.error('捕获到错误:', error);
    // 可以显示一个错误消息给用户
    //  ElMessage.error("发生了一个错误，请联系管理员。");
    // alert('发生了一个错误，请联系管理员。');
  }


};
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
  state.value.vm.form.expressCompany = expressValue.value;
  let res = await shortagePackageData(state.value.vm.form);
  // let res = await addPackageData(state.value.vm.form);
  // let res = await scanPackageData(state.value.vm.form);
  if (res.data.result.code == 1) {
    audio_success.play(); // 播放音频
    if (res.data.result.data != null) {

      state.value.vm.form = res.data.result.data;
      state.value.vm.tableData = res.data.result.data.packageDatas;
    }
  } else if (res.data.result.code == 99) {
    audio_success.play(); // 播放音频

    // signalR.send("echo", 5);
    signalR.send("echo", 9);
    state.value.vm.form.input = "";
    state.value.vm.form.sku = "";
    // state.value.vm.form.pickTaskNumber = "";
    state.value.vm.form.weight = 0;
    state.value.vm.tableData = res.data.result.data.packageDatas;

    ElMessage.success(res.data.result.msg);
  } else {
    if (res.data.result.data != null) {
      state.value.vm.form = res.data.result.data;
      state.value.vm.tableData = res.data.result.data.packageDatas;
    }
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

const scanPickNumber = async () => {
  let res = await scanSNPackage(state.value.sndata);
  if (res.data.result.code == 1) {
    ElMessage.success(res.data.result.msg);
  } else {
    ElMessage.error(res.data.result.msg);
  }
};

const scanSNPickNumber = async () => {
  let res = await scanSNPackage(state.value.sndata);
  if (res.data.result.code == 1) {
    ElMessage.success(res.data.result.msg);
  } else {
    ElMessage.error(res.data.result.msg);
  }
};
const allPackage = async (data: any) => {
  let res = await allWMSPackage(data);
  state.value.vm.packageData = res.data.result;
};

// const shortagePackage = async (data: any) => {
//   let res = await shortagePackageData(data);
//   state.value.vm.packageData = res.data.result;
// };

const addPackage = async (data: any) => {

  try {

    state.value.vm.form.expressCompany = expressValue.value;
    // allPackage(state.value.vm.form);
    let res = await addRFIDPackageData(state.value.vm.form);
    // let res = await scanPackageData(state.value.vm.form);
    if (res.data.result.code == 1) {
      signalR.send("echo", 5);
      if (res.data.result.data != null) {
        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
    } else if (res.data.result.code == 99) {
      state.value.vm.form.input = "";
      state.value.vm.form.sku = "";
      // signalR.send('echo',99)
      // state.value.vm.form.pickTaskNumber = "";
      state.value.vm.form.weight = 0;
      state.value.vm.tableData = res.data.result.data.packageDatas;
      ElMessage.success(res.data.result.msg);
    } else {
      if (res.data.result.data != null) {
        state.value.vm.form = res.data.result.data;
        state.value.vm.tableData = res.data.result.data.packageDatas;
      }
      ElMessage.error(res.data.result.msg);
    }
    input.value = true;
    input.value = false;
    nextTick(() => {
      input.value.focus();
      input.value.select();
    });
    allPackage(state.value.vm.form);
    // 你的代码逻辑
    // throw new Error('示例错误'); // 仅用于测试
  } catch (error) {
    console.error('捕获到错误:', error);
    // 可以显示一个错误消息给用户
    // ElMessage.error("发生了一个错误，请联系管理员。");
  }
};
onBeforeUnmount(async () => {
  if (signalR.state == "Connected") {
    signalR.stop();
  }
});
// const getSummaries = async (param:any) => {
//   const { columns, data } = param;
//   const sums = [];
//   columns.forEach((column, index) => {
//     if (index === 0) {
//       sums[index] = '总数';
//       return;
//     }
//     const values = data.map(item => Number(item[column.property]));
//     if (!values.every(value => isNaN(value))) {
//       sums[index] = values.reduce((prev, curr) => {
//         const value = Number(curr);
//         if (!isNaN(value)) {
//           return prev + curr;
//         } else {
//           return prev;
//         }
//       }, 0);
//       sums[index] += ' 件';
//     } else {
//       sums[index] = 'N/A';
//     }
//   });

//   return sums;
// }
const scanPackage = async () => {

  signalR.send('echo', 1)
  // 判断webSocket是否连接
  if (signalR.state != "Connected") {
    signalR.start();
    // ElMessage.error("连接状态" + signalR.state);
  }

  if (signalR.state == "Connected") {
    ElMessage.success("RFID 连接成功");
  } else {
    ElMessage.error("RFID 连接失败，请重新连接");
  }

  // alert("请扫描商品");

  // signalR.send('echo', 1)
  signalR.send("echo", 5);
  if (state.value.vm.form.pickTaskNumber != '') {
    if (state.value.vm.form.weight == 0) {
      ElMessage.error("请输入重量");
      return;
    }
  }
  if (state.value.vm.form.pickTaskNumber != '') {
    ElMessage.error("请使用RFID扫描");
    return;
  }

  state.value.vm.form.expressCompany = expressValue.value;
  let res = await scanPackageData_RFID(state.value.vm.form);
  if (res.data.result.code == 1) {
    allPackage(state.value.vm.form);
    state.value.vm.form = res.data.result.data;
    state.value.vm.tableData = res.data.result.data.packageDatas;

  } else if (res.data.result.code == 99) {
    allPackage(state.value.vm.form);
    state.value.vm.form.input = "";
    state.value.vm.form.sku = "";
    // state.value.vm.form.pickTaskNumber = "";
    state.value.vm.form.weight = 0;
    state.value.vm.tableData = res.data.result.data.packageDatas;

    ElMessage.success(res.data.result.msg);
  } else {
    // state.value.vm.form = res.data.result.data;
    // state.value.vm.tableData = res.data.result.data.packageDatas;
    ElMessage.error(res.data.result.msg);
  }

  input.value = true;
  input.value = false;
  allPackage(state.value.vm.form);
  nextTick(() => {
    input.value.focus();
    input.value.select();
  });

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
      if (res.data.result.code == 1) {
        state.value.vm.form.sku = "";
        state.value.vm.form.pickTaskNumber = "";
        state.value.vm.form.weight = 0;
        state.value.vm.tableData = [];
        ElMessage.success(res.data.result.msg);
        scanPackage();
      }
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
        // let resToken = await getExpressConfig(row);
        // // console.log("resToken")
        // if (resToken.data.result.code == 1) {
        //   // console.log(resToken)
        //   expressConfig.value = resToken.data.result.data;
        // }

        // console.log(row);
        let res = await printExpressData(row);
        // alert(res.data.result.data.expressNumber);
        // // console.log(expressConfig.value);
        // sdkParams.env = expressConfig.value.env;// 鐢熶骇锛歱ro锛涙矙绠憋細sbox銆備笉浼犻粯璁ょ敓浜э紝杞敓浜ч渶瑕佷慨鏀硅繖閲�
        // sdkParams.partnerID = expressConfig.value.partnerId;
        // sdkParams.callback = sdkCallback;
        // sdkParams.notips = true;
        // printSdk = new SCPPrint(sdkParams);
        // };
        // print(res.data.result.data.expressNumber);
        sfExpress.print(res.data.result.data)
        allPackage(state.value.vm.form);
      }
    })
    .catch(() => { });

  // token.value = res.data;

};

// Storage.prototype.setCanExpireLocal('userJson', JSON.stringify(obj), 0.1) 
// Storage.prototype.getCanExpireLocal('userJson') 
// // 打开打印页面
// const openPrint = (row: any) => {
//   ptintTitle.value = '打印';
//   printDialogRef.value.openDialog(row);
// };




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
//   // console.log(result);
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

// // 改变页面容量
// const handleSizeChange = (val: number) => {
//   tableParams.value.pageSize = val;

// };

// // 改变页码序号
// const handleCurrentChange = (val: number) => {
//   tableParams.value.page = val;

// };



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
