import request from '/@/utils/request';
enum Api {
  AddWMSASNCountQuantity = '/api/wMSASNCountQuantity/add',
  DeleteWMSASNCountQuantity = '/api/wMSASNCountQuantity/delete',
  UpdateWMSASNCountQuantity = '/api/wMSASNCountQuantity/update',
  PageWMSASNCountQuantity = '/api/wMSASNCountQuantity/page',
  GetWMSASNCountQuantity = '/api/wMSASNCountQuantity/Query',
}

// 增加入库点数
export const addWMSASNCountQuantity = (params?: any) =>
	request({
		url: Api.AddWMSASNCountQuantity,
		method: 'post',
		data: params,
	});

// 删除入库点数
export const deleteWMSASNCountQuantity = (params?: any) => 
	request({
			url: Api.DeleteWMSASNCountQuantity,
			method: 'post',
			data: params,
		});

// 编辑入库点数
export const updateWMSASNCountQuantity = (params?: any) => 
	request({
			url: Api.UpdateWMSASNCountQuantity,
			method: 'post',
			data: params,
		});

// 分页查询入库点数
export const pageWMSASNCountQuantity = (params?: any) => 
	request({
			url: Api.PageWMSASNCountQuantity,
			method: 'post',
			data: params,
		});
// 单条查询入库点数
export const getWMSASNCountQuantity = (params?: any) => 
request({
	url: `${Api.GetWMSASNCountQuantity}/${params}`,
	method: 'get'
});

 
