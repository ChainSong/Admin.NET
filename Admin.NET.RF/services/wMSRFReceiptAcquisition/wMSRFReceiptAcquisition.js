import request from '@/utils/request'

let AddWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/add';
let DeleteWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/delete';
let UpdateWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/update';
let PageWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/page';
let GetWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/Query';
let AllWMSRFReceiptAcquisition = '/api/wMSRFReceiptAcquisition/All';
let SaveAcquisitionData = '/api/wMSRFReceiptAcquisition/SaveAcquisitionData';
  
 

// 增加WMSRFReceiptAcquisition
export const addWMSRFReceiptAcquisition = (params) =>
	request({
		url: AddWMSRFReceiptAcquisition,
		method: 'post',
		data: params,
	});

// 删除WMSRFReceiptAcquisition
export const deleteWMSRFReceiptAcquisition = (params) => 
	request({
			url: DeleteWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 编辑WMSRFReceiptAcquisition
export const updateWMSRFReceiptAcquisition = (params) => 
	request({
			url: UpdateWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

// 分页查询WMSRFReceiptAcquisition
export const pageWMSRFReceiptAcquisition = (params) => 
	request({
			url: PageWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});
// 单条查询WMSRFReceiptAcquisition
export const getWMSRFReceiptAcquisition = (params) => 
request({
	url: `${GetWMSRFReceiptAcquisition}/${params}`,
	method: 'get'
});


// 编辑WMSRFReceiptAcquisition
export const allWMSRFReceiptAcquisition = (params) => 
	request({
			url:AllWMSRFReceiptAcquisition,
			method: 'post',
			data: params,
		});

		// 编辑WMSRFReceiptAcquisition
export const saveAcquisition = (params) => 
request({
		url: SaveAcquisitionData,
		method: 'post',
		data: params,
	});