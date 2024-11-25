import request from '@/utils/request'

let PageWMSRFASNCountQuantity= '/api/wMSRFASNCountQuantity/page';
let AddWMSRFASNCountQuantityDetail= '/api/wMSRFASNCountQuantity/scanAdd';
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
export const addWMSRFASNCountQuantityDetail = (params) => 
	request({
			url: AddWMSRFASNCountQuantityDetail,
			method: 'post',
			data: params,
		}); 		
		 