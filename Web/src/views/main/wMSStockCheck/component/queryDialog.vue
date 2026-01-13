<template>
	<div class="wMSPreOrder-container">
		 <el-dialog v-model="isShowDialog" :title="props.title" :width="1200" draggable="">
			<el-card>
				<el-form  label-position="top"  :model="state.header">
					<el-row :gutter="35">
						<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12"
							v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1)" v-bind:key="i.id">
							<el-form-item :label="i.displayName" v-if="i.isCreate" style="width: 90%;height: 95px;"
								:prop="i.columnName">
								<template v-if="i.type == 'TextBox'">
									{{state.header[i.columnName]}}
									<!-- <el-input placeholder="请输入内容" size="small" style="width:90%"
										v-model="state.header[i.columnName]" v-if="i.isCreate">
									</el-input> -->
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<template v-for="item in i.tableColumnsDetails">
												<el-tag v-if="item.codeInt == state.header[i.columnName]"
													v-bind:key="item.color" show-icon :type="item.color">
													{{ item.name }}
												</el-tag>
												<!-- <template v-if="item.codeStr == state.header[i.columnName]">
													<label show-icon :type="item.color" v-text="item.name"
														:key="item.codeInt"></label>
												</template> -->
											</template>
									<!-- <el-select v-model="state.header[i.columnName]" v-if="i.isCreate" placeholder="请选择"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeInt"
											:label="item.name" :value="item.codeInt">
										</el-option>
									</el-select> -->
								</template>
								<template v-if="i.type == 'DropDownListStrRemote'">
									{{ state.header[i.columnName]  }}
									<!-- <select-Remote :whereData="state.header" :isDisabled="i.isCreate" :columnData="i"
										:defaultvValue="state.header[i.columnName]"
										@select:model="data => { state.header[i.columnName] = data.text; state.header[i.relationColumn] = data.value; console.log(state.header) }"></select-Remote> -->
								</template>
								<template v-if="i.type == 'DropDownListStr'">
									{{state.header[i.columnName]}}
									<!-- <el-select v-model="state.header[i.columnName]" v-if="i.isCreate" placeholder="请选择"
										size="small" style="width:90%" filterable>
										<el-option v-for="item in i.tableColumnsDetails" :key="item.codeStr"
											:label="item.name" :value="item.codeStr">
										</el-option>
									</el-select> -->
								</template>
								<template v-if="i.type == 'DatePicker'">
									{{state.header[i.columnName]}}
									<!-- <el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate" type="date"
										placeholder="选择日期" size="small" style="width:90%">
									</el-date-picker> -->
								</template>
								<template v-if="i.type == 'DateTimePicker'">
									{{state.header[i.columnName]}}
									<!-- <el-date-picker v-model="state.header[i.columnName]" v-if="i.isCreate"
										type="datetime" start-placeholder="选择日期时间" size="small" style="width:90%">
									</el-date-picker> -->
								</template>
							</el-form-item>
						</el-col>
					</el-row>
				</el-form>
			</el-card>
			<el-card>
				<!-- <template #header>
					<div class="card-header">
						<span>盘点条件</span>
					</div>
				</template> -->
				<!-- <template v-if="state.header['stockCheckType'] == '按库区盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="库区" prop="Area">
								<el-input size="small" v-model="ruleForm.Area" placeholder="请输入库区" clearable />
							</el-form-item>
						</el-col>
					</el-row>
				</template> -->
				<!-- <template v-if="state.header['stockCheckType'] == '按库位盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="库位" prop="externNumber">
								<el-input size="small" type="textarea" v-model="ruleForm.Location" placeholder="请输入库位"
									clearable />
							</el-form-item>
						</el-col>

					</el-row>
				</template> -->
				<!-- <template v-if="state.header['stockCheckType'] == '按SKU盘点'">
					<el-row :gutter="[16, 15]">
						<el-col :xs="6" :sm="6" :md="6" :lg="6" :xl="6" class="mb20">
							<el-form-item label="SKU" prop="sku">
								<el-input size="small" type="textarea" v-model="ruleForm.sku" placeholder="请输入SKU"
									clearable />
							</el-form-item>
						</el-col>

					</el-row>
				</template> -->
				<!-- <template v-if="state.header['stockCheckType'] == '动态库位盘点'">
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
				<el-button @click="handleQuery" type="primary" size="large" class="toolbar-btn">查询</el-button> -->
			</el-card>

			<el-card>
				<el-form label-position="top" :model="state" ref="detailRuleRef"  >
							<el-table :data="state.details" height="250">
								<template v-for="(v, index) in state.tableColumnDetails">
									<el-table-column v-if="v.isCreate" :key="index" style="margin:0;padding:0;"
										:fixed="false" :prop="v.columnName" :label="v.displayName" width="150">
										<template #default="scope">
											<el-form-item :key="scope.row.key" style="margin:0;padding:0;"
												>
												<template v-if="v.type == 'TextBox'">
												 {{scope.row[v.columnName]}}
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
													 {{scope.row[v.columnName]}}
												</template>
												<template v-if="v.type == 'DropDownListStrRemote'">
													 {{scope.row[v.columnName]}}
												</template>
												<template v-if="v.type == 'DatePicker'">
													 {{scope.row[v.columnName]}}
												</template>
												<template v-if="v.type == 'DateTimePicker'">
													 {{scope.row[v.columnName]}}
												</template>
												<template v-if="v.type == 'InputNumber'">
													 {{scope.row[v.columnName]}}
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
import { pageWMSStockCheck, deleteWMSStockCheck,getWMSStockCheck } from '/@/api/main/wMSStockCheck';
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/preOrder";
import Detail from "../../../../entities/preOrderDetail";

import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';
 


const state = ref({
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Detail>(),
 

	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),

 

	// header: new Array<Details>(),
})

//父级传递来的参数
var props = defineProps({
	title: {
		type: String,
		default: "",
	},
});
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
// const ruleFormRef = ref();
const isShowDialog = ref(false);
let activeMainName: string = 'OrderInfo';
// const ruleForm = ref<any>({});
//自行添加其他规则
// const rules = ref<FormRules>({
// });

// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.header = JSON.parse(JSON.stringify(row));

	isShowDialog.value = true;
	// console.log("dasdsdasd");
	gettableColumn();
	get()
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


const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_StockCheck");
	state.value.tableColumnHeaders = res.data.result;

	let resDetail = await getByTableNameList("WMS_StockCheckDetail");
	state.value.tableColumnDetails = resDetail.data.result;

 

};

const get = async () => {
	let result = await getWMSStockCheck(state.value.header.id);
	if (result.data.result != null) {
		state.value.header = result.data.result;
		state.value.details = result.data.result.details; 
	}
	console.log(state.value );
}


// 页面加载时
onMounted(async () => {
	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




 