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

				<input placeholder="源库位 / SKU / 目标库位" v-model.trim="formData.scanValue" @confirm="handleScanAdd"
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
						<uni-td align="center">{{ item.fromLocation }}</uni-td>
						<uni-td align="center">{{ item.sku }}</uni-td>
						<!-- <uni-td align="center">{{ item.qty }}</uni-td> -->
						<view class="qty-operator">
							<button class="btn" @click="decreaseQty(index)" :disabled="!item.sku">-</button>
							<text class="qty">{{ item.qty }}</text>
							<button class="btn" @click="increaseQty(index)" :disabled="!item.sku">+</button>
						</view>
						<uni-td align="center">{{ item.toLocation }}</uni-td>
					</uni-tr>
				</uni-table>
			</view>
		</you-scroll>
	</view>
</template>

<script>
	import {
		checkScanValue,
		completeMove
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
					type: 'RF库存移动',
					opSerialNumber: '',
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
						this.handleScanAdd()
					}
				}, 500)
			},
			increaseQty(index) {
				const item = this.scanData[index]
				if (!item.qty) this.$set(this.scanData[index], 'qty', 0)
				this.scanData[index].qty++
			},

			decreaseQty(index) {
				const item = this.scanData[index]
				if (!item.qty) this.$set(this.scanData[index], 'qty', 0)
				if (this.scanData[index].qty > 0) {
					this.scanData[index].qty--
				} else {
					uni.showToast({
						title: '数量不能小于 0',
						icon: 'none'
					})
				}
			},
			async handleScanAdd() {
				const value = this.formData.scanValue?.trim();
				if (!value) return
				this.loading = true
				try {
					let resReq = await checkScanValue({
						customerId: this.formData.customerId,
						warehouseId: this.formData.warehouseId,
						scanValue: value,
						type: this.formData.type,
						opSerialNumber: this.formData.opSerialNumber
					})
					if (resReq.data.code === 200) {
						let detailSkuRes = resReq.data.result
						console.log("detailSkuRes", detailSkuRes)
						if (detailSkuRes.result === 'Success') {

							if (detailSkuRes.serialNumber)
								this.formData.opSerialNumber = detailSkuRes.serialNumber

							if (detailSkuRes.outputs && detailSkuRes.outputs.length > 0) {
								this.scanData = detailSkuRes.outputs
							}
							uni.showToast({
								title: detailSkuRes.message || '扫描成功',
								icon: 'success'
							})
						} else {
							uni.showToast({
								title: detailSkuRes.message || `${detailSkuRes.result ,detailSkuRes.message}`,
								icon: 'none'
							})
						}
						if (detailSkuRes.result === 'RFSuccess') {
							uni.showToast({
								title: '新增移库单成功',
								icon: 'success'
							})
							const addMoveOrder = this.handleScanConfirm(detailSkuRes.adjustmentId)
						}
						if (detailSkuRes.result === 'RFFaild') {
							// 提示失败，清空当前扫描的数据并返回上一层
							uni.showToast({
								title: '新增移库单失败，请重试',
								icon: 'none'
							})
						}
					} else {
						uni.showToast({
							title: '扫描失败',
							icon: 'none'
						})
					}
				} catch {
					this.resetForm(); // 清空当前扫描的数据
					// ✅ 延时 1 秒再返回，确保用户能看到提示
					setTimeout(() => {
						this.comeBack();
					}, 1000);
				} finally {
					this.loading = false;
				}
			},
			async handleScanConfirm(adjustmentId) {
				this.loading = true;
				try {
					let result = await completeMove({
						id: adjustmentId,
						type: "RF库存移动"
					})
					uni.showToast({
						title: `${result.data.result.response.data[0].msg}`,
						icon: 'none'
					});
					setTimeout(() => {
						this.comeBack();
					}, 1000);
				} catch {
					this.loading = false;
				} finally {
					this.loading = false;

					this.resetForm();
				}
			},
			// 关闭弹窗
			comeBack() {
				console.log("返回上一层") // 扫描成功后返回上一层
				uni.navigateBack({
					delta: 1 // 返回上一层
				});
			},
			// 重置表单
			resetForm() {
				this.formData = {
					scanValue: ''
				}
				this.scanData = []
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

	.qty-operator {
		display: flex;
		align-items: center;
		justify-content: center;
		gap: 10rpx;
	}

	.qty-operator .btn {
		width: 50rpx;
		height: 50rpx;
		line-height: 50rpx;
		text-align: center;
		background-color: #007aff;
		color: #fff;
		border-radius: 8rpx;
		font-size: 28rpx;
	}

	.qty-operator .qty {
		width: 60rpx;
		text-align: center;
		font-weight: bold;
	}
</style>