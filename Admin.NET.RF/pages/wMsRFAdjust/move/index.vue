<template>
	<view>
		<cu-custom bgColor="bg-gradual-blue" :isBack="true">
			<block slot="backText">返回</block>
			<block slot="content">RF移库</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view class="cu-form-group ">
				<input placeholder="请输入移库单号" v-model="form.adjustmentNumber" style="width: 100%;" name="input"></input>
				<button class="cu-btn bg-blue shadow-blur round" @click="clickQuery(form.adjustmentNumber)">查询</button>
				<button class="cu-btn bg-orange shadow-blur round" @click="showAddModal = true">新增</button>
			</view>
			<view>
				<uni-table border>
					<uni-tr>
						<uni-th width="100" align="center">移库单号</uni-th>
						<uni-th width="100" align="center">创建时间</uni-th>
						<uni-th width="100" align="center">操作</uni-th>
					</uni-tr>
					<uni-tr v-for="(item, index) in tableData" :key="index">
						<uni-td align="center">{{ item.adjustmentNumber }}</uni-td>
						<uni-td align="center">{{ item.createTime }}</uni-td>
						<uni-td align="center">
							<button class="cu-btn bg-pink shadow round sm" @click="handleOperate(item)">
								操作
							</button>
						</uni-td>
					</uni-tr>
				</uni-table>
			</view>
		</you-scroll>

		<!-- 使用封装的新增弹窗组件 -->
		<SelectCustomerAndWarehouse :show.sync="showAddModal" :warehouse-list="warehouseList"
			@success="handleAddSuccess" @close="handleModalClose" />
	</view>
</template>

<script>
	import SelectCustomerAndWarehouse from '@/pages/wMsRFAdjust/move/component/selectCustomerAndWarehouse.vue'
	import AddAdjustmentModal from '@/pages/wMsRFAdjust/move/component/addAdjustMove.vue'
	export default {
		components: {
			SelectCustomerAndWarehouse,
			AddAdjustmentModal
		},
		data() {
			return {
				showAddModal: false,
				// 可以从API获取这些数据
				warehouseList: [{
						label: '主仓库',
						value: 'MAIN'
					},
					{
						label: '备件库',
						value: 'SPARE'
					},
					{
						label: '成品库',
						value: 'PRODUCT'
					}
				],
				materialList: [{
						label: 'M001-原材料A',
						value: 'M001'
					},
					{
						label: 'M002-原材料B',
						value: 'M002'
					}
				],
				form: {
					adjustmentNumber: ''
				},
				tableData: [{
					createTime: '2016-05-02',
					adjustmentNumber: 'ADJ110',
				}, {
					createTime: '2016-05-04',
					adjustmentNumber: 'ADJ111',

				}]
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
				if (adjustmentNumber == '') {
					uni.showToast({
						title: '请输入移库单号',
						icon: 'none'
					});
					return;
				}
				uni.showLoading({
					title: '加载中...'
				});
				try {

				} catch {

				} finally {
					uni.hideLoading();
				}
			},
			//查询调整单的方法
			async searchAdj() {

			},
			// 新增成功回调
			handleAddSuccess(formData) {
				console.log('新增的数据:', formData)
				uni.navigateTo({
					url: '/pages/wMsRFAdjust/move/component/addAdjustMove'
				});
			},
			// 弹窗关闭回调
			handleModalClose() {
				console.log('弹窗已关闭')
			},
		},
		// 生命周期钩子会在组件生命周期的各个不同阶段被调用 例如这个函数就会在组件挂载完成后被调用
		mounted() {}
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