import request from '/@/utils/request';
enum Api {
  AddMMSReceipt = '/api/mMSReceipt/add',
  DeleteMMSReceipt = '/api/mMSReceipt/delete',
  UpdateMMSReceipt = '/api/mMSReceipt/update',
  PageMMSReceipt = '/api/mMSReceipt/page',
  GetMMSReceipt = '/api/mMSReceipt/Query',
  ExportReceipt = '/api/mMSReceipt/ExportReceipt',
  ExportReceiptReceiving = '/api/mMSReceipt/ExportReceiptReceiving',
  GetReceipts = '/api/mMSReceipt/GetReceipts',
}

// 增加MMSReceipt
export const addMMSReceipt = (params?: any) =>
	request({
		url: Api.AddMMSReceipt,
		method: 'post',
		data: params,
	});

// 删除MMSReceipt
export const deleteMMSReceipt = (params?: any) => 
	request({
			url: Api.DeleteMMSReceipt,
			method: 'post',
			data: params,
		});

// 编辑MMSReceipt
export const updateMMSReceipt = (params?: any) => 
	request({
			url: Api.UpdateMMSReceipt,
			method: 'post',
			data: params,
		});

// 分页查询MMSReceipt
export const pageMMSReceipt = (params?: any) => 
	request({
			url: Api.PageMMSReceipt,
			method: 'post',
			data: params,
		});
// 单条查询MMSReceipt
export const getMMSReceipt = (params?: any) => 
request({
	url: `${Api.GetMMSReceipt}/${params}`,
	method: 'get'
});


// 单条查询WMSReceipt
export const getWMSReceipt = (params?: any) =>
	request({
		url: `${Api.GetMMSReceipt}/${params}`,
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

