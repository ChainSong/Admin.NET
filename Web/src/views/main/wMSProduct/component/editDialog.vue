﻿<template>
	<div class="wMSProduct-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">

			<el-card>
				<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
					<el-row :gutter="35">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
							v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
							<el-form-item :label="i.displayName"  style="width: 90%;height: 45px;"
								:prop="i.columnName">
								<template v-if="i.type == 'TextBox'">
									<el-input placeholder="请输入内容" size="small" style="width:90%"
										v-model="state.header[i.columnName]"  >
									</el-input>
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<el-select v-model="state.header[i.columnName]"  placeholder="请选择"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
											:label="item.name" :value="item.codeInt">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DropDownListStrRemote'">
									<select-Remote :whereData="state.header" :isDisabled="i.isUpdate" :columnData="i" 
										:defaultvValue="state.header[i.columnName]"
										@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>

								</template>
								<template v-if="i.type == 'DropDownListStr'">

									<el-select v-model="state.header[i.columnName]" placeholder="请选择"  :isDisabled="i.isUpdate==0"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
											:label="item.name" :value="item.codeStr">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DatePicker'">
									<el-date-picker v-model="state.header[i.columnName]"  type="date" :isDisabled="i.isUpdate==0"
										placeholder="选择日期" size="small" style="width:90%">
									</el-date-picker>
								</template>
								<template v-if="i.type == 'DateTimePicker'">
									<el-date-picker v-model="state.header[i.columnName]"  type="datetime" :isDisabled="i.isUpdate==0"
										start-placeholder="选择日期时间" size="small" style="width:90%">
									</el-date-picker>
								</template>
							</el-form-item>
						</el-col>
					</el-row>
				</el-form>
			</el-card>

			<div>
			</div>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSProduct, updateWMSProduct, getWMSProduct } from "/@/api/main/wMSProduct";
import selectRemote from '/@/views/tools/select-remote.vue'
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/Product";
// import Detail from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";

//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});


const state = ref({
	// vm: {
	// 	id: "",
	// 	form: {
	// 		details: []
	// 	} as any,
	// 	header: new Header(),
	// 	details: new Array<Detail>(),
	// },
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	// details: new Array<Detail>(),


	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>()
	// header: new Array<Details>(),
})

let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});


//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);

// headerRule : {},
//     detailRule :{},

const isShowDialog = ref(false);

// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.header = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
	gettableColumn();
	get();
};

// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	isShowDialog.value = false;
};

// 取消
const cancel = () => {
	isShowDialog.value = false;
};

// 提交
const submit = async () => {
	// console.log("state.value.details");
	// console.log(state.value.details);
	// state.value.header.details = state.value.details
	// console.log(state.value.header);

	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {

			let result = await updateWMSProduct(state.value.header);
			if (result.data.result.code == "1") {
				closeDialog();
				ElMessage.success("修改成功");
			} else {
				ElMessage.error("修改失败:"+result.data.result.msg);
			}

		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}

	});
};

const get = async () => {
	let result = await getWMSProduct(state.value.header.id);
	state.value.header = result.data.result;
	// state.value.details = result.data.result.details;
}

const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_Product");
	state.value.tableColumnHeaders = res.data.result;
	headerRule.value = {};
	//验证
	state.value.tableColumnHeaders.forEach((a) => {
		if (a.validation.toUpperCase() == "Required".toUpperCase()) {
			//  console.log("添加验证"+a.columnName)
			headerRule.value[a.columnName] = [
				{
					required: true,
					message: a.displayName,
					trigger: "blur",
				},
			];
		}
	});


};

// 页面加载时
onMounted(async () => {
	gettableColumn();
	// get();
	// state.value.vm.details = [new Detail()];
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




