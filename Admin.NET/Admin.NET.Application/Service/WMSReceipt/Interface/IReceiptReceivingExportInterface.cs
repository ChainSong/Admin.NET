
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
    public interface IReceiptReceivingExportInterface
    {

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public UserManager _userManager { get; set; }
        public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
        public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }
        public SqlSugarRepository<WMSOrderAllocation> _repOrderAllocation { get; set; }
        
        Response<DataTable> Strategy(List<long> request);
    }
}
