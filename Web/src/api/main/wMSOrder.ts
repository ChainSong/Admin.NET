import request from '/@/utils/request';
enum Api {
	AddWMSOrder = '/api/wMSOrder/add',
	DeleteWMSOrder = '/api/wMSOrder/delete',
	UpdateWMSOrder = '/api/wMSOrder/update',
	PageWMSOrder = '/api/wMSOrder/page',
	GetWMSOrder = '/api/wMSOrder/Query',
	AutomatedAllocation = '/api/wMSOrder/AutomatedAllocation',
	CreatePickTask = '/api/wMSOrder/CreatePickTask',
	CompleteOrder = '/api/wMSOrder/CompleteOrder',
	ExportOrder = '/api/wMSOrder/ExportOrder',
	PrintShippingList = '/api/wMSOrder/PrintShippingList',
	ExportPackage = '/api/wMSOrder/ExportPackage',
	GetOrderLocation = '/api/wMSOrder/GetOrderLocation',
	ExportWMSOrderByRFID = '/api/WMSOrderReport/ExportWMSOrderByRFID',
	PrintJobList='/api/wMSOrder/PrintJobList'
}

// 增加WMSOrder
export const addWMSOrder = (params?: any) =>
	request({
		url: Api.AddWMSOrder,
		method: 'post',
		data: params,
	});

// 导出包装清单
export const exportPackage = (params?: any) =>
	request({
		url: Api.ExportPackage,
		method: 'post',
		data: params,
		responseType: 'blob',
	});

// 删除WMSOrder
export const deleteWMSOrder = (params?: any) =>
	request({
		url: Api.DeleteWMSOrder,
		method: 'post',
		data: params,
	});

// 编辑WMSOrder
export const updateWMSOrder = (params?: any) =>
	request({
		url: Api.UpdateWMSOrder,
		method: 'post',
		data: params,
	});

// 分页查询WMSOrder
export const pageWMSOrder = (params?: any) =>
	request({
		url: Api.PageWMSOrder,
		method: 'post',
		data: params,
	});
// 单条查询WMSOrder
export const getWMSOrder = (params?: any) =>
	request({
		url: `${Api.GetWMSOrder}/${params}`,
		method: 'get'
	});


//分配
export const automatedAllocation = (params?: any) =>
	request({
		url: Api.AutomatedAllocation,
		method: 'post',
		data: params,
	});

//创建拣货任务
export const createPickTask = (params?: any) =>
	request({
		url: Api.CreatePickTask,
		method: 'post',
		data: params,
	});

//完成订单
export const completeOrder = (params?: any) =>
	request({
		url: Api.CompleteOrder,
		method: 'post',
		data: params,
	});


//打印拣货任务
export const printShippingList = (params?: any) =>
	request({
		url: Api.PrintShippingList,
		method: 'post',
		data: params,
	});


// 导出出库信息
export const exportOrder = (params?: any) =>
	request({
		url: Api.ExportOrder,
		method: 'post',
		data: params,
		responseType: 'blob',
	});
// 导出出库信息
export const getOrderLocation = (params?: any) =>
	request({
		url: `${Api.GetOrderLocation}/${params}`,
		method: 'get'
	});

// 导出出库信息
export const exportWMSOrderByRFID = (params?: any) =>
	request({
		url: Api.ExportWMSOrderByRFID,
		method: 'post',
		data: params,
		responseType: 'blob',
	});
	
	//打印JOB汇总清单
export const printJobList = (params?: any) =>
	request({
		url: Api.PrintJobList,
		method: 'post',
		data: params,
	});
