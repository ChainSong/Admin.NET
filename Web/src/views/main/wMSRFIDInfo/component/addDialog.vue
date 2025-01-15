<template>
	<div class="wMSRFIDInfo-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-card>
				<el-row>
					<el-col>
						<el-upload class="upload-demo" :action="uploadURL" :headers="httpheaders"
							:on-success="ImportExcel">
							<el-button type="primary">点击上传</el-button>
							<div class="el-upload__tip">只能上传xlsx/xls文件，且不超过500kb</div>
							<div class="el-upload__tip">由于Excel版本格式冗杂，请统一将Excel单元格设置成为文本格式</div>
							<div class="el-upload__tip">请保存的时候删除一下空白行</div>
						</el-upload>
					</el-col>
				</el-row>
				<el-row>
					<el-link type="primary" @click="exportExcel">下载模板</el-link>
				</el-row>
			</el-card>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSRFIDInfo, updateWMSRFIDInfo } from "/@/api/main/wMSRFIDInfo";
import { Local, Session } from '/@/utils/storage';
import { getByTableNameList, getImportExcelTemplate } from "/@/api/main/tableColumns";
import { getAPI } from '/@/utils/axios-utils';
import { downloadByData, getFileName } from '/@/utils/download';
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
//自行添加其他规则
const rules = ref<FormRules>({
});



// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadURL = baseURL + '/api/WMSRFIDInfo/UpdateSNCode';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }

// 打开弹窗
const openDialog = (row: any) => {
	ruleForm.value = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
};

// -------------------------------非可公用部分----------------------------------------
// 上传结果
const ImportExcel = (response, file, fileList) => {
	closeDialog();
	if (response.result.code==1) {
		// state.value.orderStatus = response.result.data;
		// console.log(state.value.orderStatus);
		//导入弹框提醒
		// resultPopupShow.value = true;
		ElMessage.success(response.result.msg);
	} else {
		ElMessage.error(response.result.msg);
	}
	// if (response.result.code == "1") {
	// 	closeDialog();
	// } else {
	// 	ElMessage.error("添加失败");
	// }
	// closeDialog();
	// state.value.orderStatus = response.result.data;
	// // console.log(state.value.orderStatus);
	// //导入弹框提醒
	// resultPopupShow.value = true;
}
// 关闭弹窗
const closeDialog = () => {
	emit("reloadTable");
	isShowDialog.value = false;
};

// 取消
const cancel = () => {
	isShowDialog.value = false;
};


//获取导入的模板
// 导出日志
const exportExcel = async () => {
	// state.loading = true;
	var res = await getImportExcelTemplate({ CustomerId:0, TableName: "RFID" });
	// state.loading = false;

	var fileName = getFileName(res.headers);
	// console.log(fileName)
	// console.log(res.data)
	downloadByData(res.data as any, fileName);
};


// 提交
const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = ruleForm.value;
			if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
				await updateWMSRFIDInfo(values);
			} else {
				await addWMSRFIDInfo(values);
			}
			closeDialog();
		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}
	});
};





// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
