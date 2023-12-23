import request from '/@/utils/request';
enum Api {
	AddMMSSupplier = '/api/mMSSupplier/add',
	DeleteMMSSupplier = '/api/mMSSupplier/delete',
	UpdateMMSSupplier = '/api/mMSSupplier/update',
	PageMMSSupplier = '/api/mMSSupplier/page',
	GetMMSSupplier = '/api/mMSSupplier/Query',
	AllMMSSupplier = '/api/mMSSupplier/All',

}

// 增加MMSSupplier
export const addMMSSupplier = (params?: any) =>
	request({
		url: Api.AddMMSSupplier,
		method: 'post',
		data: params,
	});

// 删除MMSSupplier
export const deleteMMSSupplier = (params?: any) =>
	request({
		url: Api.DeleteMMSSupplier,
		method: 'post',
		data: params,
	});

// 编辑MMSSupplier
export const updateMMSSupplier = (params?: any) =>
	request({
		url: Api.UpdateMMSSupplier,
		method: 'post',
		data: params,
	});

// 分页查询MMSSupplier
export const pageMMSSupplier = (params?: any) =>
	request({
		url: Api.PageMMSSupplier,
		method: 'post',
		data: params,
	});
// 单条查询MMSSupplier
export const getMMSSupplier = (params?: any) =>
	request({
		url: `${Api.GetMMSSupplier}/${params}`,
		method: 'get'
	});

// 查询所有供应商
export const allMMSSupplier = (params?: any) =>
	request({
		url: `${Api.AllMMSSupplier}`,
		method: 'get'
	});

