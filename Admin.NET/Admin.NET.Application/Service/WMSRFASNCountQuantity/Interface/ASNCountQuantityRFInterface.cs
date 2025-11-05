// 麻省理工学院许可证
//
// 版权所有 (c) 2021-2023 zuohuaijun，大名科技（天津）有限公司  联系电话/微信：18020030720  QQ：515096995
//
// 特此免费授予获得本软件的任何人以处理本软件的权利，但须遵守以下条件：在所有副本或重要部分的软件中必须包括上述版权声明和本许可声明。
//
// 软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// 在任何情况下，作者或版权持有人均不对任何索赔、损害或其他责任负责，无论是因合同、侵权或其他方式引起的，与软件或其使用或其他交易有关。

using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Service;
public interface ASNCountQuantityRFInterface
{
    //数据库实例
    //ISqlSugarClient _db { get; set; }
    //用户仓储
    UserManager _userManager { get; set; }
    //asn仓储
    SqlSugarRepository<WMSASN> _repASN { get; set; }
    //ASNDetail 仓储
    SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

    SqlSugarRepository<WMSASNCountQuantity> _repASNCountQuantity { get; set; }
    SqlSugarRepository<WMSASNCountQuantityDetail> _repASNCountQuantityDetail { get; set; }

    //客户用户关系仓储
    SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
    //产品仓储
    SqlSugarRepository<WMSProduct> _repProduct { get; set; }

    SqlSugarRepository<WMSRFReceiptAcquisition> _repRFReceiptAcquisition { get; set; }


    //仓库用户关系仓储
    SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
    //Task<Response<OrderStatusDto>> AddStrategy(List<long> request);
    Task<Response<List<WMSASNCountQuantityDetailDto>>> ScanAddStrategy(WMSASNCountQuantityDetailDto request);
}
