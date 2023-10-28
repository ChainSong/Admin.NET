import request from '/@/utils/request';
enum Api {
  AddCustomerUserMapping = '/api/customerUserMapping/add',
  DeleteCustomerUserMapping = '/api/customerUserMapping/delete',
  UpdateCustomerUserMapping = '/api/customerUserMapping/update',
  PageCustomerUserMapping = '/api/customerUserMapping/page',
  ListCustomerUserMapping = '/api/customerUserMapping/List',
}

// 增加客户用户关系
export const addCustomerUserMapping = (params?: any) =>
	request({
		url: Api.AddCustomerUserMapping,
		method: 'post',
		data: params,
	});

// 删除客户用户关系
export const deleteCustomerUserMapping = (params?: any) => 
	request({
			url: Api.DeleteCustomerUserMapping,
			method: 'post',
			data: params,
		});

// 编辑客户用户关系
export const updateCustomerUserMapping = (params?: any) => 
	request({
			url: Api.UpdateCustomerUserMapping,
			method: 'post',
			data: params,
		});

// 分页查询客户用户关系
export const pageCustomerUserMapping = (params?: any) => 
	request({
			url: Api.PageCustomerUserMapping,
			method: 'post',
			data: params,
		});

// 分页查询客户用户关系
export const listCustomerUserMapping = (params?: any) => 
	request({
			url: Api.ListCustomerUserMapping,
			method: 'post',
			data: params,
		});


		