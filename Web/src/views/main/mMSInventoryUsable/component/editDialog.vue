<template>
	<div class="mMSInventoryUsable-container">
		<el-dialog v-model="isShowDialog" :title="props.title" :width="700" draggable="">
			<el-form :model="ruleForm" ref="ruleFormRef" size="default" label-width="100px" :rules="rules">
				<el-row :gutter="35">
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Id" prop="id">
							<el-input v-model="ruleForm.id" placeholder="请输入Id" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="ReceiptReceivingId" prop="receiptReceivingId">
							<el-input v-model="ruleForm.receiptReceivingId" placeholder="请输入ReceiptReceivingId" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="ReceiptReceivingDetailId" prop="receiptReceivingDetailId">
							<el-input v-model="ruleForm.receiptReceivingDetailId" placeholder="请输入ReceiptReceivingDetailId" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="SupplierId" prop="supplierId">
							<el-input v-model="ruleForm.supplierId" placeholder="请输入SupplierId" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="SupplierName" prop="supplierName">
							<el-input v-model="ruleForm.supplierName" placeholder="请输入SupplierName" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="WarehouseId" prop="warehouseId">
							<el-input v-model="ruleForm.warehouseId" placeholder="请输入WarehouseId" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="WarehouseName" prop="warehouseName">
							<el-input v-model="ruleForm.warehouseName" placeholder="请输入WarehouseName" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Area" prop="area">
							<el-input v-model="ruleForm.area" placeholder="请输入Area" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Location" prop="location">
							<el-input v-model="ruleForm.location" placeholder="请输入Location" clearable />
						</el-form-item>
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="SKU" prop="sKU">
							<el-input v-model="ruleForm.sKU" placeholder="请输入SKU" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="UPC" prop="uPC">
							<el-input v-model="ruleForm.uPC" placeholder="请输入UPC" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="GoodsType" prop="goodsType">
							<el-input v-model="ruleForm.goodsType" placeholder="请输入GoodsType" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="InventoryStatus" prop="inventoryStatus">
							<el-input-number v-model="ruleForm.inventoryStatus" placeholder="请输入InventoryStatus" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="SuperId" prop="superId">
							<el-input v-model="ruleForm.superId" placeholder="请输入SuperId" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="RelatedId" prop="relatedId">
							<el-input v-model="ruleForm.relatedId" placeholder="请输入RelatedId" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="GoodsName" prop="goodsName">
							<el-input v-model="ruleForm.goodsName" placeholder="请输入GoodsName" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="UnitCode" prop="unitCode">
							<el-input v-model="ruleForm.unitCode" placeholder="请输入UnitCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Onwer" prop="onwer">
							<el-input v-model="ruleForm.onwer" placeholder="请输入Onwer" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="BoxCode" prop="boxCode">
							<el-input v-model="ruleForm.boxCode" placeholder="请输入BoxCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="TrayCode" prop="trayCode">
							<el-input v-model="ruleForm.trayCode" placeholder="请输入TrayCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="BatchCode" prop="batchCode">
							<el-input v-model="ruleForm.batchCode" placeholder="请输入BatchCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="LotCode" prop="lotCode">
							<el-input v-model="ruleForm.lotCode" placeholder="请输入LotCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="PoCode" prop="poCode">
							<el-input v-model="ruleForm.poCode" placeholder="请输入PoCode" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Weight" prop="weight">
							<el-input v-model="ruleForm.weight" placeholder="请输入Weight" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Volume" prop="volume">
							<el-input v-model="ruleForm.volume" placeholder="请输入Volume" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Qty" prop="qty">
							<el-input v-model="ruleForm.qty" placeholder="请输入Qty" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="ProductionDate" prop="productionDate">
							<el-date-picker v-model="ruleForm.productionDate" type="date" placeholder="ProductionDate" />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="ExpirationDate" prop="expirationDate">
							<el-date-picker v-model="ruleForm.expirationDate" type="date" placeholder="ExpirationDate" />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Remark" prop="remark">
							<el-input v-model="ruleForm.remark" placeholder="请输入Remark" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="InventoryTime" prop="inventoryTime">
							<el-date-picker v-model="ruleForm.inventoryTime" type="date" placeholder="InventoryTime" />
							
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
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
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
						<el-form-item label="DateTime1" prop="dateTime1">
							<el-date-picker v-model="ruleForm.dateTime1" type="date" placeholder="DateTime1" />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="DateTime2" prop="dateTime2">
							<el-date-picker v-model="ruleForm.dateTime2" type="date" placeholder="DateTime2" />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Int1" prop="int1">
							<el-input-number v-model="ruleForm.int1" placeholder="请输入Int1" clearable />
							
						</el-form-item>
						
					</el-col>
					<el-col :xs="24" :sm="12" :md="12" :lg="12" :xl="12" class="mb20">
						<el-form-item label="Int2" prop="int2">
							<el-input-number v-model="ruleForm.int2" placeholder="请输入Int2" clearable />
							
						</el-form-item>
						
					</el-col>
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
	import { addMMSInventoryUsable, updateMMSInventoryUsable } from "/@/api/main/mMSInventoryUsable";
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
        await updateMMSInventoryUsable(values);
      } else {
        await addMMSInventoryUsable(values);
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




