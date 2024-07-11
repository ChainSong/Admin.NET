
using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IASNInterface
    {
        //数据库实例
        //ISqlSugarClient _db { get; set; }
        //用户仓储
        UserManager _userManager { get; set; }
        //asn仓储
        SqlSugarRepository<WMSASN> _repASN { get; set; }
        //ASNDetail 仓储
        SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
         
        //客户用户关系仓储
        SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        //产品仓储
        SqlSugarRepository<WMSProduct> _repProduct { get; set; }

        //仓库用户关系仓储
        SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser{ get; set; } 
        Task<Response<List<OrderStatusDto>>> AddStrategy(List<AddOrUpdateWMSASNInput> request);
        Task<Response<List<OrderStatusDto>>> UpdateStrategy(List<AddOrUpdateWMSASNInput> request);
    }
}
