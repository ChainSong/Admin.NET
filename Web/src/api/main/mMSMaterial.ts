import request from '/@/utils/request';
enum Api {
  AddMMSMaterial = '/api/mMSMaterial/add',
  DeleteMMSMaterial = '/api/mMSMaterial/delete',
  UpdateMMSMaterial = '/api/mMSMaterial/update',
  PageMMSMaterial = '/api/mMSMaterial/page',
  GetMMSMaterial = '/api/mMSMaterial/Query',
}

// 增加MMS_Material
export const addMMSMaterial = (params?: any) =>
	request({
		url: Api.AddMMSMaterial,
		method: 'post',
		data: params,
	});

// 删除MMS_Material
export const deleteMMSMaterial = (params?: any) => 
	request({
			url: Api.DeleteMMSMaterial,
			method: 'post',
			data: params,
		});

// 编辑MMS_Material
export const updateMMSMaterial = (params?: any) => 
	request({
			url: Api.UpdateMMSMaterial,
			method: 'post',
			data: params,
		});

// 分页查询MMS_Material
export const pageMMSMaterial = (params?: any) => 
	request({
			url: Api.PageMMSMaterial,
			method: 'post',
			data: params,
		});
// 单条查询MMS_Material
export const getMMSMaterial = (params?: any) => 
request({
	url: `${Api.GetMMSMaterial}/${params}`,
	method: 'get'
});



