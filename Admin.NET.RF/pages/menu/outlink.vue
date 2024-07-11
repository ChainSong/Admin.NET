<template>
	<view>
		<cu-custom  :isBack="true">
			<block slot="content">{{title}}</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]">
			<web-view :src="link"></web-view>
		</you-scroll>

	</view>
</template>

<script>
	import youScroll from '@/components/you-scroll'
	export default {
		name: "outlink",
		components: {
			youScroll
		},
		data() {
			return {
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar,
				title: '',
				link: null
			};
		},
		onLoad(options) {
			this.title = options.title;
			this.link = options.link;
		}
	}
</script>