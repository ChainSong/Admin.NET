import request from '/@/utils/request';
enum Api {
  CustomerSelectList = '/api/hachReport/CustomerSelectList',

  QueryOperationalTracker = '/api/hachReport/OperationalTrackerList',
  ExportOperationalTracker = '/api/hachReport/ExportOperationalTrackerList',
  
  QueryOperationalTrackerSellThru = '/api/hachReport/OperationalTrackerSellThruList',
  ExportOperationalTrackerSellThru = '/api/hachReport/ExportOperationalTrackerSellThruList'
}
// 分页查询客户用户关系
export const GetCustomerSelectList = () =>
  request({
    url: Api.CustomerSelectList,
    method: 'post',
  });

// 查询经销商配件库存健康度
export const QueryOperationalTrackerList = (params?: any) =>
  request({
    url: Api.QueryOperationalTracker,
    method: 'post',
    data: params,
  });

// 导出经销商配件库存健康度
export const ExportOperationalTrackerList = (params?: any) =>
  request({
    url: Api.ExportOperationalTracker,
    method: 'post',
    data: params,
    responseType: 'blob',
  });

  // 查询经销商配件
export const QueryOperationalTrackerSellThruList = (params?: any) =>
  request({
    url: Api.QueryOperationalTrackerSellThru,
    method: 'post',
    data: params,
  });
  
// 导出经销商配件
export const ExportOperationalTrackerSellThruList = (params?: any) =>
  request({
    url: Api.ExportOperationalTrackerSellThru,
    method: 'post',
    data: params,
    responseType: 'blob',
  });

