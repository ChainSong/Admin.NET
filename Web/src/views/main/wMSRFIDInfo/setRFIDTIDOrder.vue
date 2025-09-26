<template>
  <div class="wMSRFIDInfo-container">
    <el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
      <el-form :model="queryParams" ref="queryForm" :inline="true">

        <el-form-item label="拣货任务号">
          <el-input v-model="queryParams.pickTaskNumber" clearable="" placeholder="请输入拣货任务号" />
        </el-form-item>
      </el-form>
    </el-card>
    <el-card class="full-table" shadow="hover" style="margin-top: 8px">
      <div></div>
      <div></div>
    </el-card>
  </div>
</template>

<script lang="ts" setup="" name="setRFIDTIDOrder">
import { ref, onMounted, nextTick } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

import { setRFIDTID } from '/@/api/main/wMSRFIDInfo';
import { signalR } from '/@/utils/signalRCustom';
import { stringify } from "querystring";

const queryParams = ref<any>
  ({});


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

// 查询操作
const handleQuery = async () => {

  var res = await setRFIDTID(queryParams.value);
  if (res.code == 200) {
    
    ElMessage.success("设置成功"+res.data.data.qty);
  } else {
    ElMessage.error(res.msg);
  }
};


handleQuery();
</script>
