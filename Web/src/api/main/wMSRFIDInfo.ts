import request from '/@/utils/request';
enum Api {
  AddWMSRFIDInfo = '/api/wMSRFIDInfo/add',
  DeleteWMSRFIDInfo = '/api/wMSRFIDInfo/delete',
  UpdateWMSRFIDInfo = '/api/wMSRFIDInfo/update',
  PageWMSRFIDInfo = '/api/wMSRFIDInfo/page',
  GetWMSRFIDInfo = '/api/wMSRFIDInfo/Query',
  GetPrinrRFIDInfoByReceiptId = '/api/wMSRFIDInfo/QueryByReceiptId',
  reportScreen= '/api/wMSReport/reportScreen',
}

// 增加WMSRFIDInfo
export const addWMSRFIDInfo = (params?: any) =>
	request({
		url: Api.AddWMSRFIDInfo,
		method: 'post',
		data: params,
	});

// 删除WMSRFIDInfo
export const deleteWMSRFIDInfo = (params?: any) => 
	request({
			url: Api.DeleteWMSRFIDInfo,
			method: 'post',
			data: params,
		});

// 编辑WMSRFIDInfo
export const updateWMSRFIDInfo = (params?: any) => 
	request({
			url: Api.UpdateWMSRFIDInfo,
			method: 'post',
			data: params,
		});

// 分页查询WMSRFIDInfo
export const pageWMSRFIDInfo = (params?: any) => 
	request({
			url: Api.PageWMSRFIDInfo,
			method: 'post',
			data: params,
		});
// 单条查询WMSRFIDInfo
export const getWMSRFIDInfo = (params?: any) => 
request({
	url: `${Api.GetWMSRFIDInfo}/${params}`,
	method: 'get'
});


// 单条查询WMSRFIDInfo
export const getPrinrRFIDInfoByReceiptId = (params?: any) => 
	request({
		url: Api.GetPrinrRFIDInfoByReceiptId,
		method: 'post',
		data: params,
	});

// 大屏接口
export const reportScreen = () => 
	request({
		url: Api.reportScreen,
		method: 'post',
		// data: params,  params?: any
	});	