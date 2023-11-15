<template>
	<div class="tableColumns-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1300" draggable="">
			<el-table :data="state.tableColumns" show-overflow-tooltip border style="width: 150%">
				<el-table-column prop="tableName" label="表名" width="180">
				</el-table-column>
				<el-table-column fixed="left" prop="dbColumnName" label="字段名称" width="180">
				</el-table-column>
				<el-table-column prop="displayName" label="显示名称" width="150">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.displayName"></el-input>
					</template>
				</el-table-column>
				<el-table-column prop="isCreate" label="关键字(后台生成)">
					<template #default="scope">
						<el-switch v-model="scope.row.isKey" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="isCreate" label="添加">
					<template #default="scope">
						<el-switch v-model="scope.row.isCreate" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0" @change="CreateClose(scope.row, scope.$index)">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="isSearchCondition" label="查询条件">
					<template #default="scope">
						<el-switch v-model="scope.row.isSearchCondition" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="isShowInList" label="查询列表">
					<template #default="scope">
						<el-switch v-model="scope.row.isShowInList" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="isUpdate" label="可修改">
					<template #default="scope">
						<el-switch v-model="scope.row.isUpdate" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="isImportColumn" label="可导入">
					<template #default="scope">
						<el-switch v-model="scope.row.isImportColumn" active-color="#13ce66" inactive-color="#ff4949"
							:active-value="1" :inactive-value="0">
						</el-switch>
					</template>
				</el-table-column>
				<el-table-column prop="order" width="120px" label="排序">
					<template #default="scope">
						<el-input-number v-model="scope.row.order" style="width: 100px" size="mini" :min="1" :max="100"
							label="描述文字"></el-input-number>
					</template>
				</el-table-column>
				<el-table-column prop="validation" label="验证">
					<template #default="scope">
						<el-select v-model="scope.row.validation" size="small" placeholder="请选择">
							<el-option label="无" value=""> </el-option>
							<el-option label="Required" value="Required"> </el-option>
						</el-select>
						<!-- <el-input
                v-model="scope.row.validation"
                placeholder="请输入内容"
              ></el-input> -->
					</template>
				</el-table-column>
				<el-table-column prop="type" width="150px" label="类型">
					<template #default="scope">
						<el-select v-model="scope.row.type" size="small" placeholder="请选择">
							<el-option label="TextBox" value="TextBox"> </el-option>
							<el-option label="InputNumber " value="InputNumber"> </el-option>
							<el-option label="DropDownListInt" value="DropDownListInt"></el-option>
							<el-option label="DropDownListStr" value="DropDownListStr"></el-option>
							<el-option label="DropDownListStrRemote" value="DropDownListStrRemote"></el-option>
							<el-option label="DatePicker" value="DatePicker"> </el-option>
							<el-option label="DateTimePicker" value="DateTimePicker">
							</el-option>
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="associated" width="180px" label="拉下列表数据源">
					<template #default="scope">
						<!-- <el-input-number v-model="scope.row.associated" style="width: 100px" size="mini" :min="1" :max="100"
                label="拉下列表数据源"></el-input-number> -->
						<el-input size="small" v-model="scope.row.associated"></el-input>
					</template>
				</el-table-column>
				<el-table-column prop="relationDBColumn" width="180px" label="拉下列表关联字段">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.relationDBColumn"></el-input>
						<!-- <el-input-number v-model="scope.row.relationColumn" style="width: 100px" size="mini" :min="1" :max="100"
                label="拉下列表关联字段"></el-input-number> -->
					</template>
				</el-table-column>
				<el-table-column prop="relationColumn" width="120px" label="操作">
					<template #default="scope">
						<el-button @click="addDetailShow(scope.row, scope.$index)" type="text" size="small">添加明细</el-button>
					</template>
				</el-table-column>
				<!-- <el-table-column prop="associated" label="下拉"> </el-table-column> -->
			</el-table>

			<!-- </el-form> -->
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>

		<el-dialog title="添加" style="width:50%" v-model="dialogDetail" append-to-body :before-close="detailClose"
			width="30%">

			<el-table :data="state.tableColumnsDetails" border>
				<el-table-column prop="CodeInt" label="Int类型值" width="100">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.codeInt"></el-input>
					</template>
				</el-table-column>
				<el-table-column prop="CodeStr" label="Str类型值" width="100">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.codeStr"></el-input>
					</template>
				</el-table-column>
				<el-table-column prop="Name" label="显示描述">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.name"></el-input>
					</template>
				</el-table-column>
				<el-table-column prop="Color" label="颜色">
					<template #default="scope">
						<el-select v-model="scope.row.color" size="small" placeholder="请选择">
							<el-option label="primary" value="primary"> </el-option>
							<el-option label="success " value="success"> </el-option>
							<el-option label="danger" value="danger"></el-option>
							<el-option label="warning" value="warning"></el-option>
							<el-option label="info" value="info"></el-option>
							<el-option label="orange" value="orange"></el-option>
							<el-option label="purple" value="purple"></el-option>
							<el-option label="pink" value="pink"></el-option>
							<el-option label="red" value="red"></el-option>
							<el-option label="blue" value="blue"></el-option>
						</el-select>
					</template>
				</el-table-column>
				<el-table-column prop="Associated" label="关联数据源" width="180">
					<template #default="scope">
						<el-input size="small" v-model="scope.row.associated"></el-input>
					</template>
				</el-table-column>

			</el-table>

			<span class="dialog-footer">
				<el-button @click="addDetailLine" type="primary">添加一行</el-button>
				<el-button @click="addDetail" type="primary">确 定</el-button>
			</span>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, reactive, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addTableColumns, updateTableColumns, pageAllTableColumns, getAllTableColumns, updateTableColumnsDetail, getTableColumnsDetail } from "/@/api/main/tableColumns";

