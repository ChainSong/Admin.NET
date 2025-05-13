
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;

namespace Admin.NET.Application
{
    public interface IReceiptInventoryInterface
    {
        public SqlSugarRepository<WMSProduct> _repProduct { get; set; }
        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }
        public SqlSugarRepository<WMSASN> _repASN { get; set; }
        //注入ASNDetail仓储
        public SqlSugarRepository<WMSASNDetail> _repASNDetail { get; set; }
        public SqlSugarRepository<WMSRFIDInfo> _repRFIDInfo { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        //Response<DataTable> Strategy(List<long> request);
        public Task<Response<List<OrderStatusDto>>> Strategy(List<WMSReceiptInput> request);
    }
}
