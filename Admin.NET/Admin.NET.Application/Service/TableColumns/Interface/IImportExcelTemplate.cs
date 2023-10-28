
using Admin.NET.Application.Dtos;
using Admin.NET.Core.Entity;
using Admin.NET.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.NET.Application
{
    public interface IImportExcelTemplate
    {
        public SqlSugarRepository<TableColumns> _repTableColumns { get; set; }
        //public readonly SqlSugarRepository<TableColumnsDetail> _repDetail { get; set; }
        public UserManager _userManager { get; set; }

        Response<DataTable> Strategy(long CustomerId, long TenantId);
    }
}
