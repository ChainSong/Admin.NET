// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Core.Entity;
using Admin.NET.Core.Service;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos; 

namespace Admin.NET.Application.Service.WMSExpress.Interface;
public interface IExpressInterface
{
    public SqlSugarRepository<Admin.NET.Core.Entity.WMSPackage> _repPackage { get; set; }
    public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
    public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }
    public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

    public SqlSugarRepository<WMSOrderAddress> _repOrderAddress { get; set; }
    public SqlSugarRepository<WMSWarehouse> _repWarehouse { get; set; }
    public UserManager _userManager { get; set; }
    //public ISqlSugarClient _db { get; set; }
    public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }
    public SysCacheService _sysCacheService { get; set; }

    public SqlSugarRepository<WMSExpressDelivery> _repExpressDelivery { get; set; }
    public SqlSugarRepository<WMSExpressConfig> _repExpressConfig { get; set; }
    public SqlSugarRepository<WMSExpressFee> _repWMSExpressFee { get; set; }


    //private readonly SqlSugarRepository<WMSExpressFee> _repWMSExpressFee;

    //获取快递信息 包括快递单号 三段码等信息
    Task<Response> GetExpressData(ScanPackageInput request);

    //获取快递信息 包括快递单号 三段码等信息(子母单)
    Task<Response> GetExpressDataList(ScanPackageInput request);

    //云打印需要的方法
    Task<Response<dynamic>> PrintExpressData(ScanPackageInput request);
    Task<Response<dynamic>> PrintBatchExpressDataByPackageId(List<long> request);
    
    //获取token
    Task<Response<dynamic>> GetExpressConfig(ScanPackageInput request);



}
