import request from '/@/utils/request';
enum Api {
  AddWMSPickTask = '/api/wMSPickTask/add',
  DeleteWMSPickTask = '/api/wMSPickTask/delete',
  UpdateWMSPickTask = '/api/wMSPickTask/update',
  PageWMSPickTask = '/api/wMSPickTask/page',
  GetWMSPickTask = '/api/wMSPickTask/Query',
  AddWMSPickTaskPrintLog = '/api/wMSPickTask/AddPrintLog',
  WMSPickComplete = '/api/wMSPickTask/WMSPickComplete',
  GetPickTasks = '/api/wMSPickTask/GetPickTasks',
  
}

// 增加WMSPickTask
export const addWMSPickTask = (params?: any) =>
	request({
		url: Api.AddWMSPickTask,
		method: 'post',
		data: params,
	});

// 删除WMSPickTask
export const deleteWMSPickTask = (params?: any) => 
	request({
			url: Api.DeleteWMSPickTask,
			method: 'post',
			data: params,
		});

// 编辑WMSPickTask
export const updateWMSPickTask = (params?: any) => 
	request({
			url: Api.UpdateWMSPickTask,
			method: 'post',
			data: params,
		});

// 分页查询WMSPickTask
export const pageWMSPickTask = (params?: any) => 
	request({
			url: Api.PageWMSPickTask,
			method: 'post',
			data: params,
		});
// 单条查询WMSPickTask
export const getWMSPickTask = (params?: any) => 
request({
	url: `${Api.GetWMSPickTask}/${params}`,
	method: 'get'
});

// 多条查询WMSPickTask
export const getPickTasks = (params?: any) => 
request({
	url: Api.GetPickTasks,
	method: 'post',
	data: params,
});


// 添加打印记录
export const addWMSPickTaskPrintLog = (params?: any) => 
request({
	url: Api.AddWMSPickTaskPrintLog,
	method: 'post',
	data: params,
});

// 单条查询WMSPickTask
export const wmsPickComplete = (params?: any) => 
request({
	url: Api.WMSPickComplete,
	method: 'post',
	data: params,
});
