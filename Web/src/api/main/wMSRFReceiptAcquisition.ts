import request from '/@/utils/request';
enum Api {
  AddWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/add',
  DeleteWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/delete',
  UpdateWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/update',
  PageWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/page',
  GetWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/Query',
  AllWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/All',
  SaveAcquisitionData = '/api/wMSRFReceiptAcquisition/SaveAcquisitionData',
  
}

// 增加WMSRFReceiptAcquisition
export const addWMSRFReceiptAcquisition = (params?: any) =>
	request({
		url: Api.AddWMSRFReceiptAcquisition,
		method: 'post',
		data: params,
	});

// 删除WMSRFReceiptAcquisition
export const deleteWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.DeleteWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 编辑WMSRFReceiptAcquisition
export const updateWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.UpdateWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 分页查询WMSRFReceiptAcquisition
export const pageWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.PageWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});
// 单条查询WMSRFReceiptAcquisition
export const getWMSRFReceiptAcquisition = (params?: any) => 
request({
	url: `${Api.GetWMSRFReceiptAcquisition}/${params}`,
	method: 'get'
});


// 编辑WMSRFReceiptAcquisition
export const allWMSRFReceiptAcquisition = (params?: any) => 
	request({
			url: Api.AllWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

		// 编辑WMSRFReceiptAcquisition
export const saveAcquisition = (params?: any) => 
request({
		url: Api.SaveAcquisitionData,
		method: 'post',
		data: params,
	});