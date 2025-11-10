<template>
	<view class="cu-modal" :class="show ? 'show' : ''" @tap="handleMaskClick">
		<view class="cu-dialog" @tap.stop>
			<view class="cu-bar bg-white justify-end">
				<view class="content">填写托盘信息</view>
				<view class="action" @tap="handleClose">
					<text class="cuIcon-close text-red"></text>
				</view>
			</view>
			<view class="padding-xl dialog-content">
				<!-- 长度 -->
				<view class="cu-form-group">
					<view class="title">长&nbsp;&nbsp;度</view>
					<input v-model="formData.length" placeholder="请输入长度(cm)" type="number"
						:focus="show && !formData.length" />
					<view class="unit">cm</view>
				</view>

				<!-- 宽度 -->
				<view class="cu-form-group">
					<view class="title">宽&nbsp;&nbsp;度</view>
					<input v-model="formData.width" placeholder="请输入宽度(cm)" type="number" />
					<view class="unit">cm</view>
				</view>

				<!-- 高度 -->
				<view class="cu-form-group">
					<view class="title">高&nbsp;&nbsp;度</view>
					<input v-model="formData.height" placeholder="请输入高度(cm)" type="number" />
					<view class="unit">cm</view>
				</view>

				<!-- 体积（用户填写） -->
				<view class="cu-form-group">
					<view class="title">体&nbsp;&nbsp;积</view>
					<input v-model="formData.volume" placeholder="请输入体积" type="number" />
					<view class="unit">cm³</view>
				</view>

				<!-- 重量 -->
				<view class="cu-form-group">
					<view class="title">重&nbsp;&nbsp;量</view>
					<input v-model="formData.weight" placeholder="请输入重量" type="number" />
					<view class="unit">kg</view>
				</view>
			</view>

			<view class="cu-bar bg-white justify-end">
				<view class="action">
					<button class="cu-btn line-green text-green" @tap="handleClose">取消</button>
					<button class="cu-btn bg-green margin-left" @tap="handleSubmit" :disabled="loading || !isFormValid">
						{{ loading ? '提交中...' : '确定' }}
					</button>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		name: 'PalletInfoModal',
		props: {
			show: {
				type: Boolean,
				default: false
			}
		},
		data() {
			return {
				loading: false,
				formData: {
					length: '',
					width: '',
					height: '',
					volume: '',
					weight: ''
				}
			};
		},
		computed: {
			// 表单验证
			isFormValid() {
				return this.formData.length &&
					this.formData.width &&
					this.formData.height &&
					this.formData.volume &&
					this.formData.weight;
			}
		},
		watch: {
			show(newVal) {
				if (newVal) {
					// 弹框显示时，重置表单并自动聚焦第一个输入框
					this.$nextTick(() => {
						setTimeout(() => {
							this.resetForm();
						}, 100);
					});
				} else {
					this.resetForm();
				}
			}
		},
		methods: {
			// 点击遮罩层关闭
			handleMaskClick() {
				this.handleClose();
			},

			// 验证表单
			validateForm() {
				const errors = [];

				if (!this.formData.length) {
					errors.push('长度');
				} else if (parseFloat(this.formData.length) <= 0) {
					errors.push('长度必须大于0');
				}

				if (!this.formData.width) {
					errors.push('宽度');
				} else if (parseFloat(this.formData.width) <= 0) {
					errors.push('宽度必须大于0');
				}

				if (!this.formData.height) {
					errors.push('高度');
				} else if (parseFloat(this.formData.height) <= 0) {
					errors.push('高度必须大于0');
				}

				if (!this.formData.volume) {
					errors.push('体积');
				} else if (parseFloat(this.formData.volume) <= 0) {
					errors.push('体积必须大于0');
				}

				if (!this.formData.weight) {
					errors.push('重量');
				} else if (parseFloat(this.formData.weight) <= 0) {
					errors.push('重量必须大于0');
				}

				return errors;
			},

			handleSubmit() {
				const errors = this.validateForm();
				if (errors.length > 0) {
					const errorMsg = errors.join('、');
					uni.showToast({
						title: `请填写${errorMsg}`,
						icon: 'none',
						duration: 3000
					});
					return;
				}

				this.loading = true;

				// 模拟提交过程

				// 触发 success 事件，并传递格式化后的数据
				this.$emit('success', {
					length: parseFloat(this.formData.length),
					width: parseFloat(this.formData.width),
					height: parseFloat(this.formData.height),
					volume: parseFloat(this.formData.volume),
					weight: parseFloat(this.formData.weight)
				});

				uni.showToast({
					title: '提交成功',
					icon: 'success'
				});

				this.handleClose();
			},

			handleClose() {
				this.$emit('update:show', false);
				this.$emit('close');
			},

			resetForm() {
				this.formData = {
					length: '',
					width: '',
					height: '',
					volume: '',
					weight: ''
				};
				this.loading = false;
			}
		}
	};
</script>

<style scoped>
	.cu-modal {
		z-index: 9999;
	}

	.cu-dialog {
		width: 90%;
		max-width: 600rpx;
		border-radius: 20rpx;
		overflow: hidden;
		background: #fff;
	}

	.dialog-content {
		max-height: 70vh;
		overflow-y: auto;
	}

	.cu-form-group {
		display: flex;
		align-items: center;
		padding: 30rpx 0;
		min-height: 80rpx;
	}

	.cu-form-group .title {
		min-width: 140rpx;
		font-size: 30rpx;
		color: #333;
		font-weight: 500;
	}

	input {
		flex: 1;
		padding: 20rpx 30rpx;
		border: 2rpx solid #e0e0e0;
		border-radius: 12rpx;
		font-size: 30rpx;
		background: #fff;
		margin: 0 20rpx;
		height: 80rpx;
		line-height: 80rpx;
	}

	input:focus {
		border-color: #007aff;
		background: #f8f9fa;
	}

	.unit {
		min-width: 120rpx;
		font-size: 28rpx;
		color: #666;
		text-align: center;
		font-weight: 500;
	}

	.padding-xl {
		padding: 40rpx;
	}

	/* 按钮样式调整 */
	.cu-bar .action {
		padding: 30rpx 40rpx;
	}

	.cu-btn {
		min-width: 160rpx;
		height: 80rpx;
		line-height: 80rpx;
		font-size: 32rpx;
	}

	.margin-left {
		margin-left: 30rpx;
	}

	/* 禁用按钮样式 */
	.cu-btn[disabled] {
		background-color: #cccccc !important;
		color: #666666 !important;
		opacity: 0.6;
	}
</style>