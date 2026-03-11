import request from '/@/utils/request';
enum Api {
  AddWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/add',
  DeleteWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/delete',
  UpdateWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/update',
  PageWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/page',
  GetWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/Query',
  ExportWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/Export',
}

// 增加WMS_RFReceiptAcquisition
export const addWMSRFReceiptAcquisition = (params?: any) =>
	request({
		url: Api.AddWMSRFReceiptAcquisition,
		method: 'post',
		data: params,
	});

// 删除WMS_RFReceiptAcquisition
export const deleteWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.DeleteWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 编辑WMS_RFReceiptAcquisition
export const updateWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.UpdateWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 分页查询WMS_RFReceiptAcquisition
export const pageWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.PageWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});
// 单条查询WMS_RFReceiptAcquisition
export const getWMSRFReceiptAcquisition = (params?: any) =>
request({
	url: `${Api.GetWMSRFReceiptAcquisition}/${params}`,
	method: 'get'
});

// 导出WMS_RFReceiptAcquisition
export const exportWMSRFReceiptAcquisition = (params?: any) =>
	request({
		url: Api.ExportWMSRFReceiptAcquisition,
		method: 'post',
		data: params,
		responseType: 'blob',
	});




