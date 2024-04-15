import request from '/@/utils/request';
enum Api {
  AddWMSExpressDelivery = '/api/wMSExpressDelivery/add',
  DeleteWMSExpressDelivery = '/api/wMSExpressDelivery/delete',
  UpdateWMSExpressDelivery = '/api/wMSExpressDelivery/update',
  PageWMSExpressDelivery = '/api/wMSExpressDelivery/page',
  GetWMSExpressDelivery = '/api/wMSExpressDelivery/Query',
}

// 增加WMSExpressDelivery
export const addWMSExpressDelivery = (params?: any) =>
	request({
		url: Api.AddWMSExpressDelivery,
		method: 'post',
		data: params,
	});

// 删除WMSExpressDelivery
export const deleteWMSExpressDelivery = (params?: any) => 
	request({
			url: Api.DeleteWMSExpressDelivery,
			method: 'post',
			data: params,
		});

// 编辑WMSExpressDelivery
export const updateWMSExpressDelivery = (params?: any) => 
	request({
			url: Api.UpdateWMSExpressDelivery,
			method: 'post',
			data: params,
		});

// 分页查询WMSExpressDelivery
export const pageWMSExpressDelivery = (params?: any) => 
	request({
			url: Api.PageWMSExpressDelivery,
			method: 'post',
			data: params,
		});
// 单条查询WMSExpressDelivery
export const getWMSExpressDelivery = (params?: any) => 
request({
	url: `${Api.GetWMSExpressDelivery}/${params}`,
	method: 'get'
});



