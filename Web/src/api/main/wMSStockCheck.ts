import request from '/@/utils/request';
enum Api {
  AddWMSStockCheck = '/api/wMSStockCheck/add',
  DeleteWMSStockCheck = '/api/wMSStockCheck/delete',
  UpdateWMSStockCheck = '/api/wMSStockCheck/update',
  PageWMSStockCheck = '/api/wMSStockCheck/page',
  GetWMSStockCheck = '/api/wMSStockCheck/Query',
  GetStockCheckInventory = '/api/wMSStockCheck/GetStockCheckInventory', 
  
}

// 增加WMSStockCheck
export const addWMSStockCheck = (params?: any) =>
	request({
		url: Api.AddWMSStockCheck,
		method: 'post',
		data: params,
	});

// 删除WMSStockCheck
export const deleteWMSStockCheck = (params?: any) => 
	request({
			url: Api.DeleteWMSStockCheck,
			method: 'post',
			data: params,
		});

// 查询要盘点库存
export const getStockCheckInventory = (params?: any) => 
	request({
			url: Api.GetStockCheckInventory,
			method: 'post',
			data: params,
		});
		
// 编辑WMSStockCheck
export const updateWMSStockCheck = (params?: any) => 
	request({
			url: Api.UpdateWMSStockCheck,
			method: 'post',
			data: params,
		});

// 分页查询WMSStockCheck
export const pageWMSStockCheck = (params?: any) => 
	request({
			url: Api.PageWMSStockCheck,
			method: 'post',
			data: params,
		});
// 单条查询WMSStockCheck
export const getWMSStockCheck = (params?: any) => 
request({
	url: `${Api.GetWMSStockCheck}/${params}`,
	method: 'get'
});



