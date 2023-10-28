using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.ReceiptCore.Interface
{
   public  interface IReceiptInventoryInterface
    {

        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSInventoryUsable> _repTableInventoryUsable { get; set; }
        public SqlSugarRepository<WMSInventoryUsed> _repTableInventoryUsed { get; set; }

        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }

        public SqlSugarRepository<WMSASN> _repASN { get; set; }

        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }

        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }

        public ISqlSugarClient _db { get; set; }

        Task<Response<List<OrderStatusDto>>> Strategy(List<long> request);
    }
}
