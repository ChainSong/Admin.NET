<template>
	<div class="wMSStockCheck-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1200" draggable="">
			<el-card>
				<el-form ref="headerRuleRef" label-position="top" :rules="headerRule" :model="state.header">
					<el-row :gutter="35">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
							v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
							<el-form-item :label="i.displayName" v-if="i.isCreate" style="width: 90%;height: 95px;"
								:prop="i.columnName">
								<template v-if="i.type == 'TextBox'">
									<el-input placeholder="请输入内容" size="small" style="width:90%"
										v-model="state.header[i.columnName]" v-if="i.isCreate">
									</el-input>
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<el-select v-model="state.header[i.columnName]" v-if="i.isCreate" placeholder="请选择"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
											:label="item.name" :value="item.codeInt">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DropDownListStrRemote'">
									<select-Remote :whereData="state.header" :isDisabled="i.isCreate" :columnData="i"
										:defaultvValue="state.header[i.columnName]"
										@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote>
								</template>
								<template v-if="i.type == 'DropDownListStr'">
									<el-select v-model="state.header[i.columnName]" v-if="i.isCreate" placeholder="请选择"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
											:label="item.name" :value="item.codeStr">
										</el-option>
									</el-select>
								</template>
								<template v-if="i.type == 'DatePicker'">
									<el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate" type="date"
										placeholder="选择日期" size="small" style="width:90%">
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
			<el-card>
				<template #header>
					<div class="card-header">
						<span>盘点条件</span>
					</div>
				</template>
				<template v-if="state.header['stockCheckType'] == '按库区盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="库区" prop="Area">
								<el-input size="small" v-model="ruleForm.Area" placeholder="请输入库区" clearable />
							</el-form-item>
						</el-col>
					</el-row>
				</template>
				<template v-if="state.header['stockCheckType'] == '按库位盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="库位" prop="externNumber">
								<el-input size="small" type="textarea" v-model="ruleForm.Location" placeholder="请输入库位"
									clearable />
							</el-form-item>
						</el-col>

					</el-row>
				</template>
				<template v-if="state.header['stockCheckType'] == '按SKU盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="SKU" prop="sku">
								<el-input size="small" type="textarea" v-model="ruleForm.sku" placeholder="请输入SKU"
									clearable />
							</el-form-item>
						</el-col>

					</el-row>
				</template>
				<template v-if="state.header['stockCheckType'] == '动态库位盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="订单类型" prop="orderTypeList">
								<el-checkbox-group v-model="ruleForm.orderTypeList">
									<el-checkbox label="入库单" disabled></el-checkbox>
									<el-checkbox label="出库单" disabled></el-checkbox>
									<el-checkbox label="移库单" disabled></el-checkbox>
									<el-checkbox label="冻结单" disabled></el-checkbox>
									<el-checkbox label="调整单" disabled></el-checkbox>
								</el-checkbox-group>
							</el-form-item>
						</el-col>
					</el-row>
				</template>
				<el-button @click="handleQuery" type="primary" size="large" class="toolbar-btn">查询</el-button>
			</el-card>

			<el-card>
				<el-form label-position="top" :model="state" ref="detailRuleRef" :rules="detailRule">
							<el-table :data="state.details" height="250">
								<template v-for="(v, index) in state.tableColumnDetails">
									<el-table-column v-if="v.isCreate" :key="index" style="margin:0;padding:0;"
										:fixed="false" :prop="v.columnName" :label="v.displayName" width="150">
										<template #default="scope">
											<el-form-item :key="scope.row.key" style="margin:0;padding:0;"
												>
												<template v-if="v.type == 'TextBox'">
													<el-input placeholder="请输入内容" v-model="scope.row[v.columnName]"
														v-if="v.isCreate">
													</el-input>
												</template>
												<template v-if="v.type == 'DropDownListInt'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" placeholder="请选择" style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails"
															:key="item.codeInt" :label="item.name"
															:value="item.codeInt">
														</el-option>
													</el-select>
												</template>
												<template v-if="v.type == 'DropDownListStr'">
													<el-select v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate" placeholder="请选择" style="width: 100%">
														<el-option v-for="item in v.tableColumnsDetails"
															:key="item.codeStr" :label="item.name"
															:value="item.codeStr">
														</el-option>
													</el-select>
												</template>
												<template v-if="v.type == 'DropDownListStrRemote'">
													<select-Remote :whereData="state.header" :isDisabled="v.isCreate"
														:columnData="v"
														:key="state.details[scope.$index]"
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
													<el-input-number placeholder="请输入内容"
														v-model="state.details[scope.$index][v.columnName]"
														v-if="v.isCreate"></el-input-number>
												</template>
											</el-form-item>
										</template>
									</el-table-column>
								</template>
								<!-- <el-table-column>
									<template #default="scope">
										<el-button size="mini" type="primary"
											@click="handleDelete(scope.$index)">删除</el-button>
									</template>
								</el-table-column> -->
							</el-table>
						</el-form>
				<!-- <el-form label-position="top" ref="detailRuleRef">
					<el-table :data="state.headers" height="250">
						<el-table-column prop="date" label="日期" width="180"> </el-table-column>
						<el-table-column prop="date" label="姓名" width="180"> </el-table-column>
						<el-table-column prop="date" label="地址" width="180"> </el-table-column>
					</el-table>
				</el-form> -->
				<!-- <el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
					:total="tableParams.total" :page-sizes=	`"[10, 20, 50, 100]" small="" background=""
					@size-change="handleSizeChange" @current-change="handleCurrentChange"
					layout="total, sizes, prev, pager, next, jumper" /> -->
			</el-card>
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
import { addWMSStockCheck, updateWMSStockCheck, pageWMSStockCheck,getStockCheckInventory } from "/@/api/main/wMSStockCheck";
import { getByTableNameList, getImportExcelTemplate } from "/@/api/main/tableColumns";
import Header from "/@/entities/stockCheck";
import Details from "/@/entities/stockCheckDetail";
import TableColumns from "/@/entities/tableColumns";
import orderStatus from "/@/entities/orderStatus";
import selectRemote from '/@/views/tools/select-remote.vue'

import { selectCustomer } from '/@/api/main/wMSCustomer';
import { selectWarehouse } from '/@/api/main/wMSWarehouse';;
// import {  allWMSCustomer } from '/@/api/main/wMSCustomer';
import Customer from "/@/entities/customer";
// import CustomerUser from "/@/entities/customerUserMapping";

// import { allWMSWarehouse } from '/@/api/main/wMSWarehouse';
// import { listWarehouseUserMapping, addWarehouseUserMapping } from '/@/api/main/warehouseUserMapping';
import Warehouse from "/@/entities/Warehouse";
// import WarehouseUser from "/@/entities/warehouseUserMapping";


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
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Details>(),

    
	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),

	customers: new Array<Customer>(),
	warehouses: new Array<Warehouse>(),
	//自定义提示
	orderStatus: new Array<orderStatus>()
});

//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});
const loading = ref(false);
//自行添加其他规则
const rules = ref<FormRules>({});

//主表和明细的验证
let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});

const tableParams = ref({
	page: 1,
	pageSize: 10,
	total: 0,
});
// 打开弹窗
const openDialog = (row: any) => {
	ruleForm.value = JSON.parse(JSON.stringify(row));
	ruleForm.value.orderTypeList=['入库单','出库单','移库单','冻结单','调整单'];
	isShowDialog.value = true;
	gettableColumn();
	getBaseData();
};

//获取页面基础数据

const getBaseData = async () => {
	//获取客户数据
	var res = await selectCustomer("");
	console.log(res);
	state.value.customers = res.data.result ?? [];
	// tableParams.value.total = res.data.result?.total;
	// loading.value = false;

	//获取仓库数据
	var res = await selectWarehouse("");
	console.log(res);
	state.value.warehouses = res.data.result ?? [];
	tableParams.value.total = res.data.result?.total;
	// 获取库区数据
	// var res = await listCustomerUserMapping();
	// state.value.customers = res.data.result?.items ?? [];
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

// 查询盘点的库位信息
const handleQuery = async () => {
 
	var res = await getStockCheckInventory(Object.assign(ruleForm.value,state.value.header,tableParams));
	console.log(res);
	// state.details
	state.value.details = res.data.result.data ?? [];
	console.log("state.value.details");
	console.log(state.value.details);
};

// // 查询盘点明细
// const queryStockCheckDetail = async () => {
// 	var res = await queryStockCheckDetail(ruleForm.value.id);
// 	console.log(res);
// 	state.value.details = res.data.result ?? [];
// };

// // 添加盘点明细
// const addStockCheckDetail = async () => {
// 	var res = await addStockCheckDetail(ruleForm.value.id);
// 	console.log(res);
// 	state.value.details = res.data.result ?? [];
// };

// 提交
const submit = async () => {
	headerRuleRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = Object.assign(ruleForm.value,state.value.header);
			values.details = state.value.details;
			if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
				await updateWMSStockCheck(values);
			} else {
				await addWMSStockCheck(values);
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


//得到表字段
const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_StockCheck");
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
	let resDetail = await getByTableNameList("WMS_StockCheckDetail");
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


// // 查询操作
// const handleQuery = async () => {

// 	loading.value = true;
// 	var res = await pageWMSStockCheck(Object.assign(ruleForm, tableParams.value));
// 	state.value.headers = res.data.result?.items ?? [];
// 	tableParams.value.total = res.data.result?.total;
// 	loading.value = false;
// };


// 改变页面容量
const handleSizeChange = (val: number) => {
	tableParams.value.pageSize = val;
	handleQuery();
};

// 改变页码序号
const handleCurrentChange = (val: number) => {
	tableParams.value.page = val;
	handleQuery();
};


// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
