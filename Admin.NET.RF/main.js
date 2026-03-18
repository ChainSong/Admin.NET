import Vue from 'vue'
import App from './App'
// 引入全局存储
import store from '@/store';

import basics from './pages/basics/home.vue'
Vue.component('basics',basics)

import components from './pages/component/home.vue'
Vue.component('components',components)

import about from './pages/about/home.vue'
Vue.component('about',about)

import cuCustom from './colorui/components/cu-custom.vue'
Vue.component('cu-custom',cuCustom)

import config from '@/config/index.js';

Vue.config.productionTip = false
// 挂载全局自定义方法
Vue.prototype.$mStore = store;
Vue.prototype.$mConfig = config;

// 网络状态监听
uni.getNetworkType({
	success: res => {
		store.dispatch('networkStateChange', res.networkType);
	}
});
uni.onNetworkStatusChange(function (res) {
	store.dispatch('networkStateChange', res.networkType);
});

App.mpType = 'app'

const app = new Vue({
    ...App,
	store: store
})
app.$mount()

 



