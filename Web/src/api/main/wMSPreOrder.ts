import request from '/@/utils/request';
enum Api {
  AddWMSPreOrder = '/api/wMSPreOrder/add',
  DeleteWMSPreOrder = '/api/wMSPreOrder/delete',
  UpdateWMSPreOrder = '/api/wMSPreOrder/update',
  PageWMSPreOrder = '/api/wMSPreOrder/page',
  GetWMSPreOrder = '/api/wMSPreOrder/Query',
  PreOrderForOrder = '/api/wMSPreOrder/PreOrderForOrder',
  
  
}

// 增加WMS_PreOrder
export const addWMSPreOrder = (params?: any) =>
	request({
		url: Api.AddWMSPreOrder,
		method: 'post',
		data: params,
	});

// 删除WMS_PreOrder
export const deleteWMSPreOrder = (params?: any) => 
	request({
			url: Api.DeleteWMSPreOrder,
			method: 'post',
			data: params,
		});

// 编辑WMS_PreOrder
export const updateWMSPreOrder = (params?: any) => 
	request({
			url: Api.UpdateWMSPreOrder,
			method: 'post',
			data: params,
		});

// 分页查询WMS_PreOrder
export const pageWMSPreOrder = (params?: any) => 
	request({
			url: Api.PageWMSPreOrder,
			method: 'post',
			data: params,
		});
// 单条查询WMS_PreOrder
export const getWMSPreOrder = (params?: any) => 
request({
	url: `${Api.GetWMSPreOrder}/${params}`,
	method: 'get'
});


 //转入库单
export const preOrderForOrder = (params?: any) => 
request({
		url: Api.PreOrderForOrder,
		method: 'post',
		data: params,
	});

