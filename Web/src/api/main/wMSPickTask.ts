import request from '/@/utils/request';
enum Api {
  AddWMSPickTask = '/api/wMSPickTask/add',
  DeleteWMSPickTask = '/api/wMSPickTask/delete',
  UpdateWMSPickTask = '/api/wMSPickTask/update',
  PageWMSPickTask = '/api/wMSPickTask/page',
  GetWMSPickTask = '/api/wMSPickTask/Query',
}

// 增加WMSPickTask
export const addWMSPickTask = (params?: any) =>
	request({
		url: Api.AddWMSPickTask,
		method: 'post',
		data: params,
	});

// 删除WMSPickTask
export const deleteWMSPickTask = (params?: any) => 
	request({
			url: Api.DeleteWMSPickTask,
			method: 'post',
			data: params,
		});

// 编辑WMSPickTask
export const updateWMSPickTask = (params?: any) => 
	request({
			url: Api.UpdateWMSPickTask,
			method: 'post',
			data: params,
		});

// 分页查询WMSPickTask
export const pageWMSPickTask = (params?: any) => 
	request({
			url: Api.PageWMSPickTask,
			method: 'post',
			data: params,
		});
// 单条查询WMSPickTask
export const getWMSPickTask = (params?: any) => 
request({
	url: `${Api.GetWMSPickTask}/${params}`,
	method: 'get'
});



