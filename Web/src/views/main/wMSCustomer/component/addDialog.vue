<template>
	<div class="wMSCustomer-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-tabs v-model="activeName">
				<el-tab-pane label="基本信息" name="first">
					<el-card>
						<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
							<el-row :gutter="35">
								<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
									v-for="i in state.tableColumnHeaders" v-bind:key="i.id">
									<el-form-item :label="i.displayName" v-if="i.isCreate"
										style="width: 90%;height: 45px;" :prop="i.columnName">
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
											<!-- <select-tool :apiurl=i.associated :column=i.columnName :relationColumn=i.relationColumn
								@getChildrenVal="getChildrenVal" style="width: 100%;" size="small"></select-tool> -->
										</template>
										<template v-if="i.type == 'DropDownListStr'">
											<el-select v-model="state.header[i.columnName]" v-if="i.isCreate"
												placeholder="请选择" size="small" style="width:90%" filterable>
												<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
													:label="item.name" :value="item.codeInt">
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
												<template v-if="v.type == 'DropDownList'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" placeholder="请选择" style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails"
															:key="item.code" :label="item.name" :value="item.code">
														</el-option>
													</el-select>
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
				<el-tab-pane label="配置附件" name="fourth">
					<el-form ref="customerConfigRuleRef" label-position="top" :rules="customerConfigRule"
						:model="state.header">
						<el-row :gutter="35">
							<el-col :xs="24" :sm="24" :md="24" :lg="24" :xl="24" v-for="q in state.tableColumnConfigs"
								v-bind:key="q.id">
								<el-form-item :label="q.displayName" v-if="q.isCreate" style="width: 90%;height: 150px;"
									:prop="q.columnName">
									<template v-if="q.type == 'UploadImg'">
										<el-upload class="avatar-uploader" :action="uploadImgURL" :headers="httpheaders"
											:on-success="uploadImg">
											<img v-if="state.customerConfig[q.columnName]"
												:src="baseURL+state.customerConfig[q.columnName]" class="avatar">
											<i v-else class="el-icon-plus avatar-uploader-icon" icon="ele-Plus">+</i>
											<!-- <el-button type="primary">点击上传</el-button> -->
											<!-- <div class="el-upload__tip">只能上传图片，且不超过500kb</div> -->
										</el-upload>
									</template>
									<template v-if="q.type == 'TextBox'">
										<el-input placeholder="请输入内容" size="small" style="width:90%"
											v-model="state.customerConfig[q.columnName]" v-if="q.isCreate">
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
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSCustomer, updateWMSCustomer } from "/@/api/main/wMSCustomer";

import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/customer";
import Detail from "/@/entities/customerDetail";
import CustomerConfig from "/@/entities/customerConfig";
import TableColumns from "/@/entities/tableColumns";
import { Local, Session } from '/@/utils/storage';
//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});


const state = ref({
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
let customerConfigRuleRef = ref<any>({});
let customerConfigRule = ref({});
// let fileList = ref({});

//
let activeName: string = 'first';

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;
//给上传组件赋值url
let uploadImgURL = baseURL + '/api/WMSCustomer/UploadLogoFile';
//给上传组件赋值token
// 获取本地的 token
const accessTokenKey = 'access-token';
const accessToken = Local.get(accessTokenKey);
let httpheaders = { Authorization: "Bearer " + accessToken }
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
	state.value.header.details = state.value.details;
	state.value.header.customerConfig = state.value.customerConfig;

	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			detailRuleRef.value.validate(async (isValidDetail: boolean, fieldsDetail?: any) => {
				if (isValidDetail) {

					let result = await addWMSCustomer(state.value.header);
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



const gettableColumn = async () => {
	let res = await getByTableNameList("Customer");
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

	let restableColumnConfig = await getByTableNameList("CustomerConfig");
	console.log("asdasdasdasdasdasddasdas")
	console.log(restableColumnConfig.data.result);
	state.value.tableColumnConfigs = restableColumnConfig.data.result;

};

// -------------------------上传文件的一些方法------------------------------

// -------------------------------非可公用部分----------------------------------------
// 上传结果uploadImg
const uploadImg = (response, file, fileList) => {
	// closeDialog();
	state.value.customerConfig.customerLogo = response.result;
	// console.log(response.result.data[0].customerLogo)
	console.log("uploadData");
	console.log(response);
	console.log(file);
	console.log(fileList);
	// ElMessage.info(response.result.msg);
	// if (response.result.data!=null && response.result.data.length > 0) {
	// 	// state.value.orderStatus = response.result.data;
	// 	// // console.log(state.value.orderStatus);
	// 	// //导入弹框提醒
	// 	// resultPopupShow.value = true;
	// }else{
	// 	ElMessage.info(response.result.msg);
	// }
	// if (response.result.code == "1") {
	// 	closeDialog();
	// } else {
	// 	ElMessage.error("添加失败");
	// }
	// state.value.orderStatus = response.result.data;
	// // console.log(state.value.orderStatus);
	// //导入弹框提醒
	// resultPopupShow.value = true;
	// console.log(response)
	// state.value.orderStatus = response.result;
	// resultPopupShow.value = true;
}
// 页面加载时
onMounted(async () => {
	gettableColumn();
	state.value.details = [new Detail()];
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>

<style lang="less" scoped>
.avatar-uploader .el-upload {
	border: 1px dashed #d9d9d9;
	border-radius: 6px;
	cursor: pointer;
	position: relative;
	overflow: hidden;
}

.avatar-uploader .el-upload:hover {
	border-color: #409EFF;
}

.avatar-uploader-icon {
	font-size: 28px;
	color: #8c939d;
	width: 100px;
	height: 100px;
	line-height: 100px;
	text-align: center;
}

.avatar {
	width: 100px;
	height: 100px;
	display: block;
}

.el-icon-plus {
	border: #8c939d solid 1px;
}
</style>