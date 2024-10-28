<template>
	<div class="sys-formDes-container">
		<el-card shadow="hover" :body-style="{ paddingBottom: '0' }">
			<v-form-render :form-json="state.formJson" :form-data="state.formData" :key="state.formJson"
				:option-data="state.optionData" ref="vFormRef">
			</v-form-render>
			<el-button type="primary" @click="handleQuery">查询</el-button>
			<el-button type="primary" @click="getExcelData">下载</el-button>
			<!-- <el-button type="primary" @click="save">保存</el-button> -->

			
		</el-card>
		<el-card class="full-table" shadow="hover" style="margin-top: 8px">
			<el-table :data="formData" stripe style="width: 100%">
				<el-table-column v-for="(book, key, index) in formData[0]" :key="index" :prop="key" :label="key"
					align="center">
				</el-table-column>
			</el-table>
			<el-pagination v-model:currentPage="tableParams.page" v-model:page-size="tableParams.pageSize"
        :total="tableParams.total" :page-sizes="[10, 20, 50, 100]" small="" background=""
        @size-change="handleSizeChange" @current-change="handleCurrentChange"
        layout="total, sizes, prev, pager, next, jumper" />
		</el-card>
	</div>
</template>
<script lang="ts" setup name="renderPage">
import { ref, onMounted } from 'vue';
import { ElMessageBox, ElMessage } from "element-plus";
import { getData, addWMSLowCode, getWMSLowCode,getExcel } from "/@/api/main/wMSLowCode";
import { useRoute } from 'vue-router'
import { json } from 'stream/consumers';
import { downloadByData, getFileName } from '/@/utils/download';
// import { useRoute, useRouter, onBeforeRouteUpdate, RouteRecordRaw } from 'vue-router';
const state = ref({
	formJson: {},
	formData: [],
	optionData: [],
	configModel: { uiCode: "", sqlCode: "", menuName: "" }
});
let formData = ref("");
let optionData = ref("");
let vFormRef = ref(null);
const route = useRoute()
const tableParams = ref({
  page: 1,
  pageSize: 10,
  total: 0,
});
 
const handleQuery  = async () => {
	// let url = window.location.href;
	var res = await getData(Object.assign({"formDataModel": vFormRef.value.formDataModel,"name":route.name}, tableParams.value));
	let result = res.data.result.data ?? [];
	// vFormRef.value.formDataModel
	// vFormRef.value.formDataModel
	state.value.formData = result;
	state.value.optionData = result;
	state.value.optionData = result;
	formData.value = result;
	optionData.value = result;

};

// const getExcel  = async () => {
// 	// let url = window.location.href;
// 	var res = await getData({"formDataModel": vFormRef.value.formDataModel,"name":route.name});
// 	let result = res.data.result.data ?? [];
// 	// vFormRef.value.formDataModel
// 	// vFormRef.value.formDataModel
// 	state.value.formData = result;
// 	state.value.optionData = result;
// 	state.value.optionData = result;
// 	formData.value = result;
// 	optionData.value = result;

// };

//导出预出库单
const getExcelData = async () => { 

  let res = await getExcel({"formDataModel": vFormRef.value.formDataModel,"name":route.name});
  var fileName = getFileName(res.headers);
  downloadByData(res.data as any, fileName);
}

// state.value.formJson ={"widgetList":[],"formConfig":{"labelWidth":80,"labelPosition":"left","size":"","labelAlign":"label-left-align","cssCode":"","customClass":"","functions":"","layoutType":"PC","onFormCreated":"","onFormMounted":"","onFormDataChange":""}};

// 页面加载时
onMounted(async () => {
	var res = await getWMSLowCode(route.name);
	state.value.formJson = JSON.parse(res.data.result.uiCode);
	state.value.optionData = res.data.result.uiCode; 
});

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


 
</script>
<style lang="scss" scoped>
body {
	margin: 0; // 去除页面垂直滚动条
}
</style>