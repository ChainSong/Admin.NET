import request from '/@/utils/request';
enum Api {
  AddWMSASNReceiptDetail = '/api/wMSASNReceiptDetail/add',
  DeleteWMSASNReceiptDetail = '/api/wMSASNReceiptDetail/delete',
  UpdateWMSASNReceiptDetail = '/api/wMSASNReceiptDetail/update',
  PageWMSASNReceiptDetail = '/api/wMSASNReceiptDetail/page',
  GetWMSASNReceiptDetail = '/api/wMSASNReceiptDetail/Query',
}

// 增加入库点数
export const addWMSASNReceiptDetail = (params?: any) =>
	request({
		url: Api.AddWMSASNReceiptDetail,
		method: 'post',
		data: params,
	});

// 删除入库点数
export const deleteWMSASNReceiptDetail = (params?: any) => 
	request({
			url: Api.DeleteWMSASNReceiptDetail,
			method: 'post',
			data: params,
		});

// 编辑入库点数
export const updateWMSASNReceiptDetail = (params?: any) => 
	request({
			url: Api.UpdateWMSASNReceiptDetail,
			method: 'post',
			data: params,
		});

// 分页查询入库点数
export const pageWMSASNReceiptDetail = (params?: any) => 
	request({
			url: Api.PageWMSASNReceiptDetail,
			method: 'post',
			data: params,
		});
// 单条查询入库点数
export const getWMSASNReceiptDetail = (params?: any) => 
request({
	url: `${Api.GetWMSASNReceiptDetail}/${params}`,
	method: 'get'
});



