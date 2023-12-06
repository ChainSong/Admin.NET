import request from '/@/utils/request';
enum Api {

	QueryPrintTemplate = '/api/printTemplate/query',
	 
}
 
export const queryPrintTemplate = (params?: any) => {
	return request({
		url: `${Api.QueryPrintTemplate}/${params}`,
		method: 'get'
	});
} 