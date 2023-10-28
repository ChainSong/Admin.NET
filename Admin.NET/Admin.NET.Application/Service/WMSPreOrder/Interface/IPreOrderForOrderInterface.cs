
using Admin.NET.Core.Entity;
using Admin.NET.Core; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;

namespace Admin.NET.Application.Interface
{
    public interface IPreOrderForOrderInterface
    {
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _reppreOrderDetail { get; set; }
        public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        public SqlSugarRepository<WMSOrderDetail> _repOrderDetail { get; set; }
        public SqlSugarRepository<WMSOrder> _repOrder { get; set; }
        Task<Response<List<OrderStatusDto>>> Strategy(List<long> request);
    }
}
