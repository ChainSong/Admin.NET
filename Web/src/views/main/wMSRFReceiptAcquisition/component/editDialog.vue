<template>
	<div class="wMSRFReceiptAcquisition-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" :height="700" draggable="">
			<div style="height: 500px;">
				<el-form :model="state.vm.form" ref="queryForm" :height="700" :inline="true">
					<el-form-item label="外部单号" style="width: 100%;">
						<el-input v-model="state.vm.form.externReceiptNumber" clearable="" placeholder="请输入外部单号" />
					</el-form-item>
					<el-form-item label="扫描框" style="width: 100%;">
						<el-input v-model="state.vm.form.scanInput"  v-focus="input" v-select="input" ref="input"   
							v-on:keyup.enter="scanAcquisition" clearable="" placeholder="请扫描" />
					</el-form-item>
					<el-form-item label="SKU" style="width: 100%;">
						<el-input v-model="state.vm.form.sku" clearable="" placeholder="SKU" />
					</el-form-item>
					<el-form-item label="Lot" style="width: 100%;">
						<el-input v-model="state.vm.form.lot" clearable="" placeholder="Lot" />
					</el-form-item>
					<el-form-item label="效期" style="width: 100%;">
						<el-input v-model="state.vm.form.expirationDate" clearable="" placeholder="效期" />
					</el-form-item>
					<el-form-item label="SN" style="width: 100%;">
						<el-input v-model="state.vm.form.sn" clearable="" placeholder="SN" />
					</el-form-item>
				</el-form>
			</div>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted, nextTick } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSRFReceiptAcquisition, updateWMSRFReceiptAcquisition, saveAcquisition } from "/@/api/main/wMSRFReceiptAcquisition";
//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});
const input = ref();
//自行添加其他规则
const rules = ref<FormRules>({
});

const state = ref({
	vm: {
		id: "",
		form: {} as any,
		data: [] as any,
	},
	visible: false,
	loading: false,
});


// 打开弹窗
const openDialog = (row: any) => {
	ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.vm.form = ruleForm.value;
	isShowDialog.value = true;
};

// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	isShowDialog.value = false;
};

const scanAcquisition = async () => {

	state.value.vm.form.sku = "";
	state.value.vm.form.lot = "";
	state.value.vm.form.expirationDate = "";
	state.value.vm.form.sn = "";
	// let acquisitionData=JSON.parse(state.value.vm.form.scanInput);split('-')
	let acquisitionData = state.value.vm.form.scanInput.split('|');
 
	if (acquisitionData.length == 3) {
		state.value.vm.form.sku = acquisitionData[1];
		state.value.vm.form.sn = acquisitionData[2] ?? "";
	} else {
		 
		state.value.vm.form.sku = acquisitionData[1];
		state.value.vm.form.lot = acquisitionData[2] ?? "";
		state.value.vm.form.expirationDate = acquisitionData[3] ?? "";
		state.value.vm.form.sn = acquisitionData[4] ?? "";
	}
	let res = await saveAcquisition(state.value.vm.form);
	if (res.data.result.code == "1") {
		ElMessage.success("操作成功");
	} else {
		ElMessage.error("操作失败:" + res.data.result.msg);
	}
	nextTick(() => {
		input.value.focus();
		input.value.select();
	});

	//   state.value.tableColumnHeaders = res.data.result;
};
// // 取消
// const cancel = () => {
//   isShowDialog.value = false;
// };

// // 提交
// const submit = async () => {
//   ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
//     if (isValid) {
//       let values = ruleForm.value;
//       if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
//         await updateWMSRFReceiptAcquisition(values);
//       } else {
//         await addWMSRFReceiptAcquisition(values);
//       }
//       closeDialog();
//     } else {
//       ElMessage({
//         message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
//         type: "error",
//       });
//     }
//   });
// };





// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




