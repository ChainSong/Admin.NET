﻿import request from '/@/utils/request';
enum Api {
  AddWMSOrder = '/api/wMSOrder/add',
  DeleteWMSOrder = '/api/wMSOrder/delete',
  UpdateWMSOrder = '/api/wMSOrder/update',
  PageWMSOrder = '/api/wMSOrder/page',
  GetWMSOrder = '/api/wMSOrder/Query',
  AutomatedAllocation = '/api/wMSOrder/AutomatedAllocation',
}

// 增加WMSOrder
export const addWMSOrder = (params?: any) =>
	request({
		url: Api.AddWMSOrder,
		method: 'post',
		data: params,
	});

// 删除WMSOrder
export const deleteWMSOrder = (params?: any) => 
	request({
			url: Api.DeleteWMSOrder,
			method: 'post',
			data: params,
		});

// 编辑WMSOrder
export const updateWMSOrder = (params?: any) => 
	request({
			url: Api.UpdateWMSOrder,
			method: 'post',
			data: params,
		});

// 分页查询WMSOrder
export const pageWMSOrder = (params?: any) => 
	request({
			url: Api.PageWMSOrder,
			method: 'post',
			data: params,
		});
// 单条查询WMSOrder
export const getWMSOrder = (params?: any) => 
request({
	url: `${Api.GetWMSOrder}/${params}`,
	method: 'get'
});


 //转入库单
 export const automatedAllocation = (params?: any) => 
 request({
		 url: Api.AutomatedAllocation,
		 method: 'post',
		 data: params,
	 });
 
 