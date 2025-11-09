<!-- components/add-adjustment-modal.vue -->
<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF交接扫描</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<!-- 展示表单数据 -->
			<!-- 第一行：单号和按钮 -->
			<view class="single-line-container">
				<view class="order-info">
					<view class="label">外部单号</view>
					<view class="value">{{ formData.externOrderNumber || '--' }}</view>
				</view>
				<button class="submit-btn cu-btn bg-pink shadow round sm" @click="handleSubmit">
					提交
				</button>
			</view>

			<!-- 第二行：客户名称和仓库 -->
			<view class="double-line-container">
				<view class="double-line-item">
					<view class="label">客户名称</view>
					<view class="value">{{ formData.customerName || '--' }}</view>
				</view>
				<view class="double-line-item">
					<view class="label">仓库</view>
					<view class="value">{{ formData.warehouseName || '--' }}</view>
				</view>
			</view>

			<view class="cu-form-group">
				<view class="title">请扫描</view>

				<input placeholder="箱号" v-model.trim="formData.packageNumber" @confirm="handleScanAdd(formData)"
					@input="handleScanInput" confirm-type="done" type="text" focus />
			</view>
			<!-- 扫描数据展示表格 -->
			<view class="scan-table-container">
				<uni-table border>
					<uni-tr>
						<uni-th width="80" align="center">箱号</uni-th>
					</uni-tr>
					<uni-tr v-for="(item, index) in scanData" :key="index">
						<uni-td align="center">{{ item.packageNumber }}</uni-td>
					</uni-tr>
				</uni-table>
			</view>
		</you-scroll>

		<!-- 使用封装的新增弹窗组件 -->
		<SpecificationsEdit :show.sync="showPalletModal" @success="handlePalletSuccess" 
		 />
	</view>
</template>

<script>
	import SpecificationsEdit from '@/pages/wMsRFAdjust/handover/component/specificationsEdit.vue'
	import {
		scanPackage,completeHandover
	} from '@/services/wMsRFAdjust/handover/handover.js'
	export default {
		components: {
			SpecificationsEdit
			// AddAdjustmentModal
		},
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
				showPalletModal: false,
				loading: false,
				formData: {
					customerId: 0, // 默认空值，稍后从 URL 参数中获取
					customerName: '',
					warehouseId: 0, // 默认空值，稍后从 URL 参数中获取
					warehouseName: '',
					externOrderNumber: '',
					packageNumber: '',
					type: 'RF交接',
					opSerialNumber: ''
				},
				// 用于存储扫描数据
				scanData: [],
				scanTimer: '',
				palletInfo: null // 存储托盘信息
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
				// 解析 URL 参数
				if (options.formData) {
					try {
						const formDataFromParent = JSON.parse(decodeURIComponent(options.formData))
						// 将数据设置到子页面的 formData 中
						this.formData = {
							...this.formData,
							...formDataFromParent
						}
					} catch (error) {
						console.error('解析参数失败:', error) // 这里也修复拼写错误
					}
				}
			},
			// 监听输入事件：防抖处理（防止连续扫描重复触发）
			handleScanInput(e) {
				if (this.scanTimer) clearTimeout(this.scanTimer)
				this.scanTimer = setTimeout(() => {
					if (e.detail.value && e.detail.value.length > 3) {
						this.handleScanAdd(this.formData)
					}
				}, 500)
			},
			// 扫描
			async handleScanAdd(params) {
				params.OrderId = this.formData.id
				let res = await scanPackage(params)
				let result = res.data.result
				console.log("Res",result)
				if (result.result) {
					this.scanData = result.packages
					this.formData.packageNumber=''
					this.formData.opSerialNumber=result.serialNumber
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
			},
			// 提交交接
			handleSubmit() {
				if (this.scanData.length === 0) {
					uni.showToast({
						title: '请先扫描箱号',
						icon: 'none'
					});
					return;
				}
				// 打开托盘信息弹框
				this.showPalletModal = true;
			},
			// 托盘信息填写成功回调
			async handlePalletSuccess(palletData) {
				this.palletInfo = palletData;
console.log("palletData:",palletData)
				try {
					this.loading = true;
					uni.showLoading({
						title: '提交中...'
					});

					// 构建提交数据
					const submitData = {
						externOrderNumber: this.formData.externOrderNumber,
						customerId: this.formData.customerId,
						customerName: this.formData.customerName,
						warehouseId: this.formData.warehouseId,
						warehouseName: this.formData.warehouseName,
						packages: this.scanData,
						palletInfo: this.palletInfo,
						scanCount: this.scanData.length,
						submitTime: new Date().toISOString(),
						OpSerialNumber:this.formData.opSerialNumber,
						type: 'RF交接',
					};

					// 调用提交API
					const result = await completeHandover(submitData);

					uni.showToast({
						title: '交接提交成功',
						icon: 'success'
					});

					// 提交成功后返回上一页
					setTimeout(() => {
						uni.navigateBack({
							delta: 1
						});
					}, 1500);

				} catch (error) {
					console.error('提交失败:', error);
					uni.showToast({
						title: error.message || '提交失败',
						icon: 'none'
					});
				} finally {
					this.loading = false;
					uni.hideLoading();
				}
			},
		}
	}
</script>


<style scoped>
	/* 第一行：单号和按钮容器 */
	.single-line-container {
		display: flex;
		justify-content: space-between;
		align-items: center;
		padding: 20rpx 30rpx;
		background-color: #fff;
		border-bottom: 1rpx solid #eee;
	}

	/* 订单信息部分 */
	.order-info {
		flex: 1;
		display: flex;
		flex-direction: column;
	}

	.order-info .label {
		font-size: 24rpx;
		color: #999;
		margin-bottom: 8rpx;
	}

	.order-info .value {
		font-size: 28rpx;
		color: #333;
		font-weight: 500;
	}

	/* 提交按钮 */
	.submit-btn {
		margin-left: 20rpx;
		white-space: nowrap;
	}

	/* 第二行：客户名称和仓库 */
	.double-line-container {
		display: flex;
		background-color: #fff;
		padding: 20rpx 30rpx;
		border-bottom: 1rpx solid #eee;
	}

	.double-line-item {
		flex: 1;
		display: flex;
		flex-direction: column;
	}

	.double-line-item .label {
		font-size: 24rpx;
		color: #999;
		margin-bottom: 8rpx;
	}

	.double-line-item .value {
		font-size: 28rpx;
		color: #333;
	}

	/* 扫描输入样式 */
	.cu-form-group {
		display: flex;
		align-items: center;
		padding: 20rpx 30rpx;
		background-color: #fff;
		border-bottom: 1rpx solid #eee;
	}

	.cu-form-group .title {
		min-width: 150rpx;
		font-size: 28rpx;
		color: #666;
	}

	.scan-table-container {
		margin: 20rpx;
		background-color: #fff;
		border-radius: 10rpx;
		overflow: hidden;
	}
</style>