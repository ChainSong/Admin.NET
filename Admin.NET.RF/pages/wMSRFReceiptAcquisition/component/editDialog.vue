<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">{{form.receiptNumber}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group round">
				<input v-model="form.scanInput" id="scanInput" v-focus="input" v-select="input" ref="input" name="input"
					@keypress="scanAcquisition()" clearable="" placeholder="请扫描"></input>
				<!-- <button class="cu-btn bg-blue shadow-blur round" @tap="getOrderList()">查询</button> -->
			</view>
			<view class="cu-list menu-avatar">
				<view class="cu-item">
					<view class="content">
						<view class="text-grey">SKU:{{form.sku}}</view>
					</view>
				</view>
				<view class="cu-item">
					<view class="content">
						<view class="text-grey">Lot:{{form.lot}}</view>
					</view>
				</view>
				<view class="cu-item">
					<view class="content">
						<view class="text-grey">效期:{{form.expirationDate}}</view>
					</view>
				</view>
				<view class="cu-item">
					<view class="content">
						<view class="text-grey">sn:{{form.sn}}</view>
					</view>
				</view>
			</view>
		</you-scroll>
	</view>

</template>

<script>
	import {
		addWMSRFReceiptAcquisition,
		updateWMSRFReceiptAcquisition,
		saveAcquisition
	} from "@/services/wMSRFReceiptAcquisition/wMSRFReceiptAcquisition";
	import youScroll from '@/components/you-scroll';
	export default {
		name: "wMSRFReceiptAcquisitionDetail",
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
					sku: "",
					lot: "",
					expirationDate: "",
					externReceiptNumber: "",
					receiptNumber:"",
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
			async scanAcquisition() {
				if (event.keyCode === 13) {
					 
					this.form.sku = "";
					this.form.lot = "";
					this.form.expirationDate = "";
					this.form.sn = "";
					let acquisitionData = this.form.scanInput.split('|');

					if (acquisitionData.length == 3) {
						this.form.sku = acquisitionData[1];
						this.form.sn = acquisitionData[2] ?? "";
					} else {
						this.form.sku = acquisitionData[1];
						this.form.lot = acquisitionData[2] ?? "";
						this.form.expirationDate = acquisitionData[3] ?? "";
						// state.value.vm.form.sn = acquisitionData[4] ?? "";
					}
					let res = await saveAcquisition(this.form);
					if (res.data.result.code == "1") {
						uni.showLoading({
							title: '操作成功...'
						});
					} else {
						uni.showLoading({
							title: "操作失败:" + res.data.result.msg
						});
					}
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