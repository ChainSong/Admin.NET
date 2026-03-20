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
						<view class="text-cut">任务信息
							<button class="cu-btn line-red sm margin-left-sm" @tap="handleClearPickCache">重新拣货</button>
							<button class="cu-btn line-grey sm margin-left-sm" @tap="createL">重新打印</button>
							<button class="cu-btn line-grey sm margin-left-sm" @tap="createLTest">打印测试页面</button>
						</view>

					</view>
					<view class="content">
						<view class="desc">
							<text class="text-grey">拣货任务号: {{form.pickTaskNumber}}</text>
						</view>
						<view class="text-gray text-sm flex">
							<view class="text-cut">
								<text class="text-blue">总数:</text> {{pickedTotal}}
							</view>
							<view class="text-cut">
								<text class="text-green">已拣:</text> {{pickQty}}
							</view>
							<view class="text-cut">
								<text class="text-red">待包装:</text> {{unSubmitTotal}}
							</view>
						</view>
						<!-- 拣货结果 -->
						<view v-if="this.list.length>0">
							<view class="cu-bar bg-white solid-bottom">
								<view class="action">
									<text class="cuIcon-title text-blue"></text>拣货明细
								</view>

							</view>
							<view class="cu-list menu-avatar">
								<view v-for="(item, index)  in this.list" :key="index" class="cu-item"
									:class="{'completed-item': item.order === 99}">
									<view class="cu-avatar round lg" :class="getPickStatusClass(item)">
										{{item.pickQty}}/{{item.qty}}
									</view>
									<view class="content">
										<view class="text-grey">
											<text class="text-bold">SKU:</text>{{item.sku}}
											<text class="text-sm text-grey"
												v-if="item.goodsName">({{item.goodsName}})</text>
										</view>
										<view class="text-grey">

											<text class="text-orange">库位:</text>{{item.location}}
											<text v-if="item.area"> | 区域:{{item.area}}</text>
										</view>
										<view class="text-grey" v-if="item.batchCode">
											<text>批次:</text>{{item.batchCode}}
										</view>
										<view class="text-gray text-sm flex">
											<view class="text-cut">
												<text class="text-blue">应拣:</text> {{item.qty}}
											</view>
											<view class="text-cut">
												<text class="text-green">已拣:</text> {{item.pickQty}}
											</view>
											<view class="text-cut" v-if="item.order === 99">
												<text class="text-cyan">待拣:</text> 0
											</view>
											<view class="text-cut" v-else>
												<text class="text-red">待拣:</text> {{item.qty - item.pickQty}}
											</view>
										</view>
										<view class="text-sm text-center mt-5" v-if="item.order === 99">
											<text class="cuIcon-roundcheck text-green"></text>
											<text class="text-green text-bold">已完成</text>
										</view>
									</view>
								</view>
							</view>
						</view>
						<!-- <view class="desc" >
							<text class="text-orange">推荐库位: </text>
							<text class="text-blue text-bold">{{form.location}}</text>
						</view>
						<view class="desc">
							<text class="text-orange">推荐SKU: </text>
							<text class="text-blue text-bold">{{form.sku}}</text>
						</view> -->
					</view>
				</view>
			</view>

			<!-- 扫描区域 -->
			<view class="cu-form-group ">

				<view class="title">库位</view>
				<input disabled v-model="form.location" placeholder="库位" />
			</view>
			<view class="cu-form-group ">
				<view class="title">扫描</view>
				<input :adjust-position="false" confirm-type="search" id="scanInput" :focus="focusflag"
					v-model="form.scanInput" v-focus="input" v-select="input" ref="input" name="input"
					@confirm="scanAcquisition()" clearable="" placeholder="请扫描条码/SKU" selection-start="0"
					:selection-end="selectendlength" />
			</view>



			<!-- 包装按钮区域 -->
			<view class="padding" v-if="showPackageBtn">
				<view class="cu-bar bg-white solid-bottom">
					<view class="action">
						<text class="cuIcon-title text-orange"></text>包装操作
					</view>
				</view>
				<!-- <view class="cu-form-group">
					<view class="title">扫描箱号</view>
					<input v-model="packageForm.boxNumber" placeholder="请扫描箱号" @confirm="handleScanBoxNumber"></input>
				</view> -->
				<view class="padding flex flex-direction">
					<button class="cu-btn bg-blue lg round" @tap="handleScanBoxNumber">完成包装</button>
					<button class="cu-btn bg-green lg round margin-top-sm" @tap="openSNDialog">扫描SN</button>
					<!-- <button class="cu-btn bg-green lg round" @tap="connectPrinter">连接打印机</button> -->
				</view>
			</view>
		</you-scroll>

		<!-- 扫描SN弹窗 -->
		<view class="cu-modal" :class="snModalVisible ? 'show' : ''" @tap="closeSNDialog">
			<view class="cu-dialog" @tap.stop>
				<view class="cu-bar bg-white justify-end">
					<view class="content">扫描SN</view>
					<view class="action" @tap="closeSNDialog">
						<text class="cuIcon-close text-red"></text>
					</view>
				</view>
				<view class="padding-xl">
					<view class="cu-form-group">
						<view class="title">拣货单号</view>
						<input disabled v-model="snForm.pickTaskNumber" placeholder="拣货单号" />
					</view>
					<view class="cu-form-group">
						<view class="title">箱号</view>
						<input :focus="snBoxInputFocus" v-model="snForm.boxNumber" placeholder="请扫描箱号"
							@confirm="focusSKUInput" />
					</view>
					<view class="cu-form-group">
						<view class="title">SKU</view>
						<input :focus="snSkuInputFocus" v-model="snForm.sku" placeholder="请扫描SKU"
							@confirm="focusSNInput" />
					</view>
					<view class="cu-form-group">
						<view class="title">条码</view>
						<input :focus="snInputFocus" v-model="snForm.snCode" placeholder="请扫描条码"
							@confirm="handleScanSN" />
					</view>
				</view>
				<view class="cu-bar bg-white">
					<view class="action margin-0 flex-sub text-center" @tap="closeSNDialog">
						取消
					</view>
					<view class="action margin-0 flex-sub text-blue text-center" @tap="clearSNForm">
						清空
					</view>
				</view>
			</view>
		</view>

	</view>

