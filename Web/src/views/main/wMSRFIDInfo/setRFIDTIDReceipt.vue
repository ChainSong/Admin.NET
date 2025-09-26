<template>
  <div class="wMSRFIDInfo-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">
        <el-form-item label="入库单号">
          <el-input v-model="queryParams.receiptNumber"     v-on:keyup.enter="scanRFID" clearable="" placeholder="请输入入库单号" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
       <el-row>
            <el-col :span="8">
              <el-form-item label="总数量">
                <el-input v-model="state.totalQty" disabled="" clearable="" placeholder="" />
              </el-form-item>
            </el-col>
            <el-col :span="8">
              <el-form-item label="已扫描数量">
                <el-input v-model="state.qty" disabled="" clearable="" placeholder="" />
              </el-form-item>
            </el-col>
            <el-col :span="8">
            </el-col>
          </el-row>
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="setRFIDTIDReceipt">
import { ref, onMounted, onBeforeUnmount, nextTick } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import { setReceiptRFIDTID } from '/@/api/main/wMSRFIDInfo';
import { signalR } from '/@/utils/signalRCustom';
import { stringify } from "querystring";

const queryParams = ref<any>
  ({});



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
   qty :0,
   totalQty:0
});

// 页面加载时
onMounted(async () => {

  try {
    signalR.on('echo', (data: any) => {
      console.log("WebSocket data");
      console.log(data);
      queryParams.value.rfidStr = data;
      // state.value.vm.form.rfidInfo = data.rfidinfo;
      // console.log("state.value.vm.form");
      // console.log(state.value.vm.form);
      if (queryParams.value.rfidStr != "") {

        handleQuery();
      }

    });
  } catch (error) {
    console.error('捕获到错误:', error);
    // 可以显示一个错误消息给用户
    //  ElMessage.error("发生了一个错误，请联系管理员。");
    // alert('发生了一个错误，请联系管理员。');
  }

});

onBeforeUnmount(async () => {
  if (signalR.state == "Connected") {
    signalR.stop();
  }
});

// 查询操作
const handleQuery = async () => {

  var res = await setReceiptRFIDTID(queryParams.value);
 
  if (res.data.code == 200) {
    console.log("res");
    console.log(res);
    state.value.qty=res.data.result.data.qty;
    state.value.totalQty=res.data.result.data.totalQty;
    ElMessage.success("设置成功:" + res.data.result.data.qty);
  } else {
    ElMessage.error(res.msg);
  }
};

// 查询操作
const scanRFID = async () => {

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
   signalR.send('echo', 1)
  var res = await setReceiptRFIDTID(queryParams.value);
  if (res.code == 200) {
 
    ElMessage.success("设置成功" + res.result.data.qty);
  } else {
    ElMessage.error(res.msg);
  }
};
 
</script>
