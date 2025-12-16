import request from '/@/utils/request';
enum Api {
	AddWMSHachJMECode = '/api/wMSHachJMECode/add',
	DeleteWMSHachJMECode = '/api/wMSHachJMECode/delete',
	UpdateWMSHachJMECode = '/api/wMSHachJMECode/update',
	PageWMSHachJMECode = '/api/wMSHachJMECode/page',
	GetWMSHachJMECode = '/api/wMSHachJMECode/Query',
	Import = '/api/wMSHachJMECode/ImportExcel',
	ExportDemo = '/api/wMSHachJMECode/ExportDemo',
	PrintJME = '/api/wMSHachJMECode/PrintJME',
}

// 增加JME打印
export const addWMSHachJMECode = (params?: any) =>
	request({
		url: Api.AddWMSHachJMECode,
		method: 'post',
		data: params,
	});

// 删除JME打印
export const deleteWMSHachJMECode = (params?: any) =>
	request({
		url: Api.DeleteWMSHachJMECode,
		method: 'post',
		data: params,
	});

// 编辑JME打印
export const updateWMSHachJMECode = (params?: any) =>
	request({
		url: Api.UpdateWMSHachJMECode,
		method: 'post',
		data: params,
	});

// 编辑JME打印
export const printJME = (params?: any) =>
	request({
		url: Api.PrintJME,
		method: 'post',
		data: params,
	});
// 分页查询JME打印
export const pageWMSHachJMECode = (params?: any) =>
	request({
		url: Api.PageWMSHachJMECode,
		method: 'post',
		data: params,
	});
// 单条查询JME打印
export const getWMSHachJMECode = (params?: any) =>
	request({
		url: `${Api.GetWMSHachJMECode}/${params}`,
		method: 'get'
	});

// 导出包装清单
export const exportDemo = (params?: any) =>
	request({
		url: Api.ExportDemo,
		method: 'post',
		data: params,
		responseType: 'blob',
	});


