import request from '/@/utils/request';
enum Api {
	AddWMSReceipt = '/api/wMSReceipt/add',
	DeleteWMSReceipt = '/api/wMSReceipt/delete',
	UpdateWMSReceipt = '/api/wMSReceipt/update',
	PageWMSReceipt = '/api/wMSReceipt/page',
	GetWMSReceipt = '/api/wMSReceipt/Query',
	ExportReceipt = '/api/wMSReceipt/ExportReceipt',
	ExportReceiptReceiving = '/api/wMSReceipt/ExportReceiptReceiving',
	GetReceipts = '/api/wMSReceipt/GetReceipts',
	// GetPrintRFIDData = '/api/wMSReceipt/GetPrintRFIDData',
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

// 按照ids 查询入库单
export const getReceipts = (params?: any) =>
	request({
		url: Api.GetReceipts,
		method: 'post',
		data: params,
	});


// 单条查询WMSReceipt
export const getWMSReceipt = (params?: any) =>
	request({
		url: `${Api.GetWMSReceipt}/${params}`,
		method: 'get'
	});



// 导出入库单
export const exportReceipt = (params?: any) =>
	request({
		url: Api.ExportReceipt,
		method: 'post',
		data: params,
		responseType: 'blob',
	});

// 导出上架信息
export const exportReceiptReceiving = (params?: any) =>
	request({
		url: Api.ExportReceiptReceiving,
		method: 'post',
		data: params,
		responseType: 'blob',
	});

	
// 按照ids 打印RFID
// export const getPrintRFIDData = (params?: any) =>
// 	request({
// 		url: Api.GetPrintRFIDData,
// 		method: 'post',
// 		data: params,
// 	});

