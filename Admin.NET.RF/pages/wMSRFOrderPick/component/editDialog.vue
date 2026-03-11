<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">拣货:{{form.pickTaskNumber}}</block>
		</cu-custom>
		<view class="bluetooth-bar" v-if="printerConnected">
			<view class="bluetooth-info">
				<text class="cuIcon-bluetooth text-blue"></text>
				<text class="text-sm">{{printerDeviceName || '已连接'}}</text>
			</view>
			<view class="bluetooth-actions">
				<button class="cu-btn round bg-red sm" @tap="disconnectPrinter">断开</button>
			</view>
		</view>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<!-- 拣货信息 -->
			<view class="cu-card case">
				<view class="cu-item shadow">
					<view class="title">
						<view class="text-cut">任务信息</view>
					</view>
					<view class="content">
						<view class="desc">
							<text class="text-grey">拣货任务号: {{form.pickTaskNumber}}</text>
						</view>
						<!-- 拣货结果 -->
						<view v-if="this.list.length>0">
							<view class="cu-bar bg-white solid-bottom">
								<view class="action">
									<text class="cuIcon-title text-blue"></text>拣货明细
								</view>
							</view>
							<view class="cu-list menu-avatar">
								<view v-for="(item, index)  in this.list" :key="index"
									class="cu-item"
									:class="{'completed-item': item.order === 99}">
									<view class="cu-avatar round lg" :class="getPickStatusClass(item)">
										{{item.pickQty}}/{{item.qty}}
									</view>
									<view class="content">
										<view class="text-grey">
											<text class="text-bold">SKU:</text>{{item.sku}}
											<text class="text-sm text-grey" v-if="item.goodsName">({{item.goodsName}})</text>
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
					<button class="cu-btn bg-green lg round" @tap="connectPrinter">连接打印机</button>
				</view>
			</view>
		</you-scroll>
		
		<!-- 打印机设备选择弹窗 -->
		<view v-if="printerModalVisible" class="bluetooth-modal">
			<view class="bluetooth-modal-content">
				<view class="bluetooth-modal-header">
					<text>选择打印机</text>
					<text class="cuIcon-close text-grey" @tap="hidePrinterModal"></text>
				</view>
				<scroll-view scroll-y class="bluetooth-device-list">
					<view v-for="(device, index) in printerDevices" :key="index" class="bluetooth-device-item"
						@tap="selectPrinterDevice(device)">
						<text class="device-name">{{device.name || device.localName || '未知设备'}}</text>
						<text class="device-id">{{device.deviceId}}</text>
					</view>
					<view v-if="printerDevices.length === 0" class="no-device">
						<text>未发现打印机设备</text>
					</view>
				</scroll-view>
			</view>
		</view>
	</view>

</template>

