<template>
	<view class="cu-modal" :class="show ? 'show' : ''">
		<view class="cu-dialog">
			<view class="cu-bar bg-white justify-end">
				<view class="content">选择客户和仓库</view>
				<view class="action" @tap="handleClose">
					<text class="cuIcon-close text-red"></text>
				</view>
			</view>
			<view class="padding-xl">
				<!-- 客户选择 -->
				<view class="cu-form-group">
					<view class="title">客&nbsp;&nbsp;户</view>
					<view class="picker-wrapper">
						<select v-model="formData.customer">
							<option value="">请选择客户</option>
							<option v-for="(item, index) in customerOptions" :key="index" :value="item.value">
								{{ item.label }}
							</option>
						</select>
					</view>
				</view>

				<!-- 源仓库选择 -->
				<view class="cu-form-group">
					<view class="title">仓&nbsp;&nbsp;库</view>
					<view class="picker-wrapper">
						<select v-model="formData.warehouse">
							<option value="">请选择源仓库</option>
							<option v-for="(item, index) in warehouseOptions" :key="index" :value="item.value">
								{{ item.label }}
							</option>
						</select>
					</view>
				</view>
			</view>

			<view class="cu-bar bg-white justify-end">
				<view class="action">
					<button class="cu-btn line-green text-green" @tap="handleClose">取消</button>
					<button class="cu-btn bg-green margin-left" @tap="handleSubmit" :disabled="loading">
						{{ loading ? '提交中...' : '去扫描' }}
					</button>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		name: 'SelectCustomerAndWarehouse',
		props: {
			show: {
				type: Boolean,
				default: false
			},
			warehouseList: {
				type: Array,
				default: () => []
			},
			customerList: {
				type: Array,
				default: () => []
			}
		},
		data() {
			return {
				loading: false,
				formData: {
					customer: '',
					warehouse: ''
				},
				customerOptions: [{
						label: '客户A',
						value: 'CUSTOMER_A'
					},
					{
						label: '客户B',
						value: 'CUSTOMER_B'
					},
					{
						label: '客户C',
						value: 'CUSTOMER_C'
					}
				],
				warehouseOptions: [{
						label: '原材料仓库',
						value: 'RAW_MATERIAL'
					},
					{
						label: '成品仓库',
						value: 'FINISHED_GOODS'
					},
					{
						label: '半成品仓库',
						value: 'SEMI_FINISHED'
					},
					{
						label: '退货仓库',
						value: 'RETURN_GOODS'
					},
					{
						label: '待检仓库',
						value: 'INSPECTION'
					}
				]
			};
		},
		watch: {
			show(newVal) {
				if (!newVal) {
					this.resetForm();
				}
			},
			warehouseList(newVal) {
				if (newVal && newVal.length > 0) {
					this.warehouseOptions = newVal;
				}
			},
			customerList(newVal) {
				if (newVal && newVal.length > 0) {
					this.customerOptions = newVal;
				}
			}
		},
		methods: {
			onCustomerChange(e) {
				this.formData.customer = e.target.value;
			},
			onWarehouseChange(e) {
				this.formData.warehouse = e.target.value;
			},
			validateForm() {
				let validStr = '';
				if (!this.formData.customer) {
					validStr = '客户';
				}
				if (!this.formData.warehouse) {
					validStr += validStr ? ' 和 仓库' : '仓库';
				}
				return validStr;
			},
			handleSubmit() {
				const validRes = this.validateForm();
				if (validRes.includes("仓库") || validRes.includes("客户")) {
					alert(`请选择${validRes}`)
					return;
				}
				this.loading = true;
				setTimeout(() => {
					this.loading = false;
					// 触发 success 事件，并传递 formData
					this.$emit('success', {
						...this.formData
					});
					uni.showToast({
						title: '选择成功',
						icon: 'success'
					});
					this.handleClose();
				}, 1000);
			},
			handleClose() {
				this.$emit('update:show', false);
				this.$emit('close');
			},
			resetForm() {
				this.formData = {
					customer: '',
					warehouse: ''
				};
				this.loading = false;
			}
		}
	};
</script>

<style scoped>
	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}

	.picker-wrapper {
		display: flex;
		flex-direction: column;
	}

	select {
		padding: 10px;
		border-radius: 5px;
		border: 1px solid #ccc;
		width: 100%;
	}
</style>