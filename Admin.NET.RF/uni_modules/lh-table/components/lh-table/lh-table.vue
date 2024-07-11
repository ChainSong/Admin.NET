<template>
	<view class="gui-flex gui-column" style="height: 100%;overflow: hidden;">
		<view v-if="props.showToolbar" class="toolbar">
			<slot name="toolbar">
				<button @tap="tapToolbar('add')" class="button primary">新增</button>
				<button @tap="tapToolbar('remove')" class="button danger">删除</button>
			</slot>
		</view>
		<scroll-view ref="scrollView" class="table" scroll-x="false" scroll-y="true" @scrolltolower="scrollToBottom"
			:style="{height: state.height}">
			<view ref="scrollViewContent" style="position: sticky;left: 0;z-index: 10;box-shadow: 0 0 10rpx rgba(99, 99, 99, .5);display: inline-block;">
				<view class="column">
					<!-- 复选框 -->
					<view v-if="props.showChecker" class="column" style="background: #fff;">
						<view :style="props.headerStyle" class="column-row" style="position: sticky;top: 0;background: #f1f2f3;font-weight: bold;z-index: 10;">
							<checkbox @tap="checkAll" :checked="state.checkAll" :class="state.checkAllClass" style="width: 16px;height: 16px;"></checkbox>
						</view>
						<view @tap="tapRow(ind, 'checkboxDefault', row)" v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row column-order">
							<checkbox @tap="check(ind, row)" :checked="row.checked" style="width: 16px;height: 16px;"></checkbox>
						</view>
					</view>
					<!-- 序号列 -->
					<view v-if="props.showOrder" class="column" style="background: #fff;">
						<view :style="props.headerStyle" class="column-row" style="position: sticky;top: 0;background: #f1f2f3;font-weight: bold;">序号</view>
						<view @tap="tapRow(ind, 'orderDefault', row)" v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row column-order">
							{{ ind + 1 }}
						</view>
					</view>
					<!-- 固定列 -->
					<view v-for="col in props.columns.filter(col => col.fixed)" :key="col.name" class="column" style="background: #fff;">
						<view :style="props.headerStyle" class="column-row column-title" style="position: sticky;top: 0;background: #f1f2f3;font-weight: bold;">{{ col.title }}</view>
						<view @tap="tapRow(ind, col.name, row)" v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row" :style="{textAlign: col.align || 'center'}">
							<slot :row="row" :name="col.name">
								{{ col.formate ? col.formate(row[col.name]) : row[col.name] }}
							</slot>
						</view>
					</view>
				</view>
			</view>
			<!-- 普通列 -->
			<view ref="scrollViewContent" style="display: inline-block;">
				<view v-for="col in props.columns.filter(col => !col.fixed)" :key="col.name" class="column">
					<view :style="props.headerStyle" class="column-row column-title">{{ col.title }}</view>
					<view @tap="tapRow(ind, col.name, row)" v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row" :style="{textAlign: col.align || 'center'}">
						{{ col.formate ? col.formate(row[col.name]) : row[col.name] }}
					</view>
				</view>
			</view>
			<!-- 操作列 -->
			<view v-if="props.showOper" class="column" style="position: sticky;right: 0;z-index: 10;box-shadow: 0 0 10rpx rgba(99, 99, 99, .5);background: #fff;">
				<view :style="props.headerStyle" class="column-row column-title">操作</view>
				<view v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row">
					<slot :row="row" name="operCol">
						<text @tap="operTap('remove', ind, row)" v-if="props.operBtns.remove.show" class="oper" style="font-size: 14px;color: #EE0A25;">{{ props.operBtns.remove.text || '删除'}}</text>
						<text @tap="operTap('view', ind, row)" v-if="props.operBtns.view.show" class="oper" style="font-size: 14px;color: #166BD8;">{{ props.operBtns.view.text || '查看'}}</text>
						<text @tap="operTap('edit', ind, row)" v-if="props.operBtns.edit.show" class="oper" style="font-size: 14px;color: #166BD8;">{{ props.operBtns.edit.text || '编辑'}}</text>
					</slot>
				</view>
			</view>
			<view v-if="props.showStatistics" class="column" style="position: sticky;right: 0;z-index: 10;box-shadow: 0 0 10rpx rgba(99, 99, 99, .5);background: #fff;">
				<view :style="props.headerStyle" class="column-row column-title">统计</view>
				<view v-for="(row, ind) in props.rows" :class="row == state.currRow ? 'bg-active' : ''" :key="row.id" class="column-row">
					<slot :row="row" name="statistics">
						插槽：statistics
					</slot>
				</view>
			</view>
			<!-- 统计列 -->
		</scroll-view>
		<view v-if="props.showFooter" class="footer">
			<slot name="footer">
				<view>年度收支表</view>
			</slot>
		</view>
	</view>
