<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">{{form.receiptNumber}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input :adjust-position="false" confirm-type="search"  id="scanInput" :focus="focusflag" v-model="form.scanInput"  v-focus="input" v-select="input" ref="input" name="input"
					@confirm="scanAcquisition()" clearable="" placeholder="请扫描"  selection-start="0" :selection-end="selectendlength"></input>
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
		getRFReceiptReceiving
	} from "@/services/wMSReceiptReceiving/wMSReceiptReceiving";
	import youScroll from '@/components/you-scroll';
	export default {
		name: "wMSShelveDetail",
		components: {
			youScroll
		},
		data() {
			return {
				selectendlength:0,
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
			lpnSearchSet() {
			    this.focusflag = false;
				this.$nextTick(()=>{
					this.focusflag = true;
				})
				this.selectendlength=this.form.lpn.length;
				// document.querySelector('#lpnselect input').select()
			},
			async scanAcquisition() {
				    this.lpnSearchSet(); 
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