import request from '/@/utils/request';
enum Api {
  AddWMSLowCode = '/api/wMSLowCode/add',
  DeleteWMSLowCode = '/api/wMSLowCode/delete',
  UpdateWMSLowCode = '/api/wMSLowCode/update',
  PageWMSLowCode = '/api/wMSLowCode/page',
  GetWMSLowCode = '/api/wMSLowCode/Query',
  GetData = '/api/WMSLowCode/GetData',
//   AddLowCode = '/api/WMSLowCode/Add',
//   GetLowCode = '/api/WMSLowCode/Query', 
  InitializeLowCode= '/api/WMSLowCode/Initialize',
}

// 增加WMSLowCode
export const addWMSLowCode = (params?: any) =>
	request({
		url: Api.AddWMSLowCode,
		method: 'post',
		data: params,
	});

// 删除WMSLowCode
export const deleteWMSLowCode = (params?: any) => 
	request({
			url: Api.DeleteWMSLowCode,
			method: 'post',
			data: params,
		});

// 编辑WMSLowCode
export const updateWMSLowCode = (params?: any) => 
	request({
			url: Api.UpdateWMSLowCode,
			method: 'post',
			data: params,
		});

// 分页查询WMSLowCode
export const pageWMSLowCode = (params?: any) => 
	request({
			url: Api.PageWMSLowCode,
			method: 'post',
			data: params,
		});
// 单条查询WMSLowCode
export const getWMSLowCode = (params?: any) => 
request({
	url: `${Api.GetWMSLowCode}/${params}`,
	method: 'get'
});




// 查询单条
export const initializeLowCode = (params?: any) => 
request({
	url: `${Api.InitializeLowCode}/${params}`,
	method: 'get'
});
	

// 查询单条
// export const getLowCode = (params?: any) => 
// request({
// 	url: `${Api.GetLowCode}/${params}`,
// 	method: 'get'
// });
		
		
// // 增加
// export const addLowCode = (params?: any) =>
// 	request({
// 		url: Api.AddLowCode,
// 		method: 'post',
// 		data: params,
// 	});


// 
export const getData = (params?: any) =>
	request({
		url: Api.GetData,
		method: 'post',
		data: params,
	});

