﻿<template>
	<div class="wMSPreOrder-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">

			<el-tabs v-model="activeName">
				<el-tab-pane label="页面创建" name="PageCreate">
					<el-card>
						<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
							<el-row :gutter="35">
								<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
									v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)"
									v-bind:key="i.id">
									<el-form-item :label="i.displayName" v-if="i.isCreate"
										style="width: 90%;height: 45px;" :prop="i.columnName">
										<template v-if="i.type == 'TextBox'">
											<el-input placeholder="请输入内容" size="small" style="width:90%"
												:disabled="i.isUpdate == 0" v-model="state.header[i.columnName]"
												v-if="i.isCreate">
											</el-input>
										</template>
										<template v-if="i.type == 'DropDownListInt'">
											<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
												:disabled="i.isUpdate == 0" placeholder="请选择" size="small"
												style="width:90%" filterable>
												<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
													:label="item.name" :value="item.codeInt">
												</el-option>
											</el-select>
										</template>
										<template v-if="i.type == 'DropDownListStr'">
											<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
												placeholder="请选择" :disabled="i.isUpdate == 0" size="small"
												style="width:90%" filterable>
												<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
													:label="item.name" :value="item.codeStr">
												</el-option>
											</el-select>
										</template>
										<template v-if="i.type == 'DropDownListStrRemote'">

											<select-Remote :whereData="state.header" :isDisabled="i.isUpdate"
												:key="state.header[i.columnName]" :columnData="i"
												:defaultvValue="state.header[i.columnName]"
												@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>
										</template>

										<template v-if="i.type == 'DatePicker'">
											<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
												:disabled="i.isUpdate == 0" type="date" placeholder="选择日期" size="small"
												style="width:90%">
											</el-date-picker>
										</template>
										<template v-if="i.type == 'DateTimePicker'">
											<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
												:disabled="i.isUpdate == 0" type="datetime" start-placeholder="选择日期时间"
												size="small" style="width:90%">
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
												<template v-if="v.type == 'DropDownListInt'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="!v.isUpdate" placeholder="请选择"
														style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails"
															:key="item.codeInt" :label="item.name"
															:value="item.codeInt">
														</el-option>
													</el-select>
												</template>
												<template v-else-if="v.type == 'DropDownListStr'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="!v.isUpdate" placeholder="请选择"
														style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails"
															:key="item.codeStr" :label="item.name"
															:value="item.codeStr">
														</el-option>
													</el-select>
												</template>
												<template v-else-if="v.type == 'DropDownListStrRemote'">
													 
													<select-Remote :whereData="state.header" :isDisabled="v.isUpdate"
														:key="state.details[scope.$index]" :columnData="v"
														:defaultvValue="state.details[scope.$index][v.columnName]"
														@select:model="data => { state.details[scope.$index][v.columnName] = data.text; state.details[scope.$index][v.relationColumn] = data.value; console.log(state.details[scope.$index]) }"></select-Remote>
												</template>
												<template v-else-if="v.type == 'DatePicker'">
													<el-date-picker v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="v.isUpdate==0" type="date"
														placeholder="选择日期" style="width: 100%">
													</el-date-picker>
												</template>
												<template v-else-if="v.type == 'DatePicker'">
													<el-date-picker v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="v.isUpdate==0" type="date"
														placeholder="选择日期" style="width: 100%">
													</el-date-picker>
												</template>
												<template v-else-if="v.type == 'InputNumber'">
													<el-input-number placeholder="请输入内容"
														v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="v.isUpdate==0"></el-input-number>
												</template>
												<template v-else>
													<el-input placeholder="请输入内容"
														v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" :disabled="v.isUpdate==0">
													</el-input>
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
				</el-tab-pane>
				<el-tab-pane label="地址信息" name="AddressInfo">
					<el-card>
						<el-form ref="headerRuleRef" label-position="top" :model="state.orderAddress">
							<el-row :gutter="35">
								<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
									v-for="i in state.tableColumnOrderAddresss.filter(a => a.isCreate == 1)"
									v-bind:key="i.id">
									<el-form-item :label="i.displayName" v-if="i.isCreate"
										style="width: 90%;height: 45px;" :prop="i.columnName">
										<template v-if="i.type == 'TextBox'">
											<el-input :placeholder=i.displayName size="small" style="width:90%"
												v-model="state.orderAddress[i.columnName]" v-if="i.isCreate">
											</el-input>
										</template>
										<template v-if="i.type == 'DropDownListInt'">
											<el-select v-model="state.orderAddress[i.columnName]" v-if="i.isCreate" :isDisabled="i.isUpdate==0"
												placeholder="请选择" size="small" style="width:90%" filterable>
												<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
													:label="item.name" :value="item.codeInt">
												</el-option>
											</el-select>
										</template>
										<template v-if="i.type == 'DropDownListStrRemote'">
											<select-Remote :whereData="state.orderAddress" :isDisabled="i.isUpdate"  
												:columnData="i" :defaultvValue="state.header[i.columnName]"
												@select:model="data => { state.orderAddress[i.columnName] = data.text; state.orderAddress[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>
										</template>
										<template v-if="i.type == 'DropDownListStr'">
											<el-select v-model="state.orderAddress[i.columnName]" v-if="i.isCreate"
												placeholder="请选择" size="small" style="width:90%" filterable>
												<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
													:label="item.name" :value="item.codeStr">
												</el-option>
											</el-select>
										</template>
										<template v-if="i.type == 'DatePicker'">
											<el-date-picker v-model="state.orderAddress[i.columnName]" v-if="i.isCreate"
												type="date" placeholder="选择日期" size="small" style="width:90%">
											</el-date-picker>
										</template>
										<template v-if="i.type == 'DateTimePicker'">
											<el-date-picker v-model="state.orderAddress[i.columnName]" v-if="i.isCreate"
												type="datetime" start-placeholder="选择日期时间" size="small"
												style="width:90%">
											</el-date-picker>
										</template>

									</el-form-item>
								</el-col>
							</el-row>
						</el-form>
					</el-card>
				</el-tab-pane>
				<el-tab-pane label="扩展配置" name="extends">
					<el-form ref="extendRuleRef" label-position="top" :rules="extendRule" :model="state.extend">
						<el-row :gutter="35">
							<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" v-for="q in state.tableColumnExtends"
								v-bind:key="q.id">
								<el-form-item :label="q.displayName" v-if="q.isCreate" style="width: 90%;height: 100px;"
									:prop="q.columnName">
									<template v-if="q.type == 'UploadFile'">
										<a :href="baseURL + state.extend[q.columnName]" target="_blank">{{ state.extend[q.columnName] }}</a>
										<el-upload class="upload-demo" :action="uploadFileURL" :headers="httpheaders"
											:on-success="uploadFile">
											<el-button type="primary">点击上传</el-button>
											<div class="el-upload__tip">只能上传文件，且不超过500kb</div>
										</el-upload>
									</template>
									<template v-if="q.type == 'TextBox'">
										<el-input placeholder="请输入内容" size="small" style="width:90%"
											v-model="state.extend[q.columnName]" v-if="q.isCreate">
										</el-input>
									</template>
								</el-form-item>
							</el-col>
						</el-row>
					</el-form>
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
import { addWMSPreOrder, updateWMSPreOrder, getWMSPreOrder } from "/@/api/main/wMSPreOrder";

