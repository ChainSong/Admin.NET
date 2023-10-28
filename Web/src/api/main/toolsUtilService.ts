import request from '/@/utils/request';
// enum Api {
//   AddWMSArea = '/api/wMSArea/add',
// }

// 增加库区管理
export const getSelect = (_url:string,params?: any) =>
	request({
		url: _url,
		method: 'post',
		data: params,
	});
 