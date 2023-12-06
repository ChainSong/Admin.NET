using Admin.NET.Core.Entity;
using Admin.NET.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;

namespace Admin.NET.Application.ReceiptReceivingCore.Interface
{
    public interface IReceiptReceivingReturnInterface
    {


        public SqlSugarRepository<WMSReceipt> _repReceipt { get; set; }
        public SqlSugarRepository<WMSReceiptDetail> _repReceiptDetail { get; set; }

        public UserManager _userManager { get; set; }

        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }

        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSReceiptReceiving> _repReceiptReceiving { get; set; }

        public SqlSugarRepository<WMSLocation> _repLocation { get; set; }

        //public ISqlSugarClient _db { get; set; }
        //Task<Response<List<OrderStatusDto>>> Strategy(List<long> request);
       Task<Response<List<OrderStatusDto>>> Strategy(List<long> request);
    }
}
