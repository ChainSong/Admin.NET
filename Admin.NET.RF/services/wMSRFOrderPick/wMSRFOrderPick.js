import request from '@/utils/request'

let PageWMSRFOrderPick= '/api/WMSRFOrderPick/page';
let ScanPick= '/api/WMSRFOrderPick/ScanPick';
let ScanOrderPickTask= '/api/WMSRFOrderPick/ScanOrderPickTask';
  
 
 

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