import request from '@/utils/request'

let adjustListApi = '/api/wMSAdjustment/page';
let selectCustomerApi = '/api/wMsCustomer/selectCustomer'
let selectWarehouseApi = '/api/wMsWarehouse/selectWarehouse'
let checkScanValueApi = '/api/wMSRFAdjustMove/adjustMoveCheckScan'
let completeMoveApi = '/api/wMSRFAdjustMove/adjustMove'
//  查询wMSAdjustmentMove
export const pageAdjustList = (params) =>
	request({
		url: adjustListApi,
		method: 'post',
		data: params,
	});

//  查询客户下拉列表
export const selectCustomerList = (params) =>
	request({
		url: selectCustomerApi,
		method: 'post',
		data: params,
	});

//  查询仓库下拉列表
export const selectWarehouseList = (params) =>
	request({
		url: selectWarehouseApi,
		method: 'post',
		data: params,
	});

//  校验扫描值
export const checkScanValue = (params) =>
	request({
		url: checkScanValueApi,
		method: 'post',
		data: params,
	});
	
	
	//  校验扫描值
	export const completeMove = (params) =>
		request({
			url: completeMoveApi,
			method: 'post',
			data: params,
		});