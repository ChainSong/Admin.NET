<template>
	<div class="wMSAdjustment-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">
			<el-tabs v-model="activeName">
				<el-tab-pane label="Excel导入" name="ExcelCreate">
					<el-row>
						<el-col>
						</el-col>
						<el-col>
							<el-upload class="upload-demo" :action="uploadURL" :headers="httpheaders"
								:on-success="ImportExcel">
								<el-button type="primary">点击上传</el-button>
								<div class="el-upload__tip">只能上传xlsx/xls文件，且不超过500kb</div>
							</el-upload>
						</el-col>
					</el-row>
					<el-link type="primary" @click="exportExcel">下载模板</el-link>
				</el-tab-pane>
			</el-tabs>
			<div>
			</div>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<el-button type="primary" @click="submit" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
		<el-dialog v-model="resultPopupShow" title="导入结果" :append-to-body="true">
			<el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg"
				:type="i.statusMsg">
			</el-alert>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSAdjustment, updateWMSAdjustment } from "/@/api/main/wMSAdjustment";

import { getByTableNameList, getImportExcelTemplate } from "/@/api/main/tableColumns";
import Header from "/@/entities/adjustment";
import Detail from "/@/entities/adjustmentDetail";
import orderStatus from "/@/entities/orderStatus";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue'
import { Local, Session } from '/@/utils/storage';

import { getAPI } from '/@/utils/axios-utils';
import { downloadByData, getFileName } from '/@/utils/download';

//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});

//
const state = ref({
	visible: false,
	loading: false,
	//通用的主体和明细实体
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Detail>(),
	//通用的表字段
	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),
	//导入提示
	orderStatus: new Array<orderStatus>(),
	// header: new Array<Details>(),
})
//
let activeName: string = 'ExcelCreate';

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadURL = baseURL + '/api/wMSAdjustment/UploadExcelFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }
//导入弹框提示
const resultPopupShow = ref(false);

//主表和明细的验证
let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});


//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);

const isShowDialog = ref(false);


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
	isShowDialog.value = true;

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
	state.value.header.details = state.value.details
	// console.log(state.value.header);

	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			detailRuleRef.value.validate(async (isValidDetail: boolean, fieldsDetail?: any) => {
				if (isValidDetail) {
					await addWMSAdjustment(state.value.header);
					closeDialog();
				} else {
					ElMessage({
						message: `表单有${Object.keys(fieldsDetail).length}处验证失败，请修改后再提交`,
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


//得到表字段
const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_Adjustment");
	state.value.tableColumnHeaders = res.data.result;
	headerRule.value = {};
	//验证
	state.value.tableColumnHeaders.forEach((a) => {
		if (a.validation.toUpperCase() == "Required".toUpperCase()) {
			headerRule.value[a.columnName] = [
				{
					required: true,
					message: a.displayName,
					trigger: "blur",
				},
			];
		}
	});
	let resDetail = await getByTableNameList("WMS_AdjustmentDetail");
	state.value.tableColumnDetails = resDetail.data.result;
	detailRule.value = {};
	state.value.tableColumnDetails.forEach((a) => {
		if (a.validation.toUpperCase() == "Required".toUpperCase()) {
			detailRule.value[a.columnName] = [
				{
					required: true,
					message: a.displayName,
					trigger: "blur",
				},
			];
		}
	});

};

// -------------------------------非可公用部分----------------------------------------
// 上传结果
const ImportExcel = (response, file, fileList) => {
	console.log("response")
	console.log(response)
	if (response.result.data.length > 0) {
		state.value.orderStatus = response.result.data;
		resultPopupShow.value = true;
	} else {
		ElMessage({
			message: response.result.msg,
			type: "error",
		});
	}
	
	closeDialog();
}
//获取导入的模板
// 导出日志
const exportExcel = async () => {
	// state.loading = true;
	var res = await getImportExcelTemplate({ CustomerId: state.value.header.customerId, TableName: "WMS_Adjustment" });
	// state.loading = false;
	var fileName = getFileName(res.headers);
	downloadByData(res.data as any, fileName);
};

// 页面加载时
onMounted(async () => {
	gettableColumn();
	state.value.details = [new Detail()];
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
