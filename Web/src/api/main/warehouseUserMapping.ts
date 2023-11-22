import request from '/@/utils/request';
enum Api {
	AddWarehouseUserMapping = '/api/warehouseUserMapping/add',
	DeleteWarehouseUserMapping = '/api/warehouseUserMapping/delete',
	UpdateWarehouseUserMapping = '/api/warehouseUserMapping/update',
	PageWarehouseUserMapping = '/api/warehouseUserMapping/page',
	ListWarehouseUserMapping = '/api/warehouseUserMapping/List',
}

// 增加仓库用户关系
export const addWarehouseUserMapping = (params?: any) =>
	request({
		url: Api.AddWarehouseUserMapping,
		method: 'post',
		data: params,
	});

// 删除仓库用户关系
export const deleteWarehouseUserMapsping = (params?: any) =>
	request({
		url: Api.DeleteWarehouseUserMapping,
		method: 'post',
		data: params,
	});

// 编辑仓库用户关系
export const updateWarehouseUserMapping = (params?: any) =>
	request({
		url: Api.UpdateWarehouseUserMapping,
		method: 'post',
		data: params,
	});

// 分页查询仓库用户关系
export const pageWarehouseUserMapping = (params?: any) =>
	request({
		url: Api.PageWarehouseUserMapping,
		method: 'post',
		data: params,
	});

// 分页查询仓库用户关系
export const listWarehouseUserMapping = (params?: any) =>
	//   {
	request({
		url: Api.ListWarehouseUserMapping,
		method: 'post',
		data: params,
	});
//  	console.log(params);
//  }
