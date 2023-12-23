<template>
	<div class="mMSMaterial-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-tabs v-model="activeName">
				<el-tab-pane label="页面创建" name="PageCreate">
					<el-card>
						<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
							<el-row :gutter="35">
								<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
									v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
									<template v-if="i.isCreate">
										<el-form-item :label="i.displayName" style="width: 90%;" :prop="i.columnName">
											<template v-if="i.type == 'TextBox'">
												<el-input placeholder="请输入内容" size="small" style="width:90%"
													v-model="state.header[i.columnName]" v-if="i.isCreate">
												</el-input>
											</template>
											<template v-if="i.type == 'DropDownListInt'">
												<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
													placeholder="请选择" size="small" style="width:90%" filterable>
													<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
														:label="item.name" :value="item.codeInt">
													</el-option>
												</el-select>
											</template>
											<template v-if="i.type == 'DropDownListStrRemote'">
												<select-Remote :whereData="state.header" :isDisabled="i.isCreate"
													:columnData="i"
													@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>

											</template>
											<template v-if="i.type == 'DropDownListStr'">
												<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
													placeholder="请选择" size="small" style="width:90%" filterable>
													<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
														:label="item.name" :value="item.codeStr">
													</el-option>
												</el-select>
											</template>
											<template v-if="i.type == 'DatePicker'">
												<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
													type="date" placeholder="选择日期" size="small" style="width:90%">
												</el-date-picker>
											</template>
											<template v-if="i.type == 'DateTimePicker'">
												<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
													type="datetime" start-placeholder="选择日期时间" size="small"
													style="width:90%">
												</el-date-picker>
											</template>
										</el-form-item>
									</template>
								</el-col>
							</el-row>
						</el-form>
					</el-card>
				</el-tab-pane>
				<el-tab-pane label="Execl导入" name="ExeclCreate">
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
			<el-alert v-for="i in state.orderStatus" v-bind="i" :key="i" :title="i.externOrder + i.msg" :type="i.statusMsg">
			</el-alert>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addMMSMaterial, updateMMSMaterial } from "/@/api/main/mMSMaterial";

import { getByTableNameList, getImportExcelTemplate } from "/@/api/main/tableColumns";
import Header from "/@/entities/mMSMaterial";
// import Detail from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import { downloadByData, getFileName } from '/@/utils/download';
import { Local, Session } from '/@/utils/storage';
import orderStatus from "/@/entities/orderStatus";
//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});


const state = ref({
	// vm: {
	// 	// id: "",
	// 	// form: {
	// 	// 	customerDetails: []
	// 	// } as any,
	// 	header: new Header(),
	// 	details: new Array<Detail>(),
	// },
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	// details: new Array<Detail>(),

	//自定义提示
	orderStatus: new Array<orderStatus>(),
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

let activeName: string = 'PageCreate';

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadURL = baseURL + '/api/MMSMaterial/UploadExcelFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }
//导入弹框提示
const resultPopupShow = ref(false);

//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);

// headerRule : {},
//     detailRule :{},

const isShowDialog = ref(false);
// const ruleForm = ref<any>({});
//自行添加其他规则
// const rules = ref<FormRules>({});

//添加一行明细
// const handleAdd = (row: any) => {
// 	state.value.details.push(new Detail());
// }
// //删除一行明细
// const handleDelete = (index: any) => {
// 	state.value.details.splice(index, 1);
// }
// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
	gettableColumn();

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

			let result = await addMMSMaterial(state.value.header);
			if (result.data.result.code == "1") {
				closeDialog();
				ElMessage.success("保存成功");

			} else {
				ElMessage.error("保存失败:" + result.data.result.msg);
			}


		} else {
			ElMessage({
				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
				type: "error",
			});
		}

	});
};



const gettableColumn = async () => {
	let res = await getByTableNameList("MMS_Material");
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
	let resDetail = await getByTableNameList("CustomerDetail");
	// console.log("asdasdasdasdasdasddasdas")
	// console.log(resDetail);
	state.value.tableColumnDetails = resDetail.data.result;
	// detailRule.value = {};
	// state.value.tableColumnDetails.forEach((a) => {
	// 	if (a.validation.toUpperCase() == "Required".toUpperCase()) {
	// 		//  console.log("添加验证"+a.columnName)
	// 		detailRule.value[a.columnName] = [
	// 			{
	// 				required: true,
	// 				message: a.displayName,
	// 				trigger: "blur",
	// 			},
	// 		];
	// 	}
	// });
	// console.log(" state.value.tableColumnDetails")
	// console.log(state.value.tableColumnDetails)
	// console.log(state.value.header)
	// let resDetail = await getByTableNameList("CustomerDetail");
	// state.value.tableColumnHeaders = res.data.result;

};




// -------------------------------非可公用部分----------------------------------------
// 上传结果
const ImportExcel = (response, file, fileList) => {
	closeDialog();
	if (response.result.data.length > 0) {
		state.value.orderStatus = response.result.data;
		// console.log(state.value.orderStatus);
		//导入弹框提醒
		resultPopupShow.value = true;
	} else {
		ElMessage.info(response.result.msg);
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
	var res = await getImportExcelTemplate({ CustomerId: state.value.header.customerId, TableName: "MMS_Material" });
	var fileName = getFileName(res.headers);
	downloadByData(res.data as any, fileName);
};


// 页面加载时
onMounted(async () => {
	gettableColumn();
	// state.value.details = [new Detail()];
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




