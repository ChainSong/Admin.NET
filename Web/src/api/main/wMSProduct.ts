import request from '/@/utils/request';
enum Api {
  AddWMSProduct = '/api/wMSProduct/add',
  DeleteWMSProduct = '/api/wMSProduct/delete',
  UpdateWMSProduct = '/api/wMSProduct/update',
  PageWMSProduct = '/api/wMSProduct/page',
  GetWMSProduct = '/api/wMSProduct/Query',
}

// 增加WMSProduct
export const addWMSProduct = (params?: any) =>
	request({
		url: Api.AddWMSProduct,
		method: 'post',
		data: params,
	});

// 删除WMSProduct
export const deleteWMSProduct = (params?: any) => 
	request({
			url: Api.DeleteWMSProduct,
			method: 'post',
			data: params,
		});

// 编辑WMSProduct
export const updateWMSProduct = (params?: any) => 
	request({
			url: Api.UpdateWMSProduct,
			method: 'post',
			data: params,
		});

// 分页查询WMSProduct
export const pageWMSProduct = (params?: any) => 
	request({
			url: Api.PageWMSProduct,
			method: 'post',
			data: params,
		});
// 单条查询WMSProduct
export const getWMSProduct = (params?: any) => 
request({
	url: `${Api.GetWMSProduct}/${params}`,
	method: 'get'
});



