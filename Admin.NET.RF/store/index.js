import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);
const ACCESSTOKEN = uni.getStorageSync('access-token') || '';
const USERINFO = uni.getStorageSync('userInfo') || {};
const REFRESHTOKEN = uni.getStorageSync('x-access-token') || '';

const store = new Vuex.Store({
	state: {
		// 用户token
		accessToken: ACCESSTOKEN,
		// 用户信息
		userInfo: USERINFO,
		// 网络状态，用于下载提醒
		networkState: 'unknown',
		refreshToken: REFRESHTOKEN
	},
	getters: {
		// 获取网络状态
		networkStatus: state => {
			return state.networkState;
		},
		userInfo: state => {
			return state.userInfo;
		},
		// 判断用户是否登录
		hasLogin: state => {
			return !!state.accessToken;
		}
	},
	mutations: {
		login(state, provider) {
			state.accessToken = provider.access_token;
			state.refreshToken = provider.refresh_token;
			uni.setStorageSync('accessToken', provider.access_token);
			uni.setStorageSync('refreshToken', provider.refresh_token);
		},
		logout(state) {
			state.accessToken = '';
			state.refreshToken = '';
			state.userInfo = {};
			uni.removeStorageSync('accessToken');
			uni.removeStorageSync('refreshToken');
			uni.removeStorageSync('userInfo');
			uni.clearStorageSync();
		},
		setUserinfo(state, provider) {
			uni.setStorageSync('userInfo', provider);
			state.userInfo = provider;
		},
		setNetworkState(state, provider) {
			state.networkState = provider;
		}
	},
	actions: {
		networkStateChange({
			commit
		}, info) {
			commit('setNetworkState', info);
		},
		logout({
			commit
		}) {
			commit('logout');
		}
	}
});

export default store;