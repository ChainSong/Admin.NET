<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">{{form.receiptNumber}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input disabled=“disabled” v-model="form.location" @confirm="scanAcquisition()" clearable=""
					placeholder="库位" selection-start="0"></input>
			</view>
			<view class="cu-form-group ">
				<input :adjust-position="false" confirm-type="search" id="scanInput" :focus="focusflag"
					v-model="form.scanInput" v-focus="input" v-select="input" ref="input" name="input"
					@confirm="scanAcquisition()" clearable="" placeholder="请扫描" selection-start="0"
					:selection-end="selectendlength"></input>
			</view>
			<view v-if="this.list.length>0">
				<view class="cu-list menu-avatar">
					<view v-for="(item, index)  in this.list" :key="index" class="cu-item">
						<view class="cu-avatar round lg text-black">{{item.pickQty}}
						</view>
						<view class="content">
							<view class="text-grey">SKU:{{item.sku}}</view>
							<view class="text-grey">库位:{{item.location}} | 批次:{{item.batchCode}}</view>
							<view class="text-gray text-sm flex">
								<view class="text-cut">
									<text class="cuIcon-selection text-red  margin-right-xs">订单数量:</text>
									{{item.qty}}
									<!-- 我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。 -->
								</view>
								<view class="text-cut">
									<text class="cuIcon-selection text-red  margin-right-xs">已拣数量:</text>
									{{item.pickQty}}
									<!-- 我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。我已天理为凭，踏入这片荒芜，不再受凡人的枷锁遏制。 -->
								</view>
								<!-- 	<view class="text-cut">
									<text class="cuIcon-selection text-red  margin-right-xs">已上架数量:</text>
									{{item.qty}}
								</view> -->
							</view>
							<!-- <view class="action">
								<view class="text-grey text-xs">22:20</view>
								<view class="cu-tag round bg-grey sm">5</view>
							</view> -->
						</view>
						<!-- <view class="action">
							 	{{item.receivedQty}}
						</view> -->
					</view>
				</view>
			</view>
		</you-scroll>
	</view>

</template>

<script>
	import {
		pageWMSRFOrderPickApi,
		scanOrderPickTaskApi,
		scanPickApi
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
					sn: "",
					scanInput: "",
				},
				list: {},
				id: "",
				data: [],
				visible: false,
				loading: false,
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
		},
		methods: {
			lpnSearchSet() {
				this.focusflag = false;
				this.$nextTick(() => {
					this.focusflag = true;
				})
				this.selectendlength = this.form.scanInput.length;
			},
			async getOrderList() {
				this.lpnSearchSet();
				let that = this;
				let res = await scanPickApi(this.form);
				console.log(res.data.result.data)
				that.list = res.data.result.data;
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
			async scanAcquisition() {
				this.lpnSearchSet();
				let that = this;
				let res = await scanOrderPickTaskApi(this.form);
				console.log(res.data.result.data)

				if (res.data.result.code == "1") {

					if (res.data.result.msg == "Location") {
						that.form.location = that.form.scanInput;
					} else if (res.data.result.msg == "SKU") {

					}
					that.list = res.data.result.data;
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
</style>