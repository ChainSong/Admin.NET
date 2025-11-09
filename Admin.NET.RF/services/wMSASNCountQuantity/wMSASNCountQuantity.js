import request from '@/utils/request'

let PageWMSRFASNCountQuantity= '/api/wMSRFASNCountQuantity/page';
let AddWMSRFASNCountQuantityDetail= '/api/wMSRFASNCountQuantity/scanAdd';
let ClearWMSRFASNCountQuantityDetail= '/api/wMSRFASNCountQuantity/clear';
let GetScanTypeWMSRFASNCountQuantityDetail= '/api/wMSRFASNCountQuantity/getScanType';

// let ScanPick= '/api/wMSASNCountQuantity/ScanPick';
// let ScanOrderPickTask= '/api/wMSASNCountQuantity/ScanOrderPickTask';

// 分页查询WMSRFReceiptAcquisition
export const pageWMSRFASNCountQuantity = (params) => 
	request({
			url: PageWMSRFASNCountQuantity,
			method: 'post',
			data: params,
		}); 
		
// 添加明细
export const clearWMSRFASNCountQuantityDetail = (params) => 
	request({
			url: ClearWMSRFASNCountQuantityDetail,
			method: 'post',
			data: params,
		}); 		

// 添加明细
export const addWMSRFASNCountQuantityDetail = (params) => 
	request({
			url: AddWMSRFASNCountQuantityDetail,
			method: 'post',
			data: params,
		}); 		
		 		
 export const getScanTypeWMSRFASNCountQuantityDetail = (params) =>
 	request({
 			url: GetScanTypeWMSRFASNCountQuantityDetail,
 			method: 'post',
 			data: params,
 		}); 	
				 