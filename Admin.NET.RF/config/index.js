const CONFIG = {
	// 开发环境配置
	development: {
		assetsPath: 'http://localhost:5005/WebStatic', // 静态资源路径
		baseUrl: 'http://localhost:5005', // 后台接口请求地址
		filebaseUrl: 'http://localhost:5005', // 后台文件地址 
		hostUrl: '', // H5地址(前端运行地址)
		websocketUrl: '', // websocket服务端地址
		weixinAppId: '', // 微信公众号appid
		errorAudioPath:'/static/error.mp3',
		successAudioPath:'/static/success.mp3'
	},
	// 生产环境配置
	production: {
		assetsPath: 'https://nike.rbow-logistics.com.cn:8034/WebStatic', // 静态资源路径
		baseUrl: 'https://nike.rbow-logistics.com.cn:8034', // 后台接口请求地址
		filebaseUrl: 'https://nike.rbow-logistics.com.cn:8034', // 后台文件地址 
		hostUrl: '', // H5地址(前端运行地址)
		websocketUrl: '', // websocket服务端地址
		weixinAppId: '', // 微信公众号appid
		errorAudioPath:'/static/error.mp3',
		successAudioPath:'/static/success.mp3'
	}
};
export default CONFIG[process.env.NODE_ENV];