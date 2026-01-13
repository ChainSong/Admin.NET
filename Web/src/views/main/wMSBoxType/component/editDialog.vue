<template>
	<div class="wMSBoxType-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="35">
					<el-form-item v-show="false">
						<el-input v-model="ruleForm.id" />
					</el-form-item>
					<!-- <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="CustomerId" prop="customerId">
							<el-input v-model="ruleForm.customerId" placeholder="请输入CustomerId" clearable />
							
						</el-form-item>
						
					</el-col> -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="客户名称" prop="customerName">
							<el-input v-model="ruleForm.customerName" placeholder="请输入客户名称" clearable />
							
						</el-form-item>
						
					</el-col>
					<!-- <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="WarehouseId" prop="warehouseId">
							<el-input v-model="ruleForm.warehouseId" placeholder="请输入WarehouseId" clearable />
							
						</el-form-item>
						
					</el-col> -->
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="仓库名称" prop="warehouseName">
							<el-input v-model="ruleForm.warehouseName" placeholder="请输入仓库名称" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="箱型号" prop="boxCode">
							<el-input v-model="ruleForm.boxCode" placeholder="请输入箱型号" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="箱类型" prop="boxType">
							<el-input v-model="ruleForm.boxType" placeholder="请输入箱类型" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="长度" prop="length">
							<el-input v-model="ruleForm.length" placeholder="请输入长度" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="宽度" prop="width">
							<el-input v-model="ruleForm.width" placeholder="请输入宽度" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="高度" prop="height">
							<el-input v-model="ruleForm.height" placeholder="请输入高度" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="体积" prop="volume">
							<el-input v-model="ruleForm.volume" placeholder="请输入体积" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="净重" prop="netWeight">
							<el-input v-model="ruleForm.netWeight" placeholder="请输入净重" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="毛重" prop="grossWeight">
							<el-input v-model="ruleForm.grossWeight" placeholder="请输入毛重" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="箱状态" prop="boxStatus">
							<el-input-number v-model="ruleForm.boxStatus" placeholder="请输入箱状态" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="备注" prop="remark">
							<el-input v-model="ruleForm.remark" placeholder="请输入备注" clearable />
							
						</el-form-item>
						
					</el-col>
					<!-- <el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Str1" prop="str1">
							<el-input v-model="ruleForm.str1" placeholder="请输入Str1" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Str2" prop="str2">
							<el-input v-model="ruleForm.str2" placeholder="请输入Str2" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Str3" prop="str3">
							<el-input v-model="ruleForm.str3" placeholder="请输入Str3" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Str4" prop="str4">
							<el-input v-model="ruleForm.str4" placeholder="请输入Str4" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Str5" prop="str5">
							<el-input v-model="ruleForm.str5" placeholder="请输入Str5" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Creator" prop="creator">
							<el-input v-model="ruleForm.creator" placeholder="请输入Creator" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="CreationTime" prop="creationTime">
							<el-date-picker v-model="ruleForm.creationTime" type="date" placeholder="CreationTime" />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Updator" prop="updator">
							<el-input v-model="ruleForm.updator" placeholder="请输入Updator" clearable />
							
						</el-form-item>
						
					</el-col> -->
				</el-row>
			</el-form>
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
	import { ref,onMounted } from "vue";
	import { ElMessage } from "element-plus";
	import type { FormRules } from "element-plus";
	import { addWMSBoxType, updateWMSBoxType } from "/@/api/main/wMSBoxType";
	//父级传递来的参数
	var props = defineProps({
	title: {
	type: String,
	default: "",
	},
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
        await updateWMSBoxType(values);
      } else {
        await addWMSBoxType(values);
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





// 页面加载时
onMounted(async () => {
});

//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>




