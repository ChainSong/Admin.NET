<!-- 蓝色登录页面2 -->
<template>
	<view>
		<view class="img-a">
			<view class="t-b">
				您好，
				<br />
				欢迎使用RF终端
			</view>
		</view>
		<view class="login-view" style="">
			<view class="t-login">
				<form class="cl">
					<view class="t-a">
						<text class="txt">账号</text>
						<input type="text" name="account" placeholder="请输入您的账号" maxlength="11"
							v-model="loginform.account" />
					</view>
					<view class="t-a">
						<text class="txt">密码</text>
						<input type="password" name="code" maxlength="18" placeholder="请输入您的密码"
							v-model="loginform.password" />
					</view>
					<button @tap="login()">登 录</button>
					<view class="reg" @tap="fogetpassword()">忘记密码</view>
				</form>
				<view class="t-f"><text>—————— 上海尼望软件 ——————</text></view>
				<view class="t-e cl">
					<view class="t-g">
						<image src="@/static/cumminsLogo.png"></image>
					</view>
					<view class="t-g">
						<view class="cu-avatar round sm margin-left bg-blue"
							style="font-size: 0.8em;width:27px;height: 27px;">尼望</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>
<script>
	import {
		signIn,
		userInfo
	} from "@/services/login/index";
	export default {
		data() {
			return {
				loginform: {
					account: '',
					password: '',
					code: '',
					codeId: 0,
				}
			};
		},
		onShow() {
			if (this.$mStore.getters.hasLogin) {
				uni.navigateTo({
					url: '/pages/index/index'
				});
			}
		},
		onLoad() {},
		methods: {
			//当前登录按钮操作
			async login() {
				var that = this;
				if (!that.loginform.account) {
					uni.showToast({
						title: '请输入您的登录账号',
						icon: 'none'
					});
					return;
				}
				if (!that.loginform.password) {
					uni.showToast({
						title: '请输入您的密码',
						icon: 'none'
					});
					return;
				}

				uni.showLoading({
					title: '登录中...'
				});
				try {
					// const sm2 = require('sm-crypto').sm2
					// const publicKey =
					// 	`0484C7466D950E120E5ECE5DD85D0C90EAA85081A3A2BD7C57AE6DC822EFCCBD66620C67B0103FC8DD280E36C3B282977B722AAEC3C56518EDCEBAFB72C5A05312`;
					// const password = sm2.doEncrypt(that.loginform.password, publicKey, 1);
					await signIn({
						...that.loginform,
						password: that.loginform.password
					}).then((res) => {
						console.log("res", res)
						this.$mStore.commit('login', res.data);
						userInfo().then((u) => {
							this.$mStore.commit('setUserinfo', u.data.result);
						});

						uni.showToast({
							title: '登录成功！',
							icon: 'none'
						});
						uni.navigateTo({
							url: '/pages/menu/index'
						});
					});

				} finally {
					uni.hideLoading();
				}
			},
			//忘记密码按钮点击
			fogetpassword() {
				uni.showToast({
					title: '请联系管理员后台重置密码',
					icon: 'none'
				});
			},
		}
	};
</script>
<style>
	.txt {
		font-size: 32rpx;
		font-weight: bold;
		color: #333333;
	}

	.img-a {
		width: 100%;
		height: 450rpx;
		background-image: url(../../static/head.png);
		background-size: 100%;
	}

	.reg {
		font-size: 28rpx;
		color: #fff;
		height: 90rpx;
		line-height: 90rpx;
		border-radius: 50rpx;
		font-weight: bold;
		background: #f5f6fa;
		color: #000000;
		text-align: center;
		margin-top: 30rpx;
	}

	.login-view {
		width: 100%;
		position: relative;
		margin-top: -120rpx;
		background-color: #ffffff;
		border-radius: 8% 8% 0% 0;
	}

	.t-login {
		width: 600rpx;
		margin: 0 auto;
		font-size: 28rpx;
		padding-top: 80rpx;
	}

	.t-login button {
		font-size: 28rpx;
		background: #2796f2;
		color: #fff;
		height: 90rpx;
		line-height: 90rpx;
		border-radius: 50rpx;
		font-weight: bold;
	}

	.t-login input {
		height: 90rpx;
		line-height: 90rpx;
		margin-bottom: 50rpx;
		border-bottom: 1px solid #e9e9e9;
		font-size: 28rpx;
	}

	.t-login .t-a {
		position: relative;
	}

	.t-b {
		text-align: left;
		font-size: 42rpx;
		color: #ffffff;
		padding: 130rpx 0 0 70rpx;
		font-weight: bold;
		line-height: 70rpx;
	}

	.t-login .t-c {
		position: absolute;
		right: 22rpx;
		top: 22rpx;
		background: #5677fc;
		color: #fff;
		font-size: 24rpx;
		border-radius: 50rpx;
		height: 50rpx;
		line-height: 50rpx;
		padding: 0 25rpx;
	}

	.t-login .t-d {
		text-align: center;
		color: #999;
		margin: 80rpx 0;
	}

	.t-login .t-e {
		text-align: center;
		width: 250rpx;
		margin: 80rpx auto 0;
	}

	.t-login .t-g {
		float: left;
		width: 50%;
	}

	.t-login .t-e image {
		width: 50rpx;
		height: 50rpx;
	}

	.t-login .t-f {
		text-align: center;
		margin: 150rpx 0 0 0;
		color: #666;
	}

	.t-login .t-f text {
		margin-left: 20rpx;
		color: #aaaaaa;
		font-size: 27rpx;
	}

	.t-login .uni-input-placeholder {
		color: #aeaeae;
	}

	.cl {
		zoom: 1;
	}

	.cl:after {
		clear: both;
		display: block;
		visibility: hidden;
		height: 0;
		content: '\20';
	}
</style>