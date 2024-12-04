import * as SignalR from '@microsoft/signalr';
import { ElNotification } from 'element-plus';
// import { getToken } from '/@/utils/axios-utils';

// 初始化SignalR对象
const connections = new SignalR.HubConnectionBuilder()
	.configureLogging(SignalR.LogLevel.Information)
	.withUrl(`${import.meta.env.VITE_RFID_WEBSOCKET}/echo`, { transport: SignalR.HttpTransportType.WebSockets, skipNegotiation: true })
	.withAutomaticReconnect({
		nextRetryDelayInMilliseconds: () => {
			return 5000; // 每5秒重连一次
		},
	})
	.build();

connections.keepAliveIntervalInMilliseconds = 15 * 1000; // 心跳检测15s
connections.serverTimeoutInMilliseconds = 30 * 60 * 1000; // 超时时间30m

// 启动连接
connections.start().then(() => {
	console.log('signalRCustom启动连接');
	// connections.invoke('negotiate', 'YourParameter');
}) .catch(err => console.log("signalRCustom SignalR Connection Error: ", err));
// 断开连接
connections.onclose(async () => {
	console.log('断开连接');
});
// 重连中
connections.onreconnecting(() => {
	ElNotification({
		title: '提示',
		message: '服务器已断线...',
		type: 'error',
		position: 'bottom-right',
	});
});
// 重连成功
connections.onreconnected(() => {
	console.log('重连成功');
});

connections.on('Echo', () => { });

export { connections as signalR };
