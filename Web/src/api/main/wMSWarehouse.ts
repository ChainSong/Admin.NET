import request from '/@/utils/request';
enum Api {
  AddWMSWarehouse = '/api/wMSWarehouse/add',
  DeleteWMSWarehouse = '/api/wMSWarehouse/delete',
  UpdateWMSWarehouse = '/api/wMSWarehouse/update',
  PageWMSWarehouse = '/api/wMSWarehouse/page',
  GetWMSWarehouse = '/api/wMSWarehouse/Query',
  AllWMSWarehouse = '/api/wMSWarehouse/All',
  SelectWarehouse = '/api/wMSWarehouse/SelectWarehouse',
  GetShelf = '/api/wMSWarehouse/GetShelf',
}

// 增加WMSWarehouse
export const addWMSWarehouse = (params?: any) =>
	request({
		url: Api.AddWMSWarehouse,
		method: 'post',
		data: params,
	});

// 删除WMSWarehouse
export const deleteWMSWarehouse = (params?: any) => 
	request({
			url: Api.DeleteWMSWarehouse,
			method: 'post',
			data: params,
		});

// 编辑WMSWarehouse
export const updateWMSWarehouse = (params?: any) => 
	request({
			url: Api.UpdateWMSWarehouse,
			method: 'post',
			data: params,
		});

// 分页查询WMSWarehouse
export const pageWMSWarehouse = (params?: any) => 
	request({
			url: Api.PageWMSWarehouse,
			method: 'post',
			data: params,
		});

// 查询单条Warehouse
export const getWMSWarehouse = (params?: any) => 
request({
	url: `${Api.GetWMSWarehouse}/${params}`,
	method: 'get'
});		
// 查询单条Warehouse
export const allWMSWarehouse = (params?: any) => 
request({
	url: `${Api.AllWMSWarehouse}`,
	method: 'get'
});

// selectWarehouse
export const selectWarehouse = (params?: any) => 
	request({
		url: Api.SelectWarehouse,
		method: 'post',
		data: params,
	});
	// selectWarehouse
export const getShelf = (params?: any) => 
	request({
		url: Api.GetShelf,
		method: 'post',
		data: params,
	});
