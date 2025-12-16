import request from '/@/utils/request';
enum Api {
  AddFGFHOrder = '/api/fGFHOrder/add',
  DeleteFGFHOrder = '/api/fGFHOrder/delete',
  UpdateFGFHOrder = '/api/fGFHOrder/update',
  PageFGFHOrder = '/api/fGFHOrder/page',
  GetFGFHOrder = '/api/fGFHOrder/Query',
}

// 增加福光出库回传
export const addFGFHOrder = (params?: any) =>
	request({
		url: Api.AddFGFHOrder,
		method: 'post',
		data: params,
	});

// 删除福光出库回传
export const deleteFGFHOrder = (params?: any) => 
	request({
			url: Api.DeleteFGFHOrder,
			method: 'post',
			data: params,
		});

// 编辑福光出库回传
export const updateFGFHOrder = (params?: any) => 
	request({
			url: Api.UpdateFGFHOrder,
			method: 'post',
			data: params,
		});

// 分页查询福光出库回传
export const pageFGFHOrder = (params?: any) => 
	request({
			url: Api.PageFGFHOrder,
			method: 'post',
			data: params,
		});
// 单条查询福光出库回传
export const getFGFHOrder = (params?: any) => 
request({
	url: `${Api.GetFGFHOrder}/${params}`,
	method: 'get'
});



