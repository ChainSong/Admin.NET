import request from '/@/utils/request';
enum Api {
	AddWMSAdjustment = '/api/wMSAdjustment/add',
	DeleteWMSAdjustment = '/api/wMSAdjustment/delete',
	UpdateWMSAdjustment = '/api/wMSAdjustment/update',
	PageWMSAdjustment = '/api/wMSAdjustment/page',
	GetWMSAdjustment = '/api/wMSAdjustment/Query',
	ConfirmWMSAdjustment = '/api/wMSAdjustment/Confirm',
}

// 增加WMSAdjustment
export const addWMSAdjustment = (params?: any) =>
	request({
		url: Api.AddWMSAdjustment,
		method: 'post',
		data: params,
	});

// 删除WMSAdjustment
export const deleteWMSAdjustment = (params?: any) =>
	request({
		url: Api.DeleteWMSAdjustment,
		method: 'post',
		data: params,
	});

// 编辑WMSAdjustment
export const updateWMSAdjustment = (params?: any) =>
	request({
		url: Api.UpdateWMSAdjustment,
		method: 'post',
		data: params,
	});

// 分页查询WMSAdjustment
export const pageWMSAdjustment = (params?: any) => 
	request({
		url: Api.PageWMSAdjustment,
		method: 'post',
		data: params,
	});
 
// 单条查询WMSAdjustment
export const getWMSAdjustment = (params?: any) =>
	request({
		url: `${Api.GetWMSAdjustment}/${params}`,
		method: 'get'
	});

//执行库存调整单
export const confirmWMSAdjustment = (params?: any) =>
	request({
		url: Api.ConfirmWMSAdjustment,
		method: 'post',
		data: params,
	});





