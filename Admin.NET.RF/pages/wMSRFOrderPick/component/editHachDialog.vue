<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">拣货:{{form.pickTaskNumber}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<!-- 拣货信息 -->
			<view class="cu-card case">
				<view class="cu-item shadow">
					<view class="title">
						<view class="text-cut">任务信息</view>
					</view>
					<view class="content">
						<!-- <view class="desc">
							<text class="text-grey">订单号: {{form.orderNumber}}</text>
						</view> -->
						<view class="desc">
							<text class="text-grey">拣货任务号: {{form.pickTaskNumber}}</text>
						</view>
					<!-- 	<view class="desc">
							<text class="text-grey">拣货状态: {{getStatusText(form.pickStatus)}}</text>
						</view> -->
					</view>
				</view>
			</view>

			<!-- 扫描区域 -->
			<view class="cu-form-group ">
				<view class="title">库位</view>
				<input disabled v-model="form.location" placeholder="库位"></input>
			</view>
			<view class="cu-form-group ">
				<view class="title">扫描</view>
				<input :adjust-position="false" confirm-type="search" id="scanInput" :focus="focusflag"
					v-model="form.scanInput" v-focus="input" v-select="input" ref="input" name="input"
					@confirm="scanAcquisition()" clearable="" placeholder="请扫描条码/SKU" selection-start="0"
					:selection-end="selectendlength"></input>
			</view>

			<!-- 拣货结果 -->
			<view v-if="this.list.length>0">
				<view class="cu-bar bg-white solid-bottom">
					<view class="action">
						<text class="cuIcon-title text-blue"></text>拣货明细
					</view>
				</view>
				<view class="cu-list menu-avatar">
					<view v-for="(item, index)  in this.list" :key="index" class="cu-item">
						<view class="cu-avatar round lg bg-blue">{{item.pickQty}}
						</view>
						<view class="content">
							<view class="text-grey">
								<text class="text-bold">SKU:</text>{{item.sku}}
							</view>
							<view class="text-grey">
								<text>库位:{{item.location}}</text>
								<text v-if="item.batchCode"> | 批次:{{item.batchCode}}</text>
							</view>
							<view class="text-gray text-sm flex">
								<view class="text-cut">
									<text class="text-blue">订单数量:</text> {{item.qty}}
								</view>
								<view class="text-cut">
									<text class="text-blue">已拣数量:</text> {{item.pickQty}}
								</view>
							</view>
						</view>
					</view>
				</view>
			</view>

			<!-- 包装按钮区域 -->
			<view class="padding" v-if="showPackageBtn">
				<view class="cu-bar bg-white solid-bottom">
					<view class="action">
						<text class="cuIcon-title text-orange"></text>包装操作
					</view>
				</view>
				<view class="cu-form-group">
					<view class="title">扫描箱号</view>
					<input v-model="packageForm.boxNumber" placeholder="请扫描箱号" @confirm="handleScanBoxNumber"></input>
				</view>
				<view class="padding flex flex-direction">
					<button class="cu-btn bg-blue lg round" @tap="handleScanBoxNumber">完成包装</button>
				</view>
			</view>
		</you-scroll>
	</view>

</template>

