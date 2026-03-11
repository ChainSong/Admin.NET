import request from '/@/utils/request';
enum Api {
  AddWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/add',
  DeleteWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/delete',
  UpdateWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/update',
  PageWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/page',
  GetWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/Query',
  ExportWMSRFPackageAcquisition = '/api/wMSRFPackageAcquisition/Export',
}

// 增加WMS_RFPackageAcquisition
export const addWMSRFPackageAcquisition = (params?: any) =>
	request({
		url: Api.AddWMSRFPackageAcquisition,
		method: 'post',
		data: params,
	});

// 删除WMS_RFPackageAcquisition
export const deleteWMSRFPackageAcquisition = (params?: any) => 
	request({
			url: Api.DeleteWMSRFPackageAcquisition,
			method: 'post',
			data: params,
		});

// 编辑WMS_RFPackageAcquisition
export const updateWMSRFPackageAcquisition = (params?: any) => 
	request({
			url: Api.UpdateWMSRFPackageAcquisition,
			method: 'post',
			data: params,
		});

// 分页查询WMS_RFPackageAcquisition
export const pageWMSRFPackageAcquisition = (params?: any) => 
	request({
			url: Api.PageWMSRFPackageAcquisition,
			method: 'post',
			data: params,
		});
// 单条查询WMS_RFPackageAcquisition
export const getWMSRFPackageAcquisition = (params?: any) =>
request({
	url: `${Api.GetWMSRFPackageAcquisition}/${params}`,
	method: 'get'
});

// 导出WMS_RFPackageAcquisition
export const exportWMSRFPackageAcquisition = (params?: any) =>
	request({
		url: Api.ExportWMSRFPackageAcquisition,
		method: 'post',
		data: params,
		responseType: 'blob',
	});



