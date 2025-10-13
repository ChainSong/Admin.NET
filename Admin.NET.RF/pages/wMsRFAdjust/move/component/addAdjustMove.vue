<!-- components/add-adjustment-modal.vue -->
<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF移库</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group">
				<view class="title">请扫描</view>
				<input placeholder="源库位 / SKU / 目标库位" v-model.trim="formData.scanValue" @confirm="handleScanConfirm"
					@input="handleScanInput" confirm-type="done" type="text" focus />
			</view>
			<!-- 扫描数据展示表格 -->
			<view class="scan-table-container">
				<uni-table border>
					<uni-tr>
						<uni-th width="80" align="center">源库位</uni-th>
						<uni-th width="80" align="center">SKU</uni-th>
						<uni-th width="80" align="center">数量</uni-th>
						<uni-th width="80" align="center">目标库位</uni-th>
					</uni-tr>
					<uni-tr v-for="(item, index) in scanData" :key="index">
						<uni-td align="center">{{ item.sourceLocation }}</uni-td>
						<uni-td align="center">{{ item.sku }}</uni-td>
						<uni-td align="center">{{ item.qty }}</uni-td>
						<uni-td align="center">{{ item.targetLocation }}</uni-td>
					</uni-tr>
				</uni-table>
			</view>
		</you-scroll>
	</view>
</template>

<script>
	import {
		checkScanValue
	} from '@/services/wMsRFAdjust/move/move.js'
	export default {
		name: 'AddAdjustmentModal',
		props: {
			// 控制弹窗显示
			show: {
				type: Boolean,
				default: false
			}
		},
		data() {
			return {
				loading: false,
				formData: {
					customerId: 0, // 默认空值，稍后从 URL 参数中获取
					warehouseId: 0, // 默认空值，稍后从 URL 参数中获取
					scanValue: '', // 扫描值
					type:'库存移动',
					opSerialNumber :'',
				},
				// 用于存储扫描数据
				scanData: [],
				scanTimer: ''
			}
		},
		watch: {
			// 监听show变化，重置表单
			show(newVal) {
				if (!newVal) {
					this.resetForm()
				}
			}
		},
		methods: {
			// 获取当前页面加载的参数
			onLoad(options) {
				// 从 URL 参数获取传递的值
				this.formData.customerId = options.customer || '';
				this.formData.warehouseId = options.warehouse || '';
			},
			// 表单验证
			validateForm() {
				if (!this.formData.targetWarehouse) {
					uni.showToast({
						title: '请输入目标仓库',
						icon: 'none'
					})
					return false
				}
				if (!this.formData.quantity || this.formData.quantity <= 0) {
					uni.showToast({
						title: '请输入正确的移库数量',
						icon: 'none'
					})
					return false
				}

				return true
			},
			// 监听输入事件：防抖处理（防止连续扫描重复触发）
			handleScanInput(e) {
				if (this.scanTimer) clearTimeout(this.scanTimer)
				this.scanTimer = setTimeout(() => {
					if (e.detail.value && e.detail.value.length > 3) {
						this.handleScanConfirm()
					}
				}, 500)
			},
			async handleScanConfirm() {
				const value = this.formData.scanValue?.trim();
				if (!value) return

				this.loading = true
				try {
					const res = await checkScanValue({
						customerId: this.formData.customerId,
						warehouseId: this.formData.warehouseId,
						scanValue: value,
						type: this.formData.type
					})
					console.log("res",res)
					if (res?.result === 'Success') {
						uni.showToast({
							title: res.message || '扫描成功',
							icon: 'success'
						})

						// 更新操作序列号（后端生成的）
						if (res.serialNumber)
							this.formData.opSerialNumber = res.serialNumber

						// 更新展示数据（后端返回 outputs）
						if (res.outputs && res.outputs.length > 0) {
							this.scanData = res.outputs
						}
					} else {
						uni.showToast({
							title: res?.message || `${res.data.result.result,res.data.result.message}`,
							icon: 'none'
						})
					}
				} catch {} finally {
					this.loading = false;
				}
			},
			// 提交表单
			handleSubmit() {
				if (!this.validateForm()) {
					return
				}

				this.loading = true

				// 模拟API调用
				setTimeout(() => {
					this.loading = false

					// 触发成功事件，传递表单数据
					this.$emit('success', {
						...this.formData,
						createTime: this.getCurrentTime()
					})

					uni.showToast({
						title: '新增成功',
						icon: 'success'
					})

					// 关闭弹窗
					this.handleClose()

				}, 1000)
			},

			// 关闭弹窗
			handleClose() {
				this.$emit('update:show', false)
				this.$emit('close')
			},

			// 重置表单
			resetForm() {
				this.formData = {
					scanValue: ''
				}
				this.loading = false
			},

			// 获取当前时间
			getCurrentTime() {
				const now = new Date()
				const year = now.getFullYear()
				const month = String(now.getMonth() + 1).padStart(2, '0')
				const day = String(now.getDate()).padStart(2, '0')
				return `${year}-${month}-${day}`
			}
		}
	}
</script>

<style scoped>
	.cu-form-group .title {
		min-width: calc(4em + 15px);
	}
</style>