<script>
	import {
		pageWMSRFOrderPickApi,
		scanOrderPickTaskApi,
		scanPickApi,
		scanBoxNumberCompletePackageApi
	} from "@/services/wMSRFOrderPick/wMSRFOrderPick";
	import youScroll from '@/components/you-scroll';
	import {
		playErrorSound,
		playSuccessSound
	} from "@/services/common/playaudio.js";
	export default {
		name: "wMSShelveDetail",
		components: {
			youScroll
		},
		data() {
			return {
				selectendlength: 0,
				focusflag: true,
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar,
				loadingType: 'more',
				orderList: [],
				gridCol: 3,
				gridBorder: false,
				menuColor: 'blue',
				form: {
					sku: "",
					lot: "",
					id: "",
					expirationDate: "",
					externReceiptNumber: "",
					pickTaskNumber: "",
					orderNumber: "",
					sn: "",
					scanInput: "",
					location: "",
					pickStatus: 1,
				},
				list: [],
				packageForm: {
					boxNumber: "",
					pickTaskNumber: "",
				},
				id: "",
				data: [],
				visible: false,
				loading: false,
				showPackageBtn: false,
			};
		},
		created() {
			this.getOrderList();
		},
		filters: {
			// carNumber(val) {
			// 	return val ? val.slice(0, 1) : '';
			// }
		},
		onLoad(options) {
			console.log("options");
			console.log(options);
			this.form.pickTaskNumber = options.pickTaskNumber;
			this.form.id = options.id;
			this.packageForm.pickTaskNumber = options.pickTaskNumber;
		},
		methods: {
			lpnSearchSet() {
				this.focusflag = false;
				this.$nextTick(() => {
					this.focusflag = true;
				})
				this.selectendlength = this.form.scanInput.length;
			},
			// 获取状态文本
			getStatusText(status) {
				const statusMap = {
					1: '待拣货',
					2: '拣货中',
					3: '拣货完成',
					4: '包装完成'
				};
				return statusMap[status] || `状态${status}`;
			},
			async getOrderList() {
				this.lpnSearchSet();
				let that = this;
				let res = await scanPickApi(this.form);
				console.log(res.data.result.data)
				that.list = res.data.result.data || [];
				// 检查是否显示包装按钮
				that.checkPackageButton();
				if (res.data.result.code == "1") {
					uni.showToast({
						title: "操作成功",
						icon: 'success'
					});
					playSuccessSound();
				} else {
					uni.showToast({
						title: "操作失败:" + res.data.result.msg,
						icon: 'none'
					});
					playErrorSound();
				}

			},
			// 检查是否显示包装按钮
			checkPackageButton() {
				// 如果所有商品都已拣完，显示包装按钮
				if (this.list.length > 0) {
					this.showPackageBtn = true;
					// 更新表单状态
					this.form.pickStatus = 3;
					this.form.orderNumber = this.list[0].orderNumber || '';
				}
			},
			async scanAcquisition() {
				this.lpnSearchSet();
				let that = this;
				let res = await scanOrderPickTaskApi(this.form);
				console.log(res.data.result.data)

				if (res.data.result.code == "1") {

					if (res.data.result.msg == "Location") {
						that.form.location = that.form.scanInput;
						that.form.pickTaskNumber = res.data.result.data[0].pickTaskNumber;
						// that.form.orderNumber = res.data.result.data[0].orderNumber;
							console.log(that.form)
					} else if (res.data.result.msg == "SKU") {

					}
					that.list = res.data.result.data || [];
					// 检查是否显示包装按钮
					that.checkPackageButton();
					uni.showToast({
						title: "操作成功",
						icon: 'success'
					});
					playSuccessSound();
				} else {
					uni.showToast({
						title: "操作失败:" + res.data.result.msg,
						icon: 'none'
					});
					playErrorSound();
				}
			},
			// 扫描箱号完成包装
			async handleScanBoxNumber() {
				if (!this.packageForm.boxNumber) {
					uni.showToast({
						title: "请扫描或输入箱号",
						icon: 'none'
					});
					playErrorSound();
					return;
				}

				if (this.form.pickStatus !== 3) {
					uni.showToast({
						title: "拣货任务未完成，无法包装",
						icon: 'none'
					});
					playErrorSound();
					return;
				}

				uni.showLoading({
					title: '处理中...'
				});

				try {
					let res = await scanBoxNumberCompletePackageApi({
						pickTaskNumber: this.packageForm.pickTaskNumber,
						boxNumber: this.packageForm.boxNumber
					});

					console.log('包装结果:', res);

					if (res.data && res.data.result) {
						uni.showToast({
							title: res.data.result.msg || "包装完成",
							icon: 'success'
						});
						playSuccessSound();
						// 清空箱号输入
						this.packageForm.boxNumber = "";
						// 更新状态
						this.form.pickStatus = 4;
						this.showPackageBtn = false;
						// 延迟返回
						setTimeout(() => {
							uni.navigateBack();
						}, 1500);
					} else {
						uni.showToast({
							title: res.data?.result?.msg || "包装失败",
							icon: 'none'
						});
						playErrorSound();
					}
				} catch (error) {
					console.error('包装失败:', error);
					uni.showToast({
						title: "包装失败，请重试",
						icon: 'none'
					});
					playErrorSound();
				} finally {
					uni.hideLoading();
				}
			}
		}
	}
</script>
<style scoped>
	.cu-item {
		height: 72px !important;
	}

	.my>.cu-item {
		height: calc(100vh) !important;
		align-items: center;
	justify-content: center;
	}

	.cu-list.grid>.cu-item [class*=cuIcon],
	[class*=wlq] {
		font-size: 30px !important;
	}

	.cu-card {
		margin: 10upx;
	}

	.cu-card .cu-item {
		padding: 20upx;
		margin-bottom: 10upx;
		border-radius: 10upx;
	}

	.cu-card .title {
		font-size: 32upx;
		font-weight: bold;
		color: #333;
		margin-bottom: 10upx;
	}

	.cu-card .content .desc {
		padding: 8upx 0;
	}

	.text-bold {
		font-weight: bold;
	}

	.bg-blue {
		background-color: #007AFF;
		color: white;
	}
</style>