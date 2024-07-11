## 1.2.8（2023-09-05）
新增选中的某行样式
新增统计列
<template>
	<view class="content" style="padding: 32rpx;box-sizing: border-box;width: 100vw;height: 40vw;overflow: hidden;box-sizing: border-box;">
		<lh-table :rows="state.rows" :columns="state.columns" :showStatistics="true" @tapRow="tapRow" @tapToolbar="tapToolbar" @tapOper="tapOper" @refresh="refresh" @check="check" @checkAll="checkAll" @loadMore="loadMore">
			<!-- 统计列插槽示意 -->
			<template #statistics="{row}">
				<view>{{ row.yj + row.amount }}</view>
			</template>
		</lh-table>
	</view>
</template>

<script setup>
	import { reactive } from 'vue'
	const state = reactive({
		columns: [
			{name: 'yjbl', title: '佣金比例'},
			{name: 'yj', title: '佣金', align: 'right'},
			{name: 'amount', title: '消费金额', align: 'right'},
			{name: 'user', title: '被邀请用户'},
			{name: 'date', title: '消费时间', formate: data => data},
		],
		rows: [
			{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
			{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		]
	})
	
	const tapOper = data => {
		console.log(data)
	}
	
	const tapRow = data => {
		console.log(data);
	}
	
	const scroll = e => {
		// console.log(e);
	}
	
	const loadMore = () => {
		// state.rows = state.rows.concat([
		// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		// ])
	}
	
	const refresh = () => {
		// state.rows = [
		// 	{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()}
		// ]
	}
	
	const check = param => {
		state.rows[param.index] = param.data
	}
	
	const checkAll = rows => {
		state.rows = rows
	}
	
	const tapToolbar = btnName => {
		console.log(btnName);
	}
	
</script>

<style>
	.content {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
	}

	.logo {
		height: 200rpx;
		width: 200rpx;
		margin-top: 200rpx;
		margin-left: auto;
		margin-right: auto;
		margin-bottom: 50rpx;
	}

	.text-area {
		display: flex;
		justify-content: center;
	}

	.title {
		font-size: 36rpx;
		color: #8f8f94;
	}
</style>

## 1.2.7（2023-09-04）
修复固定列的格式化
## 1.2.6（2023-09-04）
去掉刷新，高度继承父容器
## 1.2.5（2023-09-04）
高度设置
## 1.2.4（2023-09-04）
将整体高度继承为如容器高度
## 1.2.3（2023-09-04）
#### 1、删除·列插槽

#### 2、新增·列格式化
```
<!-- 格式化列 -->
columns: [
	{name: 'yjbl', title: '佣金比例', fixed: true},
	{name: 'yj', title: '佣金', align: 'right'},
	{name: 'amount', title: '消费金额', align: 'right'},
	{name: 'user', title: '被邀请用户'},
	{name: 'date', title: '消费时间', formate: data => data},
]
```
## 1.2.2（2023-08-24）
## 功能
#### 1、固定
1. 头
2. 列
3. 序号
4. 操作列
5. 勾选列

#### 2、事件
1. 查看（操作列）
2. 删除（操作列）
3. 编辑（操作列）
4. 点击行数据
5. 上拉加载数据
6. 下来刷新数据

#### 3、插槽
1. 数据列插槽（插槽名称通过colname判断）
2. 操作列插槽


#### 4、工具栏
1. 新增
2. 删除（批量）

#### 5、下标题

## 代码
#### view 代码块
```
<lh-table :rows="state.rows" :columns="state.columns" @tapRow="tapRow" @tapToolbar="tapToolbar" @tapOper="tapOper" @refresh="refresh" @check="check" @checkAll="checkAll" @loadMore="loadMore">
	<template #yjbl="props">
		{{props.row.yjbl}}
	</template>
	<template #yj="props">
		{{props.row.yj}}
	</template>
	<template #operCol="props">
		<!-- <text>新增</text> -->
	</template>
</lh-table>
```
#### js 代码块
```
<!-- setup 语法 -->
import { reactive } from 'vue'
	
const state = reactive({
	columns: [
		{name: 'yjbl', title: '佣金比例', fixed: true},
		{name: 'yj', title: '佣金', align: 'right'},
		{name: 'amount', title: '消费金额', align: 'right'},
		{name: 'user', title: '被邀请用户'},
		{name: 'date', title: '消费时间'},
	],
	rows: [
		{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	]
})

const tapOper = data => {
	console.log(data)
}

const tapRow = data => {
	console.log(data);
}

const scroll = e => {
	// console.log(e);
}

const loadMore = () => {
	// state.rows = state.rows.concat([
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ])
}

const refresh = () => {
	// state.rows = [
	// 	{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ]
}

const check = param => {
	state.rows[param.index] = param.data
}

const checkAll = rows => {
	state.rows = rows
}

const tapToolbar = btnName => {
	console.log(btnName);
}
```
## 1.2.1（2023-08-24）
## 功能
#### 1、固定
1. 头
2. 列
3. 序号
4. 操作列
5. 勾选列

#### 2、事件
1. 查看（操作列）
2. 删除（操作列）
3. 编辑（操作列）
4. 点击行数据
5. 上拉加载数据
6. 下来刷新数据

#### 3、插槽
1. 数据列插槽（插槽名称通过colname判断）
2. 操作列插槽


#### 4、工具栏
1. 新增
2. 删除（批量）

#### 5、下标题

## 代码
#### view 代码块
```
<lh-table :rows="state.rows" :columns="state.columns" @tapRow="tapRow" @tapToolbar="tapToolbar" @tapOper="tapOper" @refresh="refresh" @check="check" @checkAll="checkAll" @loadMore="loadMore">
	<template #yjbl="props">
		{{props.row.yjbl}}
	</template>
	<template #yj="props">
		{{props.row.yj}}
	</template>
	<template #operCol="props">
		<!-- <text>新增</text> -->
	</template>
</lh-table>
```
#### js 代码块
```
<!-- setup 语法 -->
import { reactive } from 'vue'
	
const state = reactive({
	columns: [
		{name: 'yjbl', title: '佣金比例', fixed: true},
		{name: 'yj', title: '佣金', align: 'right'},
		{name: 'amount', title: '消费金额', align: 'right'},
		{name: 'user', title: '被邀请用户'},
		{name: 'date', title: '消费时间'},
	],
	rows: [
		{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	]
})

const tapOper = data => {
	console.log(data)
}

const tapRow = data => {
	console.log(data);
}

const scroll = e => {
	// console.log(e);
}

const loadMore = () => {
	// state.rows = state.rows.concat([
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ])
}

const refresh = () => {
	// state.rows = [
	// 	{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ]
}

const check = param => {
	state.rows[param.index] = param.data
}

const checkAll = rows => {
	state.rows = rows
}

const tapToolbar = btnName => {
	console.log(btnName);
}
```
## 1.1.0（2023-08-24）
## 功能
#### 1、固定
1. 头
2. 列
3. 序号
4. 操作列
5. 勾选列

#### 2、事件
1. 查看（操作列）
2. 删除（操作列）
3. 编辑（操作列）
4. 点击行数据
5. 上拉加载数据
6. 下来刷新数据

#### 3、插槽
1. 数据列插槽（插槽名称通过colname判断）
2. 操作列插槽


#### 4、工具栏
1. 新增
2. 删除（批量）

#### 5、下标题

## 代码
#### view 代码块
```
<lh-table :rows="state.rows" :columns="state.columns" @tapRow="tapRow" @tapToolbar="tapToolbar" @tapOper="tapOper" @refresh="refresh" @check="check" @checkAll="checkAll" @loadMore="loadMore">
	<template #yjbl="props">
		{{props.row.yjbl}}
	</template>
	<template #yj="props">
		{{props.row.yj}}
	</template>
	<template #operCol="props">
		<!-- <text>新增</text> -->
	</template>
</lh-table>
```
#### js 代码块
```
<!-- setup 语法 -->
import { reactive } from 'vue'
	
const state = reactive({
	columns: [
		{name: 'yjbl', title: '佣金比例', fixed: true},
		{name: 'yj', title: '佣金', align: 'right'},
		{name: 'amount', title: '消费金额', align: 'right'},
		{name: 'user', title: '被邀请用户'},
		{name: 'date', title: '消费时间'},
	],
	rows: [
		{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	]
})

const tapOper = data => {
	console.log(data)
}

const tapRow = data => {
	console.log(data);
}

const scroll = e => {
	// console.log(e);
}

const loadMore = () => {
	// state.rows = state.rows.concat([
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ])
}

const refresh = () => {
	// state.rows = [
	// 	{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// 	{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	// ]
}

const check = param => {
	state.rows[param.index] = param.data
}

const checkAll = rows => {
	state.rows = rows
}

const tapToolbar = btnName => {
	console.log(btnName);
}
```
## 1.0.2（2023-08-24）
## 功能
#### 1、固定
1. 头
2. 列
3. 序号
4. 操作列

#### 2、事件
1. 查看（操作列）
2. 删除（操作列）
3. 编辑（操作列）
4. 点击行数据
5. 上拉加载数据
6. 下来刷新数据

#### 3、插槽
1. 数据列插槽（插槽名称通过colname判断）
2. 操作列插槽


## 代码
#### view 代码块
```
<lh-table :rows="state.rows" :columns="state.columns" @tapRow="tapRow" @tapOper="tapOper" @refresh="refresh" @loadMore="loadMore">
	<template #yjbl="props">
		{{props.row.yjbl}}
	</template>
	<template #yj="props">
		{{props.row.yj}}
	</template>
	<template #operCol="props">
		<text>新增</text>
	</template>
</lh-table>
```
#### js 代码块
```
<!-- setup 语法 -->
import { reactive } from 'vue'
import LhTable from '../../components/lh-table/lh-table.vue'

const state = reactive({
	columns: [
		{name: 'yjbl', title: '佣金比例', fixed: true},
		{name: 'yj', title: '佣金', align: 'right'},
		{name: 'amount', title: '消费金额', align: 'right'},
		{name: 'user', title: '被邀请用户'},
		{name: 'date', title: '消费时间'},
	],
	rows: [
		{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	]
})

// 点击操作列按钮
const tapOper = data => {
	console.log(data)
}

// 点击行
const tapRow = data => {
	console.log(data);
}

// 模拟加载更多
const loadMore = () => {
	state.rows = state.rows.concat([
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	])
}

// 模拟刷新数据
const refresh = () => {
	state.rows = [
		{id: 0, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: '2023-08-23 10:00:25'},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 10000, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
		{id: 1, yjbl: '10%', yj: 10, user: 666666, amount: 100, date: Date.now()},
	]
}
```
## 1.0.1（2023-08-24）
修改默认值
## 1.0.0（2023-08-24）
## 发布
## 202-08-24
+ 新增
	+ 固定头
	+ 固定列
	+ 操作列
	+ 行数据插槽
	+ 操作列插槽
	+ 行数据`tabRow`事件
	+ 操作列`tapOper`事件
	+ 下拉刷新`refresh`事件
	+ 上拉加载`loadMore`事件