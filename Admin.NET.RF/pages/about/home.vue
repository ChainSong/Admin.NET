<template>
	<view>
		<view class="cu-list menu-avatar fixed">
			<view class="cu-item cur">
				<view class="cu-avatar radius lg" style="background-image:url(/static/logo.png);">
				</view>
				<view class="content">
					<view>
						<view class="text-cut">{{userInfo.realName}}</view>
					</view>
					<view class="text-gray text-sm flex">
						<view class="text-cut"> 账号：{{userInfo.account}}</view>
					</view>
				</view>
				<view class="action">
					<view class="text-grey text-xs">{{userInfo.remark}}</view>
					<view class="cuIcon-notice_forbid_fill text-gray"></view>
				</view>
			</view>
		</view>
		<view class="cu-list menu" :class="[menuBorder?'sm-border':'',menuCard?'card-menu margin-top':'']">
			<view class="cu-item">
				<view class="content">
					<text class="cuIcon-circlefill text-grey"></text>
					<text class="text-grey">账号</text>
				</view>
				<view class="action">
					<text class="text-grey">{{userInfo.account}}</text>
				</view>
			</view>
			<view class="cu-item">
				<view class="content">
					<image src="/static/logo.png" class="png" mode="aspectFit"></image>
					<text class="text-grey">手机号</text>
				</view>
				<view class="action">
					<text class="text-grey">{{userInfo.phone}}</text>
				</view>
			</view>
			<view class="cu-item">
				<view class="content">
					<text class="cuIcon-tagfill text-red  margin-right-xs"></text>
					<text class="text-grey">角色</text>
				</view>
				<view class="action">
					<view class="cu-tag round bg-orange light">{{userInfo.remark}}</view>
				</view>
			</view>
			<view class="cu-item">
				<view class="content">
					<text class="cuIcon-btn text-green"></text>
					<text class="text-grey">权限</text>
				</view>
			</view>
		</view>
		<view class="cu-list menu margin-top">
			<view class="cu-item">
				<view class="content text-center">
					<button class="cu-btn content" open-type="contact" @tap="logout">
						<text class="text-grey">退出登录</text>
					</button>
				</view>
			</view>
		</view>


	</view>
</template>

<script>
	import {
		signOut,
		userInfo
	} from "@/services/login/index";
	export default {
		data() {
			return {
				userInfo: uni.getStorageSync("userInfo"),
				modalName: null,
				gridCol: 3,
				gridBorder: false,
				menuBorder: false,
				menuArrow: true,
				menuCard: false,
				skin: false,
				listTouchStart: 0,
				listTouchDirection: null,
			};
		},
		created() {
			if (!this.userInfo) {
				this.getUserInfo();
			}
		},
		methods: {
			async getUserInfo() {
				var that = this;
				await userInfo().then((res) => {
					if (res.data.code == '200') {
						that.userInfo = res.data.result;

					} else {
						uni.showToast({
							title: res.data.message,
							icon: 'none'
						});
					}
				});
			},
			async logout() {
				await signOut().then(() => {

					this.$mStore.commit('logout');

					uni.navigateTo({
						url: `/pages/login/index`
					});
					uni.showToast({
						title: '退出成功',
						icon: 'none'
					});

				});
			},
			MenuBorder(e) {
				this.menuBorder = e.detail.value
			},
			MenuArrow(e) {
				this.menuArrow = e.detail.value
			},
			MenuCard(e) {
				this.menuCard = e.detail.value
			},
			// ListTouch触摸开始
			ListTouchStart(e) {
				this.listTouchStart = e.touches[0].pageX
			},
		}
	}
</script>

<style>
	.page {
		height: 100Vh;
		width: 100vw;
	}

	.page.show {
		overflow: hidden;
	}

	.cu-list.menu-avatar>.cu-item {
		height: 124px;
	}

	.cu-list.menu>.cu-item {
		min-height: 56px;
	}

	.switch-sex::after {
		content: "\e716";
	}

	.switch-sex::before {
		content: "\e7a9";
	}

	.switch-music::after {
		content: "\e66a";
	}

	.switch-music::before {
		content: "\e6db";
	}
</style>