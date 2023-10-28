import request from '/@/utils/request';
enum Api {
  AddWMSInventoryUsable = '/api/wMSInventoryUsable/add',
  DeleteWMSInventoryUsable = '/api/wMSInventoryUsable/delete',
  UpdateWMSInventoryUsable = '/api/wMSInventoryUsable/update',
  PageWMSInventoryUsable = '/api/wMSInventoryUsable/page',
  GetWMSInventoryUsable = '/api/wMSInventoryUsable/Query',
}

// 增加WMSInventory
export const addWMSInventoryUsable = (params?: any) =>
	request({
		url: Api.AddWMSInventoryUsable,
		method: 'post',
		data: params,
	});

// 删除WMSInventory
export const deleteWMSInventoryUsable = (params?: any) => 
	request({
			url: Api.DeleteWMSInventoryUsable,
			method: 'post',
			data: params,
		});

// 编辑WMSInventory
export const updateWMSInventoryUsable = (params?: any) => 
	request({
			url: Api.UpdateWMSInventoryUsable,
			method: 'post',
			data: params,
		});

// 分页查询WMSInventory
export const pageWMSInventoryUsable = (params?: any) => 
	request({
			url: Api.PageWMSInventoryUsable,
			method: 'post',
			data: params,
		});
// 单条查询WMSInventory
export const getWMSInventoryUsable = (params?: any) => 
request({
	url: `${Api.GetWMSInventoryUsable}/${params}`,
	method: 'get'
});



