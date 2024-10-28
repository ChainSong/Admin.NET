<template>
	<div class="wMSStockCheck-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1200" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="[16, 15]">
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="外部单号"  prop="externNumber">
							<el-input  size="small" v-model="ruleForm.externNumber" placeholder="请输入外部单号" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="客户名称" prop="customerId">
							<el-select v-model="ruleForm.customerId" clearable filterable
								  size="small" placeholder="请选择">
								<el-option v-for="item in state.customers" :key="item.value" style="width: 100%"
									:label="item.text" :value="item.value">
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="仓库名称" prop="warehouseId">
							<el-select v-model="state.warehouseId" clearable filterable
								  size="small" placeholder="请选择">
								<el-option v-for="item in state.warehouses" :key="item.value" style="width: 100%"
									:label="item.text" :value="item.value">
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="仓库名称" prop="warehouseId">
							<el-select v-model="state.warehouseId" clearable filterable
								  size="small" placeholder="请选择">
								<el-option v-for="item in state.warehouses" :key="item.value" style="width: 100%"
									:label="item.text" :value="item.value">
								</el-option>
							</el-select>
						</el-form-item>
					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="盘点时间" prop="stockCheckDate">
							<el-date-picker v-model="ruleForm.stockCheckDate" type="date"
							size="small"	placeholder="StockCheckDate" />

						</el-form-item>

					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="盘点类型" prop="stockCheckType">
							<el-input v-model="ruleForm.stockCheckType"  size="small" placeholder="请输入盘点类型" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
						<el-form-item label="备注" prop="remark">
							<el-input  size="small" v-model="ruleForm.remark" placeholder="请输入备注" clearable />
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<el-button @click="handleQuery" type="primary" size="large" class="toolbar-btn">查询</el-button>
			<el-card>
				<el-form label-position="top" ref="detailRuleRef">
					<el-table :data="state.headers" height="250">
						<el-table-column prop="date" label="日期" width="180"> </el-table-column>
						<el-table-column prop="date" label="姓名" width="180"> </el-table-column>
						<el-table-column prop="date" label="地址" width="180"> </el-table-column>
					</el-table>
				</el-form>
				<el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
					:total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
					@size-change="handleSizeChange" @current-change="handleCurrentChange"
					layout="total, sizes, prev, pager, next, jumper" />
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
import { addWMSStockCheck, updateWMSStockCheck, pageWMSStockCheck } from "/@/api/main/wMSStockCheck";
import Header from "/@/entities/stockCheck";
import Details from "/@/entities/stockCheckDetail";
import TableColumns from "/@/entities/tableColumns";
import orderStatus from "/@/entities/orderStatus";

import { selectCustomer } from '/@/api/main/wMSCustomer';
import { selectWarehouse} from '/@/api/main/wMSWarehouse';;
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
const rules = ref<FormRules>({
});
const tableParams = ref({
	page: 1,
	pageSize: 10,
	total: 0,
});
// 打开弹窗
const openDialog = (row: any) => {
	ruleForm.value = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
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

// 提交
const submit = async () => {
	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
		if (isValid) {
			let values = ruleForm.value;
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



// 查询操作
const handleQuery = async () => {

	loading.value = true;
	var res = await pageWMSStockCheck(Object.assign(ruleForm, tableParams.value));
	state.value.headers = res.data.result?.items ?? [];
	tableParams.value.total = res.data.result?.total;
	loading.value = false;
};


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
