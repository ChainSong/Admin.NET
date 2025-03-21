import request from '/@/utils/request';
enum Api {
  AddWMSPackage = '/api/wMSPackage/add',
  DeleteWMSPackage = '/api/wMSPackage/delete',
  UpdateWMSPackage = '/api/wMSPackage/update',
  PageWMSPackage = '/api/wMSPackage/page',
  GetWMSPackage = '/api/wMSPackage/Query',
  ScanPackageData = '/api/wMSPackage/ScanPackageData',
  ScanPackageData_RFID = '/api/wMSPackage/ScanPackageData_RFID',
  PrintExpress = '/api/wMSPackage/PrintExpress',
  PrintBaatchExpress = '/api/wMSPackage/PrintBatchExpress',
  AllWMSPackage = '/api/wMSPackage/All',
  AddPackageData = '/api/wMSPackage/AddPackage',
  ShortagePackageData = '/api/wMSPackage/ShortagePackage',
  ResetPackageData = '/api/wMSPackage/ResetPackageData',
  GetRFIDInfo = '/api/wMSPackage/GetRFIDInfo',
  ExportPackage = '/api/wMSPackage/ExportPackage',
  PrintPackageList = '/api/wMSPackage/PrintPackageList',
  
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
export const scanPackageData_RFID = (params?: any) => 
	request({
			url: Api.ScanPackageData_RFID,
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
// 导出一个名为 printBaatchExpress 的常量，它是一个函数
export const printBatchExpress = (params?: any) => 
	// 调用 request 函数，发送一个 HTTP 请求
	request({
		// 请求的 URL 地址，使用 Api.PrintBaatchExpress 常量
		url: Api.PrintBaatchExpress,
		// 请求的方法类型，这里使用 POST 方法
		method: 'post',
		// 请求的数据，传入的参数 params
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

// 导出出库信息
export const exportPackage = (params?: any) =>
	request({
		url: Api.ExportPackage,
		method: 'post',
		data: params,
		responseType: 'blob',
	});
	

//获取RFID信息
export const printPackageList = (params?: any) => 
	request({
		url: Api.PrintPackageList,
		method: 'post',
		data: params,
	});		
	