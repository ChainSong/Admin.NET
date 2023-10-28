
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IAdjustmentExcelInterface
    {

        //数据库实例
        //ISqlSugarClient _db { get; set; }
        //用户仓储
        UserManager _userManager { get; set; }
        ////asn仓储
        //SqlSugarRepository<WMSASN> _repASN { get; set; }
        ////客户用户关系仓储
        //SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        ////仓库用户关系仓储
        //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        //SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        //表字段仓储
        SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        
        Response<DataTable> Strategy(dynamic request);
    }
}
