import config from '@/config/index';
const innerAudioContext = uni.createInnerAudioContext();
export function playSuccessSound() {
	innerAudioContext.autoplay = true;
	innerAudioContext.src = config.successAudioPath;
	innerAudioContext.onPlay(() => {
		console.log('开始播放');
	});
	innerAudioContext.onError((err) => {
		console.log(err);
	});
}
export function playErrorSound() {	
	innerAudioContext.autoplay = true;
	innerAudioContext.src = config.errorAudioPath;
	innerAudioContext.onPlay(() => {
		console.log('开始播放');
	});
	innerAudioContext.onError((err) => {
		console.log(err);
	});
}