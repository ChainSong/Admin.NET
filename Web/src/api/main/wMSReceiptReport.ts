import request from '/@/utils/request';
enum Api {
  GetReceiptReport = '/api/wMSReceiptReport/GetReceiptReport',
}

// 增加WMSRFIDInfo
export const getReceiptReport = (params?: any) =>
	request({
		url: Api.GetReceiptReport,
		method: 'post',
		data: params,
		responseType: 'blob', // blob类型
	});