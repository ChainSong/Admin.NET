import request from '@/utils/request'

let PageRFReceiptReceiving = '/api/wMSRFReceiptReceiving/ShelveOrder';
let ScanReceiptReceiving = '/api/wMSRFReceiptReceiving/ScanReceiptReceiving';

 

// 查询未上架的订单
export const getRFReceiptReceivingApi = (params) =>
	request({
		url: PageRFReceiptReceiving,
		method: 'post',
		data: params,
	});
// 查询未上架的订单
export const scanReceiptReceivingApi = (params) =>
	request({
		url: ScanReceiptReceiving,
		method: 'post',
		data: params,
	});
// // 删除WMSRFReceiptAcquisition
// export const deleteWMSRFReceiptAcquisition = (params) => 
// 	request({
// 			url: DeleteWMSRFReceiptAcquisition,
// 			method: 'post',
// 			data: params,
// 		});

// // 编辑WMSRFReceiptAcquisition
// export const updateWMSRFReceiptAcquisition = (params) => 
// 	request({
// 			url: UpdateWMSRFReceiptAcquisition,
// 			method: 'post',
// 			data: params,
// 		});

// // 分页查询WMSRFReceiptAcquisition
// export const pageWMSRFReceiptAcquisition = (params) => 
// 	request({
// 			url: PageWMSRFReceiptAcquisition,
// 			method: 'post',
// 			data: params,
// 		});
// // 单条查询WMSRFReceiptAcquisition
// export const getWMSRFReceiptAcquisition = (params) => 
// request({
// 	url: `${GetWMSRFReceiptAcquisition}/${params}`,
// 	method: 'get'
// });


// // 编辑WMSRFReceiptAcquisition
// export const allWMSRFReceiptAcquisition = (params) => 
// 	request({
// 			url:AllWMSRFReceiptAcquisition,
// 			method: 'post',
// 			data: params,
// 		});

// 		// 编辑WMSRFReceiptAcquisition
// export const saveAcquisition = (params) => 
// request({
// 		url: SaveAcquisitionData,
// 		method: 'post',
// 		data: params,
// 	});