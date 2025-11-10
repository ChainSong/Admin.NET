<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF交接</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input placeholder="请输入出库单号" v-model="form.adjustmentNumber" style="width: 100%;" name="input"></input>
				<button class="cu-btn bg-blue shadow-blur round" @click="clickQuery(form.adjustmentNumber)">查询</button>
			</view>
			<view>
				<uni-table border>
					<uni-tr>
						<uni-th width="100" align="center">出库单号</uni-th>
						<uni-th width="100" align="center">操作</uni-th>
					</uni-tr>
					<uni-tr v-for="(item, index) in tableData" :key="index">
						<uni-td align="center">{{ item.externOrderNumber }}</uni-td>
						<uni-td align="center">
							<button class="cu-btn bg-pink shadow round sm" @click="handleOperate(item)">
								交接
							</button>
						</uni-td>
					</uni-tr>
				</uni-table>
			</view>
		</you-scroll>
	</view>
</template>

<script>
	// import AddAdjustmentModal from '@/pages/wMsRFAdjust/move/component/addAdjustMove.vue'
	import {
		pendHandoverList
	} from '@/services/wMsRFAdjust/handover/handover.js'
	export default {
		components: {
			// AddAdjustmentModal
		},
		data() {
			return {
				form: {
					type:'RF交接',
					externOrdeerNumber:''
				},
				tableData: []
			}
		},
		// methods 是一些用来更改状态与触发更新的函数 它们可以在模板中作为事件处理器绑定
		methods: {
			onPullDown(done) { // 下拉刷新
				this.menuList.length = 0;
				done(); // 完成刷新
			},
			//查询
			async clickQuery(adjustmentNumber) {
				uni.showLoading({
					title: '加载中...'
				});
				try {
					await this.searchOrder(this.form)
				} catch {

				} finally {
					uni.hideLoading();
				}
			},
			async searchOrder(params) {
				let res = await pendHandoverList(params)
				console.log("res",res.data.result)
				this.tableData = res.data.result.orders ?? [];
			},
			handleOperate(params) {
				console.log("params:", params)
				uni.navigateTo({
					url: `/pages/wMsRFAdjust/handover/component/handoverEdit?formData=${encodeURIComponent(JSON.stringify(params))}`
				});
			},
			// 弹窗关闭回调
			handleModalClose() {
				console.log('弹窗已关闭')
			},

		},
		// 生命周期钩子会在组件生命周期的各个不同阶段被调用 例如这个函数就会在组件挂载完成后被调用
		async mounted() {
			await this.searchOrder(this.form)
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

	/* 表格按钮样式 */
	.sm {
		font-size: 24rpx;
		padding: 8rpx 20rpx;
	}
</style>