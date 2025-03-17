import request from '/@/utils/request';
enum Api {
  AddSysWorkFlow = '/api/sysWorkFlow/add',
  DeleteSysWorkFlow = '/api/sysWorkFlow/delete',
  UpdateSysWorkFlow = '/api/sysWorkFlow/update',
  PageSysWorkFlow = '/api/sysWorkFlow/page',
  GetSysWorkFlow = '/api/sysWorkFlow/Query',
  GetUser = '/api/sysWorkFlow/GetUser',
  CopyWorkFlow = '/api/sysWorkFlow/CopyWorkFlow',
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
// 导出一个名为 getUser 的常量，它是一个函数
export const getUser = (params?: any) => 
// 调用 request 函数，传入一个配置对象
request({
	// 配置对象的 url 属性，使用模板字符串动态拼接 Api.GetUser
	url: `${Api.GetUser}`,
	// 配置对象的 method 属性，指定请求方法为 'get'
	method: 'get'
});



 
// 导出一个名为 copyWorkFlow 的常量，它是一个函数，用于复制工作流
export const copyWorkFlow = (params?: any) => 
	// 调用 request 函数发送 HTTP 请求
	request({
			// 请求的 URL 地址，使用 Api.CopyWorkFlow 常量
			url: Api.CopyWorkFlow,
			// 请求方法为 POST
			method: 'post',
			// 请求的数据，传入的参数 params
			data: params,
		});