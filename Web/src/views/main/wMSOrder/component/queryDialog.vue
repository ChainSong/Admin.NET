<template>
	<div class="wMSOrder-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">
			<el-container>
				<el-main>
					<el-tabs v-model="activeMainName">
						<el-tab-pane label="订单信息" name="OrderInfo">
							<el-descriptions class="margin-top" :column="2" size="small" border>
								<template v-for="i in state.tableColumnHeaders">
									<el-descriptions-item v-bind:key="i.id" :prop="i.displayName" :label="i.displayName"
										v-if="i.isCreate || i.isKey">
										<template>
											<!-- <i></i>
									{{ i.displayName }} -->
										</template>
										<template v-if="i.type == 'DropDownListStr'">
											<template v-for="item in i.tableColumnsDetails">
												<label v-if="item.codeStr == state.header[i.columnName]" v-text="item.name"
													show-icon :type="item.color" :key="item.codeStr"></label>
											</template>
										</template>
										<template v-else-if="i.type == 'DropDownListInt'">
											<template v-for="item in i.tableColumnsDetails">
												<template v-if="item.codeStr == state.header[i.columnName]">
													<label show-icon :type="item.color" v-text="item.name"
														:key="item.codeInt"></label>
												</template>
											</template>
										</template>
										<template v-else>
											<label font-family="Helvetica Neue" v-text="state.header[i.columnName]"></label>
										</template>
									</el-descriptions-item>
								</template>
							</el-descriptions>
							</el-tab-pane>
							<el-tab-pane label="地址信息" name="AddressInfo">
								<el-descriptions class="margin-top" :column="2" size="small" border>
									<template v-for="i in state.tableColumnOrderAddresss">
										<el-descriptions-item v-bind:key="i.id" :prop="i.displayName" :label="i.displayName"
											v-if="i.isCreate || i.isKey">
											<template>
												<!-- <i></i>
									{{ i.displayName }} -->
											</template>
											<template v-if="i.type == 'DropDownListStr'">
												<template v-for="item in i.tableColumnsDetails">
													<label v-if="item.codeStr == state.orderAddress[i.columnName]"
														v-text="item.name" show-icon :type="item.color"
														:key="item.codeStr"></label>
												</template>
											</template>
											<template v-else-if="i.type == 'DropDownListInt'">
												<template v-for="item in i.tableColumnsDetails">
													<template v-if="item.codeStr == state.orderAddress[i.columnName]">
														<label show-icon :type="item.color" v-text="item.name"
															:key="item.codeInt"></label>
													</template>
												</template>
											</template>
											<template v-else>
												<label font-family="Helvetica Neue"
													v-text="state.orderAddress[i.columnName]"></label>
											</template>
										</el-descriptions-item>
									</template>
								</el-descriptions>
							</el-tab-pane>
					</el-tabs>
				</el-main>
			</el-container>

			<el-tabs v-model="activeName">
				<el-tab-pane label="明细信息" name="DateilInfo">
					<el-table :data="state.details" style="width: 100%" height="250">
						<template v-for="(v, index) in state.tableColumnDetails">
							<el-table-column v-if="v.isCreate" :key="index" :fixed="false" :label="v.displayName"
								width="150">
								<template #default="scope">
									<label v-text="scope.row[v.columnName]"></label>
								</template>
							</el-table-column>
						</template>
					</el-table>
				</el-tab-pane>
				<el-tab-pane label="分配信息" name="AllocationInfo">
					<el-table :data="state.allocations" style="width: 100%" height="250">
						<template v-for="(v, index) in state.tableColumnAllocations">
							<el-table-column v-if="v.isCreate" :key="index" :fixed="false" :label="v.displayName"
								width="150">
								<template #default="scope">
									<label v-text="scope.row[v.columnName]"></label>
								</template>
							</el-table-column>
						</template>
					</el-table>
				</el-tab-pane>
			</el-tabs>


			<template #footer>
				<span class="dialog-footer">
					<!-- <el-button @click="cancel" size="default">取 消</el-button> -->
					<el-button type="primary" @click="cancel" size="default">确 定</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSOrder, updateWMSOrder, getWMSOrder } from "/@/api/main/wMSOrder";
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/Order";
import Detail from "/@/entities/OrderDetail";
import allocations from "/@/entities/orderAllocation";

import OrderAddress from "/@/entities/orderAddress";
// import Dllocations from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue'

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
	allocations: new Array<allocations>(),

	orderAddress: new OrderAddress(),
	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),
	tableColumnAllocations: new Array<TableColumns>(),
	
	tableColumnOrderAddress: new TableColumns(),
	tableColumnOrderAddresss: new Array<TableColumns>(),
	// header: new Array<Details>(),
})

// let headerRuleRef = ref<any>({});
// let headerRule = ref({});
// let detailRuleRef = ref<any>({});
// let detailRule = ref({});
//  let activeName = ref({});
let activeName: string = 'DateilInfo';

let activeMainName: string = 'OrderInfo';

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

// 打开弹窗
const openDialog = (row: any) => {
	state.value.header = JSON.parse(JSON.stringify(row));
	isShowDialog.value = true;
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

//获取界面涉及的表字段关系
const gettableColumn = async () => {
	let res = await getByTableNameList("WMS_Order");
	state.value.tableColumnHeaders = res.data.result;

	let resDetail = await getByTableNameList("WMS_OrderDetail");

	state.value.tableColumnDetails = resDetail.data.result;

	let resAllocation = await getByTableNameList("WMS_OrderAllocation");
	state.value.tableColumnAllocations = resAllocation.data.result;

	
	let resorderAddress = await getByTableNameList("WMS_OrderAddress");
	state.value.tableColumnOrderAddresss = resorderAddress.data.result;

	console.log("state.value.tableColumnAllocations")
	console.log(state.value.tableColumnAllocations)
};
//获取订单信息
const get = async () => {
	let result = await getWMSOrder(state.value.header.id);
	console.log("result");
	console.log(result);
	if (result.data.result != null) {
		state.value.header = result.data.result;
		state.value.details = result.data.result.details;
		state.value.allocations = result.data.result.allocation;
	}
	console.log("state.value.allocations");
	console.log(state.value.allocations);
}


// 页面加载时
onMounted(async () => {
	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