</template>

<script setup>
	import { reactive, defineEmits, getCurrentInstance } from 'vue'
	const instance = getCurrentInstance()
	const emits = defineEmits(['tapOper', 'tabRow', 'loadMore', 'check', 'checkAll', 'tapToolbar'])
	
	const props = defineProps({
		showStatistics: {
			type: Boolean,
			default: false
		}, 
		showFooter: {
			type: Boolean,
			default: false
		},
		showToolbar: {
			type: Boolean,
			default: false
		},
		showOper: {
			type: Boolean,
			default: false
		},
		showChecker: {
			type: Boolean,
			default: false
		},
		operBtns: {
			type: Object,
			default: () => {
				return {
					remove: {
						show: false,
						text: '删除'
					},
					view: {
						show: true,
						text: '详情'
					},
					edit: {
						show: false,
						text: '编辑'
					}
				}
			}
		},
		showOrder: {
			type: Boolean,
			default: true
		},
		columns: {
			type: Array,
			default: () => []
		},
		rows: {
			type: Array,
			default: () => []
		},
		headerStyle: {
			type: String,
			default: ''
		}
	})
	
	const state = reactive({
		currRow: null,
		checkAll: false,
		checkAllClass: '',
		height: `calc(100% - ${props.showFooter ? '80px' : '0px'} - ${props.showToolbar ? '60px' : '0px'})`
	})
	
	const scrollToBottom = e => {
		if (e.detail.direction == 'bottom') {
			emits('loadMore')
		}
	}
	
	const operTap = (type, ind, item) => {
		emits('tapOper', {
			type: type,
			index: ind,
			data: item
		})
	}
	
	const tapRow = (index, colName, data) => {
		state.currRow = data
		emits('tapRow', {
			index,
			colName,
			data
		})
	}
	
	const checkAll = () => {
		state.checkAllClass = ''
		state.checkAll = !state.checkAll
		const rows = props.rows.map(row => {
			row.checked = state.checkAll
			return row
		})
		emits('checkAll', rows)
	}
	
	const tapToolbar = name => {
		emits('tapToolbar', name)
	}
	
	const check = (ind, row) => {
		row.checked = !row.checked
		// 选中数量
		const allCount = props.rows.length
		const checkedCount = props.rows.filter(row => row.checked).length
		if (allCount == checkedCount) {
			state.checkAllClass = ''
			state.checkAll = true
		}
		if (!checkedCount) {
			state.checkAllClass = ''
			state.checkAll = false
		}
		if (checkedCount && checkedCount < allCount) {
			state.checkAll = false
			state.checkAllClass = 'variable'
		}
		emits('check', {index: ind, data: row})
	}
	
</script>

<style lang="scss" scoped>
	.footer {
		height: 80px;
		padding: 16rpx;
		font-size: 18px;
	}
	.toolbar {
		width: 100%;
		padding: 16rpx 0;
		height: 60px;
		display: flex;
		align-items: center;
		justify-content: flex-end;
		.button {
			display: inline-block;justify-content: flex-end;
			font-size: 14px;
			margin: 0;
			margin-left: 16rpx;
		}
		.primary {
			background: #007AFF;
			color: #fff;
		}
		.danger {
			background: #EE0A25;
			color: #fff;
		}
	}
	.table {
		width: calc(100vw - 64rpx);
		border: 1px solid #eee;
		white-space: nowrap;
		box-sizing: border-box;
	  .column {
			display: inline-block;
			border-left: 1px solid #eee;
			.column-row {
				white-space: nowrap;
				text-align: center;
				border-bottom: 1px solid #eee;
				padding: 12rpx 24rpx;
			}
			.column-title {
				font-weight: bold;
				color: #444;
				position: sticky;
				top: 0;
				background: #f1f2f3;
			}
		}
		.column:first-child {
			border-left: none;
		}
		.oper:nth-child(n+2) {
			margin-left: 16rpx;
		}
		.bg-active {
			color: #fff !important;
			background: #31A990;
		}
	}
	::v-deep .variable .uni-checkbox-input::after {
		content: '-';
		font-size: 28px;
		line-height: 16px;
		color: #007AFF;
	}
</style>