import Vue from 'vue'
import App from './App'
// 引入全局存储
import store from '@/store';
import * as uni from '@/components/gprint/uni.webview.1.5.8.js'; // 路径根据实际调整
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

document.addEventListener('UniAppJSBridgeReady', function() {
  Vue.prototype.$uni = uni; // Vue2
  // 或在 Vue3 中挂载到 app.config.globalProperties.$uni
  console.log('uni.webview.js 初始化成功');
});
App.mpType = 'app'

const app = new Vue({
    ...App,
	store: store
})
app.$mount()

 



