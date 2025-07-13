<template>
	<div class="wMSReceipt-container">
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
					<!-- <el-link type="primary" @click="exportExcel">下载模板</el-link> -->
				</el-tab-pane>
			</el-tabs>
			<div>
				<!-- <Button @click="cancel">{{ L("Cancel") }}</Button>
				<Button @click="save" type="primary">{{ L("OK") }}</Button> -->
			</div>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">关 闭</el-button>
					<!-- <el-button type="primary" @click="cancel" size="default">确 定</el-button> -->
				</span>
			</template>
		</el-dialog>
		<el-dialog v-model="resultPopupShow" title="导入结果" :append-to-body="true">
			<el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + ':' + i.msg"
				:type="i.statusMsg">
			</el-alert>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSReceipt, updateWMSReceipt, returnReceiptReceiving } from "/@/api/main/wMSReceiptReceiving";

import { getByTableNameList, getImportExcelTemplate } from "/@/api/main/tableColumns";
import Header from "/@/entities/Receipt";
import Detail from "/@/entities/ReceiptDetail";
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
let uploadURL = baseURL + '/api/wMSReceiptReceiving/UploadExcelFile';
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
	console.log("state.value.details");
	console.log(state.value.details);
	state.value.header.details = state.value.details
	console.log(state.value.header);

	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			detailRuleRef.value.validate(async (isValidDetail: boolean, fieldsDetail?: any) => {
				if (isValidDetail) {
					// await addWMSReceipt(state.value.header);
					// closeDialog();
					let result = await addWMSReceipt(state.value.header);
					if (result.data.result.code == "1") {
						closeDialog();
					} else {
						ElMessage.error("保存失败:" + result.data.result.msg);
					}

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



// -------------------------------非可公用部分----------------------------------------
// 上传结果
const ImportExcel = (response, file, fileList) => {
	closeDialog();
	console.log(response);
	if (response.code == 200) {
		if (response.result.data != null && response.result.data.length > 0) {
			state.value.orderStatus = response.result.data;
			// console.log(state.value.orderStatus);
			//导入弹框提醒
			resultPopupShow.value = true;
		} else {
			ElMessage.info(response.result.msg);
		}
	} else {
		ElMessage.error(response.message);
	}
	// console.log(response)
	// // state.value.orderStatus = response.result;
	// if(response.result=="成功"){
	// 	ElMessage.success("导入成功");
	// 	isShowDialog.value = false;
	// }else{
	// 	ElMessage.success(response.result);
	// }
	// resultPopupShow.value = true;
}
//获取导入的模板
// 导出日志
// const exportExcel = async () => {
// 	// state.loading = true;
// 	var res = await getImportExcelTemplate({ CustomerId: state.value.header.customerId, TableName: "WMS_Receipt" });
// 	// state.loading = false;

// 	var fileName = getFileName(res.headers);
// 	console.log(fileName)
// 	console.log(res.data)
// 	downloadByData(res.data as any, fileName);
// };

// 页面加载时
onMounted(async () => {

});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
