import request from '/@/utils/request';
enum Api {
  AddWMSASN = '/api/wMSASN/add',
  DeleteWMSASN = '/api/wMSASN/delete',
  UpdateWMSASN = '/api/wMSASN/update',
  PageWMSASN = '/api/wMSASN/page',
  GetWMSASN = '/api/wMSASN/Query',
  ASNForReceipt = '/api/wMSASN/ASNForReceipt',
  ASNForReceiptPart = '/api/wMSASN/ASNForReceiptPart',
  ExportASN = '/api/wMSASN/ExportASN',
  CancelASN = '/api/wMSASN/Cancel',
  
}

// 增加WMSASN
export const addWMSASN = (params?: any) =>
	request({
		url: Api.AddWMSASN,
		method: 'post',
		data: params,
	});

// 删除WMSASN
export const deleteWMSASN = (params?: any) => 
	request({
			url: Api.DeleteWMSASN,
			method: 'post',
			data: params,
		});

// 删除WMSASN
export const cancelWMSASN = (params?: any) => 
	request({
			url: Api.CancelASN,
			method: 'post',
			data: params,
		});
		
// 编辑WMSASN
export const updateWMSASN = (params?: any) => 
	request({
			url: Api.UpdateWMSASN,
			method: 'post',
			data: params,
		});

// 分页查询WMSASN
export const pageWMSASN = (params?: any) => 
	request({
			url: Api.PageWMSASN,
			method: 'post',
			data: params,
		});
// 单条查询WMSASN
export const getWMSASN = (params?: any) => 
request({
	url: `${Api.GetWMSASN}/${params}`,
	method: 'get'
});

//转入库单
export const asnForReceipt = (params?: any) => 
	request({
			url: Api.ASNForReceipt,
			method: 'post',
			data: params,
		});

//部分转入库单
export const asnForReceiptPart = (params?: any) => 
	request({
			url: Api.ASNForReceiptPart,
			method: 'post',
			data: params,
		});
// 导出出库信息
export const exportASN = (params?: any) =>
	request({
		url: Api.ExportASN,
		method: 'post',
		data: params,
		responseType: 'blob',
	});

	
		