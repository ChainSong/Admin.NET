<template>
  <div class="supplierUserMapping-container">
    <el-dialog v-model="isShowDialog" title="客户用户关系" :width="700" draggable="">
      <el-checkbox-group v-model="state.supplierUserCheck">
        <el-checkbox v-for="supplier in state.suppliers" :label="supplier.supplierName" :key="supplier.id"
          @change="handleCheckedCitiesChange($event, supplier)">
          {{ supplier.supplierName }}</el-checkbox>
      </el-checkbox-group>
       
      <template #footer>
        <span class="dialog-footer">
          <el-button @click="cancel" size="default"  >取 消</el-button>
          <el-button type="primary" @click="submit" size="default">确 定</el-button>
        </span>
      </template>
    </el-dialog>
  </div>
</template>
<script lang="ts" setup="" name="supplierUserMapping">
// import { ref } from "vue";
import { onMounted, reactive, ref } from 'vue';
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
//import { formatDate } from '/@/utils/formatTime';

// import editDialog from '/@/views/main/supplierUserMapping/component/editDialog.vue'
import { listSupplierUserMapping,addSupplierUserMapping } from '/@/api/main/supplierUserMapping';
import {  allMMSSupplier } from '/@/api/main/mMSSupplier';
import Supplier from "/@/entities/supplier";
import SupplierUser from "/@/entities/supplierUserMapping";
 

const loading = ref(false);


var props = defineProps({
  title: {
    type: String,
    default: "",
  },
});
// const emits = defineEmits(['handleQuery']);
// const ruleFormRef = ref();
const state = reactive({
  loading: false,
  isShowDialog: false,
  suppliers: new Array<Supplier>(),
  supplierUserCheck: [],
  supplierUsers: new Array<SupplierUser>(),
  supplierUser: new SupplierUser(),
  user: {},
});


//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});

// 打开弹窗
const openDialog = async (row: any) => {
  state.user=row;
  
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
  state.suppliers = [];
  state.supplierUserCheck = [];
  // console.log("Object.assign(row)");
  // console.log(Object.assign(row));
  // console.log(row);
  var suppliers = await allMMSSupplier();
  state.supplierUsers = [];
  state.suppliers = suppliers.data.result;
  state.suppliers.forEach(a => {
    state.supplierUsers.push({
      supplierId: a.id,
      supplierName: a.supplierName,
      status: -1,
      userId: state.user.id,
      userName: state.user.account
    })
  })

  var supplierUserCheck = await listSupplierUserMapping({ UserId: row.id });

  supplierUserCheck.data.result.forEach(a => {
    if (a.status == 1) {
      state.supplierUserCheck.push(a.supplierName);
    }
    state.supplierUsers.forEach(b => {
      if (b.supplierName == a.supplierName) {
        b.status = 1;
      }
    })
  })

  
  // suppliers.data.result.forEach(element => {
  //   state.suppliers.push(element.supplierName);
  // });
  // supplierUserCheck.data.result.forEach(element => {
  //   if (element.status == 1) {
  //     state.supplierUserCheck.push(element.supplierName);
  //   }
  // });
  loading.value = false;
};

//点击多选框 处理数据
const handleCheckedCitiesChange = async (e, w) => {
  state.supplierUsers.forEach(a => {
      if (a.supplierName == w.supplierName) {
        a.status = e ? 1 : -1
      }
    })
  // console.log("row");
  // console.log(e);
  // console.log(w);
  // state.supplierUsers.push(row);
  //  console.log( state.supplierUsers);
  //  console.log( state.supplierUserCheck);
  //  console.log( state.suppliers);

  // state.supplierUsers.forEach(a => {
  //   if (a.supplierName == row.supplierName) {
  //     a.status = value ? 1 : -1
  //   }
  // })
  // console.log(this.supplierUsers)
}

// 取消
const cancel = () => {
  isShowDialog.value = false;
};


// 提交
const submit = async () => {
  console.log("state.supplierUserCheck");
  console.log(state.supplierUsers);
  await addSupplierUserMapping(state.supplierUsers);
  isShowDialog.value = false;
  // ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
  // 	if (isValid) {
  // 		let values = ruleForm.value;
  // 		if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
  // 			await updateWarehouseUserMapping(values);
  // 		} else {
  // 			await addWarehouseUserMapping(values);
  // 		}
  // 		closeDialog();
  // 	} else {
  // 		ElMessage({
  // 			message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
  // 			type: "error",
  // 		});
  // 	}
  // });
};

// onMounted(async () => {
//   state.loading = true;
//   // var res = await getAPI(SysPosApi).apiSysPosListGet();
//   // state.posData = res.data.result ?? [];
//   // var res1 = await getAPI(SysRoleApi).apiSysRoleListGet();
//   // state.roleData = res1.data.result ?? [];
//   // state.loading = false;
// });
// // 取消
// const cancel = () => {
//   isShowDialog.value = false;
// };

// // 打开弹窗
// const openDialog = async (row: any) => {

//   isShowDialog.value = true;
// };
// const editDialogRef = ref();
// const loading = ref(false);
// const tableData = ref<any>
//   ([]);
// const queryParams = ref<any>
//   ({});
// const tableParams = ref({
//   page: 1,
//   pageSize: 10,
//   total: 0,
// });
// const editSupplierUserMappingTitle = ref("");


// // 查询操作
// const handleQuery = async () => {
//   loading.value = true;
//   var res = await pageSupplierUserMapping(Object.assign(queryParams.value, tableParams.value));
//   tableData.value = res.data.result?.items ?? [];
//   tableParams.value.total = res.data.result?.total;
//   loading.value = false;
// };

// // 打开新增页面
// const openAddSupplierUserMapping = () => {
//   editSupplierUserMappingTitle.value = '添加客户用户关系';
//   editDialogRef.value.openDialog({});
// };

// // 打开编辑页面
// const openEditSupplierUserMapping = (row: any) => {
//   editSupplierUserMappingTitle.value = '编辑客户用户关系';
//   editDialogRef.value.openDialog(row);
// };

// // 提交
// const submit = async () => {

// };

// // 删除
// const delSupplierUserMapping = (row: any) => {
//   ElMessageBox.confirm(`确定要删除吗?`, "提示", {
//     confirmButtonText: "确定",
//     cancelButtonText: "取消",
//     type: "warning",
//   })
//     .then(async () => {
//       await deleteSupplierUserMapping(row);
//       handleQuery();
//       ElMessage.success("删除成功");
//     })
//     .catch(() => { });
// };

// // 改变页面容量
// const handleSizeChange = (val: number) => {
//   tableParams.value.pageSize = val;
//   handleQuery();
// };

// // 改变页码序号
// const handleCurrentChange = (val: number) => {
//   tableParams.value.page = val;
//   handleQuery();
// };


handleQuery();
//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>