<script>
	import {
		scanOrderPickTaskApi,
		scanPickApi,
		scanBoxNumberCompletePackageApi,
		getPickTaskDetailsByLocationApi
	} from "@/services/wMSRFOrderPick/wMSRFOrderPick";
	import youScroll from '@/components/you-scroll';
	import {
		playErrorSound,
		playSuccessSound
	} from "@/services/common/playaudio.js";
	import esc from '@/components/gprint/esc.js';
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
				// 蓝牙打印机相关
				printerConnected: false,
				printerDeviceName: '',
				printerModalVisible: false,
				printerDevices: [],
				printerDeviceId: null,
				printerServiceId: null,
				printerCharacteristicId: null
			};
		},
		created() {
			// 进入页面时直接获取按库位排序的拣货明细
			this.getPickDetailsByLocation();
			// 初始化蓝牙适配器
			this.initBluetooth();
		},
		onUnload() {
			// 页面卸载时关闭打印机连接
			this.closePrinter();
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
						that.list = res.data.result.data || [];
						console.log('拣货明细列表:', that.list);
						console.log('拣货明细数量:', that.list.length);
						that.list.forEach((item, index) => {
							console.log(`第${index}项: SKU=${item.sku}, 库位=${item.location}, 应拣=${item.qty}, 已拣=${item.pickQty}`);
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
						
						// 如果包装成功，获取箱号并打印
						if (res.data.result.data && res.data.result.data.boxNumber) {
							const boxNumber = res.data.result.data.boxNumber;
							console.log('箱号:', boxNumber);
							// 调用打印方法
							await this.printBoxLabel(boxNumber);
						}
						
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
			},
			
			/**
			 * 初始化蓝牙 - 参考文档流程：开启定位 → 打开蓝牙 → 获取蓝牙状态
			 */
			async initBluetooth() {
				try {
					// 1. 开启定位
					// await this.openLocation();

					// 2. 打开蓝牙适配器
					await uni.openBluetoothAdapter();
					console.log('蓝牙适配器初始化成功');

					// 3. 获取蓝牙状态
					const state = await uni.getBluetoothAdapterState();
					console.log('蓝牙状态:', state);

				} catch (error) {
					console.error('蓝牙初始化失败', error);
				}
			},

			/**
			 * 开启定位（蓝牙搜索需要）
			 */
			openLocation() {
				return new Promise((resolve) => {
					uni.getLocation({
						type: 'wgs84',
						success: (res) => {
							console.log('定位成功', res);
							resolve(res);
						},
						fail: (error) => {
							console.error('定位失败', error);
							// 定位失败不影响后续操作
							resolve(null);
						}
					});
				});
			},

			/**
			 * 连接打印机
			 */
			async connectPrinter() {
				// 显示打印机设备选择弹窗
				this.printerModalVisible = true;
				this.printerDevices = [];

				try {
					// 搜索可用蓝牙列表
					await uni.startBluetoothDevicesDiscovery({
						allowDuplicatesKey: true,
						success: () => {
							console.log('开始搜索打印机');
						},
						fail: (error) => {
							console.error('开始搜索失败', error);
							uni.showToast({
								title: '搜索失败',
								icon: 'none'
							});
						}
					});

					// 监听设备发现事件
					uni.onBluetoothDeviceFound((res) => {
						console.log('发现设备', res.devices);
						res.devices.forEach(device => {
							// 避免重复添加
							const exists = this.printerDevices.some(d => d.deviceId === device.deviceId);
							// 过滤打印机设备（通常名称包含print或PRINTER）
							const name = (device.name || device.localName || '').toLowerCase();
							if (!exists && (name.includes('print') || name.includes('printer') || name.includes('gp') || name.includes('gprinter'))) {
								this.printerDevices.push(device);
							}
						});
					});

					// 30秒后自动停止搜索
					setTimeout(() => {
						if (this.printerModalVisible) {
							uni.stopBluetoothDevicesDiscovery();
						}
					}, 30000);

				} catch (error) {
					console.error('开始搜索异常', error);
					uni.showToast({
						title: '请开启蓝牙',
						icon: 'none'
					});
				}
			},

			/**
			 * 隐藏打印机设备选择弹窗
			 */
			async hidePrinterModal() {
				this.printerModalVisible = false;
				try {
					await uni.stopBluetoothDevicesDiscovery();
				} catch (error) {
					console.error('停止搜索失败', error);
				}
			},

			/**
			 * 选择打印机设备并连接
			 */
			async selectPrinterDevice(device) {
				uni.showLoading({
					title: '连接中...'
				});

				try {
					// 连接打印机设备
					await uni.createBLEConnection({
						deviceId: device.deviceId
					});
					this.printerDeviceId = device.deviceId;
					this.printerDeviceName = device.name || device.localName || '打印机';
					console.log('打印机连接成功', device.deviceId);

					// 停止搜索
					await uni.stopBluetoothDevicesDiscovery();

					// 获取打印机服务
					const servicesRes = await uni.getBLEDeviceServices({
						deviceId: device.deviceId
					});
					const services = servicesRes.services;
					console.log('打印机服务列表', services);

					if (services && services.length > 0) {
						// 遍历所有服务找到支持write的特征值
						for (const service of services) {
							const charRes = await uni.getBLEDeviceCharacteristics({
								deviceId: device.deviceId,
								serviceId: service.uuid
							});
							const characteristics = charRes.characteristics;
							console.log('特征值列表', characteristics);

							// 找到支持write的特征值用于发送打印指令
							const writeCharacteristic = characteristics.find(c => c.properties.write);
							if (writeCharacteristic) {
								this.printerServiceId = service.uuid;
								this.printerCharacteristicId = writeCharacteristic.uuid;
								console.log('找到写特征值', writeCharacteristic.uuid);

								this.printerConnected = true;
								this.printerModalVisible = false;

								uni.hideLoading();
								uni.showToast({
									title: '打印机连接成功',
									icon: 'success'
								});
								return;
							}
						}
					}

					throw new Error('未找到可用的服务');

				} catch (error) {
					uni.hideLoading();
					uni.showToast({
						title: '连接失败',
						icon: 'none'
					});
					console.error('打印机连接错误', error);
				}
			},

			/**
			 * 打印箱号标签
			 */
			async printBoxLabel(boxNumber) {
				if (!this.printerConnected) {
					console.log('打印机未连接，跳过打印');
					return;
				}

				if (!boxNumber) {
					console.log('箱号为空，跳过打印');
					return;
				}

				console.log('开始打印箱号:', boxNumber);

				try {
					// 创建ESC打印指令
					const command = esc.createNew();
					command.init();
					
					// 设置标题
					command.setAlign(1); // 居中
					command.setSize(2, 2); // 放大字体
					command.setBold(true);
					command.setText("包装标签\n");
					command.setPrint();
					
					// 设置箱号
					command.setAlign(1);
					command.setSize(3, 3); // 更大字体
					command.setBold(true);
					command.setText(`${boxNumber}\n`);
					command.setPrint();
					
					// 设置拣货任务号
					command.setAlign(0); // 左对齐
					command.setSize(1, 1);
					command.setBold(false);
					command.setText(`任务号: ${this.form.pickTaskNumber}\n`);
					command.setPrint();
					
					// 设置打印时间
					command.setAlign(1);
					command.setText(`时间: ${new Date().toLocaleString()}\n`);
					command.setPrint();
					
					command.setPrintAndFeedLine(3); // 走纸3行

					// 获取打印数据
					const printData = command.getData();

					// 发送十进制数据到打印机 - 参考文档：uni.writeBLECharacteristicValue
					// 安卓底层限制每次只能发送20字节，需要分包发送
					const maxPacketSize = 20;
					for (let i = 0; i < printData.length; i += maxPacketSize) {
						const chunk = printData.slice(i, i + maxPacketSize);
						await uni.writeBLECharacteristicValue({
							deviceId: this.printerDeviceId,
							serviceId: this.printerServiceId,
							characteristicId: this.printerCharacteristicId,
							value: chunk,
							success: () => {
								console.log('发送成功', i, 'to', i + maxPacketSize);
							},
							fail: (error) => {
								console.error('发送失败', error);
							}
						});

						// 延迟避免发送过快
						await new Promise(resolve => setTimeout(resolve, 20));
					}

					console.log('打印成功');
					uni.showToast({
						title: '打印成功',
						icon: 'success'
					});

				} catch (error) {
					console.error('打印错误', error);
					uni.showToast({
						title: '打印失败',
						icon: 'none'
					});
				}
			},

			/**
			 * 断开打印机连接
			 */
			async disconnectPrinter() {
				if (this.printerDeviceId) {
					try {
						await uni.closeBLEConnection({
							deviceId: this.printerDeviceId
						});
					} catch (error) {
						console.error('断开连接失败', error);
					}
				}
				this.printerConnected = false;
				this.printerDeviceName = '';
				this.printerDeviceId = null;
				this.printerServiceId = null;
				this.printerCharacteristicId = null;

				uni.showToast({
					title: '已断开连接',
					icon: 'none'
				});
			},

			/**
			 * 关闭打印机
			 */
			async closePrinter() {
				try {
					if (this.printerDeviceId) {
						await uni.closeBLEConnection({
							deviceId: this.printerDeviceId
						});
					}
				} catch (error) {
					console.error('关闭打印机失败', error);
				}
				this.printerConnected = false;
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
