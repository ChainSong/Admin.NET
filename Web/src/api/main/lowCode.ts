import request from '/@/utils/request';
enum Api {
  GetData = '/api/WMSLowCode/GetData',
  AddLowCode = '/api/WMSLowCode/Add',
  GetLowCode = '/api/WMSLowCode/Query', 
  InitializeLowCode= '/api/WMSLowCode/Initialize',
}

// 查询单条
export const initializeLowCode = (params?: any) => 
request({
	url: `${Api.InitializeLowCode}/${params}`,
	method: 'get'
});
	

// 查询单条
export const getLowCode = (params?: any) => 
request({
	url: `${Api.GetLowCode}/${params}`,
	method: 'get'
});
		
		
// 增加
export const addLowCode = (params?: any) =>
	request({
		url: Api.AddLowCode,
		method: 'post',
		data: params,
	});


// 
export const getData = (params?: any) =>
	request({
		url: Api.GetData,
		method: 'post',
		data: params,
	});



		