import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/preOrder";
import Detail from "/@/entities/preOrderDetail";
import Extend from "/@/entities/preOrderExtend";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
import OrderAddress from "/@/entities/orderAddress";
import { Local, Session } from '/@/utils/storage';
import OrderStatus from "/@/entities/orderStatus";
//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadURL = baseURL + '/api/wMSPreOrder/UploadExcelFile';
//上传附件路由
let uploadFileURL = baseURL + '/api/wMSPreOrder/UploadPreOrderFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }

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
	extend: new Extend(),

	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),

	orderAddress: new OrderAddress(),
	tableColumnOrderAddress: new TableColumns(),
	tableColumnOrderAddresss: new Array<TableColumns>(),

	tableColumnExtend: new TableColumns(),
	tableColumnExtends: new Array<TableColumns>(),
	//导入提示
	orderStatus: new Array<OrderStatus>(),
	// header: new Array<Details>(),
	// header: new Array<Details>(),
})

//导入弹框提示
const resultPopupShow = ref(false);

let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});
let addressRuleRef = ref<any>({});
let addressRule = ref({});
let extendRuleRef = ref<any>({});
let extendRule = ref({});

let activeName: string = 'PageCreate';

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
	state.value.header.details = state.value.details;
	state.value.header.orderAddress = state.value.orderAddress;
	state.value.header.extend = state.value.extend;
	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			detailRuleRef.value.validate(async (isValidDetail: boolean, fieldsDetail?: any) => {
				if (isValidDetail) {
					let result = await updateWMSPreOrder(state.value.header);
					if (result.data.result.code == "1") {
								ElMessage.success("修改成功");
								state.value.header = new Header();
								state.value.orderAddress = new OrderAddress();
								state.value.headers = new Array<Header>();
								state.value.details = [new Detail()];
								closeDialog();
							} else {
								//  ElMessage.error(result.data.result.msg);
								state.value.orderStatus = result.data.result.data;
								// console.log(state.value.orderStatus);
								//导入弹框提醒
								resultPopupShow.value = true;
							}
					// if (result.data.result.code == "1") {
					// 	ElMessage.success("修改成功");
					// 	closeDialog();
					// } else {
					// 	ElMessage.error("修改失败:" + result.data.result.msg);
					// }
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
	state.value.details = new Array<Detail>();
	state.value.orderAddress = new OrderAddress();
	let result = await getWMSPreOrder(state.value.header.id);
	state.value.header = result.data.result;
	state.value.details = result.data.result.details;
	state.value.orderAddress = result.data.result.orderAddress;
	state.value.extend = result.data.result.extend;
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
	let resorderAddress = await getByTableNameList("WMS_OrderAddress");
	state.value.tableColumnOrderAddresss = resorderAddress.data.result;
	addressRule.value = {};
	state.value.tableColumnOrderAddresss.forEach((a) => {
		if (a.validation.toUpperCase() == "Required".toUpperCase()) {
			addressRule.value[a.columnName] = [
				{
					required: true,
					message: a.displayName,
					trigger: "blur",
				},
			];
		}
	});

	let resExtend = await getByTableNameList("WMS_PreOrderExtend");
	state.value.tableColumnExtends = resExtend.data.result;
	// console.log(" state.value.tableColumnDetails")
	// console.log(state.value.tableColumnDetails)
	// console.log(state.value.header)
	// let resDetail = await getByTableNameList("CustomerDetail");
	// state.value.tableColumnHeaders = res.data.result;

};

// 上传结果uploadImg
const uploadFile = (response, file, fileList) => {
	// closeDialog();
	state.value.extend.shippingAttachmentsUrl = response.result;
}

// 页面加载时
onMounted(async () => {
	gettableColumn();
	// get();
	// state.value.vm.details = [new Detail()];
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




