import request from '/@/utils/request';
enum Api {
  AddWMSExpressConfig = '/api/wMSExpressConfig/add',
  DeleteWMSExpressConfig = '/api/wMSExpressConfig/delete',
  UpdateWMSExpressConfig = '/api/wMSExpressConfig/update',
  PageWMSExpressConfig = '/api/wMSExpressConfig/page',
  GetWMSExpressConfig = '/api/wMSExpressConfig/Query',
  GetExpressConfig = '/api/wMSExpressConfig/GetExpressConfig',
  AllExpress = '/api/wMSExpressConfig/AllExpress',
}

// 增加WMSExpressConfig
export const addWMSExpressConfig = (params?: any) =>
	request({
		url: Api.AddWMSExpressConfig,
		method: 'post',
		data: params,
	});

// 删除WMSExpressConfig
export const deleteWMSExpressConfig = (params?: any) => 
	request({
			url: Api.DeleteWMSExpressConfig,
			method: 'post',
			data: params,
		});

// 编辑WMSExpressConfig
export const updateWMSExpressConfig = (params?: any) => 
	request({
			url: Api.UpdateWMSExpressConfig,
			method: 'post',
			data: params,
		});

// 分页查询WMSExpressConfig
export const pageWMSExpressConfig = (params?: any) => 
	request({
			url: Api.PageWMSExpressConfig,
			method: 'post',
			data: params,
		});
// 单条查询WMSExpressConfig
export const getWMSExpressConfig = (params?: any) => 
request({
	url: `${Api.GetWMSExpressConfig}/${params}`,
	method: 'get'
});

// 编辑WMSExpressConfig
export const getExpressConfig = (params?: any) => 
	request({
			url: Api.GetExpressConfig,
			method: 'post',
			data: params,
		});

// 单条查询WMSExpressConfig
export const allExpress = (params?: any) => 
request({
	url: `${Api.AllExpress}`,
	method: 'get'
});
		