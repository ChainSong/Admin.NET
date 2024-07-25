import request from '/@/utils/request';
enum Api {
	AllRead = '/api/SysNotice/AllRead', 
}

// 全部已读
export const allReadApi = (params?: any) => 
request({
	url: `${Api.AllRead}`,
	method: 'get'
});
	

		