import request from '/@/utils/request';
enum Api {
  AddWMSCustomer = '/api/wMSCustomer/add',
  DeleteWMSCustomer = '/api/wMSCustomer/delete',
  UpdateWMSCustomer = '/api/wMSCustomer/update',
  PageWMSCustomer = '/api/wMSCustomer/page',
  GetWMSCustomer = '/api/wMSCustomer/query',
  AllWMSCustomer = '/api/wMSCustomer/all',
  
}

// 增加Customer
export const addWMSCustomer = (params?: any) =>
	request({
		url: Api.AddWMSCustomer,
		method: 'post',
		data: params,
	});

// 删除Customer
export const deleteWMSCustomer = (params?: any) => 
	request({
			url: Api.DeleteWMSCustomer,
			method: 'post',
			data: params,
		});

// 编辑Customer
export const updateWMSCustomer = (params?: any) => 
	request({
			url: Api.UpdateWMSCustomer,
			method: 'post',
			data: params,
		});

// 分页查询Customer
export const pageWMSCustomer = (params?: any) => 
	request({
			url: Api.PageWMSCustomer,
			method: 'post',
			data: params,
		});

		// 查询所有客户
export const allWMSCustomer = (params?: any) => 
request({
	url: `${Api.AllWMSCustomer}`,
	method: 'get'
});

// 查询单条Customer
export const getWMSCustomer = (params?: any) => 
request({
	url: `${Api.GetWMSCustomer}/${params}`,
	method: 'get'
});
	// request({
	// 		url: Api.GetWMSCustomer,
	// 		method: 'get',
	// 		data: params,
	// 	});
