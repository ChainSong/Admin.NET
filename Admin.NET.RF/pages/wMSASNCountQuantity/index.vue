<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF点数</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input placeholder="请输入预入库单号" v-model="form.asnCountQuantityNumber" style="width: 100%;"
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
							<view class="text-grey">{{item.asnCountQuantityNumber}}</view>
							<view class="text-gray text-sm flex">
				 				<view class="text-cut">
				 					<text class="cuIcon-infofill text-red  margin-right-xs">{{item.externReceiptNumber}}</text>
				 					<!-- 我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。 -->
				 				</view>
				 			</view>
							
						</view>
						<view class="action">
							 <button class="cu-btn bg-blue shadow-blur round" style="width: 60px;" @tap="goCollect(item)">点数</button>
						</view>
						<view class="action">
							 <button class="cu-btn bg-blue shadow-blur round" style="width: 60px;" @tap="goSNCollect(item)">点数/SN</button>
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
		pageWMSRFASNCountQuantity
	} from "@/services/wMSASNCountQuantity/wMSASNCountQuantity";
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
					url: '/pages/wMSASNCountQuantity/component/editDialog?asnCountQuantityNumber='
					+row.asnCountQuantityNumber+"&id="
					+row.id+"&asnId="
					+row.asnId
				});
			},
			async goSNCollect (row) {
				uni.navigateTo({
					url: '/pages/wMSASNCountQuantity/component/editSNDialog?asnCountQuantityNumber='
					+row.asnCountQuantityNumber+"&id="
					+row.id+"&asnId="
					+row.asnId
				});
			},
			//获取订单列表
			async getOrderList() {
				var that = this;
				uni.showLoading({
					title: '加载中...'
				});

				try {
					await pageWMSRFASNCountQuantity(this.form).then((res) => {
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