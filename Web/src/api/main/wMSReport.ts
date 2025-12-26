import request from '/@/utils/request';
enum Api {
	AddWMSRFIDInfo = '/api/wMSRFIDInfo/add',
	GetReceiptSN = '/api/wMsReport/GetWMSRFReceiptAcquisitionPageList',
	GetPackageSN = '/api/wMsReport/GetWMSRFPackageAcquisitionPageList',
	ExportReceiptSN = '/api/wMsReport/ExportWMSRFReceiptAcquisition',
	ExportPackageSN = '/api/wMsReport/ExportWMSRFPackageAcquisition',
}

// 增加WMSRFIDInfo
export const addWMSRFIDInfo = (params?: any) =>
	request({
		url: Api.AddWMSRFIDInfo,
		method: 'post',
		data: params,
	});

// 查询入库SN列表
export const getReceiptSNPageList = (params?: any) =>
	request({
		url: Api.GetReceiptSN,
		method: 'post',
		data: params,
	});


// 查询出库/包装 SN列表
export const getPackageSNPageList = (params?: any) =>
	request({
		url: Api.GetPackageSN,
		method: 'post',
		data: params,
	});

// 导出入库序列号
export const exportReceiptSN = (params?: any) =>
	request({
		url: Api.ExportReceiptSN,
		method: 'post',
		data: params,
		responseType: 'blob',
	});

// 导出出库(包装)序列号
export const exportPackageSN = (params?: any) =>
	request({
		url: Api.ExportPackageSN,
		method: 'post',
		data: params,
		responseType: 'blob',
	});