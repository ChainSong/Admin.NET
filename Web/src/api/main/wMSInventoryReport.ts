import request from '/@/utils/request';
enum Api {
 
	InvrntoryDataPage = '/api/WMSInventoryReport/InvrntoryDataPage',
	InvrntoryDataExport = '/api/WMSInventoryReport/InvrntoryDataExport',
	
}

// invrntoryDataPage
export const invrntoryDataPage = (params?: any) =>
	request({
		url: Api.InvrntoryDataPage,
		method: 'post',
		data: params,
	});

	// invrntoryDataPage
export const invrntoryDataExport = (params?: any) =>
request({
	url: Api.InvrntoryDataExport,
	method: 'post',
	data: params,
	responseType: 'blob',
});
	