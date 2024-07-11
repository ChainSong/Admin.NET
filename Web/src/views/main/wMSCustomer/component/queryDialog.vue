<template>
	<div class="wMSCustomer-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">

			<el-tabs v-model="activeName">
				<el-tab-pane label="基本信息" name="first">
					<el-main>
						<el-descriptions class="margin-top" :column="2" size="small" border>
							<template v-for="i in state.tableColumnHeaders">
								<el-descriptions-item v-bind:key="i.id" :prop="i.displayName" :label="i.displayName"
									v-if="i.isCreate">
									<template>
										<!-- <i></i>
									{{ i.displayName }} -->
									</template>
									<template v-if="i.type == 'TextBox'">
										<label font-family="Helvetica Neue" v-text="state.header[i.columnName]"></label>
									</template>
									<template v-if="i.type == 'DropDownListStr'">
										<template v-for="item in i.tableColumnsDetails">
											<el-tag v-if="item.codeStr == state.header[i.columnName]"
												v-bind:key="item.color" show-icon :type="item.color">
												{{ item.name }}
											</el-tag>
											<!-- <label v-if="item.codeStr == state.header[i.columnName]" v-text="item.name"
											show-icon :type="item.color" :key="item.codeStr"></label> -->
										</template>
									</template>
									<template v-if="i.type == 'DropDownListInt'">
										<template v-for="item in i.tableColumnsDetails">
											<template v-if="item.codeStr == state.header[i.columnName]">
												<el-tag v-if="item.codeInt == state.header[i.columnName]"
													v-bind:key="item.color" show-icon :type="item.color">
													{{ item.name }}
												</el-tag>
												<!-- <label show-icon :type="item.color" v-text="item.name"
												:key="item.codeInt"></label> -->
											</template>
										</template>
									</template>
								</el-descriptions-item>
							</template>
						</el-descriptions>
					</el-main>



					<el-container title="明细信息">
						<el-main>
							<el-form>
								<el-table :data="state.details" style="width: 100%" height="250">
									<template v-for="(v, index) in state.tableColumnDetails">
										<el-table-column v-if="v.isCreate" :key="index" :fixed="false"
											:label="v.displayName" width="150">
											<template #default="scope">
												<label v-text="scope.row[v.columnName]"></label>
											</template>
										</el-table-column>
									</template>
								</el-table>
							</el-form>
						</el-main>
					</el-container>
				</el-tab-pane>
				<el-tab-pane label="配置附件" name="fourth">
					<el-row :gutter="35">
						<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" v-for="q in state.tableColumnConfigs"
							v-bind:key="q.id">
							<el-form-item :label="q.displayName" v-if="q.isCreate" style="width: 90%;height: 150px;"
								:prop="q.columnName">
								<template v-if="q.type == 'UploadImg'">
									<el-image style="width: 100px; height: 100px"
										:src="baseURL + state.customerConfig[q.columnName]"></el-image>
								</template>
								<template v-if="q.type == 'TextBox'">
									<label font-family="Helvetica Neue"
										v-text="state.customerConfig[q.columnName]"></label>
								</template>
							</el-form-item>
						</el-col>
					</el-row>

				</el-tab-pane>
			</el-tabs>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<!-- <el-button type="primary" @click="cancel" size="default">确 定</el-button> -->
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSCustomer, updateWMSCustomer, getWMSCustomer } from "/@/api/main/wMSCustomer";
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/customer";
import Detail from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";
import CustomerConfig from "/@/entities/customerConfig";
import { Local, Session } from '/@/utils/storage';

const state = ref({
	vm: {
		id: "",
		form: {
			details: []
		} as any,
		header: new Header(),
		details: new Array<Detail>(),
	},
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Detail>(),



	customerConfig: new CustomerConfig(),

	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),
	tableColumnConfig: new TableColumns(),
	tableColumnConfigs: new Array<TableColumns>(),
	// header: new Array<Details>(),
})

let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});


// 主体路径
let baseURL = import.meta.env.VITE_API_URL;

let activeName: string = 'first';
//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
// const ruleFormRef = ref();
const isShowDialog = ref(false);
// const ruleForm = ref<any>({});
//自行添加其他规则
// const rules = ref<FormRules>({
// });

// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.header = JSON.parse(JSON.stringify(row));

	isShowDialog.value = true;
	gettableColumn();
	get()
};

// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	isShowDialog.value = false;
};

// 取消
const cancel = () => {
	console.log("取消");
	isShowDialog.value = false;
};

// 提交
// const submit = async () => {
// 	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
// 		if (isValid) {
// 			let values = ruleForm.value;
// 			if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
// 				await updateWMSCustomer(values);
// 			} else {
// 				await addWMSCustomer(values);
// 			}
// 			closeDialog();
// 		} else {
// 			ElMessage({
// 				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
// 				type: "error",
// 			});
// 		}
// 	});
// };


const gettableColumn = async () => {
	let res = await getByTableNameList("Customer");
	state.value.tableColumnHeaders = res.data.result;
	// headerRule.value = {};
	//验证
	// state.value.tableColumnHeaders.forEach((a) => {
	// 	if (a.validation.toUpperCase() == "Required".toUpperCase()) {
	// 		//  console.log("添加验证"+a.columnName)
	// 		headerRule.value[a.columnName] = [
	// 			{
	// 				required: true,
	// 				message: a.displayName,
	// 				trigger: "blur",
	// 			},
	// 		];
	// 	}
	// });
	let resDetail = await getByTableNameList("CustomerDetail");
	// console.log("asdasdasdasdasdasddasdas")
	// console.log(resDetail);
	state.value.tableColumnDetails = resDetail.data.result;

	let restableColumnConfig = await getByTableNameList("CustomerConfig");

	state.value.tableColumnConfigs = restableColumnConfig.data.result;
	// detailRule.value = {};
	// // state.value.tableColumnDetails.forEach((a) => {
	// // 	if (a.validation.toUpperCase() == "Required".toUpperCase()) {
	// // 		//  console.log("添加验证"+a.columnName)
	// // 		detailRule.value[a.columnName] = [
	// // 			{
	// // 				required: true,
	// // 				message: a.displayName,
	// // 				trigger: "blur",
	// // 			},
	// // 		];
	// // 	}
	// // });
	// console.log(" state.value.tableColumnDetails")
	// console.log(state.value.tableColumnDetails)
	// console.log(state.value.header)
	// let resDetail = await getByTableNameList("CustomerDetail");
	// state.value.tableColumnHeaders = res.data.result;

};

const get = async () => {
	console.log("dasdasd");
	state.value.customerConfig = [];
	state.value.details = [];
	console.log(state.value.header);
	let result = await getWMSCustomer(state.value.header.id);
	console.log(result);
	if (result.data.result != null) {
		state.value.header = result.data.result;
		state.value.details = result.data.result.details;
		// state.value.customerConfig = result.data.result.customerConfig;
		if (result.data.result.customerConfig == null) {
			state.value.customerConfig = new CustomerConfig();
		} else {
			state.value.customerConfig = result.data.result.customerConfig;
		}
	}
	// console.log(state.value.header);
	// console.log(state.value.details);

}


// 页面加载时
onMounted(async () => {
	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
