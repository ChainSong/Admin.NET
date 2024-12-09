import request from '@/utils/request'

let PageWMSRFStockCheck= '/api/wMSRFStockCheck/page';

// let ScanPick= '/api/wMSASNCountQuantity/ScanPick';
// let ScanOrderPickTask= '/api/wMSASNCountQuantity/ScanOrderPickTask';

// 分页查询WMSRFReceiptAcquisition
export const pageWMSRFStockCheckApi = (params) => 
	request({
			url: PageWMSRFStockCheck,
			method: 'post',
			data: params,
		}); 
		