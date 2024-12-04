import request from '/@/utils/request';
enum Api {
  AddWMSPackage = '/api/wMSPackage/add',
  DeleteWMSPackage = '/api/wMSPackage/delete',
  UpdateWMSPackage = '/api/wMSPackage/update',
  PageWMSPackage = '/api/wMSPackage/page',
  GetWMSPackage = '/api/wMSPackage/Query',
  ScanPackageData = '/api/wMSPackage/ScanPackageData',
  PrintExpress = '/api/wMSPackage/PrintExpress',
  AllWMSPackage = '/api/wMSPackage/All',
  AddPackageData = '/api/wMSPackage/AddPackage',
  ShortagePackageData = '/api/wMSPackage/ShortagePackage',
  ResetPackageData = '/api/wMSPackage/resetPackage',
  GetRFIDInfo = '/api/wMSPackage/GetRFIDInfo',
  
}

// 增加WMSPackage
export const addWMSPackage = (params?: any) =>
	request({
		url: Api.AddWMSPackage,
		method: 'post',
		data: params,
	});

// 删除WMSPackage
export const deleteWMSPackage = (params?: any) => 
	request({
			url: Api.DeleteWMSPackage,
			method: 'post',
			data: params,
		});

// 编辑WMSPackage
export const updateWMSPackage = (params?: any) => 
	request({
			url: Api.UpdateWMSPackage,
			method: 'post',
			data: params,
		});

// 分页查询WMSPackage
export const pageWMSPackage = (params?: any) => 
	request({
			url: Api.PageWMSPackage,
			method: 'post',
			data: params,
		});
// 分页查询WMSPackage
export const scanPackageData = (params?: any) => 
	request({
			url: Api.ScanPackageData,
			method: 'post',
			data: params,
		});		

		// 分页查询WMSPackage
export const shortagePackageData = (params?: any) => 
	request({
			url: Api.ScanPackageData,
			method: 'post',
			data: params,
		});		
		
// 分页查询WMSPackage
export const resetPackageData = (params?: any) => 
	request({
			url: Api.ResetPackageData,
			method: 'post',
			data: params,
		});				
		
		
// 单条查询WMSPackage
export const getWMSPackage = (params?: any) => 
request({
	url: `${Api.GetWMSPackage}/${params}`,
	method: 'get'
});



	// 新增箱
export const addPackageData = (params?: any) => 
request({
		url: Api.AddPackageData,
		method: 'post',
		data: params,
	});		

// 单条查询WMSPackage
export const printExpressData = (params?: any) => 
request({
	url: Api.PrintExpress,
	method: 'post',
	data: params,
});		
//获取所有包装信息
export const allWMSPackage = (params?: any) => 
request({
	url: Api.AllWMSPackage,
	method: 'post',
	data: params,
});		


//获取RFID信息
export const getRFIDInfo = (params?: any) => 
	request({
		url: Api.GetRFIDInfo,
		method: 'post',
		data: params,
	});		