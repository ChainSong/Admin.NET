import request from '/@/utils/request';
enum Api {
  AddWMSRFIDInfo = '/api/wMSRFIDInfo/add',
}

// 增加WMSRFIDInfo
export const addWMSRFIDInfo = (params?: any) =>
	request({
		url: Api.AddWMSRFIDInfo,
		method: 'post',
		data: params,
	});