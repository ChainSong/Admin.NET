import request from '/@/utils/request';
enum Api {
  AddSupplierUserMapping = '/api/supplierUserMapping/add',
  DeleteSupplierUserMapping = '/api/supplierUserMapping/delete',
  UpdateSupplierUserMapping = '/api/supplierUserMapping/update',
  PageSupplierUserMapping = '/api/supplierUserMapping/page',
  GetSupplierUserMapping = '/api/supplierUserMapping/Query',
  ListSupplierUserMapping = '/api/supplierUserMapping/List',
  
}

// 增加SupplierUserMapping
export const addSupplierUserMapping = (params?: any) =>
	request({
		url: Api.AddSupplierUserMapping,
		method: 'post',
		data: params,
	});

// 删除SupplierUserMapping
export const deleteSupplierUserMapping = (params?: any) => 
	request({
			url: Api.DeleteSupplierUserMapping,
			method: 'post',
			data: params,
		});

// 编辑SupplierUserMapping
export const updateSupplierUserMapping = (params?: any) => 
	request({
			url: Api.UpdateSupplierUserMapping,
			method: 'post',
			data: params,
		});

// 分页查询SupplierUserMapping
export const pageSupplierUserMapping = (params?: any) => 
	request({
			url: Api.PageSupplierUserMapping,
			method: 'post',
			data: params,
		});
// 单条查询SupplierUserMapping
export const getSupplierUserMapping = (params?: any) => 
request({
	url: `${Api.GetSupplierUserMapping}/${params}`,
	method: 'get'
});



// 分页查询供应商用户关系
export const listSupplierUserMapping = (params?: any) => 
	request({
			url: Api.ListSupplierUserMapping,
			method: 'post',
			data: params,
		});

