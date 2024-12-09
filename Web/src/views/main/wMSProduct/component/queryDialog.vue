<template>
	<div class="wMSProduct-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-container>
				<el-main>
					<el-descriptions class="margin-top" :column="2" size="small" border>
						<template v-for="i in state.tableColumnHeaders.filter(a => a.isCreate == 1 || a.isKey == 1)"
							v-bind:key="i.id">
							<el-descriptions-item :prop="i.displayName" :label="i.displayName">
								<!-- <template>
								</template> -->
								<template v-if="i.type == 'TextBox'">
									<label font-family="Helvetica Neue" v-text="state.header[i.columnName]"></label>
								</template>
								<template v-if="i.type == 'DropDownListStr'">
									<template v-for="item in i.tableColumnsDetails">
										<el-tag v-if="item.codeStr == state.header[i.columnName]" v-bind:key="item.color"
											show-icon :type="item.color">
											{{ item.name }}
										</el-tag>
										<!-- <label v-if="item.codeStr == state.header[i.columnName]" v-text="item.name"
											show-icon :type="item.color" :key="item.codeStr"></label> -->
									</template>
								</template>
								<template v-if="i.type == 'DropDownListInt'">
									<template v-for="item in i.tableColumnsDetails">
										<template v-if="item.codeStr == state.header[i.columnName]">
											<el-tag v-if="item.codeInt == state.header[i.columnName]"
												v-bind:key="item.color" show-icon :type="item.color">
												{{ item.name }}
											</el-tag>
											<!-- <label show-icon :type="item.color" v-text="item.name"
												:key="item.codeInt"></label> -->
										</template>
									</template>
								</template>
								<template v-if="i.type == 'DropDownListStrRemote'">
									<label font-family="Helvetica Neue" v-text="state.header[i.columnName]"></label>
								</template>
							</el-descriptions-item>
						</template>
					</el-descriptions>
				</el-main>
				
			</el-container>
			<el-container title="明细信息">
				<el-main>
					<el-form>
						<el-table :data="state.details" style="width: 100%" height="250">
							<template v-for="(v, index) in state.tableColumnDetails">
								<el-table-column v-if="v.isCreate || v.isKey" :key="index" :fixed="false"
									:label="v.displayName" width="150">
									<template #default="scope">
										<label v-text="scope.row[v.columnName]"></label>
									</template>
								</el-table-column>
							</template>

							<!-- <el-table-column fixed="right" label="操作" width="90">
								<template #header>
									<el-select placeholder="请选择">
										<template v-for="item in state.tableColumnDetails">
										<el-option v-if="item.isCreate==1" :key="item.value">
											<el-checkbox  @change="checked => showColumnOption(checked, item)"
												:true-label="1" :false-label="0" :label="item.displayName"
												:key="item.columnName" v-model="item.isCreate">{{ item.displayName}}</el-checkbox>
										</el-option>
									</template>
									</el-select>
								</template>
							</el-table-column> -->

						</el-table>
					</el-form>
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
import { addWMSProduct, updateWMSProduct, getWMSProduct } from "/@/api/main/wMSProduct";
import { getByTableNameList } from "/@/api/main/tableColumns";
import Header from "/@/entities/Product";
import Detail from "/@/entities/productBom";

// import Detail from "/@/entities/customerDetail";
import TableColumns from "/@/entities/tableColumns";
// import { getWMSCustomer } from "/@/api/main/wMSCustomer";

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
// const ruleForm = ref<any>({});
//自行添加其他规则
// const rules = ref<FormRules>({
// });

// 打开弹窗
const openDialog = (row: any) => {
	// ruleForm.value = JSON.parse(JSON.stringify(row));
	state.value.header = JSON.parse(JSON.stringify(row));

	isShowDialog.value = true;
	console.log("dasdsdasd");
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
	let res = await getByTableNameList("WMS_Product");
	state.value.tableColumnHeaders = res.data.result;

	// let resDetail = await getByTableNameList("CustomerDetail");
	// state.value.tableColumnDetails = resDetail.data.result;
	let resDetail = await getByTableNameList("WMS_ProductBom");
	// console.log("asdasdasdasdasdasddasdas")
	// console.log(resDetail);
	state.value.tableColumnDetails = resDetail.data.result;
	

};

const get = async () => {
	let result = await getWMSProduct(state.value.header.id);
	state.value.header = result.data.result;
	state.value.details = result.data.result.details;

}


// 页面加载时
onMounted(async () => {
	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




