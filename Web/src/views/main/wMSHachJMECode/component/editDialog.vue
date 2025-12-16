<template>
	<div class="wMSHachJMECode-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
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
			</el-form>
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
	import { ref,onMounted } from "vue";
	import { ElMessage } from "element-plus";
	import type { FormRules } from "element-plus";
	import { addWMSHachJMECode, updateWMSHachJMECode,exportDemo } from "/@/api/main/wMSHachJMECode";
	import { Local, Session } from '/@/utils/storage';
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
let uploadURL = baseURL + '/api/WMSHachJMECode/ImportExcel';
// let uploadURLBom = baseURL + '/api/WMSHachJMECode/UploadBomExcelFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }
//导入弹框提示
const resultPopupShow = ref(false);
// 打开弹窗
const openDialog = (row: any) => {
  ruleForm.value = JSON.parse(JSON.stringify(row));
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
  ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
    if (isValid) {
      let values = ruleForm.value;
      if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
        await updateWMSHachJMECode(values);
      } else {
        await addWMSHachJMECode(values);
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



// -------------------------------非可公用部分----------------------------------------
// 上传结果
const ImportExcel = (response, file, fileList) => {
	closeDialog();
	if (response.code == 200) {
		if (response.result.data != null && response.result.data.length > 0) {
			// state.value.orderStatus = response.result.data;
			// console.log(state.value.orderStatus);
			//导入弹框提醒
			resultPopupShow.value = true;
		} else {
			ElMessage.info(response.result.msg);
		}
	} else {
		ElMessage.error(response.message);
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
//获取导入的模板
// 导出日志
const exportExcel = async () => {
	var res = await exportDemo();
	var fileName = getFileName(res.headers);
	downloadByData(res.data as any, fileName);
};
//获取导入的模板
// 导出日志
// const exportBomExcel = async () => {
// 	var res = await getImportExcelTemplate();
// 	var fileName = getFileName(res.headers);
// 	downloadByData(res.data as any, fileName);
// };



// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




