using Admin.NET.Application.Dtos;
using Admin.NET.Core;
using Admin.NET.Core.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application.ReceiptReceivingCore.Interface
{
    public interface IMReceiptExcelInterface
    {

        public SqlSugarRepository<MMSReceipt> _repMReceipt { get; set; }
        public SqlSugarRepository<MMSReceiptDetail> _repMReceiptDetail { get; set; }
        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }
        public SqlSugarRepository<MMSSupplier> _repSupplier { get; set; }
        public SqlSugarRepository<SupplierUserMapping> _repSupplierUser { get; set; }
        public SqlSugarRepository<MMSReceiptReceivingDetail> _repMReceiptReceivingDetail { get; set; }
        public SqlSugarRepository<MMSReceiptReceiving> _repMReceiptReceiving { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<MMSInventoryUsable> _repInventoryUsable { get; set; }
        Response<DataTable> ExportReceipt(List<long> request); 
        Response<DataTable> ExportReceiptReceiving(List<long> request);
    }
}
