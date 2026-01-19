import request from '/@/utils/request';
enum Api {
  AddWMSBoxType = '/api/wMSBoxType/add',
  DeleteWMSBoxType = '/api/wMSBoxType/delete',
  UpdateWMSBoxType = '/api/wMSBoxType/update',
  PageWMSBoxType = '/api/wMSBoxType/page',
  GetWMSBoxType = '/api/wMSBoxType/Query',
  SelectBoxType = '/api/wMSBoxType/SelectBoxType',
}

// 增加WMSBoxType
export const addWMSBoxType = (params?: any) =>
	request({
		url: Api.AddWMSBoxType,
		method: 'post',
		data: params,
	});

// 删除WMSBoxType
export const deleteWMSBoxType = (params?: any) => 
	request({
			url: Api.DeleteWMSBoxType,
			method: 'post',
			data: params,
		});

// 编辑WMSBoxType
export const updateWMSBoxType = (params?: any) => 
	request({
			url: Api.UpdateWMSBoxType,
			method: 'post',
			data: params,
		});

// 分页查询WMSBoxType
export const pageWMSBoxType = (params?: any) => 
	request({
			url: Api.PageWMSBoxType,
			method: 'post',
			data: params,
		});
// 单条查询WMSBoxType
export const getWMSBoxType = (params?: any) => 
request({
	url: `${Api.GetWMSBoxType}/${params}`,
	method: 'get'
});

export const selectBoxType = (params?: any) => 
	request({
			url: Api.SelectBoxType,
			method: 'post',
			data: params,
		});

