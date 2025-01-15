using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.Interface
{
    public interface IOrderExcelInterface
    {

        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public  UserManager _userManager { get; set; }
        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public  SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }

        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }

        public SqlSugarRepository<WMSInstruction> _repInstruction { get; set; }

        public SqlSugarRepository<WMSPackage> _repPackage { get; set; }
        public SqlSugarRepository<WMSPackageDetail> _repPackageDetail { get; set; }

        public SqlSugarRepository<WMSPickTask> _repPickTask { get; set; }
        public SqlSugarRepository<WMSPickTaskDetail> _repPickTaskDetail { get; set; }

        public SqlSugarRepository<WMSInventoryUsable> _repInventoryUsable { get; set; }
 

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable> Export(WMSOrderExcellInput request);
        Response<DataTable> ExportPackage(List<long> request);
    }
    
}
