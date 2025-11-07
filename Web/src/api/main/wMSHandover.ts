import request from '/@/utils/request';
enum Api {
  AddWMSHandover = '/api/wMSHandover/add',
  DeleteWMSHandover = '/api/wMSHandover/delete',
  UpdateWMSHandover = '/api/wMSHandover/update',
  PageWMSHandover = '/api/wMSHandover/page',
  GetWMSHandover = '/api/wMSHandover/Query',
}

// 增加WMSHandover
export const addWMSHandover = (params?: any) =>
	request({
		url: Api.AddWMSHandover,
		method: 'post',
		data: params,
	});

// 删除WMSHandover
export const deleteWMSHandover = (params?: any) => 
	request({
			url: Api.DeleteWMSHandover,
			method: 'post',
			data: params,
		});

// 编辑WMSHandover
export const updateWMSHandover = (params?: any) => 
	request({
			url: Api.UpdateWMSHandover,
			method: 'post',
			data: params,
		});

// 分页查询WMSHandover
export const pageWMSHandover = (params?: any) => 
	request({
			url: Api.PageWMSHandover,
			method: 'post',
			data: params,
		});
// 单条查询WMSHandover
export const getWMSHandover = (params?: any) => 
request({
	url: `${Api.GetWMSHandover}/${params}`,
	method: 'get'
});



