<template>
	<div class="wMSASNCountQuantity-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="1200" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="35">

					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="ASN单号" prop="asnNumber">
							{{ ruleForm.asnNumber }}
						</el-form-item>

					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="点数单号" prop="asnCountQuantityNumber">
							{{ ruleForm.asnCountQuantityNumb }}
						</el-form-item>

					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="外部单号" prop="externReceiptNumber">
							{{ ruleForm.externReceiptNumber }}
						</el-form-item>

					</el-col>

					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="客户名称" prop="customerName">
							{{ ruleForm.customerName }}
						</el-form-item>

					</el-col>

					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库名称" prop="warehouseName">
							{{ ruleForm.warehouseName }}
						</el-form-item>

					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库日期" prop="expectDate">
							{{ ruleForm.expectDate }}
						</el-form-item>

					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="点数状态" prop="aSNCountQuantityStatus">
							<template v-for="item in state.asnCountQuantityoptions">
								<el-tag v-if="item.value == ruleForm.asnCountQuantityStatus" v-bind:key="item.value"
									show-icon>
									{{ item.label }}
								</el-tag>
								<!-- <template v-if="item.codeInt == state.header[i.columnName]">
											<label show-icon :type="item.color" v-text="item.name"
												:key="item.codeInt"></label>
										</template> -->
							</template>
							<!-- <el-input-number v-model="ruleForm.asnCountQuantityStatus"
								placeholder="请输入ASNCountQuantityStatus" clearable /> -->

						</el-form-item>

					</el-col>
					<!-- <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="入库单" prop="receiptType">
							{{ruleForm.receiptType}}
						</el-form-item>
						
					</el-col> -->


					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="点数人" prop="creator">
							{{ ruleForm.creator }}
						</el-form-item>

					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="创建时间" prop="creationTime">
							{{ ruleForm.creationTime }}
						</el-form-item>
					</el-col>
				</el-row>
			</el-form>
			<el-form>
				<el-table :data="ruleForm.details" style="width: 100%">
					<el-table-column prop="sku" label="sku" width="180">
					</el-table-column>
					<el-table-column prop="goodsName" label="产品名称" width="180">
					</el-table-column>
					<el-table-column prop="qty" label="数量">
					</el-table-column>
					<el-table-column prop="goodsType" label="品级">
					</el-table-column>
					<el-table-column prop="batchCode" label="批次">
					</el-table-column>
					<el-table-column prop="snCode" label="验证码">
					</el-table-column>
					<el-table-column prop="expirationDate" label="过期时间">
					</el-table-column>
				</el-table>
			</el-form>
			<template #footer>
				<span class="dialog-footer">
					<el-button @click="cancel" size="default">取 消</el-button>
					<!-- <el-button type="primary" @click="submit" size="default">确 定</el-button> -->
				</span>
			</template>
		</el-dialog>
	</div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from "vue";
import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { addWMSASNCountQuantity, updateWMSASNCountQuantity, getWMSASNCountQuantity } from "/@/api/main/wMSASNCountQuantity";
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
	asnCountQuantityoptions: [{ "label": "新增", "value": "1" }, { "label": "完成", "value": "99" }],
});
//父级传递来的函数，用于回调
const emit = defineEmits(["reloadTable"]);
const ruleFormRef = ref();
const isShowDialog = ref(false);
const ruleForm = ref<any>({});
//自行添加其他规则
const rules = ref<FormRules>({
});

// 打开弹窗
const openDialog = (row: any) => {
	ruleForm.value = JSON.parse(JSON.stringify(row));
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

const get = async () => {
	let result = await getWMSASNCountQuantity(ruleForm.value.id);
	console.log(result);
	if (result.data.result != null) {
		ruleForm.value = result.data.result;
		ruleForm.value.details = result.data.result.details;
	}
	console.log(ruleForm.value);
	console.log("ruleForm");
}
// // 提交
// const submit = async () => {
// 	ruleFormRef.value.validate(async (isValid: boolean, fields?: any) => {
// 		if (isValid) {
// 			let values = ruleForm.value;
// 			if (ruleForm.value.id != undefined && ruleForm.value.id > 0) {
// 				await updateWMSASNCountQuantity(values);
// 			} else {
// 				await addWMSASNCountQuantity(values);
// 			}
// 			closeDialog();
// 		} else {
// 			ElMessage({
// 				message: `表单有${Object.keys(fields).length}处验证失败，请修改后再提交`,
// 				type: "error",
// 			});
// 		}
// 	});
// };





// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>
