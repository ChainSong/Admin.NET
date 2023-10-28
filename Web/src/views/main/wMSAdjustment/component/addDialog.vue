<template>
	<div class="wMSAdjustment-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">
			<el-tabs v-model="activeName">
				<!-- <el-tab-pane label="页面创建" name="PageCreate">

					<el-card>
						<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
							<el-row :gutter="35">
								<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
									v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
									<el-form-item :label="i.displayName" v-if="i.isCreate" style="width: 90%;height: 45px;"
										:prop="i.columnName">
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
											<select-Remote :objData="state.header" :isDisabled="i.isCreate" :columnData="i"
												:defaultvValue="state.header[i.columnName]"
												@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationDBColumn] = data.value; console.log(state.header) }"></select-Remote>
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
												type="datetime" start-placeholder="选择日期时间" size="small" style="width:90%">
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
									<el-table-column v-if="v.isCreate" :key="index" style="margin:0;padding:0;"
										:fixed="false" :prop="v.columnName" :label="v.displayName" width="150">
										<template #default="scope">
											<el-form-item :key="scope.row.key" style="margin:0;padding:0;"
												:prop="'details.' + scope.$index + '.' + v.columnName"
												:rules="detailRule[v.columnName]">
												<template v-if="v.type == 'TextBox'">
													<el-input placeholder="请输入内容" v-model="scope.row[v.columnName]"
														v-if="v.isCreate">
													</el-input>
												</template>
												<template v-if="v.type == 'DropDownListInt'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" placeholder="请选择" style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails" :key="item.codeInt"
															:label="item.name" :value="item.codeInt">
														</el-option>
													</el-select>
												</template>
												<template v-if="v.type == 'DropDownListStr'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" placeholder="请选择" style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails" :key="item.codeStr"
															:label="item.name" :value="item.codeStr">
														</el-option>
													</el-select>
												</template>
												<template v-if="v.type == 'DropDownListStrRemote'">
													<select-Remote :objData="state.header" :isDisabled="v.isCreate"
														:columnData="v"
														:defaultvValue="state.details[scope.$index][v.columnName]"
														@select:model="data => { state.details[scope.$index][v.columnName] = data.text; state.details[scope.$index][v.relationColumn] = data.value; console.log(state.details[scope.$index]) }"></select-Remote>
												</template>
												<template v-if="v.type == 'DatePicker'">
													<el-date-picker v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" type="date" placeholder="选择日期"
														style="width: 100%">
													</el-date-picker>
												</template>
												<template v-if="v.type == 'DateTimePicker'">
													<el-date-picker v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" type="datetime" start-placeholder="选择日期时间"
														style="width: 100%">
													</el-date-picker>
												</template>
												<template v-if="v.type == 'InputNumber'">
													<el-input-number placeholder="请输入内容" size="small"
														v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate"></el-input-number>

												</template>
											</el-form-item>
										</template>
									</el-table-column>
								</template>
								<el-table-column>
									<template #default="scope">
										<el-button size="mini" type="primary"
											@click="handleDelete(scope.$index)">删除</el-button>
									</template>
								</el-table-column>
							</el-table>
						</el-form>
					</el-card>
				</el-tab-pane> -->
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
	console.log("state.value.details");
	console.log(state.value.details);
	state.value.header.details = state.value.details
	console.log(state.value.header);

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
	console.log(response)
	state.value.orderStatus = response.result;
	resultPopupShow.value = true;
	closeDialog();
}
//获取导入的模板
// 导出日志
const exportExcel = async () => {
	// state.loading = true;
	var res = await getImportExcelTemplate({ CustomerId: state.value.header.customerId, TableName: "WMS_Adjustment" });
	// state.loading = false;

	var fileName = getFileName(res.headers);
	console.log(fileName)
	console.log(res.data)
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




