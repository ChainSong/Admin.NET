<template>
	<view>
		<view class="cu-custom" :style="[{height:(isSearch?CustomBar:0) + 'px'}]">
			<view class="cu-bar fixed" :style="style" :class="[bgImage!=''?'none-bg text-white bg-img':'',bgColor]">
				<view class="action" @tap="BackPage" v-if="isBack">
					<text class="cuIcon-back"></text>
					<slot name="backText"></slot>
				</view>
				<view v-else>
					<view>
						<image :src="leftImage" style="width: 35px;height: 35px;margin-left: 5px;"></image>
					</view>
					<slot name="loginText"></slot>
				</view>
				<view class="content" :style="[{top:StatusBar + 'px'}]">
					<slot name="content"></slot>
				</view>
				<slot name="right">
					<view v-if="isLogin" class="cu-item rightLine" @tap="showModal" data-target="DialogModal1">
						<text class="text-xl text-bold text-white wlq-yonghuguanli"></text>
						<text class="text-white text-bold text-xs">{{userInfo.account}}</text>
					</view>
				</slot>
			</view>


			<view class="cu-modal" :class="modalName=='DialogModal1'?'show':''">
				<view class="cu-dialog">
					<view class="cu-bar bg-white justify-end">
						<view class="content">退出登录</view>
						<view class="action" @tap="hideModal">
							<text class="cuIcon-close text-red"></text>
						</view>
					</view>
					<view class="padding-xl bg-white">
						<view>
							<text class="text-red">是否确认退出当前登录账户[{{userInfo.account}}]？</text>
						</view>
					</view>
					<view class="cu-bar bg-white justify-end">
						<view class="action">
							<button class="cu-btn line-green text-green" @tap="hideModal">取消</button>
							<button class="cu-btn bg-green margin-left" @tap="logout">确定</button>
						</view>
					</view>
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
				modalName: null,
				userInfo: uni.getStorageSync("userInfo"),
				StatusBar: this.StatusBar,
				CustomBar: this.CustomBar
			};
		},
		name: 'cu-custom',
		computed: {
			style() {
				var StatusBar = this.StatusBar;
				var CustomBar = this.CustomBar;
				var bgImage = this.bgImage;
				var style = `height:${CustomBar}px;padding-top:${StatusBar}px;`;
				if (this.bgImage) {
					style = `${style}background-image:url(${bgImage});`;
				}
				return style
			}
		},
		props: {
			bgColor: {
				type: String,
				default: 'bg-gradual-blue'
			},
			isSearch: {
				type: [Boolean, String],
				default: true
			},
			isBack: {
				type: [Boolean, String],
				default: false
			},
			isLogin: {
				type: [Boolean, String],
				default: true
			},
			isAction: {
				type: [Boolean, String],
				default: false
			},
			bgImage: {
				type: String,
				default: ''
			},
			leftImage: {
				type: String,
				default: '/static/cumminsLogo.svg'
			}
		},
		created() {
			if (!this.userInfo) {
				this.getUserInfo();
			}
		},
		methods: {
			BackPage() {
				uni.navigateBack({
					delta: 1
				});
				if (this.isAction) {
					uni.$emit('customEvent');
				}
			},
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
			showModal(e) {
				this.modalName = e.currentTarget.dataset.target
			},
			hideModal(e) {
				this.modalName = null
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
		}
	}
</script>

<style>
	.rightLine {
		display: flex;
		flex-direction: column;
		text-align: center;
		margin-right: 2px;
		font-size: 20px;
		margin-bottom: 5px;
	}

	.rightLine text:first-child {
		margin-bottom: 2px;
	}
</style>