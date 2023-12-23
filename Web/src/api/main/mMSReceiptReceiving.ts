import request from '/@/utils/request';
enum Api {
  AddMMSReceiptReceiving = '/api/mMSReceiptReceiving/add',
  DeleteMMSReceiptReceiving = '/api/mMSReceiptReceiving/delete',
  UpdateMMSReceiptReceiving = '/api/mMSReceiptReceiving/update',
  PageMMSReceiptReceiving = '/api/mMSReceiptReceiving/page',
  GetMMSReceiptReceiving = '/api/mMSReceiptReceiving/Query',
}

// 增加MMSReceiptReceiving
export const addMMSReceiptReceiving = (params?: any) =>
	request({
		url: Api.AddMMSReceiptReceiving,
		method: 'post',
		data: params,
	});

// 删除MMSReceiptReceiving
export const deleteMMSReceiptReceiving = (params?: any) => 
	request({
			url: Api.DeleteMMSReceiptReceiving,
			method: 'post',
			data: params,
		});

// 编辑MMSReceiptReceiving
export const updateMMSReceiptReceiving = (params?: any) => 
	request({
			url: Api.UpdateMMSReceiptReceiving,
			method: 'post',
			data: params,
		});

// 分页查询MMSReceiptReceiving
export const pageMMSReceiptReceiving = (params?: any) => 
	request({
			url: Api.PageMMSReceiptReceiving,
			method: 'post',
			data: params,
		});
// 单条查询MMSReceiptReceiving
export const getMMSReceiptReceiving = (params?: any) => 
request({
	url: `${Api.GetMMSReceiptReceiving}/${params}`,
	method: 'get'
});



