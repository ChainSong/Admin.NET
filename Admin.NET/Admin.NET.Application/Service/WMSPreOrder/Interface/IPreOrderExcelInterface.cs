
using Admin.NET.Core.Entity;
using Admin.NET.Core;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin.NET.Application.Dtos;

namespace Admin.NET.Application.Interface
{
    public interface IPreOrderExcelInterface
    {

        //public SqlSugarRepository<WMSPreOrder> _repOrder { get; set; }
        public SqlSugarRepository<WMSPreOrder> _repPreOrder { get; set; }

        public SqlSugarRepository<WMSPreOrderDetail> _repPreOrderDetail { get; set; }
        //public ISqlSugarClient _db { get; set; }
        public UserManager _userManager { get; set; }


        public SqlSugarRepository<CustomerUserMapping> _repCustomerUser { get; set; }
        public SqlSugarRepository<WarehouseUserMapping> _repWarehouseUser { get; set; }
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        public SqlSugarRepository<TableColumnsDetail> _repTableColumnsDetail { get; set; }

        /// <summary>
        /// 导入(导入的时候需要奖Excel的表头替换成为字段头) 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable, List<OrderStatusDto>> Import  (dynamic request);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Response<DataTable> Export(WMSPreOrderExcelInput request);
    }
}
