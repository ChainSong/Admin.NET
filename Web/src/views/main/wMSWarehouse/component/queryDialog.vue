<template>
	<div class="wMSCustomer-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-container>
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
										<el-tag   v-if="item.codeStr == state.header[i.columnName]"  v-bind:key="item.color" show-icon :type="item.color">
													{{ item.name }}
												</el-tag>
										<!-- <label v-if="item.codeStr == state.header[i.columnName]" v-text="item.name"
											show-icon :type="item.color" :key="item.codeStr"></label> -->
									</template>
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<template v-for="item in i.tableColumnsDetails">
										<template v-if="item.codeStr == state.header[i.columnName]">
											<el-tag   v-if="item.codeInt == state.header[i.columnName]"  v-bind:key="item.color" show-icon :type="item.color">
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
			</el-container>
			<el-container title="明细信息">
				<el-main>
					<el-form>
						<el-table :data="state.details" style="width: 100%" height="250">
							<template v-for="(v, index) in state.tableColumnDetails">
								<el-table-column v-if="v.isCreate" :key="index" :fixed="false" :label="v.displayName"
									width="150">
									<template #default="scope">
										<label v-text="scope.row[v.columnName]"></label>
									</template>
								</el-table-column>
							</template>
						</el-table>
					</el-form>
				</el-main>
			</el-container>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="cancel" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSWarehouse, updateWMSWarehouse, getWMSWarehouse } from "/@/api/main/wMSWarehouse";
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/warehouse";
// import Detail from "/@/entities/customerDetail";

import Detail from "/@/entities/warehouseDetail";
import TableColumns from "/@/entities/tableColumns";
// import { getWMSCustomer } from "/@/api/main/wMSCustomer";

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
	console.log("dasdsdasd");
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
	let res = await getByTableNameList("WMS_Warehouse");
	state.value.tableColumnHeaders = res.data.result;

	let resDetail = await getByTableNameList("WMS_WarehouseDetail");
	state.value.tableColumnDetails = resDetail.data.result;
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
	// let resDetail = await getByTableNameList("CustomerDetail");
	// console.log("asdasdasdasdasdasddasdas")
	// console.log(resDetail);
	// state.value.tableColumnDetails = resDetail.data.result;
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
	// console.log("dasdasd");
	// console.log(state.value.header);
	let result = await getWMSWarehouse(state.value.header.id);
	// console.log(result);
	state.value.header = result.data.result;
	state.value.details = result.data.result.details;
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




