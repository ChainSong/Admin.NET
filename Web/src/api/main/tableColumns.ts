import request from '/@/utils/request';
enum Api {
	AddTableColumns = '/api/tableColumns/add',
	DeleteTableColumns = '/api/tableColumns/delete',
	UpdateTableColumns = '/api/tableColumns/update',
	PageTableColumns = '/api/tableColumns/page',
	PageAllTableColumns = '/api/tableColumns/pageAll',
	GetAllTableColumns = '/api/tableColumns/getAll',
	CleanTableColumnsCache = '/api/tableColumns/CleanTableColumnsCache',
	UpdateTableColumnsDetail = '/api/tableColumns/updateDetail',
	GetTableColumnsDetail = '/api/tableColumns/GetTableColumnsDetail',
	GetByTableNameList = '/api/tableColumns/GetByTableNameList',
	GetImportExcelTemplate = '/api/tableColumns/ImportExcelTemplate',

}

// 增加表管理
export const addTableColumns = (params?: any) =>
	request({
		url: Api.AddTableColumns,
		method: 'post',
		data: params,
	});

// 删除表管理
export const deleteTableColumns = (params?: any) =>
	request({
		url: Api.DeleteTableColumns,
		method: 'post',
		data: params,
	});

// 编辑表管理
export const updateTableColumns = (params?: any) =>
	request({
		url: Api.UpdateTableColumns,
		method: 'post',
		data: params,
	});
// 编辑表管理
export const updateTableColumnsDetail = (params?: any) =>
	request({
		url: Api.UpdateTableColumnsDetail,
		method: 'post',
		data: params,
	});

// 编辑表管理
export const getByTableNameList = async (params?: any) => {

	let data = { data: { result: [] } };
	// localStorage.setItem(params, null);

	let tableColumnsStorage = localStorage.getItem(params);
	if (tableColumnsStorage != null && tableColumnsStorage.length > 30) {
		return JSON.parse(tableColumnsStorage) as Array<data>;
	} else {
		let tableNameListData = await tableNameList(params);
		data.data.result = tableNameListData.data.result;
		localStorage.setItem(params, JSON.stringify(data));
		return data;
	}

}


const tableNameList = (params?: any) => {
	return request({
		url: `${Api.GetByTableNameList}/${params}`,
		method: 'get'
	});

}
// request({
// 	url: Api.GetByTableNameList,
// 	method: 'get',
// 	data: params,
// });

// 编辑表管理
export const getTableColumnsDetail = (params?: any) =>
	request({
		url: Api.GetTableColumnsDetail,
		method: 'post',
		data: params,
	});
// 分页查询表管理
export const pageTableColumns = (params?: any) =>
	request({
		url: Api.PageTableColumns,
		method: 'post',
		data: params,
	});

// 查询所有
export const getAllTableColumns = (params?: any) =>
	request({
		url: Api.GetAllTableColumns,
		method: 'post',
		data: params,
	});

// 直接查询所有的表名称
export const pageAllTableColumns = (params?: any) =>
	request({
		url: Api.PageAllTableColumns,
		method: 'post',
		data: params,
	});

// 清理表结构缓存
export const cleanTableColumnsCache = (params?: any) => {

	localStorage.setItem(params, null);
	request({
		url: Api.CleanTableColumnsCache,
		method: 'post',
		data: params,
	});
}
// 直接查询所有的表名称
export const getImportExcelTemplate = (params?: any) =>

	request({
		url: Api.GetImportExcelTemplate,
		method: 'post',
		data: params,
		responseType: 'blob',
	});
// export const GetImportExcelTemplate = (params?: any) =>
// async GetImportExcelTemplate(context: ActionContext<TableColumnsState, any>, payload: any) {
// 	//  Ajax.post('/api/services/app/Table_ColumnsDetail/GetAll',payload.data);
// 	Ajax.defaults.responseType = 'blob';
// 	let reponse = await Ajax.post('/api/services/app/Table_Columns/ImportExcelTemplate', payload.data);
// 	let blob = new Blob([reponse.data], {
// 		type: 'application/vnd.ms-excel'
// 	})
// 	let fileName = "导入模板" + '.xlsx'
// 	// 允许用户在客户端上保存文件
// 	if (window.navigator.msSaveOrOpenBlob) {
// 		navigator.msSaveBlob(blob, fileName)
// 	} else {
// 		var link = document.createElement('a')
// 		link.href = window.URL.createObjectURL(blob)
// 		link.download = fileName
// 		link.click()
// 		//释放内存
// 		window.URL.revokeObjectURL(link.href)
// 	}
// }