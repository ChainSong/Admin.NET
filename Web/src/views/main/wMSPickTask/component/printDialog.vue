<template>
	<div class="wMSPickTask-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1000" draggable="">
			<div id="preview_content" ref="previewContentRef"></div>
			<template #footer>
				<el-button @click="cancel" size="default">取 消</el-button>
				<span class="dialog-footer">
					<el-button :loading="state.waitShowPrinter" type="primary" icon="ele-Printer"
						@click="print">打印</el-button>
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted, nextTick, reactive } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSPickTask, updateWMSPickTask, getWMSPickTask, addWMSPickTaskPrintLog } from "/@/api/main/wMSPickTask";
import { getByTableNameList } from "/@/api/main/tableColumns";
import { queryPrintTemplate } from "/@/api/main/printTemplate";
import Header from "/@/entities/pickTask";
import Detail from "/@/entities/pickTaskDetail";
import TableColumns from "/@/entities/tableColumns";
import selectRemote from '/@/views/tools/select-remote.vue'
import printJS from 'print-js'
// import { getAPI } from '/@/utils/axios-utils';
// import { SysPrintApi } from '/@/api-services/api';
// import { SysPrint } from '/@/api-services/models';
import { hiprint } from 'vue-plugin-hiprint';
const state = ref({
	vm: {
		id: "",
		form: {
			details: []
		} as any,
		header: new Header(),
		details: new Array<Detail>(),
	},
	visible: false,
	loading: false,
	header: new Header(),
	headers: new Array<Header>(),
	details: new Array<Detail>(),

	tableColumnHeader: new TableColumns(),
	tableColumnHeaders: new Array<TableColumns>(),
	tableColumnDetail: new TableColumns(),
	tableColumnDetails: new Array<TableColumns>(),

	//   ----------打印需要的数据
	// dialogVisible: false,
	// waitShowPrinter: false,
	// width: 0, // 纸张宽 mm
	// printData: {}, // 打印数据
	// hiprintTemplate: {} as any,
	// header: new Array<Details>(),
})

//打印需要的data集合
const staterReactive = reactive({
	dialogVisible: false,
	waitShowPrinter: false,
	width: 0, // 纸张宽 mm
	printData: {}, // 打印数据
	hiprintTemplate: {} as any,
});

// let headerRuleRef = ref<any>({});
// let headerRule = ref({});
// let detailRuleRef = ref<any>({});
// let detailRule = ref({});
// let contentToPrintRef = ref({});

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

const previewContentRef = ref();
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
	// gettableColumn();
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

//构建打印区域
const showDialog = (hiprintTemplate: any, printData: {}, width = 210) => {
	nextTick(() => {
		const newHtml = staterReactive.hiprintTemplate.getHtml(JSON.parse(printData));
		previewContentRef.value.appendChild(newHtml[0]);
	});
};

//获取表字段信息
// const gettableColumn = async () => {
// 	let res = await getByTableNameList("WMS_PickTask");
// 	state.value.tableColumnHeaders = res.data.result;
// 	let resDetail = await getByTableNameList("WMS_PickTaskDetail");
// 	state.value.tableColumnDetails = resDetail.data.result;
// };

//打印
// const printDocument = () => {
// 	printJS({
// 		printable: previewContentRef.value,
// 		type: 'html'
// 	})
// }

//打印
const print = async () => {
	staterReactive.waitShowPrinter = true;
	// console.log(staterReactive.printData);
	//修改打印的时间和打印次数
	await addWMSPickTaskPrintLog(state.value.header.id);

	staterReactive.hiprintTemplate.print(
		staterReactive.printData,
		{},
		{
			callback: () => {
				staterReactive.waitShowPrinter = false;
			},
		}
	);
};
//打印PDF
// const toPdf = () => {
// 	staterReactive.hiprintTemplate.toPdf(	staterReactive.printData,{}, 'PDF文件');
// };

//获取订单数据
const get = async () => {

	let result = await getWMSPickTask(state.value.header.id);
	console.log("result");
	console.log(result);
	if (result.data.result != null) {
		state.value.header = result.data.result;
		state.value.details = result.data.result.details;
	}
	getPrintTemplate();
}

// 获取打印模板
const getPrintTemplate = async () => {
	// var res = await getAPI(SysPrintApi).apiSysPrintPagePost({ "name": "拣货单打印模板" });
	var res = await queryPrintTemplate("拣货单打印模板");
	// console.log("res");
	// console.log(res);
	let printData = res.data.result ?? {};
	// console.log("printData");
	// console.log(printData);
	staterReactive.hiprintTemplate = new hiprint.PrintTemplate({ template: JSON.parse(printData.template) });
	staterReactive.printData = state.value.header;
	showDialog(staterReactive.hiprintTemplate, JSON.stringify(state.value.header))
}


// 页面加载时
onMounted(async () => {

	get();
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




