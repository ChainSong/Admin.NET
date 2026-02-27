import request from '@/utils/request'

let PageWMSRFOrderPick= '/api/WMSRFOrderPick/page';
let ScanPick= '/api/WMSRFOrderPick/ScanPick';
let ScanOrderPickTask= '/api/WMSRFOrderPick/ScanRFOrderPick';
let ScanBoxNumberCompletePackage= '/api/WMSRFOrderPick/ScanBoxNumberCompletePackage';
let GetPickTaskDetailsByLocation = '/api/WMSRFOrderPick/GetPickTaskDetailsByLocation';
let GetCurrentPickTask = '/api/WMSRFOrderPick/GetCurrentPickTask';
let ApplyPickTask = '/api/WMSRFOrderPick/ApplyPickTask';
let CompletePackage = '/api/WMSRFOrderPick/CompletePackage';


// 分页查询WMSRFReceiptAcquisition
export const pageWMSRFOrderPickApi = (params) =>
		request({
			url: PageWMSRFOrderPick,
			method: 'post',
			data: params,
		});



// 分页查询WMSRFReceiptAcquisition
export const scanPickApi = (params) =>
		request({
			url: ScanPick,
			method: 'post',
			data: params,
		});



// 分页查询WMSRFReceiptAcquisition
export const scanOrderPickTaskApi = (params) =>
		request({
			url: ScanOrderPickTask,
			method: 'post',
			data: params,
		});


// 扫描箱号完成包装
export const scanBoxNumberCompletePackageApi = (params) =>
		request({
			url: ScanBoxNumberCompletePackage,
			method: 'post',
			data: params,
		});

// 获取按库位排序的拣货明细
export const getPickTaskDetailsByLocationApi = (params) =>
		request({
			url: GetPickTaskDetailsByLocation,
			method: 'post',
			data: params,
		});

// 获取当前拣货任务
export const getCurrentPickTaskApi = (params) =>
		request({
			url: GetCurrentPickTask,
			method: 'post',
			data: params,
		});

// 申请拣货任务
export const applyPickTaskApi = (params) =>
		request({
			url: ApplyPickTask,
			method: 'post',
			data: params,
		});

// 完成包装
export const completePackageApi = (params) =>
		request({
			url: CompletePackage,
			method: 'post',
			data: params,
		});