import TableColumns from "/@/entities/tableColumns";
import TableColumnsDetails from "/@/entities/tableColumnsDetails";



//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},

});

const state = ref({
	vm: {
		id: "",

		// form: {
		//     customerDetails: []
		// } as any,
	},
	visible: false,
	loading: false,
	tableColumn: new TableColumns(),
	tableColumns: new Array<TableColumns>(),
	tableColumnsDetails: new Array<TableColumnsDetails>(),
	//   tableColumnsDetail = ref();
});
// let tableColumnsDetails=reactive(Array<TableColumnsDetails>());
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});
// const dialogDetail: boolean = false;
//自行添加其他规则
const rules = ref<FormRules>({});

let dialogDetail = ref<boolean>(false);
let index: number = 0;
// 打开弹窗
const openDialog = (row: any) => {
	state.value.tableColumn = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
	handleQuery();
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

// const submitDetail = async () => {
// 	await updateTableColumnsDetail(state.value.tableColumnsDetails);
// };

// 提交
const submit = async () => {

	await updateTableColumns(state.value.tableColumns);
	closeDialog();
	ElMessage.success('操作成功');
	// } else {
	// 		await addTableColumns(values);
	// 	}
	// 	closeDialog();
	// } else {
	// 	ElMessage({
	// 		message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
	// 		type: "error",
	// 	});
	// }
	// });
};


// 查询操作
const handleQuery = async () => {
	// console.log(state.value.tableColumn);
	var res = await getAllTableColumns(Object.assign({ "TableName": state.value.tableColumn.tableName }));
	state.value.tableColumns = res.data.result;
	// console.log(res.data.result);
	// console.log("asdsadas");
	// console.log(state.value.tableColumns);
};


const addDetailShow = async (row: any, indexDetail: number) => {
	state.value.tableColumnsDetails = new Array<TableColumnsDetails>();
	index = indexDetail;
	let result = await getTableColumnsDetail(row);
	// console.log("result")
	// console.log(result.data.result)

	if (result.data.type == "success") {
		if (result.data.result.length > 0) {
			state.value.tableColumnsDetails = result.data.result;
		} else {
			if (row.tableColumnsDetails != undefined && row.tableColumnsDetails.length > 0) {
				state.value.tableColumnsDetails = row.tableColumnsDetails;
			} else {
				state.value.tableColumnsDetails.push(new TableColumnsDetails());

			}
		}
	}
	dialogDetail.value = true;
	// console.log(row.tableColumnsDetails)
	// // if(result)

	// console.log("tableColumnsDetails");
	// console.log(state.value.tableColumnsDetails);
}

const addDetail = async () => {
  	await updateTableColumnsDetail(state.value.tableColumnsDetails);
	state.value.tableColumns[index].tableColumnsDetails = state.value.tableColumnsDetails;
	dialogDetail.value = false;
}
const addDetailLine = async () => {
	state.value.tableColumnsDetails.push(new TableColumnsDetails());
}
const detailClose = async () => {
	state.value.tableColumnsDetails = new Array<TableColumnsDetails>();
	// console.log("asdasdasdasdasdasd");
	// console.log(state.value.tableColumnsDetails)
	dialogDetail.value = false;
}
const CreateClose = async (row: any, index: number) => {

	if (row.isCreate == 0) {
		state.value.tableColumns[index].isUpdate = 0;
		state.value.tableColumns[index].isSearchCondition = 0;
		state.value.tableColumns[index].isShowInList = 0;
	}
}

// 页面加载时
onMounted(async () => {

});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




