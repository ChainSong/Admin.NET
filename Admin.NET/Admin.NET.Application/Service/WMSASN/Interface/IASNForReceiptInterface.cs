
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IASNForReceiptInterface
    {

        //数据库实例
        //ISqlSugarClient _db { get; set; }
        //用户仓储
        UserManager _userManager { get; set; }
        //asn仓储
        SqlSugarRepository<WMSASN> _repASN { get; set; }

        //asnDetail仓储
        SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
        //客户用户关系仓储
        SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        //仓库用户关系仓储
        SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        //private readonly SqlSugarRepository<WMSReceipt> _repReceipt;
        //private readonly SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail;

        Task<Response<List<OrderStatusDto>>> Strategy(List<long> request);

    }
}
