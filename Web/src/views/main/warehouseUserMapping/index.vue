<template>
  <div class="warehouseUserMapping-container">
    <el-dialog v-model="isShowDialog" title="仓库用户关系" :width="700" draggable="">
      <!-- <el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px">
        <el-row :gutter="35">
          <el-form-item v-show="false">

          </el-form-item>
        </el-row>
      </el-form> -->
      <el-checkbox-group v-model="state.warehouseUserCheck">
        <el-checkbox v-for="warehouse in state.warehouses" :label="warehouse.warehouseName" :key="warehouse.id"
          @change="handleCheckedCitiesChange($event, warehouse)">
          {{ warehouse.warehouseName }}</el-checkbox>
      </el-checkbox-group>
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="cancel" size="default">取 消</el-button>
          <el-button type="primary" @click="submit" size="default">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>

<script lang="ts" setup="" name="warehouseUserMapping">
// import { ref } from "vue";
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

// import editDialog from '/@/views/main/warehouseUserMapping/component/editDialog.vue'
import { allWMSWarehouse } from '/@/api/main/wMSWarehouse';
import { listWarehouseUserMapping, addWarehouseUserMapping } from '/@/api/main/warehouseUserMapping';
import Warehouse from "/@/entities/Warehouse";
import WarehouseUser from "/@/entities/warehouseUserMapping";



const loading = ref(false);


var props = defineProps({
  title: {
    type: String,
    default: "",
  },
});

const state = reactive({
  loading: false,
  isShowDialog: false,

  warehouses: new Array<Warehouse>(),
  warehouseUserCheck: [],
  warehouseUsers: new Array<WarehouseUser>(),
  warehouseUser: new WarehouseUser(),
  user: {},
});


//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});

// 打开弹窗
const openDialog = async (row: any) => {
  state.user = row;
  console.log(state.user);
  // console.log("dasdsadasdas");
  // state.selectedTabName = '0'; // 重置为第一个 tab 页
  // state.ruleForm = JSON.parse(JSON.stringify(row));
  // if (row.id != undefined) {
  // 	var resRole = await getAPI(SysUserApi).apiSysUserOwnRoleListUserIdGet(row.id);
  // 	state.ruleForm.roleIdList = resRole.data.result;
  // 	var resExtOrg = await getAPI(SysUserApi).apiSysUserOwnExtOrgListUserIdGet(row.id);
  // 	state.ruleForm.extOrgIdList = resExtOrg.data.result;
  isShowDialog.value = true;
  handleQuery(row);
  // state.isShowDialog = false;
  // } else state.isShowDialog = true;
};

//自行添加其他规则
// 		const rules = ref<FormRules>({
// });
// 查询操作
const handleQuery = async (row: any) => {
  loading.value = true;
  state.warehouses = [];
  state.warehouseUserCheck = [];
  // console.log("Object.assign(row)");
  // console.log(Object.assign(row));
  // console.log(row);
  var warehouses = await allWMSWarehouse();
  state.warehouseUsers = [];
  state.warehouses = warehouses.data.result;
  state.warehouses.forEach(a => {
    state.warehouseUsers.push({
      warehouseId: a.id,
      warehouseName: a.warehouseName,
      status: -1,
      userId: state.user.id,
      userName: state.user.account
    })
  })

  console.log("row")
  console.log(row)

  var warehouseUserCheck = await listWarehouseUserMapping({ "userId": row.id });

  //  console.log("warehouses")
  //  console.log(state.warehouses)
  //  console.log(warehouseUserCheck.data.result)
  state.warehouseUserCheck = [];
  warehouseUserCheck.data.result.forEach(a => {
    if (a.status == 1) {
      state.warehouseUserCheck.push(a.warehouseName);
    }
    state.warehouseUsers.forEach(b => {
      if (b.warehouseName == a.warehouseName) {
        b.status = 1;
      }
    })
  })


  // console.log("state.warehouseUserCheck")
  // console.log(state.warehouseUserCheck);
  loading.value = false;
};

//点击多选框 处理数据
const handleCheckedCitiesChange = async (e, w) => {
  state.warehouseUsers.forEach(a => {
    if (a.warehouseName == w.warehouseName) {
      a.status = e ? 1 : -1
    }
  })
  // console.log("row");
  // console.log(e);
  // console.log(w);
  // state.warehouseUsers.push(row);
  //  console.log( state.warehouseUsers);
  //  console.log( state.warehouseUserCheck);
  //  console.log( state.warehouses);

  // state.warehouseUsers.forEach(a => {
  //   if (a.warehouseName == row.warehouseName) {
  //     a.status = value ? 1 : -1
  //   }
  // })
  // console.log(this.warehouseUsers)
}

// 取消
const cancel = () => {
  isShowDialog.value = false;
};


// 提交
const submit = async () => {
  console.log("state.warehouseUserCheck");
  console.log(state.warehouseUsers);
  await addWarehouseUserMapping(state.warehouseUsers);
  isShowDialog.value = false;

};



// handleQuery();
//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>


