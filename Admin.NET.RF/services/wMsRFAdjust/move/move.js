import request from '@/utils/request'

let adjustList = '/api/wMSAdjustment/page';
let selectCustomer = '/api/wMsCustomer/selectCustomer'
let selectWarehouse = '/api/wMsWarehouse/selectWarehouse'
//  查询wMSAdjustmentMove
export const pageAdjustList = (params) =>
	request({
		url: adjustList,
		method: 'post',
		data: params,
	});

//  查询客户下拉列表
export const selectCustomerList = (params) =>
	request({
		url: selectCustomer,
		method: 'post',
		data: params,
	});

//  查询仓库下拉列表
export const selectWarehouseList = (params) =>
	request({
		url: selectWarehouse,
		method: 'post',
		data: params,
	});