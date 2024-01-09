import request from '/@/utils/request';
enum Api {
  AddSysWorkFlow = '/api/sysWorkFlow/add',
  DeleteSysWorkFlow = '/api/sysWorkFlow/delete',
  UpdateSysWorkFlow = '/api/sysWorkFlow/update',
  PageSysWorkFlow = '/api/sysWorkFlow/page',
  GetSysWorkFlow = '/api/sysWorkFlow/Query',
  GetUser = '/api/sysWorkFlow/GetUser',
}

// 增加SysWorkFlow
export const addSysWorkFlow = (params?: any) =>
	request({
		url: Api.AddSysWorkFlow,
		method: 'post',
		data: params,
	});

// 删除SysWorkFlow
export const deleteSysWorkFlow = (params?: any) => 
	request({
			url: Api.DeleteSysWorkFlow,
			method: 'post',
			data: params,
		});

// 编辑SysWorkFlow
export const updateSysWorkFlow = (params?: any) => 
	request({
			url: Api.UpdateSysWorkFlow,
			method: 'post',
			data: params,
		});

// 分页查询SysWorkFlow
export const pageSysWorkFlow = (params?: any) => 
	request({
			url: Api.PageSysWorkFlow,
			method: 'post',
			data: params,
		});
// 单条查询SysWorkFlow
export const getSysWorkFlow = (params?: any) => 
request({
	url: `${Api.GetSysWorkFlow}/${params}`,
	method: 'get'
});

// 单条查询SysWorkFlow
export const getUser = (params?: any) => 
request({
	url: `${Api.GetUser}`,
	method: 'get'
});



