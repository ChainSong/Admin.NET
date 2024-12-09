<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF拣货</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input placeholder="请输入拣货任务号" v-model="form.pickTaskNumber" style="width: 100%;"
					name="input"></input>
				<button class="cu-btn bg-blue shadow-blur round" @tap="getOrderList()">查询</button>
			</view>
			<view v-if="this.list.length>0">
				<view class="cu-list menu-avatar">
					<view v-for="(item, index)  in this.list" :key="index" class="cu-item">
						<!-- <view class="cu-avatar round lg"
				 			style="background-image:url(https://ossweb-img.qq.com/images/lol/web201310/skin/big10001.jpg);">
				 		</view> -->
						<view class="content">
							<view class="text-grey">{{item.pickTaskNumber}}</view>
							<view class="text-gray text-sm flex">
				 				<view class="text-cut">
				 					<text class="cuIcon-infofill text-red  margin-right-xs">{{item.externOrderNumber}}</text>
				 				</view>
				 			</view>
							
						</view>
						<view class="action">
							 <button class="cu-btn bg-blue shadow-blur round" style="width: 60px;" @tap="goCollect(item)">拣货</button>
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
				form: {},
				list: {},
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
</style>