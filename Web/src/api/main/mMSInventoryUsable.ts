import request from '/@/utils/request';
enum Api {
  AddMMSInventoryUsable = '/api/mMSInventoryUsable/add',
  DeleteMMSInventoryUsable = '/api/mMSInventoryUsable/delete',
  UpdateMMSInventoryUsable = '/api/mMSInventoryUsable/update',
  PageMMSInventoryUsable = '/api/mMSInventoryUsable/page',
  GetMMSInventoryUsable = '/api/mMSInventoryUsable/Query',
}

// 增加MMSInventoryUsable
export const addMMSInventoryUsable = (params?: any) =>
	request({
		url: Api.AddMMSInventoryUsable,
		method: 'post',
		data: params,
	});

// 删除MMSInventoryUsable
export const deleteMMSInventoryUsable = (params?: any) => 
	request({
			url: Api.DeleteMMSInventoryUsable,
			method: 'post',
			data: params,
		});

// 编辑MMSInventoryUsable
export const updateMMSInventoryUsable = (params?: any) => 
	request({
			url: Api.UpdateMMSInventoryUsable,
			method: 'post',
			data: params,
		});

// 分页查询MMSInventoryUsable
export const pageMMSInventoryUsable = (params?: any) => 
	request({
			url: Api.PageMMSInventoryUsable,
			method: 'post',
			data: params,
		});
// 单条查询MMSInventoryUsable
export const getMMSInventoryUsable = (params?: any) => 
request({
	url: `${Api.GetMMSInventoryUsable}/${params}`,
	method: 'get'
});



