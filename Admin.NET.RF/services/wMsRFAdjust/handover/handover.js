import request from '@/utils/request'

let pendHandoverListApi = '/api/wMSRFHandover/getPendingHandoverOrder';
let scanPackageApi = '/api/wMSRFHandover/scanPackageNumber';
 let completeHandoverApi='/api/wMSRFHandover/submitHandover';
//  查询wMSAdjustmentMove
export const pendHandoverList = (params) =>
	request({
		url: pendHandoverListApi,
		method: 'post',
		data: params,
	});


//  查询wMSAdjustmentMove
export const scanPackage = (params) =>
	request({
		url: scanPackageApi,
		method: 'post',
		data: params,
	});


//  查询wMSAdjustmentMove
export const completeHandover = (params) =>
	request({
		url: completeHandoverApi,
		method: 'post',
		data: params,
	});
