import request from '/@/utils/request';
enum Api {
  AddWMSInventoryReport = '/api/wMSInventoryReport/add',
  DeleteWMSInventoryReport = '/api/wMSInventoryReport/delete',
  UpdateWMSInventoryReport = '/api/wMSInventoryReport/update',
  PageWMSInventoryReport = '/api/wMSInventoryReport/page',
  GetWMSInventoryReport = '/api/wMSInventoryReport/Query',
}

// 增加WMSInventoryReport
export const addWMSInventoryReport = (params?: any) =>
	request({
		url: Api.AddWMSInventoryReport,
		method: 'post',
		data: params,
	});

// 删除WMSInventoryReport
export const deleteWMSInventoryReport = (params?: any) => 
	request({
			url: Api.DeleteWMSInventoryReport,
			method: 'post',
			data: params,
		});

// 编辑WMSInventoryReport
export const updateWMSInventoryReport = (params?: any) => 
	request({
			url: Api.UpdateWMSInventoryReport,
			method: 'post',
			data: params,
		});

// 分页查询WMSInventoryReport
export const pageWMSInventoryReport = (params?: any) => 
	request({
			url: Api.PageWMSInventoryReport,
			method: 'post',
			data: params,
		});
// 单条查询WMSInventoryReport
export const getWMSInventoryReport = (params?: any) => 
request({
	url: `${Api.GetWMSInventoryReport}/${params}`,
	method: 'get'
});



