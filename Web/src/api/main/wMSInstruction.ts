import request from '/@/utils/request';
enum Api {
  AddWMSInstruction = '/api/wMSInstruction/add',
  DeleteWMSInstruction = '/api/wMSInstruction/delete',
  UpdateWMSInstruction = '/api/wMSInstruction/update',
  PageWMSInstruction = '/api/wMSInstruction/page',
  GetWMSInstruction = '/api/wMSInstruction/Query',
}

// 增加WMSInstruction
export const addWMSInstruction = (params?: any) =>
	request({
		url: Api.AddWMSInstruction,
		method: 'post',
		data: params,
	});

// 删除WMSInstruction
export const deleteWMSInstruction = (params?: any) => 
	request({
			url: Api.DeleteWMSInstruction,
			method: 'post',
			data: params,
		});

// 编辑WMSInstruction
export const updateWMSInstruction = (params?: any) => 
	request({
			url: Api.UpdateWMSInstruction,
			method: 'post',
			data: params,
		});

// 分页查询WMSInstruction
export const pageWMSInstruction = (params?: any) => 
	request({
			url: Api.PageWMSInstruction,
			method: 'post',
			data: params,
		});
// 单条查询WMSInstruction
export const getWMSInstruction = (params?: any) => 
request({
	url: `${Api.GetWMSInstruction}/${params}`,
	method: 'get'
});



