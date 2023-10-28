import request from '/@/utils/request';
enum Api {
  AddWMSArea = '/api/wMSArea/add',
  DeleteWMSArea = '/api/wMSArea/delete',
  UpdateWMSArea = '/api/wMSArea/update',
  PageWMSArea = '/api/wMSArea/page',
  GetWMSArea = '/api/wMSArea/Query',
}

// 增加库区管理
export const addWMSArea = (params?: any) =>
	request({
		url: Api.AddWMSArea,
		method: 'post',
		data: params,
	});

// 删除库区管理
export const deleteWMSArea = (params?: any) => 
	request({
			url: Api.DeleteWMSArea,
			method: 'post',
			data: params,
		});

// 编辑库区管理
export const updateWMSArea = (params?: any) => 
	request({
			url: Api.UpdateWMSArea,
			method: 'post',
			data: params,
		});

// 分页查询库区管理
export const pageWMSArea = (params?: any) => 
	request({
			url: Api.PageWMSArea,
			method: 'post',
			data: params,
		});

// 查询单条Customer
export const getWMSArea = (params?: any) => 
request({
	url: `${Api.GetWMSArea}/${params}`,
	method: 'get'
});
		