</template>

<script>
	import {
		pageWMSRFOrderPickApi,
		scanOrderPickTaskApi,
		scanPickApi,
		scanBoxNumberCompletePackageApi,
		getPickTaskDetailsByLocationApi,
		clearPickCacheApi,
		scanSNPackageApi
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
				printPackageNumber: "",
				printSerialNumber: "",
				pickQty: 0,
				unSubmitTotal: 0,
				pickedTotal: 0,
				// 蓝牙打印机相关
				printerConnected: false,
				printerDeviceName: '',
				printerModalVisible: false,
				printerDevices: [],
				printerDeviceId: null,
				printerServiceId: null,
				input: 0,
				printerCharacteristicId: null,
				// 标记 UniApp 桥接是否已准备就绪
				bridgeReady: false,

				// 标签打印参数（可根据实际需求修改或通过 props 传入）
				labelWidth: 50, // 标签宽度（mm）
				labelHeight: 30, // 标签高度（mm）
				gap: 12, // 标签纸间距
				// text 参数暂未使用，保留注释说明
				// 扫描SN弹窗相关
				snModalVisible: false,
				snInputFocus: false,
				snSkuInputFocus: false,
				snBoxInputFocus: false,
				snForm: {
					pickTaskNumber: '',
					sku: '',
					snCode: '',
					boxNumber: ''
				}
			};
		},
		created() {
			// 进入页面时直接获取按库位排序的拣货明细
			this.getPickDetailsByLocation();
			// 初始化蓝牙适配器
			// this.initBluetooth();
		},
		onUnload() {
			// 页面卸载时关闭打印机连接
			// this.closePrinter();
			this.loadScript({
				src: '/static/js/uni.webview.1.5.2.js',
				fun: window.h5uni,
				eventListener: 'UniAppJSBridgeReady'
			}, () => {});

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
			loadScript(options, callback) {
				if (!options.src) {
					return;
				}
				// 判断引入的js对象是否存在  
				if (typeof options.fun == 'undefined') {
					console.log('自动引入');
					var node = document.createElement('script');
					node.src = options.src;
					node.addEventListener('load', callback, false);
					document.head.appendChild(node);
				} else {
					console.log('直接渲染');
					console.log(options.eventListener || 'load');
					document.addEventListener(options.eventListener || 'load', callback, false);
				}
			},

			lpnSearchSet() {
				this.focusflag = false;
				this.$nextTick(() => {
					this.focusflag = true;
				})
				this.selectendlength = this.form.scanInput.length;
			},
			// 获取拣货状态对应的样式类
			getPickStatusClass(item) {
				if (item.order === 99) {
					return 'bg-green'; // 已完成
				} else if (item.pickQty > 0) {
					return 'bg-orange'; // 部分拣货
				} else {
					return 'bg-blue'; // 未拣货
				}
			},
			// 获取按库位排序的拣货明细（进入拣货页面时自动调用）
			async getPickDetailsByLocation() {
				this.lpnSearchSet();
				let that = this;
				uni.showLoading({
					title: '加载中...'
				});

				try {
					let res = await getPickTaskDetailsByLocationApi({
						pickTaskNumber: this.form.pickTaskNumber
					});
					console.log('按库位排序的拣货明细:', res.data.result);

					if (res.data && res.data.result && res.data.result.code == "1") {
						that.pickQty = res.data.result.data.pickQty;
						that.unSubmitTotal = res.data.result.data.unSubmitTotal;
						that.pickedTotal = res.data.result.data.pickedTotal;
						that.list = res.data.result.data.wmsrfPickTaskDetailOutputs || [];
						console.log('拣货明细列表:', that.list);
						console.log('拣货明细数量:', that.list.length);
						that.list.forEach((item, index) => {
							console.log(
								`第${index}项: SKU=${item.sku}, 库位=${item.location}, 应拣=${item.qty}, 已拣=${item.pickQty}`
							);
						});

						// 获取第一个未完成商品的库位作为推荐库位
						const firstPendingItem = that.list.find(item => item.order !== 99);
						if (firstPendingItem) {
							that.form.location = firstPendingItem.location;
							console.log('推荐库位:', that.form.location);
						} else if (that.list.length > 0) {
							// 如果都已拣完，显示第一个的库位
							that.form.location = that.list[0].location;
							console.log('所有商品已完成，显示第一个库位:', that.form.location);
						}
						// 检查是否显示包装按钮
						that.checkPackageButton();
					} else {
						uni.showToast({
							title: res.data?.result?.msg || "获取拣货明细失败",
							icon: 'none'
						});
					}
				} catch (error) {
					console.error('获取拣货明细失败:', error);
					uni.showToast({
						title: "获取拣货明细失败",
						icon: 'none'
					});
				} finally {
					uni.hideLoading();
				}
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
				console.log('扫描拣货结果:', res.data.result);

				if (res.data.result.code == "1") {

					if (res.data.result.msg == "Location") {
						that.form.location = that.form.scanInput;
						that.form.pickTaskNumber = res.data.result.data[0].pickTaskNumber;
						that.form.location = res.data.result.data[0].location;
						// that.form.sku = res.data.result.data[0].sku;
						console.log('扫描库位:', that.form)
					} else if (res.data.result.msg == "SKU") {
						that.form.location = res.data.result.data[0].location;
						console.log('扫描SKU成功，更新列表');
					}

					// 扫描成功后，重新获取按库位排序的拣货明细（保证合并展示）
					await that.getPickDetailsByLocation();

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
			// 当前箱重新拣货 - 清理Redis缓存
			async handleClearPickCache() {
				// 弹出确认对话框
				uni.showModal({
					title: '确认重新拣货',
					content: '确定要清除当前箱的拣货记录吗？清除后需要重新扫描拣货。',
					confirmText: '确定',
					confirmColor: '#e54d42',
					cancelText: '取消',
					success: async (res) => {
						if (res.confirm) {
							// 用户点击确定，执行清理操作
							uni.showLoading({
								title: '处理中...'
							});

							try {
								let result = await clearPickCacheApi({
									pickTaskNumber: this.form.pickTaskNumber
								});

								if (result.data && result.data.result && result.data.result.code == "1") {
									uni.showToast({
										title: "清除成功，请重新拣货",
										icon: 'success'
									});
									playSuccessSound();

									// 重新获取拣货明细
									await this.getPickDetailsByLocation();

									// 隐藏包装按钮
									this.showPackageBtn = false;
									this.form.pickStatus = 2; // 拣货中
								} else {
									uni.showToast({
										title: result.data?.result?.msg || "清除失败",
										icon: 'none'
									});
									playErrorSound();
								}
							} catch (error) {
								console.error('清除缓存失败:', error);
								uni.showToast({
									title: "清除失败，请重试",
									icon: 'none'
								});
								playErrorSound();
							} finally {
								uni.hideLoading();
							}
						}
					}
				});
			},
			// 扫描箱号完成包装
			async handleScanBoxNumber() {
				// if (!this.packageForm.boxNumber) {
				// 	uni.showToast({
				// 		title: "请扫描或输入箱号",
				// 		icon: 'none'
				// 	});
				// 	playErrorSound();
				// 	return;
				// }

				if (this.form.pickStatus !== 3) {
					uni.showToast({
						title: "拣货任务未完成，无法包装",
						icon: 'none'
					});
					playErrorSound();
					return;
				}

				// 弹出确认对话框
				uni.showModal({
					title: '确认包装',
					content: '确定要完成包装操作吗？',
					confirmText: '确定',
					cancelText: '取消',
					success: async (res) => {
						if (res.confirm) {
							// 用户点击确定，执行包装操作
							this.executePackage();
						}
					}
				});
			},
			// 执行包装操作
			async executePackage() {

				// 调用打印方法
				// this.printPackageNumber = "200848677507392";
				// this.printSerialNumber = "232322_01";
				// uni.showToast({
				// 	title: "开始打印",
				// 	icon: 'none'
				// });
				// this.createL();
				// return;
				uni.showLoading({
					title: '处理中...'
				});

				try {
					this.printPackageNumber = "";
					this.printSerialNumber = "";
					let res = await scanBoxNumberCompletePackageApi({
						pickTaskNumber: this.packageForm.pickTaskNumber,
						boxNumber: this.packageForm.boxNumber
					});

					console.log('包装结果:', res);

					if (res.data && res.data.result.code == 1) {
						uni.showToast({
							title: res.data.result.msg || "包装完成",
							icon: 'success'
						});
						playSuccessSound();
						// 清空扫描框
						this.form.scanInput = "";
						this.lpnSearchSet();
						// 清空箱号输入
						// this.packageForm.boxNumber = "";
						// 更新状态
						this.form.pickStatus = 4;
						this.showPackageBtn = false;
						// 如果包装成功，获取箱号并打印
						if (res.data.result.data && res.data.result.code == 1) {
							const boxNumber = res.data.result.data.packageNumber;
							const serialNumber = res.data.result.data.str4 + "_" + res.data.result.data.serialNumber;
							console.log('箱号:', boxNumber);
							// 调用打印方法
							this.printPackageNumber = boxNumber;
							this.printSerialNumber = serialNumber;
							uni.showToast({
								title: "开始打印",
								icon: 'none'
							});
							this.createL();
							// await this.printBoxLabel(boxNumber);
						}

						// 延迟返回
						// setTimeout(() => {
						// 	uni.navigateBack();
						// }, 1500);
					} else {
						uni.showToast({
							title: res.data?.result?.msg || "包装失败",
							icon: 'none'
						});
						this.lpnSearchSet();
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
			},
			createLTest() {

				console.log(this.printSerialNumber);
				console.log(this.printPackageNumber);
				h5uni.postMessage({
					data: {
						action: 'createLabel',
						data: {
							lable: {
								w: 60,
								h: 80,
								g: 2
							},
							content: [{
									"t": "qr",
									"x": 120,
									"y": 80,
									"l": "L",
									"w": 10,
									"m": "A",
									"c": "123456789"
								},
								{
									"t": "text",
									"x": 110,
									"y": 340,
									"x_m": 2,
									"y_m": 2,
									"c": "123456789"
								},
								{
									"t": "bar",
									"x": 90,
									"y": 430,
									"ct": "128",
									"h": 64,
									"r": 1,
									"n": 2,
									"w": 4,
									"c": "123456789"
								}
							]
						}
					}
				});
			},
			createL() {
				console.log(this.printSerialNumber);
				console.log(this.printPackageNumber);
				h5uni.postMessage({
					data: {
						action: 'createLabel',
						data: {
							lable: {
								w: 60,
								h: 80,
								g: 2
							},
							content: [{
									"t": "qr",
									"x": 120,
									"y": 80,
									"l": "L",
									"w": 10,
									"m": "A",
									"c": this.printSerialNumber
								},
								{
									"t": "text",
									"x": 110,
									"y": 340,
									"x_m": 2,
									"y_m": 2,
									"c": this.printSerialNumber
								},
								{
									"t": "bar",
									"x": 90,
									"y": 430,
									"ct": "128",
									"h": 64,
									"r": 1,
									"n": 2,
									"w": 4,
									"c": this.printPackageNumber
								}
							]
						}
					}
				});
			},
			// 打开扫描SN弹窗
			openSNDialog() {
				this.snForm.pickTaskNumber = this.form.pickTaskNumber;
				this.snForm.sku = '';
				this.snForm.snCode = '';
				this.snModalVisible = true;
				this.$nextTick(() => {
					this.snInputFocus = false;
				});
			},
			// 关闭扫描SN弹窗
			closeSNDialog() {
				this.snModalVisible = false;
				this.snInputFocus = false;
			},

			// 焦点移到SKU输入框
			focusSKUInput() {
				this.snSkuInputFocus = true;
			},
			// 焦点移到SN输入框
			focusSNInput() {
				this.snSkuInputFocus = false;
				this.snInputFocus = true;
			},
			// 清空SN表单
			clearSNForm() {
				this.snForm.boxNumber = '';
				this.snForm.sku = '';
				this.snForm.snCode = '';
				// 先重置所有焦点
				this.snBoxInputFocus = false;
				this.snSkuInputFocus = false;
				this.snInputFocus = false;
				// 再设置箱号焦点
				this.$nextTick(() => {
					this.snBoxInputFocus = true;
				});
			},
			// 扫描SN
			async handleScanSN() {
				if (!this.snForm.sku) {
					uni.showToast({
						title: "请输入或扫描SKU",
						icon: 'none'
					});
					playErrorSound();
					return;
				}
				if (!this.snForm.snCode) {
					uni.showToast({
						title: "请输入或扫描条码",
						icon: 'none'
					});
					playErrorSound();
					return;
				}

				uni.showLoading({
					title: '处理中...'
				});

				try {
					let res = await scanSNPackageApi({
						pickTaskNumber: this.snForm.pickTaskNumber,
						sku: this.snForm.sku,
						snCode: this.snForm.snCode
					});

					console.log('扫描SN结果:', res);

					if (res.data && res.data.result && res.data.result.code == 1) {
						uni.showToast({
							title: res.data.result.msg || "扫描成功",
							icon: 'success'
						});
						playSuccessSound();
						// 清空条码输入，保留SKU方便连续扫描
						this.snForm.snCode = '';
						this.$nextTick(() => {
							this.snInputFocus = true;
						});
					} else {
						uni.showToast({
							title: res.data?.result?.msg || "扫描失败",
							icon: 'none'
						});
						playErrorSound();
					}
				} catch (error) {
					console.error('扫描SN失败:', error);
					uni.showToast({
						title: "扫描失败，请重试",
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
		height: auto !important;
		min-height: 100px;
		padding: 20upx 0 !important;
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

	.bg-orange {
		background-color: #ff9800;
		color: white;
	}

	.bg-green {
		background-color: #4cd964;
		color: white;
	}

	.text-red {
		color: #dd524d;
	}

	.text-green {
		color: #4cd964;
	}

	.text-cyan {
		color: #0dc9d8;
	}

	.mt-5 {
		margin-top: 10upx;
	}

	.text-center {
		text-align: center;
	}

	.completed-item {
		opacity: 0.6;
		background-color: #f5f5f5;
	}

	/* 蓝牙状态栏 */
	.bluetooth-bar {
		background-color: #e0e0e0;
		padding: 10px 15px;
		display: flex;
		justify-content: space-between;
		align-items: center;
	}

	.bluetooth-info {
		display: flex;
		align-items: center;
		gap: 5px;
	}

	/* 蓝牙设备选择弹窗 */
	.bluetooth-modal {
		position: fixed;
		top: 0;
		left: 0;
		right: 0;
		bottom: 0;
		background-color: rgba(0, 0, 0, 0.5);
		display: flex;
		justify-content: center;
		align-items: center;
		z-index: 9999;
	}

	.bluetooth-modal-content {
		background-color: white;
		width: 80%;
		max-width: 400px;
		border-radius: 10px;
		overflow: hidden;
	}

	.bluetooth-modal-header {
		display: flex;
		justify-content: space-between;
		align-items: center;
		padding: 15px;
		border-bottom: 1px solid #e0e0e0;
		font-size: 16px;
		font-weight: bold;
	}

	.bluetooth-device-list {
		max-height: 400px;
		padding: 10px 0;
	}

	.bluetooth-device-item {
		padding: 15px;
		border-bottom: 1px solid #f0f0f0;
		display: flex;
		flex-direction: column;
		gap: 5px;
	}

	.bluetooth-device-item:active {
		background-color: #f5f5f5;
	}

	.device-name {
		font-size: 14px;
		color: #333;
	}

	.device-id {
		font-size: 12px;
		color: #999;
	}

	.no-device {
		padding: 40px;
		text-align: center;
		color: #999;
	}
</style>