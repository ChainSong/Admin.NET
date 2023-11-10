<template>
	<div class="wMSPreOrder-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">

			<el-card>
				<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
					<el-row :gutter="35">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
							v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
							<el-form-item :label="i.displayName" v-if="i.isCreate" style="width: 90%;height: 45px;"
								:prop="i.columnName">
								<template v-if="i.type == 'TextBox'">
									<el-input placeholder="请输入内容" size="small" style="width:90%" :disabled="i.isUpdate = 1"
										v-model="state.header[i.columnName]" v-if="i.isCreate">
									</el-input>
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
										:disabled="i.isCreate = 1" placeholder="请选择" size="small" style="width:90%"
										filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
											:label="item.name" :value="item.codeInt">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DropDownListStr'">
									<el-select v-model="state.header[i.columnName]" v-if="i.isCreate" placeholder="请选择"
										:disabled="i.isUpdate = 1" size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
											:label="item.name" :value="item.codeStr">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DropDownListStrRemote'">

									<select-Remote :objData="state.header" :isDisabled="i.isCreate" :columnData="i"
										:defaultvValue="state.header[i.columnName]"
										@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationDBColumn] = data.value; console.log(state.header) }"></select-Remote>

								</template>

								<template v-if="i.type == 'DatePicker'">
									<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
										:disabled="i.isCreate = 0" type="date" placeholder="选择日期" size="small"
										style="width:90%">
									</el-date-picker>
								</template>
								<template v-if="i.type == 'DateTimePicker'">
									<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
										:disabled="i.isCreate = 0" type="datetime" start-placeholder="选择日期时间" size="small"
										style="width:90%">
									</el-date-picker>
								</template>
							</el-form-item>
						</el-col>
					</el-row>
				</el-form>
			</el-card>
			<el-button @click="handleAdd" type="primary" size="large" class="toolbar-btn">添加一条</el-button>
			<el-card>
				<el-form label-position="top" :model="state" ref="detailRuleRef" :rules="detailRule">
					<el-table :data="state.details" height="250">
						<template v-for="(v, index) in state.tableColumnDetails">
							<el-table-column v-if="v.isCreate" :key="index" style="margin:0;padding:0;" :fixed="false"
								:prop="v.columnName" :label="v.displayName" width="150">
								<template #default="scope">
									<el-form-item :key="scope.row.key" style="margin:0;padding:0;"
										:prop="'details.' + scope.$index + '.' + v.columnName"
										:rules="detailRule[v.columnName]">

										<template v-if="v.type == 'DropDownListInt'">
											<el-select v-model="state.details[scope.$index][v.columnName]" v-if="v.isCreate"
												:disabled="v.update" placeholder="请选择" style="width: 100%">
												<el-option v-for="item in v.tableColumnsDetails" :key="item.codeInt"
													:label="item.name" :value="item.codeInt">
												</el-option>
											</el-select>
										</template>
										<template v-else-if="v.type == 'DropDownListStr'">
											<el-select v-model="state.details[scope.$index][v.columnName]" v-if="v.isCreate"
												:disabled="v.update" placeholder="请选择" style="width: 100%">
												<el-option v-for="item in v.tableColumnsDetails" :key="item.codeStr"
													:label="item.name" :value="item.codeStr">
												</el-option>
											</el-select>
										</template>
										<template v-else-if="v.type == 'DropDownListStrRemote'">
											<select-Remote :objData="state.header" :isDisabled="v.update" :columnData="v"
												:defaultvValue="state.details[scope.$index][v.columnName]"
												@select:model="data => { state.details[scope.$index][v.columnName] = data.text; state.details[scope.$index][v.relationColumn] = data.value; console.log(state.details[scope.$index]) }"></select-Remote>
										</template>
										<template v-else-if="v.type == 'DatePicker'">
											<el-date-picker v-model="state.details[scope.$index][v.columnName]"
												v-if="v.isCreate" :disabled="v.isUpdate" type="date" placeholder="选择日期"
												style="width: 100%">
											</el-date-picker>
										</template>
										<template v-else-if="v.type == 'DatePicker'">
											<el-date-picker v-model="state.details[scope.$index][v.columnName]"
												v-if="v.isCreate" :disabled="v.isUpdate" type="date" placeholder="选择日期"
												style="width: 100%">
											</el-date-picker>
										</template>
										<template v-else-if="v.type == 'InputNumber'">
											<el-input-number placeholder="请输入内容"
												v-model="state.details[scope.$index][v.columnName]" v-if="v.isCreate"
												:disabled="v.update"></el-input-number>
										</template>
										<template v-else>
											<el-input placeholder="请输入内容"
												v-model="state.details[scope.$index][v.columnName]" v-if="v.isCreate"
												:disabled="v.update">
											</el-input>
										</template>
									</el-form-item>
								</template>
							</el-table-column>
						</template>
						<el-table-column>
							<template #default="scope">
								<el-button size="mini" type="primary" @click="handleDelete(scope.$index)">删除</el-button>
							</template>
						</el-table-column>
					</el-table>
				</el-form>
			</el-card>
			<div>
				<!-- <Button @click="cancel">{{ L("Cancel") }}</Button>
				<Button @click="save" type="primary">{{ L("OK") }}</Button> -->
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
import { addWMSPreOrder, updateWMSPreOrder, getWMSPreOrder } from "/@/api/main/wMSPreOrder";

import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/preOrder";
import Detail from "/@/entities/preOrderDetail";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue'
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
	details: new Array<Detail>(),


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
// const ruleForm = ref<any>({});
//自行添加其他规则
// const rules = ref<FormRules>({});

//添加一行明细
const handleAdd = (row: any) => {
	state.value.details.push(new Detail());
}
//删除一行明细
const handleDelete = (index: any) => {
	state.value.details.splice(index, 1);
}
// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.header = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
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
	state.value.header.details = state.value.details

	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			detailRuleRef.value.validate(async (isValidDetail: boolean, fieldsDetail?: any) => {
				if (isValidDetail) {
					let result = await updateWMSPreOrder(state.value.header);
					if (result.data.result.code == "1") {
						ElMessage.success("修改成功");
						closeDialog();
					} else {
						ElMessage.error("修改失败");
					}
				} else {
					ElMessage({
						message: `表单明细有${Object.keys(fieldsDetail).length}处验证失败，请修改后再提交`,
						type: "error",
					});
				}
			})
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}

	});
};

const get = async () => {
	let result = await getWMSPreOrder(state.value.header.id);
	state.value.header = result.data.result;
	state.value.details = result.data.result.details;
}

const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_PreOrder");
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
	let resDetail = await getByTableNameList("WMS_PreOrderDetail");
	// console.log("asdasdasdasdasdasddasdas")
	// console.log(resDetail);
	state.value.tableColumnDetails = resDetail.data.result;
	detailRule.value = {};
	state.value.tableColumnDetails.forEach((a) => {
		if (a.validation.toUpperCase() == "Required".toUpperCase()) {
			//  console.log("添加验证"+a.columnName)
			detailRule.value[a.columnName] = [
				{
					required: true,
					message: a.displayName,
					trigger: "blur",
				},
			];
		}
	});
	// console.log(" state.value.tableColumnDetails")
	// console.log(state.value.tableColumnDetails)
	// console.log(state.value.header)
	// let resDetail = await getByTableNameList("CustomerDetail");
	// state.value.tableColumnHeaders = res.data.result;

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




