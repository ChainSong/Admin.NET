<template>
	<div class="wMSPreOrder-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">
			<el-container>
				<el-main>
					<el-tabs v-model="activeMainName">
						<el-tab-pane label="订单信息" name="OrderInfo">
							<el-descriptions class="margin-top" :column="2" size="small" border>
								<template
									v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1 || a.isKey == 1)">
									<el-descriptions-item :prop="i.displayName" :label="i.displayName">
										<template>
											<!-- <i></i>
									{{ i.displayName }} -->
										</template>
										<template v-if="i.type == 'DropDownListStr'">
											<template v-for="item in i.tableColumnsDetails">
												<el-tag v-if="item.codeStr == state.header[i.columnName]"
													v-bind:key="item.color" show-icon :type="item.color">
													{{ item.name }}
												</el-tag>
												<!-- <label v-if="item.codeStr == state.header[i.columnName]" v-text="item.name"
													show-icon :type="item.color" :key="item.codeStr"></label> -->
											</template>
										</template>
										<template v-else-if="i.type == 'DropDownListInt'">
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
										</template>
										<template v-else>
											<label font-family="Helvetica Neue"
												v-text="state.header[i.columnName]"></label>
										</template>
									</el-descriptions-item>
								</template>
							</el-descriptions>
							<el-container title="明细信息">
								<el-main>
									<el-form>
										<el-table :data="state.details" style="width: 100%" height="250">
											<template v-for="(v, index) in state.tableColumnDetails">
												<el-table-column v-if="v.isCreate" :key="index" :fixed="false"
													:label="v.displayName" width="150">

													<template #default="scope">
														<label v-text="scope.row[v.columnName]"></label>
													</template>
												</el-table-column>
											</template>
										</el-table>
									</el-form>
								</el-main>
							</el-container>
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
						<el-tab-pane label="扩展配置" name="extends">
							<el-descriptions  v-for="extend in state.extends" class="margin-top" :column="2" size="small" border>
							 
									<template
										v-for="q in state.tableColumnExtends.filter(q => q.isCreate == 1 || q.isKey == 1)">
										<el-descriptions-item :prop="q.displayName" :label="q.displayName">
											<template v-if="q.type == 'UploadFile'">
												<!-- <a  :href="baseURL + state.extend[q.columnName]" target="_blank">{{ state.extend[q.columnName] }}</a> -->
												<a :href="isBaseURL(extend[q.columnName])" target="_blank">{{
													extend[q.columnName] }}</a>
											</template>
											<template v-else-if="q.type == 'TextBox'">
												<label font-family="Helvetica Neue"
													v-text="extend[q.columnName]"></label>
											</template>
										</el-descriptions-item>
									</template>
							 
							</el-descriptions>
						</el-tab-pane>
					</el-tabs>
				</el-main>
			</el-container>

			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
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
import { addWMSPreOrder, updateWMSPreOrder, getWMSPreOrder } from "/@/api/main/wMSPreOrder";
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/preOrder";
import Detail from "../../../../entities/preOrderDetail";
import Extend from "/@/entities/preOrderExtend";
import OrderAddress from "/@/entities/orderAddress";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue';

// 主体路径
let baseURL = import.meta.env.VITE_API_URL;


const state = ref({
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Detail>(),
	orderAddress: new OrderAddress(),
	extends: new Array<Extend>(),

	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),


	tableColumnOrderAddress: new TableColumns(),
	tableColumnOrderAddresss: new Array<TableColumns>(),

	tableColumnExtend: new TableColumns(),
	tableColumnExtends: new Array<TableColumns>(),

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
const isBaseURL = (row: string) => {
	if (row != "" && row != undefined && row.includes("http")) {
		return row;
	} else {
		return baseURL + row;
	}
};

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
	let res = await getByTableNameList("WMS_PreOrder");
	state.value.tableColumnHeaders = res.data.result;

	let resDetail = await getByTableNameList("WMS_PreOrderDetail");
	state.value.tableColumnDetails = resDetail.data.result;


	let resorderAddress = await getByTableNameList("WMS_OrderAddress");
	state.value.tableColumnOrderAddresss = resorderAddress.data.result;

	let resExtend = await getByTableNameList("WMS_PreOrderExtend");
	state.value.tableColumnExtends = resExtend.data.result;

};

const get = async () => {
	let result = await getWMSPreOrder(state.value.header.id);
	if (result.data.result != null) {
		state.value.header = result.data.result;
		state.value.details = result.data.result.details;
		state.value.orderAddress = result.data.result.orderAddress;
		state.value.extends = result.data.result.extends;
	}
	console.log(state.value);
}


// 页面加载时
onMounted(async () => {
	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>