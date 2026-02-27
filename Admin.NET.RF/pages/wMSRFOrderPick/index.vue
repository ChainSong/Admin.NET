<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF拣货</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<!-- 查询区域 -->
			<view class="cu-form-group bg-white">
				<input placeholder="请输入拣货任务号" v-model="form.pickTaskNumber" style="width: 65%;"
					name="input"></input>
				<button class="cu-btn bg-blue shadow-blur round" @tap="getOrderList()">查询</button>
			</view>

			<!-- 任务列表 -->
			<view v-if="this.list.length>0">
				<view class="cu-list menu-avatar">
					<view v-for="(item, index)  in this.list" :key="index" class="cu-item">
						<view class="cu-avatar round lg" :class="getStatusClass(item.pickStatus)">
							<text class="text-white">{{item.pickStatus}}</text>
						</view>
						<view class="content">
							<view class="text-grey text-lg text-bold">{{item.pickTaskNumber}}</view>
							<view class="text-gray text-sm">
				 			<text class="text-blue">订单号: </text>{{item.orderNumber}}
				 			</view>
							<view class="text-gray text-sm">
				 			<text class="text-orange">外部订单: </text>{{item.externOrderNumber}}
				 			</view>
							<view class="text-gray text-sm">
				 			<text class="text-grey">仓库: </text>{{item.warehouseName}}
				 			</view>
							<view class="text-gray text-sm">
				 			<text class="text-grey">客户: </text>{{item.customerName}}
				 			</view>
							<view class="text-gray text-sm flex">
								<view class="text-cut">
									<text class="text-blue">状态: </text>{{getStatusText(item.pickStatus)}}
				 				</view>
				 			</view>
						</view>
						<view class="action">
							 <button class="cu-btn bg-blue shadow-blur round" @tap="goCollect(item)">拣货</button>
						</view>
					</view>
				</view>
			</view>
			<view v-else class="cu-list grid col-1 my">
				<view class="cu-item">
					<text class="lg text-gray cuIcon-camerarotate"></text>
					<text>暂无数据</text>
				</view>
			</view>
		</you-scroll>

	</view>
</template>

<script>
	import {
		pageWMSRFOrderPickApi
	} from "@/services/wMSRFOrderPick/wMSRFOrderPick";
	import youScroll from '@/components/you-scroll';
	export default {
		name: "WMSRFOrderPick",
		components: {
			youScroll
		},
		data() {
			return {
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar,
				loadingType: 'more',
				orderList: [],
				gridCol: 3,
				gridBorder: false,
				menuColor: 'blue',
				form: {
					pickTaskNumber: '',
				},
				list: [],
			};
		},
		created() {
			this.getOrderList();
		},
		filters: {
			carNumber(val) {
				return val ? val.slice(0, 1) : '';
			}
		},
		methods: {
			onPullDown(done) { // 下拉刷新
				this.menuList.length = 0;
				// this.getMenuList();
				done(); // 完成刷新
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
			// 获取状态对应的样式类
			getStatusClass(status) {
				const classMap = {
					1: 'bg-grey',
					2: 'bg-orange',
					3: 'bg-green',
					4: 'bg-blue'
				};
				return classMap[status] || 'bg-grey';
			},
			async goCollect (row) {
				uni.navigateTo({
					url: '/pages/wMSRFOrderPick/component/editDialog?pickTaskNumber='
					+row.pickTaskNumber+"&id="
					+row.id
				});
			},
			//获取订单列表
			async getOrderList() {
				var that = this;
				uni.showLoading({
					title: '加载中...'
				});

				try {
					await pageWMSRFOrderPickApi(this.form).then((res) => {
						console.log(res)
						this.list = res.data.result.items ?? [];
						uni.showToast({
							title: '加载完成！',
							icon: 'none'
						});
					});

				} finally {
					uni.hideLoading();
				}
			},


		}
	}
</script>
<style scoped>
	.cu-item {
		height: auto !important;
		min-height: 120px;
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

	/* 状态背景色 */
	.bg-grey {
		background-color: #8799a3;
	}

	.bg-orange {
		background-color: #f37b1d;
	}

	.bg-green {
		background-color: #39b54a;
	}

	.bg-blue {
		background-color: #0081ff;
	}

	.text-lg {
		font-size: 32upx;
	}

	.text-bold {
		font-weight: bold;
	}
</style>