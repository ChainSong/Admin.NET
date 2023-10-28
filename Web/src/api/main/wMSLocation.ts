import request from '/@/utils/request';
enum Api {
  AddwMSLocation = '/api/wMSLocation/add',
  DeletewMSLocation = '/api/wMSLocation/delete',
  UpdatewMSLocation = '/api/wMSLocation/update',
  PagewMSLocation = '/api/wMSLocation/page',
  GetwMSLocation = '/api/wMSLocation/Query',
}

// 增加WMSLocation
export const addWMSLocation = (params?: any) =>
	request({
		url: Api.AddwMSLocation,
		method: 'post',
		data: params,
	});

// 删除WMSLocation
export const deleteWMSLocation = (params?: any) => 
	request({
			url: Api.DeletewMSLocation,
			method: 'post',
			data: params,
		});

// 编辑WMSLocation
export const updateWMSLocation = (params?: any) => 
	request({
			url: Api.UpdatewMSLocation,
			method: 'post',
			data: params,
		});

// 分页查询WMSLocation
export const pageWMSLocation = (params?: any) => 
	request({
			url: Api.PagewMSLocation,
			method: 'post',
			data: params,
		});
// 单条查询WMSLocation
export const getWMSLocation = (params?: any) => 
request({
	url: `${Api.GetwMSLocation}/${params}`,
	method: 'get'
});



