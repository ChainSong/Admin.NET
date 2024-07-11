<template>
	<view>
	
	</view>
</template>

<script>
	export default {
		data() {
			return {
				PageCur: 'entrycheck'
			}
		},
		onLoad(options) {
			if (options && options.PageCur)
				this.PageCur = options.PageCur;
		},
		methods: {
			NavChange: function(e) {
				this.PageCur = e.currentTarget.dataset.cur
			}
		}
	}
</script>

<style>

</style>