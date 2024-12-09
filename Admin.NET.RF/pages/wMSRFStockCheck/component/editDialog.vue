<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">{{form.receiptNumber}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input :adjust-position="false" confirm-type="search" id="scanInput" :focus="focusflag"
					v-model="form.scanInput" v-focus="input" v-select="input" ref="input" name="input"
					@confirm="scanAcquisition()" clearable="" placeholder="请扫描" selection-start="0"
					:selection-end="selectendlength"></input>
			</view>
			<view v-if="this.list.length>0">
				<view class="cu-list menu-avatar">
					<view v-for="(item, index)  in this.list" :key="index" class="cu-item">
						<view class="cu-avatar round lg text-black"
				 			>{{item.scanQty}}
				 		</view>
						<view class="content">
							<view class="text-grey">{{item.sku}}</view>
							<view class="text-gray text-sm flex">
				 				<view class="text-cut">
				 					<text class="cuIcon-selection text-red  margin-right-xs">订单数量:</text>
									{{item.receivedQty}}
				 				</view>
								<view class="text-cut">
									<text class="cuIcon-selection text-red  margin-right-xs">已上架数量:</text>
									{{item.receiptQty}}
								</view>
				 			</view>
						</view>
					</view>
				</view>
			</view>
		</you-scroll>
	</view>

</template>

<script>
	import {
		scanReceiptReceivingApi
	} from "@/services/wMSRFReceiptReceiving/wMSReceiptReceiving";
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
					expirationDate: "",
					externReceiptNumber: "",
					receiptNumber: "",
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
			// this.getOrderList();
		},
		filters: {
			// carNumber(val) {
			// 	return val ? val.slice(0, 1) : '';
			// }
		},
		onLoad(options) {
			console.log("options");
			console.log(options);
			this.form.receiptNumber = options.receiptNumber;
		},
		methods: {
			lpnSearchSet() {
				this.focusflag = false;
				this.$nextTick(() => {
					this.focusflag = true;
				})
				this.selectendlength = this.form.scanInput.length;
			},
			async scanAcquisition() {
				this.lpnSearchSet();
				let that=this;
				let res = await scanReceiptReceivingApi(this.form);
				console.log(res.data.result.data)
				that.list=res.data.result.data;
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