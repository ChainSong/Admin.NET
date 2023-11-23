<template>
	<div class="sys-formDes-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<v-form-render :form-json="state.formJson" :form-data="state.formData" :key="state.formJson"
				:option-data="state.optionData" ref="vFormRef">
			</v-form-render>
			<el-button type="primary" @click="get">查询</el-button>
			<el-button type="primary" @click="initialize">重置</el-button>


		</el-card>
		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="formData" stripe style="width: 100%">
				<el-table-column v-for="(book, key, index) in formData[0]" :key="index" :prop="key" :label="key"
					align="center">
				</el-table-column>
			</el-table>
		</el-card>

		<el-dialog title="提示" v-model="state.dialogVisible" width="60%" :before-close="handleClose">
			<el-steps :active="state.active" finish-status="success">
				<el-step title="请录入界面设计"></el-step>
				<el-step title="请录入数据源"></el-step>
				<el-step title="请录入菜单名称"></el-step>
			</el-steps>
			<div v-if="state.active == 1">
				<el-input type="textarea" v-model="state.configModel.uiCode" show-word-limit resize="none" :rows="5"
					class="textarea-box"></el-input>

				<!-- <el-button type="primary" @click="activeOK">确 定1</el-button> -->
			</div>
			<div v-if="state.active == 2">
				<el-input type="textarea" v-model="state.configModel.sqlCode" maxlength="1000" show-word-limit resize="none"
					:rows="5" class="textarea-box"></el-input>
				<!-- <el-button type="primary" @click="activeOK">确 定2</el-button> -->
			</div>
			<div v-if="state.active == 3">
				<el-input type="textarea" v-model="state.configModel.menuName" maxlength="15" show-word-limit resize="none"
					:rows="5" class="textarea-box"></el-input>
				<!-- <el-button type="primary" @click="activeOK">确 定3</el-button> -->
			</div>
			<span class="dialog-footer">
				<!-- <el-button @click="state.dialogVisible = false">取 消</el-button> -->
				<el-button type="primary" @click="state.active--">上一步</el-button>
				<el-button type="primary" @click="activeOK">下一步</el-button>
			</span>
		</el-dialog>

	</div>
</template>
<script lang="ts" setup name="renderPage">
import { ref, onMounted } from 'vue';
import { ElMessageBox, ElMessage } from "element-plus";
import { getData, addWMSLowCode, getWMSLowCode, initializeLowCode, deleteWMSLowCode } from "/@/api/main/wMSLowCode";

import { useRoute, useRouter, onBeforeRouteUpdate, RouteRecordRaw } from 'vue-router';
const state = ref({
	formJson: {},
	formData: [],
	optionData: [],
	dialogVisible: true,
	active: 1,
	configModel: { uiCode: "", sqlCode: "", menuName: "" }
});
let formData = ref("");
let optionData = ref("");
let vFormRef = ref(null);
const route = useRoute()
// let dialogVisible = true;

const handleClose = () => {
	state.value.dialogVisible = false;
}
const activeOK = async () => {
	// alert(state.value.active);
	if (state.value.active++ > 2) {
		state.value.active=3;
		if(state.value.configModel.uiCode==""){
			ElMessage.error("请输入UI设计");
			return;
		}
		if(state.value.configModel.sqlCode==""){
			ElMessage.error("请输入数据源");
			return;
		}
		if(state.value.configModel.menuName==""){
			ElMessage.error("请输入菜单名称");
			return;
		}
		if(state.value.configModel.uiCode==""){
			ElMessage.error("请输入UI设计");
			return;
		}
		state.value.formJson = JSON.parse(state.value.configModel.uiCode);
		// alert("dasdad");
		var res = await addWMSLowCode(state.value.configModel);
		if (res.data.result.code == 1) {
			state.value.dialogVisible = false;
			ElMessage.success(res.data.result.msg);
		} else {
			ElMessage.error(res.data.result.msg);
		}

	}
	//  console.log("开始赋值渲染");

}


const initialize = async () => {
	if (state.value.configModel.menuName != "") {
		ElMessageBox.confirm(`确定要删除菜单：` + state.value.configModel.menuName + '?', "提示", {
			confirmButtonText: "确定",
			cancelButtonText: "取消",
			type: "warning",
		})
			.then(async () => {
				await deleteWMSLowCode(state.value.configModel);
				//   handleQuery();
				ElMessage.success("删除成功");
				state.value.active = 1;
				state.value.dialogVisible = true;
			})
			.catch(() => { });
		// var res = await deleteWMSLowCode(state.value.configModel);
	} else {
		state.value.active = 1;
		state.value.dialogVisible = true;
	}


	//  ElMessage.success("");
};
const get = async () => {


	var res = await getData({ "formDataModel": vFormRef.value.formDataModel, "name": state.value.configModel.menuName });
	let result = res.data.result.data ?? [];
	// vFormRef.value.formDataModel
	state.value.formData = result;
	state.value.optionData = result;
	// console.log(result);
	formData.value = result;
	optionData.value = result;


	// console.log(formData.value);
	// console.log(optionData.value);
	// console.log("vFormRef.value.formDataModel");
	// console.log(vFormRef.value.formDataModel);
	// console.log(vFormRef.value);

};

// state.value.formJson ={"widgetList":[],"formConfig":{"labelWidth":80,"labelPosition":"left","size":"","labelAlign":"label-left-align","cssCode":"","customClass":"","functions":"","layoutType":"PC","onFormCreated":"","onFormMounted":"","onFormDataChange":""}};

// 页面加载时
onMounted(async () => {

	// console.log( useRoute)
	// console.log(  useRouter )

	state.value.dialogVisible = true;
	// dialogVisible= true;
	// console.log("state.value.formJson");
	// console.log(vFormRef.value);
	// console.log(state.value.formJson);
	// submitForm();
});

</script>
<style lang="scss" scoped>
body {
	margin: 0; // 去除页面垂直滚动条
}
</style>