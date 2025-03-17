import request from '/@/utils/request';
enum Api {
  AddSysPresetQuery = '/api/sysPresetQuery/add',
  DeleteSysPresetQuery = '/api/sysPresetQuery/delete',
  UpdateSysPresetQuery = '/api/sysPresetQuery/update',
  PageSysPresetQuery = '/api/sysPresetQuery/page',
  GetSysPresetQuery = '/api/sysPresetQuery/Query',
  QueryByUser = '/api/sysPresetQuery/QueryByUser',
}

// 增加SysPresetQuery
// 导出一个名为 addSysPresetQuery 的常量，它是一个函数，用于发送添加系统预设查询的请求
export const addSysPresetQuery = (params?: any) =>
	// 调用 request 函数发送 HTTP 请求
	request({
		// 请求的 URL，使用 Api 对象中的 AddSysPresetQuery 属性
		url: Api.AddSysPresetQuery,
		// 请求方法为 POST
		method: 'post',
		// 请求的数据，传入的参数 params
		data: params,
	});

// 删除SysPresetQuery
// 导出一个名为 deleteSysPresetQuery 的常量，它是一个函数
export const deleteSysPresetQuery = (params?: any) => 
    // 调用 request 函数发送 HTTP 请求
	request({
        // 请求的 URL 地址，使用 Api.DeleteSysPresetQuery 常量
			url: Api.DeleteSysPresetQuery,
        // 请求方法为 POST
			method: 'post',
        // 请求的数据，传入的参数 params
			data: params,
		});

// 编辑SysPresetQuery
export const updateSysPresetQuery = (params?: any) => 
	request({
			url: Api.UpdateSysPresetQuery,
			method: 'post',
			data: params,
		});

// 分页查询SysPresetQuery
export const pageSysPresetQuery = (params?: any) => 
	request({
			url: Api.PageSysPresetQuery,
			method: 'post',
			data: params,
		});
// 单条查询SysPresetQuery
export const getSysPresetQuery = (params?: any) => 
request({
	url: `${Api.GetSysPresetQuery}/${params}`,
	method: 'get'
});

// 
// 导出一个名为 queryByUser 的常量，它是一个函数，用于根据用户信息进行查询
export const queryByUser = (params?: any) => 
	// 调用 request 函数，发送一个 HTTP 请求
	request({
			// 请求的 URL，使用 Api.QueryByUser 常量来指定
			url: Api.QueryByUser,
			// 请求的方法，这里使用 POST 方法
			method: 'post',
			// 请求的数据，传入的参数 params
			data: params,
		});

