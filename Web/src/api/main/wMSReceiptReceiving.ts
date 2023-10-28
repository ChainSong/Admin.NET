import request from '/@/utils/request';
enum Api {
	AddWMSReceipt = '/api/WMSReceiptReceiving/add',
	DeleteWMSReceipt = '/api/WMSReceiptReceiving/delete',
	UpdateWMSReceipt = '/api/WMSReceiptReceiving/update',
	PageWMSReceipt = '/api/WMSReceiptReceiving/page',
	GetWMSReceipt = '/api/WMSReceiptReceiving/Query',
	ReturnReceiptReceiving = '/api/WMSReceiptReceiving/ReturnReceiptReceiving',
	AddInventory = '/api/WMSReceiptReceiving/addInventory',
	
}

// 增加WMSReceipt
export const addWMSReceipt = (params?: any) =>
	request({
		url: Api.AddWMSReceipt,
		method: 'post',
		data: params,
	});

// 删除WMSReceipt
export const deleteWMSReceipt = (params?: any) =>
	request({
		url: Api.DeleteWMSReceipt,
		method: 'post',
		data: params,
	});

// 编辑WMSReceipt
export const updateWMSReceipt = (params?: any) =>
	request({
		url: Api.UpdateWMSReceipt,
		method: 'post',
		data: params,
	});

// 分页查询WMSReceipt
export const pageWMSReceipt = (params?: any) =>
	request({
		url: Api.PageWMSReceipt,
		method: 'post',
		data: params,
	});
// 单条查询WMSReceipt
export const getWMSReceipt = (params?: any) =>
	request({
		url: `${Api.GetWMSReceipt}/${params}`,
		method: 'get'
	});

// 上架回退
export const returnReceiptReceiving = (params?: any) =>
	request({
		url: `${Api.ReturnReceiptReceiving}`,
		method: 'post',
		data: params,
	});

	// 加入库存
export const addInventory = (params?: any) =>
request({
	url: `${Api.AddInventory}`,
	method: 'post',
	data: params,
});


	