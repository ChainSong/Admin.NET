<template>
	<view>
		<cu-custom :isBack="false">
			<block slot="content">菜单栏</block>
		</cu-custom>
		<you-scroll ref="scroll" :style="[{height:'calc(100vh)'}]" @onPullDown="onPullDown">
			<view v-if="menuList && menuList.length>0">
				<view v-for="(item, index) in menuList" :key="index">
					<view class="cu-bar bg-white solid-bottom">
						<view class="action">
							<text class="cuIcon-titles text-bold text-blue "></text> {{item.meta.title}}
						</view>
					</view>
					<view v-if="item.children && item.children.length>0" class="cu-list grid"
						:class="['col-' + gridCol,gridBorder?'':'no-border']">
						<view class="cu-item" v-for="(child,cindex) in item.children" :key="cindex"
							v-if="cindex<gridCol*3">
							<navigator v-if="child.meta.isLink"
								:url="'/pages/menu/outlink?title='+child.meta.title+'&link='+child.meta.isLink"
								hover-class="other-navigator-hover">
								<view :class="['cuIcon-brandfill','text-' + menuColor]"></view>
								<text>{{child.meta.title}}</text>
							</navigator>
							
							<navigator v-else :url="child.path" open-type="navigate"
								hover-class="other-navigator-hover">
								
								<view :class="['wlq-zhijian','text-' + menuColor]"></view>
								<text>{{child.meta.title}}</text>
							</navigator>
						</view>
					</view>
				</view>
			</view>
			<view v-else class="cu-list grid col-1 my">
				<view class="cu-item">
					<text class="lg text-gray cuIcon-camerarotate"></text>
					<text>暂无数据</text>
				</view>
			</view>
		</you-scroll>

	</view>
</template>

<script>
	import {
		getMenu
	} from "@/services/common/menu";
	import youScroll from '@/components/you-scroll'
	export default {
		name: "menuhome",
		components: {
			youScroll
		},
		data() {
			return {
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar,
				loadingType: 'more',
				menuList: [],
				gridCol: 3,
				gridBorder: false,
				menuColor: 'blue'
			};
		},
		created() {
			this.getMenuList();
		},
		filters: {
			carNumber(val) {
				return val ? val.slice(0, 1) : '';
			}
		},
		methods: {
			onPullDown(done) { // 下拉刷新
				this.menuList.length = 0;
				this.getMenuList();
				done(); // 完成刷新
			},
			async getMenuList() {
				var that = this;
				uni.showLoading({
					title: '加载中...'
				});

				try {
					await getMenu().then((res) => {
						that.menuList = res.data.result ?? [];
						if(that.menuList.length>0)
						{
						  that.menuList=that.menuList.filter(c=>c.name!='system')	
						}
						 // console.log("menulist", that.menuList)
						uni.showToast({
							title: '加载完成！',
							icon: 'none'
						});
					});

				} finally {
					uni.hideLoading();
				}
			},


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

	.cu-list.grid>.cu-item [class*=cuIcon],[class*=wlq] {
		font-size: 30px !important;
	}
	
</style>