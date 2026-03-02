import request from '/@/utils/request';
enum Api {
  AddWMSPackageLable = '/api/wMSPackageLable/add',
  DeleteWMSPackageLable = '/api/wMSPackageLable/delete',
  UpdateWMSPackageLable = '/api/wMSPackageLable/update',
  PageWMSPackageLable = '/api/wMSPackageLable/page',
  GetWMSPackageLable = '/api/wMSPackageLable/Query',
  PrintBoxNumber = '/api/wMSPackageLable/PrintBoxNumber',
}

// 增加WMSPackageLable
export const addWMSPackageLable = (params?: any) =>
	request({
		url: Api.AddWMSPackageLable,
		method: 'post',
		data: params,
	});

// 删除WMSPackageLable
export const deleteWMSPackageLable = (params?: any) => 
	request({
			url: Api.DeleteWMSPackageLable,
			method: 'post',
			data: params,
		});

// 编辑WMSPackageLable
export const updateWMSPackageLable = (params?: any) => 
	request({
			url: Api.UpdateWMSPackageLable,
			method: 'post',
			data: params,
		});

// 分页查询WMSPackageLable
export const pageWMSPackageLable = (params?: any) => 
	request({
			url: Api.PageWMSPackageLable,
			method: 'post',
			data: params,
		});
// 单条查询WMSPackageLable
export const getWMSPackageLable = (params?: any) => 
request({
	url: `${Api.GetWMSPackageLable}/${params}`,
	method: 'get'
});

// 打印箱号
export const printBoxNumber = (params?: any) => 
	request({
			url: Api.PrintBoxNumber,
			method: 'post',
			data: params,
		});

